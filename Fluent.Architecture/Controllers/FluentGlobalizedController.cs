using Fluent.Architecture.Model;
using Fluent.Architecture.Services;

namespace Fluent.Architecture.Controllers
{
    public abstract class FluentGlobalizedController<T> : FluentController<T> where T : FluentGlobalizedEntity
    {
        public new FluentGlobalizedService<T> Service => base.Service as FluentGlobalizedService<T>;
    }
}