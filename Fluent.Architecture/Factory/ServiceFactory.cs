using dn32.infra.controladores;
using dn32.infra.Exceptions;
using dn32.infra.Extensoes;
using dn32.infra.Nucleo.Interfaces;
using dn32.infra.Nucleo.Models;
using dn32.infra.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using dn32.infra.extensoes;

namespace dn32.infra.Factory
{
    /// <summary>
    /// Classe interna.
    /// Fábrica de serviços.
    /// </summary>
    public static class ServiceFactory
    {
        /// <summary>
        /// Cria um serviço que terá controle de transação.
        /// Essa operação deve ser exclusiva do Dn.
        /// </summary>
        /// <typeparam Nome="TS">
        /// O tipo de serviço a ser criado.
        /// </typeparam>
        /// <param Nome="httpContext">
        /// O contexto do controller.
        /// </param>
        /// <returns>
        /// O serviço criado.
        /// </returns>
        internal static TS Create<TS>(object httpContext) where TS : TransactionalService, new()
        {
            return Create(typeof(TS), httpContext) as TS;
        }

        internal static TransactionalService Create(Type serviceType, UserSessionRequest sessionRequest)
        {
            return Create(serviceType, sessionRequest.HttpContext, sessionRequest);
        }

        internal static TransactionalService Create(Type serviceType, object httpContext, UserSessionRequest sessionRequest = null)
        {
            var sessionId = Guid.NewGuid();
            serviceType = GetSpecializedService(serviceType);
            var service = InternalCreate(serviceType, sessionId);
            var userSession = sessionRequest ?? CreateUserSession(httpContext, sessionId, service);
            service.SetUserSession(userSession);
            return service;
        }

        /// <summary>
        /// MUITO CUIDADO!!!! Esse método só deve ser utilizado se você estiver muito certo do que está fazendo.
        /// </summary>
        /// <typeparam Nome="TS">
        /// O tipo de serviço a ser criado.
        /// </typeparam>
        /// <param Nome="httpContext">
        /// O contexto do controller.
        /// </param>
        /// <param Nome="justification">
        /// Explique por que você está fazendo uso desse método.
        /// </param>
        /// <returns></returns>
        public static TS Create<TS>(object httpContext, string justification) where TS : TransactionalService, new()
        {
            if (string.IsNullOrWhiteSpace(justification))
            {
                throw new IncorrectDevelopmentException("Report the justification");
            }

            return Create(typeof(TS), httpContext).DnCast<TS>();
        }

        public static TransactionalService Create(Type serviceType, object httpContext, string justification)
        {
            if (string.IsNullOrWhiteSpace(justification))
            {
                throw new IncorrectDevelopmentException("Report the justification");
            }

            if (serviceType.IsDnEntity())
            {
                serviceType = typeof(Services.DnService<>).MakeGenericType(serviceType);
            }

            return Create(serviceType, httpContext).DnCast<TransactionalService>();
        }

        //private static void InternalCreateValidation(TransactionalService service)
        //{
        //    if (service.ValidationType != null)
        //    {
        //        service.Validation = ValidationFactory.CreateNotEntity(service.ValidationType);
        //    }
        //}

        /// <summary>
        /// Cria um serviço em tempo de execução por meio de um processo de lazy-loading, à partir de um serviço original criado pelo <see cref="Dn{T}"/>.
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
        internal static object CreateInternalServiceRuntime(Type serviceType, Guid sessionId)
        {
            var service = InternalCreate(serviceType, sessionId);
            service.SetUserSession(Setup.GetUserRequestSession(sessionId));
            return service;
        }

        #region PRIVATE

        private static TransactionalService InternalCreate(Type serviceType, Guid sessionId)
        {
            return ServiceFactoryLazy.Create(serviceType, sessionId);
        }

        private static Type GetSpecializedService(Type serviceType)
        {
            var args = serviceType.GetGenericArguments();

            if (args.Any())
            {
                var entityType = args.First();
                if (!Setup.Services.TryGetValue(entityType, out serviceType))
                {
                    var type = (Setup.Config.Config.GenericServiceType) ?? typeof(Services.DnService<>);
                    serviceType = type.MakeGenericType(entityType);
                }
            }

            return serviceType;
        }

        private static UserSessionRequest CreateUserSession(object httpContext, Guid sessionId, BaseService service)
        {
            //Todo arrumar
            ITransactionObjects transactionObjects = null;// ITransactionObjects.Create();

            var serviceType = GetSpecializedService(service.GetType());

            var type = Setup.Config.Config.UserSessionRequestType ?? typeof(UserSessionRequest);
            var userSession = Activator.CreateInstance(type).DnCast<UserSessionRequest>();
            userSession.TransactionObjects = transactionObjects;
            userSession.SessionRequestId = sessionId;
            userSession.Services = new Dictionary<Type, BaseService>();
            userSession.HttpContext = httpContext;

            userSession.Services.Add(serviceType, service);
            Setup.AddSession(userSession);

            return userSession;
        }

        #endregion
    }
}