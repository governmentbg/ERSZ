using ERSZ.Infrastructure.Data.Models.Nomenclatures;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ERSZ.Infrastructure.Data.Models.Nomenclatures
{
    /// <summary>
    /// Образователно-квалификационна степен
    /// </summary>
    [Display(Name = "Образователно-квалификационна степен")]
    public class NomEducationRang : BaseCommonNomenclature
    {
        public int? EducationId { get; set; }

        [ForeignKey(nameof(EducationId))]
        public virtual NomEducation Education { get; set; }
    }
}
