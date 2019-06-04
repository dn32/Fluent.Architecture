using Fluent.Architecture.Services;

namespace Fluent.Architecture.Sample.API.client_scope
{
    public class BairroService : FluentService<Bairro>
    {
        public override Bairro Add(Bairro entity)
        {
            return base.Add(entity);
        }
    }
}
