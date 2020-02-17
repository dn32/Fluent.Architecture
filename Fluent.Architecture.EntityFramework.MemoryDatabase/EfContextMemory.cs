// ReSharper disable CommentTypo
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;

namespace Fluente.Arquitetura.EntityFramework.MemoryDatabase
{
    /// <inheritdoc />
    /// <summary>
    /// Contexto do EF no net Core
    /// </summary>
    [DbType(FluenteDbType.MEMORY)]
    public class EfContextMemory : EfContext
    {
        public EfContextMemory(string connectionString) : base(connectionString)
        {
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.ConfigureWarnings(x =>
            {
                x.Ignore(eventIds: InMemoryEventId.TransactionIgnoredWarning);
            });

            optionsBuilder.UseInMemoryDatabase(ConnectionString);
        }
    }
}
