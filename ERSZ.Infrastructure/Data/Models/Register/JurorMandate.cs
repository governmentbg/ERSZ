using ERSZ.Infrastructure.Data.Models.Base;
using ERSZ.Infrastructure.Data.Models.Nomenclatures;
using ERSZ.Infrastructure.Data.Models.Common;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using ERSZ.Infrastructure.Data.Models.Ekatte;

namespace ERSZ.Infrastructure.Data.Models.Register
{
    /// <summary>
    /// Мандати на заседател
    /// </summary>
    [Display(Name = "Мандати на заседател")]
    public class JurorMandate : UserWrtModel
    {
        [Key]
        public int Id { get; set; }

        public string JurorId { get; set; }

        /// <summary>
        /// Съд, от общото събрание на който е избран
        /// </summary>
        [Display(Name = "")]
        public int? CourtId { get; set; }

        /// <summary>
        /// Съд, към който е регистриран
        /// </summary>
        [Display(Name = "")]
        public int? RegisterCourtId { get; set; }

        public int? MunicipalityId { get; set; }

        public int MandateTypeId { get; set; }

        public int? ParentId { get; set; }

        public string MandateNo { get; set; }

        public DateTime DateStart { get; set; }
        public DateTime? DateEnd { get; set; }

        public DateTime? DateTermination { get; set; }

        public string DateTerminationDescription { get; set; }

        [ForeignKey(nameof(JurorId))]
        public virtual Juror Juror { get; set; }

        [ForeignKey(nameof(CourtId))]
        public virtual CommonCourt Court { get; set; }

        [ForeignKey(nameof(RegisterCourtId))]
        public virtual CommonCourt RegisterCourt { get; set; }

        [ForeignKey(nameof(MunicipalityId))]
        public virtual EkMunincipality Munincipality { get; set; }

        [ForeignKey(nameof(MandateTypeId))]
        public virtual NomMandateType MandateType { get; set; }

        [ForeignKey(nameof(ParentId))]
        public virtual JurorMandate MandateParent { get; set; }
    }
}
