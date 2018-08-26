// ReSharper disable CommentTypo
namespace Fluent.Architecture.Exception
{
    /// <inheritdoc />
    /// <summary>
    /// Exceção interna.
    /// Util para validar desenvilvimento incorreto.
    /// </summary>
    public class IncorrectDevelopmentException : System.Exception
    {
        public IncorrectDevelopmentException(string message) : base(message) { }
    }
}
