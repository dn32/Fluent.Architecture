// -----------------------------------------------------------------------
// <copyright company="Fluent System">
//     Copyright © Fluent System. All rights reserved.
//     TODOS OS DIREITOS RESERVADOS.
// </copyright>
// -----------------------------------------------------------------------

// ReSharper disable CommentTypo
using System;

namespace Fluent.Architecture.Exceptions
{
    /// <inheritdoc />
    /// <summary>
    /// Exceção interna.
    /// Util para validar desenvilvimento incorreto.
    /// </summary>
    public class IncorrectDevelopmentException : Exception
    {
        public IncorrectDevelopmentException(string message)
            : base(message)
        {
        }
    }
}
