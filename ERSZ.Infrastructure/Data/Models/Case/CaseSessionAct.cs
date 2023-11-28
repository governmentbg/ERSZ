using ERSZ.Infrastructure.Data.Models.Base;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ERSZ.Infrastructure.Data.Models.Case
{
    /// <summary>
    /// Актове
    /// </summary>
    [Display(Name = "Актове")]
    public class CaseSessionAct : BaseActivity
    {
        public int CaseSesionId { get; set; }
        public string ActKind { get; set; }
        public string RegNumber { get; set; }
        public DateTime RegDate { get; set; }

        [ForeignKey(nameof(CaseSesionId))]
        public virtual CaseSession CaseSesion { get; set; }
    }
}
