// -----------------------------------------------------------------------
// <copyright company="Fluent System">
//     Copyright © Fluent System. All rights reserved.
//     TODOS OS DIREITOS RESERVADOS.
// </copyright>
// -----------------------------------------------------------------------

// ReSharper disable CommentTypo
using Fluent.Architecture.Repository;

namespace Fluent.Architecture.Factory
{
    /// <summary>
    /// Interno.
    /// A fabrica de contexto do entity framework.
    /// </summary>
    internal class ContextFactory
    {
        /// <summary>
        ///  Cria um novo contexto para o entity framework.
        /// </summary>
        /// <param name="connectionString">
        /// A string de conexão com o banco de dados.
        /// </param>
        /// <returns>
        /// O contexto criado.
        /// </returns>
        internal static EfContext Create(string connectionString)
        {
            return new EfContext(connectionString);

            // Deixar essa parte de interceptador pra quando for necessários.
            // var interceptor = new ContextInterceptor(Guid.Empty);
            // return new ProxyGenerator().CreateClassProxy(typeof(EfContext), new object[] { connectionString }, interceptor) as EfContext;
        }
    }
}
