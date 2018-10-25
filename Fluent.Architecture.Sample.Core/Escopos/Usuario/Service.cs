using Fluent.Architecture.Interfaces;
using Fluent.Architecture.Model;
using Fluent.Architecture.Sample.Core.Escopos.Departamentos;
using Fluent.Architecture.Services;
using System;
using System.Linq.Expressions;

namespace Fluent.Architecture.Sample.Core.Escopos.Usuario
{
    public class DepartamentoService : FluentService<Departamento>
    {
        public void EntityChanged(Departamento entitySaved, Departamento entityChanged)
        {
        }

        public Expression<Func<Departamento, Departamento, bool>> EventCondition()
        {
            return (entitySaved, entityChanged) => entitySaved.Id != entityChanged.Id;
        }

        public string EventName()
        {
            return "IdAlterado";
        }

        protected override void CalledEvent(BaseEvent _event)
        {
            if(_event.EventName == nameof(DepartamentoIdAlteradoEvento))
            {

            }

            base.CalledEvent(_event);
        }
    }

    public class DepartamentoIdAlteradoEvento : IFluentChangeEvent<Departamento>
    {
        public Expression<Func<Departamento, Departamento, bool>> EventCondition()
        {
            return (entitySaved, entityChanged) => entitySaved.Id != entityChanged.Id;
        }

        public string EventName()
        {
            return nameof(DepartamentoIdAlteradoEvento);
        }
    }
}
