// -----------------------------------------------------------------------
// <copyright company="Fluent System">
//     Copyright © Fluent System. All rights reserved.
//     TODOS OS DIREITOS RESERVADOS.
// </copyright>
// -----------------------------------------------------------------------

// ReSharper disable CommentTypo
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Fluent.Architecture.Core.Attributes;
using Fluent.Architecture.Core.Factory;
using Fluent.Architecture.Core.Interfaces;
using Fluent.Architecture.Enumerator;
using Fluent.Architecture.Model;
using Fluent.Architecture.Repository;
using Fluent.Architecture.Services;

namespace Fluent.Architecture.Factory
{
    /// <summary>
    /// Classe interna. Nunca a deixe pública, pois o acesso a um repositório à partir de um serviço terceiro não deve ser permitido.
    /// A fábrica de repositórios.
    /// </summary>
    /// <typeparam name="T">
    ///  O tipo da entidade do repositório a ser criado.
    /// </typeparam>
    internal class RepositoryFactory<T> where T : BaseEntity
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
        internal static IFluentRepository<T> Create(ITransactionObjects transactionObjects, FluentService<T> service)
        {
            var dbType = GetTheEntityDBType(typeof(T));
            var localType = GetRepositoryType(dbType.DbType);
            localType = localType.MakeGenericType(typeof(T));

            if (Setup.Repositories.TryGetValue(typeof(T), out var repositoryType))
            {
                localType = repositoryType;
            }

            var repository = Create(localType);

            if (transactionObjects == null)
            {
                Connection connetion;

                if (string.IsNullOrWhiteSpace(dbType.Identifier))
                {
                    //Todo checar erros aqui.
                    connetion = Setup.Config.Config.Connections.Single(x => x.DBType == dbType.DbType);
                }
                else
                {
                    //Todo checar erros aqui.
                    connetion = Setup.Config.Config.Connections.Single(x => x.Identifier == dbType.Identifier);
                }

                var transactionObjectsType = repository.TransactionObjectsType;
                connetion = Setup.Config.Config.Connections.First(x => x.DBType == dbType.DbType);
                repository.TransactionObjects = TransactionObjectsFactory.Create(transactionObjectsType, connetion);
            }
            else
            {
                repository.TransactionObjects = transactionObjects;
            }

            return repository;
        }

        private static IFluentRepository<T> Create(Type repositoryType)
        {
            return Activator.CreateInstance(repositoryType) as IFluentRepository<T>;
        }

        //Todo - validar no boot se todas as entidades tem tipo de BD,ou se só tem um tipo de bd instanciado na aplicação
        private static DbTypeAttribute GetTheEntityDBType(Type type)
        {
            return type.GetCustomAttribute<DbTypeAttribute>();
        }

        private static Type GetRepositoryType(FluentDbType dbType)
        {
            if (Setup.RepositoryTypes.TryGetValue(dbType, out Type repo))
            {
                return repo;
            }

            throw new Exception($"Not found repository type for {dbType}");
        }
    }
}