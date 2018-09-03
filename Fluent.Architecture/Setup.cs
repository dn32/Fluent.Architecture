// ReSharper disable CommentTypo

using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Reflection;
using Fluent.Architecture.Attributes;
using Fluent.Architecture.Enum;
using Fluent.Architecture.Extensions;
using Fluent.Architecture.Model;
using Fluent.Architecture.Repository;
using Fluent.Architecture.Services;
using Fluent.Architecture.Util;
using Fluent.Architecture.Validation;
using System.Management.Instrumentation;
using Fluent.Architecture.Exceptions;

namespace Fluent.Architecture
{
    public static class Setup
    {
        #region PROPERTIES

        private static readonly object LockInitialization = new object();

        internal static Type TransactionObjectsType { get; set; }

        internal static Dictionary<string, Type> Services { get; set; }

        internal static Dictionary<string, Type> Repositories { get; set; }

        internal static Dictionary<string, Type> Validations { get; set; }

        internal static Dictionary<string, Type> Model { get; set; }

        internal static Dictionary<Tuple<EPropagateTypes, string>, MethodInfo> Propagators { get; set; }

        public static bool Initialized { get; set; }

        internal static Dictionary<Guid, UserSessionRequest> UserSessionList { get; set; }

        #endregion

        #region PUBLIC METHODS

         public static void SetCustomTypes(Type transactionObjectsType)
        {
            TransactionObjectsType = transactionObjectsType;
        }

        public static void DbSetup(bool createDatabaseIfNotExists)
        {
            if (createDatabaseIfNotExists)
            {
                Database.SetInitializer(new CreateDatabaseIfNotExists<EfContext>());
            }
            else
            {
                Database.SetInitializer<EfContext>(null);
            }
        }

        public static void Initialize(string connectionString, bool createDatabaseIfNotExists = true)
        {
            lock (LockInitialization)
            {
                if (Initialized)
                {
                    return;
                }

                Initialized = true;

                TransactionObjects.DataBaseConnectionString = connectionString;


                Services = new Dictionary<string, Type>();
                Repositories = new Dictionary<string, Type>();
                Validations = new Dictionary<string, Type>();
                Model = new Dictionary<string, Type>();
                Propagators = new Dictionary<Tuple<EPropagateTypes, string>, MethodInfo>();
                UserSessionList = new Dictionary<Guid, UserSessionRequest>();
                TransactionObjectsType = typeof(TransactionObjects);

                Services.Add("base", typeof(FluentService<FluentEntity>));
                Repositories.Add("base", typeof(FluentRepository<FluentEntity>));
                Validations.Add("base", typeof(FluentValidation<FluentEntity>));

                var assemblies = AppDomain.CurrentDomain.GetAssemblies().OrderBy(x => x.FullName).ToList();
                foreach (var assembly in assemblies)
                {
                    Type[] types;

                    try
                    {
                        types = assembly.GetTypes();
                    }
                    catch
                    {
                        continue;
                    }

                    types.Select(x => GlobalUtil.GetFluentEntityType(x, typeof(FluentService<BaseEntity>)))
                        .Where(x => !string.IsNullOrWhiteSpace(x.Item1)).ToList()
                        .ForEach(service => Services.Add(service.Item1, service.Item2));

                    var transactionalServices = types.Where(x => x.IsSubclassOf(typeof(TransactionalService))).ToList();
                    ValidateIfAllServicePropertiesNotHaveTheSetMethod(transactionalServices);
                    ValidateIfAllServicePropertiesAreVirtual(transactionalServices);
                    ValidateIfAllServicePropertiesNotHavePublic(transactionalServices);
                    ValidateIfAllServicePropertiesHaveDefaultConstructor(transactionalServices);

                    types.Select(x => GlobalUtil.GetFluentEntityType(x, typeof(FluentRepository<BaseEntity>)))
                       .Where(x => !string.IsNullOrWhiteSpace(x.Item1)).ToList()
                       .ForEach(service => Repositories.Add(service.Item1, service.Item2));

                    types.Select(x => GlobalUtil.GetFluentEntityType(x, typeof(FluentValidation<BaseEntity>)))
                       .Where(x => !string.IsNullOrWhiteSpace(x.Item1)).ToList()
                       .ForEach(service => Validations.Add(service.Item1, service.Item2));

                    types.Select(x => GlobalUtil.GetFluentEntityType(x, typeof(BaseEntity)))
                     .Where(x => !string.IsNullOrWhiteSpace(x.Item1) && x.Item2 != typeof(BaseEntity)).ToList()
                     .ForEach(AddModel);
                }

                // ValidateIfAllMethodsAreVirtual(Services.Values.ToList()); // To intercept
                // ValidateIfAllMethodsAreVirtual(Repositories.Values.ToList()); // To intercept
                // ValidateIfAllMethodsAreVirtual(Validations.Values.ToList()); //It is not necessary
                CheckErrorInTheRepository(Repositories.Values.ToList());

                FindPropagators(Repositories, EPropagateTypes.Repository);
                FindPropagators(Services, EPropagateTypes.Service);
                FindPropagators(Validations, EPropagateTypes.Validation);

                DbSetup(createDatabaseIfNotExists);
            }
        }

        #endregion

        #region INTERNAL METHODS

        internal static UserSessionRequest GetUserRequestSession(Guid sessionIdGuid)
        {
            if (!UserSessionList.TryGetValue(sessionIdGuid, out var userSession))
            {
                throw new InstanceNotFoundException("UserSessionRequest not fount!");
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
                    throw new IncorrectDevelopmentException($"Every service must have an empty constructor. {type}");
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
                    throw new IncorrectDevelopmentException($"All service properties must be protected virtual.{type}.{serviceProperties.First().Name}");
                }
            }
        }

        private static void AddModel(Tuple<string, Type> service)
        {
            if (Model.ContainsKey(service.Item2.Name))
            {
                throw new IncorrectDevelopmentException($"There are two entity classes with the same name {service.Item2.Name}. This is not allowed.");
            }

            Model.Add(service.Item2.Name, service.Item2);
        }

        private static void FindPropagators(Dictionary<string, Type> elements, EPropagateTypes type)
        {
            foreach (var item in elements)
            {
                var methods = item.Value.GetMethods(BindingFlags.Instance | BindingFlags.Public);
                var propagators = methods.Where(x => x.GetCustomAttribute<PropagateAttribute>() != null);

                foreach (var method in propagators)
                {
                    var key = new Tuple<EPropagateTypes, string>(type, $"{item.Key} {method.GetFriendlyName()}");
                    Propagators.Add(key, method);
                }
            }
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

        // private static void ValidateIfAllMethodsAreVirtual(IEnumerable<Type> types)
        // {
        // foreach (var item in types)
        // {
        // var methods = item.GetMethods(BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.Public);
        // foreach (var method in methods)
        // {
        // if ((!method.IsPrivate) && !method.IsVirtual && !method.IsFamily && !method.IsSpecialName && !(new[] { "GetType" }.Contains(method.Name)))
        // {
        // throw new IncorrectDevelopmentException($"The {method.ReflectedType.Name}.{method.Name} method must be set to virtual, or private.");
        // }
        // }
        // }
        // }
        #endregion
    }
}