
namespace Fluent.Architecture.Extensions
{
    /// <summary>
    /// Exceção de valização
    /// </summary>
    public class FluentValidationException: System.Exception
    {
        public FluentValidationException(string error): base(error) { }
    }
}
