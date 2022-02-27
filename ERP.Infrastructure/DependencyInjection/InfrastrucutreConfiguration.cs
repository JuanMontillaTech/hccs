using ERP.Infrastructure.Repositories;
using ERP.Services.Interfaces;

using Microsoft.Extensions.DependencyInjection;

namespace ERP.Infrastructure.DependencyInjection
{
    public static class InfrastrucutreConfiguration
    {


        public static void AddInfrastrucutreCollections(this IServiceCollection services)
        {
            services.AddTransient(typeof(IGenericRepository<>), typeof(GenericRepository<>));


        }
     

    }
}
