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
            services.AddTransient< IDirectSql , DirectSql>();         
            services.AddTransient<ITransactionService, TransactionService>();
            services.AddTransient<INumerationService, NumerationServices>();
            services.AddTransient<IRequestDgiiService, RequestDgiiService>();
            services.AddTransient<IAccountingProcess, AccountingProcess>();  
            services.AddTransient<INumerationHelper, NumerationHelper>();
            services.AddTransient<IImportService, ImportService>();
         
        }
    }
}
