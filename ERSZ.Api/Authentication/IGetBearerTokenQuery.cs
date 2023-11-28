using ERSZ.Infrastructure.Data.Models.Common;
using System.Threading.Tasks;

namespace ERSZ.Api.Authentication
{
    /// <summary>
    /// Достъп до информация на потребителя по Bearer Token
    /// </summary>
    public interface IGetBearerTokenQuery
    {
        /// <summary>
        /// Извлича информация за потребителя по token
        /// </summary>
        /// <param name="token">Идентификационен token</param>
        /// <returns></returns>
        Task<ApiKeyModel> GetDataByToken(string token);
    }
}
