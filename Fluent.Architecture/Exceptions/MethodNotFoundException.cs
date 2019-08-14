// -----------------------------------------------------------------------
// <copyright company="Fluent System">
//     Copyright © Fluent System. All rights reserved.
//     TODOS OS DIREITOS RESERVADOS.
// </copyright>
// -----------------------------------------------------------------------

// ReSharper disable CommentTypo

using Fluent.Architecture.Exceptions.ValidationException;
using System;

namespace Fluent.Architecture.Exceptions
{
    [Serializable]
    public class MethodNotFoundException : FluentValidationException
    {
        public MethodNotFoundException(string message) : base(message)
        {
        }
    }
}