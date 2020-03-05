// -----------------------------------------------------------------------
// <copyright company="DnControlador System">
//     Copyright © DnControlador System. All rights reserved.
//     TODOS OS DIREITOS RESERVADOS.
// </copyright>
// -----------------------------------------------------------------------

// ReSharper disable CommentTypo
using dn32.infra.Factory.Proxy;
using dn32.infra.servicos;
using System;

namespace dn32.infra.Factory
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
        /// <param Nome="serviceType">
        /// O tipo de serviço a ser criado.
        /// </param>
        /// <param Nome="sessionId">
        /// O identificador de sessão do usuário durante a requisição ao controller.
        /// </param>
        /// <returns>
        /// O serviço criado.
        /// </returns>
        internal static DnServicoTransacionalBase Create(Type serviceType, Guid sessionId)
        {
            return ServiceLazyClassBuilder.CreateObject(serviceType, sessionId) as DnServicoTransacionalBase;
        }
    }
}