// ReSharper disable CommentTypo
using System;

namespace Fluent.Architecture.Attributes
{
    /// <inheritdoc />
    /// <summary>
    /// Indica que o método decorado representa uma chave de valor único no banco de dados.
    /// </summary>
    [AttributeUsage(AttributeTargets.Property)]
    public class FluentUnicKeyAttribute : Attribute
    {
    }
}