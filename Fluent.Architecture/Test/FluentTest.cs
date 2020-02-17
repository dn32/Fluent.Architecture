// -----------------------------------------------------------------------
// <copyright company="Fluente System">
//     Copyright © Fluente System. All rights reserved.
//     TODOS OS DIREITOS RESERVADOS.
// </copyright>
// -----------------------------------------------------------------------

// ReSharper disable CommentTypo

using System;

namespace Fluente.Arquitetura.Test
{
    /// <summary>
    /// Extensão de testes automatizados.
    /// </summary>
    public static class FluenteTest
    {
        /// <summary>
        /// Obtem uma data baseado em string como exemplo: 31/12/18.
        /// </summary>
        /// <param name="ddMMyy">
        /// A string com a data desejada no formato ddMMyy. Exemplo: 31/12/18.
        /// </param>
        /// <returns>
        /// A data solicitada.
        /// </returns>
        public static DateTime GetDate(this string ddMMyy)
        {
            return DateTime.ParseExact(ddMMyy, "dd/MM/yy", System.Globalization.CultureInfo.InvariantCulture);
        }
    }
}
