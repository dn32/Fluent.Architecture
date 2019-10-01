// -----------------------------------------------------------------------
// <copyright company="Fluent System">
//     Copyright © Fluent System. All rights reserved.
//     TODOS OS DIREITOS RESERVADOS.
// </copyright>
// -----------------------------------------------------------------------

// ReSharper disable CommentTypo
using Fluent.Architecture.Controllers;
using Fluent.Architecture.Core.Attributes;
using Fluent.Architecture.Core.Factory;
using Fluent.Architecture.Core.Interfaces;
using Fluent.Architecture.Entities;
using Fluent.Architecture.Exceptions;
using Fluent.Architecture.Services;
using Fluent.Architecture.Specifications;
using Fluent.Architecture.Util;
using Fluent.Architecture.Validation;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;

[assembly: InternalsVisibleTo(@"Fluent.Architecture.EntityFramework, PublicKey=002400000480000094000000060200000024000052534131000400000100010001e5fbcd7e6f1d70524fc7b787a6ba4d8f332e822c5506e1831f4e59ab41e930c56bbf8cc29fa91f1270f4e873c036335c5aa4ccfc76ab13bfa7372de9d4e17de6c2d188fae9e6842d7d90d51e123836fd9f5d6be5580a32d1a12e59489519c6b93cdcf7ecd782042db1f31190350fbf937bbd6a5ae61d648773b46b9a706ccf")]
namespace Fluent.Architecture
{
    public static class Setup
    {
        #region PROPERTIES


    internal class ConfigClassValidado : IConfigValidate
    {
        public Config Config { get; set; }

        public ConfigClassValidado()
        {
            Config = new Config();
        }
    }

    public class Config
    {
        public List<Connection> Connections { get; private set; }
        public IServiceProvider ServiceProvider { get; internal set; }
        public Type UserSessionRequestType { get; internal set; }
        public Type GenericServiceType { get; internal set; }
        public Type GenericRepositoryType { get; internal set; }
        public Type GenericValidationType { get; internal set; }
        internal IRepositoryFactory RepositoryFactory { get; set; }

        public Config()
        {
            Connections = new List<Connection>();
        }
    }

    public static class Setup
    {
        #region PROPERTIES

        public static IServiceProvider ServiceProvider { get; set; }

        private static readonly object LockInitialization = new object();

        internal static Dictionary<Type, Type> Services { get; set; }

        internal static Dictionary<Type, Type> Repositories { get; set; }

        internal static Dictionary<Type, Type> Validations { get; set; }

        public static Dictionary<Type, Type> Model { get; private set; }

        public static Dictionary<Type, Type> Controllers { get; private set; }

        public static bool Initialized { get; set; }

        internal static Dictionary<Guid, UserSessionRequest> UserSessionList { get; set; }

        internal static IConfigValidate Config { get; set; }

        #endregion

        #region PUBLIC METHODS

        public static List<Type> GetFluentApiEntity()
        {
            var entities = Setup.Model.Values.ToList().Where(x => !x.IsAbstract && x.IsPublic).ToList();
            var list = entities
           .SelectMany(x => x.GetProperties()
                             .Select(p => new { type = p.PropertyType, attr = p.GetCustomAttribute<FluentCompositionAttribute>() }))
                             .Where(x => x.attr != null)
                             .Select(x => x.type.Name == "List`1" ? x.type.GenericTypeArguments[0] : x.type)
                             .ToList();

            return entities.Where(x => !list.Any(y => y.Name == x.Name)).ToList();
        }

        public static Config SetGenericServiceType(this Config configClass, Type serviceType)
        {
            configClass.GenericServiceType = serviceType;
            return configClass;
        }

        public static Config SetGenericRepositoryType(this Config configClass, Type repositoryType)
        {
            configClass.GenericRepositoryType = repositoryType;
            return configClass;
        }

        public static Config SetGenericValidationType(this Config configClass, Type ValidationType)
        {
            configClass.GenericValidationType = ValidationType;
            return configClass;
        }

        public static Config SetGenericControllerType(this Config configClass, Type controllerType)
        {
            configClass.GenericControllerType = controllerType;
            return configClass;
        }

        internal static Config SetRepositoryFactory(this Config configClass, IRepositoryFactory repositoryFactory)
        {
            configClass.RepositoryFactory = repositoryFactory;
            return configClass;
        }

        public static Config Init()
        {
            ConfigInstance ??= new Config();
            return ConfigInstance;
        }

        internal static Config ConfigInstance { get; set; }

        public static Config SetUserSessionRequestType(this Config configClass, Type userSessionRequestType)
        {
            //Todo - checar se o tipo informado é um UserSessionRequest
            configClass.UserSessionRequestType = userSessionRequestType;
            return configClass;
        }

        public static Config AddConnectionString(
                this Config configClass,
                string connectionString,
                bool createDatabaseIfNotExists,
                Type dbContextType,
                string identifier = "")
        {
            return configClass.AddConnectionString(_ => connectionString, createDatabaseIfNotExists, dbContextType, identifier);
        }

        public static Config AddConnectionString(
                this Config configClass,
                Func<UserSessionRequest, string> getConnectionString,
                bool createDatabaseIfNotExists,
                Type dbContextType,
                string identifier = "")
        {
            configClass.Connections.Add(
                new Connection
                {
                    GetConnectionString = getConnectionString,
                    DbContextType = dbContextType,
                    Identifier = identifier,
                    CreateDatabaseIfNotExists = createDatabaseIfNotExists
                });

            return configClass;
        }

        //Todo no boot da aplicação, checar se os tipos de contexto possuem o atrubuto do tipo de BD
        //Todo - checar ainda se não tem identificador igual

        public static IServiceCollection Build(this Config configClass)
        {
            var configClassValidado = new ConfigClassValidado
            {
                Config = configClass
            };

            Config = configClassValidado;

            return ClientServices;
        }

        internal static void InternalInitialize()
        {
            lock (LockInitialization)
            {
                if (Initialized)
                {
                    return;
                }

                ObjectInit();

                var typeList = LoadAssemblies();

                InitValidations(typeList);
            }
        }

        private static void ObjectInit()
        {
            Initialized = true;
            Services = new Dictionary<Type, Type>();
            Repositories = new Dictionary<Type, Type>();
            Validations = new Dictionary<Type, Type>();
            Model = new Dictionary<Type, Type>();
            Controllers = new Dictionary<Type, Type>();
            UserSessionList = new Dictionary<Guid, UserSessionRequest>();
            Services.Add(typeof(FluentEntity), typeof(FluentService<FluentEntity>));
            Repositories.Add(typeof(FluentEntity), typeof(IFluentRepository<FluentEntity>));
            Validations.Add(typeof(FluentEntity), typeof(FluentValidation<FluentEntity>));
            Controllers.Add(typeof(FluentEntity), typeof(FluentController<FluentEntity>));
        }

        private static List<Type[]> LoadAssemblies()
        {
            var assemblies = AppDomain.CurrentDomain.GetAssemblies().OrderBy(x => x.FullName).ToList();
            var typeList = new List<Type[]>();

            foreach (var assembly in assemblies)
            {
                try
                {
                    if (assembly.IsDynamic) { continue; }
                    typeList.Add(assembly.ExportedTypes.ToArray());
                }
                catch
                {
                    continue;
                }
            }

            return typeList;
        }

        private static void InitValidations(List<Type[]> typeList)
        {
            foreach (var types in typeList)
            {
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

                types.Select(x => GlobalUtil.GetFluentEntityTypeByInterface(x, typeof(IFluentRepository<BaseEntity>)))
                   .Where(x => x?.Item1 != null).ToList()
                   .ForEach(AddRepository);

                types.Select(x => GlobalUtil.GetFluentEntityType(x, typeof(FluentValidation<BaseEntity>)))
                   .Where(x => x.Item1 != null).ToList()
                   .ForEach(AddValidation);

                types.Select(x => GlobalUtil.GetFluentEntityType(x, typeof(BaseEntity)))
                    .Where(x => x.Item1 != null && x.Item2 != typeof(BaseEntity)).ToList()
                    .ForEach(AddModel);

                types.Select(x => GlobalUtil.GetFluentEntityType(x, typeof(FluentController<BaseEntity>)))
                   .Where(x => x.Item1 != null).ToList()
                   .ForEach(AddController);

            }

            // Todo - Não me recordo o motivo de estar comentado, mas acredito que tenha que descomentar
            // ValidateIfAllMethodsAreVirtual(Services.Values.ToList()); // To intercept
            // ValidateIfAllMethodsAreVirtual(Repositories.Values.ToList()); // To intercept
            // ValidateIfAllMethodsAreVirtual(Validations.Values.ToList()); //It is not necessary
            CheckErrorInTheRepository(Repositories.Values.ToList());

            // DbSetup(createDatabaseIfNotExists);
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
                    .Where(x => x.GetMethod != null && !x.GetMethod.IsPrivate && x.GetMethod.IsVirtual && x.PropertyType.IsSubclassOf(typeof(BaseService))).ToList();

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
                    .Where(x => x.GetMethod != null && !x.GetMethod.IsVirtual && x.PropertyType.IsSubclassOf(typeof(BaseService))).ToList();

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
                var serviceProperties = type.GetProperties(BindingFlags.Public | BindingFlags.Instance | BindingFlags.FlattenHierarchy)
                    .Where(x => x.GetMethod != null && x.GetMethod.IsPublic && x.PropertyType.IsSubclassOf(typeof(BaseService))).ToList();

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
            if (Services.ContainsKey(service.Item1))
            {
                throw new IncorrectDevelopmentException($"There are two service classes with the same name {service.Item1} -  {service.Item2}. This is not allowed.");
            }

            Services.Add(service.Item1, service.Item2);
        }

        private static void AddValidation(Tuple<Type, Type> validation)
        {
            if (Validations.ContainsKey(validation.Item1))
            {
                throw new IncorrectDevelopmentException($"There are two validation classes with the same name {validation.Item1} - {validation.Item2}. This is not allowed.");
            }

            Validations.Add(validation.Item1, validation.Item2);
        }

        private static void AddController(Tuple<Type, Type> controller)
        {
            if (Controllers.ContainsKey(controller.Item1))
            {
                throw new IncorrectDevelopmentException($"There are two controller classes with the same name {controller.Item1} - {controller.Item2}. This is not allowed.");
            }

            Controllers.Add(controller.Item1, controller.Item2);
        }

        private static void AddRepository(Tuple<Type, Type> repository)
        {
            if (Repositories.ContainsKey(repository.Item1))
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
                        if (parameter.Name.StartsWith("Func", StringComparison.CurrentCultureIgnoreCase) && method.Name != "RawSqlQuery")
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