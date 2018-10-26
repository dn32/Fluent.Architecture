using Fluent.Architecture.Model;
using Fluent.Architecture.Sample.Core.Escopos.Departamentos;
using Fluent.Architecture.Services;

namespace Fluent.Architecture.Sample.Core.Escopos.Usuario
{
    public class DepartamentoService : FluentService<Departamento>
    {
        //public void EntityChanged(Departamento entitySaved, Departamento entityChanged)
        //{
        //}

        //public Expression<Func<Departamento, Departamento, bool>> EventCondition()
        //{
        //    return (entitySaved, entityChanged) => entitySaved.Id != entityChanged.Id;
        //}

        protected override void ChangingEvent(FluentEventEntity eventEntity)
        {
            base.ChangingEvent(eventEntity);
        }

        protected override void ChangedEvent(FluentEventEntity eventEntity)
        {
            base.ChangedEvent(eventEntity);
        }

        protected override void ChangedAsyncEvent(FluentEventEntity eventEntity)
        {
            base.ChangedAsyncEvent(eventEntity);
        }
    }

    //public class DepartamentoIdAlteradoEvento : IFluentChangeEvent<Departamento>
    //{
    //    public Expression<Func<Departamento, Departamento, bool>> EventCondition()
    //    {
    //        return (entitySaved, entityChanged) => entitySaved.Id != entityChanged.Id;
    //    }

    //    public string EventName()
    //    {
    //        return nameof(DepartamentoIdAlteradoEvento);
    //    }
    //}
}
