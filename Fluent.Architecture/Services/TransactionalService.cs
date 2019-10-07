// -----------------------------------------------------------------------
// <copyright company="Fluent System">
//     Copyright © Fluent System. All rights reserved.
//     TODOS OS DIREITOS RESERVADOS.
// </copyright>
// -----------------------------------------------------------------------

// ReSharper disable CommentTypo

using Fluent.Architecture.Core.Interfaces;
using Fluent.Architecture.Core.Models;
using Fluent.Architecture.Factory;
using Fluent.Architecture.Specifications;
using Fluent.Architecture.Validation;

namespace Fluent.Architecture.Services
{
    ///<inheritdoc/>
    /// <summary>
    /// Serviço base para serviços sem relacionamento direto com uma entidade.
    /// </summary>
    public abstract class TransactionalService : BaseService
    {
        protected internal new TransactionalValidation Validation
        {
            get => base.Validation as TransactionalValidation;
            set => base.Validation = value;
        }

        protected internal new ITransactionlRepository Repository
        {
            get => base.Repository as ITransactionlRepository;
            set => base.Repository = value;
        }

        protected internal override void SetUserSession(UserSessionRequest sessionRequest)
        {
            base.SetUserSession(sessionRequest);

            if (ValidationType == null)
            {
                return;
            }

            Validation = ValidationFactory.Create(ValidationType);
            Validation.Init(this);
        }

        /// <summary>
        /// Objetos de transação do serviço.
        /// </summary>
        internal ITransactionObjects TransactionObjects => this.SessionRequest.TransactionObjects;

        protected T CreateSpec<T>() where T : BaseSpecification
        {
            return SpecFactory.Create<T>(this);
        }
    }
}
