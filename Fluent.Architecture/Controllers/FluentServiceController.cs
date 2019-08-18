// -----------------------------------------------------------------------
// <copyright company="Fluent System">
//     Copyright © Fluent System. All rights reserved.
//     TODOS OS DIREITOS RESERVADOS.
// </copyright>
// -----------------------------------------------------------------------

// ReSharper disable CommentTypo

using Fluent.Architecture.Core.Util;
using Fluent.Architecture.Entities;
using Fluent.Architecture.Factory;
using Fluent.Architecture.Services;
using Fluent.Architecture.Util;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Primitives;
using Newtonsoft.Json;
using System;
using System.Security.Claims;

namespace Fluent.Architecture.Controllers
{
    /// <inheritdoc />
    /// <summary>
    /// Controlador base para controladores que não possuem entidade. Nesse caso, deve-se informar o serviço a ser usado pelo controlador.
    /// O serviço é inicializado a cada ActionExecuting.
    /// </summary>
    /// <typeparam name="TS">O serviço a ser usado pelo controlador.</typeparam>
    public abstract class FluentServiceController<TS> : BaseController where TS : TransactionalService, new()
    {
        public virtual FluentPagination LastRequestPagination => Service.SessionRequest.Pagination;

        internal protected TS Service { get; set; }

        protected internal Guid SessionRequestId => Service.SessionRequestId;

        protected internal ClaimsPrincipal ServiceUser => Service.User;

        protected internal HttpContext ServiceHttpContext => Service.LocalHttpContext;

        // private IDbContextTransaction Transaction { get; set; }

        internal protected bool TransactionIsStarted { get; set; }

        [NonAction]
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
            CloseTransaction();
            return new DefaultResult(PropertySelector(data));
        }

        [NonAction]
        protected DefaultPaginationResult Result(object data, FluentPagination pagination)
        {
            CloseTransaction();
            return new DefaultPaginationResult(PropertySelector(data), pagination);
        }

        [NonAction]
        protected DefaultPaginationTermResult Result(object data, FluentPagination pagination, string term)
        {
            CloseTransaction();
            return new DefaultPaginationTermResult(PropertySelector(data), pagination, term);
        }

        [NonAction]
        public override void OnActionExecuting(ActionExecutingContext context)
        {
            OpenTransaction();
            base.OnActionExecuting(context);
        }

        [NonAction]
        public override void OnActionExecuted(ActionExecutedContext filterContext)
        {
            CloseTransaction();
            base.OnActionExecuted(filterContext);
        }

        [NonAction]
        internal protected void OpenTransaction()
        {
            Service = ServiceFactory.Create<TS>(HttpContext);
            //Transaction = Service.TransactionObjects.Session.Database.BeginTransaction();
            TransactionIsStarted = true;
        }

        [NonAction]
        internal protected void CloseTransaction()
        {
            if (TransactionIsStarted)
            {
                if (Service.SessionRequest.ContextFluentValidationException.IsValid)
                {
                    Service.TransactionObjects.Session.SaveChanges();
                    //Transaction.Commit();
                    TransactionIsStarted = false;
                }
                else
                {
                    //Transaction.Rollback();
                    TransactionIsStarted = false;
                }

                Service.Dispose(true);
            }
        }

        [NonAction]
        protected internal new JsonResult Json(object data)
        {
            return new CustomJsonResult(data);
        }
    }
}
