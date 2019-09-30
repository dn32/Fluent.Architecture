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
    public class DefaultResult
    {
        public object Data { get; set; }

        public DefaultResult(object data)
        {
            Data = data.FluentResultOrValue();
        }
    }
}
