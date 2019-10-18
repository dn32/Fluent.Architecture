// ReSharper disable CommentTypo
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Fluent.Architecture.EntityFramework.SqLite
{
    /// <inheritdoc />
    /// <summary>
    /// Contexto do EF no net Core
    /// </summary>
    [DbType(FluentDbType.SQLITE)]
    public class EfContextSqLite : EfContext
    {
        public static readonly LoggerFactory _myLoggerFactory = new LoggerFactory(new[] { new Microsoft.Extensions.Logging.Debug.DebugLoggerProvider() });
     
        public EfContextSqLite(string connectionString) : base(connectionString)
        {
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlite(ConnectionString);

#if DEBUG
            optionsBuilder
                .UseLoggerFactory(_myLoggerFactory)
                .EnableSensitiveDataLogging();
#endif

            base.OnConfiguring(optionsBuilder);
        }
    }
}
