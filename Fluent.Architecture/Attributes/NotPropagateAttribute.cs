using System;

namespace Fluent.Architecture.Attributes
{
    /// <summary>
    /// Para uso interno.
    /// Indica que o método decorado não deve ser considerado na busca por método de propagação.
    /// É útil para não interferir na refleção da obtenção do método específico solicitado pelo cliente.
    /// </summary>
    [AttributeUsage(AttributeTargets.Method)]
    public class NotPropagateAttribute : Attribute
    {
    }
}