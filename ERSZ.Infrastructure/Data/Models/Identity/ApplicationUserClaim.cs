using Microsoft.AspNetCore.Identity;

namespace ERSZ.Infrastructure.Data.Models.Identity
{
    public class ApplicationUserClaim : IdentityUserClaim<string>
    {
        public virtual ApplicationUser User { get; set; }
    }
}
