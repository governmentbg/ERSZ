using ERSZ.Infrastructure.Data.Models.Base;
using ERSZ.Infrastructure.Data.Models.Register;
using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace ERSZ.Infrastructure.Data.Models.Case
{
    public class CaseSelectionProtokol : BaseActivity
    {
        public int CaseId { get; set; }

        public int JurorMandateId { get; set; }

        public DateTime SelectionDate { get; set; }

        [ForeignKey(nameof(CaseId))]
        public virtual CaseData Case { get; set; }

        [ForeignKey(nameof(JurorMandateId))]
        public JurorMandate JurorMandate { get; set; }
    }
}