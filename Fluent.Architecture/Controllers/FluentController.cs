// -----------------------------------------------------------------------
// <copyright company="Fluent System">
//     Copyright © Fluent System. All rights reserved.
//     TODOS OS DIREITOS RESERVADOS.
// </copyright>
// -----------------------------------------------------------------------

// ReSharper disable CommentTypo

using Fluent.Architecture.Entities;
using Fluent.Architecture.Factory;
using Fluent.Architecture.Services;
using Fluent.Architecture.Specifications;

namespace Fluent.Architecture.Controllers
{
    /// <inheritdoc />
    /// <summary>
    /// Controlador genérico padrão recomendado para herança por todos os controladores que tiverem entidade.
    /// </summary>
    /// <typeparam name="T">O tipo da entidade do controller.</typeparam>
    public abstract partial class FluentController<T> : FluentServiceController<FluentService<T>> where T : BaseEntity
    {
        protected T2 CreateSpec<T2>() where T2 : BaseSpecification
        {
            return SpecFactory.Create<T2>(Service);
        }
    }
}

