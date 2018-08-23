using System;
using System.Linq;
using Fluent.Architecture.Factory;
using Fluent.Architecture.Model;

#if NET461
using System.Data.Entity;
#else
using Microsoft.EntityFrameworkCore;
#endif

namespace Fluent.Architecture.Repository
{
    /// <inheritdoc />
    /// <summary>
    /// Obtetos de transação.
    /// </summary>
    public class TransactionObjects : IDisposable
    {
        /// <summary>
        /// Sessão do EF.
        /// </summary>
        internal EfContext Session { get; set; }

        ///// <summary>
        ///// String de conexão com o banco de dados.
        ///// </summary>
        internal static string DataBaseConnectionString { get; set; }

        /// <summary>
        /// Inicializa objeto de transação.
        /// </summary>
        /// <param name="dataBaseConnectionString">
        /// String de conexão com o banco de dados.
        /// </param>
        public TransactionObjects(string dataBaseConnectionString)
        {
            DataBaseConnectionString = dataBaseConnectionString;
            Session = ContextFactory.Create(DataBaseConnectionString);
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
            return Session.Set<TX>();
        }

        internal DbSet<TX> GetObjectInputDataInternal<TX>() where TX : BaseEntity
        {
            return Session.Set<TX>();
        }

        /// <summary>
        /// Cria uma instância da classse.
        /// </summary>
        /// <returns></returns>
        internal static TransactionObjects Create()
        {
            return Activator.CreateInstance(Setup.TransactionObjectsType, DataBaseConnectionString) as TransactionObjects;
        }

        public void Dispose()
        {
            Session.Dispose();
        }
    }
}