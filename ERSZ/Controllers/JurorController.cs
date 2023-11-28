using DataTables.AspNet.Core;
using ERSZ.Core.Constants;
using ERSZ.Core.Contracts;
using ERSZ.Core.Extensions;
using ERSZ.Extensions;
using ERSZ.Infrastructure.Constants;
using ERSZ.Infrastructure.Contracts;
using ERSZ.Infrastructure.Data.Models.Common;
using ERSZ.Infrastructure.Data.Models.Nomenclatures;
using ERSZ.Infrastructure.Extensions;
using ERSZ.Infrastructure.ViewModels.Register;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace ERSZ.Controllers
{
    public class JurorController : BaseController
    {
        private readonly IRegisterService registerService;
        private readonly INomenclatureService nomService;
        private readonly IEkDitrictService ekDitrictService;
        private readonly IExcelImportService importService;

        public JurorController(IRegisterService _registerService, INomenclatureService _nomService,
            IEkDitrictService _ekDitrictService,
            IExcelImportService _importService)
        {
            registerService = _registerService;
            nomService = _nomService;
            ViewBag.MenuItemValue = "Juror";
            ekDitrictService = _ekDitrictService;
            importService = _importService;
        }

        #region Juror
        [AllowAnonymous]

        public IActionResult Index(int courtTypeId = 0, int courtId = 0, bool isEdit = false, string search = null)
        {
            var model = new FilterJurorVM()
            {
                FullName = search,
                CourtId = courtId,
                CourtTypeId = courtTypeId,
                IsEdit = isEdit,
                ActiveOnly = ((courtTypeId > 0 || courtId > 0 || !string.IsNullOrEmpty(search)) ? 1 : 0)
            };

            SetViewBagIndex();
            return View(model);
        }

        private void SetViewBagIndex()
        {
            ViewBag.CourtTypeId_ddl = registerService.GetCourtTypeDropDownList(false, true);
            ViewBag.DistrictId_ddl = ekDitrictService.GetDropDownList(false, true);
            var activeOnlyList = new List<SelectListItem>() {
                new SelectListItem("Всички","0"),
                new SelectListItem("Само с активни мандати","1"),
            };
            ViewBag.ActiveOnly_ddl = activeOnlyList;
        }

        [HttpGet]
        [AllowAnonymous]
        public IActionResult GetDDL_Court(int CourtTypeId)
        {
            Expression<Func<CommonCourt, bool>> courtIdSearch = x => true;
            if (CourtTypeId > 0)
                courtIdSearch = x => x.CourtTypeId == CourtTypeId;
            else
                courtIdSearch = x => CourtConstants.CourtType.CourtInSelected.Contains(x.CourtTypeId);

            var model = nomService.GetDropDownListExpr(courtIdSearch, false, true);
            return Json(model);
        }

        [HttpPost]
        [AllowAnonymous]
        public IActionResult Juror_LoadData(IDataTablesRequest request, FilterJurorVM filter)
        {
            var data = registerService.Juror_Select(filter);
            return request.GetResponse(data);
        }
        [AllowAnonymous]
        public IActionResult JurorTimeLine(string id)
        {
            var model = registerService.GetJurorTimeLineVM(id);
            return View(nameof(JurorTimeLine), model);
        }

        public IActionResult Add()
        {
            var model = registerService.InitJurorModel();

            SetViewBag_Juror();
            return View(nameof(Edit), model);
        }

        public IActionResult Edit(string id)
        {
            var model = registerService.GetJurorById(id);
            if (model == null)
            {
                return View("Error");
            }

            Audit_Operation = NomenclatureConstants.AuditOperations.View;
            Audit_Object = $"Заседател: {model.FullName}";
            SetViewBag_Juror();

            return View(nameof(Edit), model);
        }

        private string IsValid(JurorVM model)
        {
            if (!model.Uic.IsEGN())
            {
                return "Невалидно ЕГН";
            }

            if (model.DateStart > (model.DateEnd ?? DateTime.Now.AddYears(10)))
                return "Дата от е по-голяма от дата до";

            if (string.IsNullOrEmpty(model.CityCode))
            {
                return "Изберете населено място";
            }
            else
            {
                model.CityId = nomService.GetEkatteIdByCode(model.CityCode);
                if (model.CityId == 0)
                {
                    return "Невалидно населено място";
                }
            }

            if (model.DateEnd != null)
            {
                if (registerService.IsExistActiveMandatesAfterDate(model.DateEnd ?? DateTime.Now, model.Id))
                    return "Има активни мандати след дата до на заседателя";
            }

            return string.Empty;
        }

        [HttpPost]
        [DisableRequestSizeLimit]
        public async Task<IActionResult> Edit(ICollection<IFormFile> files, JurorVM model)
        {
            SetViewBag_Juror(model);

            string _isvalid = IsValid(model);
            if (_isvalid != string.Empty)
            {
                SetErrorMessage(_isvalid);
                return View(nameof(Edit), model);
            }

            if (files != null && files.Count() > 0)
            {
                var file = files.First();
                using (var memory = new MemoryStream())
                {
                    file.CopyTo(memory);
                    model.Content = memory.ToArray();
                    model.ContentType = file.ContentType;
                    model.FileName = Path.GetFileName(file.FileName);
                }
            }

            if (ModelState.IsValid)
            {
                var oldId = model.Id;
                var result = await registerService.Juror_SaveData(model);

                if (result.IsSuccessfull)
                {
                    var addLogItems = new List<LogOperItemModel>()
                    {
                        new LogOperItemModel()
                        {
                            Key = "Специалности",
                            Items = model.Specialities.Where(x => x.Checked).Select(x=>new LogOperItemModel(){ Value = x.Label}).ToArray()
                        }
                    };

                    var cityCodeLabel = nomService.GetEkatteById(model.CityCode);
                    ViewBag.CityCode_ddl = new List<SelectListItem>() { new SelectListItem($"{cityCodeLabel.Label}, {cityCodeLabel.Category}", model.CityCode) };
                    if (string.IsNullOrEmpty(oldId))
                    {
                        Audit_Operation = NomenclatureConstants.AuditOperations.Add;
                        Audit_Object = $"Заседател: {model.FullName}";
                        SaveLogOperation(NomenclatureConstants.AuditOperations.Add, model, model.Id, addLogItems);
                    }
                    else
                    {
                        Audit_Operation = NomenclatureConstants.AuditOperations.Edit;
                        Audit_Object = $"Заседател: {model.FullName}";
                        SaveLogOperation(NomenclatureConstants.AuditOperations.Edit, model, model.Id, addLogItems);
                    }

                    SetSuccessMessage(MessageConstant.Values.SaveOK);
                    return RedirectToAction(nameof(Edit), new { id = model.Id });
                }

                SetErrorMessage(result.ErrorMessage);
                return View(nameof(Edit), model);
            }

            SetErrorMessage(MessageConstant.Values.SaveFailed);
            return View(nameof(Edit), model);
        }

        public async Task<IActionResult> DeletePhoto(string Id)
        {

            var result = await registerService.DeletePhoto(Id);

            if (result.IsSuccessfull)
            {
                SetSuccessMessage(MessageConstant.Values.DeletePhotoOk);
            }

            return RedirectToAction(nameof(Edit), new { id = Id });
        }

        [AllowAnonymous]
        public IActionResult GetJurorTimeLineYearMandates(int id)
        {
            var model = registerService.GetDataJurorTimeLineVMByMandateOrJuror(string.Empty, id);
            return PartialView("_JurorTimeLineYearMandate", model);
        }

        [AllowAnonymous]
        public IActionResult GetJurorTimeLineYearMandatesSessionDataPartial(int CaseId, int Year, int MandateId)
        {
            var model = new SessionFilterVM()
            {
                CaseId = CaseId,
                Year = Year,
                MandateId = MandateId
            };

            return PartialView("_SessionTimeLine", model);
        }

        [AllowAnonymous]
        public JsonResult GetJurorTimeLineYearMandatesSessionData(int CaseId, int Year, int MandateId)
        {
            var model = registerService.GetSessionTimeLine(CaseId, Year, MandateId);
            return Json(model);
        }

        #endregion

        #region ViewBags

        [HttpPost]
        public IActionResult Juror_LoadMandates(IDataTablesRequest request, string jurorId)
        {
            var data = registerService.Mandate_Select(jurorId);
            return request.GetResponse(data);
        }
        void SetViewBag_Juror(JurorVM model = null)
        {
            //ViewBag.CityId_ddl = ekDitrictService.GetDropDownList();
            ViewBag.EducationId_ddl = nomService.GetDropDownList<NomEducation>();
            //
            ViewBag.Specialities_ddl = nomService.GetDropDownList<NomSpeciality>();
            if (model != null)
            {
                ViewBag.EducationRangId_ddl = nomService.GetDropDownListExpr<NomEducationRang>(x => x.Id == model.EducationRangId, false);
            }
        }

        [HttpGet]
        public IActionResult GetDDL_EducationRang(int EducationId)
        {
            Expression<Func<NomEducationRang, bool>> educationIdSearch = x => true;
            educationIdSearch = x => x.EducationId == EducationId;

            var model = nomService.GetDropDownListExpr(educationIdSearch, false, true);
            return Json(model);
        }

        #endregion

        public PartialViewResult ImportFromExcel()
        {
            return PartialView("_ImportFromExcel");
        }

        [HttpPost]
        public async Task<IActionResult> ImportFromExcel(ICollection<IFormFile> files)
        {
            if (files == null || files.Count == 0)
            {
                return null;
            }
            var file = files.FirstOrDefault();
            byte[] fileBytes;
            using (var ms = new MemoryStream())
            {
                file.CopyTo(ms);
                fileBytes = ms.ToArray();
            }
            var courts = nomService.GetDropDownList<CommonCourt>();
            ViewBag.CourtId_ddl = courts;
            ViewBag.RegisterCourtId_ddl = courts;
            ViewBag.EducationId_ddl = nomService.GetDropDownList<NomEducation>();
            ViewBag.EducationRangId_ddl = nomService.GetDropDownList<NomEducationRang>();
            ViewBag.CityId_ddl = ekDitrictService.GetDropDownList();
            ViewBag.MunicipalityId_ddl = ekDitrictService.GetMunicipalityDropDownList(false, true);
            var result = await importService.ProcessAsync(file.FileName, fileBytes, ViewData);
            return Json(result);
        }

    }
}
