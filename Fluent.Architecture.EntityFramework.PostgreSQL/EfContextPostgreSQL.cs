// ReSharper disable CommentTypo

using Microsoft.EntityFrameworkCore;

namespace dn32.infra.EntityFramework.PostgreSQL
{
    /// <inheritdoc />
    /// <summary>
    /// Contexto do EF no net Core
    /// </summary>
    [DbType(DnDbType.POSTGREE_SQL)]
    public class EfContextPostgreSQL : EfContext
    {
        public EfContextPostgreSQL(string connectionString) : base(connectionString)
        {
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseNpgsql(ConnectionString);
            base.OnConfiguring(optionsBuilder);
        }
    }
}
