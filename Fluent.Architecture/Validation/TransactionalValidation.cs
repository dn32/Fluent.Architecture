// -----------------------------------------------------------------------
// <copyright company="Fluent System">
//     Copyright © Fluent System. All rights reserved.
//     TODOS OS DIREITOS RESERVADOS.
// </copyright>
// -----------------------------------------------------------------------

using Fluent.Architecture.Exceptions.ValidationException;
using Fluent.Architecture.Services;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace Fluent.Architecture.Validation
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
        public void AddInconsistency(FluentValidationException ex)
        {
            this.Service.SessionRequest.ContextFluentValidationException.AddInconsistency(ex);
        }

        public void RunTheContextValidation()
        {
            this.Service.SessionRequest.ContextFluentValidationException.Validate();
        }

        public void RunTheContextValidation(List<TransactionalService> anotherServices)
        {
            anotherServices.SelectMany(x => x.SessionRequest.ContextFluentValidationException.Inconsistencies).ToList().ForEach(ex =>
            {
                Service.SessionRequest.ContextFluentValidationException.AddInconsistency(ex);
            });

            this.Service.SessionRequest.ContextFluentValidationException.Validate();
        }

        public void ValueMustBeInformed(object value, string message = "")
        {
            if (value != null)
            {
                return;
            }

            message = string.IsNullOrWhiteSpace(message) ? "Value can not be null" : message;
            AddInconsistency(new NullValueFluentValidationException(message));
            RunTheContextValidation();
        }
    }
}