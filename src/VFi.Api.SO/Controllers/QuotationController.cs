using Aspose.Words;
using Aspose.Words.Reporting;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using VFi.Api.SO.ViewModels;
using VFi.Application.SO.Commands;
using VFi.Application.SO.DTOs;
using VFi.Application.SO.Queries;
using VFi.Domain.SO.Models;
using VFi.NetDevPack.Context;
using VFi.NetDevPack.Mediator;

namespace VFi.Api.SO.Controllers;

[Route("api/[controller]")]
[ApiController]
public class QuotationController : ControllerBase
{
    private readonly IContextUser _context;
    private readonly IMediatorHandler _mediator;
    private readonly ILogger<QuotationController> _logger;
    private readonly Microsoft.AspNetCore.Hosting.IHostingEnvironment _environment;

    public QuotationController(IMediatorHandler mediator, IContextUser context, ILogger<QuotationController> logger, Microsoft.AspNetCore.Hosting.IHostingEnvironment environment)
    {
        _mediator = mediator;
        _context = context;
        _logger = logger;
        _environment = environment;
    }

    [HttpGet("get-list-send-transaction")]
    public async Task<IActionResult> GetListSendTransactionByOrder([FromQuery] QuotationSendTransactionRequest request)
    {
        var rs = await _mediator.Send(new SendTransactionQueryByQuotation(request.Keyword, request.Quotation));
        return Ok(rs);
    }

    [HttpGet("get-listbox-sendconfig")]
    public async Task<IActionResult> GetListboxSendConfig()
    {
        var rs = await _mediator.Send(new QuotationQuerySendConfigCombobox());
        return Ok(rs);
    }

    [HttpGet("get-listbox-sendtemplate")]
    public async Task<IActionResult> GetListboxSendTemplate()
    {
        var rs = await _mediator.Send(new QuotationQuerySendTemplateCombobox());
        return Ok(rs);
    }

    [HttpPost("builder")]
    public async Task<IActionResult> Builder([FromBody] EmailBuilderRequest request)
    {
        var query = new QuotationEmailBuilderQuery()
        {
            Subject = request.Subject,
            Template = request.Template,
            JBody = request.JBody
        };
        var result = await _mediator.Send(query);
        return Ok(result);
    }

    [HttpPost("send-email")]
    public async Task<IActionResult> SendEmail([FromBody] EmailNotifyRequest request)
    {
        var data = new QuotationEmailNotifyCommand(
            request.SenderCode,
            request.SenderName,
            request.Subject,
            request.From,
            request.To,
            request.CC,
            request.BCC,
            request.Body,
            request.TemplateCode
            );
        var result = await _mediator.SendCommand(data);
        return Ok(result);
    }

    [HttpGet("get-list-cbx")]
    public async Task<IActionResult> Get([FromQuery] QuotationListBoxRequest request)
    {
        var result = await _mediator.Send(new QuotationQueryComboBox(request.Keyword, request.ToBaseQuery(), request.PageSize, request.PageIndex));
        return Ok(result);
    }

    [HttpGet("get/{id}")]
    public async Task<IActionResult> Get(Guid id)
    {
        var rs = await _mediator.Send(new QuotationQueryById(id));
        return Ok(rs);
    }

    [HttpGet("paging")]
    public async Task<IActionResult> Paging([FromQuery] QuotationPagingRequest request)
    {
        if (_context.QueryMyData())
        {
            request.EmployeeId = _context.GetUserId().ToString();
        }
        var query = new QuotationPagingQuery(
              request.Keyword ?? "",
              request.Status,
              request.EmployeeId,
              request.Filter ?? "",
              request.Order ?? "",
              request.PageNumber,
              request.PageSize
            );
        var result = await _mediator.Send(query);
        return Ok(result);
    }

    /// Nhập thông tin
    /// </summary>
    /// <param name="request">thông tin</param>
    /// <returns>Quotation</returns>
    [HttpPost("add")]
    public async Task<IActionResult> Post([FromBody] AddQuotationRequest request)
    {
        var Code = request.Code;
        if (request.IsAuto == true)
        {
            Code = await _mediator.Send(new GetCodeQuery(request.ModuleCode, 1));
        }
        else
        {
            var useCodeCommand = new UseCodeCommand(
                request.ModuleCode,
                Code
                );
            _mediator.SendCommand(useCodeCommand);
        }
        var Id = request.Id != null ? request.Id : Guid.NewGuid();

        var addDetail = request.OrderProduct?.Select(x => new OrderProductDto(request.Calculation, request.ExchangeRate)
        {
            QuotationId = Id,
            QuotationName = Code,
            ProductId = x.ProductId,
            ProductCode = x.ProductCode,
            ProductName = x.ProductName,
            ProductImage = x.ProductImage,
            Origin = x.Origin,
            WarehouseId = x.WarehouseId,
            WarehouseCode = x.WarehouseCode,
            WarehouseName = x.WarehouseName,
            PriceListId = x.PriceListId,
            PriceListName = x.PriceListName,
            UnitType = x.UnitType,
            UnitCode = x.UnitCode,
            UnitName = x.UnitName,
            Quantity = x.Quantity,
            UnitPrice = x.UnitPrice,
            DiscountAmountDistribution = x.DiscountAmountDistribution,
            DiscountType = x.DiscountType,
            DiscountPercent = x.DiscountPercent,
            AmountDiscount = x.AmountDiscount,
            DiscountTotal = x.DiscountTotal,
            TaxRate = x.TaxRate,
            Tax = x.Tax,
            TaxCode = x.TaxCode,
            ExpectedDate = x.ExpectedDate,
            Note = x.Note,
            DisplayOrder = x.DisplayOrder,
            DeliveryStatus = x.DeliveryStatus,
            DeliveryQuantity = x.DeliveryQuantity,
            SpecificationCode1 = x.SpecificationCode1,
            SpecificationCode2 = x.SpecificationCode2,
            SpecificationCode3 = x.SpecificationCode3,
            SpecificationCode4 = x.SpecificationCode4,
            SpecificationCode5 = x.SpecificationCode5,
            SpecificationCode6 = x.SpecificationCode6,
            SpecificationCode7 = x.SpecificationCode7,
            SpecificationCode8 = x.SpecificationCode8,
            SpecificationCode9 = x.SpecificationCode9,
            SpecificationCode10 = x.SpecificationCode10,
            SpecificationCodeJson = x.SpecificationCodeJson
        }).ToList();

        var addService = request.OrderServiceAdd?.Select(x => new OrderServiceAddDto()
        {
            QuotationId = Id,
            ServiceAddId = x.ServiceAddId,
            ServiceAddName = x.ServiceAddName,
            Price = x.Price,
            Currency = x.Currency,
            Calculation = x.Calculation,
            Status = x.Status,
            Note = x.Note,
            ExchangeRate = x.ExchangeRate,
            DisplayOrder = x.DisplayOrder
        }).ToList();

        var data = new AddQuotationCommand(
            (Guid)Id,
            Code,
            request.Name,
            request.Description,
            request.Status,
            request.CustomerId,
            request.CustomerCode,
            request.CustomerName,
            request.Email,
            request.Phone,
            request.Address,
            request.StoreId,
            request.StoreCode,
            request.StoreName,
            request.ChannelId,
            request.ChannelName,
            request.DeliveryNote,
            request.DeliveryName,
            request.DeliveryAddress,
            request.DeliveryCountry,
            request.DeliveryProvince,
            request.DeliveryDistrict,
            request.DeliveryWard,
            request.DeliveryStatus,
            request.IsBill,
            request.BillName,
            request.BillAddress,
            request.BillCountry,
            request.BillProvince,
            request.BillDistrict,
            request.BillWard,
            request.BillStatus,
            request.ShippingMethodId,
            request.ShippingMethodCode,
            request.ShippingMethodName,
            request.DeliveryMethodId,
            request.DeliveryMethodCode,
            request.DeliveryMethodName,
            request.ExpectedDate,
            request.Currency,
            request.CurrencyName,
            request.Calculation,
            request.ExchangeRate,
            request.PriceListId,
            request.PriceListName,
            request.RequestQuoteId,
            request.RequestQuoteCode,
            request.ContractId,
            request.SaleOrderId,
            request.QuotationTermId,
            request.QuotationTermContent,
            request.Date,
            request.ExpiredDate,
            request.GroupEmployeeId,
            request.GroupEmployeeName,
            request.AccountId,
            request.AccountName,
            request.TypeDiscount,
            request.DiscountRate,
            request.TypeCriteria,
            request.AmountDiscount,
            request.Note,
            request.OldId,
            request.OldCode,
            JsonConvert.SerializeObject(request.File),
            addDetail,
            addService
        );

        var result = await _mediator.SendCommand(data);
        if (result.IsValid)
        {
            result.RuleSetsExecuted = new string[] { Id + "", Code, request.Email + "" };
        }
        return Ok(result);
    }

    /// <summary>
    /// Cập nhật thông tin
    /// </summary>
    /// <param name="request">Thông  tin</param>
    /// <returns>Quotation</returns>
    [HttpPut("edit")]
    public async Task<IActionResult> Put([FromBody] EditQuotationRequest request)
    {
        //chi tiết
        var updateDetail = request.OrderProduct?.Select(x => new OrderProductDto(request.Calculation, request.ExchangeRate)
        {
            Id = !String.IsNullOrEmpty(x.Id) ? new Guid(x.Id) : null,
            QuotationId = request.Id,
            QuotationName = request.Code,
            ProductId = x.ProductId,
            ProductCode = x.ProductCode,
            ProductName = x.ProductName,
            ProductImage = x.ProductImage,
            Origin = x.Origin,
            WarehouseId = x.WarehouseId,
            WarehouseCode = x.WarehouseCode,
            WarehouseName = x.WarehouseName,
            PriceListId = x.PriceListId,
            PriceListName = x.PriceListName,
            UnitType = x.UnitType,
            UnitCode = x.UnitCode,
            UnitName = x.UnitName,
            Quantity = x.Quantity,
            UnitPrice = x.UnitPrice,
            DiscountAmountDistribution = x.DiscountAmountDistribution,
            DiscountType = x.DiscountType,
            DiscountPercent = x.DiscountPercent,
            AmountDiscount = x.AmountDiscount,
            DiscountTotal = x.DiscountTotal,
            TaxRate = x.TaxRate,
            Tax = x.Tax,
            TaxCode = x.TaxCode,
            ExpectedDate = x.ExpectedDate,
            Note = x.Note,
            DisplayOrder = x.DisplayOrder,
            DeliveryStatus = x.DeliveryStatus,
            DeliveryQuantity = x.DeliveryQuantity,
            SpecificationCode1 = x.SpecificationCode1,
            SpecificationCode2 = x.SpecificationCode2,
            SpecificationCode3 = x.SpecificationCode3,
            SpecificationCode4 = x.SpecificationCode4,
            SpecificationCode5 = x.SpecificationCode5,
            SpecificationCode6 = x.SpecificationCode6,
            SpecificationCode7 = x.SpecificationCode7,
            SpecificationCode8 = x.SpecificationCode8,
            SpecificationCode9 = x.SpecificationCode9,
            SpecificationCode10 = x.SpecificationCode10,
            SpecificationCodeJson = x.SpecificationCodeJson
        }).ToList();

        //Dịch vụ
        var updateService = request.OrderServiceAdd?.Select(x => new OrderServiceAddDto()
        {
            Id = !String.IsNullOrEmpty(x.Id) ? new Guid(x.Id) : null,
            QuotationId = request.Id,
            ServiceAddId = x.ServiceAddId,
            ServiceAddName = x.ServiceAddName,
            Price = x.Price,
            Currency = x.Currency,
            Calculation = x.Calculation,
            Status = x.Status,
            Note = x.Note,
            ExchangeRate = x.ExchangeRate,
            DisplayOrder = x.DisplayOrder
        }).ToList();

        var data = new EditQuotationCommand(
            request.Id,
            request.Code,
            request.Name,
            request.Description,
            request.Status,
            request.CustomerId,
            request.CustomerCode,
            request.CustomerName,
            request.Email,
            request.Phone,
            request.Address,
            request.StoreId,
            request.StoreCode,
            request.StoreName,
            request.ChannelId,
            request.ChannelName,
            request.DeliveryNote,
            request.DeliveryName,
            request.DeliveryAddress,
            request.DeliveryCountry,
            request.DeliveryProvince,
            request.DeliveryDistrict,
            request.DeliveryWard,
            request.DeliveryStatus,
            request.IsBill,
            request.BillName,
            request.BillAddress,
            request.BillCountry,
            request.BillProvince,
            request.BillDistrict,
            request.BillWard,
            request.BillStatus,
            request.ShippingMethodId,
            request.ShippingMethodCode,
            request.ShippingMethodName,
            request.DeliveryMethodId,
            request.DeliveryMethodCode,
            request.DeliveryMethodName,
            request.ExpectedDate,
            request.Currency,
            request.CurrencyName,
            request.Calculation,
            request.ExchangeRate,
            request.PriceListId,
            request.PriceListName,
            request.RequestQuoteId,
            request.RequestQuoteCode,
            request.ContractId,
            request.SaleOrderId,
            request.QuotationTermId,
            request.QuotationTermContent,
            request.Date,
            request.ExpiredDate,
            request.GroupEmployeeId,
            request.GroupEmployeeName,
            request.AccountId,
            request.AccountName,
            request.TypeDiscount,
            request.DiscountRate,
            request.TypeCriteria,
            request.AmountDiscount,
            request.Note,
            JsonConvert.SerializeObject(request.File),
            updateDetail,
            updateService
       );

        var result = await _mediator.SendCommand(data);
        return Ok(result);
    }

    /// <summary>
    /// xoá
    /// </summary>
    /// <param name="id">Mã</param>
    /// <returns>Quotation</returns>
    [HttpDelete("delete/{id}")]
    public async Task<IActionResult> Delete(Guid id)
    {
        var data = new DeleteQuotationCommand(id);

        var result = await _mediator.SendCommand(data);

        return Ok(result);
    }

    [HttpPost("update-status")]
    public async Task<IActionResult> UpdateStatus([FromBody] UpdateStatusQuotationRequest request)
    {
        var data = new UpdateStatusQuotationCommand(
            request.Id,
            request.Status,
            request.ApproveComment
            );

        var result = await _mediator.SendCommand(data);

        return Ok(result);
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

    [HttpGet("print/{id}")]
    public async Task<IActionResult> Print(Guid id)
    {
        try
        {
            ApplyLicense();
            var path = _environment.ContentRootPath;
            var item = await _mediator.Send(new QuotationGetDataPrint(id));
            for (int i = 0; i < item.Details.Count; i++)
            {
                item.Details[i].SortOrder = i + 1;
            }
            var doc = new Document(path + "/Template/BAOGIAA4.docx");
            ReportingEngine engine = new ReportingEngine();
            engine.BuildReport(doc, item, "data");
            MemoryStream stream = new MemoryStream();
            doc.Save(stream, SaveFormat.Pdf);
            return File(stream.GetBuffer(), "application/pdf", "BAOGIA" + item.Code + ".pdf");
        }
        catch (Exception)
        {
            return NotFound();
        }
    }

    [HttpGet("excel-template")]
    public async Task<IActionResult> GetExcelTemplate()
    {
        var rs = await _mediator.Send(new QuotationExportTemplateQuery());
        if (rs != null)
        {
            return File(rs.ToArray(), "application/xlsx");
        }
        return Ok(rs);
    }

    [HttpPost("validate-excel")]
    public async Task<IActionResult> ValidateExcel([FromForm] ValidateExcelQuotationRequest request)
    {
        List<ValidateField> listField = new List<ValidateField>()
        {
            new ValidateField(){Field="productCode", IndexColumn= request.ProductCode},
            new ValidateField(){Field="productName", IndexColumn= request.ProductName},
            new ValidateField(){Field="unitCode", IndexColumn= request.UnitCode},
            new ValidateField(){Field="unitName", IndexColumn= request.UnitName},
            new ValidateField(){Field="unitPrice", IndexColumn= request.UnitPrice},
            new ValidateField(){Field="quantity", IndexColumn= request.Quantity},
            new ValidateField(){Field="discountPercent", IndexColumn= request.DiscountPercent},
            new ValidateField(){Field="tax", IndexColumn= request.Tax},
            new ValidateField(){Field="note", IndexColumn= request.Note},
        };
        var data = new ValidateExcelQuotationQuery(request.File,
                                                    request.SheetId,
                                                    request.HeaderRow,
                                                    listField);

        var result = await _mediator.Send(data);
        return Ok(result);
    }
}
