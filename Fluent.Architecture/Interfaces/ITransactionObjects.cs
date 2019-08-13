using System;
using System.Linq;
using Fluent.Architecture.Entities;
using Microsoft.EntityFrameworkCore;

namespace Fluent.Architecture.Core.Interfaces
{
    public interface ITransactionObjects : IDisposable
    {
        DbContext Session { get; }

        IQueryable<TX> GetObjectQueryInternal<TX>() where TX : BaseEntity;

        DbSet<T> GetObjectInputDataInternal<T>() where T : class;
    }
}
