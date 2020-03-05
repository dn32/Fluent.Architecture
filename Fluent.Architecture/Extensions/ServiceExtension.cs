using dn32.infra.nucleo.excecoes;
using dn32.infra.Factory;
using dn32.infra.Nucleo.Models;
using dn32.infra.servicos;
using System;
using dn32.infra.dados;

namespace dn32.infra.extensoes
{
    public static class ServiceExtension
    {
        public static DnServicoTransacionalBase GetServiceInstanceByServiceType(this Type serviceType, SessaoDeRequisicaoDoUsuario SessionRequest)
        {
            if (serviceType == null) { throw new ArgumentNullException(nameof(serviceType)); }
            if (SessionRequest == null) { throw new ArgumentNullException(nameof(SessionRequest)); }
            if (serviceType.Name == "DnDynamicProxy") { serviceType = serviceType.BaseType; }
            if (serviceType == null) { throw new ArgumentNullException(nameof(serviceType)); }

            if (!serviceType.IsSubclassOf(typeof(DnServicoTransacionalBase)))
            {
                throw new DesenvolvimentoIncorretoException($"The service instance attempt using the {nameof(GetServiceInstanceByServiceType)} method failed because the passed type is not a {nameof(DnServicoTransacionalBase)}");
            }

            if (SessionRequest.Services.TryGetValue(serviceType, out var ser))
            {
                return ser as DnServicoTransacionalBase;
            }

            var service = ServiceFactory.Create(serviceType, SessionRequest.LocalHttpContext, SessionRequest);
            SessionRequest.Services.Add(serviceType, service);

            return service;
        }

        public static DnServicoTransacionalBase GetServiceInstanceByEntity(this Type entityType, SessaoDeRequisicaoDoUsuario SessionRequest)
        {
            if (entityType?.IsSubclassOf(typeof(DnEntidade)) != true)
            {
                throw new DesenvolvimentoIncorretoException($"The service instance attempt using the {nameof(GetServiceInstanceByEntity)} method failed because the passed type is not a {nameof(DnEntidade)}");
            }

            var type = (Setup.ConfiguracoesGlobais.GenericServiceType) ?? typeof(DnServico<>);
            var serviceType = type.MakeGenericType(entityType).GetSpecializedService();
            return serviceType.GetServiceInstanceByServiceType(SessionRequest);
        }


    }
}
