
namespace Fluent.Architecture.Test
{
    public abstract class FluentInterceptor
    {
        //public abstract void Intercept(FluentInvocation invocation);
    }

    /// <summary>
    /// Classe interna.
    /// Interceptador de transações dos serviços.
    /// </summary>
    internal class TransactionInterceptor : FluentInterceptor
    {
        //internal TransactionObjects TransactionObjects { get; }
        //private Guid SessionRequestId { get; }

        //internal TransactionInterceptor(Guid sessionId)
        //{
        //    SessionRequestId = sessionId;
        //    TransactionObjects = TransactionObjects.Create(); ;
        //}

        //public override void Intercept(FluentInvocation invocation)
        //{
        //    //TransactionObjects.Session = ContextFactory.Create(TransactionObjects.DataBaseConnectionString);// new EfContext(TransactionObjects.DataBaseConnectionString);
        //    //invocation.Proceed();
        //    //TransactionObjects.Session.SaveChanges();
        //    //Setup.RemoveSession(SessionRequestId);
        //}
    }
}
