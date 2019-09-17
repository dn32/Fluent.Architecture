// -----------------------------------------------------------------------
// <copyright company="Fluent System">
//     Copyright © Fluent System. All rights reserved.
//     TODOS OS DIREITOS RESERVADOS.
// </copyright>
// -----------------------------------------------------------------------

// ReSharper disable CommentTypo

using Fluent.Architecture.Core.Interfaces;
using Fluent.Architecture.Entities;
using Fluent.Architecture.Extensions;
using Fluent.Architecture.Factory;
using Fluent.Architecture.Specifications;
using Fluent.Architecture.Validation;
using System;

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
            get => base.Validation.FluentCast<TransactionalValidation>();
            set => base.Validation = value;
        }

        protected internal new ITransactionlRepository Repository
        {
            get => base.Repository.FluentCast<ITransactionlRepository>();
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
