using Fluente.Arquitetura.Nucleo.Models;
using Fluente.Arquitetura.Base.Models;
using Fluente.Arquitetura.Base.Atributos;
using Fluente.Arquitetura.Base.Enumeradores;
using Fluente.Arquitetura.Base.Extensoes;
using Fluente.Arquitetura.Interfaces;

namespace Fluente.Arquitetura.EntityFramework.PostgreSQL
{
    [DbType(FluenteDbType.POSTGREE_SQL)]
    public abstract class FluentePostgreEntity : FluenteEntidade
    {
    }
}
