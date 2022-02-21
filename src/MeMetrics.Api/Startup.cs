using System;
using System.IO;
using System.Reflection;
using Lamar;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using MeMetrics.Api.Configurations.Extensions;
using CorrelationId;
using MeMetrics.Api.Middleware.ExceptionHandling;
using Microsoft.AspNetCore.Mvc.ApiExplorer;
using MeMetrics.Infrastructure;
using System.Collections.Generic;
using MeMetrics.Domain.Models.Calls;
using Newtonsoft.Json;

namespace MeMetrics.Api
{
    public class Startup
    {
        public IConfiguration Configuration { get; }
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureContainer(ServiceRegistry services)
        {
            services.AddCorrelationId();
            services.AddHttpContextAccessor();
            services.AddMvc();
            services.AddVersionedApiExplorer(options => options.GroupNameFormat = "'v'VVV");
            services.AddApiVersioning(o =>
            {
                o.ReportApiVersions = true;
                o.DefaultApiVersion = new ApiVersion(1, 0);
                o.AssumeDefaultVersionWhenUnspecified = true;
            });
            services.AddOptions();
            services.AddMemoryCache();
            services.AddHttpClient();
            services.AddSwagger(Configuration);
            services.AddDependencyInjection(Configuration);
            services.AddControllers();
            services.AddHealthChecks();
            services.AddApiAuthentication(Configuration);
            services.AddSwaggerGen(c =>
            {
                // This coupled with the properties in the csproj allow the swagger page to show additional comments for methods
                var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
                var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
                c.IncludeXmlComments(xmlPath);
            });
            services.AddCors();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IApiVersionDescriptionProvider provider)
        {
            var environmentConfiguration = new EnvironmentConfiguration();
            Configuration.Bind(environmentConfiguration);
            app.UseCors(o => o.AllowAnyHeader().AllowAnyMethod().WithOrigins(new[] {environmentConfiguration.ALLOWED_ORIGIN}));
            app.UseMiddleware<ExceptionMiddleware>();
            app.UseSwaggerDocumentation(provider);
            app.UseRouting();
            app.UseAuthentication();
            app.UseAuthorization();
            app.UseHealthChecks("/health");
            app.UseEndpoints(endpoints => {
                endpoints.MapControllers();
            });
        }
    }
}
