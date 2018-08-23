//using System;

//namespace Fluent.Architecture.interceptors
//{
//    /// <summary>
//    /// Classe interna.
//    /// Interceptador de contexto do entity framework.
//    /// Mesmo que não esteja em uso, é interessante manter para implementações futuras.
//    /// </summary>
//    internal class ContextInterceptor : TransactionInterceptor
//    {
//        public override void Intercept(FluentInvocation invocation)
//        {
//            invocation.Proceed();
//        }

//        internal ContextInterceptor(Guid sessionId) : base(sessionId)
//        {
//        }
//    }
//}
