// ReSharper disable CommentTypo

#if !NET461
using System;
using System.Linq;
using System.Reflection;
using Fluent.Architecture.Attributes;
using Fluent.Architecture.Model;
using Microsoft.EntityFrameworkCore;

namespace Fluent.Architecture.Repository
{
    /// <inheritdoc />
    /// <summary>
    /// Contexto do EF no net Core
    /// </summary>
    public class EfContext : DbContext
    {
        internal string ConnectionString { get; set; }

        public EfContext(string connectionString)
        {
            ConnectionString = connectionString;
        }

        /// <summary>
        /// Todas as entidades de banco de dados são adicionados automaticamente.
        /// Use <see cref="NotDbEntityAttribute"/> se não desejar que uma entidade seja adicionada.
        /// </summary>
        /// <param name="modelBuilder">
        /// O model builder do EF.
        /// </param>
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            var entityMethod = typeof(ModelBuilder).GetMethod("Entity", new Type[] { });
            var exportedTypes = Setup.Model.Values.ToList();
            foreach (var type in exportedTypes)
            {
                if (type.GetCustomAttribute<NotDbEntityAttribute>(false) != null)
                {
                    continue;
                }

                if (type.IsSubclassOf(typeof(FluentEntity)))
                {
                    if (entityMethod != null) entityMethod.MakeGenericMethod(type).Invoke(modelBuilder, new object[] { });
                }
            }

            base.OnModelCreating(modelBuilder);
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer(ConnectionString);
        }
    }
}
#endif