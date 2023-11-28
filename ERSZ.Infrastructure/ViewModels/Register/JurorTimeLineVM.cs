using ERSZ.Infrastructure.ViewModels.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ERSZ.Infrastructure.ViewModels.Register
{
    public class JurorTimeLineVM
    {
        public string Id { get; set; }

        /// <summary>
        /// Име
        /// </summary>
        public string FullName { get; set; }

        /// <summary>
        /// Образование
        /// </summary>
        public string EducationLabel { get; set; }

        /// <summary>
        /// Образователно-кв. степен
        /// </summary>
        public string EducationRangLabel { get; set; }

        /// <summary>
        /// ЕГН
        /// </summary>
        public string Uic { get; set; }

        public string CityLabel { get; set; }

        /// <summary>
        /// Адрес
        /// </summary>
        public string AddressText { get; set; }

        /// <summary>
        /// Телефон
        /// </summary>
        public string Phone { get; set; }

        /// <summary>
        /// Е-mail
        /// </summary>
        public string EMail { get; set; }

        /// <summary>
        /// Снимка
        /// </summary>
        public byte[] Content { get; set; }

        /// <summary>
        /// Специалности
        /// </summary>
        public IList<string> Specialities { get; set; }
        public IList<MandateTimeLineVM> Mandates { get; set; }
        public IList<JurorTimeLineYearMandateVM> JurorTimeLineYearMandates { get; set; }

        public JurorTimeLineVM()
        {
            Specialities = new List<string>();
            Mandates = new List<MandateTimeLineVM>();
            JurorTimeLineYearMandates = new List<JurorTimeLineYearMandateVM>();
        }
    }
}
