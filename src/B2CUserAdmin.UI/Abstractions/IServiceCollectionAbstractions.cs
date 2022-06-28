using B2CUserAdmin.UI.Services;
using B2CUserAdmin.UI.Services.Users;

namespace Microsoft.Extensions.DependencyInjection
{
    public static class IServiceCollectionAbstractions
    {
        public static IServiceCollection AddServices(this IServiceCollection services)
        {
            services.AddSingleton<UserService>();
            services.AddScoped<ClipboardService>();
            return services;
        }

    }
}
