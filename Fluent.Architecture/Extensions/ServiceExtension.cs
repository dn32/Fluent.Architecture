
using Fluent.Architecture.Exceptions;
using Fluent.Architecture.Factory;
using Fluent.Architecture.Entities;
using Fluent.Architecture.Services;
using System;

namespace Fluent.Architecture.Extensions
{
    public static class ServiceExtension
    {
        public static TransactionalService GetServiceInstanceByServiceType(this Type serviceType, UserSessionRequest SessionRequest)
        {
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
            if (!entityType.IsSubclassOf(typeof(FluentEntity)))
            {
                throw new IncorrectDevelopmentException($"The service instance attempt using the {nameof(GetServiceInstanceByEntity)} method failed because the passed type is not a {nameof(FluentEntity)}");
            }
            
            var serviceType = typeof(FluentService<>).MakeGenericType(entityType).GetSpecializedService();
            return serviceType.GetServiceInstanceByServiceType(SessionRequest);
        }


    }
}
