// -----------------------------------------------------------------------
// <copyright company="Fluent System">
//     Copyright © Fluent System. All rights reserved.
//     TODOS OS DIREITOS RESERVADOS.
// </copyright>
// -----------------------------------------------------------------------

// ReSharper disable CommentTypo

using System;
using System.Collections.Generic;
using Fluent.Architecture.Core.Interfaces;
using Fluent.Architecture.Exceptions;
using Fluent.Architecture.Factory;
using Fluent.Architecture.Entities;
using Fluent.Architecture.Repository;
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

        internal List<Interaction> Interactions { get; set; }

        internal bool ExecuteInteractions()
        {
            if (Interactions == null || Interactions.Count == 0)
            {
                return false;
            }

            foreach (var interaction in Interactions)
            {
                interaction.Action(interaction.Entity);
            }

            return true;
        }

        internal void AddInteractions<TE>(Action<FluentGlobalizedEntity> action, TE entity) where TE : FluentGlobalizedEntity
        {
            if (Interactions == null)
            {
                Interactions = new List<Interaction>();
            }

            Interactions.Add(new Interaction
            {
                Action = action,
                Entity = entity,
            });
        }

        protected T CreateSpec<T>() where T : BaseSpecification
        {
            return SpecFactory.Create<T>(this);
        }

        internal class Interaction
        {
            internal Action<FluentGlobalizedEntity> Action { get; set; }
            public FluentGlobalizedEntity Entity { get; set; }
        }
    }
}
