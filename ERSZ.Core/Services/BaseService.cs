using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using ERSZ.Core.Contracts;
using ERSZ.Infrastructure.Data.Common;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using ERSZ.Infrastructure.ViewModels.Common;
using ERSZ.Infrastructure.Contracts;

namespace ERSZ.Core.Services
{
    public class BaseService : IBaseService
    {
        protected IRepository repo;
        protected ILogger<BaseService> logger;
        protected IUserContext userContext { get; set; }

        public T GetById<T>(object id) where T : class
        {
            return repo.GetById<T>(id);
        }

        class LogOperItemModel
        {
            [JsonProperty("k")]
            public string Key { get; set; }
            [JsonProperty("v")]
            public string Value { get; set; }
        }

        public string GetValuesFromObject<T>(T obj, Microsoft.AspNetCore.Mvc.ViewFeatures.ViewDataDictionary viewData, params Expression<Func<T, object>>[] props) where T : new()
        {
            return JsonConvert.SerializeObject(getKeyValuesFromObject(obj, viewData, props));
        }

        LogOperItemModel[] getKeyValuesFromObject<T>(T obj, Microsoft.AspNetCore.Mvc.ViewFeatures.ViewDataDictionary viewData, params Expression<Func<T, object>>[] props) where T : new()
        {
            var result = new List<LogOperItemModel>();
            foreach (var propLambda in props)
            {
                PropertyInfo pInfo = GetPropertyInfo(obj, propLambda);
                var displayName = pInfo.GetCustomAttribute<DisplayAttribute>()?.Name ?? pInfo.Name;
                var value = pInfo.GetValue(obj)?.ToString();
                if (viewData != null && viewData[$"{pInfo.Name}_ddl"] != null)
                {
                    var ddls = (List<SelectListItem>)viewData[$"{pInfo.Name}_ddl"];
                    value = ddls.Where(x => x.Value == value).Select(x => x.Text).FirstOrDefault();
                }
                var propType = pInfo.PropertyType.Name;
                if (propType.Contains("boolean", StringComparison.InvariantCultureIgnoreCase))
                {
                    if (pInfo.GetValue(obj) != null)
                    {
                        if ((bool)pInfo.GetValue(obj))
                        {
                            value = "[Да]";
                        }
                        else
                        {
                            value = "[не]";
                        }
                    }
                    else
                    {
                        value = "[ ]";
                    }
                }
                if (propType.Contains("datetime", StringComparison.InvariantCultureIgnoreCase))
                {
                    if (pInfo.GetValue(obj) != null)
                    {
                        value = ((DateTime)pInfo.GetValue(obj)).ToString("dd.MM.yyyy");
                    }
                    else
                    {
                        value = "";
                    }
                }
                result.Add(new LogOperItemModel()
                {
                    Key = displayName,
                    Value = value
                });
            }
            return result.ToArray();
        }

        private PropertyInfo GetPropertyInfo<TSource, TProperty>(TSource source, Expression<Func<TSource, TProperty>> propertyLambda)
        {
            Type type = typeof(TSource);

            MemberExpression member = propertyLambda.Body as MemberExpression;

            if (member == null)
            {
                var expressionBody = propertyLambda.Body;
                if (expressionBody is UnaryExpression expression && expression.NodeType == ExpressionType.Convert)
                {
                    expressionBody = expression.Operand;
                }
                member = (MemberExpression)expressionBody;
            }

            PropertyInfo propInfo = member.Member as PropertyInfo;
            if (propInfo == null)
                throw new ArgumentException(string.Format(
                    "Expression '{0}' refers to a field, not a property.",
                    propertyLambda.ToString()));

            if (type != propInfo.ReflectedType &&
                !type.IsSubclassOf(propInfo.ReflectedType))
                throw new ArgumentException(string.Format(
                    "Expression '{0}' refers to a property that is not from type {1}.",
                    propertyLambda.ToString(),
                    type));

            return propInfo;
        }

        bool IBaseService.SaveExpireInfo<T>(ExpiredInfoVM model)
        {
            T saved;
            if (model.LongId > 0)
            {
                saved = repo.GetById<T>(model.LongId);
            }
            else
            {
                saved = repo.GetById<T>(model.Id);
            }

            if (saved != null)
            {
                saved.DateExpired = DateTime.Now;
                saved.ExpiredUserId = userContext.UserId;
                saved.ExpiredDescription = model.ExpiredDescription;
                repo.Update(saved);
                repo.SaveChanges();
                return true;
            }
            else
            {
                return false;
            }
        }
    }
}
