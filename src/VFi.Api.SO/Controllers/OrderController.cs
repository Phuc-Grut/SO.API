using Aspose.BarCode;
using Aspose.BarCode.Generation;
using Aspose.BarCode.Generation.V3;
using Aspose.Words;
using Aspose.Words.Reporting;
using Aspose.Words.Saving;
using DocumentFormat.OpenXml.Packaging;
using DocumentFormat.OpenXml.Spreadsheet;
using FluentValidation.Results;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Syncfusion.EJ2.Linq;
using VFi.Api.SO.ViewModels;
using VFi.Application.SO.Commands;
using VFi.Application.SO.DTOs;
using VFi.Application.SO.Queries;
using VFi.Domain.SO.Models;
using VFi.NetDevPack.Configuration;
using VFi.NetDevPack.Context;
using VFi.NetDevPack.Mediator;
using static VFi.Api.SO.ViewModels.OrderProductPagingRequest;

namespace VFi.Api.SO.Controllers;

[Route("api/[controller]")]
[ApiController]
[Authorize]
public partial class OrderController : ControllerBase
{
    private readonly IContextUser _context;
    private readonly IMediatorHandler _mediator;
    private readonly ILogger<OrderController> _logger;
    private readonly CodeSyntaxConfig _codeSyntax;

    [Obsolete]
    private readonly Microsoft.AspNetCore.Hosting.IHostingEnvironment _environment;

    [Obsolete]
    public OrderController(IMediatorHandler mediator, IContextUser context, CodeSyntaxConfig codeSyntax, ILogger<OrderController> logger, Microsoft.AspNetCore.Hosting.IHostingEnvironment environment)
    {
        _mediator = mediator;
        _context = context;
        _logger = logger;
        _codeSyntax = codeSyntax;
        _environment = environment;
    }

    [HttpPost("recalculate-price")]
    public async Task<ValidationResult> RecalculatePrice([FromBody] RecalculatePriceRequest request)
    {
        var cmd = new RecalculatePriceCommand(
            request.Id,
            _context.UserId,
            _context.UserName
        );
        return await _mediator.SendCommand(cmd);
    }

    [HttpGet("preview-recalculate-price/{id}")]
    public async Task<IActionResult> PreviewRecalculatedPrice(Guid id)
    {
        var result = await _mediator.Send(new PreviewRecalculatedPriceQuery(id));
        return Ok(result);
    }

    [HttpGet("get-list-send-transaction")]
    public async Task<IActionResult> GetListSendTransactionByOrder([FromQuery] OrderSendTransactionRequest request)
    {
        var rs = await _mediator.Send(new SendTransactionQueryByOrder(request.Keyword, request.Order));
        return Ok(rs);
    }

    [HttpGet("get-listbox-sendconfig")]
    public async Task<IActionResult> GetListboxSendConfig()
    {
        var rs = await _mediator.Send(new OrderQuerySendConfigCombobox());
        return Ok(rs);
    }

    [HttpGet("get-listbox-sendtemplate")]
    public async Task<IActionResult> GetListboxSendTemplate()
    {
        var rs = await _mediator.Send(new OrderQuerySendTemplateCombobox());
        return Ok(rs);
    }

    [HttpPost("builder")]
    public async Task<IActionResult> Builder([FromBody] EmailBuilderRequest request)
    {
        var query = new OrderEmailBuilderQuery()
        {
            Subject = request.Subject,
            Template = request.Template,
            JBody = request.JBody
        };
        var result = await _mediator.Send(query);
        return Ok(result);
    }

    [HttpPost("send-email")]
    public async Task<IActionResult> SendEmail([FromBody] OrderEmailNotifyRequest request)
    {
        var data = new OrderEmailNotifyCommand(
            request.Order,
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
    public async Task<IActionResult> GetListBox([FromQuery] OrderReferenceRequest request)
    {
        var result = await _mediator.Send(new OrderQueryComboBox(request.Keyword, request.ToBaseQuery(), request.PageSize, request.PageIndex));
        return Ok(result);
    }

    [HttpGet("get/{id}")]
    public async Task<IActionResult> Get(Guid id)
    {
        var rs = await _mediator.Send(new OrderQueryById(id));
        return Ok(rs);
    }

    [HttpGet("get-by-ids")]
    public async Task<IActionResult> GetByIds([FromQuery] List<Guid> ids)
    {
        var result = await _mediator.Send(new OrderQueryByIds(ids));
        return Ok(result);
    }

    [HttpGet("get-reference")]
    public async Task<IActionResult> Get([FromQuery] OrderReferenceRequest request)
    {
        var result = await _mediator.Send(new OrderGetReferenceQuery(request.ToBaseQuery()));
        return Ok(result);
    }

    /// <summary>
    /// Lấy danh sách  theo phân trang
    /// </summary>
    /// <param name="request"> phân trang</param>
    /// <returns>List Order</returns>
    [HttpGet("paging")]
    public async Task<IActionResult> Paging([FromQuery] OrderPagingRequest request, [FromQuery] string? customer = null)
    {
        if (_context.QueryMyData())
        {
            request.EmployeeId = _context.GetUserId().ToString();
        }
        var query = new OrderPagingQuery(
              request.Keyword ?? "",
              customer,
              request.EmployeeId,
              request.Status,
              request.DomesticStatus,
              request.Filter ?? "",
              request.Order ?? "",
              request.PageNumber,
              request.PageSize
          );
        var result = await _mediator.Send(query);
        return Ok(result);
    }

    /// <summary>
    /// Nhập thông tin
    /// </summary>
    /// <param name="request">thông tin</param>
    /// <returns>Order</returns>
    [HttpPost("add")]
    public async Task<IActionResult> Post([FromBody] AddOrderRequest request)
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
        var Id = Guid.NewGuid();
        var listExpectedDelivery = request.ListExpectedDelivery?.ToList();
        var addDetail = request.OrderProduct?.Select(x => new OrderProductDto(request.Calculation, request.ExchangeRate)
        {
            Id = Guid.NewGuid(),
            OrderId = Id,
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
            EstimatedDeliveryDate = x.EstimatedDeliveryDate,
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
            SpecificationCodeJson = x.SpecificationCodeJson,
            Guid = x.Guid,
            BidUsername = x.BidUsername,
            SourceCode = x.SourceCode,
            SourceLink = x.SourceLink,
        }).ToList();

        var addService = request.OrderServiceAdd?.Select(x => new OrderServiceAddDto()
        {
            OrderId = Id,
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

        var addPaymentInvoice = request.PaymentInvoice?.Select(x => new PaymentInvoiceDto()
        {
            Type = x.Type,
            Code = x.Code,
            OrderId = Id,
            OrderCode = Code,
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
            Locked = x.Locked,
            PaymentStatus = x.PaymentStatus,
            AccountId = x.AccountId,
            AccountName = x.AccountName,
            CustomerId = request.CustomerId,
            CustomerName = request.CustomerName
        }).ToList();

        var orderInvoice = request.OrderInvoice?.Select(x => new OrderInvoiceDto()
        {
            Serial = x.Serial,
            Symbol = x.Symbol,
            Number = x.Number,
            Value = x.Value,
            Date = x.Date,
            Note = x.Note,
            DisplayOrder = x.DisplayOrder
        }).ToList();

        var cmd = new AddOrderCommand(
            Id,
            request.OrderType,
            Code,
            request.OrderDate,
            request.CustomerId,
            request.CustomerName,
            request.CustomerCode,
            request.StoreId,
            request.StoreCode,
            request.StoreName,
            request.TypeDocument,
            request.ContractId,
            request.ContractName,
            request.QuotationId,
            request.QuotationName,
            request.ChannelId,
            request.ChannelName,
            request.Status,
            request.Currency,
            request.CurrencyName,
            request.Calculation,
            request.ExchangeRate,
            request.PriceListId,
            request.PriceListName,
            request.PaymentTermId,
            request.PaymentTermName,
            request.PaymentMethodName,
            request.PaymentMethodId,
            request.PaymentStatus,
            request.DeliveryAddress,
            request.DeliveryPhone,
            request.DeliveryName,
            request.DeliveryCountry,
            request.DeliveryProvince,
            request.DeliveryDistrict,
            request.DeliveryWard,
            request.DeliveryNote,
            request.EstimatedDeliveryDate,
            request.IsBill,
            request.BillAddress,
            request.BillCountry,
            request.BillProvince,
            request.BillDistrict,
            request.BillWard,
            request.BillStatus,
            request.DeliveryMethodId,
            request.DeliveryMethodCode,
            request.DeliveryMethodName,
            request.DeliveryStatus,
            request.ShippingMethodId,
            request.ShippingMethodCode,
            request.ShippingMethodName,
            request.TypeDiscount,
            request.DiscountRate,
            request.TypeCriteria,
            request.AmountDiscount,
            request.Note,
            request.GroupEmployeeId,
            request.GroupEmployeeName,
            request.AccountId,
            request.AccountName,
            request.Image,
            request.Description,
            JsonConvert.SerializeObject(request.File),
            addDetail,
            addService,
            addPaymentInvoice,
            orderInvoice,
            listExpectedDelivery
        );
        cmd.DeliveryTracking = request.DeliveryTracking;
        cmd.DeliveryCarrier = request.DeliveryCarrier;
        cmd.DeliveryPackage = request.DeliveryPackage;
        cmd.RouterShipping = request.RouterShipping;
        cmd.DomesticTracking = request.DomesticTracking;
        cmd.DomesticCarrier = request.DomesticCarrier;
        cmd.DomesticPackage = request.DomesticPackage;
        cmd.Weight = request.Weight;
        cmd.Width = request.Width;
        cmd.Height = request.Height;
        cmd.Length = request.Length;

        var result = await _mediator.SendCommand(cmd);
        return Ok(result);
    }

    /// <summary>
    /// Cập nhật thông tin
    /// </summary>
    /// <param name="request">Thông  tin</param>
    /// <returns>Order</returns>
    [HttpPut("edit")]
    public async Task<IActionResult> Put([FromBody] EditOrderRequest request)
    {
        //chi tiết
        var updateDetail = request.OrderProduct?.Select(x => new OrderProductDto(request.Calculation, request.ExchangeRate)
        {
            Id = !String.IsNullOrEmpty(x.Id) ? new Guid(x.Id) : x.Guid,
            OrderId = x.OrderId,
            OrderCode = x.OrderCode,
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
            EstimatedDeliveryDate = x.EstimatedDeliveryDate,
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
            SpecificationCodeJson = x.SpecificationCodeJson,
            Guid = x.Guid,
            BidUsername = x.BidUsername
        }).ToList();
        var listExpectedDelivery = request.ListExpectedDelivery.ToList();

        //Dịch vụ
        var updateService = request.OrderServiceAdd?.Select(x => new OrderServiceAddDto()
        {
            Id = !String.IsNullOrEmpty(x.Id) ? new Guid(x.Id) : null,
            OrderId = request.Id,
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

        //Thanh toán
        var updatePaymentInvoice = request.PaymentInvoice?.Select(x => new PaymentInvoiceDto()
        {
            Id = x.Id ?? Guid.NewGuid(),
            Type = x.Type,
            Code = x.Code,
            OrderId = request.Id,
            OrderCode = request.Code,
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
            Locked = x.Locked,
            PaymentStatus = x.PaymentStatus,
            AccountId = x.AccountId,
            AccountName = x.AccountName,
            CustomerId = request.CustomerId,
            CustomerName = request.CustomerName
        }).ToList();

        var orderInvoice = request.OrderInvoice?.Select(x => new OrderInvoiceDto()
        {
            Id = x.Id,
            Serial = x.Serial,
            Symbol = x.Symbol,
            Number = x.Number,
            Value = x.Value,
            Date = x.Date,
            Note = x.Note,
            DisplayOrder = x.DisplayOrder
        }).ToList();

        var cmd = new EditOrderCommand(
            request.Id,
            request.OrderType,
            request.Code,
            request.OrderDate,
            request.CustomerId,
            request.CustomerName,
            request.CustomerCode,
            request.StoreId,
            request.StoreCode,
            request.StoreName,
            request.TypeDocument,
            request.ContractId,
            request.ContractName,
            request.QuotationId,
            request.QuotationName,
            request.ChannelId,
            request.ChannelName,
            request.Status,
            request.Currency,
            request.CurrencyName,
            request.Calculation,
            request.ExchangeRate,
            request.PriceListId,
            request.PriceListName,
            request.PaymentTermId,
            request.PaymentTermName,
            request.PaymentMethodName,
            request.PaymentMethodId,
            request.PaymentStatus,
            request.DeliveryAddress,
            request.DeliveryPhone,
            request.DeliveryName,
            request.DeliveryCountry,
            request.DeliveryProvince,
            request.DeliveryDistrict,
            request.DeliveryWard,
            request.DeliveryNote,
            request.EstimatedDeliveryDate,
            request.IsBill,
            request.BillAddress,
            request.BillCountry,
            request.BillProvince,
            request.BillDistrict,
            request.BillWard,
            request.BillStatus,
            request.DeliveryMethodId,
            request.DeliveryMethodCode,
            request.DeliveryMethodName,
            request.DeliveryStatus,
            request.ShippingMethodId,
            request.ShippingMethodCode,
            request.ShippingMethodName,
            request.TypeDiscount,
            request.DiscountRate,
            request.TypeCriteria,
            request.AmountDiscount,
            request.Note,
            request.GroupEmployeeId,
            request.GroupEmployeeName,
            request.AccountId,
            request.AccountName,
            request.Image,
            request.Description,
            JsonConvert.SerializeObject(request.File),
            updateDetail,
            updateService,
            updatePaymentInvoice,
            orderInvoice,
            listExpectedDelivery
       );
        cmd.DeliveryTracking = request.DeliveryTracking;
        cmd.DeliveryCarrier = request.DeliveryCarrier;
        cmd.DeliveryPackage = request.DeliveryPackage;
        cmd.RouterShipping = request.RouterShipping;
        cmd.DomesticTracking = request.DomesticTracking;
        cmd.DomesticCarrier = request.DomesticCarrier;
        cmd.DomesticPackage = request.DomesticPackage;
        cmd.Weight = request.Weight;
        cmd.Width = request.Width;
        cmd.Height = request.Height;
        cmd.Length = request.Length;
        var result = await _mediator.SendCommand(cmd);
        return Ok(result);
    }

    /// <summary>
    /// xoá
    /// </summary>
    /// <param name="id">Mã</param>
    /// <returns>Order</returns>
    [HttpDelete("delete/{id}")]
    public async Task<ValidationResult> Delete(Guid id)
    {
        var data = new DeleteOrderCommand(id);

        var result = await _mediator.SendCommand(data);

        return result;
    }

    [HttpPost("approval")]
    public async Task<IActionResult> UpdateStatus([FromBody] ApprovalOrderRequest request)
    {
        var data = new ApprovalOrderCommand(
            request.Id,
            request.Status,
            request.ApproveComment
            );

        var result = await _mediator.SendCommand(data);

        return Ok(result);
    }

    [HttpPost("approvals")]
    public async Task<IActionResult> UpdateStatusMulti([FromBody] ApprovalOrdersRequest request)
    {
        var data = new ApprovalOrdersCommand(
            request.Ids,
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
            Aspose.Words.License license = new Aspose.Words.License();
            license.SetLicense(licenseFile);
            Aspose.BarCode.License licenseBarcode = new Aspose.BarCode.License();
            licenseBarcode.SetLicense(licenseFile);
        }
    }

    [HttpGet("print/{id}")]
    public async Task<IActionResult> Print(Guid id)
    {
        try
        {
            ApplyLicense();
            var path = _environment.ContentRootPath;
            var item = await _mediator.Send(new OrderGetDataPrint(id));
            for (int i = 0; i < item.Details.Count; i++)
            {
                item.Details[i].SortOrder = i + 1;
            }
            var doc = new Document(path + "/Template/DONHANGA4.docx");
            ReportingEngine engine = new ReportingEngine();
            engine.BuildReport(doc, item, "data");
            MemoryStream stream = new MemoryStream();
            //doc.Save(stream, SaveFormat.Pdf);
            PdfSaveOptions saveOptions = new PdfSaveOptions { OptimizeOutput = true };
            doc.Save(stream, saveOptions);
            return File(stream.GetBuffer(), "application/pdf", "DONHANG" + item.Code + ".pdf");
        }
        catch (Exception)
        {
            return NotFound();
        }
    }

    [HttpGet("print-stamp/{id}")]
    public async Task<IActionResult> PrintStamp(Guid id)
    {
        try
        {
            ApplyLicense();
            var path = _environment.ContentRootPath;
            var item = await _mediator.Send(new OrderGetDataPrint(id));
            var doc = new Document(path + "/Template/ORDER_STAMP.docx");
            ReportingEngine engine = new ReportingEngine();
            engine.BuildReport(doc, item, "data");

            //FontSettings fontSettings = new FontSettings();
            //fontSettings.SubstitutionSettings.DefaultFontSubstitution.Enabled = true;
            //fontSettings.SetFontsFolder("/usr/share/fonts", true);
            //fontSettings.FallbackSettings.LoadNotoFallbackSettings();
            //fontSettings.SubstitutionSettings.FontInfoSubstitution.Enabled = false;
            //doc.FontSettings = fontSettings;

            MemoryStream stream = new MemoryStream();
            PdfSaveOptions saveOptions = new PdfSaveOptions { OptimizeOutput = true };
            doc.Save(stream, saveOptions);
            return File(stream.GetBuffer(), "application/pdf", "ORDER_STAMP" + item.Code + ".pdf");
        }
        catch (Exception)
        {
            return NotFound();
        }
    }

    public class PrintOrderModel
    {
        public string Index { get; set; }
        public string CustomerCode { get; set; }
        public string CustomerName { get; set; }
        public string Tracking { get; set; }
        public string Code { get; set; }
        public string Description { get; set; }
    }

    [HttpGet("print-stamps")]
    public async Task<IActionResult> PrintStamps([FromQuery] Guid[] ids)
    {
        try
        {
            var index = 1;
            ApplyLicense();
            var path = _environment.ContentRootPath;
            var items = ids.Select(id => _mediator.Send(new OrderGetDataPrint(id))
                .GetAwaiter()
                .GetResult())
                .OrderByDescending(x => x.Tracking)
                .Select(x => new PrintOrderModel
                {
                    Index = (index++).ToString("D2"),
                    CustomerCode = x.CustomerCode,
                    CustomerName = x.CustomerName,
                    Tracking = x.Tracking,
                    Code = x.Code,
                    Description = x.Description
                }).ToList();
            var doc = new Document(path + "/Template/ORDER_STAMP.docx");
            ReportingEngine engine = new ReportingEngine();
            MemoryStream stream = new MemoryStream();
            PdfSaveOptions saveOptions = new PdfSaveOptions { OptimizeOutput = true };

            //engine.BuildReport(doc, items, "items");
            List<Document> docFinals = new List<Document>();
            //var builder = new Aspose.Words.DocumentBuilder(doc);
            engine.Options = ReportBuildOptions.AllowMissingMembers;
            foreach (var item in items)
            {
                var currentPage = doc.Clone();
                item.CustomerName = item?.CustomerName?.Trim();
                if (item != null && !string.IsNullOrEmpty(item.Tracking))
                {
                    item.Tracking += "\n";
                }
                // Điền dữ liệu cho phần đầu tiên
                engine.BuildReport(currentPage, item, "data");
                // Chèn ngắt trang
                docFinals.Add(currentPage);
            }

            foreach (var item in docFinals)
            {
                if (item != docFinals.First())
                {
                    docFinals.First().AppendDocument(item, ImportFormatMode.KeepSourceFormatting);
                }
            }

            //FontSettings fontSettings = new FontSettings();
            //fontSettings.SubstitutionSettings.DefaultFontSubstitution.Enabled = true;
            //fontSettings.SetFontsFolder("/usr/share/fonts", true);
            //fontSettings.FallbackSettings.LoadNotoFallbackSettings();
            //fontSettings.SubstitutionSettings.FontInfoSubstitution.Enabled = false;
            //doc.FontSettings = fontSettings;
            docFinals.First().Save(stream, saveOptions);

            return File(stream.GetBuffer(), "application/pdf", "ORDER_STAMPS" + string.Join("-", items.Select(x => x.Code)) + ".pdf");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, ex.Message);
            return NotFound();
        }
    }

    [HttpGet("print-label/{id}")]
    public async Task<IActionResult> PrintLabel(Guid id)
    {
        try
        {
            ApplyLicense();
            var path = _environment.ContentRootPath;
            var item = await _mediator.Send(new OrderGetDataPrint(id));
            BarcodeGenerator generator = new BarcodeGenerator(EncodeTypes.Code128, item.Code);
            generator.Parameters.Barcode.CodeTextParameters.Location = Aspose.BarCode.Generation.V3.CodeLocation.None;
            MemoryStream str = new MemoryStream();
            generator.Save(str, BarCodeImageFormat.Bmp);
            item.BarCode = str.GetBuffer();

            BarcodeGenerator generatorQr = new BarcodeGenerator(EncodeTypes.QR, item.Code);
            generatorQr.Parameters.Barcode.CodeTextParameters.Location = Aspose.BarCode.Generation.V3.CodeLocation.None;
            MemoryStream strQr = new MemoryStream();
            generatorQr.Save(strQr, BarCodeImageFormat.Bmp);
            item.QrCode = strQr.GetBuffer();

            for (int i = 0; i < item.Details.Count; i++)
            {
                item.Details[i].SortOrder = i + 1;
            }

            //FontSettings fontSettings = new FontSettings();
            //fontSettings.SubstitutionSettings.DefaultFontSubstitution.Enabled = true;
            //fontSettings.SetFontsFolder(Path.Combine("Fonts"), true);
            //fontSettings.FallbackSettings.LoadNotoFallbackSettings();
            //fontSettings.SubstitutionSettings.FontInfoSubstitution.Enabled = false;

            var doc = new Document(path + "/Template/ORDER_LABEL.docx");
            ReportingEngine engine = new ReportingEngine();
            engine.BuildReport(doc, item, "data");

            //FontSettings fontSettings = new FontSettings();
            //fontSettings.SubstitutionSettings.DefaultFontSubstitution.Enabled = true;
            //fontSettings.SetFontsFolder(Path.Combine("Fonts"), true);
            //fontSettings.FallbackSettings.LoadNotoFallbackSettings();
            //fontSettings.SubstitutionSettings.FontInfoSubstitution.Enabled = false;
            //doc.FontSettings = fontSettings;

            MemoryStream stream = new MemoryStream();
            //doc.Save(stream, SaveFormat.Pdf);
            PdfSaveOptions saveOptions = new PdfSaveOptions { OptimizeOutput = true };
            doc.Save(stream, saveOptions);
            return File(stream.GetBuffer(), "application/pdf", "DONHANG" + item.Code + ".pdf");
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.ToString());
            return NotFound();
        }
    }

    [HttpGet("export-excel/{id}")]
    public async Task<IActionResult> ExportCTNK(Guid id)
    {
        try
        {
            var rp = await _mediator.Send(new OrderGetDataPrint(id));
            for (int i = 0; i < rp?.Details?.Count; i++)
            {
                rp.Details[i].SortOrder = i + 1;
            }
            string path = _environment.ContentRootPath;
            string file = path + "/Template/DONHANG.xlsx";
            string newfile = path + "/Print/" + rp.Code + ".xlsx";

            string[] deleteFiles = Directory.GetFiles(path + "/Print/");
            foreach (string deleteFile in deleteFiles)
            {
                System.IO.File.Delete(deleteFile);
            }
            System.IO.File.Copy(file, newfile);

            string[] CellReferenceArray = new string[] { "A", "B", "C", "D", "E", "F", "G", "H", "I", "J", "K", "L", "M", "N", "O", "P", "Q", "R", "S", "T", "U", "V", "W", "X", "Y", "Z" };

            string CellTextTemplate = "Table, TableA, Merge";

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
                    CellReference cellTableA = cellTemplate.First(x => x.Name == "TableA");
                    CellReference _cellTable = Utilities.Utilities.FindOneCellByReference(file, cellTable.Text, true);
                    CellReference _cellTableA = Utilities.Utilities.FindOneCellByReference(file, cellTableA.Text, true);

                    List<CellReference> cellBeforeTable = copycellExistText.Where(x => x.RowIndex < _cellTable.RowIndex).ToList();
                    List<CellReference> cellCenterTable = copycellExistText.Where(x => x.RowIndex > _cellTable.RowIndex && x.RowIndex < _cellTableA.RowIndex).ToList();
                    List<CellReference> cellAfterTable = copycellExistText.Where(x => x.RowIndex > _cellTableA.RowIndex).ToList();

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
                                text = text + Utilities.Utilities<OrderDto>.GetValueByKeyGeneral(rp, property);
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
                        foreach (var item in rp.Details)
                        {
                            for (int i = 0; i < columns.Count(); i++)
                            {
                                var c = listBody[i];
                                var text = Utilities.Utilities<OrderProductDto>.GetValueByKeyGeneral(item, columns[i]);
                                Utilities.Utilities.SetDataCellGeneral(c, text, indexTable, shareStringPart, worksheetPart);
                            }
                            indexTable += 1;
                        }
                    }

                    if (cellCenterTable.Count() > 0)
                    {
                        foreach (CellReference cell in cellCenterTable)
                        {
                            var row = cell.RowIndex + Convert.ToUInt32(rp.Details.Count() == 0 ? 0 : rp.Details.Count() - 1);
                            if (!cell.Text.Contains("^"))
                            {
                                Utilities.Utilities.SetDataCell(cell.Text, cell.ColumnCharacter, row, cell.StyleIndex, shareStringPart, worksheetPart);
                            }
                            else
                            {
                                var text = cell.Text.Split("^")[0];
                                var property = cell.Text.Split("^")[1];
                                text = text + Utilities.Utilities<OrderDto>.GetValueByKeyGeneral(rp, property);
                                Utilities.Utilities.SetDataCell(text, cell.ColumnCharacter, row, cell.StyleIndex, shareStringPart, worksheetPart);
                            }
                        }
                    }

                    if (cellAfterTable.Count() > 0)
                    {
                        foreach (CellReference cell in cellAfterTable)
                        {
                            var row = cell.RowIndex + Convert.ToUInt32(rp.Details.Count() == 0 ? 0 : rp.Details.Count() - 1) + Convert.ToUInt32(rp.OrderServiceAdd.Count() == 0 ? 0 : rp.OrderServiceAdd.Count()) - 1;
                            if (!cell.Text.Contains("^"))
                            {
                                Utilities.Utilities.SetDataCell(cell.Text, cell.ColumnCharacter, row, cell.StyleIndex, shareStringPart, worksheetPart);
                            }
                            else
                            {
                                var text = cell.Text.Split("^")[0];
                                var property = cell.Text.Split("^")[1];
                                text = text + Utilities.Utilities<OrderDto>.GetValueByKeyGeneral(rp, property);
                                Utilities.Utilities.SetDataCell(text, cell.ColumnCharacter, row, cell.StyleIndex, shareStringPart, worksheetPart);
                            }
                        }
                    }

                    List<CellReference> cellFooterTable = cellTemplate.Where(x => x.Name == "TableA").ToList();
                    if (cellFooterTable.Count > 0)
                    {
                        foreach (var cellFooter in cellFooterTable)
                        {
                            CellReference _cellFooter = Utilities.Utilities.FindOneCellByReference(file, cellFooter.Text, true);
                            string[] columns = _cellFooter.Text.Split(",");
                            UInt32 indexTable = _cellFooter.RowIndex;
                            UInt32 indexRowTo = _cellFooter.RowIndex + Convert.ToUInt32(rp.Details.Count() == 0 ? 0 : rp.Details.Count() - 1);
                            int indexCharTable = CellReferenceArray.IndexOf(_cellFooter.ColumnCharacter);
                            List<CellReference> listBody = new List<CellReference>();
                            for (int i = 0; i < columns.Count(); i++)
                            {
                                var c = Utilities.Utilities.FindOneCellByReference(file, CellReferenceArray[i + indexCharTable] + indexTable.ToString(), false);
                                listBody.Add(c);
                            }
                            foreach (var item in rp.OrderServiceAdd)
                            {
                                for (int i = 0; i < columns.Count(); i++)
                                {
                                    var text = Utilities.Utilities<OrderServiceAddPrintDto>.GetValueByKey(item, columns[i]);
                                    var c = listBody[i];
                                    if (text != null)
                                    {
                                        Utilities.Utilities.SetDataCell(c, text ?? "", indexRowTo, shareStringPart, worksheetPart);
                                    }
                                    else
                                    {
                                        var s = columns[i];
                                        c.Text = "";
                                        Utilities.Utilities.SetDataCell(c, s, indexRowTo, shareStringPart, worksheetPart);
                                    }
                                }
                                indexRowTo = indexRowTo + 1;
                            }
                        }
                    }

                    List<CellReference> cellMerges = cellTemplate.Where(x => x.Name == "Merge").ToList();
                    if (cellMerges.Count > 0)
                    {
                        foreach (var cellMerge in cellMerges)
                        {
                            var range = cellMerge.Text;
                            var rangeColumn = range.Split(":")[0]; //A-F
                            var rangeRow = range.Split(":")[1]; //14-18
                            var colStart = rangeColumn.Split("-")[0]; //A
                            var colEnd = rangeColumn.Split("-")[1]; //F
                            var rowStart = UInt32.Parse(rangeRow.Split("-")[0]) + Convert.ToUInt32(rp.Details.Count() == 0 ? 0 : rp.Details.Count() - 1); //14 + SL chi tiết
                            var rowEnd = UInt32.Parse(rangeRow.Split("-")[1]) + Convert.ToUInt32(rp.Details.Count() == 0 ? 0 : rp.Details.Count() - 1) + +Convert.ToUInt32(rp.OrderServiceAdd.Count() == 0 ? 0 : rp.OrderServiceAdd.Count() - 1);  //18 + SL chi tiết + SL service

                            for (UInt32 i = rowStart; i <= rowEnd; i++)
                            {
                                var characterLeft = colStart;
                                var numberLeft = i.ToString();
                                var characterRight = colEnd;
                                var numberRight = i.ToString();
                                var cellStart = colStart + numberLeft;

                                var cellStartTo = characterLeft + numberLeft;
                                var cellEndTo = characterRight + numberRight;
                                var rangeTo = cellStartTo + ":" + cellEndTo;
                                CellReference ce = Utilities.Utilities.FindOneCellByReference(file, cellStart, true);
                                Utilities.Utilities.MergeCellInRange(worksheet, rangeTo, characterLeft, numberLeft, characterRight, numberRight);
                            }
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
        catch (Exception)
        {
            return Ok("NOt");
        }
    }

    [HttpPost("manage-payment")]
    public async Task<IActionResult> ManagePaymentOrder([FromBody] ManagePaymentRequest request)
    {
        var paymentInvoice = request.PaymentInvoice?.Select(x => new PaymentInvoiceDto()
        {
            Id = x.Id ?? Guid.NewGuid(),
            Type = x.Type,
            Code = x.Code,
            OrderId = request.Id,
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

        var cmd = new ManagePaymentOrderCommand(
            request.Id,
            request.PaymentStatus,
            paymentInvoice
            );

        var result = await _mediator.SendCommand(cmd);

        return Ok(result);
    }

    [HttpPost("manage-service")]
    public async Task<IActionResult> ManageServiceOrder([FromBody] ManageServiceOrderRequest request)
    {
        var orderService = request.OrderServiceAdd?.Select(x => new OrderServiceAddDto()
        {
            Id = !String.IsNullOrEmpty(x.Id) ? new Guid(x.Id) : null,
            OrderId = request.Id,
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

        var cmd = new ManageServiceOrderCommand(
            request.Id,
            orderService
            );

        var result = await _mediator.SendCommand(cmd);

        return Ok(result);
    }

    [HttpPost("note")]
    public async Task<IActionResult> Note([FromBody] NoteOrderRequest request)
    {
        var orderTracking = request.OrderTracking?.Select(x => new OrderTrackingDto()
        {
            Id = x.Id ?? Guid.NewGuid(),
            OrderId = request.Id,
            Name = x.Name,
            Status = x.Status,
            Description = x.Description,
            Image = x.Image,
            TrackingDate = x.TrackingDate
        }).ToList();

        var cmd = new NoteOrderCommand(
            request.Id,
            orderTracking
            );

        var result = await _mediator.SendCommand(cmd);

        return Ok(result);
    }

    [HttpGet("paging-detail")]
    public async Task<IActionResult> PagingDetail([FromQuery] OrderProductPagingRequest request)
    {
        var query = new OrderPagingDetailQuery(
            request.Keyword ?? "",
            request.ToBaseQuery(),
            request.Order ?? "",
            request.PageNumber,
            request.PageSize
            );
        var result = await _mediator.Send(query);
        return Ok(result);
    }

    [HttpGet("excel-template")]
    public async Task<IActionResult> GetExcelTemplate()
    {
        var rs = await _mediator.Send(new OrderExportTemplateQuery());
        if (rs != null)
        {
            return File(rs.ToArray(), "application/xlsx");
        }
        return Ok(rs);
    }

    [HttpGet("get-related-by-id/{id}")]
    public async Task<IActionResult> GetRelated(Guid id)
    {
        var queryById = new OrderQueryRelatedById(id);
        var result = await _mediator.Send(queryById);
        return Ok(result);
    }

    [HttpPost("validate-excel")]
    public async Task<IActionResult> ValidateExcel([FromForm] ValidateExcelOrder request)
    {
        List<ValidateField> listField = new List<ValidateField>()
        {
            new ValidateField(){Field="productCode", IndexColumn= request.ProductCode},
            new ValidateField(){Field="productName", IndexColumn= request.ProductName},
            new ValidateField(){Field="unitCode", IndexColumn= request.UnitCode},
            new ValidateField(){Field="unitName", IndexColumn= request.UnitName},
            new ValidateField(){Field="unitPrice", IndexColumn= request.UnitPrice},
            new ValidateField(){Field="quantity", IndexColumn= request.Quantity},
            new ValidateField(){Field="tax", IndexColumn= request.Tax},
            new ValidateField(){Field="discountPercent", IndexColumn= request.DiscountPercent},
            new ValidateField(){Field="note", IndexColumn= request.Note},
        };

        var data = new ValidateExcelOrderQuery(request.File,
                                                    request.SheetId,
                                                    request.HeaderRow,
                                                    listField);
        var result = await _mediator.Send(data);
        return Ok(result);
    }

    [HttpPut("fetch-tracking-delivery")]
    public async Task<ValidationResult> FetchTrackingDelivery([FromBody] FetchTrackingDeliveryRequest request, CancellationToken cancellationToken)
    {
        var ev = new OrderItemFetchDomesticDeliveryCommand()
        {
            Id = request.Id,
            CreatedBy = _context.GetUserId(),
            CreatedName = _context.UserName,
            Data = _context.Data,
            Data_Zone = _context.Data_Zone,
            Tenant = _context.Tenant
        };

        return await _mediator.SendCommand(ev, cancellationToken);

    }
}
