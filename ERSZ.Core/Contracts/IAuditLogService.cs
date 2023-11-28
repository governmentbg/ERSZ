using ERSZ.Core.Models;
using System.Linq;
using System.Threading.Tasks;

namespace ERSZ.Core.Contracts
{
    public interface IAuditLogService
    {
        Task<bool> SaveAuditLog(string operation, string objectInfo, string clientIp, string requestUrl, string actionInfo = null);
        IQueryable<AuditLogVM> Select(AuditLogFilterVM filter);
    }
}
