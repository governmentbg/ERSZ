using DataTables.AspNet.Core;
using ERSZ.Components;
using ERSZ.Core.Constants;
using ERSZ.Core.Contracts;
using ERSZ.Core.Models;
using ERSZ.Core.Models.Identity;
using ERSZ.Extensions;
using ERSZ.Infrastructure.Constants;
using ERSZ.Infrastructure.Contracts;
using ERSZ.Infrastructure.Data.Models.Common;
using ERSZ.Infrastructure.Data.Models.Identity;
using ERSZ.Infrastructure.Extensions;
using ERSZ.Infrastructure.ViewModels.Common;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace ERSZ.Controllers
{
    public class AccountController : BaseController
    {
        private readonly UserManager<ApplicationUser> userManager;
        private readonly SignInManager<ApplicationUser> signInManager;
        private readonly RoleManager<ApplicationRole> roleManager;
        private readonly ILogger<AccountController> logger;
        private readonly IConfiguration config;
        private readonly IAccountService accountService;
        private readonly IAuditLogService auditService;
        private readonly INomenclatureService nomService;

        public AccountController(
            UserManager<ApplicationUser> _userManager,
             SignInManager<ApplicationUser> _signInManager,
             RoleManager<ApplicationRole> _roleManager,
             ILogger<AccountController> _logger,
             IConfiguration _config,
             IAccountService _accountService,
             IAuditLogService _auditService,
             INomenclatureService _nomService
            )
        {
            userManager = _userManager;
            signInManager = _signInManager;
            roleManager = _roleManager;
            logger = _logger;
            config = _config;
            accountService = _accountService;
            auditService = _auditService;
            nomService = _nomService;
        }

     
        [MenuItem("users")]
        public IActionResult Index()
        {
            Audit_Operation = NomenclatureConstants.AuditOperations.List;
            Audit_Object = $"Управление на потребители";
            return View();
        }

        [HttpPost]
        public IActionResult ListData(IDataTablesRequest request)
        {
            var data = accountService.Select(request.Search?.Value);
            return request.GetResponse(data);
        }

        /// <summary>
        /// Регистриране на потребител
        /// </summary>
        /// <returns></returns>
        public IActionResult Add()
        {
            AccountVM model = new AccountVM()
            {
                DateFrom = DateTime.Now,
            };
            SetViewBag();
            return View(nameof(Edit), model);
        }

        [HttpPost]
        public async Task<IActionResult> Add(AccountVM model)
        {
            SetViewBag();
            if (!model.UIC.IsEGN())
            {
                ModelState.AddModelError(nameof(AccountVM.UIC), "Невалидно ЕГН");
            }

            if (model.DateFrom > (model.DateTo ?? DateTime.Now.AddYears(10)))
            {
                ModelState.AddModelError(nameof(AccountVM.DateFrom), "Дата от е по-голяма от дата до");
            }

            if (!ModelState.IsValid)
            {

                return View(nameof(Edit), model);
            }

            if (!userContext.IsUserInRole(NomenclatureConstants.Roles.GlobalAdmin))
            {
                model.CourtId = userContext.CourtId;
            }

            var checkResult = await accountService.CheckUser(model);
            if (!checkResult.IsSuccessfull)
            {
                SetErrorMessage(checkResult.ErrorMessage);
                SetViewBag();
                return View(nameof(Edit), model);
            }

            var user = new ApplicationUser
            {
                UserName = model.Email,
                Email = model.Email,
                UIC = model.UIC,
                FullName = model.FullName,
                CourtId = model.CourtId,
                DateFrom = model.DateFrom,
                DateTo = model.DateTo,
            };

            IdentityResult res = await userManager.CreateAsync(user);
            if (res.Succeeded)
            {
                Audit_Operation = NomenclatureConstants.AuditOperations.Add;
                Audit_Object = $"Потребител: {user.FullName}";
                SaveLogOperation(NomenclatureConstants.AuditOperations.Add, this.ControllerName, nameof(Edit), model, user.Id);
                SetSuccessMessage(MessageConstant.Values.SaveOK);
                return RedirectToAction(nameof(Edit), new { id = user.Id });
            }
            else
            {
                SetErrorMessage(MessageConstant.Values.SaveFailed);
                SetViewBag();
                return View(nameof(Edit), model);
            }
        }

        /// <summary>
        /// Редакция на потребител
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<IActionResult> Edit(string id)
        {
            var user = await userManager.FindByIdAsync(id);
            if (user == null)
            {
                throw new NotFoundException();
            }
            var model = new AccountVM()
            {
                Id = user.Id,
                UIC = user.UIC,
                FullName = user.FullName,
                Email = user.Email,
                CourtId = user.CourtId,
                DateFrom = user.DateFrom,
                DateTo = user.DateTo,
            };
            Func<ApplicationRole, bool> whereRoles = x => true;
            if (!userContext.IsUserInRole(NomenclatureConstants.Roles.GlobalAdmin))
            {
                whereRoles = x => x.Name != NomenclatureConstants.Roles.GlobalAdmin;
            }
            model.Roles = roleManager.Roles.ToList()
                .Where(x => whereRoles(x))
                .Select(x => new CheckListVM
                {
                    Value = x.Name,
                    Label = x.Label,
                    Checked = false
                }).OrderBy(x => x.Label).ToList();

            var userRoles = await userManager.GetRolesAsync(user);
            if ((userRoles.Any(x => x == NomenclatureConstants.Roles.GlobalAdmin)
                && !userContext.IsUserInRole(NomenclatureConstants.Roles.GlobalAdmin))
                || (user.CourtId == null && userContext.CourtId > 0))
            {
                SetErrorMessage("Нямате право да коригиране глобални потребители");
                return RedirectToAction(nameof(Index));
            }
            foreach (var role in model.Roles)
            {
                if (userRoles.Any(x => x == role.Value))
                {
                    role.Checked = true;
                }
            }

            Audit_Operation = NomenclatureConstants.AuditOperations.View;
            Audit_Object = $"Потребител: {user.FullName}";
            SetViewBag();
            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(AccountVM model)
        {
            SetViewBag();
            if (!model.UIC.IsEGN())
            {
                ModelState.AddModelError(nameof(AccountVM.UIC), "Невалидно ЕГН");
            }

            if (model.DateFrom > (model.DateTo ?? DateTime.Now.AddYears(10)))
            {
                ModelState.AddModelError(nameof(AccountVM.DateFrom), "Дата от е по-голяма от дата до");
            }

            if (!ModelState.IsValid)
            {

                return View(model);
            }

            var checkResult = await accountService.CheckUser(model);
            if (!checkResult.IsSuccessfull)
            {
                SetErrorMessage(checkResult.ErrorMessage);
                SetViewBag();
                return View(model);
            }

            //Редактиране на потребител
            var user = await userManager.FindByIdAsync(model.Id);

            //Задаване на роли/групи
            var userRoles = await userManager.GetRolesAsync(user);
            var res = await userManager.RemoveFromRolesAsync(user, userRoles);
            if (res.Succeeded)
            {
                res = await userManager.AddToRolesAsync(user, model.Roles.Where(x => x.Checked).Select(x => x.Value));

                user.UIC = model.UIC;
                user.Email = model.Email;
                user.UserName = model.Email;
                user.FullName = model.FullName;
                user.CourtId = model.CourtId;
                user.DateFrom = model.DateFrom;
                user.DateTo = model.DateTo;
                await userManager.UpdateAsync(user);
                Audit_Operation = NomenclatureConstants.AuditOperations.Edit;
                Audit_Object = $"Потребител: {user.FullName}";
                //if (savedIsLastive == true && !model.IsActive)
                //{
                //    Audit_Object += " - деактивиран.";
                //}
                //if (!savedIsLastive == true && model.IsActive)
                //{
                //    Audit_Object += " - активиран.";
                //}
                var addLogItems = new List<LogOperItemModel>()
                {
                    new LogOperItemModel()
                    {
                        Key = "Роли",
                        Items = model.Roles.Where(x => x.Checked).Select(x=>new LogOperItemModel(){ Value = x.Label}).ToArray()
                    }
                };
                SaveLogOperation(1, model, model.Id, addLogItems);
                SetSuccessMessage(MessageConstant.Values.SaveOK);
            }
            else
            {
                SetErrorMessage(MessageConstant.Values.SaveFailed);
            }

            return RedirectToAction(nameof(Edit), new { id = model.Id });
        }
        void SetViewBag()
        {
            Expression<Func<CommonCourt, bool>> courtId = x => CourtConstants.CourtType.CourtАppointment.Contains(x.CourtTypeId);

            var items = nomService.GetDropDownListExpr(courtId, false, true);
            items[0].Value = null;
            ViewBag.CourtId_ddl = nomService.GetDropDownListExpr(courtId, false, true);
            if (userContext.CourtId > 0 && !userContext.IsUserInRole(NomenclatureConstants.Roles.GlobalAdmin))
            {
                ViewBag.CourtId_ddl = items.Where(x => x.Value == userContext.CourtId.ToString()).ToList();
            }
        }


        /// <summary>
        /// Вход в системата
        /// </summary>
        /// <param name="returnUrl"></param>
        /// <param name="error"></param>
        /// <returns></returns>
        [HttpGet]
        [AllowAnonymous]
        public async Task<IActionResult> Login(string returnUrl = null, string error = null)
        {
            var model = new LoginVM
            {
                ReturnUrl = returnUrl
            };

            model.ExternalLogins = (await signInManager.GetExternalAuthenticationSchemesAsync()).ToList();

            if (!string.IsNullOrEmpty(error))
            {
                ViewBag.errorMessage = error;
            }

            return View(model);
        }

        /// <summary>
        /// Вход чрез външен доставчик на ауторизация - вход с КЕП
        /// </summary>
        /// <param name="provider"></param>
        /// <param name="returnUrl"></param>
        /// <returns></returns>
        [HttpPost]
        [AllowAnonymous]
        public IActionResult ExternalLogin(string provider, string returnUrl = null)
        {
            // Request a redirect to the external login provider.
            var redirectUrl = Url.Action("ExternalLoginCallback", new { returnUrl });
            var properties = signInManager.ConfigureExternalAuthenticationProperties(provider, redirectUrl);

            return new ChallengeResult(provider, properties);
        }

        [AllowAnonymous]
        public async Task<IActionResult> ExternalLoginCallback(string returnUrl = null, string remoteError = null)
        {
            returnUrl = returnUrl ?? Url.Content("~/");
            if (remoteError != null)
            {
                logger.LogError($"Error from external provider: {remoteError}");

                return RedirectToAction("Login", new { ReturnUrl = returnUrl });
            }

            var info = await signInManager.GetExternalLoginInfoAsync();

            if (info == null)
            {
                logger.LogError("Error loading external login information.");

                return RedirectToAction("Login", new { ReturnUrl = returnUrl });
            }

            ApplicationUser user = null;
            if (info.LoginProvider == "IdStampIT")
            {
                user = await accountService.GetByUIC(info.ProviderKey);
                var dateTime = DateTime.Now;

                if (user == null || user.DateFrom > dateTime || (user.DateTo ?? dateTime.AddYears(10)) < dateTime)
                {
                    return RedirectToAction("Login", new { ReturnUrl = returnUrl, error = "Невалиден потребител" });
                }
            }
            if (user == null)
            {
                user = await userManager.FindByLoginAsync(info.LoginProvider, info.ProviderKey);
            }
            if (user != null)
            {
                var claims = await userManager.GetClaimsAsync(user);
                var certNoClaim = claims.FirstOrDefault(c => c.Type == CustomClaimType.IdStampit.CertificateNumber);
                var currentCertNoClaim = info.Principal.Claims.FirstOrDefault(c => c.Type == CustomClaimType.IdStampit.CertificateNumber);

                if (currentCertNoClaim != null)
                {
                    if (certNoClaim != null)
                    {
                        await userManager.ReplaceClaimAsync(user, certNoClaim, currentCertNoClaim);
                    }
                    else
                    {
                        await userManager.AddClaimAsync(user, currentCertNoClaim);
                    }
                }

                await signInManager.SignInAsync(user, isPersistent: false);
                Audit_Operation = NomenclatureConstants.AuditOperations.Login;
                Audit_Object = $"Потребител: {user.FullName} ({user.Email})";
                return LocalRedirect(returnUrl);
            }

            return RedirectToAction("AccessDenied");
        }

        /// <summary>
        /// Обработка на грешка при избор на КЕП
        /// </summary>
        /// <param name="error"></param>
        /// <returns></returns>
        [HttpGet]
        [AllowAnonymous]
        public IActionResult LoginCertError(string error)
        {
            logger.LogError(error);

            return RedirectToAction(nameof(Login), new { error = "Моля изберете валиден сертификат." });
        }

        /// <summary>
        /// Изход от системата
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public async Task<IActionResult> LogOff()
        {
            await signInManager.SignOutAsync();
            return Content("ok");
        }

        ///// <summary>
        ///// Обработване на отказан достъп до функционалност
        ///// </summary>
        ///// <param name="returnUrl"></param>
        ///// <returns></returns>
        //[HttpGet]
        //[AllowAnonymous]
        //public IActionResult AccessDenied(string returnUrl)
        //{
        //    TempData[Core.Constants.MessageConstant.ErrorMessage] = Core.Constants.MessageConstant.Values.Unauthorized;
        //    return LocalRedirect("/");
        //}



        /// <summary>
        /// Журнал на промените
        /// </summary>
        /// <returns></returns>
        [MenuItem("auditlog")]
        public IActionResult AuditLog()
        {
            ViewBag.MenuItemValue = "auditLog";
            return View();
        }

        [HttpPost]
        public IActionResult AuditLog_ListData(IDataTablesRequest request, AuditLogFilterVM filter)
        {
            var data = auditService.Select(filter);
            //Audit_Operation = NomenclatureConstants.AuditOperations.List;
            //Audit_Object = "Журнал на промените";
            return request.GetResponse(data);
        }
    }
}
