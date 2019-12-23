// ReSharper disable CommentTypo
using Microsoft.EntityFrameworkCore;

namespace Fluent.Architecture.EntityFramework.Oracle
{
    /// <inheritdoc />
    /// <summary>
    /// Contexto do EF no net Core
    /// </summary>
    [DbType(FluentDbType.ORACLE)]
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
