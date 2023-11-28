using ERSZ.Infrastructure.Data.Models.Nomenclatures;
using System.ComponentModel.DataAnnotations;

namespace ERSZ.Infrastructure.Data.Models.Nomenclatures
{
    /// <summary>
    /// Вид дело
    /// </summary>
    [Display(Name = "Вид дело")]
    public class NomCaseType: BaseCommonNomenclature
    {
        public string ShortLabel { get; set; }
    }
}
