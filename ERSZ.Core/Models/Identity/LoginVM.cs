using Microsoft.AspNetCore.Authentication;
using System.Collections.Generic;

namespace ERSZ.Core.Models.Identity
{
    public class LoginVM
    {
        public IList<AuthenticationScheme> ExternalLogins { get; set; }

        public string ReturnUrl { get; set; }
    }
}
