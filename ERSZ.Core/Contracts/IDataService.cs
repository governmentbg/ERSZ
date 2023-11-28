using ERSZ.Infrastructure.Contracts.Data;
using ERSZ.Infrastructure.ViewModels.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ERSZ.Core.Contracts
{
    public interface IDataService
    {
        /// <summary>
        /// Записване на информация за дело
        /// </summary>
        /// <param name="model">ErszCaseModel</param>
        /// <returns></returns>
        Task<SaveResultVM> InsertCaseData(ErszCaseModel model);
        /// <summary>
        /// Записване на информация за отвод на дело
        /// </summary>
        /// <param name="model">ErszCaseDismissalModelparam>
        /// <returns></returns>
        Task<SaveResultVM> InsertCaseDismissal(ErszCaseDismissalModel model);
        /// <summary>
        /// Записване на информация за протокол на дело
        /// </summary>
        /// <param name="model">ErszCaseSelectionProtokolModel</param>
        /// <returns></returns>
        Task<SaveResultVM> InsertCaseSelectionProtokol(ErszCaseSelectionProtokolModel model);
        /// <summary>
        /// Записване на информация за заседания по делото
        /// </summary>
        /// <param name="model">ErszCaseSessionModel</param>
        /// <returns></returns>
        Task<SaveResultVM> InsertCaseSession(ErszCaseSessionModel model);
        /// <summary>
        /// Записване на информация за съдебни актове по делото
        /// </summary>
        /// <param name="model">ErszCaseSessionActModel</param>
        /// <returns></returns>
        Task<SaveResultVM> InsertCaseSessionAct(ErszCaseSessionActModel model);
        /// <summary>
        /// Записване на информация за суми по делото
        /// </summary>
        /// <param name="model">ErszCaseSessionAmountModel</param>
        /// <returns></returns>
        Task<SaveResultVM> InsertCaseSessionAmount(ErszCaseSessionAmountModel model);
        Task<SaveResultVM> UpdateCaseData(ErszCaseModel model);

        /// <summary>
        /// Промяна на информация за суми по делото
        /// </summary>
        /// <param name="model">ErszCaseSessionAmountModelparam>
        /// <returns></returns>
        Task<SaveResultVM> UpdateCaseSessionAmount(ErszCaseSessionAmountModel model);
      
    }
}
