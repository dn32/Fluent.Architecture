//using Fluent.Architecture.Factory;
//using Fluent.Architecture.Service;

//namespace Fluent.Architecture.Test.Mock
//{
//    /// <inheritdoc />
//    /// <summary>
//    /// Fábrica de Mock de serviço para testes com simulação de serviço.
//    /// </summary>
//    /// <typeparam name="TS">
//    /// O tipo de serviço a ser "mockado".
//    /// </typeparam>
//    public class ServiceMockFactory<TS> : MockFactory<TS> where TS : BaseService, new()
//    {

//        /// <summary>
//        /// Cria um serviço para testes.
//        /// </summary>
//        /// <typeparam name="TX"></typeparam>
//        /// <param name="httpContext"></param>
//        /// <returns></returns>
//        public static TransactionalService CreateService<TX>(object httpContext) where TX : TransactionalService, new()
//        {
//            return ServiceFactory.Create<TX>(httpContext);
//        }
//    }
//}