// ReSharper disable CommentTypo
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Fluent.Architecture.EntityFramework.Oracle
{
    /// <inheritdoc />
    /// <summary>
    /// Contexto do EF no net Core
    /// </summary>
    [DbType(FluentDbType.ORACLE)]
    public class EfContextOracle : EfContext
    {
        public static readonly LoggerFactory _myLoggerFactory = new LoggerFactory(new[] { new Microsoft.Extensions.Logging.Debug.DebugLoggerProvider() });

        public EfContextOracle(string connectionString) : base(connectionString)
        {
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
#if NETCOREAPP3_0
            throw new System.Exception("Oracle is not compatible net.core 3");
#else
            optionsBuilder.UseOracle(ConnectionString);
#endif


#if DEBUG
            optionsBuilder
                .UseLoggerFactory(_myLoggerFactory)
                .EnableSensitiveDataLogging();
#endif

            base.OnConfiguring(optionsBuilder);
        }
    }
}
