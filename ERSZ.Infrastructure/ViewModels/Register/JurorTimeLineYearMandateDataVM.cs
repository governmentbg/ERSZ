using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ERSZ.Infrastructure.ViewModels.Register
{
    public class JurorTimeLineYearMandateDataVM
    {
        public int MandateId { get; set; }
        public string MandateLabel { get; set; }
        public int Year { get; set; }
        public int? ParentId { get; set; }
        public string ParentLabel { get; set; }
        public int CaseId { get; set; }
        public string CaseLabel { get; set; }
        public int CountSession { get; set; }
    }
}
