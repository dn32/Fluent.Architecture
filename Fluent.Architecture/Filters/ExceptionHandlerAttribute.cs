#if NET461
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.Mvc;
using System.Web.UI.WebControls;
using Fluent.Architecture.Exception;
using Fluent.Architecture.Validation;

#else
using Microsoft.AspNetCore.Mvc.Filters;
#endif

namespace Fluent.Architecture.Filters
{
#if NET461
    public class ExceptionHandlerAttribute : HandleErrorAttribute
    {
        //private readonly ILog _logger;

        //public CustomHandleErrorAttribute()
        //{
        //    _logger = LogManager.GetLogger("MyLogger");
        //}

        public override void OnException(ExceptionContext filterContext)
        {
            if (filterContext.Exception is ContextFluentValidation exception)
            {
                filterContext.Result = new JsonResult
                {
                    JsonRequestBehavior = JsonRequestBehavior.AllowGet,
                    Data = exception
                }; 
            }
            else
            {
                if (filterContext.ExceptionHandled || !filterContext.HttpContext.IsCustomErrorEnabled)
                {
                    return;
                }

                if (new HttpException(null, filterContext.Exception).GetHttpCode() != 500)
                {
                    return;
                }

                if (!ExceptionType.IsInstanceOfType(filterContext.Exception))
                {
                    return;
                }

                // if the request is AJAX return JSON else view.
                if (filterContext.HttpContext.Request.Headers["X-Requested-With"] == "XMLHttpRequest")
                {
                    filterContext.Result = new JsonResult
                    {
                        JsonRequestBehavior = JsonRequestBehavior.AllowGet,
                        Data = new
                        {
                            Error = true,
                            Message = filterContext.Exception.Message
                        }
                    };
                }
                else
                {
                    var controllerName = (string) filterContext.RouteData.Values["controller"];
                    var actionName = (string) filterContext.RouteData.Values["action"];
                    var model = new HandleErrorInfo(filterContext.Exception, controllerName, actionName);

                    filterContext.Result = new ViewResult
                    {
                        ViewName = View,
                        MasterName = Master,
                        ViewData = new ViewDataDictionary<HandleErrorInfo>(model),
                        TempData = filterContext.Controller.TempData
                    };
                }

                //Todo implementar o log de erros aqui posteriormente
                // log the Error using log4net.
                // _logger.Error(filterContext.Exception.Message, filterContext.Exception);
            }


            filterContext.ExceptionHandled = true;
            filterContext.HttpContext.Response.Clear();
            filterContext.HttpContext.Response.StatusCode = 500;

            filterContext.HttpContext.Response.TrySkipIisCustomErrors = true;
        }
    }
#endif
    //Todo Implementar para net core
}

