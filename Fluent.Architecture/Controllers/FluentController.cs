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
using Microsoft.AspNetCore.Mvc;
using System;

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

        [NonAction]
        protected override void Dispose(bool disposing)
        {
            base.Dispose(disposing);
        }

        [NonAction]
        public new Type GetType()
        {
            return GetType();
        }

        [NonAction]
        public override string ToString()
        {
            return base.ToString();
        }

        [NonAction]
        public override bool Equals(object obj)
        {
            return base.Equals(obj);
        }

        [NonAction]
        public override int GetHashCode()
        {
            return base.GetHashCode();
        }
    }
}

