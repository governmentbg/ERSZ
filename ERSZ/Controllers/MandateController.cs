using DataTables.AspNet.Core;
using ERSZ.Core.Constants;
using ERSZ.Core.Contracts;
using ERSZ.Extensions;
using ERSZ.Infrastructure.Constants;
using ERSZ.Infrastructure.Contracts;
using ERSZ.Infrastructure.Data.Models.Common;
using ERSZ.Infrastructure.Data.Models.Nomenclatures;
using ERSZ.Infrastructure.ViewModels.Register;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace ERSZ.Controllers
{
    public class MandateController : BaseController
    {
        private readonly IRegisterService registerService;
        private readonly INomenclatureService nomService;
        private readonly IEkDitrictService ekDitrictService;

        public MandateController(IRegisterService _registerService, INomenclatureService _nomService,
            IEkDitrictService _ekDitrictService)
        {
            registerService = _registerService;
            nomService = _nomService;
            ViewBag.MenuItemValue = "Mandate";
            ekDitrictService = _ekDitrictService;
        }


        #region Mandate
        public IActionResult Index()
        {
            var model = new FilterMandateVM()
            {

            };

            ViewBag.MunicipalityId_ddl = ekDitrictService.GetMunicipalityDropDownList(false, true);
            ViewBag.CourtId_ddl = nomService.GetDropDownList<CommonCourt>(false, true);
            ViewBag.MandateTypeId_ddl = nomService.GetDropDownList<NomMandateType>(false, true);
            return View(model);
        }

        public IActionResult Add(string jurorId, int? mandateId = null)
        {

            var model = new MandateVM()
            {
                JurorId = jurorId,
                ParentId = mandateId,
                MandateTypeId = JurorConstants.Mandate.MandateType,
                MandateTypeLabel = JurorConstants.MandateLable.MandateType,
                DateStart = DateTime.Now,
                JurorFullName = registerService.GetJurorById(jurorId).FullName
            };

            if (mandateId > 0)
            {
                model.MandateTypeId = JurorConstants.Mandate.MandateMissionType;
                model.MandateTypeLabel = JurorConstants.MandateLable.MandateMissionType;
            }

            SetViewBag_Mandate();
            return View(nameof(Edit), model);
        }

        public IActionResult Edit(int id)
        {
            var model = registerService.GetMandateById(id);
            if (model == null)
            {
                return View("Error");
            }

            Audit_Operation = NomenclatureConstants.AuditOperations.View;
            Audit_Object = $"Мандат на: {model.JurorFullName}";
            SetViewBag_Mandate();

            return View(nameof(Edit), model);
        }

        private string IsValid(MandateVM model)
        {
            var isExist = registerService.IsExistsMandate(model);
            if (isExist)
            {
                return model.MandateTypeId == JurorConstants.Mandate.MandateType ? JurorConstants.Message.MandateExists : JurorConstants.Message.MandateMissionExists;
            }

            if (model.MandateTypeId == JurorConstants.Mandate.MandateMissionType)
            {
                var mandateVM = registerService.GetMandateById(model.ParentId ?? 0);

                if (mandateVM.DateStart > model.DateStart)
                    return "Не може началната дата на командироването да е преди началото на мандата";

                if ((mandateVM.DateEnd ?? DateTime.Now.AddYears(20)) < (model.DateEnd ?? DateTime.Now.AddYears(10)))
                    return "Не може крайната дата на командироването да е след крайната дата на мандата";
            }

            if (model.MandateTypeId == JurorConstants.Mandate.MandateType)
            {
                if (model.DateTermination != null)
                {
                    if (string.IsNullOrEmpty(model.DateTerminationDescription))
                        return "Попълнете основание за предсрочно прекратяване";
                }

                if (model.MandateNo < 1)
                    return "Попълнете номер на мандата";

                if (registerService.IsExistMandateNo(model.JurorId, model.MandateNo, model.Id > 0 ? model.Id : null))
                    return "Вече съществува такъв номер на мандата";
            }

            var juror = registerService.GetJurorById(model.JurorId);

            var dateTime = DateTime.Now;

            if (model.DateStart > (juror.DateEnd ?? dateTime.AddYears(10)))
            {
                return "Началната дата на " + (model.MandateTypeId == JurorConstants.Mandate.MandateMissionType ? "командироването" : "мандата") + " е по-голяма от крайната дата на профила на заседателя";
            }

            if (model.DateStart > (model.DateEnd ?? DateTime.Now.AddYears(10)))
                return "Дата от е по-голяма от дата до";

            if (model.DateEnd != null)
            {
                if ((model.DateEnd ?? dateTime.AddYears(10)) > (juror.DateEnd ?? dateTime.AddYears(10)))
                {
                    return "Крайната дата на " + (model.MandateTypeId == JurorConstants.Mandate.MandateMissionType ? "командироването" : "мандата") + " е по-голяма от крайната дата на профила на заседателя";
                }
            }

            return string.Empty;
        }

        [HttpPost]
        public async Task<IActionResult> Edit(MandateVM model)
        {
            SetViewBag_Mandate();

            if (ModelState.IsValid)
            {
                string _isvalid = IsValid(model);
                if (_isvalid != string.Empty)
                {
                    SetErrorMessage(_isvalid);
                    return View(nameof(Edit), model);
                }

                if (model.Id > 0)
                {
                    Audit_Operation = NomenclatureConstants.AuditOperations.Edit;
                    Audit_Object = $"Мандат на: {model.JurorFullName}";
                }
                else
                {
                    Audit_Operation = NomenclatureConstants.AuditOperations.Add;
                    Audit_Object = $"Мандат на: {model.JurorFullName}";
                }

                var modelId = model.Id;
                var result = await registerService.Mandate_SaveData(model);

                if (result.IsSuccessfull)
                {
                    SaveLogOperation(modelId, model, model.Id);
                    SetSuccessMessage(MessageConstant.Values.SaveOK);
                    return RedirectToAction(nameof(Edit), new { id = model.Id });
                }

                SetErrorMessage(result.ErrorMessage);
                return View(nameof(Edit), model);
            }


            SetErrorMessage(MessageConstant.Values.UpdateFailed);
            return View(nameof(Edit), model);
        }

        #endregion

        #region LoadData

        [HttpPost]
        public IActionResult Mandate_LoadData(IDataTablesRequest request, FilterMandateVM filter)
        {
            var data = registerService.Mandate_Select(filter);
            return request.GetResponse(data);
        }
        #endregion

        #region ViewBags
        void SetViewBag_Mandate()
        {
            Expression<Func<CommonCourt, bool>> registerCourtId = x => CourtConstants.CourtType.CourtFromSelected.Contains(x.CourtTypeId);
            Expression<Func<CommonCourt, bool>> courtId = x => CourtConstants.CourtType.CourtInSelected.Contains(x.CourtTypeId);

            ViewBag.MunicipalityId_ddl = ekDitrictService.GetMunicipalityDropDownList(false, true);
            ViewBag.JurorId_ddl = registerService.GetJurorDropDownList();
            ViewBag.RegisterCourtId_ddl = nomService.GetDropDownListExpr(registerCourtId, false, true);
            ViewBag.CourtId_ddl = nomService.GetDropDownListExpr(courtId, false, true);
            ViewBag.MandateTypeId_ddl = nomService.GetDropDownList<NomMandateType>(false, true);
        }
        #endregion
    }
}
