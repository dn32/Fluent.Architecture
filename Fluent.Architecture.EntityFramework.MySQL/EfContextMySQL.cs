// ReSharper disable CommentTypo

#if NET461

#else
using Microsoft.EntityFrameworkCore;

#endif

using Fluent.Architecture.EntityFramework;

namespace Fluent.Architecture.EntityFramework.MySQL
{
    /// <inheritdoc />
    /// <summary>
    /// Contexto do EF no net Core
    /// </summary>
    public class EfContextMySQL : EfContext
    {
        public EfContextMySQL(string connectionString) : base(connectionString)
        {
        }

#if !NET461
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseMySql(ConnectionString);
        }
#endif
    }
}
