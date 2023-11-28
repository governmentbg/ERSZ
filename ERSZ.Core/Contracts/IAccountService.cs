using ERSZ.Core.Models.Identity;
using ERSZ.Infrastructure.Data.Models.Identity;
using ERSZ.Infrastructure.ViewModels.Common;
using System.Linq;
using System.Threading.Tasks;

namespace ERSZ.Core.Contracts
{
    public interface IAccountService : IBaseService
    {
        Task<SaveResultVM> CheckUser(AccountVM model);
        Task<ApplicationUser> GetByUIC(string uic);
        Task<ApplicationUser> GetByUserName(string userName);
        IQueryable<AccountVM> Select(string fullName);
    }
}
