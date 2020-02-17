using Fluente.Arquitetura.Nucleo.Enumerator;
using Fluente.Arquitetura.Nucleo.Extensoes;
using Fluente.Arquitetura.Nucleo.Specifications;
using Fluente.Arquitetura.Extensoes;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel;
using System.Threading.Tasks;
using Fluente.Arquitetura.Nucleo.Models;
using System.Collections.Generic;
using System;
using System.IO;
using Fluente.Arquitetura.Nucleo.Util;

namespace Fluente.Arquitetura.Controllers
{
    [Route("/api/[controller]/[action]")]
    [ApiController]
    public class FluenteAPIController<T> : FluenteController<T> where T : FluenteEntity, new() // Nunca defina como abstract, pois o controle de log espera essa classe como concreta
    {
        #region MANY

        [HttpGet]
        [Description("Get a paged list of all items")]
        [FluenteAction(Pagination = true, DynamicSpec = true)]
        public virtual async Task<DefaultPaginationResult<List<T>>> List()
        {
            var spec = CreateSpec<FluenteAllSpec<T>>().SetParameter(isList: true);
            var list = Service.ListAsync(spec);
            return await ResultAsync<List<T>>(await list, LastRequestPagination);
        }

        [HttpGet]
        [Route("/api/[controller]/ListByFilter")]
        [Description("Get a paginated list of items based on filters")]
        [FluenteAction(Pagination = true, DynamicSpec = true)]
        public virtual async Task<DefaultPaginationResult<List<T>>> ListByFilterGet([FromQuery, Description("The filters to apply to the query")] Filter[] Filters)
        {
            return await InternalListByFilterAsync(Filters);
        }

        [HttpPost]
        [Route("/api/[controller]/ListByFilter")]
        [Description("Get a paginated list of items based on filters")]
        [FluenteAction(Pagination = true, DynamicSpec = true)]
        public virtual async Task<DefaultPaginationResult<List<T>>> ListByFilterPostAsync([FromBody, Description("The filters to apply to the query")] Filter[] Filters)
        {
            return await InternalListByFilterAsync(Filters);
        }

        protected async Task<DefaultPaginationResult<List<T>>> InternalListByFilterAsync([FromBody, Description("The filters to apply to the query")] Filter[] Filters)
        {
            var spec = CreateSpec<FluenteFilterSpec<T>>().SetParameter(Filters, isList: true);
            return await ResultAsync<List<T>>(await Service.ListAsync(spec), LastRequestPagination);
        }

        [HttpGet]
        [Description("Get a paginated list of items based on a term")]
        [FluenteAction(Pagination = true, DynamicSpec = true)]
        public virtual async Task<DefaultPaginationTermResult<List<T>>> ListByTerm([Description("The term to use as a comparator")] string Term)
        {
            var spec = CreateSpec<TermSpec<T>>().SetParameter(Term, isList: true);
            var list = Service.ListAsync(spec);
            return await ResultAsync<List<T>>(await list, LastRequestPagination, Term);
        }

        #endregion

        #region ONE

        [HttpGet]
        [Route("/api/[controller]/FindByEntity")]
        [Description("Get an item based on its identifiers")]
        public virtual async Task<DefaultResult<T>> FindByEntityGet([FromQuery, Description("The entity you want to query")] T Entity)
        {
            return await ResultAsync<T>(await Service.FindAsync(Entity, false));
        }

        [HttpPost]
        [Route("/api/[controller]/FindByEntity")]
        [Description("Get an item based on its identifiers")]
        public virtual async Task<DefaultResult<T>> FindByEntityPost([FromBody, Description("The entity you want to query")] T Entity)
        {
            return await ResultAsync<T>(await Service.FindAsync(Entity, false));
        }

        [HttpGet]
        [Route("/api/[controller]/FindByFilter")]
        [Description("Get an item based on its filters")]
        [FluenteAction(DynamicSpec = true)]
        public virtual async Task<DefaultResult<T>> FindByFilterGet([FromQuery, Description("The filters to apply to the query")] Filter[] Filters)
        {
            return await InternalFindByFilterAsync(Filters);
        }

        [HttpPost]
        [Route("/api/[controller]/FindByFilter")]
        [Description("Get an item based on filters")]
        [FluenteAction(DynamicSpec = true)]
        public virtual async Task<DefaultResult<T>> FindByFilterPost([FromBody, Description("The filters to apply to the query")] Filter[] Filters)
        {
            return await InternalFindByFilterAsync(Filters);
        }

        private async Task<DefaultResult<T>> InternalFindByFilterAsync([FromBody] Filter[] Filters)
        {
            var spec = CreateSpec<FluenteFilterSpec<T>>().SetParameter(Filters, isList: false);
            var item = await Service.SingleOrDefaultAsync(spec);
            return await ResultAsync<T>(item);
        }

        [HttpGet]
        [Description("Get an item based on a term")]
        [FluenteAction(DynamicSpec = true)]
        public virtual async Task<DefaultResult<T>> FindByTerm([Description("The term to use as a comparator")] string Term)
        {
            var spec = CreateSpec<TermSpec<T>>().SetParameter(Term, isList: false);
            var item = Service.SingleOrDefaultAsync(spec);
            return await ResultAsync<T>(await item);
        }

        #endregion

        #region ANOTHER

        [HttpGet]
        [Description("Get an example of the item")]
        public virtual T ExampleData()
        {
            return typeof(T).GetExampleValue() as T;
        }

        [HttpGet]
        [Description("Get total amount of items")]
        public virtual async Task<DefaultResult<int>> Count()
        {
            return await ResultAsync<int>(await Service.CountAsync());
        }

        [HttpPost]
        [Description("Get the number of items based on filters")]
        public virtual async Task<DefaultResult<int>> CountByFilter([FromBody, Description("The filters to apply to the query")] Filter[] Filters)
        {
            var spec = CreateSpec<FluenteFilterSpec<T>>().SetParameter(Filters, isList: true);
            return await ResultAsync<int>(await Service.CountAsync(spec));
        }

        [HttpGet]
        [Route("/api/[controller]/ExistsByEntity")]
        [Description("Checks if an entity exists based on filters")]
        public virtual async Task<DefaultResult<bool>> ExistsByEntityGet([FromQuery, Description("The entity that wants to check for existence")] T Entity)
        {
            return await ResultAsync<bool>(await Service.ExistsAsync(Entity));
        }

        [HttpPost]
        [Route("/api/[controller]/ExistsByEntity")]
        [Description("Checks if an item exists based on their identifiers")]
        public virtual async Task<DefaultResult<bool>> ExistsByEntityPost([FromBody, Description("The entity that wants to check for existence")] T Entity)
        {
            return await ResultAsync<bool>(await Service.ExistsAsync(Entity));
        }

        [HttpGet]
        [Description("Get item type schema")]
        public virtual string JsonForm([Description("If you want to generate a tablet layout")] bool Tablet = false)
        {
            return typeof(T).GetFluenteJsonSchema(Tablet).ToFluenteJson();
        }

        [HttpGet]
        [Description("Get data import template")]
        public ActionResult ImportationTemplate(EnumTemplateType type, int addExample = 0)
        {
            using var workbook = DataImportationUtil.ImportationTemplateXLSX<T>(addExample);
            var fs = new MemoryStream();
            workbook.SaveAs(fs);
            fs.Position = 0;
            return new FileStreamResult(fs, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet") { FileDownloadName = $"{typeof(T).Name}.xlsx" };
        }

        [HttpPost]
        [Description("Data Import File Upload")]
        public async Task<ActionResult> UploadImportFile(EnumTemplateType type)
        {
            var file = HttpContext.Request.Form.Files[0];
            if (file.ContentType == "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet")
            {
                using var stream = file.OpenReadStream();
                using var workbook = await Service.ImportFileStreamAsync(stream);

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

        // POST api/user/Add/
        [HttpPost]
        [Description("Add an item")]
        public virtual async Task<DefaultResult<T>> Add([FromBody, Description("The entity you want to add")] T Entity)
        {
            return await ResultAsync<T>(await Service.AddAsync(Entity));
        }

        // POST api/user/AddOrUpdate/
        [HttpPost]
        [Description("Add or update an item")]
        public virtual async Task<DefaultResult<T>> AddOrUpdate([FromBody, Description("The entity you want to add or update")] T Entity)
        {
            return await ResultAsync<T>(await Service.AddOrUpdateAsync(Entity));
        }

        // POST api/user/AddRange
        [HttpPost]
        [Description("Add an item collection")]
        public virtual async Task<DefaultResult<T[]>> AddRange([FromBody, Description("The entities you want to add")] T[] Entities)
        {
            await Service.AddRangeAsync(Entities);
            return await ResultAsync<T[]>(Entities);
        }

        // PUT api/user/Update
        [HttpPut]
        [Description("Update an item")]
        public virtual async Task<DefaultResult<bool>> Update([FromBody, Description("The entity you want to update")] T Entity)
        {
            await Service.UpdateAsync(Entity);
            return await ResultAsync<bool>(true);
        }

        // PUT api/user/UpdateRange
        [HttpPut]
        [Description("Update an item collection")]
        public virtual async Task<DefaultResult<bool>> UpdateRange([FromBody, Description("The entities you want to update")] T[] Entities)
        {
            await Service.UpdateRangeAsync(Entities);
            return await ResultAsync<bool>(true);
        }

        // DELETE api/user/Remove
        [HttpPost]
        [HttpDelete]
        [Description("Remove item based on their identifiers")]
        public virtual async Task<DefaultResult<bool>> Remove([FromBody, Description("The entity you want to remove")] T Entity)
        {
            await Service.RemoveAsync(Entity);
            return await ResultAsync<bool>(true);
        }

        // DELETE api/user/RemoveRange
        [HttpPost]
        [HttpDelete]
        [Description("Remove a collection of items based on their identifiers")]
        public virtual async Task<DefaultResult<bool>> RemoveRange([FromBody, Description("The entities you want to remove")] T[] Entities)
        {
            await Service.RemoveRangeAsync(Entities);
            return await ResultAsync<bool>(true);
        }

        [HttpDelete]
        [Description("Physically delete all elements of a table")]
        public virtual async Task<DefaultResult<bool>> Truncate([FromHeader, Description("Confirmation that you really want to delete the data. If yes, enter \"Yes\"")] string ERASE_ALL_DATA = "false")
        {
            await Service.TruncateAsync(ERASE_ALL_DATA);
            return await ResultAsync<bool>(true);
        }
    }
}