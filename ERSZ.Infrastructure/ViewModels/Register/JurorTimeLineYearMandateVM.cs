using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ERSZ.Infrastructure.ViewModels.Register
{
    public class JurorTimeLineYearMandateVM
    {
        public int MandateId { get; set; }
        public string MandateLabel { get; set; }
        public int Year { get; set; }

        public IList<JurorTimeLineYearMandateDataVM> JurorTimeLineYearMandateDatas { get; set; }

        public JurorTimeLineYearMandateVM()
        {
            JurorTimeLineYearMandateDatas = new List<JurorTimeLineYearMandateDataVM>();
        }
    }
}
