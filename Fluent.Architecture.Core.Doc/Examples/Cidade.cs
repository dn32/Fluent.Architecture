using System;
using Fluent.Architecture.Core.Attributes;
using Fluent.Architecture.Attributes;
using Fluent.Architecture.Core.Enumerator;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Collections.Generic;
using Newtonsoft.Json;
using Fluent.Architecture.Entities;
using Fluent.Architecture.Core.Doc.Examples;

namespace Max.Services.Model.Crud
{

    [Table("MXSCIDADE")]
    //[DbType(FluentDbType.SQL_SERVER)]
    [Display(Name = "Cidades")]
    [FluentJsonForm(name = "Cidades", desc = "Cadastro de Cidade", group = "Cadastros")]
    public partial class Cidade : FluentEntity
    {

        [FluentRequired, Key, Searchable, FluentJsonProperty(
                name = "Código",
                desc = "Informar código",
                min = 1,
                max = 50,
                lGrid = 3,
                value = "",
                group = "Informações gerais",
                tgroup = EnumGrupType.TAB,
                grid = "Código",
                form = EnumForm.TEXTBOX
        )]
        public string codcidade { get; set; }


        [FluentRequired, Searchable, FluentUniqueKey, FluentJsonProperty(
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


        [FluentRequired, Searchable, FluentUniqueKey, FluentJsonProperty(
                name = "Nome",
                desc = "Informar nome",
                min = 1,
                max = 50,
                lGrid = 6,
                value = "",
                group = "Informações gerais",
                tgroup = EnumGrupType.TAB,
                grid = "Nome",
                form = EnumForm.TEXTBOX
        )]
        public string nomecidade { get; set; }


        [FluentRequired, Searchable, FluentJsonProperty(
                name = "UF",
                desc = "Informar uf",
                min = 2,
                max = 2,
                lGrid = 3,
                value = "",
                group = "Informações gerais",
                tgroup = EnumGrupType.TAB,
                grid = "",
                form = EnumForm.HIDDEN
        )]
        public string uf { get; set; }

        [FluentJsonProperty(
                name = "UF",
                desc = "Informar uf",
                min = 2,
                max = 2,
                form = EnumForm.COMBOBOX,
                lGrid = 3,
                value = "",
                grid = "",
                group = "Informações gerais",
                tgroup = EnumGrupType.TAB
        )]
        [FluentAggregation(Display = nameof(Fluent.Architecture.Core.Doc.Examples.Estado.estado), PropertyForFindByProximity = nameof(Fluent.Architecture.Core.Doc.Examples.Estado.estado), LocalKey = nameof(uf), ExternalKey = nameof(Fluent.Architecture.Core.Doc.Examples.Estado.uf))]
        [ForeignKey(nameof(uf))]
        public virtual Estado Estado { get; set; }

    }
}

