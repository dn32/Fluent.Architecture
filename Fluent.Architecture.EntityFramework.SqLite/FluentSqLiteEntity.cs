using Fluent.Architecture.Core.Models;

namespace Fluent.Architecture.EntityFramework.SqLite
{
    [DbType(FluentDbType.SQLITE)]
    public abstract class FluentSqLiteEntity : FluentEntity
    {
    }
}
