using Fluente.Arquitetura.Nucleo.Doc.Controllers;
using Fluente.Arquitetura.Nucleo.Doc.Interfaces;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;

namespace Fluente.Arquitetura
{
    public static class FluenteDocExtension
    {
        internal static string ApiBaseUrl { get; set; }

        public static string G(this string key)
        {
            return key == null ? null : (FluenteGlobalization?.Get(key) ?? key);
        }

        internal static IFluenteGlobalization FluenteGlobalization { get; set; }

        public static IMvcBuilder AddFluenteDoc(this IMvcBuilder builder, string apiBaseUrl)
        {
            ApiBaseUrl = apiBaseUrl;
            builder.Services.AddFluenteDoc();
            return builder;
        }

        public static IMvcBuilder AddFluenteGlobalizationDoc<T>(this IMvcBuilder builder) where T : IFluenteGlobalization, new()
        {
            FluenteGlobalization = new T();
            return builder;
        }

        public static IServiceCollection AddFluenteDoc(this IServiceCollection services)
        {
            services.Configure<StaticFileOptions>(opts =>
            {
                opts.FileProvider = new DocEmbeddedStaticFileProvider();
            });

            return services;
        }

        public static IApplicationBuilder UseFluenteDoc(this IApplicationBuilder app)
        {
            app.UseStaticFiles();
            return app;
        }
    }
}
