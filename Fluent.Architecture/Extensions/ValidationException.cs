
namespace Fluent.Architecture.Extensions
{
    /// <summary>
    /// Exceção de valização
    /// </summary>
    public class ValidationException: System.Exception
    {
        public ValidationException(string error): base(error) { }
    }
}
