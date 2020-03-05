using dn32.infra.dados;
using dn32.infra.Interfaces;
using dn32.infra.Nucleo.Factory;
using dn32.infra.Nucleo.Interfaces;
using dn32.infra.Nucleo.Models;
using dn32.infra.servicos;
using System;
using System.Linq;
using System.Reflection;
using dn32.infra.nucleo.excecoes;

namespace dn32.infra.EntityFramework
{
    /// <summary>
    /// Classe interna. Nunca a deixe pública, pois o acesso a um repositório à partir de um serviço terceiro não deve ser permitido.
    /// A fábrica de repositórios.
    /// </summary>
    /// <typeparam Nome="T">
    ///  O tipo da entidade do repositório a ser criado.
    /// </typeparam>
    internal class RepositoryFactory : IRepositoryFactory
    {
        /// <summary>
        /// Cria um novo repositório.
        /// </summary>
        /// <param Nome="transactionObjects">
        /// Os objetos de controle de transação do repositório.
        /// </param>
        /// <param Nome="service">
        /// O serviço qual o repositório representa.
        /// </param>
        /// <returns>
        /// O repositório criado.
        /// </returns>
        public IDnRepository<T> Create<T>(ITransactionObjects transactionObjects, DnServico<T> service) where T : EntidadeBase
        {
            if (Setup.ConfiguracoesGlobais.Conexoes == null) { throw new DesenvolvimentoIncorretoException($"Arquitetura was not initialized properly"); }

            var dbType = GetTheEntityDBType(typeof(T));
            if (dbType == null)
            {
                if (Setup.ConfiguracoesGlobais.Conexoes.Count == 1)
                {
                    dbType = Setup.ConfiguracoesGlobais.Conexoes.Single().TipoDoContexto.GetCustomAttribute<DbTypeAttribute>() ?? throw new DesenvolvimentoIncorretoException($"The entity {typeof(T).Name} needs a database type specification. Example: [DbType (DnDbType.ORACLE)]"); ;
                }
            }

            if (dbType == null)
            {
                throw new DesenvolvimentoIncorretoException($"The entity {typeof(T).Name} needs a database type specification. Example: [DbType (DnDbType.ORACLE)]"); ;
            }

            var localType = Setup.ConfiguracoesGlobais.GenericRepositoryType?.MakeGenericType(typeof(T)) ?? typeof(DnEFRepository<T>);

            if (Setup.Repositorios.TryGetValue(typeof(T), out var repositoryType))
            {
                localType = repositoryType;
            }

            var repository = Create<T>(localType);

            if (transactionObjects == null)
            {
                Connection connetion;

                if (string.IsNullOrWhiteSpace(dbType.Identifier))
                {
                    var conn = Setup.ConfiguracoesGlobais.Conexoes.Where(x => x.TipoDoContexto.GetCustomAttribute<DbTypeAttribute>()?.DbType == dbType.DbType);
                    if (conn.Count() > 1)
                    {
                        throw new DesenvolvimentoIncorretoException($"More than one connection of the same type was found with the same type \"{dbType.DbType}\". Adicionar identifiers for them.");
                    }

                    if (conn.Count() == 0)
                    {
                        throw new DesenvolvimentoIncorretoException($"Could not find connection of requested \"{dbType.DbType}\" type in entity \"{typeof(T).Name}\"");
                    }
                    connetion = conn.Single();
                }
                else
                {
                    if (Setup.ConfiguracoesGlobais.Conexoes == null) { throw new DesenvolvimentoIncorretoException($"Arquitetura was not initialized properly"); }
                    var conn = Setup.ConfiguracoesGlobais.Conexoes.Where(x =>
                                    x.TipoDoContexto.GetCustomAttribute<DbTypeAttribute>()?.DbType == dbType.DbType &&
                                    x.IdentificadorDaConexao.Equals(dbType.Identifier, StringComparison.InvariantCultureIgnoreCase));
                    if (conn.Count() > 1)
                    {
                        throw new DesenvolvimentoIncorretoException($"More than one connection of the same type was found with the same identifier \"{dbType.Identifier}\"");
                    }

                    if (conn.Count() == 0)
                    {
                        throw new DesenvolvimentoIncorretoException($"Could not find connection of requested \"{dbType.DbType}\" type and identifier \"{dbType.Identifier}\" in entity \"{typeof(T).Name}\"");
                    }
                    connetion = conn.Single();
                }

                var transactionObjectsType = repository.TransactionObjectsType;
                transactionObjects = TransactionObjectsFactory.Create(transactionObjectsType, connetion, service.SessaoDaRequisicao);
                service.SessaoDaRequisicao.ObjetosDaTransacao = transactionObjects;
            }

            repository.TransactionObjects = transactionObjects;
            repository.Service = service;

            return repository;
        }

        internal IDnRepository<T> Create<T>(Type repositoryType) where T : EntidadeBase
        {
            return Activator.CreateInstance(repositoryType) as IDnRepository<T>;
        }

        //Todo - validar no boot se todas as entidades tem tipo de BD,ou se só tem um tipo de bd instanciado na aplicação
        private DbTypeAttribute GetTheEntityDBType(Type type)
        {
            return type.GetCustomAttribute<DbTypeAttribute>();
        }
    }
}