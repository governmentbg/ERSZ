using DataTables.AspNet.AspNetCore;
using ERSZ.Core.Configuration;
using ERSZ.Core.Contracts;
using ERSZ.Core.Services;
using ERSZ.Infrastructure.Constants;
using ERSZ.Infrastructure.Contracts;
using ERSZ.Infrastructure.Data;
using ERSZ.Infrastructure.Data.Common;
using ERSZ.Infrastructure.Data.Models.Identity;
using ERSZ.Infrastructure.Data.Models.UserContext;
using ERSZ.Infrastructure.Services;
using LinqKit;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.OAuth;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.AspNetCore.Mvc.Routing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using MongoDB.Driver;
using System;
using System.Security.Claims;
using System.Threading.Tasks;

namespace ERSZ.Extensions
{
    /// <summary>
    /// Описва услугите и контекстите на приложението
    /// </summary>
    public static class IOWebAppServiceCollectionExtension
    {
        /// <summary>
        /// Регистрира услугите на приложението в  IoC контейнера
        /// </summary>
        /// <param name="services">Регистрирани услуги</param>
        public static void AddApplicationServices(this IServiceCollection services)
        {
            services.AddHttpContextAccessor();
            services.TryAddSingleton<IActionContextAccessor, ActionContextAccessor>();
            services.AddScoped<IUrlHelper>(x =>
            {
                var actionContext = x.GetRequiredService<IActionContextAccessor>().ActionContext;
                var factory = x.GetRequiredService<IUrlHelperFactory>();
                return factory.GetUrlHelper(actionContext);
            });

            services.RegisterDataTables();
            services.AddAutoMapper(typeof(IOWebFrameworkProfile).Assembly);

            services.AddScoped<IUserClaimsPrincipalFactory<ApplicationUser>, ApplicationClaimsPrincipalFactory>();
            services.AddScoped<INomenclatureService, NomenclatureService>();
            services.AddScoped<IRegisterService, RegisterService>();
            services.AddScoped<IUserContext, UserContext>();
            services.AddScoped<IEkDitrictService, EkDistrictService>();
            services.AddScoped<IEkEkatteService, EkEkatteService>();
            services.AddScoped<IAccountService, AccountService>();
            services.AddScoped<ILogOperationService, LogOperationService>();
            services.AddScoped<IAuditLogService, AuditLogService>();
            services.AddScoped<IExcelImportService, ExcelImportService>();

            services.AddScoped<ICdnService, CdnService>();
        }

        public static void AddApplicationAuthentication(this WebApplicationBuilder builder)
        {

            IConfiguration Configuration = builder.Configuration;

            builder.Services.AddAuthentication(x =>
            {
                x.DefaultScheme = CookieAuthenticationDefaults.AuthenticationScheme;
            }).AddCookie()
                        .AddStampIT(options =>
                        {
                            options.AppId = Configuration.GetValue<string>("Authentication:StampIT:AppId");
                            options.AppSecret = Configuration.GetValue<string>("Authentication:StampIT:AppSecret");
                            options.Scope.Add("pid");
                            options.ClaimActions.DeleteClaim(ClaimTypes.NameIdentifier);
                            options.ClaimActions.MapJsonKey(ClaimTypes.NameIdentifier, "pid");
                            options.ClaimActions.MapJsonKey(CustomClaimType.IdStampit.CertificateNumber, "certno");
                            options.AuthorizationEndpoint = Configuration.GetValue<string>("Authentication:StampIT:AuthorizationEndpoint");
                            options.TokenEndpoint = Configuration.GetValue<string>("Authentication:StampIT:TokenEndpoint");
                            options.UserInformationEndpoint = Configuration.GetValue<string>("Authentication:StampIT:UserInformationEndpoint");
                            options.Events = new OAuthEvents()
                            {
                                OnRemoteFailure = context => HandleRemoteFailure(context)
                            };
                        });

            int cookieMaxAgeMinutes = Configuration.GetValue<int>("Authentication:CookieMaxAgeMinutes");
            builder.Services.ConfigureApplicationCookie(options =>
            {
                options.SlidingExpiration = true;
                options.ExpireTimeSpan = TimeSpan.FromMinutes(cookieMaxAgeMinutes);
            });
        }

        private static Task HandleRemoteFailure(RemoteFailureContext context)
        {
            context.Response.Redirect($"/account/logincerterror?error={context.Failure}");
            context.HandleResponse();

            return Task.FromResult(0);
        }

        /// <summary>
        /// Регистрира контекстите на приложението в IoC контейнера
        /// </summary>
        /// <param name="services">Регистрирани услуги</param>
        /// <param name="Configuration">Настройки на приложението</param>
        public static void AddApplicationDbContext(this IServiceCollection services, IConfiguration Configuration)
        {
            services.AddDbContext<ApplicationDbContext>(options =>
                options.UseNpgsql(Configuration.GetConnectionString("DefaultConnection"),
                    m => m.MigrationsAssembly("ERSZ.Infrastructure"))
                .UseSnakeCaseNamingConvention()
                .WithExpressionExpanding())

                ;
            AppContext.SetSwitch("Npgsql.EnableLegacyTimestampBehavior", true);
            services.AddScoped<IRepository, Repository>();

            services.AddSingleton<IMongoClient>(s => new MongoClient(Configuration.GetConnectionString("MongoDbConnection")));
        }

        /// <summary>
        /// Регистрира Identity provider в IoC контейнера
        /// </summary>
        /// <param name="services">Регистрирани услуги</param>
        public static void AddApplicationIdentity(this IServiceCollection services)
        {
            services.AddIdentity<ApplicationUser, ApplicationRole>(options =>
            {
                options.User.RequireUniqueEmail = false;
                options.SignIn.RequireConfirmedAccount = true;

            })
            .AddUserStore<ApplicationUserStore>()
            .AddRoleStore<ApplicationRoleStore>()
            .AddDefaultTokenProviders();
        }
    }
}
