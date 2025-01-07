namespace VFi.Application.SO.DTOs;

public class FileDto
{
    public Guid? Id { get; set; }
    public string? Name { get; set; }
    public string? Path { get; set; }
    public decimal? Size { get; set; }
    public bool? Status { get; set; }
    public string? Title { get; set; }
    public string? Type { get; set; }
    public string? VirtualPath { get; set; }
}
