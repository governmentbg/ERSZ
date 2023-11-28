using System;
using System.ComponentModel.DataAnnotations;

namespace ERSZ.Infrastructure.ViewModels.Register
{
    public class ReportFullVM
    {
        public string JurorId { get; set; }
        public string RegNumber { get; set; }
        public string MandateNo { get; set; }
        public string JurorName { get; set; }
        public string[] Specialities { get; set; }
        public string MandateCourt { get; set; }
        public string KomandirovkaCourt { get; set; }
        public DateTime MandateFrom { get; set; }
        public DateTime? MandateTo { get; set; }
        public string CaseType { get; set; }
        public string ShortNumber { get; set; }
        public int CaseYear { get; set; }
        public bool CaseIsFinished { get; set; }
        public string SessionKind { get; set; }
        public string SessionResult { get; set; }
        public DateTime SessionDate { get; set; }
        public ReportFullActVM[] Acts { get; set; }
    }

    public class ReportFullActVM
    {
        public string ActKind { get; set; }
        public string RegNumber { get; set; }
        public DateTime RegDate { get; set; }
    }

    public class FilterReportFullVM
    {
        public bool IsGlobalReport { get; set; }

        [Display(Name = "Дата от")]
        public DateTime? SessionDateFrom { get; set; }
        [Display(Name = "Дата до")]
        public DateTime? SessionDateTo { get; set; }
        [Display(Name = "Регистров номер")]
        public string RegisterNumber { get; set; }
        [Display(Name = "Имена заседател")]
        public string JurorName { get; set; }

        [Display(Name = "Специалност")]
        public int? SpecialityId { get; set; }

        [Display(Name = "Апелативен район")]
        public int? AppealRegionId { get; set; }
        [Display(Name = "Съд на назначаване")]
        public int? MandateCourtId { get; set; }
        [Display(Name = "Съд на командироване")]
        public int? KomandirovkaCourtId { get; set; }
        [Display(Name = "Вид дело")]
        public int? CaseTypeId { get; set; }
        [Display(Name = "No дело")]
        public string CaseNumber { get; set; }
        [Display(Name = "Година")]
        public int CaseYear { get; set; }
        [Display(Name = "Свършено/ несвършено")]
        public int? CaseIsFinished { get; set; }
        [Display(Name = "Вид акт")]
        public string ActType { get; set; }
        [Display(Name = "Резултат от заседанието")]
        public string SessionResult { get; set; }

    }
}
