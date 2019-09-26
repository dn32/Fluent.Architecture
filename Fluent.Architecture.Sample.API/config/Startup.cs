using Fluent.Architecture.Controllers;
using Fluent.Architecture.EntityFramework;
using Fluent.Architecture.EntityFramework.MySQL;
using Fluent.Architecture.EntityFramework.SqLite;
using Fluent.Architecture.EntityFramework.SqlServer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Authorization;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using Swashbuckle.AspNetCore.SwaggerGen;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;

namespace Fluent.Architecture.Sample.API
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        public void ConfigureServices(IServiceCollection services)
        {
            var jsonSerializerSettings = new JsonSerializerSettings
            {
                ContractResolver = new CamelCasePropertyNamesContractResolver()
            };

            services
                .AddMvc()
                .AddNewtonsoftJson(x => x.SerializerSettings.ContractResolver = new CamelCasePropertyNamesContractResolver())
                .AddFluentArchitecture(jsonSerializerSettings)
                .UseEntityFramework()
                .AddConnectionString("Data Source=sample.db;", createDatabaseIfNotExists: true, typeof(EfContextSqLite))
                .AddConnectionString("Data Source=51.83.33.154;Initial Catalog=sample;User ID=sa;Password=miGcp1926*;", createDatabaseIfNotExists: true, typeof(EfContextSQLServer))
                .SetGenericControllerType(typeof(FluentAPIController<>))
                .Build()
                .AddFluentDoc();
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseStaticFiles();
            app.UseRouting();
            app.UseCors("default");
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapDefaultControllerRoute();
            });

            app.UseFluentDoc();
        }
    }
}
