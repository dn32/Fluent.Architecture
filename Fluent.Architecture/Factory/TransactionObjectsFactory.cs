using dn32.infra.Nucleo.Interfaces;
using dn32.infra.Nucleo.Models;
using System;

namespace dn32.infra.Nucleo.Factory
{
    public static class TransactionObjectsFactory
    {
        /// <summary>
        /// Cria uma instância da classse.
        /// </summary>
        /// <returns>
        /// A insância da classe.
        /// </returns>
        public static ITransactionObjects Create(Type transactionObjectsType, Connection connection, SessaoDeRequisicaoDoUsuario userSessionRequest)
        {
            return Activator.CreateInstance(transactionObjectsType, connection, userSessionRequest) as ITransactionObjects;
        }
    }
}
