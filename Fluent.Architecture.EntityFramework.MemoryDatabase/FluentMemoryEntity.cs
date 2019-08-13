using Fluent.Architecture.Entities;

namespace Fluent.Architecture.EntityFramework.MemoryDatabase
{
    [DbType(FluentDbType.MEMORY)]
    public abstract class FluentMySQLEntity : FluentEntity
    {
    }
}
