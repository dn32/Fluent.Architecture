using Fluent.Architecture.EntityFramework;

namespace Max.Infraestrutura.ClassesBase
{
    public abstract class MaxRepositorio<T> : FluentEFRepository<T> where T : MaxEntidade
    {
    }
}
