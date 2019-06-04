using Fluent.Architecture.Exceptions.ValidationException;
using Fluent.Architecture.Validation;

namespace Fluent.Architecture.Sample.API.ClientScope
{
    public class ClientValidation : FluentValidation<Client>
    {
        public override void Add(Client client)
        {
            if (!client.Name.Contains(' '))
            {
                AddInconsistency(new FluentValidationException("Enter full name"));
            }

            base.Add(client);
        }
    }
}
