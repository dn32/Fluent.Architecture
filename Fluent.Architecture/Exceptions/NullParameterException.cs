namespace Fluent.Architecture.Exceptions
{
    using System;

    public class NullParameterException : Exception
    {
        public NullParameterException(string parameter) : base(parameter)
        {
        }
    }
}