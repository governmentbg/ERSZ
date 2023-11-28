using ERSZ.Infrastructure.Data.Models.Common;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ERSZ.Infrastructure.Data.Models.Base
{
    public class BaseActivity
    {
        [Key]
        public int Id { get; set; }

        public int CourtId { get; set; }

        public string Gid { get; set; }

        public DateTime DateCreated { get; set; }
        public DateTime? DateModified { get; set; }

        [ForeignKey(nameof(CourtId))]
        public virtual CommonCourt Court { get; set; }
    }
}
