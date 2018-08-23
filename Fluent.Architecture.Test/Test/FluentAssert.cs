namespace Fluent.Architecture.Test.Test
{
    /// <summary>
    /// Extensão de testes automatizados para comparação.
    /// </summary>
    public class FluentAssert
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
        /// <param name="message">
        /// Mensagem de erro em caso de serem diferentes. (Opcinal)
        /// </param>
        public static void Equal(object obj1, object obj2, string message= "The objects are different")
        {
            if (!new FluentTest().CompareObjects(obj1, obj2))
            {
                throw  new System.Exception(message);
            }
        }
    }
}