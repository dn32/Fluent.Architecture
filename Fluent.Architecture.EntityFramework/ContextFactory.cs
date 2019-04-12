// -----------------------------------------------------------------------
// <copyright company="Fluent System">
//     Copyright © Fluent System. All rights reserved.
//     TODOS OS DIREITOS RESERVADOS.
// </copyright>
// -----------------------------------------------------------------------

// ReSharper disable CommentTypo
#if NET461
using System.Data.Entity;
using System.Runtime.CompilerServices;

#else
using Microsoft.EntityFrameworkCore;
using System.Runtime.CompilerServices;

#endif

[assembly: InternalsVisibleTo(@"Fluent.Architecture.EntityFramework.SqlServer, PublicKey=00240000048000009400000006020000002400005253413100040000010001002d98533364f3b3fbd11e7a3f14cd73d169e1daabd62ba2d1e5bc6a48a9bc709a503960db0e76c190e7a8dcefaed037e539682d6a891b242ddb91a3ab20fbfa0c04fb6304c8903857e1ed75399850fca4037dd2c810749e75770e5d455e950ccb9d06cf6fea5f30b00557a29408ce4c45021c412eca32616f47809bfe2cf404cc")]
namespace Fluent.Architecture.EntityFramework
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
