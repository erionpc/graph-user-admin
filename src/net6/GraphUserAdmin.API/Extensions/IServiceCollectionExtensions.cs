using GraphUserAdmin.API.Models;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GraphUserAdmin.API.Extensions
{
    public static class SwaggerConfiguration
    {
        public static IServiceCollection ConfigureSwagger(this IServiceCollection serviceCollection, AuthenticationConfig authenticationOptions)
        {
            serviceCollection.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo
                {
                    Version = "v1",
                    Title = "Graph User Admin API by erionpc",
                    Description = "Web API for managing Graph users",
                    TermsOfService = new Uri("https://example.com/terms"),
                    Contact = new OpenApiContact
                    {
                        Name = "ERIONPC",
                        Email = "erionpc@gmail.com",
                        Url = new Uri("https://erionpc.wordpress.com"),
                    },
                    License = new OpenApiLicense
                    {
                        Name = "Use under license",
                        Url = new Uri("https://www.gnu.org/licenses/lgpl-3.0.en.html"),
                    }
                });
                c.AddSecurityDefinition("aad-jwt", new OpenApiSecurityScheme
                {
                    Type = SecuritySchemeType.OAuth2,
                    Flows = new OpenApiOAuthFlows
                    {
                        Implicit = new OpenApiOAuthFlow
                        {
                            AuthorizationUrl = new Uri(authenticationOptions.AuthorizationUrl!),
                            Scopes = Constants.DelegatedPermissions.All.ToDictionary(p =>
                                $"{authenticationOptions.ApplicationIdUri}/{p}")
                        }
                    }
                });
                c.OperationFilter<OAuthSecurityRequirementOperationFilter>();
            });

            return serviceCollection;
        }
    }

    internal class OAuthSecurityRequirementOperationFilter : IOperationFilter
    {
        private readonly string _appIdUri;

        public OAuthSecurityRequirementOperationFilter(IOptions<AuthenticationConfig> authenticationOptions)
        {
            _appIdUri = authenticationOptions.Value.ApplicationIdUri!;
        }

        public void Apply(OpenApiOperation operation, OperationFilterContext context)
        {
            operation.Security.Add(new OpenApiSecurityRequirement
            {
                {
                    new OpenApiSecurityScheme
                    {
                        Reference = new OpenApiReference
                        {
                            Type = ReferenceType.SecurityScheme,
                            Id = "aad-jwt"
                        },
                        UnresolvedReference = true
                    },
                    Constants.DelegatedPermissions.All.Select( x=> $"{_appIdUri}/{x}" ).ToArray()
                }
            });
        }
    }
}
