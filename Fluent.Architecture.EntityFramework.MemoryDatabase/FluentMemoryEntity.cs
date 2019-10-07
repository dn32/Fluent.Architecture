using Fluent.Architecture.Core.Models;

namespace Fluent.Architecture.EntityFramework.MemoryDatabase
{
    [DbType(FluentDbType.MEMORY)]
    public abstract class FluentMySQLEntity : FluentEntity
    {
    }
}
