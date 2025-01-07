namespace VFi.Application.SO.DTOs;

public class ExcelFieldToExportParams
{
    public string HeaderText { get; set; } = null!;
    public string? Field { get; set; }
    public int? Width { get; set; }
    public int? ValueType { get; set; }
    public List<ExcelFieldToExportParams> Columns { get; set; } = new List<ExcelFieldToExportParams>();

}
public class CellReferenceDto
{
    public string ColumnCharacter { get; set; } = null!;
    public int RowIndex { get; set; }
    public int? StyleIndex { get; set; }
    public string Name { get; set; } = null!;

}

public class CellRefDto
{
    public string ColumnCharacter { get; set; }
    public UInt32 RowIndex { get; set; }
    public UInt32 StyleIndex { get; set; }
    public string Name { get; set; }
    public string Text { get; set; }
    public string? TextCopy { get; set; }
}

public class HeaderColumnDto
{
    public string Text { get; set; }
    public string Field { get; set; }
}
public class ExcelValidateField
{
    public string? Field { get; set; } = null!;
    public int? IndexColumn { get; set; }
}
