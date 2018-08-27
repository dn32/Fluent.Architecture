// ReSharper disable CommentTypo

using System.Linq;
using Fluent.Architecture.Attributes;
using Fluent.Architecture.Model;
using Fluent.Architecture.Util;
using Fluent.Architecture.Exception;
using Fluent.Architecture.Services;
#if NET461
using System.Web.Mvc;
#else
using Microsoft.AspNetCore.Mvc;
#endif

namespace Fluent.Architecture.Controllers
{
    /// <inheritdoc />
    /// <summary>
    /// Controlador genérico padrão recomendado para herança por todos os controladores que tiverem entidade.
    /// </summary>
    /// <typeparam name="T">O tipo da entidade do controller.</typeparam>
    public abstract class FluentController<T> : FluentServiceController<FluentService<T>> where T : BaseEntity
    {
        #region PROPAGATION
        
        [PropagateInit, NotPropagate]
        protected T PropagateMethod(string methodName)
        {
            return PropagateInternal(methodName, new object[] { }) as T;
        }

        [PropagateInit, NotPropagate]
        protected T PropagateMethod(string methodName, params object[] parameters)
        {
            return PropagateInternal(methodName, parameters) as T;
        }

        [PropagateInit, NotPropagate]
        protected TX PropagateMethod<TX>(string methodName, object[] parameters)
        {
            return (TX)PropagateInternal(methodName, parameters);
        }

        [PropagateInit, NotPropagate]
        protected T PropagateMethod(string methodName, object parameter)
        {
            return PropagateInternal(methodName, parameter == null ? new object[] { } : new object[] { parameter }) as T;
        }

        [PropagateInit, NotPropagate]
        protected TX PropagateMethod<TX>(string methodName, object parameter)
        {
            return (TX)PropagateInternal(methodName, parameter == null ? new object[] { } : new[] { parameter });
        }

        //===========================

        [PropagateInit, NotPropagate]
        protected T Propagate()
        {
            return PropagateInternal(string.Empty, new object[] { }) as T;
        }

        [PropagateInit, NotPropagate]
        protected T Propagate(object[] parameters)
        {
            return PropagateInternal(string.Empty, parameters) as T;
        }

        [PropagateInit, NotPropagate]
        protected TX Propagate<TX>(object[] parameters)
        {
            return (TX)PropagateInternal(string.Empty, parameters);
        }

        [PropagateInit, NotPropagate]
        protected TX Propagate<TX>(object parameter)
        {
            return (TX)PropagateInternal(string.Empty, new[] { parameter });
        }

        [PropagateInit, NotPropagate]
        protected T Propagate(object parameter)
        {
            return (T)PropagateInternal(string.Empty, new object[] { parameter });
        }

        //===================================

        [NotPropagate]
        private object PropagateInternal(string methodName, object[] parameters)
        {
            if (string.IsNullOrWhiteSpace(methodName))
            {
                methodName = GlobalUtil.GetMethodForPropagation()?.Name;
            }

            return Service.PropagateService(methodName, parameters);
        }

        #endregion

#if NET461
        protected internal new JsonResult Json(object data)
        {
            return Json(data, JsonRequestBehavior.AllowGet);
        }
#else

#endif
    }
}
