// -----------------------------------------------------------------------
// <copyright company="Fluent System">
//     Copyright © Fluent System. All rights reserved.
//     TODOS OS DIREITOS RESERVADOS.
// </copyright>
// -----------------------------------------------------------------------

// ReSharper disable CommentTypo
using System;
using Fluent.Architecture.Factory.Proxy;
using Fluent.Architecture.Services;

namespace Fluent.Architecture.Factory
{
    /// <summary>
    /// Método interno.
    /// Fábrica de serviços com propriedades capazes de se injetar depenência por meio de um padrão de lazy-loading.
    /// </summary>
    internal class ServiceFactoryLazy
    {
        /// <summary>
        /// Cria um serviço em tempo de execução por meio de um processo de lazy-loading.
        /// </summary>
        /// <param name="serviceType">
        /// O tipo de serviço a ser criado.
        /// </param>
        /// <param name="sessionId">
        /// O identificador de sessão do usuário durante a requisição ao controller.
        /// </param>
        /// <returns>
        /// O serviço criado.
        /// </returns>
        internal static TransactionalService Create(Type serviceType, Guid sessionId)
        {
            return ServiceLazyClassBuilder.CreateObject(serviceType, sessionId) as TransactionalService;
        }
    }
}