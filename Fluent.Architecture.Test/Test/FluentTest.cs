using System;
using Newtonsoft.Json;

namespace Fluent.Architecture.Test.Test
{
    /// <summary>
    /// Extensão de testes automatizados.
    /// </summary>
    public class FluentTest
    {
        /// <summary>
        /// Verifica se dois objetos são iguais comparando os valores e não a referência.
        /// </summary>
        /// <param name="obj1">
        /// Primeiro objeto a ser comparado.
        /// </param>
        /// <param name="obj2">
        /// Segundo objeto a ser comparado.
        /// </param>
        public bool CompareObjects(object obj1, object obj2)
        {
            var json1 = JsonConvert.SerializeObject(obj1);
            var json2 = JsonConvert.SerializeObject(obj2);
            return json1 == json2;
        }

        /// <summary>
        /// Obtem uma data baseado em string como exemplo: 31/12/18.
        /// </summary>
        /// <param name="ddMMyy">
        /// A string com a data desejada no formato ddMMyy. Exemplo: 31/12/18.
        /// </param>
        /// <returns>
        /// A data solicitada.
        /// </returns>
        public static DateTime GetDate(string ddMMyy)
        {
            return DateTime.ParseExact(ddMMyy, "dd/MMM/yy", System.Globalization.CultureInfo.InvariantCulture);
        }
    }
}
