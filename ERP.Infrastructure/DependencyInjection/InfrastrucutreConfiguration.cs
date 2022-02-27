using ERP.Infrastructure.DBContexts;
using ERP.Infrastructure.Repositories;
using ERP.Services.Interfaces;

using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace ERP.Infrastructure.DependencyInjection
{
    public static class InfrastrucutreConfiguration
    {


        public static void AddInfrastrucutreCollections(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddTransient(typeof(IGenericRepository<>), typeof(GenericRepository<>));

            services.AddDbContext<ApplicationDbContext>(options =>
          options.UseSqlServer(
              configuration.GetConnectionString("DefaultConnection"),
              b => b.MigrationsAssembly(typeof(ApplicationDbContext).Assembly.FullName)));
        }
     

    }
}
