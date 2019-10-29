// -----------------------------------------------------------------------
// <copyright company="Fluent System">
//     Copyright © Fluent System. All rights reserved.
//     TODOS OS DIREITOS RESERVADOS.
// </copyright>
// -----------------------------------------------------------------------

// ReSharper disable CommentTypo

using Fluent.Architecture.Extensions;

namespace Fluent.Architecture.Controllers
{
    public interface IDefaultResult { }

    public class DefaultResult<T> : IDefaultResult
    {
        public T Data { get; set; }

        public DefaultResult(object data)
        {
            Data = data.FluentResultOrValue<T>();
        }
    }
}
