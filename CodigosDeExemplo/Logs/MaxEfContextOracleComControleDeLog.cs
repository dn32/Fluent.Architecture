using Fluent.Architecture.Controllers;
using Fluent.Architecture.Core.Extensions;
using Fluent.Architecture.Core.Models;
using Fluent.Architecture.EntityFramework.Oracle;
using Fluent.Architecture.Extensions;
using Max.Infraestrutura.config.Logs;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Max.Infraestrutura.Logs
{
    public class MaxEfContextOracleComControleDeLog : EfContextOracle
    {
        const bool SALVAR_PROPRIEDADES = false;

        public MaxEfContextOracleComControleDeLog(string conexao) : base(conexao)
        {
        }

        public virtual void ArmazenarLogs(ICollection<FluentEventEntity> fluentEventEntityList)
        {
            var origem = UserSessionRequest?.LocalHttpContext.Request.Headers["EhFormularioDinamico"].Count > 0 ? EnumOrigemLogDeOperacao.FormularioDinamico : EnumOrigemLogDeOperacao.API;
            var metodoDeEntrada = "Indefinido";
            var stackTrace = new System.Diagnostics.StackTrace();
            for (int i = 0; i < stackTrace.FrameCount; i++)
            {
                var method = stackTrace.GetFrame(i).GetMethod();
                if (method.DeclaringType != null && !method.DeclaringType.IsAbstract && method.DeclaringType.IsSubclassOf(typeof(BaseController)))
                {
                    metodoDeEntrada = $"{method.DeclaringType.GetFriendlyName()}.{method.Name}";
                    break;
                }
            }

            var logDeOperacao = new LogDeOperacao
            {
                Usuario = UserSessionRequest?.LocalHttpContext?.User?.Identity?.Name ?? "Nenhum",
                Data = DateTime.Now,
                Origem = origem,
                Metodo = metodoDeEntrada,
                Entidades = new List<LogDeOperacaoEntidade>()
            };

            foreach (var fluentEventEntity in fluentEventEntityList)
            {
                if (fluentEventEntity.EntityState == EntityState.Detached) { return; }
                if (fluentEventEntity.EntityState == EntityState.Unchanged) { return; }
                var listaDePropriedadesAIgnorar = new[] { "ATUALIZID", "CODOPERACAO", "DTATUALIZ" };
                var properties = fluentEventEntity
                                   .Properties
                                   .Where(x => fluentEventEntity.EntityState == EntityState.Added || fluentEventEntity.EntityState == EntityState.Deleted || x.Changed)
                                   .Where(x => !listaDePropriedadesAIgnorar.Contains(x.PropertyName))
                                   .Select(x => new LogDeOperacaoPropriedade
                                   {
                                       Nome = x.PropertyName,
                                       ValorOriginal = fluentEventEntity.EntityState == EntityState.Added ? null : x.OriginalValue.ToFluentJson(),
                                       NovoValor = fluentEventEntity.EntityState == EntityState.Deleted ? null : x.CurrentValue.ToFluentJson()
                                   })
                                   .ToList();


                if (fluentEventEntity.ChangedEntity.CurrentValues["CODOPERACAO"].FluentCast<int>(false) == 2)
                {
                    fluentEventEntity.EntityState = EntityState.Deleted;
                }

                if (fluentEventEntity.EntityState == EntityState.Modified && properties.Count == 0) { break; }

                var keys = fluentEventEntity.CurrentEntity.GetKeyValues().Select(x => new Tuple<string, object>(x.ColumnName, x.Value)).ToDictionary(x => x.Item1, l => l.Item2);
                
                if (!SALVAR_PROPRIEDADES) { properties.Clear(); }

                var logDeOperacaoEntidade = new LogDeOperacaoEntidade
                {
                    Entidade = fluentEventEntity.CurrentEntityType.FullName,
                    Propriedades = properties,
                    Chaves = keys.ToFluentJsonOrPrimitive(),
                    TipoDeOperacao = (EnumLogDeOperacaoTipoDeOperacao)fluentEventEntity.EntityState.GetHashCode()
                };

                logDeOperacao.Entidades.Add(logDeOperacaoEntidade);
            }

            Add(logDeOperacao);
            SaveChanges();
        }
    }
}
