using VFi.Domain.SO.Models;

namespace VFi.Api.SO.ViewModels;

public class CellReference
{
    public string? ColumnCharacter { get; set; }
    public UInt32 RowIndex { get; set; }
    public UInt32 StyleIndex { get; set; }
    public string? Name { get; set; }
    public string? Text { get; set; }
    public string? TextCopy { get; set; }
}
