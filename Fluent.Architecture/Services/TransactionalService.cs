// ReSharper disable CommentTypo

using System;
using System.Collections.Generic;
using Fluent.Architecture.Model;
using Fluent.Architecture.Repository;
using Fluent.Architecture.Sample.Test.SupportElements.Model;

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
