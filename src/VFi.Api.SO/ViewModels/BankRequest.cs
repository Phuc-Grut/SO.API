using Microsoft.AspNetCore.Mvc;
using VFi.Domain.SO.Models;

namespace VFi.Api.SO.ViewModels;

public class BankRequest : FopPagingRequest
{
}

public class AddBankRequest
{
    public string Code { get; set; }
    public string? Qrbin { get; set; }
    public string? ShortName { get; set; }
    public string? Name { get; set; }
    public string? EnglishName { get; set; }
    public string? Address { get; set; }
    public int DisplayOrder { get; set; }
    public int Status { get; set; }
    public string? Note { get; set; }
    public string? Image { get; set; }

}
public class EditBankRequest : AddBankRequest
{
    public string Id { get; set; }
}
