//using System;
//using Fluent.Architecture.Mock;
//using Fluent.Architecture.Test;

//namespace Fluent.Architecture.interceptors
//{
//    /// <summary>
//    /// Classe interna.
//    /// Interceptador de Mock usado em testes automatizados para operações com simulação em serviços.
//    /// </summary>
//    internal class TransactionInterceptorMock : FluentInterceptor
//    {
//        private readonly bool _transactional;

//        public TransactionInterceptorMock(bool transactional, Guid sessionId)// : base(sessionId)
//        {
//            _transactional = transactional;
//        }

//        public static void Add(string token, object @return)
//        {
//            Setup.Mocks.Add(token, @return);
//        }

//        public override void Intercept(FluentInvocation invocation)
//        {
//            var key = FluentMockUtil.CreateKeyForMock(invocation);
//            if (Setup.Mocks.TryGetValue(key, out var @return))
//            {
//                invocation.ReturnValue = @return;
//                return;
//            }

//            if (_transactional)
//            {
//                //base.Intercept(invocation);
//                return;
//            }

//            invocation.Proceed();
//        }
//    }
//}