// -----------------------------------------------------------------------
// <copyright company="Fluent System">
//     Copyright © Fluent System. All rights reserved.
//     TODOS OS DIREITOS RESERVADOS.
// </copyright>
// -----------------------------------------------------------------------

// ReSharper disable CommentTypo
using Fluent.Architecture.Repository;

#if NET461
using System.Data.Entity;

#else
using Microsoft.EntityFrameworkCore;

#endif

namespace Fluent.Architecture.Factory
{
    /// <summary>
    /// Interno.
    /// A fabrica de contexto do entity framework.
    /// </summary>
    internal class ContextFactory
    {
        /// <summary>
        ///  Cria um novo contexto para o entity framework.
        /// </summary>
        /// <param name="connectionString">
        /// A string de conexão com o banco de dados.
        /// </param>
        /// <returns>
        /// O contexto criado.
        /// </returns>
        internal static EfContext Create(Connection connection)
        {
            var connectionString = connection.GetConnectionString(null);
            var createDatabaseIfNotExists = connection.CreateDatabaseIfNotExists;

            CreateDB(connectionString, createDatabaseIfNotExists);
            return new EfContext(connectionString);

            // Deixar essa parte de interceptador pra quando for necessários.
            // var interceptor = new ContextInterceptor(Guid.Empty);
            // return new ProxyGenerator().CreateClassProxy(typeof(EfContext), new object[] { connectionString }, interceptor) as EfContext;
        }


        private static void CreateDB(string connectionString, bool createDatabaseIfNotExists)
        {
#if NET461
            if (createDatabaseIfNotExists)
            {
                Database.SetInitializer(new CreateDatabaseIfNotExists<EfContext>());
            }
            else
            {
                Database.SetInitializer<EfContext>(null);
            }
#else
            if (createDatabaseIfNotExists)
            {
                var context = new EfContext(connectionString);
                context.Database.EnsureCreated();
                context.Database.Migrate();
            }
#endif
        }
    }
}
