using ERSZ.Infrastructure.Data.Models.Base;
using ERSZ.Infrastructure.Data.Models.Register;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;


namespace ERSZ.Infrastructure.Data.Models.Case
{
    /// <summary>
    /// Отвод/самоотвод на заседател
    /// </summary>
    [Display(Name = "Отвод/самоотвод на заседател")]
    public class CaseDismissal : BaseActivity
    {
        public int CaseId { get; set; }
        public string JurorId { get; set; }
        public string DismissalKind { get; set; }
        public string Reason { get; set; }
        public DateTime DismissalDate { get; set; }

        [ForeignKey(nameof(JurorId))]
        public virtual Juror Juror { get; set; }

        [ForeignKey(nameof(CaseId))]
        public virtual CaseData Case { get; set; }
    }
}
