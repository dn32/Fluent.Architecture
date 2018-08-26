using System;

namespace Fluent.Architecture.Test
{
    /// <summary>
    /// Extensão de testes automatizados.
    /// </summary>
    public static class FluentTest
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
            return DateTime.ParseExact(ddMMyy, "dd/MMM/yy", System.Globalization.CultureInfo.InvariantCulture);
        }
    }
}
