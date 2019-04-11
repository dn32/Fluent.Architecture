using Fluent.Architecture.Core.Interfaces;
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
        public static ITransactionObjects Create(Type transactionObjectsType, Connection connection)
        {
            return Activator.CreateInstance(transactionObjectsType, connection) as ITransactionObjects;
        }
    }
}
