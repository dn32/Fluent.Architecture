using dn32.infra.nucleo.excecoes;
using dn32.infra.Filters;
using dn32.infra.Nucleo.Interfaces;
using dn32.infra.Nucleo.Models;
using dn32.infra.Nucleo.Services;
using dn32.infra.Services;
using dn32.infra.Specifications;
using dn32.infra.Util;
using dn32.infra.Validation;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
using dn32.infra.dados;
using dn32.infra.nucleo.controladores;

[assembly: InternalsVisibleTo(@"dn32.infra.EntityFramework, PublicKey=00240000048000009400000006020000002400005253413100040000010001000da51e0f449f6ee7879b256b497e9f64eda760b5fac3d47a4ba8a54664303024f451098b69154691fad078fe77ee79ac2b6a9770fd7a6555a4c49a2a58e82f411939e1eb44ac4a1327acdd13f2c8ec7698644d019f04197838434be8cb53877f1d22acab90ae7735acc363fdb393a11fa34afe780d1c5fb26f37a8fd6e4d9b9f")]
[assembly: InternalsVisibleTo(@"dn32.infra.Doc, PublicKey=00240000048000009400000006020000002400005253413100040000010001008963bf4072062c4090dd8b8b1b3335b78ac84c4e55c7903a918af1d62ecf0e2ab5504ca1fa722b67f5968cdbbf2f1436cc9303018d57511caefbae6cf903f681d721a1122bcdc4f35fa4aafade1e9900468a69aba391d3e9c2eb3087bd37727bbcc30f704666c62beccdca492d8e5467088b696c39306fa582637041a8c40dc4")]
namespace dn32.infra
{
    public static class Setup
    {
        #region PROPERTIES


        internal static IServiceCollection ClientServices { get; set; }

        public static IServiceProvider ServiceProvider { get; set; }

        private static readonly object LockInitialization = new object();

        internal static Dictionary<Type, Type> Services { get; set; }

        internal static Dictionary<Type, Type> Repositories { get; set; }

        internal static Dictionary<Type, Type> Validations { get; set; }

        public static Dictionary<Type, Type> Model { get; private set; }

        public static Dictionary<Type, Type> Controllers { get; private set; }

        public static bool Initialized { get; set; }

        internal static Dictionary<Guid, UserSessionRequest> UserSessionList { get; set; }

        public static IConfigValidate Config { get; set; }

        #endregion

        #region PUBLIC METHODS

        public static List<Type> GetDnApiEntity()
        {
            return Setup.Model.Values.ToList().Where(x => !x.IsAbstract && x.IsPublic).ToList();
        }

        public static Config SetGenericServiceType(this Config configClass, Type serviceType)
        {
            if (configClass != null)
            {
                configClass.GenericServiceType = serviceType;
            }

            return configClass;
        }

        public static Config SetGenericRepositoryType(this Config configClass, Type repositoryType)
        {
            if (configClass != null)
            {
                configClass.GenericRepositoryType = repositoryType;
            }

            return configClass;
        }

        public static Config SetGenericValidationType(this Config configClass, Type ValidationType)
        {
            if (configClass != null)
            {
                configClass.GenericValidationType = ValidationType;
            }

            return configClass;
        }

        public static Config SetGenericControllerType(this Config configClass, Type controllerType)
        {
            if (configClass != null)
            {
                configClass.GenericControllerType = controllerType;
            }

            return configClass;
        }

        public static Config UseJwt<Service>(this Config configClass, DnJwtInfo jwtInfo) where Service : DnAuthenticationService
        {
            configClass.JwtInfo = jwtInfo;
            configClass.JwtInfo.DnAuthenticationServiceType = typeof(Service);
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
            if (configClass != null)
            {
                configClass.UserSessionRequestType = userSessionRequestType;
            }

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
            if (configClass == null)
            {
                return configClass;
            }

            if (configClass.Connections == null)
            {
                configClass.Connections = new List<Connection>();
            }

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
                LoadAssemblies();

                InitValidations();
            }
        }

        internal static List<Type> AllTypes { get; set; }

        private static void ObjectInit()
        {
            Initialized = true;
            Services = new Dictionary<Type, Type>();
            Repositories = new Dictionary<Type, Type>();
            Validations = new Dictionary<Type, Type>();
            Model = new Dictionary<Type, Type>();
            Controllers = new Dictionary<Type, Type>();
            UserSessionList = new Dictionary<Guid, UserSessionRequest>();
            Services.Add(typeof(DnEntidade), typeof(Services.DnService<DnEntidade>));
            Repositories.Add(typeof(DnEntidade), typeof(IDnRepository<DnEntidade>));
            Validations.Add(typeof(DnEntidade), typeof(DnValidation<DnEntidade>));
            Controllers.Add(typeof(DnEntidade), typeof(DnControlador<DnEntidade>));
        }

        private static void LoadAssemblies()
        {
            AllTypes = AppDomain.CurrentDomain.GetAssemblies()
                                    .Where(x => !x.IsDynamic)
                                    .OrderBy(x => x.FullName)
                                    .SelectMany(x => x.ExportedTypes)
                                    .ToList();
        }

        private static void InitValidations()
        {
            var types = AllTypes;
            var transactionalServices = types.Where(x => x.IsSubclassOf(typeof(TransactionalService))).ToList();

            ValidateIfAllServicePropertiesNotHaveTheSetMethod(transactionalServices);
            ValidateIfAllServicePropertiesAreVirtual(transactionalServices);
            ValidateIfAllServicePropertiesNotHavePublic(transactionalServices);
            ValidateIfAllServicePropertiesHaveDefaultConstructor(transactionalServices);

            ValidateSpecifications(types.Where(x => x.IsSubclassOf(typeof(BaseSpecification))).ToList());
            ValidateController(types.Where(x => x.IsSubclassOf(typeof(DnControladorBase))).ToList());

            types.Select(x => GlobalUtil.GetDnEntityType(x, typeof(Services.DnService<EntidadeBase>)))
                .Where(x => x.Item1 != null).ToList()
                .ForEach(AddService);

            types.Select(x => GlobalUtil.GetDnEntityTypeByInterface(x, typeof(IDnRepository<EntidadeBase>)))
               .Where(x => x?.Item1 != null).ToList()
               .ForEach(AddRepository);

            types.Select(x => GlobalUtil.GetDnEntityType(x, typeof(DnValidation<EntidadeBase>)))
               .Where(x => x.Item1 != null).ToList()
               .ForEach(AddValidation);

            types.Select(x => GlobalUtil.GetDnEntityType(x, typeof(EntidadeBase)))
                .Where(x => x.Item1 != null && x.Item2 != typeof(EntidadeBase)).ToList()
                .ForEach(AddModel);

            types.Select(x => GlobalUtil.GetDnEntityType(x, typeof(DnControlador<EntidadeBase>)))
               .Where(x => x.Item1 != null).ToList()
               .ForEach(AddController);

            // Todo - Não me recordo o motivo de estar comentado, mas acredito que tenha que descomentar
            // ValidateIfAllMethodsAreVirtual(Services.Valores.ToList()); // To intercept
            // ValidateIfAllMethodsAreVirtual(Repositories.Valores.ToList()); // To intercept
            // ValidateIfAllMethodsAreVirtual(Validations.Valores.ToList()); //It is not necessary
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
                    throw new DesenvolvimentoIncorretoException($"A specification can not have a parameterized constructor {type}");
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
                    throw new DesenvolvimentoIncorretoException($"A controller can not have public methods that receive specifications as a parameter {type}");
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
                    throw new DesenvolvimentoIncorretoException($"Every repository must have an empty constructor. {type}");
                }
            }
        }

        private static void ValidateIfAllServicePropertiesNotHaveTheSetMethod(IEnumerable<Type> types)
        {
            foreach (var type in types)
            {
                if (type == null) { continue; }
                var serviceProperties = type.GetProperties(BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.FlattenHierarchy)
                    .Where(x => x.GetMethod?.IsPrivate == false && x.GetMethod?.IsVirtual == true && x.PropertyType.IsSubclassOf(typeof(BaseService))).ToList();

                if (serviceProperties == null)
                {
                    continue;
                }

                foreach (var prop in serviceProperties)
                {
                    if (prop.SetMethod != null)
                    {
                        throw new DesenvolvimentoIncorretoException($"The property {type}.{prop.Name} has a set method. Servico properties are not allowed to have the set method.");
                    }
                }
            }
        }

        private static void ValidateIfAllServicePropertiesAreVirtual(IEnumerable<Type> types)
        {
            foreach (var type in types)
            {
                var serviceProperties = type?.GetProperties(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.FlattenHierarchy)
                    .Where(x => x.GetMethod?.IsVirtual == false && x.PropertyType.IsSubclassOf(typeof(BaseService))).ToList();

                if (serviceProperties != null && serviceProperties.Any())
                {
                    throw new DesenvolvimentoIncorretoException($"All service properties must be protected virtual. {type}.{serviceProperties.First().Name}");
                }
            }
        }

        private static void ValidateIfAllServicePropertiesNotHavePublic(IEnumerable<Type> types)
        {
            foreach (var type in types)
            {
                var serviceProperties = type?.GetProperties(BindingFlags.Public | BindingFlags.Instance | BindingFlags.FlattenHierarchy)
                    .Where(x => x.GetMethod?.IsPublic == true && x.PropertyType.IsSubclassOf(typeof(BaseService))).ToList();

                if (serviceProperties != null && serviceProperties.Any())
                {
                    throw new DesenvolvimentoIncorretoException($"All repository properties must be protected virtual.{type}.{serviceProperties.First().Name}");
                }
            }
        }

        private static void AddModel(Tuple<Type, Type> service)
        {
            if (Model.ContainsKey(service.Item2))
            {
                throw new DesenvolvimentoIncorretoException($"There are two entity classes with the same Nome {service.Item2.Name}. This is not allowed.");
            }

            Model.Add(service.Item2, service.Item2);
        }

        private static void AddService(Tuple<Type, Type> service)
        {
            if (Services.ContainsKey(service.Item1))
            {
                throw new DesenvolvimentoIncorretoException($"There are two service classes with the same Nome {service.Item1} -  {service.Item2}. This is not allowed.");
            }

            Services.Add(service.Item1, service.Item2);
        }

        private static void AddValidation(Tuple<Type, Type> validation)
        {
            if (Validations.ContainsKey(validation.Item1))
            {
                throw new DesenvolvimentoIncorretoException($"There are two validation classes with the same Nome {validation.Item1} - {validation.Item2}. This is not allowed.");
            }

            Validations.Add(validation.Item1, validation.Item2);
        }

        private static void AddController(Tuple<Type, Type> controller)
        {
            if (Controllers.ContainsKey(controller.Item1))
            {
                throw new DesenvolvimentoIncorretoException($"There are two controller classes with the same Nome {controller.Item1} - {controller.Item2}. This is not allowed.");
            }

            Controllers.Add(controller.Item1, controller.Item2);
        }

        private static void AddRepository(Tuple<Type, Type> repository)
        {
            if (Repositories.ContainsKey(repository.Item1))
            {
                throw new DesenvolvimentoIncorretoException($"There are two entity repository with the same Nome {repository.Item1} - {repository.Item2}. This is not allowed.");
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
                    if (method.IsPublic && method.ReturnType.Name == typeof(IEnumerable<DnEntidade>).Name || method.ReturnType.Name == typeof(IQueryable<DnEntidade>).Name)
                    {
                        throw new DesenvolvimentoIncorretoException($"The use of non-materialized returns in repositories is not allowed. Change the return type and execute the ToList before the return in the {name}.");
                    }

                    var parameters = method.GetParameters().Select(x => x.ParameterType).ToList();
                    foreach (var parameter in parameters)
                    {
                        if (parameter.Name.StartsWith("Func", StringComparison.CurrentCultureIgnoreCase) && method.Name != "RawSqlQuery")
                        {
                            throw new DesenvolvimentoIncorretoException($"You should not use Func as the input parameter of the repository methods, since Func requires the materialization of the entire list of entities. {name}");
                        }
                    }
                }
            }
        }

        #endregion
    }
}