using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ERSZ.Core.Models
{
    public class CourtTypeModel: IEquatable<CourtTypeModel>
    {
        public int CourtId { get; set; }
        public int CourtTypeId { get; set; }
        public string CourtLabel { get; set; }
        public int JurorCount { get; set; }
        public string CssClass { get; set; }
        public bool Equals(CourtTypeModel other)
        {
           return CourtLabel == other.CourtLabel && CssClass == other.CssClass;
        }
    }


    public class Court_JurorMandate_JoinModel
    {
        public string MapId { get; set; }
        public string CourtType { get; set; }
        public string CourtTypeCode { get; set; }
        public DateTime MandateDateStart { get; set; }
        public DateTime? MandateDateEnd { get; set; }
    }
}
