using dn32.infra.dados;

namespace Fluente.Arquitetura.EntityFramework.MemoryDatabase
{
    [DbType(FluenteDbType.MEMORY)]
    public abstract class FluenteMySQLEntity : FluenteEntidade
    {
    }
}
