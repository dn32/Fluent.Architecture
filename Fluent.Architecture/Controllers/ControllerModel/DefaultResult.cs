// -----------------------------------------------------------------------
// <copyright company="Fluent System">
//     Copyright © Fluent System. All rights reserved.
//     TODOS OS DIREITOS RESERVADOS.
// </copyright>
// -----------------------------------------------------------------------

// ReSharper disable CommentTypo

namespace Fluent.Architecture.Controllers
{
    public class DefaultResult
    {
        public object Data { get; set; }

        public DefaultResult(object data)
        {
            Data = data;
        }
    }
}
