using ERSZ.Infrastructure.Constants;
using ERSZ.Infrastructure.Data.Common;
using ERSZ.Infrastructure.Data.Models.Common;
using ERSZ.Infrastructure.Data.Models.Identity;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using System.Security.Claims;
using System.Threading.Tasks;

namespace ERSZ.Extensions
{
    public class ApplicationClaimsPrincipalFactory : UserClaimsPrincipalFactory<ApplicationUser, ApplicationRole>
    {
        private readonly IRepository repo;
        public ApplicationClaimsPrincipalFactory(
            UserManager<ApplicationUser> userManager,
            RoleManager<ApplicationRole> roleManager,
            IOptions<IdentityOptions> options,
            IRepository _repo) : base(userManager, roleManager, options)
        {
            repo = _repo;
        }

        public async override Task<ClaimsPrincipal> CreateAsync(ApplicationUser user)
        {
            var principal = await base.CreateAsync(user);

            ((ClaimsIdentity)principal.Identity).AddClaim(new Claim(CustomClaimType.FullName, user.FullName));
            if (user.CourtId > 0)
            {
                ((ClaimsIdentity)principal.Identity).AddClaim(new Claim(CustomClaimType.CourtId, user.CourtId.Value.ToString()));
                var _court = repo.GetById<CommonCourt>(user.CourtId);
                ((ClaimsIdentity)principal.Identity).AddClaim(new Claim(CustomClaimType.CourtName, _court.Label));

            }
            return principal;
        }



    }
}
