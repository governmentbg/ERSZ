using DataTables.AspNet.Core;
using ERSZ.Core.Contracts;
using ERSZ.Extensions;
using ERSZ.Infrastructure.Constants;
using ERSZ.Infrastructure.Data.Models.Common;
using ERSZ.Infrastructure.Data.Models.Nomenclatures;
using ERSZ.Infrastructure.ViewModels.Register;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Linq;
using System.Linq.Expressions;

namespace ERSZ.Controllers
{
    public class ReportController : BaseController
    {
        private readonly IRegisterService registerService;
        private readonly INomenclatureService nomService;

        public ReportController(IRegisterService _registerService,
                                INomenclatureService _nomService)
        {
            registerService = _registerService;
            nomService = _nomService;
            ViewBag.MenuItemValue = "Report";
        }

        public IActionResult IndexReportAggregatedData()
        {
            var model = new FilterReportAggregatedDataVM()
            {
                DateFrom = new DateTime(DateTime.Now.Year, 1, 1),
                DateTo = new DateTime(DateTime.Now.Year, 12, 31),
            };

            Expression<Func<CommonCourt, bool>> courtIdSearch = x => CourtConstants.CourtType.CourtInSelected.Contains(x.CourtTypeId);

            ViewBag.CourtMandateId_ddl = nomService.GetDropDownListExpr(courtIdSearch, false, true);
            ViewBag.CourtKomandirovkaId_ddl = nomService.GetDropDownListExpr(courtIdSearch, false, true);
            return View(model);
        }

        [HttpPost]
        public IActionResult ReportAggregatedData_LoadData(IDataTablesRequest request, FilterReportAggregatedDataVM filter)
        {
            var data = registerService.ReportAggregatedData(filter);
            return request.GetResponse(data);
        }

        public IActionResult ReportFullData()
        {
            var model = new FilterReportFullVM();
            ViewBag.CaseTypeId_ddl = nomService.GetDropDownList<NomCaseType>(false, true);
            Expression<Func<CommonCourt, bool>> courtIdSearch = x => CourtConstants.CourtType.CourtInSelected.Contains(x.CourtTypeId);

            ViewBag.SpecialityId_ddl = nomService.GetDropDownList<NomSpeciality>(false, true);
            ViewBag.MandateCourtId_ddl = nomService.GetDropDownListExpr(courtIdSearch, false, true);
            ViewBag.KomandirovkaCourtId_ddl = nomService.GetDropDownListExpr(courtIdSearch, false, true);
            return View(model);
        }

        [HttpPost]
        public IActionResult ReportFullData_LoadData(IDataTablesRequest request, FilterReportFullVM filter)
        {
            var data = registerService.ReportFullData_Select(filter);
            return request.GetResponse(data);
        }

        public IActionResult IndexCourtLocalReport()
        {
            var model = new FilterCourtLocalReportVM();

            ViewBag.CaseTypeId_ddl = nomService.GetDropDownList<NomCaseType>(false, true);
            ViewBag.IsFinished_ddl = nomService.GetDDL_FinishedCase();
            return View(model);
        }

        [HttpPost]
        public IActionResult CourtLocalReport_LoadData(IDataTablesRequest request, FilterCourtLocalReportVM filter)
        {
            var data = registerService.CourtLocalReport(filter);
            return request.GetResponse(data);
        }
    }
}
