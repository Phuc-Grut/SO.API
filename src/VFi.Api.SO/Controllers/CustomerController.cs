using System.ComponentModel.DataAnnotations;
using DocumentFormat.OpenXml.Packaging;
using DocumentFormat.OpenXml.Spreadsheet;
using Microsoft.AspNetCore.Mvc;
using Syncfusion.EJ2.Linq;
using VFi.Api.SO.ViewModels;
using VFi.Application.SO.Commands;
using VFi.Application.SO.DTOs;
using VFi.Application.SO.Queries;
using VFi.Domain.SO.Models;
using VFi.NetDevPack.Configuration;
using VFi.NetDevPack.Context;
using VFi.NetDevPack.Mediator;

namespace VFi.Api.SO.Controllers;

[Route("api/[controller]")]
[ApiController]
public partial class CustomerController : ControllerBase
{
    private readonly IContextUser _context;
    private readonly IMediatorHandler _mediator;
    private readonly ILogger<CustomerController> _logger;
    private readonly CodeSyntaxConfig _codeSyntax;
    private readonly IWebHostEnvironment _environment;

    public CustomerController(IMediatorHandler mediator, IContextUser context, ILogger<CustomerController> logger, CodeSyntaxConfig codeSyntax,
        IWebHostEnvironment environment)
    {
        _mediator = mediator;
        _context = context;
        _environment = environment;
        _logger = logger;
        _codeSyntax = codeSyntax;
    }

    [HttpGet("checkemail/{email}")]
    public async Task<IActionResult> CheckEmail(string email)
    {
        var result = await _mediator.Send(new AccountEmailCheckExist(email));
        return Ok(result);
    }

    [HttpGet("get-by-id/{id}")]
    public async Task<IActionResult> Get(Guid id)
    {
        var rs = await _mediator.Send(new CustomerQueryById(id));
        return Ok(rs);
    }

    [HttpGet("get-list-cbx")]
    public async Task<IActionResult> Get([FromQuery] ComboBoxRequest request)
    {
        var result = await _mediator.Send(new CustomerQueryComboBox(request.Status, request.KeyWord));
        return Ok(result);
    }

    [HttpGet("paging")]
    public async Task<IActionResult> Paging([FromQuery] CustomerPagingRequest request)
    {
        if (_context.QueryMyData())
        {
            request.EmployeeId = _context.GetUserId().ToString();
        }
        var query = new CustomerPagingQuery(
              request.Keyword ?? "",
              request.ToBaseQuery(),
              request.Filter ?? "",
              request.Order ?? "",
              request.PageNumber,
              request.PageSize
          );
        var result = await _mediator.Send(query);
        return Ok(result);
    }

    [HttpPost("add")]
    public async Task<IActionResult> Add([FromBody] AddCustomerRequest request)
    {
        int UsedStatus = 1;
        var Code = request.Code;
        if (request.IsAuto == true)
        {
            Code = await _mediator.Send(new GetCodeQuery(request.ModuleCode, UsedStatus));
        }
        else
        {
            var useCodeCommand = new UseCodeCommand(
                request.ModuleCode,
                Code
                );
            _mediator.SendCommand(useCodeCommand);
        }
        var Id = Guid.NewGuid();

        var addAddress = request.ListAddress?.Select(x => new CustomerAddressDto()
        {
            CustomerId = Id,
            Name = x.Name,
            Country = x.Country,
            Province = x.Province,
            District = x.District,
            Ward = x.Ward,
            Address = x.Address,
            Phone = x.Phone,
            Email = x.Email,
            ShippingDefault = x.ShippingDefault,
            BillingDefault = x.BillingDefault,
            Note = x.Note,
            Status = x.Status,
            SortOrder = x.SortOrder
        }).ToList();

        var addContact = request.ListContact?.Select(x => new CustomerContactDto()
        {
            CustomerId = Id,
            Name = x.Name,
            Gender = x.Gender,
            Phone = x.Phone,
            Email = x.Email,
            Facebook = x.Facebook,
            JobTitle = x.JobTitle,
            Tags = x.Tags,
            Address = x.Address,
            Status = x.Status,
            SortOrder = x.SortOrder
        }).ToList();

        var addBank = request.ListBank?.Select(x => new CustomerBankDto()
        {
            CustomerId = Id,
            Name = x.Name,
            BankCode = x.BankCode,
            BankName = x.BankName,
            BankBranch = x.BankBranch,
            AccountName = x.AccountName,
            AccountNumber = x.AccountNumber,
            Default = x.Default,
            Status = x.Status,
            SortOrder = x.SortOrder
        }).ToList();

        var CustomerAddCommand = new CustomerAddCommand(
          Id,
          request.CustomerSourceId,
          request.Image,
          request.Type,
          Code,
          request.Alias,
          request.Name,
          request.Phone,
          request.Email,
          request.Fax,
          request.Country,
          request.Province,
          request.District,
          request.Ward,
          request.ZipCode,
          request.Address,
          request.Website,
          request.TaxCode,
          request.BusinessSector,
          request.CompanyName,
          request.CompanyPhone,
          request.CompanySize,
          request.Capital,
          request.EstablishedDate,
          request.Tags,
          request.Note,
          request.Status,
          request.EmployeeId,
          request.GroupEmployeeId,
          request.IsVendor,
          request.IsAuto,
          request.Gender,
          request.Year,
          request.Month,
          request.Day,
          request.CustomerGroup,
          request.Representative,
          request.Revenue,
          request.IdName,
          request.IdNumber,
          request.IdDate,
          request.IdIssuer,
          request.IdImage1,
          request.IdImage2,
          request.IdStatus,
          request.CccdNumber,
          request.DateRange,
          request.Birthday,
          request.IssuedBy,
          request.LeadId,
          addAddress,
          addContact,
          addBank);
        var result = await _mediator.SendCommand(CustomerAddCommand);

        return Ok(result);
    }

    [HttpPut("edit")]
    public async Task<IActionResult> Put([FromBody] EditCustomerRequest request)
    {
        var CustomerId = new Guid(request.Id);

        var customerAddress = request.ListAddress?.Select(x => new CustomerAddressDto()
        {
            Id = !string.IsNullOrEmpty(x.Id) ? new Guid(x.Id) : null,
            CustomerId = new Guid(request.Id),
            Name = x.Name,
            Country = x.Country,
            Province = x.Province,
            District = x.District,
            Ward = x.Ward,
            Address = x.Address,
            Phone = x.Phone,
            Email = x.Email,
            ShippingDefault = x.ShippingDefault,
            BillingDefault = x.BillingDefault,
            Note = x.Note,
            Status = x.Status,
            SortOrder = x.SortOrder
        }).ToList();

        var customerContact = request.ListContact?.Select(x => new CustomerContactDto()
        {
            Id = !string.IsNullOrEmpty(x.Id) ? new Guid(x.Id) : null,
            CustomerId = new Guid(request.Id),
            Name = x.Name,
            Gender = x.Gender,
            Phone = x.Phone,
            Email = x.Email,
            Facebook = x.Facebook,
            JobTitle = x.JobTitle,
            Tags = x.Tags,
            Address = x.Address,
            Status = x.Status,
            SortOrder = x.SortOrder
        }).ToList();

        var customerBank = request.ListBank?.Select(x => new CustomerBankDto()
        {
            Id = x.Id,
            CustomerId = new Guid(request.Id),
            Name = x.Name,
            BankCode = x.BankCode,
            BankName = x.BankName,
            BankBranch = x.BankBranch,
            AccountName = x.AccountName,
            AccountNumber = x.AccountNumber,
            Default = x.Default,
            Status = x.Status,
            SortOrder = x.SortOrder
        }).ToList();

        var CustomerEditCommand = new CustomerEditCommand(
           CustomerId,
           request.CustomerSourceId,
           request.Image,
           request.Type,
           request.Code,
           request.Alias,
           request.Name,
           request.Phone,
           request.Email,
           request.Fax,
           request.Country,
           request.Province,
           request.District,
           request.Ward,
           request.ZipCode,
           request.Address,
           request.Website,
           request.TaxCode,
           request.BusinessSector,
           request.CompanyName,
           request.CompanyPhone,
           request.CompanySize,
           request.Capital,
           request.EstablishedDate,
           request.Tags,
           request.Note,
           request.Status,
           request.EmployeeId,
           request.GroupEmployeeId,
           request.IsVendor,
           request.IsAuto,
           request.Gender,
           request.Year,
           request.Month,
           request.Day,
           request.CurrencyId,
           request.Currency,
           request.CurrencyName,
           request.PriceListId,
           request.PriceListName,
           request.DebtLimit,
           request.CustomerGroup,
           request.Representative,
           request.Revenue,
           request.IdName,
           request.IdNumber,
           request.IdDate,
           request.IdIssuer,
           request.IdImage1,
           request.IdImage2,
           request.IdStatus,
           request.CccdNumber,
           request.DateRange,
           request.Birthday,
           request.IssuedBy,
           request.AccountId,
           request.AccountEmail,
           request.AccountEmailVerified,
           request.AccountUsername,
           request.AccountCreatedDate,
           request.AccountPhone,
           request.AccountPhoneVerified,
           customerAddress,
           customerContact,
           customerBank
       );

        var result = await _mediator.SendCommand(CustomerEditCommand);
        return Ok(result);
    }

    [HttpDelete("delete/{id}")]
    public async Task<IActionResult> Delete(string id)
    {
        var CustomerId = new Guid(id);
        if (await _mediator.Send(new CustomerQueryById(CustomerId)) == null)
            return BadRequest(new ValidationResult("Customer not exists"));

        var result = await _mediator.SendCommand(new CustomerDeleteCommand(CustomerId));

        return Ok(result);
    }

    [HttpPut("update-account")]
    public async Task<IActionResult> UpdateAccount([FromBody] UpdateAccountCustomerRequest request)
    {
        var command = new CustomerUpdateAccountCommand(
           request.Id,
           request.AccountId,
           request.AccountEmail,
           request.AccountEmailVerified,
           request.AccountUsername,
           request.AccountCreatedDate,
           request.AccountPhone,
           request.AccountPhoneVerified
       );

        var result = await _mediator.SendCommand(command);

        return Ok(result);
    }

    [HttpGet("get-by-code/{code}")]
    public async Task<IActionResult> GetByCode(string code)
    {
        var rs = await _mediator.Send(new CustomerQueryByCode(code));
        return Ok(rs);
    }

    [HttpPut("update-finance")]
    public async Task<IActionResult> UpdateFinance([FromBody] UpdateFinanceCustomerRequest request)
    {
        var command = new CustomerUpdateFinanceCommand(
           request.Id,
           request.CurrencyId,
           request.Currency,
           request.CurrencyName,
           request.PriceListId,
           request.PriceListName,
           request.DebtLimit
       );

        var result = await _mediator.SendCommand(command);
        return Ok(result);
    }

    [HttpGet("export-excel")]
    public async Task<IActionResult> ExportExcel([FromQuery] CustomerPagingRequest request)
    {
        try
        {
            //var items = await _mediator.Send(new CustomerExcelQueryAll());
            var query = new CustomerExcelQueryAll(
                  request.Keyword ?? "",
                  request.Type,
                  request.Status
              );

            var items = await _mediator.Send(query);
            string path = _environment.ContentRootPath;
            string file = path + "/Template/KHACHHANG.xlsx";
            string newfile = path + "/Print/" + "testt" + ".xlsx";

            string[] deleteFiles = Directory.GetFiles(path + "/Print/");
            foreach (string deleteFile in deleteFiles)
            {
                System.IO.File.Delete(deleteFile);
            }
            System.IO.File.Copy(file, newfile);

            string[] CellReferenceArray = new string[] { "A", "B", "C", "D", "E", "F", "G", "H", "I", "J", "K", "L", "M", "N", "O", "P", "Q", "R", "S", "T", "U", "V", "W", "X", "Y", "Z" };

            string CellTextTemplate = "BodyTable";

            using (SpreadsheetDocument spreadSheet = SpreadsheetDocument.Open(newfile, true))
            {
                SharedStringTablePart shareStringPart;
                if (spreadSheet.WorkbookPart.GetPartsOfType<SharedStringTablePart>().Count() > 0)
                {
                    shareStringPart = spreadSheet.WorkbookPart.GetPartsOfType<SharedStringTablePart>().First();
                }
                else
                {
                    shareStringPart = spreadSheet.WorkbookPart.AddNewPart<SharedStringTablePart>();
                }
                WorksheetPart worksheetPart = spreadSheet.WorkbookPart.WorksheetParts.First();
                Stylesheet stylesheet = spreadSheet.WorkbookPart.WorkbookStylesPart.Stylesheet;
                Worksheet worksheet = worksheetPart.Worksheet;

                SheetData sheetData = worksheet.GetFirstChild<SheetData>();
                List<CellReference> cellTemplate = Utilities.Utilities.FindCellByText(file, CellTextTemplate);
                List<CellReference> cellExistText = Utilities.Utilities.FindCellExistText(file);
                if (cellExistText.Count() > 0)
                {
                    var copycellExistText = cellExistText.Select(x => new CellReference()
                    {
                        RowIndex = x.RowIndex,
                        Text = x.Text,
                        ColumnCharacter = x.ColumnCharacter,
                        Name = x.Name,
                        StyleIndex = x.StyleIndex,
                        TextCopy = x.TextCopy
                    }).ToList();

                    foreach (var cell in cellExistText)
                    {
                        UInt32 rowindex = cell.RowIndex;
                        var c = cell;
                        c.Text = "";
                        Utilities.Utilities.SetDataCell(c, "", rowindex, shareStringPart, worksheetPart);
                    }

                    CellReference cellTable = cellTemplate.First(x => x.Name == "BodyTable");
                    CellReference _cellTable = Utilities.Utilities.FindOneCellByReference(file, cellTable.Text, true);

                    List<CellReference> cellBeforeTable = copycellExistText.Where(x => x.RowIndex < _cellTable.RowIndex).ToList();
                    List<CellReference> cellAfterTable = copycellExistText.Where(x => x.RowIndex > _cellTable.RowIndex).ToList();

                    if (cellBeforeTable.Count() > 0)
                    {
                        foreach (CellReference cell in cellBeforeTable)
                        {
                            Utilities.Utilities.SetDataCell(cell.Text, cell.ColumnCharacter, cell.RowIndex, cell.StyleIndex, shareStringPart, worksheetPart);
                        }
                    }

                    if (cellAfterTable.Count() > 0)
                    {
                        foreach (CellReference cell in cellAfterTable)
                        {
                            UInt32 row = cell.RowIndex + Convert.ToUInt32(items?.Count() - 1);
                            Utilities.Utilities.SetDataCell(cell.Text, cell.ColumnCharacter, row, cell.StyleIndex, shareStringPart, worksheetPart);
                        }
                    }

                    if (cellTable != null && _cellTable != null)
                    {
                        string[] columns = _cellTable.Text.Split(",");
                        UInt32 indexTable = _cellTable.RowIndex;
                        int indexCharTable = CellReferenceArray.IndexOf(_cellTable.ColumnCharacter);

                        List<CellReference> listBody = new List<CellReference>();

                        for (int i = 0; i < columns.Length; i++)
                        {
                            var c = Utilities.Utilities.FindOneCellByReference(file, CellReferenceArray[i + indexCharTable] + indexTable.ToString(), false);
                            listBody.Add(c);
                        }

                        foreach (var data in items.Take(500).ToList())
                        {
                            for (int i = 0; i < columns.Length; i++)
                            {
                                var c = listBody[i];
                                var text = Utilities.Utilities<CustomerDto>.GetValueByKey(data, columns[i]);
                                Utilities.Utilities.SetDataCell(c, text ?? "", indexTable, shareStringPart, worksheetPart);
                            }
                            indexTable += 1;
                        }
                    }

                    foreach (var cell in cellTemplate)
                    {
                        UInt32 rowindex = cell.RowIndex;
                        var c = cell;
                        c.Text = "";
                        Utilities.Utilities.SetDataCell(c, "", rowindex, shareStringPart, worksheetPart);
                    }
                }

                worksheetPart.Worksheet.Save();
            }
            var filepath = Path.Combine(path, "Print", newfile);
            return File(System.IO.File.ReadAllBytes(filepath), "application/xlsx", System.IO.Path.GetFileName(filepath));
        }
        catch (Exception ex)
        {
            return BadRequest(new ValidationResult(ex.Message));
        }
    }

    [HttpGet("excel-template")]
    public async Task<IActionResult> GetExcelTemplate()
    {
        var rs = await _mediator.Send(new CustomerImportTemplateQuery());
        if (rs != null)
        {
            return File(rs.ToArray(), "application/xlsx");
        }
        return Ok(rs);
    }

    [HttpPost("validate-excel")]
    public async Task<IActionResult> ValidateExcel([FromForm] ValidateImportCustomer request)
    {
        List<ValidateField> listField = new List<ValidateField>()
        {
            new ValidateField(){Field="code", IndexColumn= request.Code},
            new ValidateField(){Field="name", IndexColumn= request.Name},
            new ValidateField(){Field="type", IndexColumn= request.Type},
            new ValidateField(){Field="isVendor", IndexColumn= request.IsVendor},
            new ValidateField(){Field="day", IndexColumn= request.Day},
            new ValidateField(){Field="month", IndexColumn= request.Month},
            new ValidateField(){Field="year", IndexColumn= request.Year},
            new ValidateField(){Field="gender", IndexColumn= request.Gender},
            new ValidateField(){Field="phone", IndexColumn= request.Phone},
            new ValidateField(){Field="email", IndexColumn= request.Email},
            new ValidateField(){Field="taxCode", IndexColumn= request.TaxCode},
            new ValidateField(){Field="zipCode", IndexColumn= request.ZipCode},
            new ValidateField(){Field="fax", IndexColumn= request.Fax},
            new ValidateField(){Field="website", IndexColumn= request.Website},
            new ValidateField(){Field="businessSector", IndexColumn= request.BusinessSector},
            new ValidateField(){Field="companySize", IndexColumn= request.CompanySize},
            new ValidateField(){Field="capital", IndexColumn= request.Capital},
            new ValidateField(){Field="country", IndexColumn= request.Country},
            new ValidateField(){Field="province", IndexColumn= request.Province},
            new ValidateField(){Field="district", IndexColumn= request.District},
            new ValidateField(){Field="ward", IndexColumn= request.Ward},
            new ValidateField(){Field="address", IndexColumn= request.Address},
            new ValidateField(){Field="idNumber", IndexColumn= request.IdNumber},
            new ValidateField(){Field="idDate", IndexColumn= request.IdDate},
            new ValidateField(){Field="idIssuer", IndexColumn= request.IdIssuer},
            new ValidateField(){Field="customerGroup", IndexColumn= request.CustomerGroup},
            new ValidateField(){Field="groupEmployeeName", IndexColumn= request.GroupEmployeeName},
            new ValidateField(){Field="employeeName", IndexColumn= request.EmployeeName},
            new ValidateField(){Field="customerSource", IndexColumn= request.CustomerSource},
            new ValidateField(){Field="note", IndexColumn= request.Note},
        };

        var data = new ValidateCustomerImportQuery(request.File,
                                                    request.SheetId,
                                                    request.HeaderRow,
                                                    listField);
        var result = await _mediator.Send(data);
        return Ok(result);
    }

    [HttpPost("import-excel")]
    public async Task<IActionResult> ImportExcel([FromBody] IEnumerable<ImportCustomerRequest> request)
    {
        var customer = request.Select(x => new CustomerImportDto()
        {
            Code = x.Code,
            Name = x.Name,
            IsVendor = x.IsVendor,
            Type = x.Type,
            Year = x.Year,
            Month = x.Month,
            Day = x.Day,
            Gender = x.Gender,
            Phone = x.Phone,
            Email = x.Email,
            TaxCode = x.TaxCode,
            ZipCode = x.ZipCode,
            Fax = x.Fax,
            Website = x.Website,
            BusinessSector = x.BusinessSector,
            BusinessSectorId = x.BusinessSectorId,
            CompanySize = x.CompanySize,
            Capital = x.Capital,
            Country = x.Country,
            Province = x.Province,
            District = x.District,
            Ward = x.Ward,
            Address = x.Address,
            IdNumber = x.IdNumber,
            IdDate = x.IdDate,
            IdIssuer = x.IdIssuer,
            EmployeeId = x.EmployeeId,
            EmployeeName = x.EmployeeName,
            CustomerGroup = x.CustomerGroup,
            CustomerGroupId = x.CustomerGroupId,
            SalesGroup = x.SalesGroup,
            SalesGroupId = x.SalesGroupId,
            CustomerSource = x.CustomerSource,
            CustomerSourceId = x.CustomerSourceId,
            GroupEmployeeId = x.GroupEmployeeId,
            GroupEmployeeName = x.GroupEmployeeName,
            Note = x.Note,
            Status = x.Status ?? 1
        });

        var data = new ImportExcelCustomerCommand(customer);
        var result = await _mediator.SendCommand(data);
        return Ok(result);
    }
}
