// ReSharper disable CommentTypo

using System;
using System.Collections.Generic;
using Fluent.Architecture.Factory;
using Fluent.Architecture.Model;
using Fluent.Architecture.Repository;
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

        protected internal new TransactionlRepository Repository
        {
            get => base.Repository as TransactionlRepository;
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
            Validation.Init(this, this.Repository);
        }

        /// <summary>
        /// Objetos de transação do serviço.
        /// </summary>
        internal TransactionObjects TransactionObjects => this.SessionRequest.TransactionObjects;

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

        internal class Interaction
        {
            internal Action<FluentGlobalizedEntity> Action { get; set; }
            public FluentGlobalizedEntity Entity { get; set; }
        }
    }
}
