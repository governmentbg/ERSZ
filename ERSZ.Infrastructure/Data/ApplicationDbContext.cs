using ERSZ.Infrastructure.Contracts;
using ERSZ.Infrastructure.Data.Models.Case;
using ERSZ.Infrastructure.Data.Models.Common;
using ERSZ.Infrastructure.Data.Models.Ekatte;
using ERSZ.Infrastructure.Data.Models.Identity;
using ERSZ.Infrastructure.Data.Models.Nomenclatures;
using ERSZ.Infrastructure.Data.Models.Register;
using Microsoft.EntityFrameworkCore;

namespace ERSZ.Infrastructure.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            #region Identity configuration

            builder.ApplyConfiguration(new ApplicationUserConfiguration());
            builder.ApplyConfiguration(new ApplicationRoleConfiguration());
            builder.ApplyConfiguration(new ApplicationUserRoleConfiguration());
            builder.ApplyConfiguration(new ApplicationUserClaimConfiguration());
            builder.ApplyConfiguration(new ApplicationUserLoginConfiguration());
            builder.ApplyConfiguration(new ApplicationRoleClaimConfiguration());
            builder.ApplyConfiguration(new ApplicationUserTokenConfiguration());

            #endregion


            builder.Entity<CommonCourtEkatte>()
               .HasKey(x => new { x.CourtId, x.EkEkatteId });

            builder
                .Entity<Juror>()
                .Property(e => e.Id)
                .ValueGeneratedOnAdd();

            builder.Entity<CounterValueVM>().HasNoKey();
        }

        #region Case

        public DbSet<CaseData> CaseData { get; set; }
        public DbSet<CaseSelectionProtokol> CaseSelectionProtokol { get; set; }
        public DbSet<CaseDismissal> CaseDismissal { get; set; }
        public DbSet<CaseSession> CaseSession { get; set; }
        public DbSet<CaseSessionAct> CaseSessionAct { get; set; }
        public DbSet<CaseSessionAmount> CaseSessionAmount { get; set; }

        #endregion

        #region Common
        public DbSet<AuditLog> AuditLog { get; set; }
        public DbSet<CommonCourt> CommonCourt { get; set; }
        public DbSet<MongoFile> MongoFile { get; set; }
        public DbSet<LogOperation> LogOperation { get; set; }
        public DbSet<Counter> Counters { get; set; }

        #endregion

        #region Ekatte
        public DbSet<EkEkatte> EkEkatte { get; set; }

        #endregion

        #region Register
        public DbSet<Juror> Jurors { get; set; }

        #endregion


        #region Nomenclatures

        public DbSet<NomCourtType> NomCourtType { get; set; }

        #endregion

        #region DbQueries
        public DbSet<CounterValueVM> CounterValueVMs { get; set; }
        #endregion
    }
}
