using Fluent.Architecture.Repository;
using Fluent.Architecture.Services;

namespace Fluent.Architecture.Validation
{
    public class TransactionalValidation : BaseValidation
    {
        ///// <summary>
        ///// O repositório do serviço.
        ///// </summary>
        protected internal new TransactionlRepository Repository
        {
            get => base.Repository as TransactionlRepository;
            set => base.Repository = value;
        }

        /// <summary>
        /// A validação do serviço.
        /// </summary>
        protected internal new TransactionalService Service
        {
            get => base.Service as TransactionalService;
            set => base.Service = value;
        }
    }
}