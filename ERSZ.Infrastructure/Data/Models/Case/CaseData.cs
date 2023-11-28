using ERSZ.Infrastructure.Data.Models.Base;
using ERSZ.Infrastructure.Data.Models.Nomenclatures;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace ERSZ.Infrastructure.Data.Models.Case
{
    public class CaseData : BaseActivity
    {
        public int CaseTypeId { get; set; }

        /// <summary>
        /// 14-цифрен номер
        /// </summary>
        public string RegNumber { get; set; }

        public string ShortNumber { get; set; }
        public int RegYear { get; set; }

        public bool? IsFinished { get; set; }

        [ForeignKey(nameof(CaseTypeId))]
        public virtual NomCaseType CaseType { get; set; }

        public virtual ICollection<CaseSession> Sessions { get; set; }
        public virtual ICollection<CaseSelectionProtokol> Protocols { get; set; }
        public virtual ICollection<CaseDismissal> Dismissals { get; set; }
        public virtual ICollection<CaseSessionAmount> Amounts { get; set; }

        public CaseData()
        {
            Sessions = new HashSet<CaseSession>();
            Protocols = new HashSet<CaseSelectionProtokol>();
            Dismissals = new HashSet<CaseDismissal>();
            Amounts = new HashSet<CaseSessionAmount>();
        }
    }
}
