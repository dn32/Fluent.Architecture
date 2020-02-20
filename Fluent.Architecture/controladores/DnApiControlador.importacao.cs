using dn32.infra.enumeradores;
using dn32.infra.Nucleo.Util;
using Microsoft.AspNetCore.Mvc;
using System;
using System.IO;
using System.Threading.Tasks;

namespace dn32.infra.nucleo.controladores
{
    public partial class DnApiControlador<T>
    {
        //Todo - O processo de importação não foi concluído. Falta tratar os enumeradores e eventuais tipos especiais e possíveis nulos
        [HttpGet]
        public ActionResult ObterTemplateDeImportacao(EnumTipoDeTemplate tipo, int quantidadeDeExemplosAAdicionar = 0)
        {
            using var workbook = DataImportationUtil.ImportationTemplateXLSX<T>(quantidadeDeExemplosAAdicionar);
            var fs = new MemoryStream(); //Todo - vazamento de memória aqui. Encontrar uma forma de finalizar
            workbook.SaveAs(fs);
            fs.Position = 0;
            return new FileStreamResult(fs, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet") { FileDownloadName = $"{typeof(T).Name}.xlsx" };
        }

        [HttpPost]
        public async Task<ActionResult> EnviarArquivoParaImportacao(EnumTipoDeTemplate tipo)
        {
            var arquivo = this.HttpContext.Request.Form.Files[0];
            if (arquivo.ContentType != "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet")
            {
                throw new InvalidOperationException("Formato de arquivo inválido");
            }

            using var stream = arquivo.OpenReadStream();
            using var workbook = await this.Servico.ImportFileStreamAsync(stream);

            var fs = new MemoryStream(); //Todo - vazamento de memória aqui. Encontrar uma forma de finalizar
            workbook.SaveAs(fs);
            fs.Position = 0;
            return new FileStreamResult(fs, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet") { FileDownloadName = $"{typeof(T).Name}.xlsx" };
        }
    }
}