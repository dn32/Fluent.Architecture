// ReSharper disable CommentTypo
using Microsoft.EntityFrameworkCore;

namespace Fluente.Arquitetura.EntityFramework.Oracle
{
    /// <inheritdoc />
    /// <summary>
    /// Contexto do EF no net Core
    /// </summary>
    [DbType(FluenteDbType.ORACLE)]
    public class EfContextOracle : EfContext
    {

        public EfContextOracle(string connectionString) : base(connectionString)
        {
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
#if !NETCOREAPP3_1
            optionsBuilder.UseOracle(ConnectionString);
#endif
            base.OnConfiguring(optionsBuilder);
        }
    }
}
