using ERSZ.Core.Contracts;
using ERSZ.Infrastructure.Contracts.Data;
using ERSZ.Infrastructure.Data.Common;
using ERSZ.Infrastructure.Data.Models.Base;
using ERSZ.Infrastructure.Data.Models.Case;
using ERSZ.Infrastructure.Data.Models.Common;
using ERSZ.Infrastructure.Data.Models.Nomenclatures;
using ERSZ.Infrastructure.Data.Models.Register;
using ERSZ.Infrastructure.ViewModels.Common;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace ERSZ.Core.Services
{
    public class DataService : BaseService, IDataService
    {
        private readonly IRegisterService registerService;
        private readonly INomenclatureService nomenclatureService;

        public DataService(IRepository _repository, ILogger<BaseService> _logger,
            IRegisterService _registerService, INomenclatureService _nomenclatureService)
        {
            nomenclatureService = _nomenclatureService;
            repo = _repository;
            logger = _logger;
            registerService = _registerService;
        }
        public async Task<SaveResultVM> InsertCaseData(ErszCaseModel model)
        {

            var result = InitSaveResultCase();

            if (model == null)
            {
                result.IsSuccessfull = false;
                result.ErrorMessage = ErszConstants.Messages.DataMapFailed;
                result.ErrorCode = ErszConstants.ResultCodes.NullValue;
                return result;
            }

            var validationContext = await isContextValid<ErszCaseModel, CaseData>(model);

            if (!validationContext.IsSuccessfull)
            {
                return validationContext;
            }

            int caseTypeId = await nomenclatureService.GetNomIdByCode<NomCaseType>(model.CaseTypeCode ?? "");

            if (caseTypeId <= 0)
            {
                result.IsSuccessfull = false;
                result.ErrorMessage = ErszConstants.Messages.CaseTypeNotFound;
                result.ErrorCode = ErszConstants.ResultCodes.InvalidCode;
                return result;
            }

            if (string.IsNullOrEmpty(model.RegNumber))
            {
                result.IsSuccessfull = false;
                result.ErrorMessage = ErszConstants.Messages.DataMapFailed;
                result.ErrorCode = ErszConstants.ResultCodes.NullValue;
                return result;
            }

            if (model.RegNumber.Length != ErszConstants.RegNumberLength)
            {
                result.IsSuccessfull = false;
                result.ErrorMessage = ErszConstants.Messages.RegNumberLength;
                result.ErrorCode = ErszConstants.ResultCodes.InvalidValue;
                return result;
            }

            var regNumberCheckResult = validateCaseRegNumber(model.RegNumber, model.RegYear);
            if (!regNumberCheckResult.IsSuccessfull)
            {
                result.IsSuccessfull = false;
                result.ErrorMessage = ErszConstants.Messages.RegNumberInvalid;
                result.ErrorCode = ErszConstants.ResultCodes.InvalidValue;
                return result;
            }

            var shortNumber = int.Parse(model.RegNumber.Substring(model.RegNumber.Length - 5, 5));

            try
            {
                await repo.AddAsync(new CaseData()
                {
                    CaseTypeId = caseTypeId,
                    CourtId = validationContext.CourtId,
                    DateCreated = DateTime.Now,
                    RegNumber = model.RegNumber,
                    Gid = model.Context.Gid,
                    RegYear = model.RegYear,
                    ShortNumber = shortNumber.ToString(),
                    IsFinished = model.IsFinished,
                    DateModified = DateTime.Now
                });

                await repo.SaveChangesAsync();
                return result;
            }
            catch (Exception ex)
            {
                logger.LogError(ex.Message, ex);
                result.IsSuccessfull = false;
                result.ErrorMessage = ErszConstants.Messages.GeneralException;
                result.ErrorCode = ErszConstants.ResultCodes.GeneralException;
                return result;
            }
        }

        public async Task<SaveResultVM> UpdateCaseData(ErszCaseModel model)
        {

            var result = InitSaveResultCase();

            if (model == null)
            {
                result.IsSuccessfull = false;
                result.ErrorMessage = ErszConstants.Messages.DataMapFailed;
                result.ErrorCode = ErszConstants.ResultCodes.NullValue;
                return result;
            }

            var validationContext = await isContextValid<ErszCaseModel, CaseData>(model);

            if (!validationContext.IsSuccessfull)
            {
                return validationContext;
            }

            var erszBaseActivitySaveResultWrapper = ValidateBaseActivityByGid<CaseData>(model.Context.Gid);

            if (erszBaseActivitySaveResultWrapper.BaseActivity == null)
            {
                return erszBaseActivitySaveResultWrapper.SaveResultCaseVM;
            }

            int caseTypeId = await nomenclatureService.GetNomIdByCode<NomCaseType>(model.CaseTypeCode ?? "");

            if (caseTypeId <= 0)
            {
                result.IsSuccessfull = false;
                result.ErrorMessage = ErszConstants.Messages.CaseTypeNotFound;
                result.ErrorCode = ErszConstants.ResultCodes.InvalidCode;
                return result;
            }

            if (string.IsNullOrEmpty(model.RegNumber))
            {
                result.IsSuccessfull = false;
                result.ErrorMessage = ErszConstants.Messages.DataMapFailed;
                result.ErrorCode = ErszConstants.ResultCodes.NullValue;
                return result;
            }

            if (model.RegNumber.Length != ErszConstants.RegNumberLength)
            {
                result.IsSuccessfull = false;
                result.ErrorMessage = ErszConstants.Messages.RegNumberLength;
                result.ErrorCode = ErszConstants.ResultCodes.InvalidValue;
                return result;
            }

            var regNumberCheckResult = validateCaseRegNumber(model.RegNumber, model.RegYear);
            if (!regNumberCheckResult.IsSuccessfull)
            {
                result.IsSuccessfull = false;
                result.ErrorMessage = ErszConstants.Messages.RegNumberInvalid;
                result.ErrorCode = ErszConstants.ResultCodes.InvalidValue;
                return result;
            }

            var shortNumber = int.Parse(model.RegNumber.Substring(model.RegNumber.Length - 5, 5));

            try
            {
                var caseModel = repo.All<CaseData>()
                                        .Where(c => c.Gid == model.Context.Gid)
                                        .FirstOrDefault();

                caseModel.IsFinished = model.IsFinished;

                await repo.SaveChangesAsync();
                return result;
            }
            catch (Exception ex)
            {
                logger.LogError(ex.Message, ex);
                result.IsSuccessfull = false;
                result.ErrorMessage = ErszConstants.Messages.GeneralException;
                result.ErrorCode = ErszConstants.ResultCodes.GeneralException;
                return result;
            }
        }

        public async Task<SaveResultVM> InsertCaseDismissal(ErszCaseDismissalModel model)
        {
            var result = InitSaveResultCase();

            if (model == null)
            {
                result.IsSuccessfull = false;
                result.ErrorMessage = ErszConstants.Messages.DataMapFailed;
                result.ErrorCode = ErszConstants.ResultCodes.NullValue;
                return result;
            }

            var validationContext = await isContextValid<ErszCaseDismissalModel, CaseDismissal>(model);

            if (!validationContext.IsSuccessfull)
            {
                return validationContext;
            }

            ValidateString(model.DismissalKind, result);

            ValidateString(model.Reason, result);

            if (!result.IsSuccessfull)
            {
                return result;
            }

            var erszBaseActivitySaveResultWrapper = ValidateBaseActivityByGid<CaseData>(model.CaseGid);

            if (erszBaseActivitySaveResultWrapper.BaseActivity == null)
            {
                return erszBaseActivitySaveResultWrapper.SaveResultCaseVM;
            }

            try
            {
                await repo.AddAsync(new CaseDismissal()
                {
                    CaseId = erszBaseActivitySaveResultWrapper.BaseActivity.Id,
                    CourtId = validationContext.CourtId,
                    Gid = model.Context.Gid,
                    JurorId = validationContext.JurorId,
                    DismissalDate = model.DismissalDate,
                    Reason = model.Reason,
                    DateCreated = DateTime.Now,
                    DateModified = DateTime.Now,
                    DismissalKind = model.DismissalKind,
                });

                await repo.SaveChangesAsync();
                return result;
            }
            catch (Exception ex)
            {
                logger.LogError(ex.Message, ex);
                result.IsSuccessfull = false;
                result.ErrorMessage = ErszConstants.Messages.GeneralException;
                result.ErrorCode = ErszConstants.ResultCodes.GeneralException;
                return result;
            }
        }

        //Check FileContentType and FileContent
        public async Task<SaveResultVM> InsertCaseSelectionProtokol(ErszCaseSelectionProtokolModel model)
        {
            var result = InitSaveResultCase();

            if (model == null)
            {
                result.IsSuccessfull = false;
                result.ErrorMessage = ErszConstants.Messages.DataMapFailed;
                result.ErrorCode = ErszConstants.ResultCodes.NullValue;
                return result;
            }

            var validationContext = await isContextValid<ErszCaseSelectionProtokolModel, CaseSelectionProtokol>(model);

            if (!validationContext.IsSuccessfull)
            {
                return validationContext;
            }

            ValidateString(model.FileContentType, result);

            ValidateString(model.FileContent, result);

            if (!result.IsSuccessfull)
            {
                return result;
            }

            var erszBaseActivitySaveResultWrapper = ValidateBaseActivityByGid<CaseData>(model.CaseGid);

            if (erszBaseActivitySaveResultWrapper.BaseActivity == null)
            {
                return erszBaseActivitySaveResultWrapper.SaveResultCaseVM;
            }

            try
            {
                await repo.AddAsync(new CaseSelectionProtokol()
                {
                    CaseId = erszBaseActivitySaveResultWrapper.BaseActivity.Id,
                    CourtId = validationContext.CourtId,
                    Gid = model.Context.Gid,
                    JurorMandateId = validationContext.JurorMandateId,
                    SelectionDate = model.SelectionDate,
                    DateCreated = DateTime.Now,
                    DateModified = DateTime.Now,
                });

                await repo.SaveChangesAsync();
                return result;
            }
            catch (Exception ex)
            {
                logger.LogError(ex.Message, ex);
                result.IsSuccessfull = false;
                result.ErrorMessage = ErszConstants.Messages.GeneralException;
                result.ErrorCode = ErszConstants.ResultCodes.GeneralException;
                return result;
            }
        }

        public async Task<SaveResultVM> InsertCaseSession(ErszCaseSessionModel model)
        {
            var result = InitSaveResultCase();

            if (model == null)
            {
                result.IsSuccessfull = false;
                result.ErrorMessage = ErszConstants.Messages.DataMapFailed;
                result.ErrorCode = ErszConstants.ResultCodes.NullValue;
                return result;
            }

            var validationContext = await isContextValid<ErszCaseSessionModel, CaseSession>(model);

            if (!validationContext.IsSuccessfull)
            {
                return validationContext;
            }

            ValidateString(model.SessionKind, result);

            ValidateString(model.State, result);

            //ValidateString(model.Result, result);

            //ValidateString(model.ResultBase, result);

            if (!result.IsSuccessfull)
            {
                return result;
            }

            var erszBaseActivitySaveResultWrapper = ValidateBaseActivityByGid<CaseData>(model.CaseGid);

            if (erszBaseActivitySaveResultWrapper.BaseActivity == null)
            {
                return erszBaseActivitySaveResultWrapper.SaveResultCaseVM;
            }

            try
            {
                await repo.AddAsync(new CaseSession()
                {
                    CaseId = erszBaseActivitySaveResultWrapper.BaseActivity.Id,
                    CourtId = validationContext.CourtId,
                    Gid = model.Context.Gid,
                    State = model.State,
                    ResultBase = model.ResultBase,
                    Result = model.Result,
                    SessionKind = model.SessionKind,
                    DateCreated = DateTime.Now,
                    DateModified = DateTime.Now,
                    DateStart = model.DateStart,
                    DateEnd = model.DateEnd,
                });

                await repo.SaveChangesAsync();
                return result;
            }
            catch (Exception ex)
            {
                logger.LogError(ex.Message, ex);
                result.IsSuccessfull = false;
                result.ErrorMessage = ErszConstants.Messages.GeneralException;
                result.ErrorCode = ErszConstants.ResultCodes.GeneralException;
                return result;
            }
        }

        public async Task<SaveResultVM> InsertCaseSessionAct(ErszCaseSessionActModel model)
        {
            var result = InitSaveResultCase();

            if (model == null)
            {
                result.IsSuccessfull = false;
                result.ErrorMessage = ErszConstants.Messages.DataMapFailed;
                result.ErrorCode = ErszConstants.ResultCodes.NullValue;
                return result;
            }

            var validationContext = await isContextValid<ErszCaseSessionActModel, CaseSessionAct>(model);

            if (!validationContext.IsSuccessfull)
            {
                return validationContext;
            }

            ValidateString(model.ActKind, result);

            ValidateString(model.RegNumber, result);

            if (!result.IsSuccessfull)
            {
                return result;
            }

            var erszBaseActivitySaveResultWrapper = ValidateBaseActivityByGid<CaseSession>(model.CaseSessionGid);

            if (erszBaseActivitySaveResultWrapper.BaseActivity == null)
            {
                return erszBaseActivitySaveResultWrapper.SaveResultCaseVM;
            }

            try
            {
                await repo.AddAsync(new CaseSessionAct()
                {
                    CaseSesionId = erszBaseActivitySaveResultWrapper.BaseActivity.Id,
                    CourtId = validationContext.CourtId,
                    ActKind = model.ActKind,
                    Gid = model.Context.Gid,
                    RegNumber = model.RegNumber,
                    RegDate = model.RegDate,
                    DateCreated = DateTime.Now,
                    DateModified = DateTime.Now,
                });

                await repo.SaveChangesAsync();
                return result;
            }
            catch (Exception ex)
            {
                logger.LogError(ex.Message, ex);
                result.IsSuccessfull = false;
                result.ErrorMessage = ErszConstants.Messages.GeneralException;
                result.ErrorCode = ErszConstants.ResultCodes.GeneralException;
                return result;
            }
        }

        public async Task<SaveResultVM> InsertCaseSessionAmount(ErszCaseSessionAmountModel model)
        {
            var result = InitSaveResultCase();

            if (model == null)
            {
                result.IsSuccessfull = false;
                result.ErrorMessage = ErszConstants.Messages.DataMapFailed;
                result.ErrorCode = ErszConstants.ResultCodes.NullValue;
                return result;
            }

            var validationContext = await isContextValid<ErszCaseSessionAmountModel, CaseSessionAmount>(model);

            if (!validationContext.IsSuccessfull)
            {
                return validationContext;
            }

            var caseSession = getBaseActivityByGid<CaseSession>(model.CaseSessionGid);

            if (caseSession == null)
            {
                result.IsSuccessfull = false;
                result.ErrorMessage = ErszConstants.Messages.CaseSessionNotFound;
                result.ErrorCode = ErszConstants.ResultCodes.InvalidCaseGid;
                return result;
            }

            try
            {
                await repo.AddAsync(new CaseSessionAmount()
                {
                    CaseId = caseSession.CaseId,
                    CaseSesionId = caseSession.Id,
                    CourtId = validationContext.CourtId,
                    JurorMandateId = validationContext.JurorMandateId,
                    Gid = model.Context.Gid,
                    Fee = model.Fee,
                    Fine = model.Fine,
                    Expences = model.Expences,
                    FineIsPaid = model.FineIsPaid,
                    DateCreated = DateTime.Now,
                    DateModified = DateTime.Now,
                });

                await repo.SaveChangesAsync();
                return result;
            }
            catch (Exception ex)
            {
                logger.LogError(ex.Message, ex);
                result.IsSuccessfull = false;
                result.ErrorMessage = ErszConstants.Messages.GeneralException;
                result.ErrorCode = ErszConstants.ResultCodes.GeneralException;
                return result;
            }
        }

        public async Task<SaveResultVM> UpdateCaseSessionAmount(ErszCaseSessionAmountModel model)
        {
            var result = InitSaveResultCase();

            if (model == null)
            {
                result.IsSuccessfull = false;
                result.ErrorMessage = ErszConstants.Messages.DataMapFailed;
                result.ErrorCode = ErszConstants.ResultCodes.NullValue;
                return result;
            }

            var validationContext = await isContextValid<ErszCaseSessionAmountModel, CaseSessionAmount>(model, true);

            if (!validationContext.IsSuccessfull)
            {
                return validationContext;
            }

            var caseSession = getBaseActivityByGid<CaseSession>(model.CaseSessionGid);

            if (caseSession == null)
            {
                result.IsSuccessfull = false;
                result.ErrorMessage = ErszConstants.Messages.CaseSessionNotFound;
                result.ErrorCode = ErszConstants.ResultCodes.InvalidCaseGid;
                return result;
            }

            var caseSessionAmount = repo.All<CaseSessionAmount>()
                .Where(x => x.CaseId == caseSession.CaseId)
                //.Where(x => x.JurorId == validationContext.JurorId)
                .FirstOrDefault();

            if (caseSessionAmount == null)
            {
                result.IsSuccessfull = false;
                result.ErrorCode = ErszConstants.ResultCodes.InvalidValue;
                result.ErrorMessage = ErszConstants.Messages.CaseSessionAmountNotFound;
                return result;
            }

            try
            {
                caseSessionAmount.CaseId = caseSession.CaseId;
                caseSessionAmount.CaseSesionId = caseSession.Id;
                caseSessionAmount.CourtId = validationContext.CourtId;
                //caseSessionAmount.JurorId = validationContext.JurorId;
                caseSessionAmount.Fee = model.Fee;
                caseSessionAmount.Fine = model.Fine;
                caseSessionAmount.Expences = model.Expences;
                caseSessionAmount.FineIsPaid = model.FineIsPaid;
                caseSessionAmount.DateCreated = DateTime.Now;
                caseSessionAmount.DateModified = DateTime.Now;

                repo.Update(caseSessionAmount);
                await repo.SaveChangesAsync();
                return result;
            }
            catch (Exception ex)
            {
                logger.LogError(ex.Message, ex);
                result.IsSuccessfull = false;
                result.ErrorMessage = ErszConstants.Messages.GeneralException;
                result.ErrorCode = ErszConstants.ResultCodes.GeneralException;
                return result;
            }
        }
        private void ValidateString(string data, SaveResultCaseVM result)
        {
            if (string.IsNullOrEmpty(data))
            {
                result.IsSuccessfull = false;
                result.ErrorMessage = ErszConstants.Messages.DataMapFailed;
                result.ErrorCode = ErszConstants.ResultCodes.NullValue;
            }
        }
        private ErszBaseActivitySaveResultWrapper ValidateBaseActivityByGid<T>(string gid) where T : BaseActivity
        {
            var caseDataWrapper = new ErszBaseActivitySaveResultWrapper();
            var result = InitSaveResultCase();

            if (string.IsNullOrEmpty(gid))
            {
                result.IsSuccessfull = false;
                result.ErrorMessage = ErszConstants.Messages.GidNullOrEmpty;
                result.ErrorCode = ErszConstants.ResultCodes.NullValue;
                caseDataWrapper.SaveResultCaseVM = result;
                return caseDataWrapper;
            }

            var caseData = getBaseActivityByGid<T>(gid);
            caseDataWrapper.BaseActivity = caseData;

            if (caseData == null)
            {
                result.IsSuccessfull = false;
                result.ErrorMessage = ErszConstants.Messages.CaseNotFound;
                result.ErrorCode = ErszConstants.ResultCodes.InvalidCaseGid;
                caseDataWrapper.SaveResultCaseVM = result;
                return caseDataWrapper;
            }

            return caseDataWrapper;
        }

        private SaveResultCaseVM InitSaveResultCase()
        {
            return new SaveResultCaseVM()
            {
                IsSuccessfull = true,
                ErrorCode = ErszConstants.ResultCodes.OK,
                ErrorMessage = ""
            };
        }

        private async Task<SaveResultCaseVM> isContextValid<T, Y>(T model, bool isForAmount = false) where T : ErszBaseModel where Y : BaseActivity
        {
            var result = InitSaveResultCase();

            if (model.Context == null)
            {
                result.IsSuccessfull = false;
                result.ErrorCode = ErszConstants.ResultCodes.NullValue;
                result.ErrorMessage = ErszConstants.Messages.DataMapFailed;
                return result;
            }

            if (!isForAmount)
            {
                if (string.IsNullOrEmpty(model.Context.Gid))
                {
                    result.IsSuccessfull = false;
                    result.ErrorCode = ErszConstants.ResultCodes.NullValue;
                    result.ErrorMessage = ErszConstants.Messages.GidNullOrEmpty;
                    return result;
                }

                var baseActivity = getBaseActivityByGid<Y>(model.Context.Gid);

                if (baseActivity != null)
                {
                    result.IsSuccessfull = false;
                    result.ErrorCode = ErszConstants.ResultCodes.InvalidValue;
                    result.ErrorMessage = ErszConstants.Messages.GidExists;
                    return result;
                }
            }

            var juror = registerService.GetJurorByUic(model.Context.JurorUic);

            if (juror == null)
            {
                result.IsSuccessfull = false;
                result.ErrorMessage = ErszConstants.Messages.JurorNotFound;
                result.ErrorCode = ErszConstants.ResultCodes.InvalidCode;
                return result;
            }

            int courtId = await nomenclatureService.GetNomIdByCode<CommonCourt>(model.Context.CourtCode ?? "");

            if (courtId <= 0)
            {
                result.IsSuccessfull = false;
                result.ErrorMessage = ErszConstants.Messages.CourtNotFound;
                result.ErrorCode = ErszConstants.ResultCodes.InvalidCode;

                return result;
            }

            var mandateId = repo.AllReadonly<JurorMandate>()
                .Include(x => x.Court)
                .Where(x => x.JurorId.Equals(juror.Id))
                .Where(x => x.Court.Code.Equals(model.Context.CourtCode))
                .Where(x => (x.DateEnd ?? DateTime.Now) >= DateTime.Now)
                .Select(x => x.Id)
                .FirstOrDefault();

            if (mandateId == 0)
            {
                result.IsSuccessfull = false;
                result.ErrorMessage = ErszConstants.Messages.JurorMandatesNotFound;
                result.ErrorCode = ErszConstants.ResultCodes.InvalidCode;

                return result;
            }

            result.CourtId = courtId;
            result.JurorId = juror.Id;
            result.JurorMandateId = mandateId;
            return result;
        }

        private T getBaseActivityByGid<T>(string gid, bool detached = true) where T : BaseActivity
        {
            if (detached)
            {
                return repo.AllReadonly<T>().Where(x => x.Gid == gid).FirstOrDefault();
            }
            else
            {
                return repo.All<T>().Where(x => x.Gid == gid).FirstOrDefault();
            }
        }

        private SaveResultVM validateCaseRegNumber(string regNumber, int regYear)
        {
            SaveResultVM result = new SaveResultVM()
            {
                IsSuccessfull = false
            };
            if (string.IsNullOrEmpty(regNumber) || regNumber.Length != 14)
            {
                result.ErrorMessage = "Невалиден номер дело.";
                return result;
            }

            try
            {
                var yearStr = regNumber.Substring(0, 4);
                var courtStr = regNumber.Substring(4, 3);
                var characterStr = regNumber.Substring(7, 2);
                var numberStr = regNumber.Substring(9, 5);

                var caseYear = int.Parse(yearStr);
                if (caseYear != regYear)
                {
                    result.ErrorMessage = $"Невалидна година на дело {regYear}";
                    return result;
                }

                var CourtId = repo.AllReadonly<CommonCourt>()
                                                           .Where(x => x.Code == courtStr)
                                                           .Select(x => x.Id)
                                                           .FirstOrDefault();
                if (CourtId == 0)
                {
                    result.ErrorMessage = $"Невалиден код на съд: {courtStr}";
                    return result;
                }


                result.IsSuccessfull = true;
            }
            catch (Exception ex)
            {
                result.ErrorMessage = "Грешка при декодиране на номер дело.";
                result.IsSuccessfull = false;
            }
            return result;
        }

    }



}

