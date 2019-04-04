// -----------------------------------------------------------------------
// <copyright company="Fluent System">
//     Copyright © Fluent System. All rights reserved.
//     TODOS OS DIREITOS RESERVADOS.
// </copyright>
// -----------------------------------------------------------------------


// ReSharper disable CommentTypo

using Fluent.Architecture.Model;
using Fluent.Architecture.Util;
using Fluent.Architecture.Services;
using System;
using System.Collections.Generic;
using Fluent.Architecture.Exceptions;
using Fluent.Architecture.Factory;
using Fluent.Architecture.Specifications;

namespace Fluent.Architecture.Controllers
{
    /// <inheritdoc />
    /// <summary>
    /// Controlador genérico padrão recomendado para herança por todos os controladores que tiverem entidade.
    /// </summary>
    /// <typeparam name="T">O tipo da entidade do controller.</typeparam>
    public abstract class FluentController<T> : FluentServiceController<FluentService<T>> where T : BaseEntity
    {
        protected T CreateSpec<T>() where T : BaseSpecification
        {
            return SpecFactory.Create<T>(Service);
        }
    }
}

