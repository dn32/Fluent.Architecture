using ClosedXML.Excel;
using Fluent.Architecture.Core.Attributes;
using Fluent.Architecture.Extensions;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;

namespace Fluent.Architecture.Core.Util
{
    public static class DataImportationUtil
    {
        public static XLWorkbook ImportationTemplateXLSX<T>(int addExample)
        {
            var schema = typeof(T).GetFluentJsonSchema(false);
            var table = schema.FluentJsonForm.propName;
            var properties = schema.Properties.Where(x => x.FluentComposition == null && x.FluentAggregation == null).ToList();

            var workbook = new XLWorkbook();
            var worksheet = workbook.Worksheets.Add(table);
            var propertiesExample = typeof(T).GetProperties();
            int i;

            for (i = 0; i < properties.Count; i++)
            {
                var property = properties[i];
                var headerCell = worksheet.Column(i + 1).Cell(1);
                headerCell.Value = property.propName;

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

                var schema = typeof(T).GetFluentJsonSchema(false);
                var table = schema.FluentJsonForm.propName;
                var properties = schema.Properties.Where(x => x.FluentComposition == null && x.FluentAggregation == null).ToList();

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
                        var exampleProperty = propertiesExample.FirstOrDefault(x => x.Name == property.PropNameCaseSensitive);
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
                        property.SetValue(entidade, cell.Value?.ToString() ?? property.PropertyType.GetFluentDefaultValue());
                    }
                }
            }

            return null;
        }

        private static void Example<T>(int addExample, IXLWorksheet worksheet, PropertyInfo[] propertiesExample, int i, FluentJsonPropertyAttribute property)
        {
            for (var x = 0; x < addExample; x++)
            {
                var example = typeof(T).GetExampleValue();
                var exampleCell = worksheet.Column(i + 1).Cell(2 + x);
                var exampleProperty = propertiesExample.FirstOrDefault(x => x.Name == property.PropNameCaseSensitive);
                exampleCell.Value = exampleProperty?.GetValue(example) ?? "";
            }
        }

        private static void Style(FluentJsonPropertyAttribute property, IXLCell headerCell)
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

            if (property.required)
                headerCell.Style.Font.Bold = true;
        }

        private static void Comment(FluentJsonPropertyAttribute property, IXLCell headerCell)
        {
            var isKeyComment = property.IsKey || property.IsFluentUniqueKeyKey ? $"\nIs Key" : "";
            var isPkComment = property.IsFk ? $"\nIs Fk" : "";
            var isRequiredComment = property.required ? $"\nIs required" : "";
            var isListComment = property.IsList ? $"\nIs list" : "";
            var isEnumComment = property.IsEnum ? $"\nIs enum" : "";
            var enumValues = property.IsEnum ? "\nValues: " + string.Join(", ", property.Enums.Select(x => $"{x.Key} = {x.Value}")) : "";
            var comment = property.min == 0 && property.max == 0 ? "" : $"{property.name}\nMin: {property.min}, Max: {property.max}{isKeyComment}{isPkComment}{isRequiredComment}{isListComment}{isEnumComment}{enumValues}";
            headerCell.Comment.AddText(comment);
        }
    }
}
