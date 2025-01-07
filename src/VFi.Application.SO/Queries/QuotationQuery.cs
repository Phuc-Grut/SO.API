using System.ComponentModel;
using DocumentFormat.OpenXml.Packaging;
using DocumentFormat.OpenXml.Spreadsheet;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Newtonsoft.Json;
using VFi.Application.SO.DTOs;
using VFi.Domain.SO.Interfaces;
using VFi.Domain.SO.Models;
using VFi.Infra.SO.Repository;
using VFi.NetDevPack.FopExpression;
using VFi.NetDevPack.Queries;
using VFi.NetDevPack.Utilities;

namespace VFi.Application.SO.Queries;

public class SendTransactionQueryByQuotation : IQuery<IEnumerable<SendTransactionDto>>
{
    public SendTransactionQueryByQuotation(string? keyword, string? quotation)
    {
        Quotation = quotation;
        Keyword = keyword;
    }

    public string? Keyword { get; set; }
    public string? Quotation { get; set; }
}

public class QuotationQuerySendConfigCombobox : IQuery<IEnumerable<SendConfigComboboxDto>>
{
    public QuotationQuerySendConfigCombobox()
    {
    }
}

public class QuotationQuerySendTemplateCombobox : IQuery<IEnumerable<SendTemplateComboboxDto>>
{

    public QuotationQuerySendTemplateCombobox()
    {
    }
}

public class QuotationEmailBuilderQuery : IQuery<EmailBodyDto>
{
    public QuotationEmailBuilderQuery()
    {
    }
    public string Template { get; set; }
    public string Subject { get; set; }
    public string JBody { get; set; }
}
public class QuotationQueryComboBox : IQuery<IEnumerable<QuotationListBoxDto>>
{
    public QuotationQueryComboBox(string? keyword, QuotationParams queryParams, int pagesize, int pageindex)
    {
        QueryParams = queryParams;
        Keyword = keyword;
        PageSize = pagesize;
        PageIndex = pageindex;
    }
    public QuotationParams? QueryParams { get; set; }
    public string? Keyword { get; set; }
    public int PageSize { get; set; }
    public int PageIndex { get; set; }
}

public class QuotationQueryById : IQuery<QuotationDto>
{
    public QuotationQueryById()
    {
    }

    public QuotationQueryById(Guid id)
    {
        QuotationId = id;
    }

    public Guid QuotationId { get; set; }
}
public class QuotationGetDataPrint : IQuery<QuotationPrintDto>
{
    public QuotationGetDataPrint()
    {
    }

    public QuotationGetDataPrint(Guid id)
    {
        QuotationId = id;
    }

    public Guid QuotationId { get; set; }
}

public class QuotationPagingQuery : FopQuery, IQuery<PagedResult<List<QuotationDto>>>
{
    public QuotationPagingQuery(string? keyword, int? status, string? employeeId, string filter, string order, int pageNumber, int pageSize)
    {
        Keyword = keyword;
        Filter = filter;
        Order = order;
        PageNumber = pageNumber;
        PageSize = pageSize;
        Status = status;
        EmployeeId = employeeId;
    }
    public string? EmployeeId { get; set; }
}
public class QuotationExportTemplateQuery : IQuery<MemoryStream>
{
    public QuotationExportTemplateQuery()
    {

    }
}
public class ValidateExcelQuotationQuery : IQuery<List<QuotationValidateDto>>
{
    public ValidateExcelQuotationQuery(IFormFile file, string sheetId, int headerRow, List<ValidateField> listField)
    {
        File = file;
        ListField = listField;
        SheetId = sheetId;
        HeaderRow = headerRow;
    }
    public string SheetId { get; set; } = null!;
    public IFormFile File { get; set; }
    public int HeaderRow { get; set; }
    public List<ValidateField> ListField { get; set; }
}
public class QuotationQuery : IQueryHandler<QuotationQueryComboBox, IEnumerable<QuotationListBoxDto>>,
                                        IQueryHandler<QuotationQueryById, QuotationDto>,
                                        IQueryHandler<QuotationPagingQuery, PagedResult<List<QuotationDto>>>,
                                        IQueryHandler<QuotationGetDataPrint, QuotationPrintDto>,
                                        IQueryHandler<ValidateExcelQuotationQuery, List<QuotationValidateDto>>,
                                        IQueryHandler<QuotationExportTemplateQuery, MemoryStream>,
                                        IQueryHandler<QuotationQuerySendConfigCombobox, IEnumerable<SendConfigComboboxDto>>,
                                        IQueryHandler<QuotationQuerySendTemplateCombobox, IEnumerable<SendTemplateComboboxDto>>,
                                        IQueryHandler<QuotationEmailBuilderQuery, EmailBodyDto>,
                                        IQueryHandler<SendTransactionQueryByQuotation, IEnumerable<SendTransactionDto>>
{
    private readonly IQuotationRepository _repository;
    private readonly IQuotationTermRepository _quotationTermRepository;
    private readonly IPIMRepository _PIMRepository;
    private readonly IExportTemplateRepository _exportTemplateRepository;
    private readonly IEmailMasterRepository _emailMasterRepository;
    public QuotationQuery(IQuotationRepository respository,
                            IQuotationTermRepository quotationTermRepository,
                            IPIMRepository PIMRepository,
                            IExportTemplateRepository exportTemplateRepository,
                            IEmailMasterRepository emailMasterRepository
        )
    {
        _repository = respository;
        _quotationTermRepository = quotationTermRepository;
        _PIMRepository = PIMRepository;
        _exportTemplateRepository = exportTemplateRepository;
        _emailMasterRepository = emailMasterRepository;
    }

    public async Task<QuotationDto> Handle(QuotationQueryById request, CancellationToken cancellationToken)
    {
        var data = await _repository.GetById(request.QuotationId);
        var inventory = new List<INVENTORY_DETAIL_BY_LISTID>();
        var productPrice = new List<SP_GET_PRODUCT_PRICE_BY_LISTID>();
        var listId = new List<string>();
        var listProductId = new List<Guid?>();
        var i = 1;
        var source = data.OrderProduct.Select(x => x.ProductId).Distinct();
        foreach (var x in source)
        {
            listProductId.Add(x);
            if (listProductId.Count() == 50)
            {
                listId.Add(string.Join(",", listProductId));
                listProductId.RemoveRange(0, 50);
            }
            if (i == source.Count() && listProductId.Count() > 0)
            {
                listId.Add(string.Join(",", listProductId));
            }
            i++;
        }
        foreach (var o in listId)
        {
            var dataInventory = await _PIMRepository.GetInventoryDetail(o);
            foreach (var x in dataInventory)
            {
                var rs = new INVENTORY_DETAIL_BY_LISTID()
                {
                    Id = x.Id,
                    WarehouseId = x.WarehouseId,
                    Code = x.Code,
                    Name = x.Name,
                    ProductId = x.ProductId,
                    StockQuantity = x.StockQuantity,
                    ReservedQuantity = x.ReservedQuantity,
                    PlannedQuantity = x.PlannedQuantity
                };
                inventory.Add(rs);
            }
            var dataPrice = await _PIMRepository.GetProductPrice(o);
            foreach (var x in dataPrice)
            {
                var rs = new SP_GET_PRODUCT_PRICE_BY_LISTID()
                {
                    Id = x.Id,
                    Code = x.Code,
                    Name = x.Name,
                    Price = x.Price,
                    UnitId = x.UnitId,
                    Currency = x.Currency
                };
                productPrice.Add(rs);
            }
        }
        var termName = "";
        if (data.QuotationTermId != null)
        {
            var term = await _quotationTermRepository.GetById((Guid)data.QuotationTermId);
            termName = term?.Name;
        }
        var result = new QuotationDto()
        {
            Id = data.Id,
            Code = data.Code,
            Name = data.Name,
            Description = data.Description,
            Status = data.Status,
            CustomerId = data.CustomerId,
            CustomerCode = data.CustomerCode,
            CustomerName = data.CustomerName,
            Email = data.Email,
            Phone = data.Phone,
            Address = data.Address,
            StoreId = data.StoreId,
            StoreCode = data.StoreCode,
            StoreName = data.StoreName,
            ChannelId = data.ChannelId,
            ChannelName = data.ChannelName,
            DeliveryNote = data.DeliveryNote,
            DeliveryName = data.DeliveryName,
            DeliveryAddress = data.DeliveryAddress,
            DeliveryCountry = data.DeliveryCountry,
            DeliveryProvince = data.DeliveryProvince,
            DeliveryDistrict = data.DeliveryDistrict,
            DeliveryWard = data.DeliveryWard,
            DeliveryStatus = data.DeliveryStatus,
            IsBill = data.IsBill,
            BillAddress = data.BillAddress,
            BillCountry = data.BillCountry,
            BillProvince = data.BillProvince,
            BillDistrict = data.BillDistrict,
            BillWard = data.BillWard,
            BillStatus = data.BillStatus,
            ShippingMethodId = data.ShippingMethodId,
            ShippingMethodCode = data.ShippingMethodCode,
            ShippingMethodName = data.ShippingMethodName,
            DeliveryMethodId = data.DeliveryMethodId,
            DeliveryMethodCode = data.DeliveryMethodCode,
            DeliveryMethodName = data.DeliveryMethodName,
            ExpectedDate = data.ExpectedDate,
            Currency = data.Currency,
            CurrencyName = data.CurrencyName,
            Calculation = data.Calculation,
            ExchangeRate = data.ExchangeRate,
            PriceListId = data.PriceListId,
            PriceListName = data.PriceListName,
            RequestQuoteId = data.RequestQuoteId,
            RequestQuoteCode = data.RequestQuoteCode,
            ContractId = data.ContractId,
            SaleOrderId = data.SaleOrderId,
            QuotationTermId = data.QuotationTermId,
            TermName = termName,
            QuotationTermContent = data.QuotationTermContent,
            Date = data.Date,
            ExpiredDate = data.ExpiredDate,
            GroupEmployeeId = data.GroupEmployeeId,
            GroupEmployeeName = data.GroupEmployeeName,
            AccountId = data.AccountId,
            AccountName = data.AccountName,
            TypeDiscount = data.TypeDiscount,
            DiscountRate = data.DiscountRate,
            TypeCriteria = data.TypeCriteria,
            AmountDiscount = data.AmountDiscount,
            Note = data.Note,
            ApproveDate = data.ApproveDate,
            ApproveBy = data.ApproveBy,
            ApproveComment = data.ApproveComment,
            CreatedBy = data.CreatedBy,
            CreatedDate = data.CreatedDate,
            UpdatedBy = data.UpdatedBy,
            UpdatedDate = data.UpdatedDate,
            CreatedByName = data.CreatedByName,
            UpdatedByName = data.UpdatedByName,
            OldId = data.OldId,
            OldCode = data.OldCode,
            File = JsonConvert.DeserializeObject<List<FileDto>>(string.IsNullOrEmpty(data.File) ? "" : data.File),
            OrderProduct = data.OrderProduct.Select(x => new OrderProductDto(data.Calculation, data.ExchangeRate)
            {
                Id = x.Id,
                OrderId = x.OrderId,
                ContractId = x.ContractId,
                ContractName = x.ContractName,
                QuotationId = x.QuotationId,
                QuotationName = x.QuotationName,
                ProductId = x.ProductId,
                ProductCode = x.ProductCode,
                ProductName = x.ProductName,
                ProductImage = x.ProductImage,
                ProductPrice = productPrice.Where(y => y.Id == x.ProductId).FirstOrDefault()?.Price,
                ProductCurrency = productPrice.Where(y => y.Id == x.ProductId).FirstOrDefault()?.Currency,
                Origin = x.Origin,
                WarehouseId = x.WarehouseId,
                WarehouseCode = x.WarehouseCode,
                WarehouseName = x.WarehouseName,
                StockQuantity = !String.IsNullOrEmpty(x.WarehouseCode) ? inventory.Where(y => y.ProductId == x.ProductId && y.Code == x.WarehouseCode).FirstOrDefault()?.StockQuantity : inventory.Where(y => y.ProductId == x.ProductId).Sum(y => y.StockQuantity),
                TotalStockQuantity = inventory.Where(y => y.ProductId == x.ProductId).Sum(y => y.StockQuantity),
                ReservedQuantity = !String.IsNullOrEmpty(x.WarehouseCode) ? inventory.Where(y => y.ProductId == x.ProductId && y.Code == x.WarehouseCode).FirstOrDefault()?.ReservedQuantity : inventory.Where(y => y.ProductId == x.ProductId).Sum(y => y.ReservedQuantity),
                PlannedQuantity = !String.IsNullOrEmpty(x.WarehouseCode) ? inventory.Where(y => y.ProductId == x.ProductId && y.Code == x.WarehouseCode).FirstOrDefault()?.PlannedQuantity : inventory.Where(y => y.ProductId == x.ProductId).Sum(y => y.PlannedQuantity),
                Currency = data.Currency,
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
                QuantityReturned = x.QuantityReturned,
                DisplayOrder = x.DisplayOrder,
                CreatedBy = x.CreatedBy,
                CreatedDate = x.CreatedDate,
                UpdatedBy = x.UpdatedBy,
                UpdatedDate = x.UpdatedDate,
                CreatedByName = x.CreatedByName,
                UpdatedByName = x.UpdatedByName,
                DeliveryStatus = x.DeliveryStatus,
                DeliveryQuantity = x.DeliveryQuantity,
                IsDiscount = (x.DiscountPercent > 0 || x.AmountDiscount > 0),
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
            }).OrderBy(x => x.DisplayOrder).ToList(),
            OrderServiceAdd = data.OrderServiceAdd.Select(x => new OrderServiceAddDto()
            {
                Id = x.Id,
                OrderId = x.OrderId,
                QuotationId = x.QuotationId,
                ServiceAddId = x.ServiceAddId,
                ServiceAddName = x.ServiceAddName,
                Price = x.Price,
                Currency = x.Currency,
                Calculation = x.Calculation,
                Status = x.Status,
                Note = x.Note,
                CreatedDate = x.CreatedDate,
                CreatedBy = x.CreatedBy,
                UpdatedBy = x.UpdatedBy,
                UpdatedDate = x.UpdatedDate,
                UpdatedByName = x.UpdatedByName,
                CreatedByName = x.CreatedByName,
                ExchangeRate = x.ExchangeRate,
                DisplayOrder = x.DisplayOrder
            }).OrderBy(x => x.DisplayOrder).ToList()
        };
        return result;
    }

    public async Task<PagedResult<List<QuotationDto>>> Handle(QuotationPagingQuery request, CancellationToken cancellationToken)
    {
        var response = new PagedResult<List<QuotationDto>>();
        var fopRequest = FopExpressionBuilder<Quotation>.Build(request.Filter, request.Order, request.PageNumber, request.PageSize);
        var filter = new Dictionary<string, object>();
        if (request.Status != null)
            filter.Add("status", request.Status.ToString());
        if (!string.IsNullOrEmpty(request.EmployeeId))
            filter.Add("employeeId", request.EmployeeId);
        var (datas, count) = await _repository.Filter(request.Keyword, filter, fopRequest);

        var data = datas.Select(x => new QuotationDto()
        {
            Id = x.Id,
            Code = x.Code,
            Date = x.Date,
            ExpiredDate = x.ExpiredDate,
            CustomerId = x.CustomerId,
            CustomerName = x.CustomerName,
            Phone = x.Phone,
            Email = x.Email,
            AccountId = x.AccountId,
            AccountName = x.AccountName,
            Status = x.Status,
            ApproveComment = x.ApproveComment,
            RequestQuoteCode = x.RequestQuoteCode,
            Currency = x.Currency,
            CurrencyName = x.CurrencyName,
            TotalAmountTax = x.TotalAmountTax
        }).ToList();
        response.Items = data;
        response.TotalCount = count;
        response.PageNumber = request.PageNumber;
        response.PageSize = request.PageSize;
        return response;
    }

    public async Task<IEnumerable<QuotationListBoxDto>> Handle(QuotationQueryComboBox request, CancellationToken cancellationToken)
    {
        var filter = new Dictionary<string, object>();
        if (request.QueryParams.Status != null)
        {
            filter.Add("status", request.QueryParams.Status);
        }
        if (request.QueryParams.Date != null)
        {
            filter.Add("date", request.QueryParams.Date);
        }
        if (request.QueryParams.CustomerId != null)
        {
            filter.Add("customerId", request.QueryParams.CustomerId);
        }
        if (request.QueryParams.IsContract != null)
        {
            filter.Add("isContract", request.QueryParams.IsContract.ToString());
        }
        if (request.QueryParams.IsOrder != null)
        {
            filter.Add("isOrder", request.QueryParams.IsOrder.ToString());
        }
        if (request.QueryParams.FromDate != null)
        {
            filter.Add("fromDate", request.QueryParams.FromDate);
        }
        if (request.QueryParams.ToDate != null)
        {
            filter.Add("toDate", request.QueryParams.ToDate);
        }
        if (request.QueryParams.Currency != null)
        {
            filter.Add("currency", request.QueryParams.Currency);
        }
        var Contracts = await _repository.GetListBox(request.Keyword, filter, request.PageSize, request.PageIndex);
        var result = Contracts.Select(x => new QuotationListBoxDto()
        {
            Id = x.Id,
            Code = x.Code,
            Name = x.Name,
            Date = x.Date,
            ContractId = x.ContractId,
            ContractCode = x.Contract?.FirstOrDefault()?.Code,
            ContractName = x.Contract?.FirstOrDefault()?.Name,
            AccountName = x.AccountName,
            CustomerId = x.CustomerId,
            CustomerCode = x.CustomerCode,
            CustomerName = x.CustomerName,
            Currency = x.Currency,
            CurrencyName = x.CurrencyName,
            PriceListId = x.PriceListId,
            PriceListName = x.PriceListName,
            TypeDiscount = x.TypeDiscount,
            DiscountRate = x.DiscountRate,
            AmountDiscount = x.AmountDiscount,
            TypeCriteria = x.TypeCriteria,
            Status = x.Status,
            TotalAmountTax = x.TotalAmountTax
        });
        return result;
    }
    private List<QuotationDetailPrintDto> CalculatorDiscount(Quotation obj)
    {
        List<QuotationDetailPrintDto> list = new List<QuotationDetailPrintDto>();
        decimal? discountTotal = 0;
        if (obj.TypeDiscount == 0)
        {
            discountTotal = obj.OrderProduct.Sum(x => x.Quantity * x.UnitPrice) * (decimal)(obj.DiscountRate ?? 0) / 100;
        }
        else
        {
            discountTotal = obj.AmountDiscount;
        }
        decimal? amountTotal = obj.OrderProduct.Sum(x => (x.Quantity * x.UnitPrice) - (decimal)(x.DiscountTotal ?? 0));
        decimal? quantityTotal = obj.OrderProduct.Sum(x => x.Quantity);

        decimal? totalAmountDiscount = 0;
        decimal? amountNoTax = 0;
        decimal? amountTax = 0;
        decimal? totalAmountTax = 0;
        foreach (var a in obj.OrderProduct.OrderBy(x => x.DisplayOrder))
        {
            totalAmountDiscount = a.DiscountAmountDistribution + (a.AmountDiscount ?? (a.Quantity * a.UnitPrice * (decimal?)a.DiscountPercent / 100));
            amountNoTax = (a.Quantity * a.UnitPrice) - totalAmountDiscount;
            amountTax = amountNoTax * (decimal)(a.TaxRate ?? 0) / 100;
            totalAmountTax = amountNoTax + amountTax;

            list.Add(new QuotationDetailPrintDto()
            {
                Id = a.Id,
                ProductCode = a.ProductCode,
                ProductName = a.ProductName,
                UnitName = a.UnitName,
                Tax = a.Tax,
                Quantity = FormatNumber.AddPeriod(a.Quantity, obj.Currency != "VND" ? 2 : 0),
                UnitPrice = FormatNumber.AddPeriod(a.UnitPrice, obj.Currency != "VND" ? 2 : 0),
                AmountNoVat = FormatNumber.AddPeriod(a.Quantity * a.UnitPrice, obj.Currency != "VND" ? 2 : 0),
                AmountNoDiscount = a.Quantity * a.UnitPrice,
                DiscountAmountDistribution = a.DiscountAmountDistribution,
                TotalAmountDiscount = totalAmountDiscount,
                AmountNoTax = amountNoTax,
                AmountTax = amountTax,
                TotalAmountTax = totalAmountTax
            });
        }
        return list;
    }
    class SetAddress
    {
        public string? Address { get; set; }
        public string? Ward { get; set; }
        public string? District { get; set; }
        public string? Province { get; set; }
        public string? Country { get; set; }
    }
    private string setAddress(SetAddress data)
    {
        List<string?> address = new List<string?>();
        if (!string.IsNullOrEmpty(data.Address))
        {
            address.Add(data.Address);
        }
        if (!string.IsNullOrEmpty(data.Ward))
        {
            address.Add(data.Ward);
        }
        if (!string.IsNullOrEmpty(data.District))
        {
            address.Add(data.District);
        }
        if (!string.IsNullOrEmpty(data.Province))
        {
            address.Add(data.Province);
        }
        if (!string.IsNullOrEmpty(data.Country))
        {
            address.Add(data.Country);
        }
        return string.Join(", ", address);
    }
    public async Task<QuotationPrintDto> Handle(QuotationGetDataPrint request, CancellationToken cancellationToken)
    {
        var data = await _repository.GetById(request.QuotationId);
        List<QuotationDetailPrintDto> detail = CalculatorDiscount(data);
        var address = new SetAddress();
        address.Address = data.DeliveryAddress;
        address.Ward = data.DeliveryWard;
        address.District = data.DeliveryDistrict;
        address.Province = data.DeliveryProvince;
        address.Country = data.DeliveryCountry;
        var result = new QuotationPrintDto()
        {
            Id = data.Id,
            Code = data.Code,
            CustomerName = data.CustomerName,
            Address = setAddress(address),
            Date = Convert.ToDateTime(data.Date).ToString("dd/MM/yyyy"),
            ExpiredDate = Convert.ToDateTime(data.ExpiredDate).ToString("dd/MM/yyyy"),
            Note = data.Description,
            Currency = data.Currency,
            CurrencyName = data.CurrencyName,
            CreatedDateString = "Ngày " + Convert.ToDateTime(data.CreatedDate).ToString("dd") + " tháng " + Convert.ToDateTime(data.CreatedDate).ToString("MM") + " năm " + Convert.ToDateTime(data.CreatedDate).ToString("yyyy"),
            PrintedDateString = "Ngày " + Convert.ToDateTime(DateTime.Now).ToString("dd") + " tháng " + Convert.ToDateTime(DateTime.Now).ToString("MM") + " năm " + Convert.ToDateTime(DateTime.Now).ToString("yyyy"),
            TotalAmountNoTax = FormatNumber.AddPeriod(detail?.Sum(x => x.AmountNoDiscount), data.Currency != "VND" ? 2 : 0),
            TotalDiscount = FormatNumber.AddPeriod(detail?.Sum(x => x.TotalAmountDiscount), data.Currency != "VND" ? 2 : 0),
            TotalTaxValue = FormatNumber.AddPeriod(detail?.Sum(x => x.AmountTax), data.Currency != "VND" ? 2 : 0),
            OrderServiceAdd = data.OrderServiceAdd.Select(x => new OrderServiceAddPrintDto()
            {
                ServiceAddName = x.ServiceAddName,
                Price = x.Price,
                PriceString = FormatNumber.AddPeriod(x.Price, data.Currency != "VND" ? 2 : 0)
            }).OrderBy(x => x.SortOrder).ToList(),
            TotalAmount = FormatNumber.AddPeriod(detail?.Sum(x => x.TotalAmountTax) + data.OrderServiceAdd.Sum(x => x.Price), data.Currency != "VND" ? 2 : 0),
            TotalAmountText = Utilities.NumberToText_Currency((double)(detail?.Sum(x => x.TotalAmountTax) + data.OrderServiceAdd.Sum(x => x.Price)), data.Currency),
            Details = detail
        };
        return result;
    }
    public async Task<MemoryStream> Handle(QuotationExportTemplateQuery request, CancellationToken cancellationToken)
    {
        var contentBytes = await _exportTemplateRepository.GetTemplate("MAU_IMPORT_CHI_TIET_BAO_GIA");

        MemoryStream memoryStream = new();

        memoryStream.SetLength(0);

        memoryStream.Write(contentBytes, 0, contentBytes.Length);

        string[] CellReferenceArray = new string[] { "A", "B", "C", "D", "E", "F", "G", "H", "I", "J", "K", "L", "M", "N", "O", "P", "Q", "R", "S", "T", "U", "V", "W", "X", "Y", "Z", "AA", "AB", "AC", "AD", "AE", "AF", "AG", "AH", "AI", "AJ", "AK", "AL", "AM", "AN", "AO", "AP", "AQ", "AR", "AS", "AT", "AU", "AV", "AW", "AX", "AY", "AZ" };
        using (SpreadsheetDocument spreadSheet = SpreadsheetDocument.Open(memoryStream, true))
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
        }
        return memoryStream;
    }
    public static int? GetColumnIndex(string cellRef)
    {
        if (string.IsNullOrEmpty(cellRef))
            return null;
        cellRef = cellRef.ToUpper();
        int columnIndex = -1;
        int mulitplier = 1;
        foreach (char c in cellRef.ToCharArray().Reverse())
        {
            if (char.IsLetter(c))
            {
                columnIndex += mulitplier * ((int)c - 64);
                mulitplier = mulitplier * 26;
            }
        }
        return columnIndex;
    }
    public async Task<List<QuotationValidateDto>> Handle(ValidateExcelQuotationQuery request, CancellationToken cancellationToken)
    {
        List<QuotationValidateDto> result = new List<QuotationValidateDto>();
        try
        {
            var taxlist = await _PIMRepository.GetListBox();
            using (MemoryStream stream = new MemoryStream())
            {
                request.File.CopyTo(stream);
                using (SpreadsheetDocument doc = SpreadsheetDocument.Open(stream, false))
                {
                    WorkbookPart workbookPart = doc.WorkbookPart;
                    Worksheet theWorksheet = ((WorksheetPart)workbookPart.GetPartById(request.SheetId)).Worksheet;
                    SheetData? thesheetdata = (SheetData?)theWorksheet?.GetFirstChild<SheetData>();
                    for (int i = (request.HeaderRow); thesheetdata is not null && i < thesheetdata.ChildElements.Count; i++)
                    {
                        Row thecurrentRow = (Row)thesheetdata.ChildElements[i];
                        QuotationValidateDto QuotationValidateDto = new QuotationValidateDto()
                        {
                            Row = thecurrentRow.RowIndex
                        };
                        foreach (ValidateField field in request.ListField)
                        {
                            if (field.IndexColumn >= 0 && field.IndexColumn < thecurrentRow.ChildElements.Count)
                            {
                                Cell thecurrentCell = thecurrentRow.Elements<Cell>().FirstOrDefault(cell => GetColumnIndex(cell.CellReference) == field.IndexColumn);
                                if (thecurrentCell != null)
                                {
                                    string cellValue = "";
                                    if (thecurrentCell.DataType != null && thecurrentCell.DataType == CellValues.SharedString)
                                    {
                                        int id;
                                        if (Int32.TryParse(thecurrentCell.InnerText, out id))
                                        {
                                            SharedStringItem item = workbookPart.SharedStringTablePart.SharedStringTable.Elements<SharedStringItem>().ElementAt(id);
                                            if (item != null && item.Text != null)
                                            {
                                                cellValue = item.Text.Text;
                                            }
                                        }
                                    }
                                    else
                                    {
                                        cellValue = (thecurrentCell.CellValue == null) ? thecurrentCell.InnerText : thecurrentCell.CellValue.Text;
                                    }
                                    switch (field.Field)
                                    {
                                        case "productCode":
                                            //QuotationValidateDto.ProductCode = cellValue;
                                            if (String.IsNullOrEmpty(cellValue))
                                            {
                                                QuotationValidateDto.Errors.Add("productCode isRequired");
                                            }
                                            else
                                            {
                                                QuotationValidateDto.ProductCode = cellValue;

                                            }
                                            break;

                                        case "productName":
                                            if (!String.IsNullOrEmpty(cellValue) && String.IsNullOrEmpty(QuotationValidateDto.ProductCode))
                                            {
                                                QuotationValidateDto.ProductName = cellValue;
                                                QuotationValidateDto.Errors.Add($"ProductName {cellValue} invalid");
                                            }
                                            break;
                                        case "unitCode":
                                            if (String.IsNullOrEmpty(cellValue))
                                            {
                                                QuotationValidateDto.Errors.Add("unitCode isRequired");
                                            }
                                            else
                                            {
                                                QuotationValidateDto.UnitCode = cellValue;
                                            }
                                            break;

                                        case "unitName":

                                            if (!String.IsNullOrEmpty(cellValue) && String.IsNullOrEmpty(QuotationValidateDto.UnitCode))
                                            {
                                                QuotationValidateDto.Errors.Add($"Unit name {cellValue} invalid");
                                            }
                                            break;
                                        case "quantity":
                                            try
                                            {
                                                double quantity;
                                                if (!String.IsNullOrEmpty(cellValue))
                                                {
                                                    if (!double.TryParse(cellValue, out quantity))
                                                    {
                                                        QuotationValidateDto.Errors.Add("quantity notincorrectformat");
                                                    }
                                                    else
                                                    {
                                                        if (quantity <= 0)
                                                        {
                                                            QuotationValidateDto.Errors.Add("quantity mustbegreaterthan0");
                                                            cellValue = "0";
                                                        }
                                                    }

                                                }

                                            }
                                            catch (Exception ex)
                                            {
                                                QuotationValidateDto.Errors.Add("" + ex + "");
                                            }

                                            QuotationValidateDto.Quantity = ((cellValue ?? "") == "" ? "0" : cellValue).Replace(",", ".");
                                            break;
                                        case "unitPrice":
                                            try
                                            {
                                                double amount;
                                                if (!String.IsNullOrEmpty(cellValue))
                                                {
                                                    if (!double.TryParse(cellValue, out amount))
                                                    {
                                                        QuotationValidateDto.Errors.Add("UnitPrice notincorrectformat");
                                                    }
                                                    else
                                                    {
                                                        if (amount < 0)
                                                        {
                                                            QuotationValidateDto.Errors.Add("UnitPrice mustbegreaterorequalthan0");
                                                            cellValue = "0";
                                                        }
                                                    }
                                                }
                                            }
                                            catch (Exception ex)
                                            {
                                                QuotationValidateDto.Errors.Add("" + ex + "");
                                            }
                                            QuotationValidateDto.UnitPrice = cellValue.Replace(",", ".");
                                            break;
                                        case "discountPercent":
                                            var a = cellValue.Replace(',', '.');
                                            double dp;
                                            if (!String.IsNullOrEmpty(a))
                                            {
                                                if (!double.TryParse(a, out dp))
                                                {
                                                    QuotationValidateDto.Errors.Add($"UnitPriceExpected {a} notincorrectformat");

                                                }
                                                else
                                                {
                                                    QuotationValidateDto.DiscountPercent = dp.ToString();
                                                }
                                            }
                                            break;
                                        case "tax":
                                            try
                                            {
                                                if (!string.IsNullOrEmpty(cellValue))
                                                {
                                                    var taxcheck = taxlist.Where(x => x.Name == cellValue || x.Code == cellValue).FirstOrDefault();
                                                    if (taxcheck != null)
                                                    {
                                                        QuotationValidateDto.TaxCategoryId = taxcheck.Id.ToString();
                                                        QuotationValidateDto.Tax = taxcheck.Name;
                                                        QuotationValidateDto.TaxCode = taxcheck.Code;
                                                        QuotationValidateDto.TaxRate = taxcheck.Rate.ToString();
                                                    }
                                                    else
                                                    {
                                                        QuotationValidateDto.Errors.Add("tax incorrect");
                                                        QuotationValidateDto.TaxCategoryId = cellValue;
                                                    }
                                                }
                                                else
                                                {
                                                    QuotationValidateDto.TaxCategoryId = null;
                                                }
                                            }
                                            catch (Exception ex)
                                            {
                                                QuotationValidateDto.Errors.Add("" + ex + "");
                                            }

                                            break;
                                        case "note":
                                            QuotationValidateDto.Note = cellValue.Replace("\n", ". ");
                                            break;
                                        default:
                                            break;
                                    }
                                }
                            }
                        }

                        if (!String.IsNullOrEmpty(QuotationValidateDto.ProductCode))
                        {
                            result.Add(QuotationValidateDto);
                        }
                    }
                }
            }
        }
        catch (Exception ex) { }
        var resultCode = result.Where(x => !string.IsNullOrEmpty(x.ProductCode)).Select(x => x.ProductCode);
        var getlistunti = await _PIMRepository.GetUnitPaging(1, 10000, null, null);

        if (resultCode is not null && resultCode.Count() > 0)
        {
            var getproduct = await _PIMRepository.GetProductByListCode(resultCode.ToList());// get code true
            var e = getproduct.Select(x => x.Code);

            var getProductContai = result.Where(x => e.Contains(x.ProductCode)).ToList();
            var getProductError = result.Where(x => !e.Contains(x.ProductCode)).ToList();

            if (getProductContai != null && getProductContai.Count() > 0)
            {
                foreach (var i in getProductContai)
                {
                    var a = result.Where(x => x.ProductCode == i.ProductCode.Trim()).FirstOrDefault();
                    var getP = getproduct.Where(x => x.Code == i.ProductCode.Trim()).FirstOrDefault();
                    if (a != null)
                    {
                        a.ProductId = getP.Id;
                        a.ProductName = getP.Name;
                        a.ProductCode = getP.Code;
                        a.UnitType = getP.UnitType;
                        a.StockQuantity = getP.StockQuantity;
                        var unit = getlistunti.FirstOrDefault(x => x.Code.Equals(a.UnitCode) && x.GroupUnitCode == a.UnitType);
                        if (unit is not null)
                        {
                            a.UnitCode = unit.Code;
                            a.UnitName = unit.Name;
                            a.UnitId = unit.Id;
                        }
                        else
                        {
                            a.Errors.Add($"Unit code {a.UnitCode} invalid");
                            a.UnitCode = a.UnitCode;
                            a.UnitName = a.UnitName;

                        }
                    }
                }
            }

            if (getProductError != null && getProductError.Count() > 0)
            {
                foreach (var i in getProductError)
                {
                    var a = result.Where(x => x.ProductCode == i.ProductCode.Trim()).FirstOrDefault();

                    if (a != null)
                    {
                        a.Errors.Add($"{a.ProductCode} invalid");
                    }
                }
            }
        }
        return result;
    }
    public async Task<IEnumerable<SendConfigComboboxDto>> Handle(QuotationQuerySendConfigCombobox request, CancellationToken cancellationToken)
    {
        var data = await _emailMasterRepository.GetListboxSendConfig();

        var result = data.Select(x => new SendConfigComboboxDto
        {
            Key = x.Code,
            Value = x.Id,
            Label = x.Name,
            From = x.From,
            FromName = x.FromName
        });

        return result;
    }

    public async Task<IEnumerable<SendTemplateComboboxDto>> Handle(QuotationQuerySendTemplateCombobox request, CancellationToken cancellationToken)
    {
        var data = await _emailMasterRepository.GetListboxSendTemplate();

        var result = data.Select(x => new SendTemplateComboboxDto
        {
            Key = x.Code,
            Value = x.Id,
            Label = x.Name,
            Description = x.Description,
            Status = x.Status,
            Type = x.Type
        });

        return result;
    }

    public async Task<EmailBodyDto> Handle(QuotationEmailBuilderQuery request, CancellationToken cancellationToken)
    {
        var data = await _emailMasterRepository.EmailBuilder(request.Subject, request.JBody, request.Template);

        var result = new EmailBodyDto
        {
            Subject = data.Subject,
            Body = data.Body,
            BodyText = data.BodyText
        };

        return result;
    }

    public async Task<IEnumerable<SendTransactionDto>> Handle(SendTransactionQueryByQuotation request, CancellationToken cancellationToken)
    {
        var filter = new Dictionary<string, string?> {
            { "keyword", request.Keyword },
            { "quotation", request.Quotation }
        };

        var data = await _emailMasterRepository.GetListSendTransaction(filter);
        var result = data.Select(x => new SendTransactionDto
        {
            Id = x.Id,
            Order = x.Order,
            Subject = x.Subject,
            SendDate = x.SendDate,
            Open = x.Open,
            Click = x.Click,
        });

        return result;
    }
}
