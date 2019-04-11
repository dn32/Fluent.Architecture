
using System;
using System.Linq;
using Fluent.Architecture.Model;

#if NET461
using System.Data.Entity;

#else
using Microsoft.EntityFrameworkCore;

#endif

namespace Fluent.Architecture.Core.Interfaces
{
    public interface ITransactionObjects : IDisposable
    {
        DbContext Session { get; }

        IQueryable<TX> GetObjectQueryInternal<TX>() where TX : BaseEntity;

        DbSet<T> GetObjectInputDataInternal<T>() where T : class;
    }
}
