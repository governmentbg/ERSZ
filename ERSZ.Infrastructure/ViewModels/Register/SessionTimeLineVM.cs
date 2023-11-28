using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ERSZ.Infrastructure.ViewModels.Register
{
    public class SessionTimeLineVM
    {
        public int Id { get; set; }
        public int CaseId { get; set; }
        public string CaseLabel { get; set; }
        public string SessionKind { get; set; }
        public string State { get; set; }
        public string Result { get; set; }
        public DateTime DateStart { get; set; }

        public decimal Fee { get; set; }
        public decimal Expences { get; set; }
        public decimal Fine { get; set; }

        public virtual ICollection<ActTimeLineVM> Acts { get; set; }

        public SessionTimeLineVM()
        {
            Acts = new HashSet<ActTimeLineVM>();
        }
    }
}
