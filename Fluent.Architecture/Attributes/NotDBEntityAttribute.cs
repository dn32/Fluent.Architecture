using System;

namespace Fluent.Architecture.Attributes
{
    /// <inheritdoc />
    /// <summary>
    /// Indica que o método decorado não será uma entidade no banco de dados.
    /// Entidades que herdarem da entidade decorada não serão afetados, ou seja, serão entidades do banco de dados se não forem decoradas também.
    /// </summary>
    [AttributeUsage(AttributeTargets.Class, Inherited = false)]
    public class NotDbEntityAttribute : Attribute
    {
    }
}