using Fluent.Architecture.Core.Attributes;
using Fluent.Architecture.Attributes;
using Fluent.Architecture.Core.Enumerator;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Fluent.Architecture.Entities;

namespace Fluent.Architecture.Core.Doc.Examples
{

    [Table("ERP_MXSESTADO")]
  //  [DbType(FluentDbType.SQL_SERVER)]
    [Display(Name = "Estados (UF)")]
    [FluentJsonForm(name = "Estados (UF)", desc = "Cadastro de Estado (UF)", group = "Cadastros")]
    public partial class Estado : FluentEntity
    {

        [FluentRequired, Key, Searchable, FluentJsonProperty(
                name = "UF",
                desc = "Informar uf",
                min = 2,
                max = 2,
                lGrid = 3,
                value = "",
                group = "Informações gerais",
                tgroup = EnumGrupType.TAB,
                grid = "Uf",
                form = EnumForm.TEXTBOX
        )]
        public string uf { get; set; }


        [FluentRequired, Searchable, FluentUniqueKey, FluentJsonProperty(
                name = "Nome",
                desc = "Informar nome",
                min = 1,
                max = 30,
                lGrid = 3,
                value = "",
                group = "Informações gerais",
                tgroup = EnumGrupType.TAB,
                grid = "Nome",
                form = EnumForm.TEXTBOX
        )]
        public string estado { get; set; }


        [ FluentJsonProperty(
                name = "Código IBGE",
                desc = "Informar código ibge",
                min = 1,
                max = 50,
                lGrid = 3,
                value = "",
                group = "Informações gerais",
                tgroup = EnumGrupType.TAB,
                grid = "",
                form = EnumForm.TEXTBOX
        )]
        public string codibge { get; set; }

    }
}

