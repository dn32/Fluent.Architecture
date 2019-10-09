using Fluent.Architecture.Core.Doc.Controllers;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;

namespace Fluent.Architecture
{
    public static class FluentDocExtension
    {
        internal static string ApiBaseUrl { get; set; }

        public static void AddFluentDoc(this IMvcBuilder builder, string apiBaseUrl)
        {
            ApiBaseUrl = apiBaseUrl;
            builder.Services.AddFluentDoc();
        }

        public static void AddFluentDoc(this IServiceCollection services)
        {
            services.Configure<StaticFileOptions>(opts =>
            {
                opts.FileProvider = new DocEmbeddedStaticFileProvider();
            });
        }

        public static void UseFluentDoc(this IApplicationBuilder app)
        {
            app.UseStaticFiles();
        }
    }
}
