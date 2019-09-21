using Fluent.Architecture.Core.Attributes;
using Fluent.Architecture.Attributes;
using Fluent.Architecture.Core.Enumerator;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Fluent.Architecture.Entities;

namespace Max.Services.Model.Crud
{

    [Table("MXSBANCO")]
    //[DbType(FluentDbType.SQL_SERVER)]
    [Display(Name = "Bancos")]
    [FluentJsonForm(name = "Bancos", desc = "Cadastro de Bancos (Financeiro)", group = "Financeiro")]
    public partial class Banco : FluentEntity
    {

        [FluentRequired, Key, Searchable, FluentJsonProperty(
                name = "Código",
                desc = "Informar código",
                min = 1,
                max = 9999,
                lGrid = 3,
                value = "",
                group = "Informações gerais",
                tgroup = EnumGrupType.TAB,
                grid = "Código",
                form = EnumForm.NUMBER
        )]
        public string codbanco { get; set; }


        [FluentRequired, Searchable, FluentUniqueKey, FluentJsonProperty(
                name = "Nome",
                desc = "Informar nome",
                min = 1,
                max = 60,
                lGrid = 6,
                value = "",
                group = "Informações gerais",
                tgroup = EnumGrupType.TAB,
                grid = "Nome",
                form = EnumForm.TEXTBOX
        )]
        public string nome { get; set; }

    }
}

