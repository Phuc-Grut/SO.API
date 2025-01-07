using VFi.Application.SO.DTOs;
using VFi.Domain.SO.Models;

namespace VFi.Api.SO.ViewModels;

public class ExcelLoadInfoSheetRequest
{
    public IFormFile File { get; set; } = null!;
}
public class ExcelLoadInfoColumnRequest
{
    public string SheetId { get; set; } = null!;
    public int HeaderRow { get; set; }
    public IFormFile File { get; set; } = null!;
}

public class ExcelExportRequest
{
    public Guid Id { get; set; }
    public string? Keyword { get; set; }
    public int CountRowHeader { get; set; }
    public List<ExcelFieldToExportParams> Columns { get; set; } = new List<ExcelFieldToExportParams>();
}
