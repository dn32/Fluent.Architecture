// ReSharper disable CommentTypo
using Microsoft.EntityFrameworkCore;

namespace dn32.infra.EntityFramework.MySQL
{
    /// <inheritdoc />
    /// <summary>
    /// Contexto do EF no net Core
    /// </summary>
    [DbType(DnDbType.MYSQL)]
    public class EfContextMySQL : EfContext
    {
        public EfContextMySQL(string connectionString) : base(connectionString)
        {
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseMySQL(ConnectionString);
            base.OnConfiguring(optionsBuilder);
        }
    }
}
