using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ERSZ.Infrastructure.ViewModels.Register
{
    public class ActTimeLineVM
    {
        public int Id { get; set; }
        public string ActKind { get; set; }
        public string RegNumber { get; set; }
        public DateTime RegDate { get; set; }
    }
}
