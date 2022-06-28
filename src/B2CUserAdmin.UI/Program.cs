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

            var identityServerApiClientConfiguration = builder.Configuration.GetSection("IdentityServerApiClient").Get<HttpConfiguration>();

            builder.Services.AddHttpClient(identityServerApiClientConfiguration.Name, client => client.BaseAddress = identityServerApiClientConfiguration.BaseAddress)
            .AddHttpMessageHandler<BaseAddressAuthorizationMessageHandler>()
            .AddHttpMessageHandler(sp => sp.GetRequiredService<AuthorizationMessageHandler>()
            .ConfigureHandler(
                authorizedUrls: new[] { identityServerApiClientConfiguration.BaseAddress?.ToString() }));

            // Supply HttpClient instances that include access tokens when making requests to the server project
            builder.Services.AddScoped(sp => sp.GetRequiredService<IHttpClientFactory>().CreateClient(identityServerApiClientConfiguration.Name));

            builder.Services.AddMsalAuthentication(options =>
            {
                builder.Configuration.Bind("AzureAdB2C", options.ProviderOptions.Authentication);
                // I'm asking for all the scopes up front. Maybe want to use ITokenProvider - _maybe_
                options.ProviderOptions.DefaultAccessTokenScopes.AddRange(identityServerApiClientConfiguration.Scopes ?? Array.Empty<string>());
            });
            builder.Services.AddBlazorApplicationInsights();
            builder.Services.AddServices(); // custom abstraction class for added project services
            builder.Services.AddBlazoredToast();
            await builder.Build().RunAsync();
        }
    }



}
