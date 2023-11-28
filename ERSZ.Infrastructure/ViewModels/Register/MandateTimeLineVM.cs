using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ERSZ.Infrastructure.ViewModels.Register
{
    public class MandateTimeLineVM
    {
        public int Id { get; set; }

        /// <summary>
        /// Ид на родител
        /// </summary>
        public int? ParentId { get; set; }

        /// <summary>
        /// Дата на родител
        /// </summary>
        public DateTime ParentDateFrom { get; set; }

        /// <summary>
        /// Съд
        /// </summary>
        public string CourtLabel { get; set; }

        /// <summary>
        /// От дата
        /// </summary>
        public DateTime DateFrom { get; set; }

        /// <summary>
        ///  До дата
        /// </summary>
        public DateTime? DateTo { get; set; }

        /// <summary>
        /// Вид
        /// </summary>
        public string MandateTypeLabel { get; set; }

        /// <summary>
        /// Дата на предсрочно прекратяване
        /// </summary>
        public DateTime? DateTermination { get; set; }

        /// <summary>
        /// Предсрочно прекратяване
        /// </summary>
        public bool IsDateTermination { get; set; }

        /// <summary>
        /// Командировки
        /// </summary>
        public IList<MandateTimeLineVM> BusinessTrip { get; set; }

        public string MandateDate
        {
            get
            {
                return DateFrom.ToString("dd.MM.yyг. hh.mmч.") + " - " + (DateTo == null ? "Не е зададена крайна дата" : DateTo?.ToString("dd.MM.yyг. hh.mmч."));
            }
        }

        public MandateTimeLineVM()
        {
            BusinessTrip = new List<MandateTimeLineVM>();
        }
    }
}
