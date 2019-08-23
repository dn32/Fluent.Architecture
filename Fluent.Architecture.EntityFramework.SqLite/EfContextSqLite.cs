// ReSharper disable CommentTypo
using Microsoft.EntityFrameworkCore;

namespace Fluent.Architecture.EntityFramework.SqLite
{
    /// <inheritdoc />
    /// <summary>
    /// Contexto do EF no net Core
    /// </summary>
    [DbType(FluentDbType.SQLITE)]
    public class EfContextSqLite : EfContext
    {
        public EfContextSqLite(string connectionString) : base(connectionString)
        {
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlite(ConnectionString);
            base.OnConfiguring(optionsBuilder);
        }
    }
}
