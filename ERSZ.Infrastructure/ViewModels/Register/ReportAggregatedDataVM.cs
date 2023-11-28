using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ERSZ.Infrastructure.ViewModels.Register
{
    public class ReportAggregatedDataVM
    {
        public int PrentMandateId { get; set; }
        public int MandateId { get; set; }
        public string RegNumber { get; set; }
        public string FullName { get; set; }
        public string CourtMandateLabel { get; set; }
        public string CourtKomandirovkaLabel { get; set; }
        public DateTime DateFrom { get; set; }
        public DateTime? DateTo { get; set; }
        public int CountCase { get; set; }
        public int CountAct { get; set; }
        public decimal Fee { get; set; }
        public decimal Expences { get; set; }
        public decimal Fine { get; set; }
    }

    public class FilterReportAggregatedDataVM
    {
        [Display(Name = "От дата на мандата")]
        public DateTime? DateFrom { get; set; }

        [Display(Name = "До дата на мандата")]
        public DateTime? DateTo { get; set; }

        [Display(Name = "Съд на назначаване")]
        public int CourtMandateId { get; set; }

        [Display(Name = "Съд на командироване")]
        public int CourtKomandirovkaId { get; set; }

        [Display(Name = "Регистров номер")]
        public string RegNumber { get; set; }

        [Display(Name = "Име")]
        public string FullName { get; set; }

        [Display(Name = "ЕГН")]
        public string Uic { get; set; }

        [Display(Name = "Вид заседание")]
        public string SessionKind { get; set; }

        [Display(Name = "Резултат от заседание")]
        public string SessionState { get; set; }

        [Display(Name = "Съдебен акт")]
        public string ActKind { get; set; }
    }
}
