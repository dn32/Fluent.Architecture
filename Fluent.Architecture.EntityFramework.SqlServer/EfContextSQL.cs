// ReSharper disable CommentTypo

namespace Fluent.Architecture.EntityFramework.SqlServer
{
    /// <inheritdoc />
    /// <summary>
    /// Contexto do EF no net Core
    /// </summary>
    [DbType(FluentDbType.SQL_SERVER)]
    public class EfContextSQLServer : EfContext
    {
        public EfContextSQLServer(string connectionString) : base(connectionString)
        {
        }

#if !NET461
        protected override void OnConfiguring(Microsoft.EntityFrameworkCore.DbContextOptionsBuilder optionsBuilder)
        {
            Microsoft.EntityFrameworkCore.SqlServerDbContextOptionsExtensions.UseSqlServer(optionsBuilder, ConnectionString);
        }
#endif
    }
}
