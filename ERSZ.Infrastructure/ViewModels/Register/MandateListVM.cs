using ERSZ.Infrastructure.Constants;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ERSZ.Infrastructure.ViewModels.Register
{
    public class MandateListVM
    {
        public int Id { get; set; }
        public int? ParentId { get; set; }
        public DateTime ParentDateFrom { get; set; }
        public string JurorId { get; set; }
        public string JurorFullName { get; set; }
        public string CourtLabel { get; set; }
        public int MandateTypeId { get; set; }
        public string MandateType { get; set; }
        public DateTime DateFrom { get; set; }
        public DateTime? DateTo { get; set; }
        public DateTime? DateTermination { get; set; }
        public bool IsDateTermination { get; set; }
        public bool IsActiveJuror { get; set; }
        public int MandateNo { get; set; }
        public string MandateTypeNo { get; set; }

        public bool IsExpired
        {
            get
            {
                return DateTo < DateTime.Now;
            }
        }

        public string MandateInfo
        {
            get
            {
                return CourtLabel + "&nbsp;&nbsp;&nbsp;" + DateFrom.ToString("dd.MM.yyг. hh.mmч.") + " - " + DateTo?.ToString("dd.MM.yyг. hh.mmч.");
            }
        }

        public string MandateInfoShort
        {
            get
            {
                return "от " + DateFrom.ToString("dd.MM.yyг. hh.mmч.") + " до " + DateTo?.ToString("dd.MM.yyг. hh.mmч.");
            }
        }

        public bool IsMandate
        {
            get
            {
                return MandateTypeId == JurorConstants.Mandate.MandateType;
            }
        }

        public ICollection<MandateListVM> BusinessTrip { get; set; }
        public MandateListVM()
        {
            BusinessTrip = new List<MandateListVM>();
        }
    }
}
