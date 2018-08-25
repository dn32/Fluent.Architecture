using System.Linq;
using Fluent.Architecture.Model;
using Fluent.Architecture.Repository;
using Fluent.Architecture.Service;

namespace Fluent.Architecture.Specifications
{
    /// <summary>
    /// Especificação base para todas as especificações do sistema.
    /// </summary>
    public abstract class BaseSpecification
    {
        private TransactionObjects TransactionObjects { get; }

        /// <summary>
        /// Inicializa a especificação.
        /// </summary>
        /// <param name="service">
        /// Quanquer serviço para obtenção dos objetos de transação,
        /// </param>
        protected BaseSpecification(TransactionalService service)
        {
            TransactionObjects = service.TransactionObjects;
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
        protected IQueryable<TX> Get<TX>() where TX : BaseEntity
        {
            return TransactionObjects.GetObjectQueryInternal<TX>();
        }

    }
}