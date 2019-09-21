using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Authorization;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.OpenApi.Models;
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
            //services.AddAuthorization(options =>
            //{
            //    options.DefaultPolicy = new AuthorizationPolicyBuilder()
            //        .RequireAuthenticatedUser()
            //        .RequireRole("MyScope")
            //        .Build();
            //});

            services
                .AddMvc()
                .AddNewtonsoftJson(options => options.SerializerSettings.ContractResolver = new CamelCasePropertyNamesContractResolver());
            services.AddSwaggerGen(c =>
            {
                c.EnableAnnotations();

                c.SwaggerDoc("v1", new OpenApiInfo
                {
                    Version = "v1",
                    Title = "API de integração",
                    Description = "API de integração",
                    Contact = new OpenApiContact { Name = "Fluent API", Url = new Uri("http://www.dn32.com.br") },
                    TermsOfService = new Uri("http://www.dn32.com.br/api/v1/termo.html"),
                });

                c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
                {
                    Description = "JWT Authorization header using the Bearer scheme. Example: \"Authorization: Bearer {token}\"",
                    Name = "Authorization",
                    In = ParameterLocation.Header,
                    Type = SecuritySchemeType.ApiKey
                });

                c.SchemaFilter<SwaggerFilterOutControllers>();
                c.DocumentFilter<CustomDocumentFilter>();


                //var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
                //var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
                //c.IncludeXmlComments(xmlPath);
                c.OperationFilter<AuthorizationHeaderParameterOperationFilter>();
            });


        }
        internal class SwaggerFilterOutControllers : ISchemaFilter
        {
            public void Apply(OpenApiSchema schema, SchemaFilterContext context)
            {
                // schema.vendorExtensions.Add("title", mfc.SystemType.Name);
            }
        }
        /// <summary>
        /// Classe AuthorizationHeaderParameterOperationFilter
        /// </summary>
        public class AuthorizationHeaderParameterOperationFilter : IOperationFilter
        {
            /// <summary>
            /// Construtor Apply
            /// </summary>
            /// <param name="operation">Operação</param>
            /// <param name="context">OperationFilterContext</param>
            public void Apply(OpenApiOperation operation, OperationFilterContext context)
            {
                var filterPipeline = context.ApiDescription.ActionDescriptor.FilterDescriptors;
                var isAuthorized = filterPipeline.Select(filterInfo => filterInfo.Filter).Any(filter => filter is AuthorizeFilter);
                var allowAnonymous = filterPipeline.Select(filterInfo => filterInfo.Filter).Any(filter => filter is IAllowAnonymousFilter);

                if (isAuthorized && !allowAnonymous)
                {
                    if (operation.Parameters == null)
                        operation.Parameters = new List<OpenApiParameter>();

                    operation.Parameters.Add(new OpenApiParameter
                    {
                        Name = "Authorization",
                        Description = "access token",
                        Required = true
                    });
                }
            }
        }
        public class CustomDocumentFilter : IDocumentFilter
        {
            public void Apply(OpenApiDocument swaggerDoc, DocumentFilterContext context)
            {
                var dic = new OpenApiPaths();
                var paths = swaggerDoc.Paths.OrderBy(e => e.Key).OrderBy(e => GetOrder(e.Value.Operations.First().Key)).ToList();
                paths.ForEach(x => dic.Add(x.Key, x.Value));
                swaggerDoc.Paths = dic;
            }

            private int GetOrder(OperationType opration)
            {
                switch (opration)
                {
                    case OperationType.Get:
                        return 1000;
                    case OperationType.Head:
                        return 2000;
                    case OperationType.Post:
                        return 3000;
                    case OperationType.Put:
                        return 4000;
                    case OperationType.Delete:
                        return 5000;
                    case OperationType.Options:
                        return 6000;
                    case OperationType.Patch:
                        return 7000;
                    case OperationType.Trace:
                        return 8000;
                    default:
                        return 9000;
                }
            }
        }

        public void Configure(IApplicationBuilder app)
        {
            //app.UseHsts();
            //app.UseHttpsRedirection();

            app.UseSwagger();
            app.UseSwaggerUI(s =>
            {
                s.SwaggerEndpoint("../swagger/v1/swagger.json", "Integração API v1.0");
            });

            app.UseStaticFiles();

            app.UseRouting();

            app.UseCors("default");

            //app.UseAuthentication();
            //app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapDefaultControllerRoute();//.RequireAuthorization();
            });

            ArchitectureInit.Setup(app.ApplicationServices);
        }
    }
}
