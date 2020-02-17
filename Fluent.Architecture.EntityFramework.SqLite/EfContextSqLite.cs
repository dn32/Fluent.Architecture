// ReSharper disable CommentTypo
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Fluente.Arquitetura.EntityFramework.SqLite
{
    /// <inheritdoc />
    /// <summary>
    /// Contexto do EF no net Core
    /// </summary>
    [DbType(FluenteDbType.SQLITE)]
    public class EfContextSqLite : EfContext
    {
        public static LoggerFactory LoggerFactory;

        public EfContextSqLite(string connectionString) : base(connectionString)
        {
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlite(ConnectionString);

#if DEBUG
#if NETCOREAPP3_1
            LoggerFactory ??= new LoggerFactory(new[] { new Microsoft.Extensions.Logging.Debug.DebugLoggerProvider() });

            optionsBuilder
                .UseLoggerFactory(LoggerFactory)
                .EnableSensitiveDataLogging();
#endif
#endif

            base.OnConfiguring(optionsBuilder);
        }
    }
}
