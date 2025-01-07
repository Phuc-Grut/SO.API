

namespace VFi.Application.SO.DTOs;

public class ComboBoxDto
{
    public Guid Value { get; set; }
    public string? Label { get; set; }
    public string? Key { get; set; }
}
public class ListboxIdStringDto
{
    public string Value { get; set; } = null!;
    public string Label { get; set; } = null!;
    public string? Key { get; set; }
}
public class ListboxIdIntDto
{
    public int Value { get; set; }
    public string Label { get; set; } = null!;
    public string? Key { get; set; }
}

public class ShippingCarrierComboBoxDto
{
    public Guid Id { get; set; }
    public string? Code { get; set; }
    public string? Name { get; set; }
    public string? Description { get; set; }
}