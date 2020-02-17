// -----------------------------------------------------------------------
// <copyright company="Fluente System">
//     Copyright © Fluente System. All rights reserved.
//     TODOS OS DIREITOS RESERVADOS.
// </copyright>
// -----------------------------------------------------------------------

// ReSharper disable CommentTypo

using Fluente.Arquitetura.Nucleo.Models;
using Fluente.Arquitetura.Factory;
using Fluente.Arquitetura.Services;
using Fluente.Arquitetura.Specifications;
using Microsoft.AspNetCore.Mvc;
using System;

namespace Fluente.Arquitetura.Controllers
{
    /// <inheritdoc />
    /// <summary>
    /// Controlador genérico padrão recomendado para herança por todos os controladores que tiverem entidade.
    /// </summary>
    /// <typeparam name="T">O tipo da entidade do controller.</typeparam>
    public abstract partial class FluenteController<T> : FluenteServiceController<FluenteService<T>> where T : BaseEntity
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

