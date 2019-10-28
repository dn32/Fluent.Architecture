using Fluent.Architecture.Core.Doc.Controllers;
using Fluent.Architecture.Core.Doc.Interfaces;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;

namespace Fluent.Architecture
{
    public static class FluentDocExtension
    {
        internal static string ApiBaseUrl { get; set; }

        public static string G(this string key)
        {
            return key == null ? null : (FluentGlobalization?.Get(key) ?? key);
        }

        internal static IFluentGlobalization FluentGlobalization { get; set; }

        public static IMvcBuilder AddFluentDoc(this IMvcBuilder builder, string apiBaseUrl)
        {
            ApiBaseUrl = apiBaseUrl;
            builder.Services.AddFluentDoc();
            return builder;
        }

        public static IMvcBuilder AddFluentGlobalizationDoc<T>(this IMvcBuilder builder) where T : IFluentGlobalization, new()
        {
            FluentGlobalization = new T();
            return builder;
        }

        public static IServiceCollection AddFluentDoc(this IServiceCollection services)
        {
            services.Configure<StaticFileOptions>(opts =>
            {
                opts.FileProvider = new DocEmbeddedStaticFileProvider();
            });

            return services;
        }

        public static IApplicationBuilder UseFluentDoc(this IApplicationBuilder app)
        {
            app.UseStaticFiles();
            return app;
        }
    }
}
