using ERSZ.Infrastructure.Data.Models.Base;
using ERSZ.Infrastructure.Data.Models.Nomenclatures;
using ERSZ.Infrastructure.Data.Models.Ekatte;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ERSZ.Infrastructure.Data.Models.Register
{
    /// <summary>
    /// Заседател
    /// </summary>
    public class Juror : UserWrtModel
    {
        [Key]
        public string Id { get; set; }


        [Display(Name = "Идентификатор")]
        public string Uic { get; set; }

        //[Display(Name = "Вид идентификатор")]
        //public int UicTypeId { get; set; }

        [Display(Name = "Собствено име")]
        public string FirstName { get; set; }

        [Display(Name = "Бащино име")]
        public string MiddleName { get; set; }

        [Display(Name = "Фамилия 1")]
        public string FamilyName { get; set; }

        [Display(Name = "Фамилия 2")]
        public string Family2Name { get; set; }

        [Display(Name = "Наименование")]
        public string FullName { get; set; }

        public int AddressCityId { get; set; }
        public string AddressText { get; set; }
        public int EducationId { get; set; }
        public int? EducationRangId { get; set; }
        public DateTime DateStart { get; set; }
        public DateTime? DateEnd { get; set; }
        public string Phone { get; set; }
        public string EMail { get; set; }

        public string FileName { get; set; }
        public byte[] Content { get; set; }
        public string ContentType { get; set; }

        public string RegNumber { get; set; }


        [ForeignKey(nameof(AddressCityId))]
        public virtual EkEkatte AddressCity { get; set; }

        [ForeignKey(nameof(EducationId))]
        public virtual NomEducation Education { get; set; }

        [ForeignKey(nameof(EducationRangId))]
        public virtual NomEducationRang EducationRang { get; set; }

        [InverseProperty("Juror")]
        public virtual ICollection<JurorMandate> Mandates { get; set; }


        public virtual ICollection<JurorSpeciality> Specialities { get; set; }

        public Juror()
        {
            Mandates = new HashSet<JurorMandate>();
            Specialities = new HashSet<JurorSpeciality>();
        }
    }
}
