using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using dn32.infra.dados;

namespace dn32.infra.Nucleo.Interfaces
{
    public interface ITransactionObjects : IDisposable
    {
        DbContext Session { get; }

        IQueryable<TX> GetObjectQueryInternal<TX>() where TX : EntidadeBase;

        DbSet<T> GetObjectInputDataInternal<T>() where T : EntidadeBase;

        IQueryable GetObjectInputDataInternal(Type type);
    }
}
