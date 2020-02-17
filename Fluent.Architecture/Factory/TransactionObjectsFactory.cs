using Fluente.Arquitetura.Nucleo.Interfaces;
using Fluente.Arquitetura.Nucleo.Models;
using System;

namespace Fluente.Arquitetura.Nucleo.Factory
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
            return Activator.CreateInstance(transactionObjectsType, connection, userSessionRequest) as ITransactionObjects;
        }
    }
}
