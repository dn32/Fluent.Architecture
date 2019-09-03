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
        public static readonly LoggerFactory _myLoggerFactory =new LoggerFactory(new[] {new Microsoft.Extensions.Logging.Debug.DebugLoggerProvider()});

        public EfContextOracle(string connectionString) : base(connectionString)
        {
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseOracle(ConnectionString);

#if DEBUG
            optionsBuilder
                .UseLoggerFactory(_myLoggerFactory)
                .EnableSensitiveDataLogging();
#endif

            base.OnConfiguring(optionsBuilder);
        }
    }
}
