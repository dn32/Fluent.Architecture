using Fluent.Architecture.Core.Attributes;
using Fluent.Architecture.Enumerator;
using Fluent.Architecture.Model;
using Fluent.Architecture.Repository;
using System;

namespace Fluent.Architecture.Sample
{
    [DbType(FluentDbType.ORACLE)]
    public class EntidadeTeste : FluentEntity
    {
        public int Id { get; set; }

        public string Name { get; set; }
    }

    public class RepositorioPadraoSQL<T> : FluentSQLRepository<T> where T : BaseEntity
    {
        public RepositorioPadraoSQL() : base("SQL")
        {
        }
    }

    public class EntidadeTesteRepositorio : RepositorioPadraoSQL<EntidadeTeste>
    {
        public override EntidadeTeste Add(EntidadeTeste entity)
        {
            return base.Add(entity);
        }
    }

    public class ArchitectureInit
    {
        public static void Setup(IServiceProvider serviceProvider)
        {
            Architecture.Setup
                .Init()
                .AddConnectionString("conexao.com.br", "SQL")
                .AddConnectionString(GetConnectionStringOracle, "ORACLE")
                .SetServiceProvider(serviceProvider)
                .Build()
                .Run();
        }

        public static string GetConnectionStringOracle(object sessionId)
        {
            return "conn";
        }
    }
}
