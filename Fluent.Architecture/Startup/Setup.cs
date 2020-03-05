using dn32.infra.nucleo.excecoes;
using dn32.infra.Filters;
using dn32.infra.Nucleo.Interfaces;
using dn32.infra.Nucleo.Models;
using dn32.infra.Nucleo.Services;
using dn32.infra.servicos;
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
using dn32.infra.nucleo.servicos;
using Newtonsoft.Json;
using dn32.infra.extensoes;
using dn32.infra.Nucleo.Factory;
using System.Collections.Concurrent;

[assembly: InternalsVisibleTo(@"dn32.infra.EntityFramework, PublicKey=00240000048000009400000006020000002400005253413100040000010001000da51e0f449f6ee7879b256b497e9f64eda760b5fac3d47a4ba8a54664303024f451098b69154691fad078fe77ee79ac2b6a9770fd7a6555a4c49a2a58e82f411939e1eb44ac4a1327acdd13f2c8ec7698644d019f04197838434be8cb53877f1d22acab90ae7735acc363fdb393a11fa34afe780d1c5fb26f37a8fd6e4d9b9f")]
[assembly: InternalsVisibleTo(@"dn32.infra.Doc, PublicKey=00240000048000009400000006020000002400005253413100040000010001008963bf4072062c4090dd8b8b1b3335b78ac84c4e55c7903a918af1d62ecf0e2ab5504ca1fa722b67f5968cdbbf2f1436cc9303018d57511caefbae6cf903f681d721a1122bcdc4f35fa4aafade1e9900468a69aba391d3e9c2eb3087bd37727bbcc30f704666c62beccdca492d8e5467088b696c39306fa582637041a8c40dc4")]
namespace dn32.infra
{
    public static class Setup
    {
        #region PROPRIEDADES

        public static Dictionary<Type, Type> Modelos { get; private set; }

        public static Dictionary<Type, Type> Controladores { get; private set; }

        public static bool Inicializado { get; set; }

        public static DnConfiguracoesGlobais ConfiguracoesGlobais { get; set; }

        internal static Dictionary<Type, Type> Servicos { get; set; }

        internal static Dictionary<Type, Type> Repositorios { get; set; }

        internal static Dictionary<Type, Type> Validacoes { get; set; }

        internal static ConcurrentDictionary<Guid, SessaoDeRequisicaoDoUsuario> SessoesDeRequisicoesDeUsuarios { get; set; }

        internal static List<Type> TodosOsTipos { get; set; }

        private static object TravaDeInicializacao { get; set; } = new object();

        private static IServiceCollection ServiceCollection { get; set; }

        #endregion

        #region MÉTODOS PÚBLICOS

        #region DEFINIÇÕES

        public static DnConfiguracoesGlobais DefinirTipoGenericoDeServico<S>(this DnConfiguracoesGlobais configuracoes) where S : DnServicoBase
        {
            if (configuracoes != null)
                configuracoes.GenericServiceType = typeof(S);

            return configuracoes;
        }

        public static DnConfiguracoesGlobais DefinirTipoGenericoDeRepositorio<R>(this DnConfiguracoesGlobais configuracoes) where R : ITransactionlRepository
        {
            if (configuracoes != null)
                configuracoes.GenericRepositoryType = typeof(R);

            return configuracoes;
        }

        public static DnConfiguracoesGlobais DefinirTipoGenericoDeValidacao<V>(this DnConfiguracoesGlobais configuracoes) where V : TransactionalValidation
        {
            if (configuracoes != null)
                configuracoes.GenericValidationType = typeof(V);

            return configuracoes;
        }

        public static DnConfiguracoesGlobais DefinirTipoGenericoDeControlador<C>(this DnConfiguracoesGlobais configuracoes) where C : DnControladorBase
        {
            if (configuracoes != null)
                configuracoes.GenericControllerType = typeof(C);

            return configuracoes;
        }

        public static DnConfiguracoesGlobais DefinirTipoGenericoDeSessaoDeRequisicao<T>(this DnConfiguracoesGlobais configuracoes) where T : SessaoDeRequisicaoDoUsuario
        {
            if (configuracoes != null)
                configuracoes.UserSessionRequestType = typeof(T);

            return configuracoes;
        }

        internal static DnConfiguracoesGlobais DefinirFabricaDeRepositorio(this DnConfiguracoesGlobais configuracoes, IRepositoryFactory fabricaDeRepositorio)
        {
            if (configuracoes != null)
                configuracoes.FabricaDeRepositorio = fabricaDeRepositorio;

            return configuracoes;
        }

        #endregion

        public static List<Type> ObterEntidades()
            => Modelos.Values.Where(x => !x.IsAbstract && x.IsPublic).ToList();

        public static DnConfiguracoesGlobais UsarJWT<S>(this DnConfiguracoesGlobais configuracoes, InformacoesDoJWT informacoesDoJWT) where S : DnAuthenticationService
        {
            configuracoes.InformacoesDoJWT = informacoesDoJWT;
            configuracoes.InformacoesDoJWT.DnAuthenticationServiceType = typeof(S);
            return configuracoes;
        }

        public static DnConfiguracoesGlobais AdicionarStringDeConexao<T>(
            this DnConfiguracoesGlobais configuracoes,
            string stringDeConexao,
            bool criarOBancoDeDadosCasoNaoExista,
            Type tipoDoContexto,
            string identificadorDaConexao = "") =>
             configuracoes.AdicionarStringDeConexao(_ => stringDeConexao, criarOBancoDeDadosCasoNaoExista, tipoDoContexto, identificadorDaConexao);

        public static DnConfiguracoesGlobais AdicionarStringDeConexao(
                this DnConfiguracoesGlobais configuracoes,
                Func<SessaoDeRequisicaoDoUsuario, string> obterStringDeConexao,
                bool criarOBancoDeDadosCasoNaoExista,
                Type tipoDoContexto,
                string identificadorDaConexao = "")
        {
            if (configuracoes == null)
            {
                return configuracoes;
            }

            configuracoes.Conexoes.Add(
                new Connection
                {
                    ObterStringDeConexao = obterStringDeConexao,
                    TipoDoContexto = tipoDoContexto,
                    IdentificadorDaConexao = identificadorDaConexao,
                    CriarOBancoDeDadosCasoNaoExista = criarOBancoDeDadosCasoNaoExista
                });

            return configuracoes;
        }

        //Todo no boot da aplicação, checar se os tipos de contexto possuem o atributo do tipo de BD
        //Todo - checar ainda se não tem identificador igual
        public static IServiceCollection Build(this DnConfiguracoesGlobais configuracoes)
        {
            ConfiguracoesGlobais = configuracoes;
            return ServiceCollection;
        }

        public static DnConfiguracoesGlobais AddDnArquitetura(this IMvcBuilder builder, JsonSerializerSettings jsonSerializerSettings)
        {
            if (jsonSerializerSettings is null)
                throw new ArgumentNullException(nameof(jsonSerializerSettings));

            ExtensoesJson.ConfiguracoesDeSerializacao = jsonSerializerSettings;
            ServiceCollection = builder.Services;
            InicializacaoInterna();
            builder.ConfigureApplicationPartManager(apm => apm.FeatureProviders.Add(new ControllerFactory()));

            ConfiguracoesGlobais ??= new DnConfiguracoesGlobais();
            return ConfiguracoesGlobais;
        }

        internal static void InicializacaoInterna()
        {
            lock (TravaDeInicializacao)
            {
                if (Inicializado) { return; }
                InicializarObjetos();
                CarregarAssemblies();
                ExecutarValidacoes();
            }
        }


        private static void InicializarObjetos()
        {
            Inicializado = true;
            Servicos = new Dictionary<Type, Type>();
            Repositorios = new Dictionary<Type, Type>();
            Validacoes = new Dictionary<Type, Type>();
            Modelos = new Dictionary<Type, Type>();
            Controladores = new Dictionary<Type, Type>();
            SessoesDeRequisicoesDeUsuarios = new ConcurrentDictionary<Guid, SessaoDeRequisicaoDoUsuario>();
            Servicos.Add(typeof(DnEntidade), typeof(servicos.DnServico<DnEntidade>));
            Repositorios.Add(typeof(DnEntidade), typeof(IDnRepository<DnEntidade>));
            Validacoes.Add(typeof(DnEntidade), typeof(DnValidation<DnEntidade>));
            Controladores.Add(typeof(DnEntidade), typeof(DnControlador<DnEntidade>));
        }

        private static void CarregarAssemblies()
        {
            TodosOsTipos = AppDomain.CurrentDomain.GetAssemblies()
                                    .Where(x => !x.IsDynamic)
                                    .OrderBy(x => x.FullName)
                                    .SelectMany(x => x.ExportedTypes)
                                    .ToList();
        }

        private static void ExecutarValidacoes()
        {
            var tipos = TodosOsTipos;
            var servicos = tipos.Where(x => x.IsSubclassOf(typeof(DnServicoTransacionalBase))).ToList();

            ValidateIfAllServicePropertiesNotHaveTheSetMethod(servicos);
            ValidateIfAllServicePropertiesAreVirtual(servicos);
            ValidateIfAllServicePropertiesNotHavePublic(servicos);
            ValidateIfAllServicePropertiesHaveDefaultConstructor(servicos);

            ValidarEspecificacoes(tipos.Where(x => x.IsSubclassOf(typeof(BaseSpecification))).ToList());
            ValidarControladores(tipos.Where(x => x.IsSubclassOf(typeof(DnControladorBase))).ToList());

            tipos.Select(x => GlobalUtil.GetDnEntityType(x, typeof(servicos.DnServico<EntidadeBase>)))
                .Where(x => x.Item1 != null).ToList()
                .ForEach(AddService);

            tipos.Select(x => GlobalUtil.GetDnEntityTypeByInterface(x, typeof(IDnRepository<EntidadeBase>)))
               .Where(x => x?.Item1 != null).ToList()
               .ForEach(AddRepository);

            tipos.Select(x => GlobalUtil.GetDnEntityType(x, typeof(DnValidation<EntidadeBase>)))
               .Where(x => x.Item1 != null).ToList()
               .ForEach(AddValidation);

            tipos.Select(x => GlobalUtil.GetDnEntityType(x, typeof(EntidadeBase)))
                .Where(x => x.Item1 != null && x.Item2 != typeof(EntidadeBase)).ToList()
                .ForEach(AddModel);

            tipos.Select(x => GlobalUtil.GetDnEntityType(x, typeof(DnControlador<EntidadeBase>)))
               .Where(x => x.Item1 != null).ToList()
               .ForEach(AddController);

            // Todo - Não me recordo o motivo de estar comentado, mas acredito que tenha que descomentar
            // ValidateIfAllMethodsAreVirtual(Services.Valores.ToList()); // To intercept
            // ValidateIfAllMethodsAreVirtual(Repositories.Valores.ToList()); // To intercept
            // ValidateIfAllMethodsAreVirtual(Validations.Valores.ToList()); //It is not necessary
            CheckErrorInTheRepository(Repositorios.Values.ToList());

            // DbSetup(createDatabaseIfNotExists);
        }

        private static void ValidarEspecificacoes(List<Type> especificacoes)
        {
            especificacoes.ForEach(type =>
            {
                if (type.GetConstructors().Any(x => x.GetParameters().Any()))
                    throw new DesenvolvimentoIncorretoException($"A especificação '{type}' possui parâmetros no construtor e isso não é permitido. Crie um método para passar os parâmetros.");
            });
        }

        private static void ValidarControladores(List<Type> controladores)
        {
            controladores.ForEach(type =>
            {
                if (type.GetMethods().Any(x => x.IsPublic && x.GetParameters().Any(y => y.ParameterType.IsSubclassOf(typeof(BaseSpecification)))))
                    throw new DesenvolvimentoIncorretoException($"O controlador '{type}' possui um ou mais métosos público(s) que recebe(m) uma especificacao como parâmetro. Isso não é permitido.");
            });
        }

        #endregion

        #region MÉTODOS INTERNOS

        internal static SessaoDeRequisicaoDoUsuario ObterSessaoDeUmaRequisicao(Guid identificadorDaSessao)
        {
            if (!SessoesDeRequisicoesDeUsuarios.TryGetValue(identificadorDaSessao, out var sessao))
                throw new Exception($"Não foi encontrada uma sessão de requisição com o identificadorDaSessao: '{identificadorDaSessao}'");

            return sessao;
        }

        internal static void AddSession(SessaoDeRequisicaoDoUsuario sessaoDeRequisicaoDoUsuario) =>
            SessoesDeRequisicoesDeUsuarios.TryAdd(sessaoDeRequisicaoDoUsuario.IdentificadorDaSessao, sessaoDeRequisicaoDoUsuario);
        
        internal static void RemoverSessaoDeRequisicao(Guid sessionId) => SessoesDeRequisicoesDeUsuarios.TryRemove(sessionId, out _);

        #endregion

        #region PRIVATE
        //Todo - Traduzir
        private static void ValidateIfAllServicePropertiesHaveDefaultConstructor(IEnumerable<Type> tipos)
        {
            foreach (var type in tipos)
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
                    .Where(x => x.GetMethod?.IsPrivate == false && x.GetMethod?.IsVirtual == true && x.PropertyType.IsSubclassOf(typeof(DnServicoBase))).ToList();

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
                    .Where(x => x.GetMethod?.IsVirtual == false && x.PropertyType.IsSubclassOf(typeof(DnServicoBase))).ToList();

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
                    .Where(x => x.GetMethod?.IsPublic == true && x.PropertyType.IsSubclassOf(typeof(DnServicoBase))).ToList();

                if (serviceProperties != null && serviceProperties.Any())
                {
                    throw new DesenvolvimentoIncorretoException($"All repository properties must be protected virtual.{type}.{serviceProperties.First().Name}");
                }
            }
        }

        private static void AddModel(Tuple<Type, Type> service)
        {
            if (Modelos.ContainsKey(service.Item2))
            {
                throw new DesenvolvimentoIncorretoException($"There are two entity classes with the same Nome {service.Item2.Name}. This is not allowed.");
            }

            Modelos.Add(service.Item2, service.Item2);
        }

        private static void AddService(Tuple<Type, Type> service)
        {
            if (Servicos.ContainsKey(service.Item1))
            {
                throw new DesenvolvimentoIncorretoException($"There are two service classes with the same Nome {service.Item1} -  {service.Item2}. This is not allowed.");
            }

            Servicos.Add(service.Item1, service.Item2);
        }

        private static void AddValidation(Tuple<Type, Type> validation)
        {
            if (Validacoes.ContainsKey(validation.Item1))
            {
                throw new DesenvolvimentoIncorretoException($"There are two validation classes with the same Nome {validation.Item1} - {validation.Item2}. This is not allowed.");
            }

            Validacoes.Add(validation.Item1, validation.Item2);
        }

        private static void AddController(Tuple<Type, Type> controller)
        {
            if (Controladores.ContainsKey(controller.Item1))
            {
                throw new DesenvolvimentoIncorretoException($"There are two controller classes with the same Nome {controller.Item1} - {controller.Item2}. This is not allowed.");
            }

            Controladores.Add(controller.Item1, controller.Item2);
        }

        private static void AddRepository(Tuple<Type, Type> repository)
        {
            if (Repositorios.ContainsKey(repository.Item1))
            {
                throw new DesenvolvimentoIncorretoException($"There are two entity repository with the same Nome {repository.Item1} - {repository.Item2}. This is not allowed.");
            }

            Repositorios.Add(repository.Item1, repository.Item2);
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