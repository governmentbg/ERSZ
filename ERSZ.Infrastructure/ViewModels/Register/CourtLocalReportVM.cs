using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ERSZ.Infrastructure.ViewModels.Register
{
    public class CourtLocalReportVM
    {
        public string RegNumber { get; set; }
        public string MandateNo { get; set; }
        public string FullName { get; set; }
        public string[] Specialities { get; set; }
        public DateTime MandateDateFrom { get; set; }
        public DateTime? MandateDateTo { get; set; }
        public string CaseType { get; set; }
        public string CaseRegNumber { get; set; }
        public string SessionLabel { get; set; }
        public string[] SessionActs { get; set; }
    }

    public class FilterCourtLocalReportVM
    {
        [Display(Name = "От дата на заседание")]
        public DateTime? DateFrom { get; set; }

        [Display(Name = "До дата на заседание")]
        public DateTime? DateTo { get; set; }

        [Display(Name = "Вид дело")]
        public int CaseTypeId { get; set; }

        [Display(Name = "Номер на дело")]
        public string CaseRegNumber { get; set; }

        [Display(Name = "Свършени/несвършени")]
        public string IsFinished { get; set; }

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
