using ERP.Domain;
using ERP.Domain.Entity;

using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Microsoft.EntityFrameworkCore.ChangeTracking;


using System.Threading;
using System.Security.Cryptography.X509Certificates;
using ERP.Services.Interfaces;

namespace ERP.Infrastructure.DBContexts
{
    public class ApplicationDbContext : DbContext
    {
        private readonly ICurrentUser _getCurrentUser;

        public string conection = "";
        public IConfiguration _config { get; }
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options, IConfiguration configuration, ICurrentUser getCurrentUser) : base(options)
        {
            _config = configuration;

            _getCurrentUser = getCurrentUser;

        }
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {


            conection = _config.GetConnectionString("DefaultConnection");

            conection = conection.Replace("DbName", _getCurrentUser.DataBaseName());


            optionsBuilder.UseSqlServer(conection);
        }



        #region Implementation
        public override int SaveChanges()
        {
            CompleteFields();
            return base.SaveChanges();
        }
        public override int SaveChanges(bool acceptAllChangesOnSuccess)
        {
            CompleteFields();
            return base.SaveChanges(acceptAllChangesOnSuccess);
        }
        public override Task<int> SaveChangesAsync(bool acceptAllChangesOnSuccess, CancellationToken cancellationToken = default)
        {
            CompleteFields();
            return base.SaveChangesAsync(acceptAllChangesOnSuccess, cancellationToken);
        }
        public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            CompleteFields();
            return base.SaveChangesAsync(cancellationToken);
        }
        private void CompleteFields()
        {
            foreach (EntityEntry e in ChangeTracker.Entries())
            {

                foreach (var entry in ChangeTracker.Entries<Audit>())
                {
                    switch (entry.State)
                    {
                        case EntityState.Added:

                            entry.Entity.Id = Guid.NewGuid();
                            entry.Entity.CreatedBy = _getCurrentUser.UserEmail();
                            entry.Entity.CreatedDate = DateTime.UtcNow;
                            entry.Entity.IsActive = true;

                            break;
                        case EntityState.Modified:

                            entry.Entity.LastModifiedBy = _getCurrentUser.UserEmail();
                            entry.Entity.LastModifiedDate = DateTime.UtcNow;
                            break;
                    }
                }
            }

        }
        #endregion 
        public DbSet<PaymentTerm> PaymentTerms { get; set; }
        
        public DbSet<PaymentMethod> PaymentMethod { get; set; }

        public DbSet<Form> Form { get; set; }

        public DbSet<Formfields> Formfields { get; set; }
 
        public DbSet<Roll> Roll { get; set; }

        public DbSet<RollForm> RollForm { get; set; }

        public DbSet<UserRoll> UserRoll { get; set; }

        public DbSet<Banks> Banks { get; set; }

        public DbSet<Currency> Currency { get; set; }

        public DbSet<Module> Module { get; set; }

        public DbSet<ConfigurationSell> ConfigurationSell { get; set; }

        public DbSet<ConfigurationPurchase> ConfigurationPurchase { get; set; }

        public DbSet<TransactionReceipt> TransactionReceipt { get; set; }

        public DbSet<Payroll> Payroll { get; set; }

        public DbSet<Payment> Paymen { get; set; }

        public DbSet<Company> Company { get; set; }

        public DbSet<ConfigurationReport> ConfigurationReports { get; set; }

        public DbSet<Transactions> Transactions { get; set; }

        public DbSet<TransactionsDetails> TransactionsDetails { get; set; }

        public DbSet<Concept> Concept { get; set; }

        public DbSet<Contact> Contacts { get; set; }

       

        public DbSet<Taxes> Taxes { get; set; }
       
        public DbSet<TypeRegister> TypeRegisters { get; set; }

        public DbSet<Journal> Journals { get; set; }

        public DbSet<JournaDetails> JournaDetails { get; set; }

        public DbSet<Numeration> Numerations { get; set; }

        public DbSet<Files> Files { get; set; }

        public DbSet<Sys_User> Sys_User { get; set; }

        public DbSet<LedgerAccount> LedgerAccounts { get; set; }

        public DbSet<BoxBalance> BoxBalances { get; set; }
    }
}
