// -----------------------------------------------------------------------
// <copyright company="Fluent System">
//     Copyright © Fluent System. All rights reserved.
//     TODOS OS DIREITOS RESERVADOS.
// </copyright>
// -----------------------------------------------------------------------

// ReSharper disable CommentTypo
using Fluent.Architecture.Core.Factory;
using Fluent.Architecture.Core.Interfaces;
using Fluent.Architecture.Entities;
using Fluent.Architecture.Services;
using System;
using System.Linq;
using System.Reflection;

namespace Fluent.Architecture.EntityFramework
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
        public IFluentRepository<T> Create<T>(ITransactionObjects transactionObjects, FluentService<T> service) where T : BaseEntity
        {
            var dbType = GetTheEntityDBType(typeof(T));
            var localType = typeof(FluentEFRepository<T>);
            //localType = localType.MakeGenericType(typeof(T));

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
                    connetion = Setup.Config.Config.Connections.Single(x => x.DbContextType.GetCustomAttribute<DbTypeAttribute>().DbType == dbType.DbType);

                }
                else
                {
                    connetion = Setup.Config.Config.Connections.Single(x => x.Identifier.Equals(dbType.Identifier, StringComparison.InvariantCultureIgnoreCase));
                }

                var transactionObjectsType = repository.TransactionObjectsType;
                transactionObjects = TransactionObjectsFactory.Create(transactionObjectsType, connetion);
                service.SessionRequest.TransactionObjects = transactionObjects;
            }

            repository.TransactionObjects = transactionObjects;
            repository.Service = service;

            return repository;
        }

        internal IFluentRepository<T> Create<T>(Type repositoryType) where T : BaseEntity
        {
            return Activator.CreateInstance(repositoryType) as IFluentRepository<T>;
        }

        //Todo - validar no boot se todas as entidades tem tipo de BD,ou se só tem um tipo de bd instanciado na aplicação
        private DbTypeAttribute GetTheEntityDBType(Type type)
        {
            return type.GetCustomAttribute<DbTypeAttribute>();
        }
    }
}