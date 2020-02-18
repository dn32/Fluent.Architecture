// -----------------------------------------------------------------------
// <copyright company="Dn System">
//     Copyright © Dn System. All rights reserved.
//     TODOS OS DIREITOS RESERVADOS.
// </copyright>
// -----------------------------------------------------------------------

// ReSharper disable CommentTypo

using dn32.infra.Exceptions.ValidationException;
using System;

namespace dn32.infra.Exceptions
{
    [Serializable]
    public class MethodNotFoundException : DnValidationException
    {
        public MethodNotFoundException(string message) : base(message)
        {
        }
    }
}