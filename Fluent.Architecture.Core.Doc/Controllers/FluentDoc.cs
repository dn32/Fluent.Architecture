using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;

namespace Fluent.Architecture.Core.Doc.Controllers
{
    public static class FluentDoc
    {
        public static void AddFluentDoc(this IServiceCollection services)
        {
            services.Configure<StaticFileOptions>(opts =>
            {
                opts.FileProvider = new DocEmbeddedStaticFileProvider();
            });
        }
    }
}
