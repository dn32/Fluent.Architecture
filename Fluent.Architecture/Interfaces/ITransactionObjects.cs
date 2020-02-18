using Fluente.Arquitetura.Nucleo.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using Fluente.Arquitetura.Base.Atributos;
using Fluente.Arquitetura.Base.Enumeradores;
using Fluente.Arquitetura.Base.Extensoes;
using Fluente.Arquitetura.Base.Atributos;
using Fluente.Arquitetura.Base.Enumeradores;
using Fluente.Arquitetura.Base.Extensoes;
using Fluente.Arquitetura.Base.Atributos;
using Fluente.Arquitetura.Base.Enumeradores;
using Fluente.Arquitetura.Base.Extensoes;
using Fluente.Arquitetura.Base.Models;
using Fluente.Arquitetura.Base.Atributos;
using Fluente.Arquitetura.Base.Enumeradores;
using Fluente.Arquitetura.Base.Extensoes;
using Fluente.Arquitetura.Base.Models;
using Fluente.Arquitetura.Base.Atributos;
using Fluente.Arquitetura.Base.Enumeradores;
using Fluente.Arquitetura.Base.Extensoes;
using Fluente.Arquitetura.Interfaces;
namespace Fluente.Arquitetura.Nucleo.Interfaces
{
    public interface ITransactionObjects : IDisposable
    {
        DbContext Session { get; }

        IQueryable<TX> GetObjectQueryInternal<TX>() where TX : EntidadeBase;

        DbSet<T> GetObjectInputDataInternal<T>() where T : EntidadeBase;

        IQueryable GetObjectInputDataInternal(Type type);
    }
}
