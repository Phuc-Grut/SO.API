using System.ComponentModel.DataAnnotations;
using Aspose.Words;
using Aspose.Words.Reporting;
using DocumentFormat.OpenXml.Packaging;
using DocumentFormat.OpenXml.Spreadsheet;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Syncfusion.EJ2.Linq;
using VFi.Api.SO.ViewModels;
using VFi.Application.SO.Commands;
using VFi.Application.SO.DTOs;
using VFi.Application.SO.Queries;
using VFi.Domain.SO.Models;
using VFi.NetDevPack.Mediator;

namespace VFi.Api.SO.Controllers;

[Route("api/[controller]")]
[ApiController]
public class SalesDiscountController : ControllerBase
{
    private readonly IMediatorHandler _mediator;
    private readonly ILogger<SalesDiscountController> _logger;
    private readonly Microsoft.AspNetCore.Hosting.IHostingEnvironment _environment;

    public SalesDiscountController(
        IMediatorHandler mediator,
        ILogger<SalesDiscountController> logger,
        Microsoft.AspNetCore.Hosting.IHostingEnvironment environment)
    {
        _mediator = mediator;
        _logger = logger;
        _environment = environment;
    }

    [HttpGet("paging")]
    public async Task<IActionResult> Pagedresult([FromQuery] SalesDiscountPagingRequest request)
    {
        var query = new SalesDiscountPagingQuery(
            request.Keyword ?? "",
            request.CustomerId,
            request.Status,
            request.Filter ?? "",
            request.Order ?? "",
            request.PageNumber,
            request.PageSize);
        var result = await _mediator.Send(query);
        return Ok(result);
    }

    [HttpGet("get-by-id/{id}")]
    public async Task<IActionResult> Get(string id)
    {
        var Id = new Guid(id);
        SalesDiscountQueryById queryById = new SalesDiscountQueryById(Id);
        var result = await _mediator.Send(queryById);
        return Ok(result);
    }

    [HttpPost("add")]
    public async Task<IActionResult> Add([FromBody] AddSalesDiscountRequest request)
    {
        var SalesDiscountId = Guid.NewGuid();

        int UsedStatus = 1;
        var Code = request.Code;
        if ((bool)request.IsAuto)
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

        var listDetail = request.ListDetail?.Select(x => new SalesDiscountProductDto()
        {
            Id = Guid.NewGuid(),
            SalesDiscountId = SalesDiscountId,
            SalesOrderId = x.SalesOrderId,
            SalesOrderCode = x.SalesOrderCode,
            OrderProductId = x.OrderProductId,
            ProductName = x.ProductName,
            ProductCode = x.ProductCode,
            ProductId = x.ProductId,
            ProductImage = x.ProductImage,
            Quantity = x.Quantity,
            ReasonDiscount = x.ReasonDiscount,
            TaxCategoryId = x.TaxCategoryId,
            TaxRate = x.TaxRate,
            Tax = x.Tax,
            UnitPrice = x.UnitPrice,
            UnitId = x.UnitId,
            UnitName = x.UnitName,
            UnitCode = x.UnitCode,
            GroupUnitId = x.GroupUnitId,
            UnitType = x.UnitType,
            DisplayOrder = x.DisplayOrder,
            DiscountAmountDistribution = x.DiscountAmountDistribution,
            AmountDiscount = x.AmountDiscount,
            DiscountPercent = x.DiscountPercent,
        }).ToList();

        var SalesDiscountAddCommand = new SalesDiscountAddCommand(
            SalesDiscountId,
            Code,
            request.CustomerId,
            request.CustomerName,
            request.CustomerCode,
            request.CustomerAddressId,
            request.SalesOrderCode,
            request.SalesOrderId,
            request.CurrencyId,
            request.CurrencyCode,
            request.CurrencyName,
            request.ExchangeRate,
            request.EmployeeId,
            request.EmployeeName,
            request.EmployeeCode,
            request.Note,
            request.Status,
            request.TypeDiscount,
            request.DiscountDate,
            JsonConvert.SerializeObject(request.Files),
            listDetail
      );

        var result = await _mediator.SendCommand(SalesDiscountAddCommand);
        if (result.IsValid == false && request.IsAuto == true && result.Errors[0].ToString() == "Code already exists")
        {
            int loopTime = 5;
            for (int i = 0; i < loopTime; i++)
            {
                SalesDiscountAddCommand.Code = await _mediator.Send(new GetCodeQuery(request.ModuleCode, UsedStatus));
                var res = await _mediator.SendCommand(SalesDiscountAddCommand);
                if (res.IsValid == true)
                {
                    return Ok(new { errors = res.Errors, isValid = res.IsValid, ruleSetsExecuted = res.RuleSetsExecuted, returnCode = Code, returnId = SalesDiscountId });
                }
            }
        }
        return Ok(new { errors = result.Errors, isValid = result.IsValid, ruleSetsExecuted = result.RuleSetsExecuted, returnCode = Code, returnId = SalesDiscountId });
    }

    [HttpPut("process")]
    public async Task<IActionResult> Put([FromBody] ProcessSalesDiscountRequest request)
    {
        var _id = new Guid(request.Id);
        SalesDiscountDto dataSalesDiscount = await _mediator.Send(new SalesDiscountQueryById(_id));

        if (dataSalesDiscount == null)
            return BadRequest(new ValidationResult("Sales discount not exists"));
        var SalesDiscountEditCommand = new SalesDiscountProcessCommand(
            _id,
            request.ApproveComment,
            request.Status
       );

        var result = await _mediator.SendCommand(SalesDiscountEditCommand);
        return Ok(result);
    }

    [HttpPut("edit")]
    public async Task<IActionResult> Put([FromBody] EditSalesDiscountRequest request)
    {
        /*var Id = new Guid(request.Id);
        SalesDiscountDto dataSalesDiscount = await _mediator.Send(new SalesDiscountQueryById(Id));

        if (dataSalesDiscount == null) return BadRequest(new ValidationResult("SalesDiscount not exists"));*/
        if (request.IsUploadFile != null && request.IsUploadFile == true)
        {
            var purchaseDiscountUploadFileCommand = new SalesDiscountUploadFileCommand(request.Id, JsonConvert.SerializeObject(request.Files));
            var rs = await _mediator.SendCommand(purchaseDiscountUploadFileCommand);
            return Ok(rs);
        }
        var listDetail = request.ListDetail?.Select(x => new SalesDiscountProductDto()
        {
            Id = (Guid)(x.Id == null ? Guid.NewGuid() : x.Id),
            SalesDiscountId = request.Id,
            SalesOrderId = x.SalesOrderId,
            OrderProductId = x.OrderProductId,
            ProductName = x.ProductName,
            ProductCode = x.ProductCode,
            ProductId = x.ProductId,
            ProductImage = x.ProductImage,
            Quantity = x.Quantity,
            ReasonDiscount = x.ReasonDiscount,
            TaxCategoryId = x.TaxCategoryId,
            TaxRate = x.TaxRate,
            Tax = x.Tax,
            UnitPrice = x.UnitPrice,
            UnitId = x.UnitId,
            UnitName = x.UnitName,
            UnitCode = x.UnitCode,
            GroupUnitId = x.GroupUnitId,
            UnitType = x.UnitType,
            DisplayOrder = x.DisplayOrder,
            DiscountAmountDistribution = x.DiscountAmountDistribution,
            AmountDiscount = x.AmountDiscount,
            DiscountPercent = x.DiscountPercent,
        }).ToList();

        var SalesDiscountEditCommand = new SalesDiscountEditCommand(
            request.Id,
            request.Code,
            request.CustomerId,
            request.CustomerName,
            request.CustomerCode,
            request.CustomerAddressId,
            request.SalesOrderCode,
            request.SalesOrderId,
            request.CurrencyId,
            request.CurrencyCode,
            request.CurrencyName,
            request.ExchangeRate,
            request.EmployeeId,
            request.EmployeeName,
            request.EmployeeCode,
            request.Note,
            request.Status,
            request.TypeDiscount,
            request.DiscountDate,
            JsonConvert.SerializeObject(request.Files),
            listDetail
       );

        var result = await _mediator.SendCommand(SalesDiscountEditCommand);
        return Ok(result);
    }

    [HttpPost("duplicate")]
    public async Task<IActionResult> Duplicate([FromBody] DuplicateSalesDiscount request)
    {
        int UsedStatus = 1;
        var Code = request.Code;
        if ((bool)request.IsAuto)
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
        var item = new SalesDiscountDuplicateCommand(
            Guid.NewGuid(),
            request.SalesDiscountId,
            Code
            );
        var result = await _mediator.SendCommand(item);

        if (result.IsValid == false && request.IsAuto == true && result.Errors[0].ToString() == "requestNumber AlreadyExists")
        {
            int loopTime = 5;
            for (int i = 0; i < loopTime; i++)
            {
                item.Code = await _mediator.Send(new GetCodeQuery(request.ModuleCode, UsedStatus));
                var res = await _mediator.SendCommand(item);
                if (res.IsValid == true)
                {
                    return Ok(new { errors = res.Errors, isValid = res.IsValid, ruleSetsExecuted = res.RuleSetsExecuted, returnCode = Code });
                }
            }
        }
        return Ok(new { errors = result.Errors, isValid = result.IsValid, ruleSetsExecuted = result.RuleSetsExecuted, returnCode = Code });
    }

    [HttpDelete("delete/{id}")]
    public async Task<IActionResult> Delete(string id)
    {
        var SalesDiscountId = new Guid(id);
        if (await _mediator.Send(new SalesDiscountQueryById(SalesDiscountId)) == null)
            return BadRequest(new ValidationResult("SalesDiscount not exists"));

        var result = await _mediator.SendCommand(new SalesDiscountDeleteCommand(SalesDiscountId));

        return Ok(result);
    }

    [HttpGet("print/{id}")]
    public async Task<IActionResult> Print(Guid id)
    {
        try
        {
            ApplyLicense();
            var path = _environment.ContentRootPath;
            var item = await _mediator.Send(new SalesDiscountQueryById(id));
            /*item.TotalAmountText = item.TotalAmount != null ? NetDevPack.Utilities.Utilities.NumberToText_Currency(Decimal.ToDouble(Math.Round((decimal)item.TotalAmount, 2)), item.CurrencyCode ?? "") : "";
            item.ExportMonth = Convert.ToDateTime(DateTime.Now).ToString("MM");
            item.ExportYear = Convert.ToDateTime(DateTime.Now).ToString("yyyy");*/
            var doc = new Aspose.Words.Document(path + "/Template/GGHM.docx");
            ReportingEngine engine = new ReportingEngine();
            engine.BuildReport(doc, item, "data");
            MemoryStream stream = new MemoryStream();
            doc.Save(stream, SaveFormat.Pdf);

            return File(stream.GetBuffer(), "application/pdf", "GGHM" + item.Code + ".pdf");
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }

    private void ApplyLicense()
    {
        string licenseFile = _environment.ContentRootPath + "/Data/Aspose.lic";
        if (System.IO.File.Exists(licenseFile))
        {
            License license = new License();
            license.SetLicense(licenseFile);
        }
    }

    [HttpGet("export-excel/{id}")]
    public async Task<IActionResult> ExportCTNK(Guid id)
    {
        try
        {
            var rp = await _mediator.Send(new SalesDiscountQueryById(id));
            /* rp.TotalAmountText = rp.TotalAmount != null ? NetDevPack.Utilities.Utilities.NumberToText_Currency(Decimal.ToDouble(Math.Round((decimal)rp.TotalAmount, 2)), rp.CurrencyCode ?? "") : "";*/
            string path = _environment.ContentRootPath;
            string file = path + "/Template/GGHM.xlsx";
            string newfile = path + "/Print/" + rp.Code.Replace("/", "_") + ".xlsx";

            string[] deleteFiles = Directory.GetFiles(path + "/Print/");
            foreach (string deleteFile in deleteFiles)
            {
                System.IO.File.Delete(deleteFile);
            }
            System.IO.File.Copy(file, newfile);

            string[] CellReferenceArray = new string[] { "A", "B", "C", "D", "E", "F", "G", "H", "I", "J", "K", "L", "M", "N", "O", "P", "Q", "R", "S", "T", "U", "V", "W", "X", "Y", "Z" };

            string CellTextTemplate = "Table, Merge";

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
                List<CellReference> cellTemplate = Utilities.Utilities.FindCellByText(file, CellTextTemplate).Where(x => x.Name != "").ToList();
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

                    CellReference cellTable = cellTemplate.First(x => x.Name == "Table");
                    CellReference _cellTable = Utilities.Utilities.FindOneCellByReference(file, cellTable.Text, true);
                    List<CellReference> cellBeforeTable = copycellExistText.Where(x => x.RowIndex < _cellTable.RowIndex).ToList();
                    List<CellReference> cellAfterTable = copycellExistText.Where(x => x.RowIndex > _cellTable.RowIndex).ToList();

                    if (cellBeforeTable.Count() > 0)
                    {
                        foreach (CellReference cell in cellBeforeTable)
                        {
                            if (!cell.Text.Contains("^"))
                            {
                                Utilities.Utilities.SetDataCell(cell.Text, cell.ColumnCharacter, cell.RowIndex, cell.StyleIndex, shareStringPart, worksheetPart);
                            }
                            else
                            {
                                var text = cell.Text.Split("^")[0];
                                var property = cell.Text.Split("^")[1];
                                text = text + Utilities.Utilities<SalesDiscountDto>.GetValueByKeyGeneral(rp, property);
                                Utilities.Utilities.SetDataCell(text, cell.ColumnCharacter, cell.RowIndex, cell.StyleIndex, shareStringPart, worksheetPart);
                            }
                        }
                    }

                    if (cellTable != null && _cellTable != null)
                    {
                        string[] columns = _cellTable.Text.Split(",");
                        UInt32 indexTable = _cellTable.RowIndex;
                        int indexCharTable = CellReferenceArray.IndexOf(_cellTable.ColumnCharacter);
                        List<CellReference> listBody = new List<CellReference>();
                        for (int i = 0; i < columns.Count(); i++)
                        {
                            var c = Utilities.Utilities.FindOneCellByReference(file, CellReferenceArray[i + indexCharTable] + indexTable.ToString(), false);
                            listBody.Add(c);
                        }
                        foreach (var item in rp.ListDetails)
                        {
                            for (int i = 0; i < columns.Count(); i++)
                            {
                                var c = listBody[i];
                                var text = Utilities.Utilities<SalesDiscountProductDto>.GetValueByKeyGeneral(item, columns[i]);
                                Utilities.Utilities.SetDataCellGeneral(c, text, indexTable, shareStringPart, worksheetPart);
                            }
                            indexTable += 1;
                        }
                    }

                    if (cellAfterTable.Count() > 0)
                    {
                        foreach (CellReference cell in cellAfterTable)
                        {
                            var row = cell.RowIndex + Convert.ToUInt32(rp.ListDetails.Count() == 0 ? 0 : rp.ListDetails.Count() - 1);
                            if (!cell.Text.Contains("^"))
                            {
                                Utilities.Utilities.SetDataCell(cell.Text, cell.ColumnCharacter, row, cell.StyleIndex, shareStringPart, worksheetPart);
                            }
                            else
                            {
                                var text = cell.Text.Split("^")[0];
                                var property = cell.Text.Split("^")[1];
                                text = text + Utilities.Utilities<SalesDiscountDto>.GetValueByKeyGeneral(rp, property);
                                Utilities.Utilities.SetDataCell(text, cell.ColumnCharacter, row, cell.StyleIndex, shareStringPart, worksheetPart);
                            }
                        }
                    }

                    List<CellReference> cellMerges = cellTemplate.Where(x => x.Name == "Merge").ToList();
                    if (cellMerges.Count > 0)
                    {
                        foreach (var cellMerge in cellMerges)
                        {
                            var range = cellMerge.Text;
                            var cellStart = range.Split(":")[0];
                            var cellEnd = range.Split(":")[1];
                            var rowStart = UInt32.Parse(cellStart.Substring(1)) + Convert.ToUInt32(rp.ListDetails.Count() == 0 ? 0 : rp.ListDetails.Count() - 1);
                            var characterLeft = cellStart.Substring(0, 1);
                            var numberLeft = rowStart.ToString();
                            var characterRight = cellEnd.Substring(0, 1);
                            var numberRight = rowStart.ToString();

                            var cellStartTo = characterLeft + numberLeft;
                            var cellEndTo = characterRight + numberRight;
                            var rangeTo = cellStartTo + ":" + cellEndTo;
                            CellReference ce = Utilities.Utilities.FindOneCellByReference(file, cellStart, true);
                            Utilities.Utilities.MergeCellInRange(worksheet, rangeTo, characterLeft, numberLeft, characterRight, numberRight);
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
            return BadRequest(ex.Message);
        }
    }

    [HttpGet("excel-template")]
    public async Task<IActionResult> GetExcelTemplate()
    {
        var rs = await _mediator.Send(new SalesDiscountExportTemplateQuery());
        if (rs != null)
        {
            return File(rs.ToArray(), "application/xlsx");
        }
        return Ok(rs);
    }

    [HttpPost("validate-excel")]
    public async Task<IActionResult> ValidateExcel([FromForm] ValidateExcelExportSalesDiscount request)
    {
        List<ValidateField> listField = new List<ValidateField>()
        {
            new ValidateField(){Field="productCode", IndexColumn= request.ProductCode},
            new ValidateField(){Field="productName", IndexColumn= request.ProductName},
            new ValidateField(){Field="unitCode", IndexColumn= request.UnitCode},
            new ValidateField(){Field="unitName", IndexColumn= request.UnitName},
            new ValidateField(){Field="quantity", IndexColumn= request.Quantity},
            new ValidateField(){Field="unitPrice", IndexColumn= request.UnitPrice},
            new ValidateField(){Field="discountPercent", IndexColumn= request.DiscountPercent},
            new ValidateField(){Field="tax", IndexColumn= request.Tax},
            new ValidateField(){Field="reasonDiscount", IndexColumn= request.ReasonDiscount},
        };

        var data = new ValidateExcelSalesDiscountQuery(request.File,
                                                    request.SheetId,
                                                    request.HeaderRow,
                                                    listField);
        var result = await _mediator.Send(data);
        return Ok(result);
    }

    [HttpPost("manage-payment")]
    public async Task<IActionResult> ManagePayment([FromBody] ManagePaymentRequest request)
    {
        var paymentInvoice = request.PaymentInvoice?.Select(x => new PaymentInvoiceDto()
        {
            Id = x.Id ?? Guid.NewGuid(),
            Type = x.Type,
            Code = x.Code,
            SaleDiscountId = request.Id,
            Description = x.Description,
            Amount = x.Amount,
            Currency = x.Currency,
            CurrencyName = x.CurrencyName,
            Calculation = x.Calculation,
            ExchangeRate = x.ExchangeRate,
            PaymentDate = x.PaymentDate,
            PaymentMethodName = x.PaymentMethodName,
            PaymentMethodCode = x.PaymentMethodCode,
            PaymentMethodId = x.PaymentMethodId,
            BankName = x.BankName,
            BankAccount = x.BankAccount,
            BankNumber = x.BankNumber,
            PaymentCode = x.PaymentCode,
            PaymentNote = x.PaymentNote,
            Note = x.Note,
            Status = x.Status,
            PaymentStatus = x.PaymentStatus,
            AccountId = x.AccountId,
            AccountName = x.AccountName
        }).ToList();

        var cmd = new ManagePaymentSDCommand(
            request.Id,
            request.PaymentStatus,
            paymentInvoice
            );

        var result = await _mediator.SendCommand(cmd);

        return Ok(result);
    }
}
