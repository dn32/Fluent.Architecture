// -----------------------------------------------------------------------
// <copyright company="Fluente System">
//     Copyright © Fluente System. All rights reserved.
//     TODOS OS DIREITOS RESERVADOS.
// </copyright>
// -----------------------------------------------------------------------

using Fluente.Arquitetura.Exceptions.ValidationException;
using Fluente.Arquitetura.Services;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace Fluente.Arquitetura.Validation
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
        /// <param name="service">
        /// O serviço que a validação representa.
        /// </param>
        /// <param name="repository">
        /// O repositório que a validação representa.
        /// </param>
        protected internal virtual void Init(TransactionalService service)
        {
            Service = service;
        }

        /// <summary>
        /// Adiciona uma nova inconsistência ao contexto da requisição.
        /// </summary>
        /// <param name="ex">
        /// A inconsitência.
        /// </param>
        public void AddInconsistency(FluenteValidationException ex)
        {
            this.Service.SessionRequest.ContextFluenteValidationException.AddInconsistency(ex);
        }

        public void ClearInconsistencies()
        {
            this.Service.SessionRequest.ContextFluenteValidationException.Inconsistencies.Clear();
        }

        public void RunTheContextValidation()
        {
            if (PauseRunTheContextValidation) return;
            this.Service.SessionRequest.ContextFluenteValidationException.Validate();
        }

        public bool PauseRunTheContextValidation { get; set; }

        public void RunTheContextValidation(List<TransactionalService> anotherServices)
        {
            if (PauseRunTheContextValidation) return;

            anotherServices.SelectMany(x => x.SessionRequest.ContextFluenteValidationException.Inconsistencies).ToList().ForEach(ex =>
            {
                Service.SessionRequest.ContextFluenteValidationException.AddInconsistency(ex);
            });

            this.Service.SessionRequest.ContextFluenteValidationException.Validate();
        }

        public void ValueMustBeInformed(object value, string message = "")
        {
            if (value != null)
            {
                return;
            }

            message = string.IsNullOrWhiteSpace(message) ? "Value can not be null" : message;
            AddInconsistency(new NullValueFluenteValidationException(message));
            RunTheContextValidation();
        }
    }
}