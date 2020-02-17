using Fluente.Arquitetura.Nucleo.Models;

namespace Fluente.Arquitetura.EntityFramework.MemoryDatabase
{
    [DbType(FluenteDbType.MEMORY)]
    public abstract class FluenteMySQLEntity : FluenteEntidade
    {
    }
}
