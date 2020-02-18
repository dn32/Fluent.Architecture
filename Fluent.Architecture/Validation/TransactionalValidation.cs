// -----------------------------------------------------------------------
// <copyright company="Dn System">
//     Copyright © Dn System. All rights reserved.
//     TODOS OS DIREITOS RESERVADOS.
// </copyright>
// -----------------------------------------------------------------------

using dn32.infra.Exceptions.ValidationException;
using dn32.infra.Services;
using System.Collections.Generic;
using System.Linq;

namespace dn32.infra.Validation
{
    public abstract class TransactionalValidation : BaseValidation
    {
        /// <summary>
        /// A validação do serviço.
        /// </summary>
        protected internal new TransactionalService Service
        {
            get => base.Service as TransactionalService;
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
        protected internal virtual void Init(TransactionalService service)
        {
            Service = service;
        }

        /// <summary>
        /// Adiciona uma nova inconsistência ao contexto da requisição.
        /// </summary>
        /// <param Nome="ex">
        /// A inconsitência.
        /// </param>
        public void AddInconsistency(DnValidationException ex)
        {
            this.Service.SessionRequest.ContextDnValidationException.AddInconsistency(ex);
        }

        public void ClearInconsistencies()
        {
            this.Service.SessionRequest.ContextDnValidationException.Inconsistencies.Clear();
        }

        public void RunTheContextValidation()
        {
            if (PauseRunTheContextValidation) return;
            this.Service.SessionRequest.ContextDnValidationException.Validate();
        }

        public bool PauseRunTheContextValidation { get; set; }

        public void RunTheContextValidation(List<TransactionalService> anotherServices)
        {
            if (PauseRunTheContextValidation) return;

            anotherServices.SelectMany(x => x.SessionRequest.ContextDnValidationException.Inconsistencies).ToList().ForEach(ex =>
            {
                Service.SessionRequest.ContextDnValidationException.AddInconsistency(ex);
            });

            this.Service.SessionRequest.ContextDnValidationException.Validate();
        }

        public void ValueMustBeInformed(object value, string message = "")
        {
            if (value != null)
            {
                return;
            }

            message = string.IsNullOrWhiteSpace(message) ? "Valor can not be null" : message;
            AddInconsistency(new NullValueDnValidationException(message));
            RunTheContextValidation();
        }
    }
}