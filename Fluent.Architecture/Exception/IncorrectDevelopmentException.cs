namespace Fluent.Architecture.Exception
{
    /// <inheritdoc />
    /// <summary>
    /// Exceção interna.
    /// Util para validar desenvilvimento incorreto.
    /// </summary>
    internal class IncorrectDevelopmentException : System.Exception
    {
        public IncorrectDevelopmentException(string message) : base(message) { }
    }
}
