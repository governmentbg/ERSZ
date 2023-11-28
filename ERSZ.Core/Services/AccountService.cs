using ERSZ.Core.Contracts;
using ERSZ.Core.Models.Identity;
using ERSZ.Infrastructure.Contracts;
using ERSZ.Infrastructure.Data.Common;
using ERSZ.Infrastructure.Data.Models.Identity;
using ERSZ.Infrastructure.ViewModels.Common;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace ERSZ.Core.Services
{
    public class AccountService : BaseService, IAccountService
    {
        private readonly IUserContext userContext;
        public AccountService(IRepository _repo, ILogger<AccountService> _logger, IUserContext _userContext)
        {
            repo = _repo;
            logger = _logger;
            userContext = _userContext;
        }

        public IQueryable<AccountVM> Select(string fullName)
        {
            Expression<Func<ApplicationUser, bool>> whereSearch = x => true;
            Expression<Func<ApplicationUser, bool>> whereCourts = x => true;
            if (userContext.CourtId > 0)
            {
                whereCourts = x => x.CourtId == userContext.CourtId;
            }

            return repo.AllReadonly<ApplicationUser>()
                            //.Where(x => EF.Functions.ILike(x.FullName, fullName.ToPaternSearch()))
                            .Where(whereSearch)
                            .Where(whereCourts)
                            .Select(x => new AccountVM
                            {
                                Id = x.Id,
                                Email = x.UserName,
                                FullName = x.FullName,
                                CourtName = (x.CourtId > 0) ? x.Court.Label : "Всички съдилища",
                                DateFrom = x.DateFrom,
                                DateTo = x.DateTo,
                                IsActive = x.DateTo == null ? true : x.DateTo >= DateTime.Now ? true : false,
                            }).AsQueryable();
        }



        public async Task<SaveResultVM> CheckUser(AccountVM model)
        {

            if (await repo.AllReadonly<ApplicationUser>()
                        .Where(x => x.UserName == model.Email && x.Id != model.Id)
                        .AnyAsync())
            {
                return new SaveResultVM(false, "Съществува потребител с това потребителско име.");
            }

            if (await repo.AllReadonly<ApplicationUser>()
                        .Where(x => x.UIC == model.UIC && x.Id != model.Id)
                        .AnyAsync())
            {
                return new SaveResultVM(false, "Съществува потребител с това ЕГН.");
            }
            //Ако е избрано Всички съдилища - да се записва null
            if (model.CourtId <= 0)
            {
                model.CourtId = null;
            }

            return new SaveResultVM(true);
        }

        public async Task<ApplicationUser> GetByUserName(string userName)
        {
            var dateTime = DateTime.Now;
            return await repo.All<ApplicationUser>()
                                      .Where(x => EF.Functions.ILike(x.UserName, userName))
                                      .Where(x => (x.DateTo ?? dateTime.AddYears(10)) >= dateTime)
                                      .FirstOrDefaultAsync();
        }

        public async Task<ApplicationUser> GetByUIC(string uic)
        {
            var dateTime = DateTime.Now;
            return await repo.All<ApplicationUser>()
                            .Where(x => x.UIC == uic)
                            .Where(x => (x.DateTo ?? dateTime.AddYears(10)) >= dateTime)
                            .FirstOrDefaultAsync();
        }
    }
}
