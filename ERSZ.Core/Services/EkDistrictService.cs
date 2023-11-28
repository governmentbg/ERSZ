using ERSZ.Core.Contracts;
using ERSZ.Core.Models;
using ERSZ.Infrastructure.Constants;
using ERSZ.Infrastructure.Contracts;
using ERSZ.Infrastructure.Data.Common;
using ERSZ.Infrastructure.Data.Models.Common;
using ERSZ.Infrastructure.Data.Models.Ekatte;
using ERSZ.Infrastructure.Data.Models.Register;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace ERSZ.Core.Services
{
    public class EkDistrictService : BaseService, IEkDitrictService
    {
        public EkDistrictService(
           ILogger<EkDistrictService> _logger,
           IRepository _repo)
        {
            logger = _logger;
            repo = _repo;
        }
        public IQueryable<EkDistrict> EkDistrictSelect()
        {
            return repo.AllReadonly<EkDistrict>()
                .Select(x => x)
                .AsQueryable();
        }

        public async Task<List<CourtTypeModel>> AllDistrictCourtsInformationByMapId(string mapId = "")
        {

            try
            {
                var courtsList = await repo.AllReadonly<CommonCourtEkatte>()
                    .Include(x => x.Ekatte)
                    .ThenInclude(y => y.District)
                    .Where(x => x.Ekatte.District.MapId == (mapId ?? ""))
                    .Select(x => x.CourtId)
                    .ToListAsync();

                var courtTypeInformationList = await repo.AllReadonly<JurorMandate>()
                                                         .Include(x => x.Court)
                                                         .ThenInclude(y => y.CourtType)
                                                         .Where(x => courtsList.Contains(x.CourtId ?? 0))
                                                         .Where(x => x.DateStart <= DateTime.Now)
                                                         .Where(x => (x.DateEnd ?? DateTime.Now) >= DateTime.Now)
                                                         .Where(x => x.MandateTypeId == JurorConstants.Mandate.MandateType)
                                                         .GroupBy(x => new { x.CourtId, x.Court.ShortLabel, x.Court.CourtType.CssClass })
                                                         .Select(x => new CourtTypeModel()
                                                         {
                                                             CourtId = x.Key.CourtId ?? 0,
                                                             CourtLabel = x.Key.ShortLabel,
                                                             JurorCount = x.Count(),
                                                             CssClass = x.Key.CssClass
                                                         })
                                                         .ToListAsync();

                if (courtTypeInformationList == null)
                {
                    courtTypeInformationList = new List<CourtTypeModel>();
                }

                return courtTypeInformationList;
            }
            catch (Exception ex)
            {
                logger.LogError(ex.Message, ex);
                return new List<CourtTypeModel>(); ;
            }
        }
        public List<SelectListItem> GetDropDownList(bool addDefaultElement = true, bool addAllElement = false)
        {
            var result = repo.AllReadonly<EkDistrict>()
                        .OrderBy(x => x.Name)
                        .Select(x => new SelectListItem()
                        {
                            Text = x.Name,
                            Value = x.DistrictId.ToString()
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

        public EkDistrict GetEkDistrictByMapId(string mapId = "")
        {
            return EkDistrictSelect()
                .Where(x => x.MapId == (mapId ?? ""))
                .FirstOrDefault();
        }

        public async Task<List<CourtTypeModel>> AllDistrictCourtsInformation()
        {

            var courtTypeInformationList = await repo.AllReadonly<JurorMandate>()
                    .Include(x => x.Court)
                    .ThenInclude(y => y.CourtType)
                    .Where(x => CourtConstants.CourtType.CourtInSelected.Contains(x.Court.CourtTypeId))
                    .Where(x => x.DateStart <= DateTime.Now)
                    .Where(x => (x.DateEnd ?? DateTime.Now) >= DateTime.Now)
                    .Where(x => x.MandateTypeId == JurorConstants.Mandate.MandateType)
                    .Distinct()
                    .GroupBy(x => new { x.Court.CourtType.Label, x.Court.CourtType.CssClass, x.Court.CourtTypeId })
                    .Select(x => new CourtTypeModel()
                    {
                        CourtTypeId = x.Key.CourtTypeId,
                        CourtLabel = x.Key.Label,
                        JurorCount = x.Count(),
                        CssClass = x.Key.CssClass
                    })
                    .ToListAsync();

            if (courtTypeInformationList == null)
            {
                courtTypeInformationList = new List<CourtTypeModel>();
            }

            return courtTypeInformationList;
        }

        public List<SelectListItem> GetMunicipalityDropDownList(bool addDefaultElement = true, bool addAllElement = false)
        {
            var result = repo.AllReadonly<EkMunincipality>()
                         .OrderBy(x => x.Name)
                         .Select(x => new SelectListItem()
                         {
                             Text = x.Name,
                             Value = x.MunicipalityId.ToString()
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
