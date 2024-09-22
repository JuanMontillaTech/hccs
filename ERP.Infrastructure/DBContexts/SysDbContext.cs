 
using System;
using System.Threading;
using System.Threading.Tasks;
using ERP.Domain.Entity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
 
using ERP.Services.Interfaces;
using Microsoft.EntityFrameworkCore.ChangeTracking;

namespace ERP.Infrastructure.DBContexts;

public class SysDbContext : DbContext
{
   
    private readonly ICurrentUser _getCurrentUser;
    private IConfiguration Config { get; } 
    public SysDbContext(DbContextOptions<SysDbContext> options, IConfiguration configuration , ICurrentUser getCurrentUser ) : base(options)
    {
        Config = configuration;

        _getCurrentUser = getCurrentUser;

    }
    
    public DbSet<SysCompany> SysCompany { get; set; }
    
    public DbSet<SysUserCompany> SysUserCompany { get; set; } 
    
    public DbSet<SysUser> SysUser { get; set; } 
    
    public DbSet<Form> Form { get; set; }
    public DbSet<DataWay> DataWay { get; set; }
   
    public DbSet<Formfields> Formfields { get; set; }
    public DbSet<FormGrid> FormGrid { get; set; } 
    public DbSet<FormRule> FormRule { get; set; } 
    public DbSet<Section> Section { get; set; }
    public DbSet<Module> Module { get; set; }
    
    

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)  =>
        optionsBuilder.UseSqlServer(Config.GetConnectionString("SysDefaultConnection"));
 

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

}