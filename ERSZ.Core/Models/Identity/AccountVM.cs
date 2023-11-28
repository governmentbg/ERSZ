using ERSZ.Infrastructure;
using ERSZ.Infrastructure.Constants;
using ERSZ.Infrastructure.Contracts;
using ERSZ.Infrastructure.ViewModels.Common;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace ERSZ.Core.Models.Identity
{
    public class AccountVM
    {
        public string Id { get; set; }

        [Display(Name = "ЕГН")]
        [Required(ErrorMessage = "Въведете {0}.")]
        [AddToLog]
        [AutoSanitize]
        public string UIC { get; set; }

        [Display(Name = "Имена")]
        [Required(ErrorMessage = "Въведете {0}.")]
        [AddToLog]
        [AutoSanitize]
        public string FullName { get; set; }

        [Display(Name = "Електронна поща")]
        [Required(ErrorMessage = "Въведете {0}.")]
        [RegularExpression(NomenclatureConstants.EmailRegexPattern, ErrorMessage = "Невалидна електронна поща")]
        [AddToLog]
        [AutoSanitize]
        public string Email { get; set; }

        [Display(Name = "Съд")]
        [AddToLog]
        public int? CourtId { get; set; }

        public string CourtName { get; set; }

        [Display(Name = "От дата")]
        [Required(ErrorMessage = "Въведете {0}.")]
        [AddToLog]
        public DateTime DateFrom { get; set; }

        [Display(Name = "До дата")]
        [AddToLog]
        public DateTime? DateTo { get; set; }

        public bool IsActive { get; set; }

        [Display(Name = "Роли")]
        public IList<CheckListVM> Roles { get; set; }

        public AccountVM()
        {
            Roles = new List<CheckListVM>();
        }
    }
}
