using dn32.infra.dados;

namespace dn32.infra.EntityFramework.MemoryDatabase
{
    [DbType(FluenteDbType.MEMORY)]
    public abstract class FluenteMySQLEntity : FluenteEntidade
    {
    }
}
