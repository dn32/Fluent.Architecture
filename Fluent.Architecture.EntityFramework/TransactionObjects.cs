// -----------------------------------------------------------------------
// <copyright company="Fluent System">
//     Copyright © Fluent System. All rights reserved.
//     TODOS OS DIREITOS RESERVADOS.
// </copyright>
// -----------------------------------------------------------------------

// ReSharper disable CommentTypo
#if NET461
using System.Data.Entity;

#else
using Microsoft.EntityFrameworkCore;

#endif

using System.Linq;
using Fluent.Architecture.Model;
using Fluent.Architecture.Core.Interfaces;
using System.Runtime.CompilerServices;

[assembly: InternalsVisibleTo(@"Fluent.Architecture.EntityFramework.SqlServer, PublicKey=00240000048000009400000006020000002400005253413100040000010001002d98533364f3b3fbd11e7a3f14cd73d169e1daabd62ba2d1e5bc6a48a9bc709a503960db0e76c190e7a8dcefaed037e539682d6a891b242ddb91a3ab20fbfa0c04fb6304c8903857e1ed75399850fca4037dd2c810749e75770e5d455e950ccb9d06cf6fea5f30b00557a29408ce4c45021c412eca32616f47809bfe2cf404cc")]
namespace Fluent.Architecture.EntityFramework
{
    /// <summary>
    /// Obtetos de transação.
    /// </summary>
    public class TransactionObjects : ITransactionObjects
    {
        public void Dispose()
        {
            this.Session.Dispose();
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="TransactionObjects"/> class. 
        /// Inicializa objeto de transação.
        /// </summary>
        /// <param name="dataBaseConnectionString">
        /// String de conexão com o banco de dados.
        /// </param>
        public TransactionObjects(Connection connection)
        {
            this.Session = ContextFactory.Create(connection);
        }

        public DbSet<TX> GetObjectInputDataInternal<TX>() where TX : class
        {
            return this.Session.Set<TX>();
        }

        /// <summary>
        /// Obtem o referência de uma tabela do banco de dados.
        /// </summary>
        /// <typeparam name="TX">
        /// Tipo de entidade desejada.
        /// </typeparam>
        /// <returns>
        /// A referência da tabela do banco de dados.
        /// </returns>
        public virtual IQueryable<TX> GetObjectQueryInternal<TX>() where TX : BaseEntity
        {
            return this.Session.Set<TX>();
        }

        /// <summary>
        /// Sessão do EF.
        /// </summary>
        public DbContext Session { get; set; }
    }
}