// -----------------------------------------------------------------------
// <copyright company="Fluent System">
//     Copyright © Fluent System. All rights reserved.
//     TODOS OS DIREITOS RESERVADOS.
// </copyright>
// -----------------------------------------------------------------------

// ReSharper disable CommentTypo

using System.Linq;
using Fluent.Architecture.Exceptions;
using Fluent.Architecture.Model;
using Fluent.Architecture.Services;

namespace Fluent.Architecture.Specifications
{
    /// <summary>
    /// Especificação base para todas as especificações do sistema.
    /// </summary>
    public abstract class BaseSpecification
    {
        internal BaseSpecification() { }

        public TransactionalService Service { get; set; }

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