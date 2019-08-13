using Fluent.Architecture.Entities;

namespace Fluent.Architecture.EntityFramework.SqLite
{
    [DbType(FluentDbType.SQLITE)]
    public abstract class FluentSqLiteEntity : FluentEntity
    {
    }
}
