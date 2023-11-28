﻿using Microsoft.AspNetCore.Identity;

namespace ERSZ.Infrastructure.Data.Models.Identity
{
    public class ApplicationUserLogin : IdentityUserLogin<string>
    {
        public virtual ApplicationUser User { get; set; }
    }
}
