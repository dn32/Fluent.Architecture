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
    [AttributeUsage(AttributeTargets.Enum, Inherited = false)]
    public class FluenteEnumValueForSetNullAttribute : Attribute
    {
        public int Value { get; }

        public FluenteEnumValueForSetNullAttribute(int value)
        {
            Value = value;
        }
    }
}