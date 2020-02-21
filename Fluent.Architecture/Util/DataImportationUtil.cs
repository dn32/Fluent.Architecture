using ClosedXML.Excel;
using dn32.infra.extensoes;
using dn32.infra.nucleo.atributos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace dn32.infra.Nucleo.Util
{
    public static class DataImportationUtil
    {
        public static XLWorkbook ImportationTemplateXLSX<T>(int addExample)
        {
            var schema = typeof(T).GetDnJsonSchema(false);
            var table = schema.Formulario.NomeDaPropriedade;
            var properties = schema.Propriedades.Where(x => x.Composicao == null && x.Agregacao == null).ToList();

            var workbook = new XLWorkbook();
            var worksheet = workbook.Worksheets.Add(table);
            var propertiesExample = typeof(T).GetProperties();
            int i;

            for (i = 0; i < properties.Count; i++)
            {
                var property = properties[i];
                var headerCell = worksheet.Column(i + 1).Cell(1);
                headerCell.Value = property.NomeDaPropriedade;

                Example<T>(addExample, worksheet, propertiesExample, i, property);
                Style(property, headerCell);
                Comment(property, headerCell);
            }

            var headerCell2 = worksheet.Column(i + 1).Cell(1);
            headerCell2.Value = "Status";

            worksheet.Columns().AdjustToContents();
            return workbook;
        }

        public static List<Tuple<IXLCell, T>> ImportFileStream<T>(XLWorkbook workbook)
        {
            {
                var worksheets = workbook.Worksheets.ToList();
                var worksheet = worksheets.FirstOrDefault(x => x.Name.Equals(typeof(T).Name, StringComparison.InvariantCultureIgnoreCase));
                if (worksheet == null) throw new InvalidOperationException($"worksheet {typeof(T).Name} not found");

                var schema = typeof(T).GetDnJsonSchema(false);
                var table = schema.Formulario.NomeDaPropriedade;
                var properties = schema.Propriedades.Where(x => x.Composicao == null && x.Agregacao == null).ToList();

                var propertiesExample = typeof(T).GetProperties();
                int i;
                var list = new List<Tuple<IXLCell, T>>();
                int item = 0;

                while (true)
                {
                    var entidade = Activator.CreateInstance<T>();
                    var quantNull = 0;

                    for (i = 0; i < properties.Count; i++)
                    {
                        var property = properties[i];
                        var example = typeof(T).GetExampleValue();
                        var exampleCell = worksheet.Column(i + 1).Cell(2 + item);
                        var exampleProperty = propertiesExample.FirstOrDefault(x => x.Name == property.NomeDaPropriedadeCaseSensitive);
                        var valor = exampleCell.Value.ToString();
                        exampleProperty.SetValue(entidade, valor);
                        if (string.IsNullOrWhiteSpace(valor)) quantNull++;
                    }

                    if (quantNull == properties.Count) break;
                    var statusCell = worksheet.Column(i + 1).Cell(2 + item);
                    list.Add(new Tuple<IXLCell, T>(statusCell, entidade));
                    item++;
                }

                return list;
            }

            {
                var worksheets = workbook.Worksheets.ToList();
                var worksheet = worksheets.FirstOrDefault(x => x.Name.Equals(typeof(T).Name, StringComparison.InvariantCultureIgnoreCase));
                if (worksheet == null) throw new InvalidOperationException($"worksheet {typeof(T).Name} not found");
                var columns = worksheet.ColumnsUsed().ToList();
                var cells = worksheet.Cells().ToList();
                var type = typeof(T);
                var properties = type.GetProperties();
                var xlsxProperty = new List<PropertyInfo>();



                var propertiesCount = columns.Count;
                var itemsCount = cells.Count / propertiesCount - 1;

                for (int i = 0; i < propertiesCount; i++)
                {
                    var cell = cells[i];
                    var valor = cell.Value;
                    var property = properties.FirstOrDefault(x => x.Name == cell.Value?.ToString());
                    xlsxProperty.Add(property);
                }


                var list = new List<Tuple<IXLCell, T>>();

                for (int linha = 1; linha <= itemsCount; linha++)
                {
                    var statusCell = worksheet.Column(propertiesCount + 1).Cell(linha + 1);

                    var entidade = Activator.CreateInstance<T>();
                    list.Add(new Tuple<IXLCell, T>(statusCell, entidade));

                    for (int i = 0; i < propertiesCount; i++)
                    {
                        var property = xlsxProperty[i];
                        if (property == null) { continue; }
                        var celNum = linha * columns.Count + i;
                        var cell = cells[celNum];
                        var valor = cell.Value;
                        property.SetValue(entidade, cell.Value?.ToString() ?? property.PropertyType.GetDnDefaultValue());
                    }
                }
            }

            return null;
        }

        private static void Example<T>(int addExample, IXLWorksheet worksheet, PropertyInfo[] propertiesExample, int i, DnPropriedadeJsonAtributo property)
        {
            for (var x = 0; x < addExample; x++)
            {
                var example = typeof(T).GetExampleValue();
                var exampleCell = worksheet.Column(i + 1).Cell(2 + x);
                var exampleProperty = propertiesExample.FirstOrDefault(x => x.Name == property.NomeDaPropriedadeCaseSensitive);
                exampleCell.Value = exampleProperty?.GetValue(example) ?? "";
            }
        }

        private static void Style(DnPropriedadeJsonAtributo property, IXLCell headerCell)
        {
            headerCell.Style.Font.FontSize = 11;
            headerCell.Style.Protection.Locked = true;
            headerCell.Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
            headerCell.Style.Fill.BackgroundColor = XLColor.FromArgb(0xF8F8F8);
            headerCell.Style.Font.FontColor = XLColor.Black;

            headerCell.Style.Border.TopBorder = XLBorderStyleValues.Medium;
            headerCell.Style.Border.RightBorder = XLBorderStyleValues.Medium;
            headerCell.Style.Border.BottomBorder = XLBorderStyleValues.Medium;
            headerCell.Style.Border.LeftBorder = XLBorderStyleValues.Medium;
            headerCell.Style.Border.BottomBorderColor = XLColor.FromArgb(0x777777);
            headerCell.Style.Border.TopBorderColor = XLColor.FromArgb(0x777777);
            headerCell.Style.Border.LeftBorderColor = XLColor.FromArgb(0x777777);
            headerCell.Style.Border.RightBorderColor = XLColor.FromArgb(0x777777);

            if (property.EhRequerido)
                headerCell.Style.Font.Bold = true;
        }

        private static void Comment(DnPropriedadeJsonAtributo property, IXLCell headerCell)
        {
            var isKeyComment = property.EhChave || property.EhDnChaveUnica ? $"\nIs Key" : "";
            var isPkComment = property.EhChaveExterna ? $"\nIs Fk" : "";
            var isRequiredComment = property.EhRequerido ? $"\nIs EhRequerido" : "";
            var isListComment = property.EhLista ? $"\nIs list" : "";
            var isEnumComment = property.EhEnumerador ? $"\nIs enum" : "";
            var enumValues = property.EhEnumerador ? "\nValues: " + string.Join(", ", property.Enumeradores.Select(x => $"{x.Key} = {x.Value}")) : "";
            var comment = property.Minimo == 0 && property.Maximo == 0 ? "" : $"{property.Nome}\nMin: {property.Minimo}, TamanhoMaximo: {property.Maximo}{isKeyComment}{isPkComment}{isRequiredComment}{isListComment}{isEnumComment}{enumValues}";
            headerCell.Comment.AddText(comment);
        }
    }
}
