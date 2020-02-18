using Fluente.Arquitetura.Exceptions;
using Fluente.Arquitetura.Nucleo.Models;
using Fluente.Arquitetura.Factory;
using Fluente.Arquitetura.Services;
using System;
using Fluente.Arquitetura.Base.Models;
using Fluente.Arquitetura.Base.Atributos;
using Fluente.Arquitetura.Base.Enumeradores;
using Fluente.Arquitetura.Base.Extensoes;
using Fluente.Arquitetura.Interfaces;

namespace Fluente.Arquitetura.Extensoes
{
    public static class ServiceExtension
    {
        public static TransactionalService GetServiceInstanceByServiceType(this Type serviceType, UserSessionRequest SessionRequest)
        {
            if (serviceType == null) { throw new ArgumentNullException(nameof(serviceType)); }
            if (SessionRequest == null) { throw new ArgumentNullException(nameof(SessionRequest)); }
            if (serviceType.Name == "FluenteDynamicProxy") { serviceType = serviceType.BaseType; }
            if (serviceType == null) { throw new ArgumentNullException(nameof(serviceType)); }

            if (!serviceType.IsSubclassOf(typeof(TransactionalService)))
            {
                throw new IncorrectDevelopmentException($"The service instance attempt using the {nameof(GetServiceInstanceByServiceType)} method failed because the passed type is not a {nameof(TransactionalService)}");
            }

            if (SessionRequest.Services.TryGetValue(serviceType, out var ser))
            {
                return ser as TransactionalService;
            }

            var service = ServiceFactory.Create(serviceType, SessionRequest.LocalHttpContext, SessionRequest);
            SessionRequest.Services.Add(serviceType, service);

            return service;
        }

        public static TransactionalService GetServiceInstanceByEntity(this Type entityType, UserSessionRequest SessionRequest)
        {
            if (entityType?.IsSubclassOf(typeof(FluenteEntidade)) != true)
            {
                throw new IncorrectDevelopmentException($"The service instance attempt using the {nameof(GetServiceInstanceByEntity)} method failed because the passed type is not a {nameof(FluenteEntidade)}");
            }

            var type = (Setup.Config.Config.GenericServiceType) ?? typeof(FluenteService<>);
            var serviceType = type.MakeGenericType(entityType).GetSpecializedService();
            return serviceType.GetServiceInstanceByServiceType(SessionRequest);
        }


    }
}
