using ERSZ.Infrastructure.Contracts;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace ERSZ.Infrastructure.ViewModels.Common
{
    public class ExpiredInfoVM : IExpiredInfo
    {
        public int Id { get; set; }
        public long LongId { get; set; }
        public string StringId { get; set; }
        public string FileContainerName { get; set; }
        public DateTime? DateExpired { get; set; }
        public string ExpiredUserId { get; set; }
        
        [Display(Name = "Причина за премахването")]
        [Required(ErrorMessage = "Въведете {0}.")]
        public string ExpiredDescription { get; set; }

        public string ExpireSubmitUrl { get; set; }
        public string DialogTitle { get; set; }
        public string ReturnUrl { get; set; }
        public string SourceType { get; set; }
        public string SourceId { get; set; }
    }
}
