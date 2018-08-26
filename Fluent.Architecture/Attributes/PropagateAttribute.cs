// ReSharper disable CommentTypo
using System;

namespace Fluent.Architecture.Attributes
{
    /// <summary>
    /// Indica que o método decorado com esse atributo oferece o padrão de propagação.
    /// </summary>
    [AttributeUsage(AttributeTargets.Method)]
    public class PropagateAttribute : Attribute
    {
    }
}
