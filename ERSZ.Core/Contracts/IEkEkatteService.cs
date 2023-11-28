using ERSZ.Infrastructure.Data.Models.Ekatte;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ERSZ.Core.Contracts
{
    public interface IEkEkatteService:IBaseService
    {
        IQueryable<EkEkatte> EkEkatteSelect();
        List<SelectListItem> GetDropDownList(bool addDefaultElement = true, bool addAllElement = false);
    }
}
