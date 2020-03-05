using dn32.infra.nucleo.controladores;
using dn32.infra.nucleo.excecoes;
using dn32.infra.Nucleo.Interfaces;
using dn32.infra.Nucleo.Models;
using dn32.infra.servicos;
using System;
using System.Collections.Generic;
using System.Linq;
using dn32.infra.extensoes;
using dn32.infra.nucleo.servicos;

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
        /// Essa operação deve ser exclusiva do DnControlador.
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
        internal static TS Create<TS>(object httpContext) where TS : DnServicoTransacionalBase, new()
        {
            return Create(typeof(TS), httpContext) as TS;
        }

        internal static DnServicoTransacionalBase Create(Type serviceType, SessaoDeRequisicaoDoUsuario sessionRequest)
        {
            return Create(serviceType, sessionRequest.HttpContext, sessionRequest);
        }

        internal static DnServicoTransacionalBase Create(Type serviceType, object httpContext, SessaoDeRequisicaoDoUsuario sessionRequest = null)
        {
            var sessionId = Guid.NewGuid();
            serviceType = GetSpecializedService(serviceType);
            var service = InternalCreate(serviceType, sessionId);
            var userSession = sessionRequest ?? CreateUserSession(httpContext, sessionId, service);
            service.DefinirSessaoDoUsuario(userSession);
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
        public static TS Create<TS>(object httpContext, string justification) where TS : DnServicoTransacionalBase, new()
        {
            if (string.IsNullOrWhiteSpace(justification))
            {
                throw new DesenvolvimentoIncorretoException("Report the justification");
            }

            return Create(typeof(TS), httpContext).DnCast<TS>();
        }

        public static DnServicoTransacionalBase Create(Type serviceType, object httpContext, string justification)
        {
            if (string.IsNullOrWhiteSpace(justification))
            {
                throw new DesenvolvimentoIncorretoException("Report the justification");
            }

            if (serviceType.IsDnEntity())
            {
                serviceType = typeof(servicos.DnServico<>).MakeGenericType(serviceType);
            }

            return Create(serviceType, httpContext).DnCast<DnServicoTransacionalBase>();
        }

        //private static void InternalCreateValidation(TransactionalService service)
        //{
        //    if (service.ValidationType != null)
        //    {
        //        service.Validation = ValidationFactory.CreateNotEntity(service.ValidationType);
        //    }
        //}

        /// <summary>
        /// Cria um serviço em tempo de execução por meio de um processo de lazy-loading, à partir de um serviço original criado pelo <see cref="DnControllerController{T}"/>.
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
            service.DefinirSessaoDoUsuario(Setup.ObterSessaoDeUmaRequisicao(sessionId));
            return service;
        }

        #region PRIVATE

        private static DnServicoTransacionalBase InternalCreate(Type serviceType, Guid sessionId)
        {
            return ServiceFactoryLazy.Create(serviceType, sessionId);
        }

        private static Type GetSpecializedService(Type serviceType)
        {
            var args = serviceType.GetGenericArguments();

            if (args.Any())
            {
                var entityType = args.First();
                if (!Setup.Servicos.TryGetValue(entityType, out serviceType))
                {
                    var type = (Setup.ConfiguracoesGlobais.GenericServiceType) ?? typeof(servicos.DnServico<>);
                    serviceType = type.MakeGenericType(entityType);
                }
            }

            return serviceType;
        }

        private static SessaoDeRequisicaoDoUsuario CreateUserSession(object httpContext, Guid sessionId, DnServicoBase service)
        {
            //Todo arrumar
            ITransactionObjects transactionObjects = null;// ITransactionObjects.Create();

            var serviceType = GetSpecializedService(service.GetType());

            var type = Setup.ConfiguracoesGlobais.UserSessionRequestType ?? typeof(SessaoDeRequisicaoDoUsuario);
            var userSession = Activator.CreateInstance(type).DnCast<SessaoDeRequisicaoDoUsuario>();
            userSession.ObjetosDaTransacao = transactionObjects;
            userSession.IdentificadorDaSessao = sessionId;
            userSession.Services = new Dictionary<Type, DnServicoBase>();
            userSession.HttpContext = httpContext;

            userSession.Services.Add(serviceType, service);
            Setup.AddSession(userSession);

            return userSession;
        }

        #endregion
    }
}