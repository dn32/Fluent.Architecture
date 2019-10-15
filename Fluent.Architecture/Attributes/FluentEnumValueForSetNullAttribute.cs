// -----------------------------------------------------------------------
// <copyright company="Fluent System">
//     Copyright © Fluent System. All rights reserved.
//     TODOS OS DIREITOS RESERVADOS.
// </copyright>
// -----------------------------------------------------------------------

// ReSharper disable CommentTypo
using System;

namespace Fluent.Architecture.Attributes
{
    [AttributeUsage(AttributeTargets.Enum, Inherited = false)]
    public class FluentEnumValueForSetNullAttribute : Attribute
    {
        public int Value { get; }
     
        public FluentEnumValueForSetNullAttribute(int value)
        {
            Value = value;
        }
    }
}