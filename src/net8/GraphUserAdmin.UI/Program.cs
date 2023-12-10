using GraphUserAdmin.UI.Services.Users;
using BlazorApplicationInsights;
using Blazored.Toast;
using Microsoft.AspNetCore.Components.WebAssembly.Authentication;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using GraphUserAdmin.UI.Configuration;

namespace GraphUserAdmin.UI
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            var builder = WebAssemblyHostBuilder.CreateDefault(args);
            builder.RootComponents.Add<App>("#app");

            var appConfiguration = builder.Configuration.Get<AppConfiguration>();

            builder.Services.AddHttpClient(appConfiguration!.ApiClientConfig!.Name!, client => client.BaseAddress = appConfiguration.ApiClientConfig!.BaseAddress)
                            .AddHttpMessageHandler<BaseAddressAuthorizationMessageHandler>()
                            .AddHttpMessageHandler(sp => sp.GetRequiredService<AuthorizationMessageHandler>()
                            .ConfigureHandler(authorizedUrls: new[] { appConfiguration.ApiClientConfig!.BaseAddress!.ToString() }));

            builder.Services.AddScoped(sp => sp.GetRequiredService<IHttpClientFactory>().CreateClient(appConfiguration.ApiClientConfig!.Name!));

            builder.Services.AddMsalAuthentication(options =>
            {
                builder.Configuration.Bind("TenantConfig", options.ProviderOptions.Authentication);
                options.ProviderOptions.DefaultAccessTokenScopes.AddRange(appConfiguration.ApiClientConfig.Scopes ?? []);
            });
            builder.Services.AddBlazorApplicationInsights();
            builder.Services.AddSingleton<UserService>();
            builder.Services.AddSingleton(appConfiguration);

            builder.Services.AddBlazoredToast();
            await builder.Build().RunAsync();
        }
    }
}
