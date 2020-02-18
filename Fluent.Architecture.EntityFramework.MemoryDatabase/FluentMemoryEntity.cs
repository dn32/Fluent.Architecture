using Fluente.Arquitetura.Nucleo.Models;
using Fluente.Arquitetura.Base.Models;
using Fluente.Arquitetura.Base.Atributos;
using Fluente.Arquitetura.Base.Enumeradores;
using Fluente.Arquitetura.Base.Extensoes;
using Fluente.Arquitetura.Interfaces;

namespace Fluente.Arquitetura.EntityFramework.MemoryDatabase
{
    [DbType(FluenteDbType.MEMORY)]
    public abstract class FluenteMySQLEntity : FluenteEntidade
    {
    }
}
