// ReSharper disable CommentTypo

using System;

namespace Fluent.Architecture.Exceptions
{
    [Serializable]
    public class MethodNotFoundException : Exception
    {
        public MethodNotFoundException(string message) : base(message)
        {
        }
    }
}