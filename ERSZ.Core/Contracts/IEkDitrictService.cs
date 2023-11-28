using ERSZ.Core.Models;
using ERSZ.Infrastructure.Data.Models.Ekatte;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ERSZ.Core.Contracts
{
    public interface IEkDitrictService: IBaseService
    {
        IQueryable<EkDistrict> EkDistrictSelect();
        EkDistrict GetEkDistrictByMapId(string mapId);
        Task<List<CourtTypeModel>> AllDistrictCourtsInformationByMapId(string mapId="");
        Task<List<CourtTypeModel>> AllDistrictCourtsInformation();
        List<SelectListItem> GetDropDownList(bool addDefaultElement = true, bool addAllElement = false);

        List<SelectListItem> GetMunicipalityDropDownList(bool addDefaultElement = true, bool addAllElement = false);
    }
}
