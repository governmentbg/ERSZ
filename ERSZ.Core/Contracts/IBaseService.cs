using ERSZ.Infrastructure.Contracts;
using ERSZ.Infrastructure.Data.Models.Base;
using ERSZ.Infrastructure.ViewModels.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace ERSZ.Core.Contracts
{
    public interface IBaseService
    {
        T GetById<T>(object id) where T : class;
        string GetValuesFromObject<T>(T obj, Microsoft.AspNetCore.Mvc.ViewFeatures.ViewDataDictionary viewData, params Expression<Func<T, object>>[] props) where T : new();
        bool SaveExpireInfo<T>(ExpiredInfoVM model) where T : class, IExpiredInfo;
    }
}
