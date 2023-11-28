using ERSZ.Infrastructure.Data.Models.Nomenclatures;
using System.ComponentModel.DataAnnotations;

namespace ERSZ.Infrastructure.Data.Models.Nomenclatures
{
    /// <summary>
    /// Видове прикачени документи
    /// </summary>
    [Display(Name = "Видове прикачени документи")]
    public class NomFileType : BaseCommonNomenclature
    {
        public int SourceType { get; set; }
    }
}
