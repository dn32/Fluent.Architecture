// ReSharper disable CommentTypo

#if NET461

#else
using Microsoft.EntityFrameworkCore;

#endif

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

#if !NET461

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder
             //   .UseLoggerFactory(MyLoggerFactory) // Warning: Do not create a new ILoggerFactory instance each time
                .UseOracle(ConnectionString);

          //  MyLoggerFactory.AddDebug(LogLevel.Information);
        }
#endif
    }
}
