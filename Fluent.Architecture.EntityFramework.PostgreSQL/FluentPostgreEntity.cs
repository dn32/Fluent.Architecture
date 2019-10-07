using Fluent.Architecture.Core.Models;

namespace Fluent.Architecture.EntityFramework.PostgreSQL
{
    [DbType(FluentDbType.POSTGREE_SQL)]
    public abstract class FluentPostgreEntity : FluentEntity
    {
    }
}
