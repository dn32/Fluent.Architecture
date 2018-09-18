using System;
using Fluent.Architecture.Repository;
using Fluent.Architecture.Services;

namespace Fluent.Architecture.Validation
{
    public abstract class BaseValidation
    {
        ///// <summary>
        ///// O repositório do serviço.
        ///// </summary>
        protected virtual BaseRepository Repository { get; set; }

        /// <summary>
        /// A validação do serviço.
        /// </summary>
        protected virtual BaseService Service { get; set; }
    }
}