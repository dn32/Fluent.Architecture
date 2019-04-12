// ReSharper disable CommentTypo

#if NET461

#else
using Microsoft.EntityFrameworkCore;

#endif

using Fluent.Architecture.EntityFramework;

namespace Fluent.Architecture.Repository
{
    /// <inheritdoc />
    /// <summary>
    /// Contexto do EF no net Core
    /// </summary>
    public class EfContextSQL : EfContext
    {
        public EfContextSQL(string connectionString) : base(connectionString)
        {
        }

#if !NET461
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer(ConnectionString);
        }
#endif
    }
}
