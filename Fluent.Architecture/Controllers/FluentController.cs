// -----------------------------------------------------------------------
// <copyright company="Fluent System">
//     Copyright © Fluent System. All rights reserved.
//     TODOS OS DIREITOS RESERVADOS.
// </copyright>
// -----------------------------------------------------------------------

// ReSharper disable CommentTypo

using Fluent.Architecture.Entities;
using Fluent.Architecture.Services;
using Fluent.Architecture.Factory;
using Fluent.Architecture.Specifications;
using Newtonsoft.Json;
using Microsoft.Extensions.Primitives;
using Fluent.Architecture.Core.Util;
using Microsoft.AspNetCore.Mvc;

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

        protected object PropertySelector(object element)
        {
            Request.Headers.TryGetValue("propertyToIgnore", out StringValues propertyToIgnoreValues);
            Request.Headers.TryGetValue("propertyToShow", out StringValues propertyToShowValues);
            return JsonConvert.DeserializeObject(JsonConvert.SerializeObject(element,
                         Formatting.Indented, new JsonSerializerSettings
                         {
                             ContractResolver = new PropertySelectorDynamicContractJsonResolver(propertyToIgnoreValues, propertyToShowValues)
                         }));
        }

        [NonAction]
        protected DefaultResult Result(object data)
        {
            return new DefaultResult(PropertySelector(data));
        }

        [NonAction]
        protected DefaultPaginationResult Result(object data, FluentPagination pagination)
        {
            return new DefaultPaginationResult(PropertySelector(data), pagination);
        }

        [NonAction]
        protected DefaultPaginationTermResult Result(object data, FluentPagination pagination, string term)
        {
            return new DefaultPaginationTermResult(PropertySelector(data), pagination, term);
        }
    }
}

