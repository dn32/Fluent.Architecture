// -----------------------------------------------------------------------
// <copyright company="Fluent System">
//     Copyright © Fluent System. All rights reserved.
//     TODOS OS DIREITOS RESERVADOS.
// </copyright>
// -----------------------------------------------------------------------

// ReSharper disable CommentTypo
#if NET461
using System.Data.Entity;
using System.Management.Instrumentation;

#else
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

#endif

using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Fluent.Architecture.Model;
using Fluent.Architecture.Repository;
using Fluent.Architecture.Services;
using Fluent.Architecture.Util;
using Fluent.Architecture.Validation;
using Fluent.Architecture.Controllers;
using Fluent.Architecture.Exceptions;
using Fluent.Architecture.Specifications;
using Fluent.Architecture.Factory;
using Fluent.Architecture.Core.Interfaces;

namespace Fluent.Architecture
{
    public static class Setup
    {
        #region PROPERTIES

#if !NET461
        public static IServiceProvider ServiceProvider { get; set; }
#endif

        private static readonly object LockInitialization = new object();

        internal static Type TransactionObjectsType { get; set; }

        internal static GlobalizationService GlobalizationService { get; set; }

        internal static Dictionary<Type, Type> Services { get; set; }

        internal static Dictionary<Type, Type> Repositories { get; set; }

        internal static Dictionary<Type, Type> Validations { get; set; }

        public static Dictionary<Type, Type> Model { get; private set; }

        public static bool Initialized { get; set; }

        internal static Dictionary<Guid, UserSessionRequest> UserSessionList { get; set; }

#endregion

#region PUBLIC METHODS

        /// <summary>
        /// Permite definir um tipo para TransactionObjectsType que é o contexto da aplicação referente ao banco de dados.
        /// Muito útil para o controle de mock de testes automatizados.
        /// </summary>
        /// <param name="transactionObjectsType">
        /// O tipo a ser definido.
        /// </param>
        public static void SetCustomTypes(Type transactionObjectsType)
        {
            TransactionObjectsType = transactionObjectsType;
        }

        public static void SetGlobalizationServiceType<TS>(object httpContext) where TS : GlobalizationService, new()
        {
            GlobalizationService = ServiceFactory.Create<TS>(httpContext);
        }

        public static void DbSetup(bool createDatabaseIfNotExists)
        {
            // Todo - Arrumar essa implementação
#if NET461
            //if (createDatabaseIfNotExists)
            //{
            //    Database.SetInitializer(new CreateDatabaseIfNotExists<EfContext>());
            //}
            //else
            //{
            //    Database.SetInitializer<EfContext>(null);
            //}
#else
            //if (createDatabaseIfNotExists)
            //{
            //        var context = ServiceProvider.GetRequiredService<EfContext>();
            //        context.Database.Migrate();
            //}
#endif
        }


#if NET461
        public static void Initialize(string connectionString, bool createDatabaseIfNotExists = true)
        {
            InternalInitialize(connectionString, createDatabaseIfNotExists);
        }
#else
        public static void Initialize(IServiceProvider serviceProvider, string connectionString, bool createDatabaseIfNotExists = true)
        {
            ServiceProvider = serviceProvider;
            InternalInitialize(connectionString, createDatabaseIfNotExists);
        }
#endif

        private static void InternalInitialize(string connectionString, bool createDatabaseIfNotExists = true)
        {
            lock (LockInitialization)
            {
                if (Initialized)
                {
                    return;
                }

                Initialized = true;

                // Todo - Avaliar impacto de separação de pacote
               // TransactionObjects.DataBaseConnectionString = connectionString;

                Services = new Dictionary<Type, Type>();
                Repositories = new Dictionary<Type, Type>();
                Validations = new Dictionary<Type, Type>();
                Model = new Dictionary<Type, Type>();
                UserSessionList = new Dictionary<Guid, UserSessionRequest>();
                // Todo - Avaliar impacto de separação de pacote
               // TransactionObjectsType = typeof(TransactionObjects);

                Services.Add(typeof(FluentEntity), typeof(FluentService<FluentEntity>));
                // Todo - Avaliar impacto de separação de pacote
               // Repositories.Add(typeof(FluentEntity), typeof(FluentRepository<FluentEntity>));
                Validations.Add(typeof(FluentEntity), typeof(FluentValidation<FluentEntity>));

                var assemblies = AppDomain.CurrentDomain.GetAssemblies().OrderBy(x => x.FullName).ToList();
                foreach (var assembly in assemblies)
                {
                    Type[] types;

                    try
                    {
                        types = assembly.GetTypes();
                    }
#pragma warning disable CA1031 // Do not catch general exception types
                    catch
                    {
                        continue;
                    }
#pragma warning restore CA1031 // Do not catch general exception types

                    var transactionalServices = types.Where(x => x.IsSubclassOf(typeof(TransactionalService))).ToList();

                    ValidateIfAllServicePropertiesNotHaveTheSetMethod(transactionalServices);
                    ValidateIfAllServicePropertiesAreVirtual(transactionalServices);
                    ValidateIfAllServicePropertiesNotHavePublic(transactionalServices);
                    ValidateIfAllServicePropertiesHaveDefaultConstructor(transactionalServices);

                    ValidateSpecifications(types.Where(x => x.IsSubclassOf(typeof(BaseSpecification))).ToList());
                    ValidateController(types.Where(x => x.IsSubclassOf(typeof(BaseController))).ToList());

                    types.Select(x => GlobalUtil.GetFluentEntityType(x, typeof(FluentService<BaseEntity>)))
                        .Where(x => x.Item1 != null).ToList()
                        .ForEach(AddService);

                    types.Select(x => GlobalUtil.GetFluentEntityType(x, typeof(IFluentRepository<BaseEntity>)))
                       .Where(x => x.Item1 != null).ToList()
                       .ForEach(AddRepository);

                    types.Select(x => GlobalUtil.GetFluentEntityType(x, typeof(FluentValidation<BaseEntity>)))
                       .Where(x => x.Item1 != null).ToList()
                       .ForEach(AddValidation);

                    types.Select(x => GlobalUtil.GetFluentEntityType(x, typeof(BaseEntity)))
                        .Where(x => x.Item1 != null && x.Item2 != typeof(BaseEntity)).ToList()
                        .ForEach(AddModel);
                }

                // ValidateIfAllMethodsAreVirtual(Services.Values.ToList()); // To intercept
                // ValidateIfAllMethodsAreVirtual(Repositories.Values.ToList()); // To intercept
                // ValidateIfAllMethodsAreVirtual(Validations.Values.ToList()); //It is not necessary
                CheckErrorInTheRepository(Repositories.Values.ToList());

                DbSetup(createDatabaseIfNotExists);
            }
        }

        //Todo testar
        private static void ValidateSpecifications(List<Type> specs)
        {
            specs.ForEach(type =>
            {
                if (type.GetConstructors().Any(x => x.GetParameters().Any()))
                {
                    throw new IncorrectDevelopmentException($"A specification can not have a parameterized constructor {type}");
                }
            });
        }

        //Todo testar
        private static void ValidateController(List<Type> controllers)
        {
            controllers.ForEach(type =>
            {
                if (type.GetMethods().Any(x => x.IsPublic && x.GetParameters().Any(y => y.ParameterType.IsSubclassOf(typeof(BaseSpecification)))))
                {
                    throw new IncorrectDevelopmentException($"A controller can not have public methods that receive specifications as a parameter {type}");
                }
            });
        }

#endregion

#region INTERNAL METHODS

        internal static UserSessionRequest GetUserRequestSession(Guid sessionIdGuid)
        {
            if (!UserSessionList.TryGetValue(sessionIdGuid, out var userSession))
            {
                throw new Exception("UserSessionRequest not found!");
            }

            return userSession;
        }

        internal static void AddSession(UserSessionRequest userSessionRequest)
        {
            lock (UserSessionList)
            {
                UserSessionList.Add(userSessionRequest.SessionRequestId, userSessionRequest);
            }
        }

        internal static void RemoveSession(Guid sessionId)
        {
            lock (UserSessionList)
            {
                UserSessionList.Remove(sessionId);
            }
        }

#endregion

#region PRIVATE

        private static void ValidateIfAllServicePropertiesHaveDefaultConstructor(IEnumerable<Type> types)
        {
            foreach (var type in types)
            {
                if (type.IsAbstract)
                {
                    continue;
                }

                var generic = type.GetGenericArguments();
                if (generic.Length > 0 && generic.First().Name == "T")
                {
                    continue;
                }

                var defaultConstructor = type.GetConstructors().Any(x => !x.GetParameters().Any());
                if (!defaultConstructor)
                {
                    throw new IncorrectDevelopmentException($"Every repository must have an empty constructor. {type}");
                }
            }
        }

        private static void ValidateIfAllServicePropertiesNotHaveTheSetMethod(IEnumerable<Type> types)
        {
            foreach (var type in types)
            {
                var serviceProperties = type?.GetProperties(BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.FlattenHierarchy)
                    .Where(x => !x.GetMethod.IsPrivate && x.GetMethod.IsVirtual && x.PropertyType.IsSubclassOf(typeof(BaseService))).ToList();

                if (serviceProperties == null)
                {
                    continue;
                }

                foreach (var prop in serviceProperties)
                {
                    if (prop.SetMethod != null)
                    {
                        throw new IncorrectDevelopmentException($"The property {type}.{prop.Name} has a set method. Service properties are not allowed to have the set method.");
                    }
                }
            }
        }

        private static void ValidateIfAllServicePropertiesAreVirtual(IEnumerable<Type> types)
        {
            foreach (var type in types)
            {
                var serviceProperties = type?.GetProperties(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.FlattenHierarchy)
                    .Where(x => !x.GetMethod.IsVirtual && x.PropertyType.IsSubclassOf(typeof(BaseService))).ToList();

                if (serviceProperties != null && serviceProperties.Any())
                {
                    throw new IncorrectDevelopmentException($"All service properties must be protected virtual. {type}.{serviceProperties.First().Name}");
                }
            }
        }

        private static void ValidateIfAllServicePropertiesNotHavePublic(IEnumerable<Type> types)
        {
            foreach (var type in types)
            {
                var serviceProperties = type?.GetProperties(BindingFlags.Public | BindingFlags.Instance | BindingFlags.FlattenHierarchy)
                    .Where(x => x.GetMethod.IsPublic && x.PropertyType.IsSubclassOf(typeof(BaseService))).ToList();

                if (serviceProperties != null && serviceProperties.Any())
                {
                    throw new IncorrectDevelopmentException($"All repository properties must be protected virtual.{type}.{serviceProperties.First().Name}");
                }
            }
        }

        private static void AddModel(Tuple<Type, Type> service)
        {
            if (Model.ContainsKey(service.Item2))
            {
                throw new IncorrectDevelopmentException($"There are two entity classes with the same name {service.Item2.Name}. This is not allowed.");
            }

            Model.Add(service.Item2, service.Item2);
        }

        private static void AddService(Tuple<Type, Type> service)
        {
            if (Model.ContainsKey(service.Item1))
            {
                throw new IncorrectDevelopmentException($"There are two service classes with the same name {service.Item1} -  {service.Item2}. This is not allowed.");
            }

            Services.Add(service.Item1, service.Item2);
        }


        private static void AddValidation(Tuple<Type, Type> validation)
        {
            if (Model.ContainsKey(validation.Item1))
            {
                throw new IncorrectDevelopmentException($"There are two validation classes with the same name {validation.Item1} - {validation.Item2}. This is not allowed.");
            }

            Validations.Add(validation.Item1, validation.Item2);
        }

        private static void AddRepository(Tuple<Type, Type> repository)
        {
            if (Model.ContainsKey(repository.Item1))
            {
                throw new IncorrectDevelopmentException($"There are two entity repository with the same name {repository.Item1} - {repository.Item2}. This is not allowed.");
            }

            Repositories.Add(repository.Item1, repository.Item2);
        }

        private static void CheckErrorInTheRepository(IEnumerable<Type> types)
        {
            foreach (var type in types)
            {
                var methods = type.GetMethods(BindingFlags.Instance | BindingFlags.Public);
                foreach (var method in methods)
                {
                    var name = $"{type.Name}.{method.Name}";
                    if (method.IsPublic && method.ReturnType.Name == typeof(IEnumerable<FluentEntity>).Name || method.ReturnType.Name == typeof(IQueryable<FluentEntity>).Name)
                    {
                        throw new IncorrectDevelopmentException($"The use of non-materialized returns in repositories is not allowed. Change the return type and execute the ToList before the return in the {name}.");
                    }

                    var parameters = method.GetParameters().Select(x => x.ParameterType).ToList();
                    foreach (var parameter in parameters)
                    {
                        if (parameter.Name.StartsWith("Func", StringComparison.CurrentCultureIgnoreCase))
                        {
                            throw new IncorrectDevelopmentException($"You should not use Func as the input parameter of the repository methods, since Func requires the materialization of the entire list of entities. {name}");
                        }
                    }
                }
            }
        }

#endregion
    }
}