using System;
using Fluent.Architecture.Model;
using Fluent.Architecture.Repository;
using Fluent.Architecture.Service;

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
        internal static FluentRepository<T> Create(TransactionObjects transactionObjects, FluentService<T> service)
        {
            var localType = typeof(FluentRepository<T>);
            if (Setup.Repositories.TryGetValue(typeof(T).Name, out var epositoryType))
            {
                localType = epositoryType;
            }

            var repository = Create(localType);
            repository.TransactionObjects = transactionObjects;
            repository.Service = service;
            return repository;
        }

        private static FluentRepository<T> Create(Type repositoryType)
        {
            //if (!Setup.InTest)
            //{
                return Activator.CreateInstance(repositoryType) as FluentRepository<T>;
            //}

            //var interceptor = new TransactionInterceptorMock(false, Guid.Empty);
            //return RepositoryClassBuilder.Create<T>(repositoryType, interceptor) as FluentRepository<T>;
        }
    }
}