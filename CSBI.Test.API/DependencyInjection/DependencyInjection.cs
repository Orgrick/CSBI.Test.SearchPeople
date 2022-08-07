using CSBI.Test.Abstractions.Services;
using CSBI.Test.Application.Services;
using CSBI.Test.DataAccess.EF;
using Microsoft.EntityFrameworkCore;

namespace CSBI.Test.API.DependencyInjection
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddApplication(this IServiceCollection services, Action<AppOptions> options)
        {
            var appOpt = new AppOptions();
            options.Invoke(appOpt);

            services.AddScoped<IClientService, ClientService>();
            services.AddDbContext<ClientsContext>(opt => opt.UseSqlServer(appOpt.ConnectionString));

            return services;
        }
    }
}
