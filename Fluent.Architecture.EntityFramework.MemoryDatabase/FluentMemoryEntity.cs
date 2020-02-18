using Fluente.Arquitetura.Base.Models;

namespace Fluente.Arquitetura.EntityFramework.MemoryDatabase
{
    [DbType(FluenteDbType.MEMORY)]
    public abstract class FluenteMySQLEntity : FluenteEntidade
    {
    }
}
