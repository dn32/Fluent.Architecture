using Fluente.Arquitetura.Nucleo.Models;
using Fluente.Arquitetura.Base.Models;
using Fluente.Arquitetura.Base.Atributos;
using Fluente.Arquitetura.Base.Enumeradores;
using Fluente.Arquitetura.Base.Extensoes;
using Fluente.Arquitetura.Interfaces;

namespace Fluente.Arquitetura.EntityFramework.SqLite
{
    [DbType(FluenteDbType.SQLITE)]
    public abstract class FluenteSqLiteEntity : FluenteEntidade
    {
    }
}
