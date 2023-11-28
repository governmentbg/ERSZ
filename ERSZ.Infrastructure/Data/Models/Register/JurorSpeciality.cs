using ERSZ.Infrastructure.Data.Models.Base;
using ERSZ.Infrastructure.Data.Models.Nomenclatures;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ERSZ.Infrastructure.Data.Models.Register
{
    /// <summary>
    /// Специалности към заседател
    /// </summary>
    [Display(Name = "Специалности към заседател")]
    public class JurorSpeciality : UserWrtModel
    {
        [Key]
        public int Id { get; set; }

        public string JurorId { get; set; }

        public int SpecialityId { get; set; }

        [ForeignKey(nameof(JurorId))]
        public virtual Juror Juror { get; set; }

        [ForeignKey(nameof(SpecialityId))]
        public virtual NomSpeciality Speciality { get; set; }

    }
}
