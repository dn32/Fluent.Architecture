using Fluent.Architecture.Core.Attributes;
using Fluent.Architecture.Enumerator;
using Fluent.Architecture.Model;

namespace Fluent.Architecture.EntityFramework.MySQL
{
    [DbType(FluentDbType.MYSQL)]
    public abstract class FluentMySQLEntity : FluentEntity
    {
    }
}
