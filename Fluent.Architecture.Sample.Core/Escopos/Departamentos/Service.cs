using Fluent.Architecture.Model;
using Fluent.Architecture.Sample.Core.Escopos.Departamentos;
using Fluent.Architecture.Services;

namespace Fluent.Architecture.Sample.Core.Escopos.Usuario
{
    public class DepartamentoService : FluentService<Departamento>
    {
        protected override void ChangingEvent(FluentEventEntity eventEntity)
        {
        }

        protected override void ChangedEvent(FluentEventEntity eventEntity)
        {
        }

        protected override void ChangedAsyncEvent(FluentEventEntity eventEntity)
        {
        }
    }
}
