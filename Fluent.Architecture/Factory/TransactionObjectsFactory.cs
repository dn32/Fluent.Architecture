using Fluent.Architecture.Core.Interfaces;
using Fluent.Architecture.Entities;
using Fluent.Architecture.Extensions;
using System;

namespace Fluent.Architecture.Core.Factory
{
    public static class TransactionObjectsFactory
    {
        /// <summary>
        /// Cria uma instância da classse.
        /// </summary>
        /// <returns>
        /// A insância da classe.
        /// </returns>
        public static ITransactionObjects Create(Type transactionObjectsType, Connection connection, UserSessionRequest userSessionRequest)
        {
            return Activator.CreateInstance(transactionObjectsType, connection, userSessionRequest)?.FluentCast<ITransactionObjects>() ?? throw new InvalidOperationException($"Building failed {transactionObjectsType.Name}");
        }
    }
}
