﻿using ERSZ.Infrastructure.Data.Models.Common;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace ERSZ.Infrastructure.Data.Models.Identity
{
    public class ApplicationUser : IdentityUser
    {
        public virtual ICollection<ApplicationUserClaim> Claims { get; set; }
        public virtual ICollection<ApplicationUserLogin> Logins { get; set; }
        public virtual ICollection<ApplicationUserToken> Tokens { get; set; }
        public virtual ICollection<ApplicationUserRole> UserRoles { get; set; }
        public bool MustChangePassword { get; set; }
        public string UIC { get; set; }
        public string FullName { get; set; }
        public int? CourtId { get; set; }
        public bool IsActive { get; set; }

        public DateTime DateFrom { get; set; }

        public DateTime? DateTo { get; set; }
        
        public virtual CommonCourt Court { get; set; }
    }
}
