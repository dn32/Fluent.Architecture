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

namespace Max.Services.Model.Crud
{

    [Table("MXSATIVI")]
    //[DbType(FluentDbType.SQL_SERVER)]
    [Display(Name = "Atividades")]
    [FluentJsonForm(name = "Atividades", desc = "Cadastro de Ramo de Atividade", group = "Clientes")]
    public partial class Atividade : FluentEntity
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
        public string codativ { get; set; }


        [FluentRequired, Searchable, FluentJsonProperty(
                name = "Ramo de Atividade",
                desc = "Informar ramo de atividade",
                min = 1,
                max = 60,
                lGrid = 6,
                value = "",
                group = "Informações gerais",
                tgroup = EnumGrupType.TAB,
                grid = "Ramo De Atividade",
                form = EnumForm.TEXTBOX
        )]
        public string ramo { get; set; }


        [FluentRequired, FluentJsonProperty(
                name = "% Desconto",
                desc = "Informar % desconto",
                min = 0,
                max = 999,
                lGrid = 3,
                value = "0",
                group = "Informações gerais",
                tgroup = EnumGrupType.TAB,
                grid = "",
                form = EnumForm.NUMBER
        )]
        public decimal? percdesc { get; set; }


        [FluentRequired, FluentJsonProperty(
                name = "Calcular ST",
                desc = "Informar calcular st",
                min = 1,
                max = 1,
                lGrid = 3,
                value = "",
                group = "Informações gerais",
                tgroup = EnumGrupType.TAB,
                grid = "",
                form = EnumForm.COMBOBOX
        )]
        public int calculast { get; set; }

    }
}

