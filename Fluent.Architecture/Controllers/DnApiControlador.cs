using dn32.infra.dados;
using dn32.infra.enumeradores;
using dn32.infra.extensoes;
using dn32.infra.Extensoes;
using dn32.infra.nucleo.atributos;
using dn32.infra.Nucleo.Specifications;
using dn32.infra.Nucleo.Util;
using Microsoft.AspNetCore.Mvc;
using System;
using System.ComponentModel;
using System.IO;
using System.Threading.Tasks;

namespace dn32.infra.nucleo.controladores
{
    [Route("/api/[controller]/[action]")]
    [ApiController] // Nunca defina como abstrato, pois o controle de log espera essa classe como concreta
    public partial class DnApiControlador<T> : DnControlador<T> where T : DnEntidade, new()
    {
        #region ANOTHER

        [HttpGet]
        [Description("Get an example of the item")]
        public virtual T ExampleData()
        {
            return typeof(T).GetExampleValue() as T;
        }

        [HttpGet]
        [Description("Get total amount of items")]
        public virtual async Task<ResultadoPadrao<int>> Count()
        {
            return await this.CrieResultadoAsync<int>(await this.Servico.CountAsync());
        }

        [HttpPost]
        [Description("Get the number of items based on filters")]
        public virtual async Task<ResultadoPadrao<int>> CountByFilter([FromBody, Description("The filters to apply to the query")] Filtro[] Filters)
        {
            var spec = this.CriarEspecificacao<DnFilterSpec<T>>().SetParameter(Filters, ehLista: true);
            return await this.CrieResultadoAsync<int>(await this.Servico.CountAsync(spec));
        }

        [HttpGet]
        [Route("/api/[controller]/ExistsByEntity")]
        [Description("Checks if an entity exists based on filters")]
        public virtual async Task<ResultadoPadrao<bool>> ExistsByEntityGet([FromQuery, Description("The entity that wants to check for existence")] T Entity)
        {
            return await this.CrieResultadoAsync<bool>(await this.Servico.ExistsAsync(Entity));
        }

        [HttpPost]
        [Route("/api/[controller]/ExistsByEntity")]
        [Description("Checks if an item exists based on their identifiers")]
        public virtual async Task<ResultadoPadrao<bool>> ExistsByEntityPost([FromBody, Description("The entity that wants to check for existence")] T Entity)
        {
            return await this.CrieResultadoAsync<bool>(await this.Servico.ExistsAsync(Entity));
        }

        [HttpGet]
        [Description("Get item type schema")]
        public virtual string JsonForm([Description("If you want to generate a tablet layout")] bool Tablet = false)
        {
            return typeof(T).GetDnJsonSchema(Tablet).SerializarParaDnJson();
        }

        [HttpGet]
        [Description("Get data import template")]
        public ActionResult ImportationTemplate(EnumTipoDeTemplate type, int addExample = 0)
        {
            using var workbook = DataImportationUtil.ImportationTemplateXLSX<T>(addExample);
            var fs = new MemoryStream();
            workbook.SaveAs(fs);
            fs.Position = 0;
            return new FileStreamResult(fs, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet") { FileDownloadName = $"{typeof(T).Name}.xlsx" };
        }

        [HttpPost]
        [Description("Data Import File Upload")]
        public async Task<ActionResult> UploadImportFile(EnumTipoDeTemplate type)
        {
            var file = HttpContext.Request.Form.Files[0];
            if (file.ContentType == "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet")
            {
                using var stream = file.OpenReadStream();
                using var workbook = await this.Servico.ImportFileStreamAsync(stream);

                var fs = new MemoryStream();
                workbook.SaveAs(fs);
                fs.Position = 0;
                return new FileStreamResult(fs, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet") { FileDownloadName = $"{typeof(T).Name}.xlsx" };
            }
            else
            {
                throw new InvalidOperationException("Formato de arquivo inválido");
            }
        }

        #endregion               

        // POST api/user/Adicionar/
        [HttpPost]
        public virtual async Task<ResultadoPadrao<T>> Adicionar([FromBody, Description("The entity you want to add")] T entidade)
        {
            return await this.CrieResultadoAsync<T>(await this.Servico.AddAsync(entidade));
        }

        // POST api/user/AdicionarOuAtualizar/
        [HttpPost]
        public virtual async Task<ResultadoPadrao<T>> AdicionarOuAtualizar([FromBody, Description("The entity you want to add or update")] T Entity)
        {
            return await this.CrieResultadoAsync<T>(await this.Servico.AddOrUpdateAsync(Entity));
        }

        // POST api/user/AdicionarLista
        [HttpPost]
        public virtual async Task<ResultadoPadrao<T[]>> AdicionarLista([FromBody, Description("The entities you want to add")] T[] Entities)
        {
            await this.Servico.AddRangeAsync(Entities);
            return await this.CrieResultadoAsync<T[]>(Entities);
        }

        // PUT api/user/Atualizar
        [HttpPut]
        public virtual async Task<ResultadoPadrao<bool>> Atualizar([FromBody, Description("The entity you want to update")] T Entity)
        {
            await this.Servico.UpdateAsync(Entity);
            return await this.CrieResultadoAsync<bool>(true);
        }

        // PUT api/user/AtualizarLista
        [HttpPut]
        public virtual async Task<ResultadoPadrao<bool>> AtualizarLista([FromBody, Description("The entities you want to update")] T[] Entities)
        {
            await this.Servico.UpdateRangeAsync(Entities);
            return await this.CrieResultadoAsync<bool>(true);
        }

        // DELETE api/user/Remover
        [HttpPost]
        [HttpDelete]
        public virtual async Task<ResultadoPadrao<bool>> Remover([FromBody, Description("The entity you want to remove")] T Entity)
        {
            await this.Servico.RemoveAsync(Entity);
            return await this.CrieResultadoAsync<bool>(true);
        }

        // DELETE api/user/RemoverLista
        [HttpPost]
        [HttpDelete]
        public virtual async Task<ResultadoPadrao<bool>> RemoverLista([FromBody, Description("The entities you want to remove")] T[] Entities)
        {
            await this.Servico.RemoveRangeAsync(Entities);
            return await this.CrieResultadoAsync<bool>(true);
        }

        [HttpDelete]
        public virtual async Task<ResultadoPadrao<bool>> EliminarTudo([FromHeader, Description("Confirmation that you really want to delete the data. If yes, enter \"Yes\"")] string APAGAR_TUDO = "false")
        {
            await this.Servico.TruncateAsync(APAGAR_TUDO);
            return await this.CrieResultadoAsync<bool>(true);
        }
    }
}