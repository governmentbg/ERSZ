namespace ERSZ.Controllers
{
    public class StatReportController : BaseController
    {
/*        
        private readonly IStatReportService reportService;
        private readonly INomenclatureService nomService;
        public StatReportController(IStatReportService _reportService, INomenclatureService _nomService)
        {
            reportService = _reportService;
            nomService = _nomService;
            ViewBag.MenuItemValue = "StatReport";
        }

        #region NomStatReport

       
        public IActionResult ReportIndex(int integrationId)
        {
            var model = new FilterStatReportVM()
            {
                IntegrationId = integrationId
            };
            ViewBag.IntegrationLabel = nomService.GetById<NomIntegration>(integrationId)?.Label;
            ViewBag.CatalogId_ddl = nomService.GetDropDownListExpr<NomCatalog>(x => x.IntegrationId == integrationId, false, true);
            ViewBag.ReportTypeId_ddl = nomService.GetDropDownList<NomStatReportType>(false, true);

            return View(model);
        }

        public IActionResult Report_ChangeOrder(int id, bool moveUp)
        {
            return Json(nomService.ChangeOrder_IOrderable<NomStatReport>(moveUp, id));
        }

        [HttpPost]
        public IActionResult Report_LoadData(IDataTablesRequest request, FilterStatReportVM filter)
        {
            var data = reportService.StatReport_Select(filter);
            return request.GetResponse(data);
        }

        public IActionResult ReportAdd(int integrationId)
        {
            var model = new NomStatReport()
            {
                IntegrationId = integrationId,
                DateStart = DateTime.Now,
                IsActive = true
            };
            SetViewBag_Report(model);
            return View(nameof(ReportEdit), model);
        }
        public IActionResult ReportEdit(int id)
        {
            var model = nomService.GetById<NomStatReport>(id);
            SetViewBag_Report(model);

            //string values = nomService.GetValuesFromObject(model,
            //        ViewData,
            //        q => q.CatalogId,
            //        q => q.ReportTypeId,
            //        q => q.Label,
            //        q => q.Code,
            //        q => q.IsActive,
            //        q => q.DateStart,
            //        q => q.DateEnd
            //    );

            return View(nameof(ReportEdit), model);
        }

        void SetViewBag_Report(NomStatReport model)
        {
            ViewBag.ReportTypeId_ddl = nomService.GetDropDownList<NomStatReportType>(false);
            if (model.IntegrationId == 0 && model.CatalogId > 0)
            {
                model.IntegrationId = reportService.GetById<NomCatalog>(model.CatalogId).IntegrationId;
            }
            ViewBag.ReportCategoryId_ddl = nomService.GetDropDownListExpr<NomStatReportCategory>(x => x.IntegrationId == model.IntegrationId, false);
            ViewBag.CatalogId_ddl = nomService.GetDropDownListExpr<NomCatalog>(x => x.IntegrationId == model.IntegrationId);
            ViewBag.EntityList = nomService.GetEntityListByIntegration(model.IntegrationId, model.EntityList);

            //Second Integration Type
            ViewBag.SecondIntegrationId_ddl = nomService.GetDropDownList<NomIntegration>(true);
        }

        [HttpPost]
        public async Task<IActionResult> ReportEdit(NomStatReport model)
        {
            var result = await reportService.StatReport_SaveData(model);
            if (result.IsSuccessfull)
            {
                SetSuccessMessage(MessageConstant.Values.SaveOK);
                return RedirectToAction(nameof(ReportEdit), new { id = model.Id });
            }
            SetViewBag_Report(model);
            SetErrorMessage(MessageConstant.Values.SaveFailed);
            return View(nameof(ReportEdit), model);
        }

        #endregion


        #region StatReportCol

        [HttpPost]
        public IActionResult ReportCol_LoadData(IDataTablesRequest request, int statReportId)
        {
            var data = reportService.StatReportCol_Select(statReportId);
            return request.GetResponse(data);
        }

        public IActionResult ReportCol_ChangeOrder(int id, bool moveUp)
        {
            var model = nomService.GetById<NomStatReportCol>(id);
            return Json(nomService.ChangeOrder_IOrderable<NomStatReportCol>(moveUp, id, x => x.StatReportId == model.StatReportId));
        }

        public IActionResult ReportColAdd(int statReportId)
        {
            var model = new NomStatReportCol()
            {
                StatReportId = statReportId,
                IsActive = true
            };
            SetViewBag_ReportCol(model);
            return View(nameof(ReportColEdit), model);
        }
        public IActionResult ReportColEdit(int id)
        {
            var model = nomService.GetById<NomStatReportCol>(id);
            SetViewBag_ReportCol(model);


            return View(nameof(ReportColEdit), model);
        }

        [HttpPost]
        public async Task<IActionResult> ReportColEdit(NomStatReportCol model)
        {
            var result = await reportService.StatReportCol_SaveData(model);
            if (result.IsSuccessfull)
            {
                SetSuccessMessage(MessageConstant.Values.SaveOK);
                return Redirect(Url.Action(nameof(ReportEdit), new { id = model.StatReportId }) + "#tabCols");
            }
            SetViewBag_ReportCol(model);
            SetErrorMessage(MessageConstant.Values.SaveFailed);
            return View(nameof(ReportColEdit), model);
        }

        void SetViewBag_ReportCol(NomStatReportCol model)
        {
            var catalogId = reportService.GetById<NomStatReport>(model.StatReportId).CatalogId;
            ViewBag.CatalogCodeId_ddl = nomService.GetDropDownListExpr<NomCatalogCode>(x => x.CatalogId == catalogId, true, false, true, true);
        }


        #endregion

        #region StatReportCaseCode

        [HttpPost]
        public IActionResult ReportCaseCode_LoadData(IDataTablesRequest request, int statReportId)
        {
            var data = reportService.StatReportCaseCode_Select(statReportId);
            return request.GetResponse(data);
        }

        public IActionResult ReportCaseCode_ChangeOrder(int id, bool moveUp)
        {
            var model = nomService.GetById<NomStatReportCode>(id);
            return Json(nomService.ChangeOrder_IOrderable<NomStatReportCode>(moveUp, id, x => x.StatReportId == model.StatReportId));
        }

        public IActionResult ReportCaseCodeAdd(int statReportId)
        {
            var model = new NomStatReportCode()
            {
                StatReportId = statReportId,
                IsActive = true
            };
            SetViewBag_ReportCaseCode(model);
            return View(nameof(ReportCaseCodeEdit), model);
        }
        public IActionResult ReportCaseCodeEdit(int id)
        {
            var model = nomService.GetById<NomStatReportCode>(id);
            SetViewBag_ReportCaseCode(model);


            return View(nameof(ReportCaseCodeEdit), model);
        }

        [HttpPost]
        public async Task<IActionResult> ReportCaseCodeEdit(NomStatReportCode model)
        {
            var result = await reportService.StatReportCaseCode_SaveData(model);
            if (result.IsSuccessfull)
            {
                SetSuccessMessage(MessageConstant.Values.SaveOK);
                return Redirect(Url.Action(nameof(ReportEdit), new { id = model.StatReportId }) + "#tabCols");
            }
            SetViewBag_ReportCaseCode(model);
            SetErrorMessage(MessageConstant.Values.SaveFailed);
            return View(nameof(ReportCaseCodeEdit), model);
        }

        void SetViewBag_ReportCaseCode(NomStatReportCode model)
        {
            var catalogId = reportService.GetById<NomStatReport>(model.StatReportId).CatalogId;
            ViewBag.CaseCodeId_ddl = nomService.GetDropDownListExpr<NomCaseCode>(x => x.CaseCodeCatalogs.Any(), true, false, true, true);
        }

        #endregion

        //#region NomStatReportItem

        //public IActionResult ReportItemIndex(int statReportId)
        //{
        //    TempData["NomenclatureName"] = nameof(NomStatReportItem);
        //    ViewBag.statReportId = statReportId;
        //    return View();
        //}

        //[HttpPost]
        //public IActionResult ReportItem_LoadData(IDataTablesRequest request, int statReportId)
        //{
        //    var data = nomService.GetStatReportItems(statReportId);
        //    return request.GetResponse(data);
        //}

        //public IActionResult ReportItem_Add(int statReportId)
        //{
        //    var model = new NomStatReportItem()
        //    {
        //        StatReportId = statReportId,
        //        DateStart = DateTime.Now,
        //        IsActive = true
        //    };
        //    SetViewBag_ReportItem();
        //    return View(nameof(ReportItem_Edit), model);
        //}
        //public IActionResult ReportItem_Edit(int id)
        //{
        //    var model = nomService.GetById<NomStatReportItem>(id);
        //    SetViewBag_ReportItem();
        //    return View(nameof(ReportItem_Edit), model);
        //}

        //void SetViewBag_ReportItem()
        //{
        //    ViewBag.IntegrationId_ddl = nomService.GetDropDownList<NomIntegration>(false);
        //}

        //[HttpPost]
        //public IActionResult ReportItem_Edit(NomStatReportItem model)
        //{

        //    if (nomService.SaveItem<NomStatReportItem>(model))
        //    {
        //        return RedirectToAction(nameof(ReportItem_Edit), new { id = model.Id });
        //    }
        //    SetViewBag_ReportItem();
        //    return View(nameof(ReportItem_Edit), model);
        //}

        //#endregion

        */

    }
}
