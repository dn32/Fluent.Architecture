// ReSharper disable CommentTypo

#if NET461

#else
using Microsoft.EntityFrameworkCore;

#endif

namespace Fluent.Architecture.EntityFramework.PostgreSQL
{
    /// <inheritdoc />
    /// <summary>
    /// Contexto do EF no net Core
    /// </summary>
    [DbType(FluentDbType.POSTGREE_SQL)]
    public class EfContextPostgreSQL : EfContext
    {
        public EfContextPostgreSQL(string connectionString) : base(connectionString)
        {
        }

#if !NET461
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseNpgsql(ConnectionString);
        }
#endif
    }
}
