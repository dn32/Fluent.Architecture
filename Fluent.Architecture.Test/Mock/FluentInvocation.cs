// -----------------------------------------------------------------------
// <copyright company="Fluent System">
//     Copyright © Fluent System. All rights reserved.
//     TODOS OS DIREITOS RESERVADOS.
// </copyright>
// -----------------------------------------------------------------------

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

        public void Proceed()
        {
            throw new NotImplementedException();
        }
    }
}