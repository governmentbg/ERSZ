using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ERSZ.Infrastructure.ViewModels.Register
{
    public class JurorListVM
    {
        public string  Id { get; set; }
        public string FullName { get; set; }
        public bool IsActive { get; set; }
        public byte[] Content { get; set; }
        public ICollection<string> Specialities { get; set; }
        public ICollection<MandateListVM> Mandates { get; set; }
        public JurorListVM()
        {
            Specialities= new List<string>();   
            Mandates= new List<MandateListVM>();   
        }
    }
}
