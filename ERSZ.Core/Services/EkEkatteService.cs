using ERSZ.Core.Contracts;
using ERSZ.Infrastructure.Data.Common;
using ERSZ.Infrastructure.Data.Models.Ekatte;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ERSZ.Core.Services
{
    public class EkEkatteService:BaseService,IEkEkatteService
    {
        public EkEkatteService(
         ILogger<EkEkatteService> _logger,
         IRepository _repo)
        {
            logger = _logger;
            repo = _repo;
        }
        public IQueryable<EkEkatte> EkEkatteSelect()
        {
            return repo.AllReadonly<EkEkatte>()
                .Select(x => x)
                .AsQueryable();
        }

        public List<SelectListItem> GetDropDownList(bool addDefaultElement = true, bool addAllElement = false)
        {
            var result = repo.AllReadonly<EkEkatte>()
                        .OrderBy(x => x.Name)
                        .Select(x => new SelectListItem()
                        {
                            Text = x.Name,
                            Value = x.Id.ToString()
                        }).ToList() ?? new List<SelectListItem>();

            if (addDefaultElement)
            {
                result = result
                    .Prepend(new SelectListItem() { Text = "Избери", Value = "-1" })
                    .ToList();
            }

            if (addAllElement)
            {
                result = result
                    .Prepend(new SelectListItem() { Text = "Всички", Value = "-2" })
                    .ToList();
            }
            return result;
        }
    }
}
