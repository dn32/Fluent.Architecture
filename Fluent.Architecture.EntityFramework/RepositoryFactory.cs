using Fluente.Arquitetura.Base.Models;
using Fluente.Arquitetura.Base.Atributos;
using Fluente.Arquitetura.Base.Enumeradores;
using Fluente.Arquitetura.Base.Extensoes;
using Fluente.Arquitetura.Interfaces;
using Fluente.Arquitetura.Nucleo.Factory;
using Fluente.Arquitetura.Nucleo.Interfaces;
using Fluente.Arquitetura.Exceptions;
using Fluente.Arquitetura.Services;
using Fluente.Arquitetura.Nucleo.Models;
using System;
using System.Linq;
using System.Reflection;

namespace Fluente.Arquitetura.EntityFramework
{
    /// <summary>
    /// Classe interna. Nunca a deixe pública, pois o acesso a um repositório à partir de um serviço terceiro não deve ser permitido.
    /// A fábrica de repositórios.
    /// </summary>
    /// <typeparam name="T">
    ///  O tipo da entidade do repositório a ser criado.
    /// </typeparam>
    internal class RepositoryFactory : IRepositoryFactory
    {
        /// <summary>
        /// Cria um novo repositório.
        /// </summary>
        /// <param name="transactionObjects">
        /// Os objetos de controle de transação do repositório.
        /// </param>
        /// <param name="service">
        /// O serviço qual o repositório representa.
        /// </param>
        /// <returns>
        /// O repositório criado.
        /// </returns>
        public IFluenteRepository<T> Create<T>(ITransactionObjects transactionObjects, FluenteService<T> service) where T : EntidadeBase
        {
            if (Setup.Config?.Config?.Connections == null) { throw new IncorrectDevelopmentException($"Arquitetura was not initialized properly"); }

            var dbType = GetTheEntityDBType(typeof(T));
            if (dbType == null)
            {
                if (Setup.Config.Config.Connections.Count == 1)
                {
                    dbType = Setup.Config.Config.Connections.Single().DbContextType.GetCustomAttribute<DbTypeAttribute>() ?? throw new IncorrectDevelopmentException($"The entity {typeof(T).Name} needs a database type specification. Example: [DbType (FluenteDbType.ORACLE)]"); ;
                }
            }

            if (dbType == null)
            {
                throw new IncorrectDevelopmentException($"The entity {typeof(T).Name} needs a database type specification. Example: [DbType (FluenteDbType.ORACLE)]"); ;
            }

            var localType = Setup.Config?.Config?.GenericRepositoryType?.MakeGenericType(typeof(T)) ?? typeof(FluenteEFRepository<T>);

            if (Setup.Repositories.TryGetValue(typeof(T), out var repositoryType))
            {
                localType = repositoryType;
            }

            var repository = Create<T>(localType);

            if (transactionObjects == null)
            {
                Connection connetion;

                if (string.IsNullOrWhiteSpace(dbType.Identifier))
                {
                    var conn = Setup.Config?.Config?.Connections.Where(x => x.DbContextType.GetCustomAttribute<DbTypeAttribute>()?.DbType == dbType.DbType);
                    if (conn.Count() > 1)
                    {
                        throw new IncorrectDevelopmentException($"More than one connection of the same type was found with the same type \"{dbType.DbType}\". Add identifiers for them.");
                    }

                    if (conn.Count() == 0)
                    {
                        throw new IncorrectDevelopmentException($"Could not find connection of requested \"{dbType.DbType}\" type in entity \"{typeof(T).Name}\"");
                    }
                    connetion = conn.Single();
                }
                else
                {
                    if (Setup.Config?.Config?.Connections == null) { throw new IncorrectDevelopmentException($"Arquitetura was not initialized properly"); }
                    var conn = Setup.Config.Config.Connections.Where(x =>
                                    x.DbContextType.GetCustomAttribute<DbTypeAttribute>()?.DbType == dbType.DbType &&
                                    x.Identifier.Equals(dbType.Identifier, StringComparison.InvariantCultureIgnoreCase));
                    if (conn.Count() > 1)
                    {
                        throw new IncorrectDevelopmentException($"More than one connection of the same type was found with the same identifier \"{dbType.Identifier}\"");
                    }

                    if (conn.Count() == 0)
                    {
                        throw new IncorrectDevelopmentException($"Could not find connection of requested \"{dbType.DbType}\" type and identifier \"{dbType.Identifier}\" in entity \"{typeof(T).Name}\"");
                    }
                    connetion = conn.Single();
                }

                var transactionObjectsType = repository.TransactionObjectsType;
                transactionObjects = TransactionObjectsFactory.Create(transactionObjectsType, connetion, service.SessionRequest);
                service.SessionRequest.TransactionObjects = transactionObjects;
            }

            repository.TransactionObjects = transactionObjects;
            repository.Service = service;

            return repository;
        }

        internal IFluenteRepository<T> Create<T>(Type repositoryType) where T : EntidadeBase
        {
            return Activator.CreateInstance(repositoryType) as IFluenteRepository<T>;
        }

        //Todo - validar no boot se todas as entidades tem tipo de BD,ou se só tem um tipo de bd instanciado na aplicação
        private DbTypeAttribute GetTheEntityDBType(Type type)
        {
            return type.GetCustomAttribute<DbTypeAttribute>();
        }
    }
}