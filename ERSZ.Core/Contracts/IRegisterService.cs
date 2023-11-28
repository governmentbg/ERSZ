using ERSZ.Infrastructure.Data.Models.Case;
using ERSZ.Infrastructure.Data.Models.Register;
using ERSZ.Infrastructure.ViewModels.Common;
using ERSZ.Infrastructure.ViewModels.Register;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ERSZ.Core.Contracts
{
    public interface IRegisterService : IBaseService
    {
        Task<SaveResultVM> Juror_SaveData(JurorVM jurorVM);
        IQueryable<JurorListVM> Juror_Select(FilterJurorVM model);
        Task<SaveResultVM> Mandate_SaveData(MandateVM mandateVM);
        IQueryable<MandateListVM> Mandate_Select(FilterMandateVM model);
        IQueryable<MandateListVM> Mandate_Select(string jurorId);
        List<SelectListItem> GetJurorDropDownList(bool addDefaultElement = true, bool addAllElement = false, bool orderByNumber = true);
        List<SelectListItem> GetCourtTypeDropDownList(bool addDefaultElement = true, bool addAllElement = false, bool orderByNumber = true);
        List<SelectListItem> GetCourtDropDownList(bool addDefaultElement = true, bool addAllElement = false, bool orderByNumber = true);
        JurorVM GetJurorById(string id);
        MandateVM GetMandateById(int id);
        Juror GetJurorByUic(string uic);
        bool IsExistsMandate(MandateVM mandateVM);
        JurorTimeLineVM GetJurorTimeLineVM(string Id);
        List<JurorTimeLineYearMandateVM> GetDataJurorTimeLineVMByMandateOrJuror(string JurorId, int? MandateId);
        List<SessionTimeLineVM> GetSessionTimeLine(int CaseId, int Year, int MandateId);
        bool IsExistActiveMandatesAfterDate(DateTime dateTime, string JurorId);
        Task<SaveResultVM> DeletePhoto(string Id);
        IQueryable<ReportAggregatedDataVM> ReportAggregatedData(FilterReportAggregatedDataVM ModelFilter);
        JurorVM InitJurorModel();
        IQueryable<ReportFullVM> ReportFullData_Select(FilterReportFullVM filter);
        IQueryable<CourtLocalReportVM> CourtLocalReport(FilterCourtLocalReportVM ModelFilter);
        bool IsExistMandateNo(string JurorId, int MandateNo, int? Id);
    }
}
