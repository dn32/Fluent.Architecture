using Fluent.Architecture.Core.Attributes;
using Fluent.Architecture.Enumerator;
using Fluent.Architecture.Model;

namespace Fluent.Architecture.EntityFramework.PostgreSQL
{
    [DbType(FluentDbType.POSTGREE_SQL)]
    public abstract class FluentPostgreEntity : FluentEntity
    {
    }
}
