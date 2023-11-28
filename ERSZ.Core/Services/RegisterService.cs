using ERSZ.Core.Constants;
using ERSZ.Core.Contracts;
using ERSZ.Core.Extensions;
using ERSZ.Infrastructure.Constants;
using ERSZ.Infrastructure.Contracts;
using ERSZ.Infrastructure.Data.Common;
using ERSZ.Infrastructure.Data.Models.Case;
using ERSZ.Infrastructure.Data.Models.Common;
using ERSZ.Infrastructure.Data.Models.Ekatte;
using ERSZ.Infrastructure.Data.Models.Nomenclatures;
using ERSZ.Infrastructure.Data.Models.Register;
using ERSZ.Infrastructure.Extensions;
using ERSZ.Infrastructure.ViewModels.Common;
using ERSZ.Infrastructure.ViewModels.Register;
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
    public class RegisterService : BaseService, IRegisterService
    {
        private readonly INomenclatureService nomService;
        private readonly IUserContext userContext;
        public RegisterService(IRepository _repo,
            ILogger<RegisterService> _logger,
            INomenclatureService _nomService,
            IUserContext _userContext)
        {
            repo = _repo;
            logger = _logger;
            nomService = _nomService;
            userContext = _userContext;
        }

        public JurorVM GetJurorById(string id)
        {
            var juror = repo.AllReadonly<Juror>()
                                .Include(x => x.Specialities)
                                .Where(x => x.Id == id)
                                .FirstOrDefault();

            if (juror != null)
                return MapVMToJuror(juror);

            return null;
        }

        public JurorVM InitJurorModel()
        {
            var model = new JurorVM()
            {
                DateStart = DateTime.Now
            };
            model.Specialities = repo.AllReadonly<NomSpeciality>().Where(x => x.IsActive).ToList().Select(x => new CheckListVM
            {
                Label = x.Label,
                Value = x.Id.ToString()
            }).ToList();
            return model;
        }

        public MandateVM GetMandateById(int id)
        {
            var mandate = repo.AllReadonly<JurorMandate>()
                              .Include(x => x.Juror)
                              .Include(x => x.MandateType)
                              .FirstOrDefault(x => x.Id == id);

            if (mandate != null)
                return MapVMToJMandate(mandate);

            return null;
        }

        public IQueryable<MandateListVM> Mandate_Select(FilterMandateVM model)
        {
            DateTime dateFromSearch = model.DateFrom == null ? DateTime.Now.AddYears(-100) : (DateTime)model.DateFrom;
            DateTime dateToSearch = model.DateTo == null ? DateTime.Now.AddYears(100) : (DateTime)model.DateTo;

            Expression<Func<JurorMandate, bool>> dateSearch = x => true;
            if (model.DateFrom != null || model.DateTo != null)
                dateSearch = x => x.DateStart.Date >= dateFromSearch.Date && (x.DateEnd ?? dateToSearch.Date) <= dateToSearch.Date;

            Expression<Func<JurorMandate, bool>> nameSearch = x => true;
            if (string.IsNullOrEmpty(model.JurorName) == false)
                nameSearch = x => EF.Functions.ILike(x.Juror.FullName, model.JurorName.ToPaternSearch());

            Expression<Func<JurorMandate, bool>> courtSearch = x => true;
            if (model.CourtId > 0)
            {
                courtSearch = x => x.CourtId == model.CourtId;
            }
            var municipalities = repo.AllReadonly<EkMunincipality>()
                    .ToList();
            Expression<Func<JurorMandate, bool>> municipalitySearch = x => true;
            if (model.MunicipalityId > 0)
                municipalitySearch = x => municipalities
                    .Any(m => m.MunicipalityId == model.MunicipalityId);


            Expression<Func<JurorMandate, bool>> mandateTypeSearch = x => true;
            if (model.MandateTypeId > 0)
                mandateTypeSearch = x => x.MandateTypeId == model.MandateTypeId;

            //check logic for include -> Specialities
            return repo.AllReadonly<JurorMandate>()
                       .Include(x => x.Juror)
                       .Include(x => x.Court)
                       .Include(x => x.MandateType)
                       .Where(dateSearch)
                       .Where(nameSearch)
                       .Where(courtSearch)
                       .Where(mandateTypeSearch)
                       .Where(municipalitySearch)
                       .Select(x => new MandateListVM()
                       {
                           Id = x.Id,
                           ParentDateFrom = x.ParentId == null ? x.DateStart : x.MandateParent.DateStart,
                           MandateTypeId = x.MandateTypeId,
                           MandateType = x.MandateType.Label,
                           DateFrom = x.DateStart,
                           DateTo = x.DateEnd,
                           CourtLabel = x.Court.Label,
                           JurorId = x.JurorId,
                           JurorFullName = x.Juror.FullName,
                       })
                       .AsQueryable();
        }

        public IQueryable<MandateListVM> Mandate_Select(string jurorId)
        {
            //check logic for include -> Specialities
            return repo.AllReadonly<JurorMandate>()
                       .Where(x => x.JurorId == jurorId)
                       .Select(x => new MandateListVM()
                       {
                           Id = x.Id,
                           MandateTypeId = x.MandateTypeId,
                           MandateType = x.MandateType.Label,
                           DateFrom = x.DateStart,
                           DateTo = x.DateEnd,
                           CourtLabel = x.Court.Label,
                           JurorId = x.JurorId,
                           DateTermination = x.DateTermination,
                           IsDateTermination = x.DateTermination != null,
                           ParentId = x.ParentId,
                           ParentDateFrom = x.ParentId == null ? x.DateStart : x.MandateParent.DateStart,
                           MandateNo = string.IsNullOrEmpty(x.MandateNo) ? 0 : int.Parse(x.MandateNo),
                           MandateTypeNo = x.MandateType.Label + (x.MandateTypeId == JurorConstants.Mandate.MandateType ? (string.IsNullOrEmpty(x.MandateNo) ? string.Empty : " номер " + int.Parse(x.MandateNo)) : string.Empty)
                       })
                       .OrderBy(x => x.ParentDateFrom)
                       .ThenBy(x => x.DateFrom)
                       .ThenBy(x => x.Id)
                       .AsQueryable();
        }
        public List<SelectListItem> GetJurorDropDownList(bool addDefaultElement = true, bool addAllElement = false, bool orderByNumber = true)
        {

            var result = repo.AllReadonly<Juror>()
               .Where(x => x.DateStart <= DateTime.Now)
               .Where(x => (x.DateEnd ?? DateTime.Now) >= DateTime.Now)
               .OrderBy(x => x.FullName)
               .Select(x => new SelectListItem()
               {
                   Text = x.FullName,
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

        public List<SelectListItem> GetCourtTypeDropDownList(bool addDefaultElement = true, bool addAllElement = false, bool orderByNumber = true)
        {

            var result = repo.AllReadonly<NomCourtType>()
               .Where(x => x.IsActive)
               .Where(x => CourtConstants.CourtType.CourtInSelected.Contains(x.Id))
               .Where(x => x.DateStart <= DateTime.Now)
               .Where(x => (x.DateEnd ?? DateTime.Now) >= DateTime.Now)
               .OrderBy(x => x.Label)
               .Select(x => new SelectListItem()
               {
                   Text = x.Label,
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

        public List<SelectListItem> GetCourtDropDownList(bool addDefaultElement = true, bool addAllElement = false, bool orderByNumber = true)
        {

            var result = repo.AllReadonly<CommonCourt>()
               .Where(x => x.IsActive)
               .Where(x => x.DateStart <= DateTime.Now)
               .Where(x => (x.DateEnd ?? DateTime.Now) >= DateTime.Now)
               .OrderBy(x => x.Label)
               .Select(x => new SelectListItem()
               {
                   Text = x.ShortLabel,
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
        public IQueryable<JurorListVM> Juror_Select(FilterJurorVM model)
        {
            DateTime dateFromSearch = model.DateFrom == null ? DateTime.Now.AddYears(-100) : (DateTime)model.DateFrom;
            DateTime dateToSearch = model.DateTo == null ? DateTime.Now.AddYears(100) : (DateTime)model.DateTo;
            DateTime dtNow = DateTime.Now.Date;

            Expression<Func<Juror, bool>> dateSearch = x => true;
            if (model.DateFrom != null || model.DateTo != null)
                dateSearch = x => x.Mandates.Any(m => m.DateStart >= dateFromSearch && (m.DateEnd ?? dateToSearch) <= dateToSearch.MakeEndDate());


            Expression<Func<Juror, bool>> courtSearch = x => true;
            if (model.CourtId > 0)
            {
                if (model.ActiveOnly > 0)
                {
                    courtSearch = x => x.Mandates.Any(m => m.DateStart < dtNow && (m.DateEnd ?? dateToSearch) > dtNow && m.CourtId == model.CourtId);

                }
                else
                {
                    courtSearch = x => x.Mandates.Any(m => m.CourtId == model.CourtId);
                }
            }
            else
            {
                if (model.CourtTypeId > 0)
                {
                    if (model.ActiveOnly > 0)
                    {
                        courtSearch = x => x.Mandates.Any(m => m.DateStart < dtNow && (m.DateEnd ?? dateToSearch) > dtNow && m.Court.CourtTypeId == model.CourtTypeId);

                    }
                    else
                    {
                        courtSearch = x => x.Mandates.Any(m => m.Court.CourtTypeId == model.CourtTypeId);
                    }
                }
            }

            Expression<Func<Juror, bool>> districtSearch = x => true;
            if (model.DistrictId > 0)
            {
                var courtIdsByDistrict = repo.AllReadonly<CommonCourtEkatte>()
                .Include(x => x.Ekatte)
                .Where(x => x.Ekatte.DistrictId == model.DistrictId)
                .Select(x => x.CourtId)
                .ToList();
                if (courtIdsByDistrict.Count() > 0)
                {
                    if (model.ActiveOnly > 0)
                    {
                        districtSearch = x => x.Mandates.Any(m => m.DateStart < dtNow && (m.DateEnd ?? dateToSearch) > dtNow && courtIdsByDistrict.Contains(m.CourtId ?? 0));
                    }
                    else
                    {
                        districtSearch = x => x.Mandates.Any(m => courtIdsByDistrict.Contains(m.CourtId ?? 0));
                    }
                }
            }

            Expression<Func<Juror, bool>> nameSearch = x => true;
            if (string.IsNullOrEmpty(model.FullName) == false)
                nameSearch = x => EF.Functions.ILike(x.FullName, model.FullName.ToPaternSearch());

            Expression<Func<Juror, bool>> wherePublicMode = x => true;
            if (!model.IsEdit)
            {
                wherePublicMode = x => x.Mandates.Any(m => (m.DateEnd ?? dateToSearch) >= dtNow);
            }

            Expression<Func<Juror, bool>> whereActiveOnly = x => true;
            if (model.ActiveOnly == 1)
            {
                whereActiveOnly = x => x.Mandates.Any(m => (m.DateEnd ?? dateToSearch) >= dtNow);
            }


            return repo.AllReadonly<Juror>()
                .Where(nameSearch)
                .Where(dateSearch)
                .Where(courtSearch)
                .Where(districtSearch)
                .Where(wherePublicMode)
                .Where(whereActiveOnly)
                .Select(x => new JurorListVM()
                {
                    Id = x.Id,
                    Specialities = x.Specialities
                                    .Select(y => y.Speciality.Label)
                                    .ToList(),
                    FullName = x.FullName,
                    IsActive = x.DateEnd == null ? true : (x.DateEnd > DateTime.Now ? true : false),
                    Content = x.Content,
                    Mandates = x.Mandates.Where(m => m.MandateTypeId == JurorConstants.Mandate.MandateType)
                                         .Where(m => model.ActiveOnly <= 0 || m.DateStart < dtNow && (m.DateEnd ?? dateToSearch) > dtNow)
                                         .OrderByDescending(m => m.DateStart)
                                         .Select(y => new MandateListVM()
                                         {
                                             CourtLabel = y.Court.Label,
                                             DateFrom = y.DateStart,
                                             DateTo = y.DateEnd,
                                             MandateType = y.MandateType.Label,
                                             IsActiveJuror = x.DateEnd == null ? true : (x.DateEnd > DateTime.Now ? true : false),
                                             BusinessTrip = x.Mandates.Where(m => m.MandateTypeId == JurorConstants.Mandate.MandateMissionType && m.ParentId == y.Id)
                                                                      .Where(m => model.ActiveOnly <= 0 || m.DateStart < dtNow && (m.DateEnd ?? dateToSearch) > dtNow)
                                                                      .OrderByDescending(m => m.DateStart)
                                                                      .Select(m => new MandateListVM()
                                                                      {
                                                                          CourtLabel = m.Court.Label,
                                                                          DateFrom = m.DateStart,
                                                                          DateTo = m.DateEnd,
                                                                          MandateType = m.MandateType.Label,
                                                                          IsActiveJuror = x.DateEnd == null ? true : (x.DateEnd > DateTime.Now ? true : false),
                                                                      })
                                                                      .ToList()
                                         })
                                         .ToList()
                })
                .OrderBy(x => x.FullName)
                .AsQueryable();
        }

        public bool IsExistsMandate(MandateVM mandateVM)
        {
            Expression<Func<JurorMandate, bool>> parentId = x => true;
            if (mandateVM.ParentId != null)
                parentId = x => x.ParentId == mandateVM.ParentId;

            Expression<Func<JurorMandate, bool>> courtCheckByMandateType = x =>
                (x.CourtId == mandateVM.CourtId);

            return repo.AllReadonly<JurorMandate>()
                       .Where(x => x.MandateTypeId == mandateVM.MandateTypeId)
                       .Where(x => x.JurorId == mandateVM.JurorId)
                       .Where(parentId)
                       .Where(x => ((mandateVM.DateEnd ?? DateTime.Now.AddYears(10)) >= x.DateStart))
                       .Where(x => (mandateVM.DateStart <= (x.DateEnd ?? DateTime.Now.AddYears(10))))
                       .Where(courtCheckByMandateType)
                       .Where(x => mandateVM.Id > 0 ? x.Id != mandateVM.Id : true)
                       .Any();
        }

        public async Task<SaveResultVM> Mandate_SaveData(MandateVM mandateVM)
        {
            try
            {
                var mandate = new JurorMandate();
                if (mandateVM.Id == 0)
                {
                    MapMandateToVM(mandate, mandateVM);
                    await repo.AddAsync(mandate);
                }
                else
                {
                    //edit
                    mandate = repo.All<JurorMandate>().FirstOrDefault(x => x.Id.Equals(mandateVM.Id));
                    MapMandateToVM(mandate, mandateVM);
                }

                await repo.SaveChangesAsync();
                mandateVM.Id = mandate.Id;
                return new SaveResultVM(true);
            }
            catch (Exception ex)
            {
                logger.LogError(ex, ex.Message);
                return new SaveResultVM()
                {
                    IsSuccessfull = false,
                    ErrorMessage = MessageConstant.Values.SaveFailed
                };
            }
        }

        public async Task<SaveResultVM> Juror_SaveData(JurorVM jurorVM)
        {
            try
            {

                if (IsUicFound(jurorVM))
                {
                    return new SaveResultVM()
                    {
                        IsSuccessfull = false,
                        ErrorMessage = JurorConstants.Message.UicExist
                    };
                }

                var juror = new Juror();

                if (jurorVM.Id == null)
                {
                    //add
                    MapJurorToVM(juror, jurorVM);

                    juror.RegNumber = generateRegisterNumber();

                    await repo.AddAsync(juror);
                }
                else
                {
                    //edit
                    juror = repo.GetById<Juror>(jurorVM.Id);

                    MapJurorToVM(juror, jurorVM);
                    await UpdateJurorSpecialities(juror, jurorVM);
                }

                await repo.SaveChangesAsync();
                jurorVM.Id = juror.Id;
                return new SaveResultVM(true);

            }
            catch (Exception ex)
            {
                logger.LogError(ex, ex.Message);
                return new SaveResultVM()
                {
                    IsSuccessfull = false,
                    ErrorMessage = MessageConstant.Values.SaveFailed
                };
            }
        }

        public async Task<SaveResultVM> DeletePhoto(string Id)
        {
            try
            {
                var juror = repo.GetById<Juror>(Id);
                juror.Content = null;
                juror.ContentType = null;
                juror.FileName = null;
                repo.Update(juror);
                await repo.SaveChangesAsync();
                return new SaveResultVM(true);
            }
            catch (Exception ex)
            {
                logger.LogError(ex, ex.Message);
                return new SaveResultVM()
                {
                    IsSuccessfull = false,
                    ErrorMessage = MessageConstant.Values.SaveFailed
                };
            }
        }

        public JurorTimeLineVM GetJurorTimeLineVM(string Id)
        {
            var juror = repo.AllReadonly<Juror>()
                            .Where(x => x.Id == Id)
                            .Select(x => new JurorTimeLineVM()
                            {
                                Id = x.Id,
                                Uic = x.Uic,
                                AddressText = x.AddressText,
                                EducationLabel = x.Education.Label,
                                EducationRangLabel = x.EducationRang.Label,
                                FullName = x.FullName,
                                Phone = x.Phone,
                                EMail = x.EMail,
                                Content = x.Content,
                                CityLabel = x.AddressCity.Name,
                                Mandates = x.Mandates.Where(m => m.ParentId == null).Select(m => new MandateTimeLineVM()
                                {
                                    Id = m.Id,
                                    MandateTypeLabel = m.MandateType.Label,
                                    DateFrom = m.DateStart,
                                    DateTo = m.DateEnd,
                                    CourtLabel = m.Court.Label,
                                    DateTermination = m.DateTermination,
                                    IsDateTermination = m.DateTermination != null,
                                    ParentId = m.ParentId,
                                    ParentDateFrom = m.ParentId == null ? m.DateStart : m.MandateParent.DateStart,
                                    BusinessTrip = x.Mandates.Where(y => y.MandateTypeId == JurorConstants.Mandate.MandateMissionType && y.ParentId == m.Id).Select(y => new MandateTimeLineVM()
                                    {
                                        Id = y.Id,
                                        MandateTypeLabel = y.MandateType.Label,
                                        DateFrom = y.DateStart,
                                        DateTo = y.DateEnd,
                                        CourtLabel = y.Court.Label,
                                        DateTermination = y.DateTermination,
                                        IsDateTermination = y.DateTermination != null,
                                        ParentId = y.ParentId,
                                        ParentDateFrom = y.ParentId == null ? y.DateStart : y.MandateParent.DateStart,
                                    }).ToList()
                                }).ToList(),
                                Specialities = x.Specialities
                                                .Select(y => y.Speciality.Label)
                                                .ToList()
                            })
                            .FirstOrDefault();

            //juror.JurorTimeLineYearMandates = FillDataJurorTimeLineVMByMandateOrJuror(juror.Id, null);

            return juror;
        }

        public List<JurorTimeLineYearMandateVM> GetDataJurorTimeLineVMByMandateOrJuror(string JurorId, int? MandateId)
        {
            List<JurorTimeLineYearMandateVM> result = new List<JurorTimeLineYearMandateVM>();

            var caseSessionAmounts = MandateId != null ? repo.AllReadonly<CaseSessionAmount>()
                                                             .Include(x => x.Case)
                                                             .ThenInclude(x => x.CaseType)
                                                             .Include(x => x.CaseSesion)
                                                             .Include(x => x.JurorMandate)
                                                             .ThenInclude(x => x.Court)
                                                             .Where(x => x.JurorMandateId == MandateId)
                                                             .ToList() :
                                                         repo.AllReadonly<CaseSessionAmount>()
                                                             .Include(x => x.Case)
                                                             .ThenInclude(x => x.CaseType)
                                                             .Include(x => x.CaseSesion)
                                                             .Include(x => x.JurorMandate)
                                                             .ThenInclude(x => x.Court)
                                                             .Where(x => x.JurorMandate.JurorId == JurorId)
                                                             .ToList();

            List<JurorTimeLineYearMandateDataVM> data = new List<JurorTimeLineYearMandateDataVM>();

            foreach (var caseSessionAmount in caseSessionAmounts)
            {
                var element = data.Where(x => x.MandateId == caseSessionAmount.JurorMandateId &&
                                              x.Year == caseSessionAmount.CaseSesion.DateStart.Year &&
                                              x.CaseId == caseSessionAmount.CaseId)
                                  .FirstOrDefault();

                if (element != null)
                {
                    element.CountSession++;
                }
                else
                {
                    JurorTimeLineYearMandateDataVM jurorTimeLineYearMandateDataVM = new JurorTimeLineYearMandateDataVM()
                    {
                        MandateId = caseSessionAmount.JurorMandateId,
                        MandateLabel = caseSessionAmount.JurorMandate.DateStart.ToString("dd.MM.yyyy") + " - " + (caseSessionAmount.JurorMandate.DateEnd != null ? (caseSessionAmount.JurorMandate.DateEnd ?? DateTime.Now).ToString("dd.MM.yyyy") : ""),
                        Year = caseSessionAmount.CaseSesion.DateStart.Year,
                        ParentId = caseSessionAmount.JurorMandate.ParentId,
                        ParentLabel = (caseSessionAmount.JurorMandate.ParentId != null) ? "Командироване" + caseSessionAmount.JurorMandate.Court.Label : String.Empty,
                        CaseId = caseSessionAmount.CaseId,
                        CaseLabel = $"{caseSessionAmount.Case.CaseType.ShortLabel} {caseSessionAmount.Case.ShortNumber}/{caseSessionAmount.Case.RegYear}",
                        CountSession = 1
                    };

                    data.Add(jurorTimeLineYearMandateDataVM);
                }
            }

            foreach (var dataVM in data)
            {
                if (!result.Any(x => x.MandateId == (dataVM.ParentId == null ? dataVM.MandateId : dataVM.ParentId) &&
                                     x.Year == dataVM.Year))
                {
                    var dataElement = new JurorTimeLineYearMandateVM()
                    {
                        MandateId = (dataVM.ParentId == null ? dataVM.MandateId : dataVM.ParentId ?? 0),
                        Year = dataVM.Year,
                        MandateLabel = dataVM.MandateLabel
                    };

                    result.Add(dataElement);
                }
            }

            foreach (var jurorTimeLineYearMandateVM in result)
            {
                jurorTimeLineYearMandateVM.JurorTimeLineYearMandateDatas = data.Where(x => (x.MandateId == jurorTimeLineYearMandateVM.MandateId || x.ParentId == jurorTimeLineYearMandateVM.MandateId) && (x.Year == jurorTimeLineYearMandateVM.Year)).ToList();
            }

            return result;
        }

        public List<SessionTimeLineVM> GetSessionTimeLine(int CaseId, int Year, int MandateId)
        {
            var actIds = repo.AllReadonly<CaseSessionAmount>()
                             .Where(x => x.JurorMandateId == MandateId &&
                                         x.CaseId == CaseId &&
                                         x.CaseSesion.DateStart.Year == Year)
                             .Select(x => x.CaseSesionId)
                             .ToList();

            var sessionTimeLines = repo.AllReadonly<CaseSession>()
                                       .Where(x => actIds.Contains(x.Id))
                                       .Select(x => new SessionTimeLineVM()
                                       {
                                           CaseId = x.CaseId,
                                           CaseLabel = $"{x.Case.CaseType.ShortLabel} {x.Case.ShortNumber}/{x.Case.RegYear}",
                                           SessionKind = x.SessionKind,
                                           State = x.State,
                                           Result = x.Result,
                                           DateStart = x.DateStart,
                                           Fee = x.Amounts.Any(a => a.JurorMandateId == MandateId && a.CaseSesionId == x.Id) ? x.Amounts.Where(a => a.JurorMandateId == MandateId && a.CaseSesionId == x.Id).FirstOrDefault().Fee : 0,
                                           Expences = x.Amounts.Any(a => a.JurorMandateId == MandateId && a.CaseSesionId == x.Id) ? x.Amounts.Where(a => a.JurorMandateId == MandateId && a.CaseSesionId == x.Id).FirstOrDefault().Expences : 0,
                                           Fine = x.Amounts.Any(a => a.JurorMandateId == MandateId && a.CaseSesionId == x.Id) ? x.Amounts.Where(a => a.JurorMandateId == MandateId && a.CaseSesionId == x.Id).FirstOrDefault().Fine : 0,
                                           Acts = x.Acts.Select(m => new ActTimeLineVM() { Id = m.Id, ActKind = m.ActKind, RegDate = m.RegDate, RegNumber = m.RegNumber }).ToList()
                                       })
                                       .ToList();

            return sessionTimeLines;
        }

        public Juror GetJurorByUic(string uic)
        {
            return repo.AllReadonly<Juror>()
               .Where(x => x.Uic.Equals(uic ?? ""))
               .FirstOrDefault();
        }

        public IQueryable<ReportAggregatedDataVM> ReportAggregatedData(FilterReportAggregatedDataVM ModelFilter)
        {
            List<ReportAggregatedDataVM> result = new List<ReportAggregatedDataVM>();

            var dateNow = DateTime.Now;

            Expression<Func<CaseSessionAmount, bool>> dateFromSearch = x => true;
            if (ModelFilter.DateFrom != null)
                dateFromSearch = x => x.JurorMandate.DateStart >= ModelFilter.DateFrom;

            Expression<Func<CaseSessionAmount, bool>> dateToSearch = x => true;
            if (ModelFilter.DateTo != null)
                dateToSearch = x => (x.JurorMandate.DateEnd ?? dateNow) <= ModelFilter.DateTo.MakeEndDate();

            Expression<Func<CaseSessionAmount, bool>> courtMandateSearch = x => true;
            if (ModelFilter.CourtMandateId > 0)
                courtMandateSearch = x => x.JurorMandate.CourtId == ModelFilter.CourtMandateId && x.JurorMandate.MandateTypeId == JurorConstants.Mandate.MandateType;

            Expression<Func<CaseSessionAmount, bool>> courtKomandirovkaSearch = x => true;
            if (ModelFilter.CourtKomandirovkaId > 0)
            {
                if (ModelFilter.CourtMandateId > 0)
                    courtKomandirovkaSearch = x => x.JurorMandate.CourtId == ModelFilter.CourtKomandirovkaId && x.JurorMandate.MandateTypeId == JurorConstants.Mandate.MandateMissionType && x.JurorMandate.MandateParent.CourtId == ModelFilter.CourtMandateId;
                else
                    courtKomandirovkaSearch = x => x.JurorMandate.CourtId == ModelFilter.CourtKomandirovkaId && x.JurorMandate.MandateTypeId == JurorConstants.Mandate.MandateMissionType;
            }

            Expression<Func<CaseSessionAmount, bool>> regNumberSearch = x => true;
            if (!string.IsNullOrEmpty(ModelFilter.RegNumber))
                regNumberSearch = x => EF.Functions.ILike(x.JurorMandate.Juror.RegNumber, ModelFilter.RegNumber.ToPaternSearch());

            Expression<Func<CaseSessionAmount, bool>> fullNameSearch = x => true;
            if (!string.IsNullOrEmpty(ModelFilter.FullName))
                fullNameSearch = x => EF.Functions.ILike(x.JurorMandate.Juror.FullName, ModelFilter.FullName.ToPaternSearch());

            Expression<Func<CaseSessionAmount, bool>> uicSearch = x => true;
            if (!string.IsNullOrEmpty(ModelFilter.Uic))
                uicSearch = x => x.JurorMandate.Juror.Uic == ModelFilter.Uic;

            Expression<Func<CaseSessionAmount, bool>> sessionKindSearch = x => true;
            if (!string.IsNullOrEmpty(ModelFilter.SessionKind))
                sessionKindSearch = x => EF.Functions.ILike(x.CaseSesion.SessionKind, ModelFilter.SessionKind.ToPaternSearch());

            Expression<Func<CaseSessionAmount, bool>> sessionStateSearch = x => true;
            if (!string.IsNullOrEmpty(ModelFilter.SessionState))
                sessionStateSearch = x => EF.Functions.ILike(x.CaseSesion.State, ModelFilter.SessionState.ToPaternSearch());

            Expression<Func<CaseSessionAmount, bool>> actKindSearch = x => true;
            if (!string.IsNullOrEmpty(ModelFilter.SessionState))
                actKindSearch = x => x.CaseSesion.Acts.Any(x => EF.Functions.ILike(x.ActKind, ModelFilter.ActKind.ToPaternSearch()));

            var caseSessionAmounts = repo.AllReadonly<CaseSessionAmount>()
                                         .Include(x => x.Case)
                                         .Include(x => x.CaseSesion)
                                         .ThenInclude(x => x.Acts)
                                         .Include(x => x.JurorMandate)
                                         .ThenInclude(x => x.MandateParent)
                                         .ThenInclude(x => x.Court)
                                         .Include(x => x.JurorMandate)
                                         .ThenInclude(x => x.Court)
                                         .Include(x => x.JurorMandate)
                                         .ThenInclude(x => x.Juror)
                                         .Where(dateFromSearch)
                                         .Where(dateToSearch)
                                         .Where(courtMandateSearch)
                                         .Where(courtKomandirovkaSearch)
                                         .Where(regNumberSearch)
                                         .Where(fullNameSearch)
                                         .Where(uicSearch)
                                         .Where(sessionKindSearch)
                                         .Where(sessionStateSearch)
                                         .Where(actKindSearch)
                                         .ToList();

            var mandatId = 0;
            List<int> caseId = new List<int>();

            foreach (var caseSessionAmount in caseSessionAmounts.OrderBy(x => x.JurorMandateId))
            {
                if (mandatId != caseSessionAmount.JurorMandateId)
                {
                    mandatId = caseSessionAmount.JurorMandateId;
                    caseId.Clear();
                }

                var item = result.Where(x => (x.PrentMandateId == (caseSessionAmount.JurorMandate.ParentId == null ? caseSessionAmount.JurorMandateId : caseSessionAmount.JurorMandate.ParentId)) &&
                                             (x.MandateId == caseSessionAmount.JurorMandateId))
                                 .FirstOrDefault();

                if (item == null)
                {
                    item = new ReportAggregatedDataVM()
                    {
                        PrentMandateId = (caseSessionAmount.JurorMandate.ParentId == null ? caseSessionAmount.JurorMandateId : (caseSessionAmount.JurorMandate.ParentId ?? 0)),
                        MandateId = caseSessionAmount.JurorMandateId,
                        RegNumber = caseSessionAmount.JurorMandate.Juror.RegNumber,
                        FullName = caseSessionAmount.JurorMandate.Juror.FullName,
                        CourtMandateLabel = caseSessionAmount.JurorMandate.Court.Label,
                        CourtKomandirovkaLabel = (caseSessionAmount.JurorMandate.ParentId == null ? string.Empty : caseSessionAmount.JurorMandate.MandateParent.Court.Label),
                        DateFrom = (caseSessionAmount.JurorMandate.ParentId == null ? caseSessionAmount.JurorMandate.DateStart : caseSessionAmount.JurorMandate.MandateParent.DateStart),
                        DateTo = (caseSessionAmount.JurorMandate.ParentId == null ? caseSessionAmount.JurorMandate.DateEnd : caseSessionAmount.JurorMandate.MandateParent.DateEnd),
                        CountCase = 1,
                        CountAct = (!string.IsNullOrEmpty(ModelFilter.SessionState) ? caseSessionAmount.CaseSesion.Acts.Count(x => x.ActKind.ToUpper().Contains(ModelFilter.ActKind.ToUpper())) : caseSessionAmount.CaseSesion.Acts.Count()),
                        Fee = caseSessionAmount.Fee,
                        Expences = caseSessionAmount.Expences,
                        Fine = caseSessionAmount.Fine
                    };

                    result.Add(item);
                    caseId.Add(caseSessionAmount.CaseId);
                }
                else
                {
                    if (!caseId.Any(x => x == caseSessionAmount.CaseId))
                    {
                        item.CountCase++;
                        caseId.Add(caseSessionAmount.CaseId);
                    }

                    item.CountAct = item.CountAct + ((!string.IsNullOrEmpty(ModelFilter.SessionState) ? caseSessionAmount.CaseSesion.Acts.Count(x => x.ActKind.ToUpper().Contains(ModelFilter.ActKind.ToUpper())) : caseSessionAmount.CaseSesion.Acts.Count()));
                    item.Fee = item.Fee + caseSessionAmount.Fee;
                    item.Expences = item.Expences + caseSessionAmount.Expences;
                    item.Fine = item.Fine + caseSessionAmount.Fine;
                }
            }

            return result.OrderBy(x => x.PrentMandateId).ThenBy(x => x.MandateId).AsQueryable();
        }

        private bool IsUicFound(JurorVM model)
        {
            return repo.AllReadonly<Juror>()
                 .Any(x => x.Uic == model.Uic && x.Id != model.Id);
        }

        public bool IsExistActiveMandatesAfterDate(DateTime dateTime, string JurorId)
        {
            return repo.AllReadonly<JurorMandate>()
                       .Any(x => x.JurorId == JurorId &&
                                 x.DateEnd > dateTime);
        }

        private async Task UpdateJurorSpecialities(Juror juror, JurorVM model)
        {
            var jurorSpecialities = repo.All<JurorSpeciality>()
                           .Where(x => x.JurorId.Equals(model.Id))
                           .ToList();

            var checkedSpecialities = model.Specialities
                .Where(x => x.Checked)
                .Select(x => int.Parse(x.Value))
                .ToList();

            repo.DeleteRange(jurorSpecialities
                .Where(x => !checkedSpecialities.Contains(x.SpecialityId))
                );

            foreach (var modelSpecialityId in checkedSpecialities)
            {
                var specialityFoundCount = jurorSpecialities
                    .Where(x => x.SpecialityId == modelSpecialityId)
                    .Count();

                if (specialityFoundCount == 0)
                {
                    await repo.AddAsync(new JurorSpeciality()
                    {
                        JurorId = juror.Id,
                        SpecialityId = modelSpecialityId,
                        DateWrt = DateTime.Now
                    });
                }
            }
        }
        private void MapMandateToVM(JurorMandate mandate, MandateVM mandateVM)
        {
            mandate.JurorId = mandateVM.JurorId;
            mandate.CourtId = mandateVM.CourtId;
            mandate.ParentId = mandateVM.ParentId;
            mandate.RegisterCourtId = mandateVM.RegisterCourtId > 0 ? mandateVM.RegisterCourtId : null;
            mandate.MandateTypeId = mandateVM.MandateTypeId;
            mandate.DateStart = mandateVM.DateStart;
            mandate.DateEnd = mandateVM.DateEnd;
            mandate.DateWrt = DateTime.Now;
            mandate.UserId = userContext.UserId;
            mandate.MunicipalityId = mandateVM.MunicipalityId;
            mandate.DateTermination = mandateVM.DateTermination;
            mandate.DateTerminationDescription = mandateVM.DateTerminationDescription;
            mandate.MandateNo = mandateVM.MandateNo.ToString();
        }

        private MandateVM MapVMToJMandate(JurorMandate mandate)
        {
            MandateVM model = new MandateVM();
            model.Id = mandate.Id;
            model.JurorId = mandate.JurorId;
            model.CourtId = mandate.CourtId;
            model.RegisterCourtId = mandate.RegisterCourtId;
            model.MandateTypeId = mandate.MandateTypeId;
            model.MandateTypeLabel = mandate.MandateType.Label;
            model.DateStart = mandate.DateStart;
            model.DateEnd = mandate.DateEnd;
            model.MunicipalityId = mandate.MunicipalityId;
            model.JurorFullName = mandate.Juror.FullName;
            model.ParentId = mandate.ParentId;
            model.DateTermination = mandate.DateTermination;
            model.DateTerminationDescription = mandate.DateTerminationDescription;
            model.MandateNo = (string.IsNullOrEmpty(mandate.MandateNo) ? 0 : int.Parse(mandate.MandateNo));
            return model;
        }

        /// <summary>
        /// Set juror props values from VM
        /// </summary>
        /// <param name="juror">Entity</param>
        /// <param name="model">JurorVM</param>
        private void MapJurorToVM(Juror juror, JurorVM model)
        {

            juror.Uic = model.Uic;
            juror.FirstName = model.FirstName;
            juror.MiddleName = model.MiddleName;
            juror.FamilyName = model.FamilyName;
            juror.FullName = $"{model.FirstName} {model.MiddleName} {model.FamilyName}";
            juror.AddressCityId = model.CityId;
            juror.AddressText = model.AddressText;
            juror.DateWrt = DateTime.Now;
            juror.UserId = userContext.UserId;
            juror.DateStart = model.DateStart;
            juror.DateEnd = model.DateEnd;
            juror.EducationId = model.EducationId;
            juror.EducationRangId = model.EducationRangId < 1 ? null : model.EducationRangId;
            juror.Phone = model.Phone;
            juror.EMail = model.EMail;
            //juror.RegNumber = model.RegNumber;

            if (model.Content != null)
            {
                juror.Content = model.Content;
                juror.ContentType = model.ContentType;
                juror.FileName = model.FileName;
            }

            if (string.IsNullOrEmpty(model.Id) && model.Specialities != null && model.Specialities.Where(x => x.Checked).ToList().Count > 0)
            {
                var jSpec = new List<JurorSpeciality>();
                jSpec.AddRange(
                    model.Specialities.Where(x => x.Checked)
                    .Select(x => new JurorSpeciality { SpecialityId = int.Parse(x.Value), DateWrt = DateTime.Now }));
                juror.Specialities = jSpec;

            }
        }

        /// <summary>
        /// Set VM props values from juror
        /// </summary>
        /// <param name="juror">Entity</param>
        /// <returns>JurorVM</returns>
        private JurorVM MapVMToJuror(Juror juror)
        {
            JurorVM model = new JurorVM();
            model.Id = juror.Id;
            model.Uic = juror.Uic;
            model.FirstName = juror.FirstName;
            model.MiddleName = juror.MiddleName;
            model.FamilyName = juror.FamilyName;
            model.CityId = juror.AddressCityId;
            if (model.CityId > 0)
            {
                model.CityCode = nomService.GetById<EkEkatte>(model.CityId)?.Ekatte;
            }
            model.AddressText = juror.AddressText;
            model.DateStart = juror.DateStart;
            model.DateEnd = juror.DateEnd;
            model.EducationId = juror.EducationId;
            model.EducationRangId = juror.EducationRangId;
            model.FullName = juror.FullName;
            model.Phone = juror.Phone;
            model.EMail = juror.EMail;
            model.Content = juror.Content;
            model.ContentType = juror.ContentType;
            model.FileName = juror.FileName;
            model.RegNumber = juror.RegNumber;
            model.Specialities = repo.AllReadonly<NomSpeciality>().Where(x => x.IsActive).ToList().Select(x => new CheckListVM
            {
                Label = x.Label,
                Value = x.Id.ToString(),
                Checked = juror.Specialities.Any(s => s.SpecialityId == x.Id)
            }).ToList();
            return model;
        }

        private string generateRegisterNumber()
        {
            var counterValue = nomService.GetCounterValue(1);
            return $"{DateTime.Now:yyyy}{counterValue:D4}";
        }

        public IQueryable<ReportFullVM> ReportFullData_Select(FilterReportFullVM filter)
        {

            Expression<Func<CaseSessionAmount, bool>> whereMandateCourt = x => true;
            if (filter.MandateCourtId > 0)
            {
                whereMandateCourt = x => (x.JurorMandate.CourtId == filter.MandateCourtId && x.JurorMandate.ParentId == null)
                 || (x.JurorMandate.ParentId > 0 && x.JurorMandate.MandateParent.CourtId == filter.MandateCourtId);
            }

            Expression<Func<CaseSessionAmount, bool>> whereKomandirovkaCourt = x => true;
            if (filter.KomandirovkaCourtId > 0)
            {
                whereKomandirovkaCourt = x => (x.JurorMandate.CourtId == filter.KomandirovkaCourtId && x.JurorMandate.ParentId > 0);
            }





            Expression<Func<CaseSessionAmount, bool>> whereSessionDateFrom = x => true;
            if (filter.SessionDateFrom != null)
            {
                whereSessionDateFrom = x => x.CaseSesion.DateStart >= filter.SessionDateFrom;
            }
            Expression<Func<CaseSessionAmount, bool>> whereSessionDateTo = x => true;
            if (filter.SessionDateTo != null)
            {
                whereSessionDateTo = x => x.CaseSesion.DateStart <= filter.SessionDateTo.MakeEndDate();
            }

            Expression<Func<CaseSessionAmount, bool>> whereRegisterNumber = x => true;
            if (filter.RegisterNumber != null)
            {
                whereRegisterNumber = x => x.JurorMandate.Juror.RegNumber == filter.RegisterNumber;
            }
            Expression<Func<CaseSessionAmount, bool>> whereFullName = x => true;
            if (filter.JurorName != null)
            {
                whereFullName = x => EF.Functions.ILike(x.JurorMandate.Juror.FullName, filter.JurorName.ToPaternSearch());
            }
            Expression<Func<CaseSessionAmount, bool>> whereSpeciality = x => true;
            if (filter.SpecialityId > 0)
            {
                whereSpeciality = x => x.JurorMandate.Juror.Specialities.Any(s => s.SpecialityId == filter.SpecialityId);
            }
            Expression<Func<CaseSessionAmount, bool>> whereCaseType = x => true;
            if (filter.CaseTypeId > 0)
            {
                whereCaseType = x => x.Case.CaseTypeId == filter.CaseTypeId;
            }
            Expression<Func<CaseSessionAmount, bool>> whereCaseNumber = x => true;
            if (!string.IsNullOrEmpty(filter.CaseNumber))
            {
                whereCaseNumber = x => x.Case.ShortNumber == filter.CaseNumber;
            }
            Expression<Func<CaseSessionAmount, bool>> whereCaseYear = x => true;
            if (filter.CaseYear > 0)
            {
                whereCaseYear = x => x.Case.RegYear == filter.CaseYear;
            }
            Expression<Func<CaseSessionAmount, bool>> whereCaseIsFinished = x => true;
            if (filter.CaseIsFinished > 0)
            {
                switch (filter.CaseIsFinished)
                {
                    case 1:
                        whereCaseIsFinished = x => x.Case.IsFinished == true;
                        break;
                    case 2:
                        whereCaseIsFinished = x => !(x.Case.IsFinished ?? false);
                        break;
                }
            }
            Expression<Func<CaseSessionAmount, bool>> whereActType = x => true;
            if (!string.IsNullOrEmpty(filter.ActType))
            {
                whereActType = x => x.CaseSesion.Acts.Any(a => EF.Functions.ILike(a.ActKind, filter.ActType.ToPaternSearch()));
            }
            Expression<Func<CaseSessionAmount, bool>> whereSessionResult = x => true;
            if (!string.IsNullOrEmpty(filter.SessionResult))
            {
                whereSessionResult = x => EF.Functions.ILike(x.CaseSesion.Result, filter.SessionResult.ToPaternSearch());
            }

            Expression<Func<CaseSessionAmount, bool>> whereGlobalReport = x => true;
            if (userContext.CourtId > 0)
            {
                whereGlobalReport = x => x.Case.CourtId == userContext.CourtId;
            }

            return repo.AllReadonly<CaseSessionAmount>()
                        .Where(whereMandateCourt)
                        .Where(whereKomandirovkaCourt)
                        .Where(whereSessionDateFrom)
                        .Where(whereSessionDateTo)
                        .Where(whereRegisterNumber)
                        .Where(whereSpeciality)
                        .Where(whereFullName)
                        .Where(whereCaseType)
                        .Where(whereCaseNumber)
                        .Where(whereCaseYear)
                        .Where(whereCaseIsFinished)
                        .Where(whereActType)
                        .Where(whereSessionResult)
                        .Where(whereGlobalReport)
                        .Select(x => new ReportFullVM
                        {
                            JurorId = x.JurorMandate.JurorId,
                            RegNumber = x.JurorMandate.Juror.RegNumber,
                            MandateCourt = (x.JurorMandate.ParentId > 0) ? x.JurorMandate.MandateParent.Court.ShortLabel : x.JurorMandate.Court.ShortLabel,
                            KomandirovkaCourt = (x.JurorMandate.ParentId > 0) ? x.JurorMandate.Court.ShortLabel : "",
                            MandateNo = (x.JurorMandate.ParentId > 0) ? x.JurorMandate.MandateParent.MandateNo : x.JurorMandate.MandateNo,
                            MandateFrom = (x.JurorMandate.ParentId > 0) ? x.JurorMandate.MandateParent.DateStart : x.JurorMandate.DateStart,
                            MandateTo = (x.JurorMandate.ParentId > 0) ? x.JurorMandate.MandateParent.DateEnd : x.JurorMandate.DateEnd,
                            JurorName = x.JurorMandate.Juror.FullName,
                            Specialities = x.JurorMandate.Juror.Specialities
                                            .Select(s => s.Speciality.Label).ToArray(),
                            CaseType = x.Case.CaseType.ShortLabel,
                            ShortNumber = x.Case.ShortNumber,
                            CaseYear = x.Case.RegYear,
                            CaseIsFinished = x.Case.IsFinished == true,
                            SessionKind = x.CaseSesion.SessionKind,
                            SessionResult = x.CaseSesion.Result,
                            SessionDate = x.CaseSesion.DateStart,
                            Acts = x.CaseSesion.Acts.Select(a => new ReportFullActVM
                            {
                                ActKind = a.ActKind,
                                RegNumber = a.RegNumber,
                                RegDate = a.RegDate
                            }).ToArray()
                        }).AsQueryable();

        }

        public IQueryable<CourtLocalReportVM> CourtLocalReport(FilterCourtLocalReportVM ModelFilter)
        {
            var dateNow = DateTime.Now;

            Expression<Func<CaseSessionAmount, bool>> dateSearch = x => true;
            if (ModelFilter.DateFrom != null || ModelFilter.DateTo != null)
                dateSearch = x => x.CaseSesion.DateStart >= ModelFilter.DateFrom && x.CaseSesion.DateEnd <= ModelFilter.DateTo.MakeEndDate();

            Expression<Func<CaseSessionAmount, bool>> caseTypeSearch = x => true;
            if (ModelFilter.CaseTypeId > 0)
                caseTypeSearch = x => x.Case.CaseTypeId == ModelFilter.CaseTypeId;

            Expression<Func<CaseSessionAmount, bool>> caseRegNumberSearch = x => true;
            if (!string.IsNullOrEmpty(ModelFilter.CaseRegNumber))
                caseRegNumberSearch = x => EF.Functions.ILike(x.Case.RegNumber, ModelFilter.CaseRegNumber.ToPaternSearch());

            Expression<Func<CaseSessionAmount, bool>> isFinishedSearch = x => true;
            switch (ModelFilter.IsFinished)
            {
                case NomenclatureConstants.IsFinishedValue.DoneCase:
                    isFinishedSearch = x => (x.Case.IsFinished ?? false);
                    break;
                case NomenclatureConstants.IsFinishedValue.UnfinishedCase:
                    isFinishedSearch = x => !(x.Case.IsFinished ?? false);
                    break;
            }

            Expression<Func<CaseSessionAmount, bool>> regNumberSearch = x => true;
            if (!string.IsNullOrEmpty(ModelFilter.RegNumber))
                regNumberSearch = x => EF.Functions.ILike(x.JurorMandate.Juror.RegNumber, ModelFilter.RegNumber.ToPaternSearch());

            Expression<Func<CaseSessionAmount, bool>> fullNameSearch = x => true;
            if (!string.IsNullOrEmpty(ModelFilter.FullName))
                fullNameSearch = x => EF.Functions.ILike(x.JurorMandate.Juror.FullName, ModelFilter.FullName.ToPaternSearch());

            Expression<Func<CaseSessionAmount, bool>> uicSearch = x => true;
            if (!string.IsNullOrEmpty(ModelFilter.Uic))
                uicSearch = x => x.JurorMandate.Juror.Uic == ModelFilter.Uic;

            Expression<Func<CaseSessionAmount, bool>> sessionKindSearch = x => true;
            if (!string.IsNullOrEmpty(ModelFilter.SessionKind))
                sessionKindSearch = x => EF.Functions.ILike(x.CaseSesion.SessionKind, ModelFilter.SessionKind.ToPaternSearch());

            Expression<Func<CaseSessionAmount, bool>> sessionStateSearch = x => true;
            if (!string.IsNullOrEmpty(ModelFilter.SessionState))
                sessionStateSearch = x => EF.Functions.ILike(x.CaseSesion.State, ModelFilter.SessionState.ToPaternSearch());

            Expression<Func<CaseSessionAmount, bool>> actKindSearch = x => true;
            if (!string.IsNullOrEmpty(ModelFilter.SessionState))
                actKindSearch = x => x.CaseSesion.Acts.Any(x => EF.Functions.ILike(x.ActKind, ModelFilter.ActKind.ToPaternSearch()));

            Expression<Func<CaseSessionAmount, bool>> userCourtSearch = x => true;
            if (userContext.CourtId > 0)
                userCourtSearch = x => x.JurorMandate.CourtId == userContext.CourtId;

            return repo.AllReadonly<CaseSessionAmount>()
                       .Where(dateSearch)
                       .Where(caseTypeSearch)
                       .Where(caseRegNumberSearch)
                       .Where(isFinishedSearch)
                       .Where(regNumberSearch)
                       .Where(fullNameSearch)
                       .Where(uicSearch)
                       .Where(sessionKindSearch)
                       .Where(sessionStateSearch)
                       .Where(actKindSearch)
                       .Where(userCourtSearch)
                       .Select(x => new CourtLocalReportVM()
                       {
                           RegNumber = x.JurorMandate.Juror.RegNumber,
                           MandateNo = x.JurorMandate.MandateNo,
                           FullName = x.JurorMandate.Juror.FullName,
                           Specialities = x.JurorMandate.Juror.Specialities.Select(s => s.Speciality.Label).ToArray(),
                           MandateDateFrom = x.JurorMandate.DateStart,
                           MandateDateTo = x.JurorMandate.DateEnd,
                           CaseType = x.Case.CaseType.Label,
                           CaseRegNumber = x.Case.ShortNumber,
                           SessionLabel = x.CaseSesion.SessionKind + " / " + x.CaseSesion.DateStart.ToString("dd.MM.yyyy HH:mm"),
                           SessionActs = x.CaseSesion.Acts.Select(a => a.ActKind + " " + a.RegNumber + "/" + a.RegDate.ToString("dd.MM.yyyy")).ToArray()
                       })
                       .AsQueryable();
        }

        public bool IsExistMandateNo(string JurorId, int MandateNo, int? Id)
        {
            return repo.AllReadonly<JurorMandate>()
                       .Any(x => x.JurorId == JurorId &&
                                 x.MandateNo == MandateNo.ToString() &&
                                 (Id > 0 ? x.Id != Id : true));
        }
    }
}
