using Fluent.Architecture.Validation;

namespace Fluent.Architecture.Sample.API.client_scope
{
    public class BairroValidation : FluentValidation<Bairro>
    {
        public override void Add(Bairro entity)
        {
            base.Add(entity);
        }
    }
}
