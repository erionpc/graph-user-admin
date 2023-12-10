using System;
using Azure.Identity;
using GraphUserAdmin.API.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Microsoft.Graph;
using Microsoft.Identity.Client;

namespace GraphUserAdmin.API.Services
{
    public class GraphClientBase
    {
        private readonly GraphServiceClient _graphClient;

        protected GraphServiceClient GraphClient => _graphClient;
        protected AppConfiguration AppSettings;
        protected ILogger<GraphClientBase> Logger;

        protected GraphClientBase(ILogger<GraphClientBase> logger, AppConfiguration configuration)
        {
            AppSettings = configuration;
            Logger = logger;

            var credential = new ClientSecretCredential(
                AppSettings.GraphClientConfig!.TenantId, 
                AppSettings.GraphClientConfig!.ClientId,
                AppSettings.GraphClientConfig!.ClientSecret);

            _graphClient = new GraphServiceClient(
                credential, 
                configuration.GraphClientConfig!.Scopes, 
                baseUrl: configuration.GraphClientConfig!.GraphApiBaseUrl);
        }
    }
}
