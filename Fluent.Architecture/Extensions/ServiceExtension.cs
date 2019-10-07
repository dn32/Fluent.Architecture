using Fluent.Architecture.Exceptions;
using Fluent.Architecture.Core.Models;
using Fluent.Architecture.Factory;
using Fluent.Architecture.Services;
using System;

namespace Fluent.Architecture.Extensions
{
    public static class ServiceExtension
    {
        public static TransactionalService GetServiceInstanceByServiceType(this Type serviceType, UserSessionRequest SessionRequest)
        {
            if (serviceType == null) { throw new ArgumentNullException(nameof(serviceType)); }
            if (SessionRequest == null) { throw new ArgumentNullException(nameof(SessionRequest)); }

            if (serviceType.Name == "FluentDynamicProxy")
            {
                serviceType = serviceType.BaseType;
            }

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
            if (entityType?.IsSubclassOf(typeof(FluentEntity)) != true)
            {
                throw new IncorrectDevelopmentException($"The service instance attempt using the {nameof(GetServiceInstanceByEntity)} method failed because the passed type is not a {nameof(FluentEntity)}");
            }

            var type = (Setup.Config.Config.GenericServiceType) ?? typeof(FluentService<>);
            var serviceType = type.MakeGenericType(entityType).GetSpecializedService();
            return serviceType.GetServiceInstanceByServiceType(SessionRequest);
        }


    }
}
