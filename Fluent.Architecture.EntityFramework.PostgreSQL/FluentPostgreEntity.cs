using Fluent.Architecture.Entities;

namespace Fluent.Architecture.EntityFramework.PostgreSQL
{
    [DbType(FluentDbType.POSTGREE_SQL)]
    public abstract class FluentPostgreEntity : FluentEntity
    {
    }
}
