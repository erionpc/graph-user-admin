using B2CUserAdmin.API.Models;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace B2CUserAdmin.API.Extensions
{
    public static class SwaggerConfiguration
    {
        public static IServiceCollection ConfigureSwagger(this IServiceCollection serviceCollection, AuthenticationConfig authenticationOptions)
        {
            // Register the Swagger generator, defining 1 or more Swagger documents
            serviceCollection.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo
                {
                    Version = "v1",
                    Title = "Azure B2C Identity store API by Kocho",
                    Description = "Web API for managing claims used by Azure B2C",
                    TermsOfService = new Uri("https://example.com/terms"),
                    Contact = new OpenApiContact
                    {
                        Name = "Kocho Ltd",
                        Email = "info@kocho.co.uk",
                        Url = new Uri("https://kocho.co.uk"),
                    },
                    License = new OpenApiLicense
                    {
                        Name = "Use under LICX",
                        Url = new Uri("https://example.com/license"),
                    }
                });
                c.AddSecurityDefinition("aad-jwt", new OpenApiSecurityScheme
                {
                    Type = SecuritySchemeType.OAuth2,
                    Flows = new OpenApiOAuthFlows
                    {

                        Implicit = new OpenApiOAuthFlow
                        {
                            AuthorizationUrl = new Uri(authenticationOptions.AuthorizationUrl),
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
            _appIdUri = authenticationOptions.Value.ApplicationIdUri;
        }

        public void Apply(OpenApiOperation operation, OperationFilterContext context)
        {
            // Get custom attributes on action and controller

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
