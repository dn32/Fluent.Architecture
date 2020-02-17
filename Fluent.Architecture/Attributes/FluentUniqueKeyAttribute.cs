// -----------------------------------------------------------------------
// <copyright company="Fluente System">
//     Copyright © Fluente System. All rights reserved.
//     TODOS OS DIREITOS RESERVADOS.
// </copyright>
// -----------------------------------------------------------------------

// ReSharper disable CommentTypo
using System;

namespace Fluente.Arquitetura.Attributes
{
    /// <inheritdoc />
    /// <summary>
    /// Indica que o método decorado representa uma chave de valor único no banco de dados.
    /// </summary>
    [AttributeUsage(AttributeTargets.Property)]
    public class FluenteUniqueKeyAttribute : Attribute
    {
    }
}