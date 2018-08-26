// ReSharper disable CommentTypo
using Fluent.Architecture.Attributes;
using Fluent.Architecture.Model;
using Fluent.Architecture.Service;
using Fluent.Architecture.Util;
using System.Text;
using Fluent.Architecture.Exception;
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
#if PROPAGATION
        ///// <summary>
        // /// Invoca um método no repositório, passando por serviço e validação quando houverem. 
        // /// </summary>
        // /// <param name="methodName">Método a ser invocado no repositório.</param>
        // /// <param name="parameters">Parametros opcionais a serem enviados.</param>
        // /// <returns>O resultado da operação solocitada quando houver.</returns>
        // [NotPropagate]
        // protected object Propagate(string methodName, params object[] parameters)
        // {
        //     return PropagateInternal<T>(methodName, parameters);
        // }

        // /// <summary>
        // /// Invoca um método no repositório com o mesmo nome do método onde a invocação do <see cref="Propagate(string,object[])">Propagate</see> está sendo feita, passando por serviço e validação quando houverem. 
        // /// </summary>
        // /// <param name="parameters">Parametros opcionais a serem enviados.</param>
        // /// <returns>O resultado da operação solocitada quando houver.</returns>
        // [NotPropagate]
        // protected object Propagate(params object[] parameters)
        // {
        //     return PropagateInternal<T>(string.Empty, parameters);
        // }

        // /// <summary>
        // /// Invoca um método no repositório, passando por serviço e validação quando houverem. 
        // /// </summary>
        // /// <typeparam name="T2">O tipo de entidade para o qual o repositório se refere.</typeparam>
        // /// <param name="methodName">Método a ser invocado no repositório.</param>
        // /// <param name="parameters">Parametros opcionais a serem enviados.</param>
        // /// <returns>O resultado da operação solocitada quando houver.</returns>
        // [NotPropagate]
        // protected object Propagate<T2>(string methodName, params object[] parameters) where T2 : BaseEntity
        // {
        //     return PropagateInternal<T2>(methodName, parameters);
        // }

        // /// <summary>
        // /// Invoca um método no repositório com o mesmo nome do método onde a invocação do <see cref="Propagate(string,object[])">Propagate</see> está sendo feita, passando por serviço e validação quando houverem. 
        // /// </summary>
        // /// <typeparam name="T2">O tipo de entidade para o qual o repositório se refere.</typeparam>
        // /// <param name="parameters">Parametros opcionais a serem enviados.</param>
        // /// <returns>O resultado da operação solocitada quando houver.</returns>
        // [NotPropagate]
        // protected object Propagate<T2>(params object[] parameters) where T2 : BaseEntity
        // {
        //     return PropagateInternal<T2>(string.Empty, parameters);
        // }

        [NotPropagate]
        protected T Propagate(params object[] parameters)
        {
            return PropagateInternal<T>(string.Empty, parameters) as T;
        }

        [NotPropagate]
        protected T Propagate(string methodName, params object[] parameters)
        {
            return PropagateInternal<T>(methodName, parameters) as T;
        }

        [NotPropagate]
        private object PropagateInternal<T2>(string methodName, object[] parameters) where T2 : BaseEntity
        {
            if (string.IsNullOrWhiteSpace(methodName))
            {
                methodName = GlobalUtil.GetMethodByCallerType(GetType())?.Name;

                if (string.IsNullOrWhiteSpace(methodName))
                {
                    throw new IncorrectDevelopmentException($"A propagation request was unsuccessful.\nThe request does not indicate the method name and could not be obtained by reflection.\nMake sure that the method that calls the propagation is not decorated with {nameof(NotPropagateAttribute)}, as it should not be.");
                }
            }

            return Service.PropagateService<T2>(methodName, parameters);
        }

        ///// <summary>
        ///// Método interno.
        ///// Invoca um método no repositório, passando por serviço e validação quando houverem. 
        ///// </summary>
        ///// <typeparam name="T2">O tipo de entidade para o qual o repositório se refere.</typeparam>
        ///// <param name="methodName">Método a ser invocado no repositório.</param>
        ///// <param name="parameters">Parametros opcionais a serem enviados.</param>
        ///// <returns>O resultado da operação solocitada quando houver.</returns>
        //[NotPropagate]
        //private object PropagateInternal<T2>(string methodName, object[] parameters) where T2 : BaseEntity
        //{
        //    if (string.IsNullOrEmpty(methodName))
        //    {
        //        methodName = GlobalUtil.GetMethodNameByCallerType(GetType());
        //    }

        //    var ret = GlobalUtil.GetPropagationMethod<T, T>(methodName, Service, GetType(), parameters, true);
        //    return ret ?? Service.PropagateService<T2>(methodName, this, parameters);
        //}
#endif

#if NET461
        protected internal new JsonResult Json(object data)
        {
            return Json(data, JsonRequestBehavior.AllowGet);
        }
#else

#endif
    }
}
