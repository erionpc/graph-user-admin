using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Microsoft.OpenApi.Models;
using Microsoft.Identity.Web;
using Microsoft.Net.Http.Headers;
using Microsoft.AspNetCore.Http;
using B2CUserAdmin.API.Abstractions;
using B2CUserAdmin.API.Services;
using B2CUserAdmin.API.Models;

namespace B2CUserAdmin.API
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        private AuthenticationConfig authenticationOptions = new();

        public void ConfigureServices(IServiceCollection services)
        {
            authenticationOptions = Configuration.GetSection("AzureAd").Get<AuthenticationConfig>();
            services.AddSingleton(authenticationOptions);            

            services.ConfigureSwagger(authenticationOptions);
            
            services.AddTransient<IUserService, UserService>();

            services.AddRazorPages();
            services.AddControllers();
            
            services.AddHttpClient();
            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                .AddJwtBearer(o =>
                {
                    o.Authority = authenticationOptions.Authority;
                    o.Audience = authenticationOptions.ClientId;
                });

            services.AddAuthorization();
            services.AddSwaggerGen();
            services.AddApplicationInsightsTelemetry();
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            app.UseSwagger();

            if (env.IsDevelopment())
            {
                app.UseSwaggerUI(o =>
                {
                    o.SwaggerEndpoint("/swagger/v1/swagger.json", "AAD B2C Admin API");
                    o.OAuthClientId(authenticationOptions.ClientId);
                    o.DefaultModelRendering(Swashbuckle.AspNetCore.SwaggerUI.ModelRendering.Example);
                    o.DocExpansion(Swashbuckle.AspNetCore.SwaggerUI.DocExpansion.None);
                    o.DisplayRequestDuration();
                    o.DefaultModelsExpandDepth(-1);
                });

                app.UseDeveloperExceptionPage();
            }

            var corsOrigins = Configuration.GetSection("Cors:Origins").Get<string[]>();
            app.UseCors(policy =>
                policy.WithOrigins(corsOrigins)
                .AllowAnyHeader()
                .AllowAnyMethod()
                .AllowCredentials());

            app.UseHttpsRedirection();

            app.Use(async (context, next) =>
            {
                if (context.Request.Headers.TryGetValue("X-Forwarded-Host", out var originatingHost))
                {
                    context.Request.Host = new HostString(originatingHost!);
                }
                await next();
            });

            app.UseBlazorFrameworkFiles();
            app.UseStaticFiles();
            app.UseStaticFiles();

            app.UseRouting();
            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers()
                         .RequireAuthorization();

                endpoints.MapRazorPages();

                endpoints.MapFallbackToFile("{*path:nonfile}", "index.html");
            });
        }
    }
}
