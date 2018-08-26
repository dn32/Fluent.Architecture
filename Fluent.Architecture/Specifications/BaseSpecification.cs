// ReSharper disable CommentTypo

using System.Linq;
using Fluent.Architecture.Controllers;
using Fluent.Architecture.Model;
using Fluent.Architecture.Repository;
using Fluent.Architecture.Services;

namespace Fluent.Architecture.Specifications
{
    /// <summary>
    /// Especificação base para todas as especificações do sistema.
    /// </summary>
    public abstract class BaseSpecification<T> where T : BaseEntity
    {
        private FluentController<T> Controller { get; set; }

        private TransactionalService Service { get; set; }

        /// <summary>
        /// Inicializa a especificação.
        /// </summary>
        /// <param name="service">
        /// Quanquer serviço para obtenção dos objetos de transação,
        /// </param>
        protected BaseSpecification(TransactionalService service)
        {
            Service = service;
        }

        //Todo doc
        protected BaseSpecification(FluentController<T> controller)
        {
            Controller = controller;
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
            var transactionObjects = Service == null ? Controller.Service.TransactionObjects : Service.TransactionObjects;
            return transactionObjects.GetObjectQueryInternal<TX>();
        }
    }
}