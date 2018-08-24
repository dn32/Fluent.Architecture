using System;
using System.Reflection;

namespace Fluent.Architecture.Test.Mock
{
    public class FluentInvocation
    {
        public MethodInfo Method { get; set; }
        public Type TargetType { get; set; }
        public object[] Arguments { get; set; }
        public object ReturnValue { get; set; }

        internal void Proceed()
        {
            throw new NotImplementedException();
        }
    }
}