// -----------------------------------------------------------------------
// <copyright company="DnControlador System">
//     Copyright © DnControlador System. All rights reserved.
//     TODOS OS DIREITOS RESERVADOS.
// </copyright>
// -----------------------------------------------------------------------

using dn32.infra.nucleo.erros_de_validacao;
using dn32.infra.servicos;
using System.Collections.Generic;
using System.Linq;

namespace dn32.infra.Validation
{
    public abstract class TransactionalValidation : BaseValidation
    {
        /// <summary>
        /// A validação do serviço.
        /// </summary>
        protected internal new DnServicoTransacionalBase Service
        {
            get => base.Service as DnServicoTransacionalBase;
            set => base.Service = value;
        }

        /// <summary>
        /// Inicializa a classe preenchendo suas dependências.
        /// </summary>
        /// <param Nome="service">
        /// O serviço que a validação representa.
        /// </param>
        /// <param Nome="repository">
        /// O repositório que a validação representa.
        /// </param>
        protected internal virtual void Init(DnServicoTransacionalBase service)
        {
            Service = service;
        }

        /// <summary>
        /// Adiciona uma nova inconsistência ao contexto da requisição.
        /// </summary>
        /// <param Nome="ex">
        /// A inconsitência.
        /// </param>
        public void AddInconsistency(DnErroDeValidacao ex)
        {
            this.Service.SessaoDaRequisicao.ContextDnValidationException.AddInconsistency(ex);
        }

        public void ClearInconsistencies()
        {
            this.Service.SessaoDaRequisicao.ContextDnValidationException.Inconsistencies.Clear();
        }

        public void RunTheContextValidation()
        {
            if (PauseRunTheContextValidation) return;
            this.Service.SessaoDaRequisicao.ContextDnValidationException.Validate();
        }

        public bool PauseRunTheContextValidation { get; set; }

        public void RunTheContextValidation(List<DnServicoTransacionalBase> anotherServices)
        {
            if (PauseRunTheContextValidation) return;

            anotherServices.SelectMany(x => x.SessaoDaRequisicao.ContextDnValidationException.Inconsistencies).ToList().ForEach(ex =>
            {
                Service.SessaoDaRequisicao.ContextDnValidationException.AddInconsistency(ex);
            });

            this.Service.SessaoDaRequisicao.ContextDnValidationException.Validate();
        }

        public void ValueMustBeInformed(object value, string message = "")
        {
            if (value != null)
            {
                return;
            }

            message = string.IsNullOrWhiteSpace(message) ? "Valor can not be null" : message;
            AddInconsistency(new DnValorNuloErroDeValidacao(message));
            RunTheContextValidation();
        }
    }
}