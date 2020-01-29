// -----------------------------------------------------------------------
// <copyright company="Fluent System">
//     Copyright © Fluent System. All rights reserved.
//     TODOS OS DIREITOS RESERVADOS.
// </copyright>
// -----------------------------------------------------------------------

// ReSharper disable CommentTypo

using Fluent.Architecture.Core.Attributes;
using Fluent.Architecture.Core.Extensions;

namespace Fluent.Architecture.Core.Models
{
    [FluentDoc]
    public class DefaultResult<T>
    {
        public T Data { get; set; }

        public DefaultResult() 
        {
            Data = default;
        }

        public DefaultResult(T data)
        {
            Data = data.FluentResultOrValue<T>();
        }
    }
}
