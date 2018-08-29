// ReSharper disable CommentTypo

using Fluent.Architecture.Repository;

namespace Fluent.Architecture.Services
{
    ///<inheritdoc/>
    /// <summary>
    /// Serviço base para serviços sem relacionamento direto com uma entidade.
    /// </summary>
    public class TransactionalService : BaseService
    {
        /// <summary>
        /// Objetos de transação do serviço.
        /// </summary>
        internal TransactionObjects TransactionObjects => this.SessionRequest.TransactionObjects;
    }
}
