using B2CUserAdmin.UI.Services.Users;
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

namespace B2CUserAdmin.UI
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            var builder = WebAssemblyHostBuilder.CreateDefault(args);
            builder.RootComponents.Add<App>("#app");

            var apiClientConfiguration = builder.Configuration.GetSection("ApiClient").Get<HttpConfiguration>();

            builder.Services.AddHttpClient(apiClientConfiguration.Name!, client => client.BaseAddress = apiClientConfiguration.BaseAddress)
                            .AddHttpMessageHandler<BaseAddressAuthorizationMessageHandler>()
                            .AddHttpMessageHandler(sp => sp.GetRequiredService<AuthorizationMessageHandler>()
                            .ConfigureHandler(authorizedUrls: new[] { apiClientConfiguration.BaseAddress?.ToString() }));

            builder.Services.AddScoped(sp => sp.GetRequiredService<IHttpClientFactory>().CreateClient(apiClientConfiguration.Name!));

            builder.Services.AddMsalAuthentication(options =>
            {
                builder.Configuration.Bind("AzureAdB2C", options.ProviderOptions.Authentication);
                options.ProviderOptions.DefaultAccessTokenScopes.AddRange(apiClientConfiguration.Scopes ?? Array.Empty<string>());
            });
            builder.Services.AddBlazorApplicationInsights();
            builder.Services.AddSingleton<UserService>();
            builder.Services.AddSingleton(apiClientConfiguration);

            builder.Services.AddBlazoredToast();
            await builder.Build().RunAsync();
        }
    }
}
