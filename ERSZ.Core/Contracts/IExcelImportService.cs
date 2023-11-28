using ERSZ.Infrastructure.ViewModels.Common;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using System.Threading.Tasks;

namespace ERSZ.Core.Contracts
{
    public interface IExcelImportService : IBaseService
    {
        Task<SaveResultVM> ProcessAsync(string fileName, byte[] fileContent, ViewDataDictionary viewData);
    }
}
