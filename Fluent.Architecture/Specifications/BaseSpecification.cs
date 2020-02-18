using dn32.infra.Exceptions;
using dn32.infra.Services;
using System.Linq;
using dn32.infra.dados;

namespace dn32.infra.Specifications
{
    /// <summary>
    /// Especificação base para todas as especificações do sistema.
    /// </summary>
    public abstract class BaseSpecification
    {
        internal BaseSpecification() { }

        protected bool IgnoreOrder { get; set; } = false;

        public TransactionalService Service { get; set; }

        /// <summary>
        /// Obtem o referência de uma tabela do banco de dados.
        /// </summary>
        /// <typeparam Nome="TX">
        /// Tipo de entidade desejada.
        /// </typeparam>
        /// <returns>
        /// A referência da tabela do banco de dados.
        /// </returns>
        protected IQueryable<TX> Get<TX>() where TX : EntidadeBase
        {
            if (Service == null)
            {
                throw new IncorrectDevelopmentException($"Failed to initialize specification [{GetType().Name}].\nYou must use [CreateSpec] present in the service or controller.");
            }

            var transactionObjects = Service.TransactionObjects;
            return transactionObjects.GetObjectQueryInternal<TX>();
        }

        internal void SetService(TransactionalService service)
        {
            this.Service = service;
        }
    }
}