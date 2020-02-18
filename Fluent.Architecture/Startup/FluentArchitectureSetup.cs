using Fluente.Arquitetura.Nucleo.Factory;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;
using System.Runtime.CompilerServices;
using dn32.infra.extensoes;

[assembly: InternalsVisibleTo(@"Fluente.Arquitetura.EntityFramework, PublicKey=002400000480000094000000060200000024000052534131000400000100010001e5fbcd7e6f1d70524fc7b787a6ba4d8f332e822c5506e1831f4e59ab41e930c56bbf8cc29fa91f1270f4e873c036335c5aa4ccfc76ab13bfa7372de9d4e17de6c2d188fae9e6842d7d90d51e123836fd9f5d6be5580a32d1a12e59489519c6b93cdcf7ecd782042db1f31190350fbf937bbd6a5ae61d648773b46b9a706ccf")]
namespace Fluente.Arquitetura
{
    public static class FluenteArquiteturaSetup
    {
        public static Config AddFluenteArquitetura(this IMvcBuilder builder, JsonSerializerSettings jsonSerializerSettings)
        {
            if (jsonSerializerSettings is null)
            {
                throw new System.ArgumentNullException(nameof(jsonSerializerSettings));
            }

            ExtensoesJson.ConfiguracoesDeSerializacao = jsonSerializerSettings;
            Setup.ClientServices = builder.Services;
            Setup.InternalInitialize();
            builder.ConfigureApplicationPartManager(apm => apm.FeatureProviders.Add(new ControllerFactory()));
            return Setup.Init();
        }
    }
}