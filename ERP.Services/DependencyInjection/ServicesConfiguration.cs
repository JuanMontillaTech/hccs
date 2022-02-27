using ERP.Services.Implementations;
using ERP.Services.Interfaces;

using Microsoft.Extensions.DependencyInjection;


namespace ERP.Services.DependencyInjection
{
    public static class ServicesConfiguration
    {
        public static void AddAplicationServices(this IServiceCollection services)
        {
            services.AddTransient<IAWS, AWS>();
            services.AddTransient<ISecurityService, SecurityService>();


        }
    }
}
