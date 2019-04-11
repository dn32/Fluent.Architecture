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

using System;
using System.Linq;
using Fluent.Architecture.Model;
using Fluent.Architecture.Core.Interfaces;
using Fluent.Architecture.Factory;

namespace Fluent.Architecture.Repository
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

        ///// <summary>
        ///// String de conexão com o banco de dados.
        ///// </summary>
        internal static Func<object, string> GetConnectionString { get; set; }
    }
}