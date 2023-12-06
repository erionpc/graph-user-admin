using System;
using Azure.Identity;
using B2CUserAdmin.API.Models;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Microsoft.Graph;
using Microsoft.Identity.Client;

namespace B2CUserAdmin.API.Services
{
    public class GraphClientBase
    {
        protected string[] scopes = new string[] { "https://graph.microsoft.com/.default" };

        private GraphServiceClient _graphClient;

        protected GraphServiceClient GraphClient => _graphClient;

        protected AuthenticationConfig AppSettings;
        protected ILogger<GraphClientBase> Logger;

        protected GraphClientBase(ILogger<GraphClientBase> logger, AuthenticationConfig configuration)
        {
            AppSettings = configuration;
            Logger = logger;

            var credential = new ClientSecretCredential(AppSettings.TenantName, AppSettings.ClientId, AppSettings.ClientSecret);
            var tokenProvider = new TokenCredentialAuthProvider(credential);
            _graphClient = new GraphServiceClient(tokenProvider);
        }
    }
}
