using DocumentFormat.OpenXml.Packaging;
using DocumentFormat.OpenXml.Spreadsheet;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using VFi.Application.SO.DTOs;
using VFi.Domain.SO.Interfaces;
using VFi.Domain.SO.Models;
using VFi.NetDevPack.FopExpression;
using VFi.NetDevPack.Queries;

namespace VFi.Application.SO.Queries;

public class ExcelQueryGetInfoSheet : IQuery<List<ListboxIdStringDto>>
{
    public ExcelQueryGetInfoSheet(IFormFile file)
    {
        File = file;
    }
    public IFormFile File { get; set; }
}

public class ExcelQueryGetInfoColumn : IQuery<List<ListboxIdIntDto>>
{
    public ExcelQueryGetInfoColumn(IFormFile file, string sheetId, int headerRow)
    {
        File = file;
        HeaderRow = headerRow;
        SheetId = sheetId;
    }
    public IFormFile File { get; set; }
    public string SheetId { get; set; }
    public int HeaderRow { get; set; }
}

public class ExcelQueryHandler : IQueryHandler<ExcelQueryGetInfoSheet, List<ListboxIdStringDto>>,
                                 IQueryHandler<ExcelQueryGetInfoColumn, List<ListboxIdIntDto>>
{
    public ExcelQueryHandler()
    {
    }

    public async Task<List<ListboxIdStringDto>> Handle(ExcelQueryGetInfoSheet request, CancellationToken cancellationToken)
    {
        List<ListboxIdStringDto> result = new List<ListboxIdStringDto>();
        try
        {
            using (MemoryStream stream = new MemoryStream())
            {
                await request.File.CopyToAsync(stream);
                using (SpreadsheetDocument doc = SpreadsheetDocument.Open(stream, false))
                {
                    WorkbookPart workbookPart = doc.WorkbookPart;
                    Sheets thesheetcollection = workbookPart.Workbook.GetFirstChild<Sheets>();
                    List<string> excelResult = new List<string>();
                    foreach (Sheet thesheet in thesheetcollection)
                    {
                        if (thesheet is not null)
                        {
                            result.Add(new ListboxIdStringDto()
                            {
                                Label = thesheet.Name?.Value ?? "",
                                Key = thesheet.Id,
                                Value = thesheet.Id?.Value ?? "",
                            });
                        }
                    }
                };
            };
        }
        catch
        {
            return new List<ListboxIdStringDto>();
        }
        return result;
    }

    public async Task<List<ListboxIdIntDto>> Handle(ExcelQueryGetInfoColumn request, CancellationToken cancellationToken)
    {
        List<ListboxIdIntDto> result = new List<ListboxIdIntDto>();
        try
        {
            using (MemoryStream stream = new MemoryStream())
            {
                await request.File.CopyToAsync(stream);
                using (SpreadsheetDocument doc = SpreadsheetDocument.Open(stream, false))
                {
                    WorkbookPart workbookPart = doc.WorkbookPart;
                    Sheets thesheetcollection = workbookPart.Workbook.GetFirstChild<Sheets>();
                    //statement to get the worksheet object by using the sheet id  
                    Worksheet theWorksheet = ((WorksheetPart)workbookPart.GetPartById(request.SheetId)).Worksheet;
                    SheetData thesheetdata = (SheetData)theWorksheet.GetFirstChild<SheetData>();

                    foreach (Row thecurrentrRow in thesheetdata)
                    {
                        if (thecurrentrRow.RowIndex == request.HeaderRow)
                        {
                            foreach (Cell thecurrentCell in thecurrentrRow)
                            {
                                if (thecurrentCell.DataType != null)
                                {
                                    if (thecurrentCell.DataType == CellValues.SharedString)
                                    {
                                        int id;
                                        if (Int32.TryParse(thecurrentCell.InnerText, out id))
                                        {
                                            SharedStringItem item = workbookPart.SharedStringTablePart.SharedStringTable.Elements<SharedStringItem>().ElementAt(id);
                                            if (item.Text != null)
                                            {
                                                result.Add(new ListboxIdIntDto()
                                                {
                                                    Label = item.Text.Text,
                                                    Key = item.Text.Text,
                                                    Value = result.Count(),
                                                });
                                            }
                                        }
                                    }
                                }
                            }

                            break;
                        }
                    }
                };
            };
        }
        catch
        {
            return new List<ListboxIdIntDto>();
        }
        return result;
    }
}
