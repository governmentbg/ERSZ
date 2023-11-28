using ERSZ.Core.Contracts;
using ERSZ.Infrastructure.Constants;
using ERSZ.Infrastructure.Contracts;
using ERSZ.Infrastructure.Data.Common;
using ERSZ.Infrastructure.Data.Models.Common;
using ERSZ.Infrastructure.Data.Models.Ekatte;
using ERSZ.Infrastructure.Data.Models.Nomenclatures;
using ERSZ.Infrastructure.Data.Models.Register;
using ERSZ.Infrastructure.Extensions;
using ERSZ.Infrastructure.ViewModels.Common;
using ERSZ.Infrastructure.ViewModels.Register;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.Extensions.Logging;
using NPOI.HSSF.UserModel;
using NPOI.SS.UserModel;
using NPOI.SS.Util;
using NPOI.XSSF.UserModel;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace ERSZ.Core.Services
{
    public class ExcelImportService : BaseService, IExcelImportService
    {
        private readonly IRegisterService registerService;
        private readonly ILogOperationService logOperation;
        private readonly IUserContext userContext;
        private IEnumerable<NomSpeciality> nomSpecialities;
        public ExcelImportService(
            ILogger<ExcelImportService> _logger,
            IRepository _repo,
            IRegisterService _registerService,
            ILogOperationService _logOperation,
            IUserContext _userContext)
        {
            logger = _logger;
            repo = _repo;
            registerService = _registerService;
            logOperation = _logOperation;
            userContext = _userContext;
        }

        public async Task<SaveResultVM> ProcessAsync(string fileName, byte[] fileContent, ViewDataDictionary viewData)
        {
            ExcelSheetData = CreateExcelSheetObjectFromByteArray(fileName, fileContent);
            if (ExcelSheetData == null)
            {
                return new SaveResultVM(false, "Проблем при изчитане на данни от excel.");
            }
            string message = "";
            int jurorsAdded = 0;
            int mandatesAdded = 0;
            try
            {
                nomSpecialities = repo.AllReadonly<NomSpeciality>().ToList();
                var jurors = mapToModel();
                foreach (var juror in jurors)
                {
                    if (string.IsNullOrEmpty(juror.Id))
                    {
                        var saveResult = await registerService.Juror_SaveData(juror);
                        if (saveResult.IsSuccessfull)
                        {
                            juror.MandateModel.JurorId = juror.Id;
                            await registerService.Mandate_SaveData(juror.MandateModel);

                            var addItems = new List<LogOperItemModel>()
                            {
                                new LogOperItemModel()
                                {
                                    Key = "Специалности",
                                    Items = juror.Specialities.Where(x => x.Checked).Select(x=>new LogOperItemModel(){ Value = x.Label}).ToArray()
                                }
                            };
                            var userData = logOperation.GetValuesFromObject(juror, viewData, addItems);
                            logOperation.SaveLogOperation(NomenclatureConstants.AuditOperations.Import, "Juror", "Edit", juror.Id, userContext.LogName, userData);
                            var userDataMandate = logOperation.GetValuesFromObject(juror.MandateModel, viewData);
                            logOperation.SaveLogOperation(NomenclatureConstants.AuditOperations.Import, "Mandate", "Edit", juror.MandateModel.Id.ToString(), userContext.LogName, userDataMandate);

                            jurorsAdded++;
                        }
                    }
                    else
                    {
                        var savedMandateFor = repo.AllReadonly<JurorMandate>()
                                                .Where(x => x.JurorId == juror.Id && x.CourtId == juror.MandateModel.CourtId)
                                                .Where(x => x.DateStart >= juror.MandateModel.DateStart.AddDays(-10)
                                                && x.DateStart <= juror.MandateModel.DateStart.AddDays(10))
                                                .FirstOrDefault();
                        if (savedMandateFor == null)
                        {
                            var mSave = await registerService.Mandate_SaveData(juror.MandateModel);
                            if (mSave.IsSuccessfull)
                            {
                                mandatesAdded++;
                            }
                        }
                    }
                }
                message = $"Изчетени {jurors.Count} бр. заседатели. Нови заседатели: {jurorsAdded} бр.; Добавени мандати: {mandatesAdded} бр.";
            }
            catch (Exception ex)
            {
                return new SaveResultVM(false, ex.Message);
            }
            return new SaveResultVM(true, message);
        }

        private void validateRequiredText(string value, int row, int colIndex)
        {
            if (string.IsNullOrEmpty(value))
            {
                throw new Exception($"Поле {GetCellDataFromISheet_AsString(0, colIndex)} е задължително, ред {row}");
            }
        }

        private bool validatePattern(string value, string pattern)
        {
            if (string.IsNullOrEmpty(value))
            {
                return true;
            }
            try
            {
                var rule = new Regex(pattern);
                return rule.IsMatch(value);
            }
            catch (ArgumentException)
            {
                return false;
            }
        }

        private List<JurorVM> mapToModel()
        {
            var result = new List<JurorVM>();

            int maxRow = getMaxRow();
            for (int row = 1; row <= maxRow; row++)
            {
                var juror = new JurorVM()
                {
                    MandateModel = new MandateVM()
                };


                int colIndex = 0;
                juror.Uic = GetCellDataFromISheet_AsString(row, colIndex);

                validateRequiredText(juror.Uic, row, colIndex);

                if (!juror.Uic.IsEGN())
                {
                    throw new Exception($"Невалидна стойност в поле {GetCellDataFromISheet_AsString(0, colIndex)}, ред {row}");
                }
                colIndex++;
                juror.FirstName = GetCellDataFromISheet_AsString(row, colIndex);
                validateRequiredText(juror.FirstName, row, colIndex);

                colIndex++;
                juror.MiddleName = GetCellDataFromISheet_AsString(row, colIndex);
                validateRequiredText(juror.MiddleName, row, colIndex);

                colIndex++;
                juror.FamilyName = GetCellDataFromISheet_AsString(row, colIndex);
                validateRequiredText(juror.FamilyName, row, colIndex);

                colIndex++;
                string adrEkatte = GetCellDataFromISheet_AsString(row, colIndex);
                validateRequiredText(adrEkatte, row, colIndex);
                var adrCityCode = repo.AllReadonly<EkEkatte>()
                                    .Where(e => e.Ekatte == adrEkatte)
                                    .Select(x => x.Ekatte)
                                    .FirstOrDefault();
                if (!string.IsNullOrEmpty(adrCityCode))
                {
                    juror.CityCode = adrCityCode;
                }
                else
                {
                    throw new Exception($"Невалидна стойност в поле {GetCellDataFromISheet_AsString(0, colIndex)}, ред {row}");
                }

                colIndex++;
                juror.AddressText = GetCellDataFromISheet_AsString(row, colIndex);
                validateRequiredText(juror.AddressText, row, colIndex);

                colIndex++;
                juror.Phone = GetCellDataFromISheet_AsString(row, colIndex);
                if (!validatePattern(juror.Phone, NomenclatureConstants.PhoneRegexPattern))
                {
                    throw new Exception($"Невалидна стойност в поле {GetCellDataFromISheet_AsString(0, colIndex)}, ред {row}");
                }


                colIndex++;
                juror.EMail = GetCellDataFromISheet_AsString(row, colIndex);
                if (!validatePattern(juror.EMail, NomenclatureConstants.EmailRegexPattern))
                {
                    throw new Exception($"Невалидна стойност в поле {GetCellDataFromISheet_AsString(0, colIndex)}, ред {row}");
                }

                colIndex++;
                juror.EducationId = GetIdByCode<NomEducation>(GetCellDataFromISheet_AsString(row, colIndex));
                if (juror.EducationId <= 0)
                {
                    throw new Exception($"Невалидна стойност в поле {GetCellDataFromISheet_AsString(0, colIndex)}, ред {row}");
                }

                colIndex++;
                var educationRang = GetCellDataFromISheet_AsString(row, colIndex);
                if (!string.IsNullOrEmpty(educationRang))
                {
                    juror.EducationRangId = GetIdByCode<NomEducationRang>(educationRang);
                    if (juror.EducationRangId <= 0)
                    {
                        throw new Exception($"Невалидна стойност {GetCellDataFromISheet_AsString(row, colIndex)} в поле {GetCellDataFromISheet_AsString(0, colIndex)}, ред {row}");
                    }
                }

                colIndex++;
                var specList = new List<CheckListVM>();
                string specialities = GetCellDataFromISheet_AsString(row, colIndex);
                char[] separators = { ',', '.', ';' };
                foreach (var spec in specialities.Split(separators, StringSplitOptions.RemoveEmptyEntries))
                {
                    var specId = GetIdByCode<NomSpeciality>(spec);
                    if (specId <= 0)
                    {
                        throw new Exception($"Невалидна стойност {spec} в поле {GetCellDataFromISheet_AsString(0, colIndex)}, ред {row}");
                    }
                    specList.Add(new CheckListVM()
                    {
                        Value = specId.ToString(),
                        Label = nomSpecialities.Where(x => x.Id == specId).Select(x => x.Label).FirstOrDefault(),
                        Checked = true
                    });
                }
                juror.Specialities = specList;

                colIndex++;
                var txtValue = GetCellDataFromISheet_AsString(row, colIndex);
                //  Данни за мандат
                validateRequiredText(txtValue, row, colIndex);
                juror.MandateModel.RegisterCourtId = GetIdByCode<CommonCourt>(txtValue);
                if (juror.MandateModel.RegisterCourtId <= 0)
                {
                    throw new Exception($"Невалидна стойност {txtValue} в поле {GetCellDataFromISheet_AsString(0, colIndex)}, ред {row}");
                }

                colIndex++;
                txtValue = GetCellDataFromISheet_AsString(row, colIndex);
                validateRequiredText(txtValue, row, colIndex);
                juror.MandateModel.MunicipalityId = repo.AllReadonly<EkMunincipality>().Where(x => x.Ekatte == txtValue).Select(x => x.MunicipalityId).FirstOrDefault();
                if (juror.MandateModel.MunicipalityId <= 0)
                {
                    throw new Exception($"Невалидна стойност {txtValue} в поле {GetCellDataFromISheet_AsString(0, colIndex)}, ред {row}");
                }


                juror.Id = repo.AllReadonly<Juror>().Where(x => x.Uic == juror.Uic).Select(x => x.Id).FirstOrDefault();


                colIndex++;
                txtValue = GetCellDataFromISheet_AsString(row, colIndex);
                validateRequiredText(txtValue, row, colIndex);
                //  Данни за мандат
                juror.MandateModel.JurorId = juror.Id;
                juror.MandateModel.MandateTypeId = JurorConstants.Mandate.MandateType;
                juror.MandateModel.CourtId = GetIdByCode<CommonCourt>(txtValue);
                if (juror.MandateModel.CourtId <= 0)
                {
                    throw new Exception($"Невалидна стойност {txtValue} в поле {GetCellDataFromISheet_AsString(0, colIndex)}, ред {row}");
                }

                //DateStart
                colIndex++;
                var dt = GetCellDataFromISheet_AsDateTime(row, colIndex);
                if (dt != null)
                {
                    juror.MandateModel.DateStart = dt.Value;
                    juror.DateStart = juror.MandateModel.DateStart;
                }
                else
                {
                    throw new Exception($"Невалидна стойност {GetCellDataFromISheet_AsString(row, colIndex)} в поле {GetCellDataFromISheet_AsString(row, colIndex)}, ред {row}");
                }

                //DateEnd
                colIndex++;
                dt = GetCellDataFromISheet_AsDateTime(row, colIndex);
                if (dt != null)
                {
                    juror.MandateModel.DateEnd = dt.Value;
                }

                result.Add(juror);
            }

            return result;
        }



        private int GetIdByCode<T>(string code) where T : BaseCommonNomenclature
        {
            return repo.AllReadonly<T>().Where(x => x.Code == code).Select(x => x.Id).FirstOrDefault();
        }

        #region Base Excel methods
        ISheet ExcelSheetData;

        int safeIntFromCell(int row, int col)
        {
            var textValue = GetCellDataFromISheet_AsString(row, col);
            if (!string.IsNullOrEmpty(textValue))
            {
                return int.Parse(textValue);
            }
            return 0;
        }


        int getMaxRow()
        {
            int pos = 1;
            while (true)
            {
                if (string.IsNullOrEmpty(GetCellDataFromISheet_AsString(pos, 0)))
                {
                    return pos - 1;
                }
                pos++;
            }
        }


        ISheet CreateExcelSheetObjectFromByteArray(string fileName, byte[] fileContent)
        {
            ISheet sheet;
            using (var stream = new MemoryStream(fileContent))
            {
                try
                {
                    if (Path.GetExtension(fileName) == ".xls")
                    {
                        HSSFWorkbook hssfwb = new HSSFWorkbook(stream); //This will read the Excel 97-2000 formats  
                        sheet = hssfwb.GetSheetAt(0);
                    }
                    else
                    {
                        XSSFWorkbook hssfwb = new XSSFWorkbook(stream); //This will read 2007 Excel format  
                        sheet = hssfwb.GetSheetAt(0);
                    }
                }
                catch (Exception ex)
                {
                    this.logger.LogError(ex, $"Invalid file {fileName}");
                    return null;
                }
            }

            return sheet;
        }

        /// <summary>
        /// Хвърля ProcessException!!!
        /// </summary>
        /// <param name="Row"></param>
        /// <param name="Column"></param>
        /// <param name="IsHaveMergedCell"></param>
        /// <returns></returns>
        protected string GetCellDataFromISheet_AsString(int Row, int Column, bool IsHaveMergedCell = false)
        {
            var value = GetCellDataFromISheet(Row, Column, IsHaveMergedCell);
            if (value != null)
            {
                return value.ToString();
            }
            else
            {
                return string.Empty;
            }

            //return GetCellDataFromISheet(Row, Column, IsHaveMergedCell).ToString();
        }
        protected DateTime? GetCellDataFromISheet_AsDateTime(int Row, int Column, bool IsHaveMergedCell = false)
        {
            var value = GetCellDataFromISheet(Row, Column, IsHaveMergedCell);
            if (value != null)
            {
                return Convert.ToDateTime(value);
            }
            else
            {
                return (DateTime?)null;
            }

            //return GetCellDataFromISheet(Row, Column, IsHaveMergedCell).ToString();
        }

        /// <summary>
        /// Хвърля ProcessException!!!
        /// </summary>
        /// <param name="Row"></param>
        /// <param name="Column"></param>
        /// <param name="IsHaveMergedCell"></param>
        /// <returns></returns>
        object GetCellDataFromISheet(int Row, int Column, bool IsHaveMergedCell = false)
        {
            object result = null;

            try
            {
                // Изчитане на данни от клетка ПРИ наличие на Merge-нати клетки
                if (IsHaveMergedCell)
                {
                    IRow row = this.ExcelSheetData.GetRow(Row);
                    ICell cellBeforeTestMerged = row.GetCell(Column);
                    ICell cell = GetFirstCellInMergedRegionContainingCell(cellBeforeTestMerged);
                    result = GetCellValue(cell ?? cellBeforeTestMerged);
                }
                else // Изчитане на данни от клетка БЕЗ наличие на Merge-нати клетки
                {
                    IRow row = this.ExcelSheetData.GetRow(Row);
                    if (row == null)
                    {
                        return null;
                    }
                    ICell cell = row.GetCell(Column);
                    if (cell == null)
                    {
                        return null;
                    }
                    result = GetCellValue(cell);
                }
            }
            catch (Exception ex)
            {
                this.logger.LogError(ex, @"GetCellDataFromISheet");
                throw new Exception($"Invalid value at row {Row}, col {Column}");
            }

            return result;
        }

        private ICell GetFirstCellInMergedRegionContainingCell(ICell cell)
        {
            if (cell != null && cell.IsMergedCell)
            {
                ISheet sheet = cell.Sheet;
                for (int i = 0; i < sheet.NumMergedRegions; i++)
                {
                    CellRangeAddress region = sheet.GetMergedRegion(i);

                    if ((region.FirstRow <= cell.RowIndex && region.LastRow >= cell.RowIndex)
                     && (region.FirstColumn <= cell.ColumnIndex && region.LastColumn >= cell.ColumnIndex))
                    {
                        IRow row = sheet.GetRow(region.FirstRow);
                        ICell firstCell = row?.GetCell(region.FirstColumn);
                        return firstCell;
                    }
                }
                return null;
            }
            return cell;
        }


        Object GetCellValue(ICell cell)
        {
            Object result = null;
            CellType _CellType;

            if (cell == null) return result;

            if (cell.CellType == CellType.Formula)
                _CellType = cell.CachedFormulaResultType;
            else
                _CellType = cell.CellType;

            switch (_CellType)
            {
                case CellType.Blank:
                    result = cell.StringCellValue;
                    result = null;
                    break;

                case CellType.Boolean:
                    result = cell.BooleanCellValue;
                    break;

                case CellType.Error:
                    //result = cell.ErrorCellValue;
                    result = "ERROR";
                    break;

                case CellType.Numeric:
                    if (HSSFDateUtil.IsCellDateFormatted(cell))
                        result = cell.DateCellValue;
                    else
                        result = cell.NumericCellValue;
                    break;

                case CellType.String:
                    result = cell.StringCellValue;
                    break;

                case CellType.Unknown:
                    result = "UNKNOWN";
                    break;

            }
            return result;
            //return result.ToString();
        }

        #endregion
    }
}
