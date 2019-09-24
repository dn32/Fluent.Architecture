using Fluent.Architecture.Controllers;
using Fluent.Architecture.Entities;
using Max.Services.Model.Crud;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Fluent.Architecture.Core.Doc.Examples
{
    public abstract class MaxEntidade : FluentEntity
    {
        [BindNever]
        [JsonIgnore]
        public long ATUALIZID { get; set; }

        [BindNever]
        [JsonIgnore]
        public int CODOPERACAO { get; set; }

        [BindNever]
        [JsonIgnore]
        public DateTime DTATUALIZ { get; set; }
    }

    public class MaxAPIController<T> : FluentAPIController<T> where T : MaxEntidade, new()
    {
    }

    [Route("api/[controller]/[action]")]
    public class BaseController<T> : MaxAPIController<T> where T : MaxEntidade, new()
    {
    }

    public class CidadetadoraController : BaseController<Cidade>
    {
    }

    public class EstadoController : BaseController<Estado>
    {
    }
    public class AtividadeController : BaseController<Atividade>
    {
    }
    public class BancoController : BaseController<Banco>
    {
    }
}
