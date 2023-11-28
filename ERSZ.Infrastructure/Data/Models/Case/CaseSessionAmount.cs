using ERSZ.Infrastructure.Data.Models.Base;
using ERSZ.Infrastructure.Data.Models.Register;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ERSZ.Infrastructure.Data.Models.Case
{
    /// <summary>
    /// Суми
    /// </summary>
    [Display(Name = "Суми")]
    public class CaseSessionAmount : BaseActivity
    {
        public int JurorMandateId { get; set; }
        public int CaseId { get; set; }
        public int CaseSesionId { get; set; }
        public decimal Fee { get; set; }
        public decimal Expences { get; set; }
        public decimal Fine { get; set; }
        public bool FineIsPaid { get; set; }

        [ForeignKey(nameof(JurorMandateId))]
        public JurorMandate JurorMandate { get; set; }

        [ForeignKey(nameof(CaseId))]
        public virtual CaseData Case { get; set; }

        [ForeignKey(nameof(CaseSesionId))]
        public virtual CaseSession CaseSesion { get; set; }
    }
}
