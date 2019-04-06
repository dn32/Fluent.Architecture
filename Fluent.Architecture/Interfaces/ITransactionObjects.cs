
using System;
using System.Linq;
using Fluent.Architecture.Model;

namespace Fluent.Architecture.Core.Interfaces
{
    public interface ITransactionObjects : IDisposable
    {
        object Session { get; }

        IQueryable<TX> GetObjectQueryInternal<TX>() where TX : BaseEntity;
        object GetObjectInputDataInternal<T>();
    }
}
