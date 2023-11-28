using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ERSZ.Infrastructure.Data.Models.Nomenclatures
{
    /// <summary>
    /// Вид съд
    /// </summary>
    [Display(Name = "Вид съд")]
    public class NomCourtType : BaseCommonNomenclature
    {
        public string CssClass { get; set; }
    }
}
