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
using GraphUserAdmin.API.Abstractions;
using GraphUserAdmin.API.Services;
using GraphUserAdmin.API.Configuration;
using GraphUserAdmin.API.Extensions;

namespace GraphUserAdmin.API
{
    public class Startup(IConfiguration configuration)
    {
        public IConfiguration Configuration { get; } = configuration;

        private AppConfiguration appConfiguration = new();

        public void ConfigureServices(IServiceCollection services)
        {
            appConfiguration = Configuration.Get<AppConfiguration>()!;
            services.AddSingleton(appConfiguration);

            services.ConfigureSwagger(appConfiguration.TenantConfig!);
            
            services.AddSingleton<IUserService, UserService>();

            services.AddRazorPages();
            services.AddControllers();
            
            services.AddHttpClient();
            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                .AddJwtBearer(JwtBearerDefaults.AuthenticationScheme, options => Configuration.Bind("TenantConfig", options));

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
                    o.SwaggerEndpoint("/swagger/v1/swagger.json", "Graph Users Admin API");
                    o.OAuthClientId(appConfiguration.TenantConfig!.ClientId);
                    o.DefaultModelRendering(Swashbuckle.AspNetCore.SwaggerUI.ModelRendering.Example);
                    o.DocExpansion(Swashbuckle.AspNetCore.SwaggerUI.DocExpansion.None);
                    o.DisplayRequestDuration();
                    o.DefaultModelsExpandDepth(-1);
                });

                app.UseDeveloperExceptionPage();
            }

            app.UseCors(policy =>
                policy.WithOrigins(appConfiguration.CorsConfig!.AllowedOrigins!)
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
