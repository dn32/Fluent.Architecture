// -----------------------------------------------------------------------
// <copyright company="Fluente System">
//     Copyright © Fluente System. All rights reserved.
//     TODOS OS DIREITOS RESERVADOS.
// </copyright>
// -----------------------------------------------------------------------

// ReSharper disable CommentTypo

using Fluente.Arquitetura.Exceptions.ValidationException;
using System;

namespace Fluente.Arquitetura.Exceptions
{
    [Serializable]
    public class MethodNotFoundException : FluenteValidationException
    {
        public MethodNotFoundException(string message) : base(message)
        {
        }
    }
}