namespace Fluent.Architecture.interceptors
{
    public abstract class FluentInterceptor
    {
        public abstract void Intercept(FluentInvocation invocation);
    }
}