using ERSZ.Infrastructure.Constants;
using ERSZ.Infrastructure.Contracts;
using ERSZ.Infrastructure.Data.Models.Identity;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using System;
using System.Linq;
using System.Security.Claims;

namespace ERSZ.Infrastructure.Data.Models.UserContext
{
    public class UserContext : IUserContext
    {
        private ClaimsPrincipal User;

        public UserContext(IHttpContextAccessor _ca)
        {
            User = _ca.HttpContext.User;
        }


        public string UserId
        {
            get
            {
                string userId = String.Empty;

                if (User != null && User.Claims != null && User.Claims.Count() > 0)
                {
                    var subClaim = User.Claims
                        .FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier);

                    if (subClaim != null)
                    {
                        userId = subClaim.Value;
                    }
                }

                return userId;
            }
        }
        public string Email
        {
            get
            {
                string email = String.Empty;

                if (User != null && User.Claims != null && User.Claims.Count() > 0)
                {

                    var claimEmail = User.Claims
                        .FirstOrDefault(c => c.Type == ClaimTypes.Name);

                    email = claimEmail?.Value;
                }

                return email;
            }
        }
        public string LogName
        {
            get
            {
                string logName = String.Empty;


                logName = string.Format("{0} ({1})", this.FullName, this.Email);

                return logName;
            }
        }
        public string FullName
        {
            get
            {
                string fullName = String.Empty;

                if (User != null && User.Claims != null && User.Claims.Count() > 0)
                {
                    var subClaim = User.Claims
                        .FirstOrDefault(c => c.Type == CustomClaimType.FullName);

                    if (subClaim != null)
                    {
                        fullName = subClaim.Value;
                    }
                }

                return fullName;
            }
        }

        public int? CourtId
        {
            get
            {
                int? result = null;

                if (User != null && User.Claims != null && User.Claims.Count() > 0)
                {
                    var subClaim = User.Claims
                        .FirstOrDefault(c => c.Type == CustomClaimType.CourtId);

                    if (subClaim != null)
                    {
                        result = int.Parse(subClaim.Value);
                    }
                }

                return result;
            }
        }

        public string CourtName
        {
            get
            {
                string courtName = String.Empty;

                if (User != null && User.Claims != null && User.Claims.Count() > 0)
                {
                    var subClaim = User.Claims
                        .FirstOrDefault(c => c.Type == CustomClaimType.CourtName);

                    if (subClaim != null)
                    {
                        courtName = subClaim.Value;
                    }
                }

                return courtName;
            }
        }

        public bool IsUserInRole(string role)
        {
            switch (role)
            {
                case NomenclatureConstants.Roles.Admin:
                case NomenclatureConstants.Roles.Report:
                    return User.IsInRole(role) || User.IsInRole(NomenclatureConstants.Roles.GlobalAdmin);
                default:
                    return User.IsInRole(role);
            }

        }

    }
}

