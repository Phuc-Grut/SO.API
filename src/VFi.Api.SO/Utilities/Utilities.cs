using System.ComponentModel;
using System.Data;
using System.Reflection;
using System.Text.RegularExpressions;
using DocumentFormat.OpenXml;
using DocumentFormat.OpenXml.ExtendedProperties;
using DocumentFormat.OpenXml.Math;
using DocumentFormat.OpenXml.Packaging;
using DocumentFormat.OpenXml.Spreadsheet;
using DocumentFormat.OpenXml.Wordprocessing;
using Syncfusion.EJ2.Linq;
using VFi.Api.SO.ViewModels;
using VFi.Api.SO.ViewModels;

namespace VFi.Api.SO.Utilities;

public class Utilities
{
    public static List<CellReference> FindCellExistText(string file)
    {
        using (SpreadsheetDocument spreadSheetTemplate = SpreadsheetDocument.Open(file, true))
        {

            //UInt32[] styles = new UInt32[] { 12 };
            WorkbookPart workbookPartTemplate = spreadSheetTemplate.WorkbookPart;
            SharedStringTablePart sstpart = workbookPartTemplate.GetPartsOfType<SharedStringTablePart>().First();
            SharedStringTable sst = sstpart.SharedStringTable;

            WorksheetPart worksheetPartTemplate = workbookPartTemplate.WorksheetParts.First();
            Worksheet sheetTemplate = worksheetPartTemplate.Worksheet;

            var cellss = sheetTemplate.Descendants<Cell>()
                .Where(x =>
                    //styles.Contains(x.StyleIndex) ||
                    x.DataType != null
                    && x.DataType == CellValues.SharedString
                    && x.CellValue != null
                    && sst.ChildElements[int.Parse(x.CellValue?.Text)]?.InnerText != null
                    && sst.ChildElements[int.Parse(x.CellValue?.Text)]?.InnerText != ""
                ).ToList();
            var cells = cellss.Select(x =>
            {
                var columnCharacter = x.CellReference.ToString().Substring(0, 1);
                var rowIndex = uint.Parse((string)x.CellReference.ToString().Substring(1));
                var Text = x.CellValue == null ? "" : sst.ChildElements[int.Parse(x.CellValue.Text)].InnerText;
                return new CellReference
                {
                    ColumnCharacter = x.CellReference.ToString().Substring(0, 1),
                    RowIndex = uint.Parse((string)x.CellReference.ToString().Substring(1)),
                    Text = x.CellValue == null ? "" : sst.ChildElements[int.Parse(x.CellValue.Text)].InnerText,
                    StyleIndex = x.StyleIndex ?? 0
                };
            }).ToList();
            return cells == null ? null : cells;
        }
    }
    public static List<CellReference> FindCellByText(string file, string text)
    {
        using (SpreadsheetDocument spreadSheetTemplate = SpreadsheetDocument.Open(file, true))
        {
            WorkbookPart workbookPartTemplate = spreadSheetTemplate.WorkbookPart;
            SharedStringTablePart sstpart = workbookPartTemplate.GetPartsOfType<SharedStringTablePart>().First();
            SharedStringTable sst = sstpart.SharedStringTable;

            WorksheetPart worksheetPartTemplate = workbookPartTemplate.WorksheetParts.First();
            Worksheet sheetTemplate = worksheetPartTemplate.Worksheet;

            var cells = sheetTemplate.Descendants<Cell>()
                .Where(x =>
                x.DataType != null
                && x.DataType == CellValues.SharedString
                && sst.ChildElements[int.Parse(x.CellValue.Text)].InnerText.Contains("^")
                && text.Contains(sst.ChildElements[int.Parse(x.CellValue.Text)].InnerText.Split("^")[0]))
                .Select(x =>
                {
                    return new CellReference
                    {
                        ColumnCharacter = x.CellReference.ToString().Substring(0, 1),
                        RowIndex = uint.Parse((string)x.CellReference.ToString().Substring(1)),
                        Name = sst.ChildElements[int.Parse(x.CellValue.Text)].InnerText.Split("^")[0],
                        Text = sst.ChildElements[int.Parse(x.CellValue.Text)].InnerText.Split("^")[1] ?? "",
                        StyleIndex = x.StyleIndex ?? 0
                    };
                }).ToList();

            return cells == null ? null : cells;
        }
    }
    public static List<CellReference> FindCellByReference(string file, string text, uint rowFrom)
    {
        using (SpreadsheetDocument spreadSheetTemplate = SpreadsheetDocument.Open(file, true))
        {

            WorkbookPart workbookPartTemplate = spreadSheetTemplate.WorkbookPart;
            SharedStringTablePart sstpart = workbookPartTemplate.GetPartsOfType<SharedStringTablePart>().First();
            SharedStringTable sst = sstpart.SharedStringTable;

            WorksheetPart worksheetPartTemplate = workbookPartTemplate.WorksheetParts.First();
            Worksheet sheetTemplate = worksheetPartTemplate.Worksheet;

            var cells = sheetTemplate.Descendants<Cell>()
                .Where(x => text.Contains(x.CellReference.Value) && uint.Parse((string)x.CellReference.ToString().Substring(1)) >= rowFrom && x.DataType != null && x.DataType == CellValues.SharedString)
                .Select(x => new CellReference
                {
                    ColumnCharacter = x.CellReference.ToString().Substring(0, 1),
                    RowIndex = uint.Parse((string)x.CellReference.ToString().Substring(1)),
                    Name = sst.ChildElements[int.Parse(x.CellValue.Text)].InnerText != null ? (sst.ChildElements[int.Parse(x.CellValue.Text)].InnerText.Contains("^") ? sst.ChildElements[int.Parse(x.CellValue.Text)].InnerText.Split("^")[0] : "") : "",
                    Text = sst.ChildElements[int.Parse(x.CellValue.Text)].InnerText != null ? (sst.ChildElements[int.Parse(x.CellValue.Text)].InnerText.Contains("^") ? sst.ChildElements[int.Parse(x.CellValue.Text)].InnerText.Split("^")[1] : sst.ChildElements[int.Parse(x.CellValue.Text)].InnerText) : "",
                    StyleIndex = x.StyleIndex
                })
                .ToList();
            return cells == null ? null : cells;
        }
    }

    public static CellReference FindOneCellByReference(string file, string text, bool? getText = false)
    {
        using (SpreadsheetDocument spreadSheetTemplate = SpreadsheetDocument.Open(file, true))
        {

            WorkbookPart workbookPartTemplate = spreadSheetTemplate.WorkbookPart;
            SharedStringTablePart sstpart = workbookPartTemplate.GetPartsOfType<SharedStringTablePart>().First();
            SharedStringTable sst = sstpart.SharedStringTable;

            WorksheetPart worksheetPartTemplate = workbookPartTemplate.WorksheetParts.First();
            Worksheet sheetTemplate = worksheetPartTemplate.Worksheet;

            var cells = sheetTemplate.Descendants<Cell>()
                .Where(x => x.CellReference == text)
                .Select(x => new CellReference
                {
                    ColumnCharacter = x.CellReference.ToString().Substring(0, 1),
                    RowIndex = uint.Parse((string)x.CellReference.ToString().Substring(1)),
                    Name = "",
                    Text = getText == true ? (x.CellValue != null ? sst.ChildElements[int.Parse(x.CellValue.Text)].InnerText != null ? sst.ChildElements[int.Parse(x.CellValue.Text)].InnerText : "" : "") : "",
                    StyleIndex = x.StyleIndex
                })
                .First();
            return cells == null ? null : cells;
        }
    }

    public static int InsertSharedStringItem(string text, SharedStringTablePart shareStringPart)
    {
        // If the part does not contain a SharedStringTable, create one.
        if (shareStringPart.SharedStringTable == null)
        {
            shareStringPart.SharedStringTable = new SharedStringTable();
        }

        int i = 0;

        foreach (SharedStringItem item in shareStringPart.SharedStringTable.Elements<SharedStringItem>())
        {
            if (item.InnerText == text)
            {
                return i;
            }

            i++;
        }

        shareStringPart.SharedStringTable.AppendChild(new SharedStringItem(new DocumentFormat.OpenXml.Spreadsheet.Text(text)));
        shareStringPart.SharedStringTable.Save();

        return i;
    }
    public static Cell InsertCell(string columnName, uint rowIndex, Row row)
    {
        string cellReference = columnName + rowIndex;
        if (row.Elements<Cell>().Where(c => c.CellReference.Value == columnName + rowIndex).Count() > 0)
        {
            return row.Elements<Cell>().Where(c => c.CellReference.Value == cellReference).First();
        }
        else
        {
            Cell refCell = null;
            foreach (Cell cell in row.Elements<Cell>())
            {
                if (string.Compare(cell.CellReference.Value, cellReference, true) > 0)
                {
                    refCell = cell;
                    break;
                }
            }

            Cell newCell = new Cell() { CellReference = cellReference };
            row.InsertBefore(newCell, refCell);
            return newCell;
        }
    }

    public static Cell InsertCellInWorksheet(string columnName, uint rowIndex, WorksheetPart worksheetPart)
    {
        Worksheet worksheet = worksheetPart.Worksheet;
        SheetData sheetData = worksheet.GetFirstChild<SheetData>();
        string cellReference = columnName + rowIndex;

        Row row;
        if (sheetData.Elements<Row>().Where(r => r.RowIndex == rowIndex).Count() != 0)
        {
            row = sheetData.Elements<Row>().Where(r => r.RowIndex == rowIndex).First();
        }
        else
        {
            row = new Row() { RowIndex = rowIndex };
            sheetData.Append(row);
        }

        if (row.Elements<Cell>().Where(c => c.CellReference.Value == columnName + rowIndex).Count() > 0)
        {
            return row.Elements<Cell>().Where(c => c.CellReference.Value == cellReference).First();
        }
        else
        {
            Cell refCell = null;
            foreach (Cell cell in row.Elements<Cell>())
            {
                if (string.Compare(cell.CellReference.Value, cellReference, true) > 0)
                {
                    refCell = cell;
                    break;
                }
            }

            Cell newCell = new Cell() { CellReference = cellReference };
            row.InsertBefore(newCell, refCell);
            return newCell;
        }
    }
    public static void SetDataCell(CellReference c, string text, SharedStringTablePart shareStringPart, WorksheetPart worksheetPart)
    {
        if (c != null)
        {
            int index = InsertSharedStringItem(c.Text == "" ? text : c.Text + " " + text, shareStringPart);
            Cell cell = InsertCellInWorksheet(c.ColumnCharacter, c.RowIndex, worksheetPart);
            cell.CellValue = new CellValue(index.ToString());
            cell.DataType = new EnumValue<CellValues>(CellValues.SharedString);
            cell.StyleIndex = c.StyleIndex;
        }
    }
    public static void SetDataCell(CellReference c, string text, uint rowindex, SharedStringTablePart shareStringPart, WorksheetPart worksheetPart)
    {
        if (c != null)
        {
            int index = InsertSharedStringItem(c.Text == "" ? text : c.Text + " " + text, shareStringPart);
            Cell cell = InsertCellInWorksheet(c.ColumnCharacter, rowindex, worksheetPart);
            cell.CellValue = new CellValue(index.ToString());
            cell.DataType = new EnumValue<CellValues>(CellValues.SharedString);
            cell.StyleIndex = c.StyleIndex;
        }
    }
    public static void SetDataCell(string text, string character, uint rowindex, UInt32Value style, SharedStringTablePart shareStringPart, WorksheetPart worksheetPart)
    {
        int index = InsertSharedStringItem(text, shareStringPart);
        Cell cell = InsertCellInWorksheet(character, rowindex, worksheetPart);
        cell.CellValue = new CellValue(index.ToString());
        cell.DataType = new EnumValue<CellValues>(CellValues.SharedString);
        cell.StyleIndex = style;
    }
    public static void SetDataCellGeneral<T>(CellReference c, T text, uint rowindex, SharedStringTablePart shareStringPart, WorksheetPart worksheetPart)
    {
        if (c != null)
        {
            Cell cell = InsertCellInWorksheet(c.ColumnCharacter, rowindex, worksheetPart);
            int index = InsertSharedStringItem(c.Text == "" ? (text != null ? text.ToString() : "") : c.Text + " " + text, shareStringPart);

            if (text is int || text is double || text is float || text is decimal)
            {
                cell.CellValue = new CellValue(text.ToString());
                cell.DataType = new EnumValue<CellValues>(CellValues.Number);
            }
            else
            {
                cell.CellValue = new CellValue(index.ToString());
                cell.DataType = new EnumValue<CellValues>(CellValues.SharedString);
            }
            cell.StyleIndex = c.StyleIndex;
        }
    }
    public static Row InsertRow(SheetData sheetData, uint rowIndex)
    {
        Row row;
        if (sheetData.Elements<Row>().Where(r => r.RowIndex == rowIndex).Count() != 0)
        {
            row = sheetData.Elements<Row>().Where(r => r.RowIndex == rowIndex).First();
        }
        else
        {
            row = new Row() { RowIndex = rowIndex };
            sheetData.Append(row);
        }
        return row;
    }
    public static void MergeCellInRange(Worksheet worksheet, string range, string? characterLeft, string? numberLeft, string? characterRight, string? numberRight)
    {
        string[] CellReferenceArray = new string[] { "A", "B", "C", "D", "E", "F", "G", "H", "I", "J", "K", "L", "M", "N", "O", "P", "Q", "R", "S", "T", "U", "V", "W", "X", "Y", "Z",
                "AA", "AB", "AC", "AD", "AE", "AF", "AG", "AH", "AI", "AJ", "AK", "AL", "AM", "AN", "AO", "AP", "AQ", "AR", "AS", "AT", "AU", "AV", "AW", "AX", "AY", "AZ"};

        if (characterLeft != characterRight && numberLeft != numberRight)
        {

        }
        if (characterLeft != characterRight && numberLeft == numberRight)
        {
            for (var x = CellReferenceArray.IndexOf(characterLeft); x <= CellReferenceArray.IndexOf(characterRight); x++)
            {
                CreateSpreadsheetCellIfNotExist(worksheet, CellReferenceArray[x] + numberLeft);
            }
        }
        if (characterLeft == characterRight && numberLeft != numberRight)
        {

        }

        MergeCells mergeCells;
        if (worksheet.Elements<MergeCells>().Count() > 0)
        {
            mergeCells = worksheet.Elements<MergeCells>().First();
        }
        else
        {
            mergeCells = new MergeCells();
            worksheet.InsertAfter(mergeCells, worksheet.Elements<SheetData>().First());
        }
        MergeCell mergeCell = new MergeCell() { Reference = new StringValue(range) };

        mergeCells.Append(mergeCell);
        worksheet.Save();
        //Row r = sheetData.Elements<Row>().Where(a => a.RowIndex == rowIndex).First();
        //Cell c1 = r.Descendants<Cell>().Where(a => a.CellReference == range.Split(":")[0]).First();
        //c1.CellValue = new CellValue(text);
        //c1.DataType = new EnumValue<CellValues>(CellValues.String);
    }

    private static uint GetRowIndex(string cellName)
    {
        Regex regex = new Regex(@"\d+");
        Match match = regex.Match(cellName);

        return uint.Parse(match.Value);
    }

    private static string GetColumnName(string cellName)
    {
        Regex regex = new Regex("[A-Za-z]+");
        Match match = regex.Match(cellName);

        return match.Value;
    }

    private static void CreateSpreadsheetCellIfNotExist(Worksheet worksheet, string cellName)
    {
        string columnName = GetColumnName(cellName);
        uint rowIndex = GetRowIndex(cellName);

        IEnumerable<Row> rows = worksheet.Descendants<Row>().Where(r => r.RowIndex.Value == rowIndex);

        if (rows.Count() == 0)
        {
            Row row = new Row() { RowIndex = new UInt32Value(rowIndex) };
            Cell cell = new Cell() { CellReference = new StringValue(cellName) };
            row.Append(cell);
            worksheet.Descendants<SheetData>().First().Append(row);
            worksheet.Save();
        }
        else
        {
            Row row = rows.First();

            IEnumerable<Cell> cells = row.Elements<Cell>().Where(c => c.CellReference.Value == cellName);

            if (cells.Count() == 0)
            {
                Cell cell = new Cell() { CellReference = new StringValue(cellName) };
                row.Append(cell);
                worksheet.Save();
            }
        }
    }
}
public class Utilities<T>
{
    public static string GetValueByKey(T data, string expression)
    {
        if (expression == null || expression == "")
        {
            return null;
        }
        var upper = char.ToUpper(expression[0]) + expression.Substring(1);
        Type objectType = data.GetType();
        PropertyInfo property = objectType.GetProperty(upper);

        if (property != null)
        {
            string value = property.GetValue(data)?.ToString() ?? "";
            return value;
        }
        return null;

    }
    public static object GetValueByKeyGeneral(object data, string expression)
    {
        if (expression == null || expression == "")
        {
            return null;
        }
        Type objectType = data.GetType();
        var upper = char.ToUpper(expression[0]) + expression.Substring(1);
        PropertyInfo property = objectType.GetProperty(upper);

        if (property != null)
        {
            return property.GetValue(data);
        }
        return null;
    }
}
