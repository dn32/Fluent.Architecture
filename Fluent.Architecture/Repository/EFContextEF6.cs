// -----------------------------------------------------------------------
// <copyright company="Fluent System">
//     Copyright © Fluent System. All rights reserved.
//     TODOS OS DIREITOS RESERVADOS.
// </copyright>
// -----------------------------------------------------------------------

// ReSharper disable CommentTypo

#if NET461

using System;
using System.Data.Entity;
using System.Data.Entity.ModelConfiguration.Conventions;
using System.Linq;
using System.Reflection;
using Fluent.Architecture.Attributes;
using Fluent.Architecture.Model;

namespace Fluent.Architecture.Repository
{
    /// <inheritdoc />
    /// <summary>
    /// Contexto do EF no NET461
    /// </summary>
    public class EfContext : DbContext
    {
        public EfContext(string connectionString) : base(connectionString)
        {
        }

        /// <summary>
        /// Todas as entidades de banco de dados são adicionados automaticamente.
        /// Use <see cref="NotDbEntityAttribute"/> se não desejar que uma entidade seja adicionada.
        /// </summary>
        /// <param name="modelBuilder">
        /// O model builder do EF.
        /// </param>
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Conventions.Remove<PluralizingTableNameConvention>();
            var entityMethod = typeof(DbModelBuilder).GetMethod("Entity", Array.Empty<Type>());
            var exportedTypes = Setup.Model.Values.ToList();
            foreach (var type in exportedTypes)
            {
                if (type.IsAbstract || type.GetCustomAttribute<NotDbEntityAttribute>(false) != null)
                {
                    continue;
                }

                if (type.IsSubclassOf(typeof(FluentEntity)))
                {
                    if (entityMethod != null) entityMethod.MakeGenericMethod(type).Invoke(modelBuilder, Array.Empty<object>());
                }
            }

            base.OnModelCreating(modelBuilder);
        }
    }
}

#endif