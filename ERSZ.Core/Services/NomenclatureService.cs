using ERSZ.Core.Contracts;
using ERSZ.Core.Extensions;
using ERSZ.Core.Models.Nomenclatures;
using ERSZ.Infrastructure.Constants;
using ERSZ.Infrastructure.Contracts;
using ERSZ.Infrastructure.Data.Common;
using ERSZ.Infrastructure.Data.Models.Base;
using ERSZ.Infrastructure.Data.Models.Common;
using ERSZ.Infrastructure.Data.Models.Ekatte;
using ERSZ.Infrastructure.Data.Models.Nomenclatures;
using ERSZ.Infrastructure.ViewModels.Common;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace ERSZ.Core.Services
{
    public class NomenclatureService : BaseService, INomenclatureService
    {
        public NomenclatureService(
            ILogger<NomenclatureService> _logger,
            IRepository _repo)
        {
            logger = _logger;
            repo = _repo;
        }

        public int GetEkatteIdByCode(string ekatte)
        {
            return repo.AllReadonly<EkEkatte>().Where(x => x.Ekatte == ekatte).Select(x => x.Id).FirstOrDefault();
        }

        public HierarchicalNomenclatureDisplayModel GetEkatte(string query)
        {
            var result = new HierarchicalNomenclatureDisplayModel();
            query = query?.ToLower();

            var ekatte = repo.AllReadonly<EkEkatte>()
                .Include(e => e.Munincipality)
                .Include(e => e.District)
                .Where(e => e.Name.ToLower().Contains(query ?? e.Name.ToLower()))
                .Select(e => new HierarchicalNomenclatureDisplayItem()
                {
                    Id = e.Ekatte,
                    Label = String.Format("{0} {1}", e.TVM, e.Name),
                    Category = String.Format("общ. {0}, обл. {1}", e.Munincipality.Name, e.District.Name)
                });

            result.Data.AddRange(ekatte);

            //var sobr = repo.AllReadonly<EkSobr>()
            //    .Where(s => s.Name.ToLower().Contains(query ?? s.Name.ToLower()))
            //    .Select(s => new HierarchicalNomenclatureDisplayItem()
            //    {
            //        Id = s.Ekatte,
            //        Label = s.Name,
            //        Category = s.Area1 != null ? s.Area1.Substring(s.Area1.IndexOf(')') + 1).Trim() : "Селищни образования"
            //    });

            //result.Data.AddRange(sobr);

            result.Data = result.Data
                .OrderBy(d => d.Category)
                .ToList();

            return result;
        }

        public HierarchicalNomenclatureDisplayItem GetEkatteById(string id)
        {
            var result = repo.AllReadonly<EkEkatte>()
                .Include(e => e.Munincipality)
                .Include(e => e.District)
                .Where(e => e.Ekatte == id)
                .Select(e => new HierarchicalNomenclatureDisplayItem()
                {
                    Id = e.Ekatte,
                    Label = String.Format("{0} {1}", e.TVM, e.Name),
                    Category = String.Format("общ. {0}, обл. {1}", e.Munincipality.Name, e.District.Name)
                })
                .FirstOrDefault();

            //if (result == null)
            //{
            //    result = repo.AllReadonly<EkSobr>()
            //    .Where(s => s.Ekatte == id)
            //    .Select(s => new HierarchicalNomenclatureDisplayItem()
            //    {
            //        Id = s.Ekatte,
            //        Label = s.Name,
            //        Category = s.Area1 != null ? s.Area1.Substring(s.Area1.IndexOf(')') + 1).Trim() : "Селищни образования"
            //    })
            //    .FirstOrDefault();
            //}

            return result;
        }

        public IEnumerable<LabelValueVM> GetStreet(string ekatte, string query)
        {
            query = query?.ToLower();
            return repo.AllReadonly<EkStreet>().Where(x => x.Ekatte == ekatte && x.Name.ToLower().Contains(query))
                        .OrderBy(x => x.Name)
                        .Select(x => new LabelValueVM
                        {
                            Value = x.Code,
                            Label = x.Name
                        });
        }

        public LabelValueVM GetStreetByCode(string ekatte, string code)
        {
            return repo.AllReadonly<EkStreet>().Where(x => x.Ekatte == ekatte && x.Code == code)
                        .OrderBy(x => x.Name)
                        .Select(x => new LabelValueVM
                        {
                            Value = x.Code,
                            Label = x.Name
                        }).FirstOrDefault();
        }

        public bool ChangeOrder<T>(ChangeOrderModel model) where T : class, ICommonNomenclature
        {
            bool result = false;

            try
            {
                var nomList = repo.All<T>()
                    .ToList();

                int maxOrderNumber = nomList
                    .Max(x => x.OrderNumber);
                int minOrderNumber = nomList
                    .Min(x => x.OrderNumber);
                var currentElement = nomList
                    .Where(x => x.Id == model.Id)
                    .FirstOrDefault();

                if (currentElement != null)
                {
                    if (model.Direction == "up" && currentElement.OrderNumber > minOrderNumber)
                    {
                        var previousElement = nomList
                            .Where(x => x.OrderNumber == currentElement.OrderNumber - 1)
                            .FirstOrDefault();

                        if (previousElement != null)
                        {
                            previousElement.OrderNumber = currentElement.OrderNumber;
                        }

                        currentElement.OrderNumber -= 1;
                    }

                    if (model.Direction == "down" && currentElement.OrderNumber < maxOrderNumber)
                    {
                        var nextElement = nomList
                            .Where(x => x.OrderNumber == currentElement.OrderNumber + 1)
                            .FirstOrDefault();

                        if (nextElement != null)
                        {
                            nextElement.OrderNumber = currentElement.OrderNumber;
                        }

                        currentElement.OrderNumber += 1;
                    }

                    repo.SaveChanges();
                }

                result = true;
            }
            catch (Exception ex)
            {
                logger.LogError("ChangeOrder", ex);
            }

            return result;
        }

        public List<SelectListItem> GetDropDownList<T>(bool addDefaultElement, bool addAllElement, bool orderByNumber) where T : class, ICommonNomenclature
        {
            var result = repo.AllReadonly<T>()
                        .ToSelectList(addDefaultElement, addAllElement, orderByNumber);

            return result;
        }

        public List<SelectListItem> GetDropDownListExpr<T>(Expression<Func<T, bool>> filterExpr, bool addDefaultElement = true, bool addAllElement = false, bool orderByNumber = true, bool appendCode = false) where T : class, ICommonNomenclature
        {
            var result = repo.AllReadonly<T>()
                             .Where(filterExpr)
                             .ToSelectList(addDefaultElement, addAllElement, orderByNumber, appendCode);

            return result;
        }

        public T GetItem<T>(int id) where T : class, ICommonNomenclature
        {
            var item = repo.GetById<T>(id);

            return item;
        }

        public IQueryable<CommonNomenclatureListItem> GetList<T>() where T : class, ICommonNomenclature
        {
            return repo.AllReadonly<T>()
                .Select(x => new CommonNomenclatureListItem()
                {
                    Id = x.Id,
                    Code = x.Code,
                    DateStart = x.DateStart,
                    DateEnd = x.DateEnd,
                    Description = x.Description,
                    IsActive = x.IsActive,
                    Label = x.Label,
                    OrderNumber = x.OrderNumber
                }).OrderBy(x => x.OrderNumber);
        }

        public IQueryable<CommonNomenclatureListItem> GetActiveList<T>() where T : class, ICommonNomenclature
        {
            DateTime now = DateTime.Today;
            return repo.AllReadonly<T>()
                .Where(n => n.IsActive && n.DateStart <= now)
                .Where(n => n.DateEnd == null || n.DateEnd >= now)
                .Select(x => new CommonNomenclatureListItem()
                {
                    Id = x.Id,
                    Code = x.Code,
                    DateStart = x.DateStart,
                    DateEnd = x.DateEnd,
                    Description = x.Description,
                    IsActive = x.IsActive,
                    Label = x.Label,
                    OrderNumber = x.OrderNumber
                }).OrderBy(x => x.OrderNumber);
        }

        public bool SaveItem<T>(T entity) where T : class, ICommonNomenclature
        {
            bool result = false;

            try
            {
                if (entity.Id > 0)
                {
                    repo.Update(entity);
                }
                else
                {
                    int maxOrderNumber = repo.AllReadonly<T>()
                        .Select(x => x.OrderNumber)
                        .OrderByDescending(x => x)
                        .FirstOrDefault();

                    entity.OrderNumber = maxOrderNumber + 1;
                    repo.Add(entity);
                }

                repo.SaveChanges();

                result = true;
            }
            catch (Exception ex)
            {
                logger.LogError(ex, $"Грешка при запис на номенклатура ({ typeof(T).ToString() })");
            }

            return result;
        }

        public async Task<int> GetNomIdByCode<T>(string code) where T : class, ICommonNomenclature
        {
            if (string.IsNullOrEmpty(code))
            {
                return -1;
            }
            code = code.Trim();
            var dtNow = DateTime.Now;
            return await repo.AllReadonly<T>()
                                .Where(x => x.Code == code)
                                .Where(x => x.IsActive == true)
                                //.Where(x => x.DateStart <= dtNow && (x.DateEnd ?? DateTime.MaxValue) >= dtNow)
                                .Select(x => x.Id)
                                //.DefaultIfEmpty(0)
                                .FirstOrDefaultAsync();
        }

        public async Task<ObjectIdVM> GetObjectId<T>(string code) where T : class, IObjectParentNomenclature
        {

            if (string.IsNullOrEmpty(code))
            {
                return new ObjectIdVM()
                {
                    IsFound = false
                };
            }

            code = code.Trim();
            var dtNow = DateTime.Now;
            return await repo.AllReadonly<T>()
                                .Where(x => x.Code == code)
                                .Where(x => x.IsActive == true)
                                //.Where(x => x.DateStart <= dtNow && (x.DateEnd ?? DateTime.MaxValue) >= dtNow)
                                .Select(x => new ObjectIdVM
                                {
                                    Id = x.Id,
                                    ObjectId = x.ObjectId,
                                    IsFound = true
                                })
                                .FirstOrDefaultAsync();
        }

        public async Task<List<CommonItemVM>> GetCommonItemList<T>(Expression<Func<T, bool>> addWhere = null) where T : class, ICommonNomenclature
        {
            Expression<Func<T, bool>> _addWhere = x => true;
            if (addWhere != null)
            {
                _addWhere = addWhere;

            }
            return await repo.AllReadonly<T>()
                                .Where(x => x.IsActive == true)
                                .Where(_addWhere)
                                .Select(x => new CommonItemVM
                                {
                                    Id = x.Id,
                                    Code = x.Code,
                                    Label = x.Label
                                }).ToListAsync();
        }

        public async Task<List<CommonItemVM>> GetCommonObjectItemList<T>(Expression<Func<T, bool>> addWhere = null) where T : class, IObjectParentNomenclature
        {
            Expression<Func<T, bool>> _addWhere = x => true;
            if (addWhere != null)
            {
                _addWhere = addWhere;

            }
            return await repo.AllReadonly<T>()
                                .Where(x => x.IsActive == true)
                                .Where(_addWhere)
                                .Select(x => new CommonItemVM
                                {
                                    Id = x.Id,
                                    ObjectId = x.ObjectId,
                                    Code = x.Code,
                                    Label = x.Label
                                }).ToListAsync();
        }

        public List<SelectListItem> GetCourtsByApealRegion(int apealRegionId, bool addDefaultElement = false, bool addAllElement = false, bool orderByNumber = true)
        {
            Expression<Func<CommonCourt, bool>> _addWhere = x => x.ApealRegionId == apealRegionId;
            if (apealRegionId == -2)
            {
                _addWhere = x => true;
            }
            return repo.AllReadonly<CommonCourt>()
                                    .Where(x => x.IsActive == true)
                                    .Where(_addWhere)
                                    .OrderBy(x => x.Label)
                                    .ToSelectList(addDefaultElement, addAllElement, orderByNumber);
        }


        public async Task<List<SelectListItem>> GetCourtsByDistrict(int districtId, bool addDefaultElement = false, bool addAllElement = false, bool orderByNumber = true)
        {

            Expression<Func<CommonCourt, bool>> _addWhere = x => true;

            if (districtId > 0)
            {
                var courtIds = await repo.AllReadonly<CommonCourtEkatte>()
                                                .Where(x => x.Ekatte.DistrictId == districtId)
                                                .Select(x => x.CourtId)
                                                .ToArrayAsync();

                _addWhere = x => courtIds.Contains(x.Id);
            }
            return repo.AllReadonly<CommonCourt>()
                                    .Where(x => x.IsActive == true)
                                    .Where(_addWhere)
                                    .OrderBy(x => x.Label)
                                    .ToSelectList(addDefaultElement, addAllElement, orderByNumber);
        }

        private void getChildCourts(IQueryable<CommonCourt> allData, List<CommonCourt> filterData, int? parentId, int[] courtTypes)
        {
            Expression<Func<CommonCourt, bool>> _addWhere = x => true;
            if (courtTypes.Any())
            {
                _addWhere = x => courtTypes.Contains(x.CourtTypeId);
            }
            var currentItems = allData.Where(x => x.ParentId == parentId).Where(_addWhere).ToList();
            foreach (var item in currentItems)
            {
                filterData.Add(item);
                getChildCourts(allData, filterData, item.Id, courtTypes);
            }

        }

        public List<SelectListItem> GetDDL_EkDistricts(bool addDefaultElement = false, bool addAllElement = false)
        {
            DateTime dtNow = DateTime.Now;
            return repo.AllReadonly<EkDistrict>()
                            .Select(x => new BaseCommonNomenclature
                            {
                                Id = x.DistrictId,
                                Label = x.Name,
                                IsActive = true,
                                OrderNumber = 0,
                                DateStart = DateTime.MinValue,
                                DateEnd = null
                            }).ToSelectList(addDefaultElement, addAllElement, false);
        }


        public bool ChangeOrder_IOrderable<T>(bool moveUp, int id, Expression<Func<T, bool>> where = null) where T : class, IOrderable
        {
            if (where == null)
            {
                where = x => true;
            }
            var current = repo.GetById<T>(id);
            T next;
            if (moveUp)
            {
                next = repo.All<T>().Where(where).Where(x => x.OrderNumber < current.OrderNumber).OrderByDescending(x => x.OrderNumber).FirstOrDefault();
            }
            else
            {
                next = repo.All<T>().Where(where).Where(x => x.OrderNumber > current.OrderNumber).OrderBy(x => x.OrderNumber).FirstOrDefault();
            }
            if (next != null)
            {
                int _tmp = current.OrderNumber;
                current.OrderNumber = next.OrderNumber;
                next.OrderNumber = _tmp;
                repo.SaveChanges();
                return true;
            }
            return false;
        }

        public int GetCounterValue(int counterId, int valueCount = 1)
        {
            var counter = repo.ExecuteProc<CounterValueVM>("public.get_counter_value({0},{1})", counterId, valueCount).FirstOrDefault();

            return counter.Value;

        }

        public List<SelectListItem> GetDDL_FinishedCase(bool addDefaultElement = true)
        {
            var selectListItems = new List<SelectListItem>();
            selectListItems.Add(new SelectListItem() { Text = NomenclatureConstants.IsFinishedText.DoneCase, Value = NomenclatureConstants.IsFinishedValue.DoneCase });
            selectListItems.Add(new SelectListItem() { Text = NomenclatureConstants.IsFinishedText.UnfinishedCase, Value = NomenclatureConstants.IsFinishedValue.UnfinishedCase });

            if (addDefaultElement)
            {
                selectListItems = selectListItems.Prepend(new SelectListItem() { Text = NomenclatureConstants.IsFinishedText.All, Value = NomenclatureConstants.IsFinishedValue.All }).ToList();
            }

            return selectListItems;
        }
    }
}
