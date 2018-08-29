// ReSharper disable CommentTypo
using System;
using System.Collections.Generic;
using System.Linq;
using Fluent.Architecture.Controllers;
using Fluent.Architecture.Model;
using Fluent.Architecture.Repository;
using Fluent.Architecture.Services;
#if NET461
#else
using Microsoft.AspNetCore.Http;
#endif

namespace Fluent.Architecture.Factory
{
    /// <summary>
    /// Classe interna.
    /// Fábrica de serviços.
    /// </summary>
    internal class ServiceFactory
    {
        /// <summary>
        /// Cria um serviço que terá controle de transação.
        /// Essa operação deve ser exclusiva do FluentController.
        /// </summary>
        /// <typeparam name="TS">
        /// O tipo de serviço a ser criado.
        /// </typeparam>
        /// <param name="httpContext">
        /// O contexto do controller.
        /// </param>
        /// <returns>
        /// O serviço criado.
        /// </returns>
        internal static TS Create<TS>(object httpContext) where TS : TransactionalService, new()
        {
            // Todo IMPORTANTE checar quem está chamando e barrar chamas externas
            var sessionId = Guid.NewGuid();
            var service = InternalCreate<TS>(sessionId);
            var userSession = CreateUserSession(httpContext, sessionId, service);
            service.SetUserSession(userSession);
            return service;
        }

        /// <summary>
        /// Cria um serviço em tempo de execução por meio de um processo de lazy-loading, à partir de um serviço original criado pelo <see cref="FluentController{T}"/>.
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
        internal static object CreateInternalServiceRuntime(Type serviceType, Guid sessionId)
        {
            var service = InternalCreate(serviceType, sessionId);
            service.SetUserSession(Setup.GetUserRequestSession(sessionId));
            return service;
        }

        #region PRIVATE

        private static TS InternalCreate<TS>(Guid sessionId) where TS : TransactionalService
        {
            var serviceType = GetSpecializedService(typeof(TS));
            return InternalCreate(serviceType, sessionId) as TS;
        }

        private static TransactionalService InternalCreate(Type serviceType, Guid sessionId)
        {
            return ServiceFactoryLazy.Create(serviceType, sessionId);
        }

        private static Type GetSpecializedService(BaseService service)
        {
            var serviceType = service.GetType();

            //// if (serviceType.IsAssignableFrom(typeof(IFluentDynamicProxy)))
            //// {
            //// // Todo "IFluentDynamicProxy ainda não está sendo usado
            //// serviceType = service.GetType().BaseType;
            //// }
            return GetSpecializedService(serviceType);
        }

        private static Type GetSpecializedService(Type serviceType)
        {
            var args = serviceType.GetGenericArguments();

            if (args.Any())
            {
                var entityType = args.First();
                if (!Setup.Services.TryGetValue(entityType.Name, out serviceType))
                {
                    serviceType = typeof(FluentService<>).MakeGenericType(entityType);
                }
            }

            return serviceType;
        }

        private static UserSessionRequest CreateUserSession(object httpContext, Guid sessionId, BaseService service)
        {
            var transactionObjects = TransactionObjects.Create(); 

            var serviceType = GetSpecializedService(service);
            var userSession = new UserSessionRequest
            {
                TransactionObjects = transactionObjects,
                SessionRequestId = sessionId,
                Services = new Dictionary<Type, BaseService>(),
                HttpContext = httpContext
            };

            userSession.Services.Add(serviceType, service);
            Setup.AddSession(userSession);

            return userSession;
        }

        #endregion
    }
}