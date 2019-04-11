using Fluent.Architecture.Controllers;

namespace Fluent.Architecture.Sample
{
    public class ControllerTeste : FluentController<EntidadeTeste>
    {
        public void Add(EntidadeTeste entity)
        {
            Service.Add(entity);
        }
    }
}
