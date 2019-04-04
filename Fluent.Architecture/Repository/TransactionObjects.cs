// -----------------------------------------------------------------------
// <copyright company="Fluent System">
//     Copyright © Fluent System. All rights reserved.
//     TODOS OS DIREITOS RESERVADOS.
// </copyright>
// -----------------------------------------------------------------------

// ReSharper disable CommentTypo

using System;
using System.Linq;
using Fluent.Architecture.Factory;
using Fluent.Architecture.Model;
using System.Data.Entity;

namespace Fluent.Architecture.Repository
{
    /// <summary>
    /// Obtetos de transação.
    /// </summary>
    public class TransactionObjects
    {
        public void Dispose()
        {
            this.Session.Dispose();
        }

        /// <summary>
        /// Cria uma instância da classse.
        /// </summary>
        /// <returns>
        /// A insância da classe.
        /// </returns>
        internal static TransactionObjects Create()
        {
            return Activator.CreateInstance(Setup.TransactionObjectsType, DataBaseConnectionString) as TransactionObjects;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="TransactionObjects"/> class. 
        /// Inicializa objeto de transação.
        /// </summary>
        /// <param name="dataBaseConnectionString">
        /// String de conexão com o banco de dados.
        /// </param>
        public TransactionObjects(string dataBaseConnectionString)
        {
            DataBaseConnectionString = dataBaseConnectionString;
            this.Session = ContextFactory.Create(DataBaseConnectionString);
        }

        internal DbSet<TX> GetObjectInputDataInternal<TX>() where TX : BaseEntity
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
        protected internal virtual IQueryable<TX> GetObjectQueryInternal<TX>() where TX : BaseEntity
        {
            return this.Session.Set<TX>();
        }

        /// <summary>
        /// Sessão do EF.
        /// </summary>
        protected internal EfContext Session { get; set; }

        ///// <summary>
        ///// String de conexão com o banco de dados.
        ///// </summary>
        internal static string DataBaseConnectionString { get; set; }
    }
}