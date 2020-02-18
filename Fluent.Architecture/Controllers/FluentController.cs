using dn32.infra.Factory;
using dn32.infra.Services;
using dn32.infra.Specifications;
using Microsoft.AspNetCore.Mvc;
using System;
using dn32.infra.dados;

namespace dn32.infra.Controllers
{
    /// <inheritdoc />
    /// <summary>
    /// Controlador genérico padrão recomendado para herança por todos os controladores que tiverem entidade.
    /// </summary>
    /// <typeparam Nome="T">O tipo da entidade do controller.</typeparam>
    public abstract partial class DnController<T> : DnServiceController<DnService<T>> where T : EntidadeBase
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

