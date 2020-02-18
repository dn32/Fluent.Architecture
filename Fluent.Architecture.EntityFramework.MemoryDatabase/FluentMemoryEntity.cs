using dn32.infra.dados;

namespace dn32.infra.EntityFramework.MemoryDatabase
{
    [DbType(DnDbType.MEMORY)]
    public abstract class DnMySQLEntity : DnEntidade
    {
    }
}
