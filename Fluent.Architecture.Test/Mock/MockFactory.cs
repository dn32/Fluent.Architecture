using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using Fluent.Architecture.Extensions;

namespace Fluent.Architecture.Test.Mock
{
    /// <summary>
    /// Fábrica de Mock para testes simulados.
    /// </summary>
    /// <typeparam name="TX"></typeparam>
    public class MockFactory<TX>
    {
        protected string Key;

        internal MockFactory() { }

        /// <summary>
        /// Cria um novo mMck.
        /// </summary>
        /// <typeparam name="TResult">
        /// O tipo de retorno do Mock.
        /// </typeparam>
        /// <param name="expression">
        /// O método que deseja simular.
        /// </param>
        /// <returns>
        /// O <see cref="MockFactory{TX}"/> para definir o valor de return facilmente.
        /// </returns>
        public MockFactory<TX> Create<TResult>(Expression<Func<TX, TResult>> expression)
        {
            dynamic body = expression.Body;
            var methodName = ((MethodInfo)body.Method).GetFriendlyName();

            var parameters = ((ReadOnlyCollection<Expression>)body.Arguments).Select(GetExpressionValue).ToArray();
            Key = FluentMockUtil.CreateKeyForMock(typeof(TX).FullName, methodName, parameters);
            return this ;
        }

        /// <summary>
        /// Permite definir o valor desejado para o retorno do Mock.
        /// </summary>
        /// <param name="return">
        /// O valor de retorno desejado.
        /// </param>
        public virtual void Return(object @return)
        {
            throw new NotImplementedException();
            //Architecture.Setup.SetInTest();
            //TransactionInterceptorMock.Add(Key, @return);
        }

        /// <summary>
        /// Obtem o valor de uma expression, navegando recursivamente por sua hierarquia até encontrar a expression de valor equivalente.
        /// </summary>
        /// <param name="expression">
        /// A expression a ser avaliada.
        /// </param>
        /// <returns>
        /// O valor da expression.
        /// </returns>
        private static object GetExpressionValue(Expression expression)
        {
            var propValue = expression.GetType().GetProperty("Value");
            if (propValue == null)
            {
                expression = ((dynamic)expression).Expression;
                return GetExpressionValue(expression);
            }

            return propValue.GetValue(expression);
        }
    }
}