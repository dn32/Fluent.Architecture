// -----------------------------------------------------------------------
// <copyright company="Fluent System">
//     Copyright © Fluent System. All rights reserved.
//     TODOS OS DIREITOS RESERVADOS.
// </copyright>
// -----------------------------------------------------------------------

// ReSharper disable CommentTypo

using System.Threading.Tasks;

namespace Fluent.Architecture.Controllers
{
    public class DefaultResult
    {
        public object Data { get; set; }

        public DefaultResult(object data)
        {
            if (data != null)
            {
                if(data.GetType().GetGenericTypeDefinition() == typeof(Task<>))
                {
                    Data = data.GetType().GetProperty("Result").GetValue(data);
                }
                else
                {
                    Data = data;
                }
            }
        }
    }
}
