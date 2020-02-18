using dn32.infra.Factory;
using dn32.infra.Specifications;
using Microsoft.AspNetCore.Mvc;
using System;
using dn32.infra.dados;

namespace dn32.infra.nucleo.controladores
{
    public abstract class DnControlador<T> : DnControladorDeServico<Services.DnService<T>> where T : EntidadeBase
    {
        protected T2 CriarEspecificacao<T2>() where T2 : BaseSpecification
        {
            return SpecFactory.Create<T2>(this.Servico);
        }

        [NonAction]
        protected override void Dispose(bool finalizando)
        {
            base.Dispose(finalizando);
        }

        [NonAction]
        public new Type GetType()
        {
            return base.GetType();
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

