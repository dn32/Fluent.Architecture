#if NET461
// ReSharper disable CommentTypo

using Fluent.Architecture.Model;
using Fluent.Architecture.Util;
using Fluent.Architecture.Services;
using System.Web.Mvc;
using System;

namespace Fluent.Architecture.Controllers
{
    /// <inheritdoc />
    /// <summary>
    /// Controlador genérico padrão recomendado para herança por todos os controladores que tiverem entidade.
    /// </summary>
    /// <typeparam name="T">O tipo da entidade do controller.</typeparam>
    public abstract class FluentController<T> : FluentServiceController<FluentService<T>> where T : BaseEntity
    {

        protected T PropagateMethod(string methodName)
        {
            return this.PropagateInternal(methodName, Array.Empty<object>()) as T;
        }

        protected T PropagateMethod(string methodName, params object[] parameters)
        {
            return this.PropagateInternal(methodName, parameters) as T;
        }

        protected TX PropagateMethod<TX>(string methodName, object[] parameters)
        {
            return (TX)this.PropagateInternal(methodName, parameters);
        }

        protected T PropagateMethod(string methodName, object parameter)
        {
            return this.PropagateInternal(methodName, parameter == null ? Array.Empty<object>() : new[] { parameter }) as T;
        }

        protected TX PropagateMethod<TX>(string methodName, object parameter)
        {
            return (TX)this.PropagateInternal(methodName, parameter == null ? Array.Empty<object>() : new[] { parameter });
        }

        //===========================

        protected T Propagate()
        {
            return this.PropagateInternal(string.Empty, Array.Empty<object>()) as T;
        }

        protected T Propagate(object[] parameters)
        {
            return this.PropagateInternal(string.Empty, parameters) as T;
        }

        protected TX Propagate<TX>(object[] parameters)
        {
            return (TX)this.PropagateInternal(string.Empty, parameters);
        }

        protected TX Propagate<TX>(object parameter)
        {
            return (TX)this.PropagateInternal(string.Empty, new[] { parameter });
        }

        protected T Propagate(object parameter)
        {
            return (T)this.PropagateInternal(string.Empty, new[] { parameter });
        }

        //===================================

        private object PropagateInternal(string methodName, object[] parameters)
        {
            if (string.IsNullOrWhiteSpace(methodName))
            {
                methodName = GlobalUtil.GetMethodForPropagation()?.Name;
            }

            return this.Service.PropagateService(methodName, parameters);
        }

        protected internal new JsonResult Json(object data)
        {
            return new CustomJsonResult
            {
                Data = data,
            };
        }

        protected override JsonResult Json(object data, string contentType, System.Text.Encoding contentEncoding, JsonRequestBehavior behavior)
        {
            return new CustomJsonResult
            {
                Data = data,
                ContentType = contentType,
                ContentEncoding = contentEncoding,
                JsonRequestBehavior = behavior
            };
        }
    }
}
#endif