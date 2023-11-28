using ERSZ.Infrastructure.Data.Models.Base;
using ERSZ.Infrastructure.Data.Models.Nomenclatures;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ERSZ.Infrastructure.Data.Models.Case
{
    /// <summary>
    /// Заседания
    /// </summary>
    [Display(Name = "Заседания")]
    public class CaseSession : BaseActivity
    {
        public int CaseId { get; set; }
        public string SessionKind { get; set; }
        public string State { get; set; }
        public string Result { get; set; }
        public string ResultBase { get; set; }
        public DateTime DateStart { get; set; }
        public DateTime DateEnd { get; set; }

        [ForeignKey(nameof(CaseId))]
        public virtual CaseData Case { get; set; }

        public virtual ICollection<CaseSessionAct> Acts { get; set; }
        public virtual ICollection<CaseSessionAmount> Amounts { get; set; }

        public CaseSession()
        {
            Acts = new HashSet<CaseSessionAct>();
            Amounts = new HashSet<CaseSessionAmount>();
        }

    }
}
