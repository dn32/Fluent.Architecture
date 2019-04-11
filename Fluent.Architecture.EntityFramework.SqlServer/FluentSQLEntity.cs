using Fluent.Architecture.Core.Attributes;
using Fluent.Architecture.Enumerator;
using Fluent.Architecture.Model;

namespace Fluent.Architecture.EntityFramework.SqlServer
{
    [DbType(FluentDbType.SQL_SERVER)]
    public abstract class FluentSQLEntity : FluentEntity
    {
    }
}
