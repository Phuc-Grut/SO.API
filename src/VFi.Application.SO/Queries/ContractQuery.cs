using DocumentFormat.OpenXml.Packaging;
using DocumentFormat.OpenXml.Spreadsheet;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using VFi.Application.SO.DTOs;
using VFi.Domain.SO.Interfaces;
using VFi.Domain.SO.Models;
using VFi.Infra.SO.Context;
using VFi.NetDevPack.FopExpression;
using VFi.NetDevPack.Queries;

namespace VFi.Application.SO.Queries;

public class ContractQueryById : IQuery<ContractDto>
{
    public ContractQueryById()
    {
    }

    public ContractQueryById(Guid id)
    {
        Id = id;
    }

    public Guid Id { get; set; }
}

public class ContractQueryComboBox : IQuery<IEnumerable<ContractListBoxDto>>
{
    public ContractQueryComboBox(string? keyword, ContractParams queryParams, int pagesize, int pageindex)
    {
        QueryParams = queryParams;
        Keyword = keyword;
        PageSize = pagesize;
        PageIndex = pageindex;
    }
    public ContractParams? QueryParams { get; set; }
    public string? Keyword { get; set; }
    public int PageSize { get; set; }
    public int PageIndex { get; set; }
}
public class ContractQueryCheckCode : IQuery<bool>
{

    public ContractQueryCheckCode(string code)
    {
        Code = code;
    }

    public string Code { get; set; }
}

public class ContractPagingQuery : FopQuery, IQuery<PagedResult<List<ContractDto>>>
{
    public ContractPagingQuery(string? keyword, int? status, string? employeeId, string filter, string order, int pageNumber, int pageSize)
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
public class ContractExportTemplateQuery : IQuery<MemoryStream>
{
    public ContractExportTemplateQuery()
    {

    }

}
public class ValidateExcelContractQuery : IQuery<List<ContractValidateDto>>
{
    public ValidateExcelContractQuery(IFormFile file, string sheetId, int headerRow, List<ValidateField> listField)
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
// lấy dữ liệu ,khai báo
public class ContractQueryHandler : IQueryHandler<ContractQueryComboBox, IEnumerable<ContractListBoxDto>>,
                                         IQueryHandler<ContractQueryCheckCode, bool>,
                                         IQueryHandler<ContractQueryById, ContractDto>,
                                          IQueryHandler<ContractExportTemplateQuery, MemoryStream>,
                                         IQueryHandler<ContractPagingQuery, PagedResult<List<ContractDto>>>,
    IQueryHandler<ValidateExcelContractQuery, List<ContractValidateDto>>
{
    private readonly IContractRepository _repository;
    protected readonly SqlCoreContext Db;
    private readonly IPIMRepository _PIMRepository;
    private readonly IExportTemplateRepository _exportTemplateRepository;
    public ContractQueryHandler(
        IContractRepository contractRespository,
        SqlCoreContext context,
        IPIMRepository PIMRepository,
              IExportTemplateRepository exportTemplateRepository
        )
    {
        _repository = contractRespository;
        Db = context;
        _PIMRepository = PIMRepository;
        _exportTemplateRepository = exportTemplateRepository;
    }
    public async Task<bool> Handle(ContractQueryCheckCode request, CancellationToken cancellationToken)
    {
        var data = await _repository.GetByCode(request.Code);
        if (data is null)
        {
            return false;
        }
        return true;
    }

    public async Task<ContractDto> Handle(ContractQueryById request, CancellationToken cancellationToken)
    {
        var item = await _repository.GetById(request.Id);
        var inventory = new List<INVENTORY_DETAIL_BY_LISTID>();
        var productPrice = new List<SP_GET_PRODUCT_PRICE_BY_LISTID>();
        var listId = new List<string>();
        var listProductId = new List<Guid?>();
        var i = 1;
        var source = item.OrderProduct.Select(x => x.ProductId).Distinct();
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
            //var dataPrice = await _PIMRepository.GetProductPrice(o);
            //foreach (var x in dataPrice)
            //{
            //    var rs = new SP_GET_PRODUCT_PRICE_BY_LISTID()
            //    {
            //        Id = x.Id,
            //        Code = x.Code,
            //        Name = x.Name,
            //        Price = x.Price,
            //        UnitId = x.UnitId,
            //        Currency = x.Currency
            //    };
            //    productPrice.Add(rs);
            //}
        }
        var result = new ContractDto()
        {
            Id = item.Id,
            Code = item.Code,
            Name = item.Name,
            ContractTypeId = item.ContractTypeId,
            ContractTypeCode = item.ContractType?.Code,
            ContractTypeName = item.ContractTypeName,
            QuotationId = item.QuotationId,
            QuotationName = item.QuotationName,
            OrderId = item.OrderId,
            OrderCode = item.OrderCode,
            StartDate = item.StartDate,
            EndDate = item.EndDate,
            SignDate = item.SignDate,
            CustomerId = item.CustomerId,
            CustomerCode = item.CustomerCode,
            CustomerName = item.CustomerName,
            Country = item.Country,
            Province = item.Province,
            District = item.District,
            Ward = item.Ward,
            Address = item.Address,
            Currency = item.Currency,
            CurrencyName = item.CurrencyName,
            Calculation = item.Calculation,
            ExchangeRate = item.ExchangeRate,
            Status = item.Status,
            TypeDiscount = item.TypeDiscount,
            DiscountRate = item.DiscountRate,
            TypeCriteria = item.TypeCriteria,
            AmountDiscount = item.AmountDiscount,
            AccountName = item.AccountName,
            GroupEmployeeId = item.GroupEmployeeId,
            GroupEmployeeName = item.GroupEmployeeName,
            AccountId = item.AccountId,
            ContractTermId = item.ContractTermId,
            ContractTermName = item.ContractTermName,
            ContractTermContent = item.ContractTermContent,
            ApproveDate = item.ApproveDate,
            ApproveBy = item.ApproveBy,
            ApproveComment = item.ApproveComment,
            PaymentDueDate = item.PaymentDueDate,
            DeliveryDate = item.DeliveryDate,
            Buyer = item.Buyer,
            Saler = item.Saler,
            AmountLiquidation = item.AmountLiquidation,
            LiquidationDate = item.LiquidationDate,
            LiquidationReason = item.LiquidationReason,
            Description = item.Description,
            Note = item.Note,
            File = JsonConvert.DeserializeObject<List<FileDto>>(string.IsNullOrEmpty(item.File) ? "" : item.File),
            HasPreviousContract = item.HasPreviousContract,
            Paid = item.Paid,
            Received = item.Received,
            CreatedBy = item.CreatedBy,
            CreatedDate = item.CreatedDate,
            UpdatedBy = item.UpdatedBy,
            UpdatedDate = item.UpdatedDate,
            CreatedByName = item.CreatedByName,
            UpdatedByName = item.UpdatedByName,
            OrderProduct = item.OrderProduct.Select(x => new OrderProductDto(item.Calculation, item.ExchangeRate)
            {
                Id = x.Id,
                OrderId = x.OrderId,
                OrderCode = x.OrderCode,
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
                CreatedBy = x.CreatedBy,
                CreatedDate = x.CreatedDate,
                UpdatedBy = x.UpdatedBy,
                UpdatedDate = x.UpdatedDate,
                CreatedByName = x.CreatedByName,
                UpdatedByName = x.UpdatedByName,
                DeliveryStatus = x.DeliveryStatus,
                DeliveryQuantity = x.DeliveryQuantity,
                IsDiscount = (x.DiscountPercent > 0 || x.AmountDiscount > 0),
                RemainQty = (double?)(x.Quantity - (Db.OrderProduct.Where(y => y.Order.ContractId == item.Id && y.Order.Status != 9 && y.ProductId == x.ProductId).Sum(y => y.Quantity))),
                SellQty = (double?)Db.OrderProduct.Where(y => y.Order.ContractId == item.Id && y.Order.Status != 9 && y.ProductId == x.ProductId).Sum(y => y.Quantity),
                StockQuantity = inventory.Where(y => y.ProductId == x.ProductId).Sum(y => y.StockQuantity),
                ReservedQuantity = inventory.Where(y => y.ProductId == x.ProductId).Sum(y => y.ReservedQuantity),
                PlannedQuantity = inventory.Where(y => y.ProductId == x.ProductId).Sum(y => y.PlannedQuantity),
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
            }).OrderBy(x => x.DisplayOrder).ToList(),
        };

        return result;
    }

    public async Task<PagedResult<List<ContractDto>>> Handle(ContractPagingQuery request, CancellationToken cancellationToken)
    {
        var response = new PagedResult<List<ContractDto>>();

        var fopRequest = FopExpressionBuilder<Contract>.Build(request.Filter, request.Order, request.PageNumber, request.PageSize);

        var filterListBox = new Dictionary<string, object>();
        if (request.Status != null)
            filterListBox.Add("status", request.Status.ToString());
        if (!string.IsNullOrEmpty(request.EmployeeId))
            filterListBox.Add("employeeId", request.EmployeeId);

        var (Contracts, count) = await _repository.Filter(request.Keyword, filterListBox, fopRequest);

        var data = Contracts.Select(obj => new ContractDto()
        {
            Id = obj.Id,
            Code = obj.Code,
            ContractTypeName = obj.ContractTypeName,
            CustomerCode = obj.CustomerCode,
            CustomerName = obj.CustomerName,
            SignDate = obj.SignDate,
            StartDate = obj.StartDate,
            EndDate = obj.EndDate,
            AccountName = obj.AccountName,
            Status = obj.Status,
            ApproveComment = obj.ApproveComment,
            Currency = obj.Currency,
            CurrencyName = obj.CurrencyName,
            TotalAmountTax = obj.TotalAmountTax,
            CreateOderStatus = obj.OrderProduct.Where(x => x.Quantity - (Db.OrderProduct.Where(y => y.Order.ContractId == obj.Id && y.Order.Status != 3 && y.ProductId == x.ProductId).Sum(y => y.Quantity)) > 0).Count() > 0 ? 1 : 0
        }).ToList();
        response.Items = data;
        response.TotalCount = count;
        response.PageNumber = request.PageNumber;
        response.PageSize = request.PageSize;
        return response;
    }

    public async Task<IEnumerable<ContractListBoxDto>> Handle(ContractQueryComboBox request, CancellationToken cancellationToken)
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
        var Contracts = await _repository.GetListBox(request.Keyword, filter, request.PageSize, request.PageIndex);
        var result = Contracts.Select(x => new ContractListBoxDto()
        {
            Id = x.Id,
            Code = x.Code,
            Name = x.Name,
            SignDate = x.SignDate,
            QuotationId = x.QuotationId,
            QuotationCode = x.Quotation?.Code,
            QuotationName = x.QuotationName,
            AccountName = x.AccountName,
            CustomerId = x.CustomerId,
            CustomerCode = x.CustomerCode,
            CustomerName = x.CustomerName,
            TypeDiscount = x.TypeDiscount,
            DiscountRate = x.DiscountRate,
            AmountDiscount = x.AmountDiscount,
            TypeCriteria = x.TypeCriteria,
            Status = x.Status,
            Currency = x.Currency,
            CurrencyName = x.CurrencyName,
            TotalAmountTax = x.TotalAmountTax
        });
        return result;
    }
    public async Task<MemoryStream> Handle(ContractExportTemplateQuery request, CancellationToken cancellationToken)
    {
        var contentBytes = await _exportTemplateRepository.GetTemplate("MAU_IMPORT_CHI_TIET_HOP_DONG");

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

    public async Task<List<ContractValidateDto>> Handle(ValidateExcelContractQuery request, CancellationToken cancellationToken)
    {
        List<ContractValidateDto> result = new List<ContractValidateDto>();
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
                        ContractValidateDto ContractValidateDto = new ContractValidateDto()
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
                                            if (String.IsNullOrEmpty(cellValue))
                                            {
                                                ContractValidateDto.Errors.Add($"productCode {cellValue} invalid");
                                            }
                                            else
                                            {
                                                ContractValidateDto.ProductCode = cellValue;
                                            }
                                            ContractValidateDto.ProductCode = cellValue;

                                            break;
                                        case "productName":
                                            if (!String.IsNullOrEmpty(cellValue) && String.IsNullOrEmpty(ContractValidateDto.ProductCode))
                                            {
                                                ContractValidateDto.Errors.Add("productName donotmatch");
                                                ContractValidateDto.ProductName = cellValue;
                                            }
                                            break;
                                        case "unitCode":
                                            if (String.IsNullOrEmpty(cellValue))
                                            {
                                                ContractValidateDto.Errors.Add($"unitCode {cellValue} invalid");
                                            }
                                            else
                                            {
                                                ContractValidateDto.UnitCode = cellValue;
                                            }
                                            break;
                                        case "unitName":
                                            if (!String.IsNullOrEmpty(cellValue) && String.IsNullOrEmpty(ContractValidateDto.UnitCode))
                                            {
                                                ContractValidateDto.UnitName = cellValue;
                                            }
                                            break;
                                        case "unitPrice":
                                            try
                                            {
                                                double amount;
                                                if (!String.IsNullOrEmpty(cellValue))
                                                {
                                                    if (!double.TryParse(cellValue, out amount))
                                                    {
                                                        ContractValidateDto.Errors.Add($"UnitPrice {cellValue} notincorrectformat");
                                                    }
                                                }
                                            }

                                            catch
                                            {
                                                ContractValidateDto.Errors.Add("UnitPrice notincorrectformat");
                                            }

                                            ContractValidateDto.UnitPrice = cellValue.Replace(',', '.');
                                            ;

                                            break;
                                        case "quantity":
                                            try
                                            {
                                                double quantityRequest;
                                                if (!String.IsNullOrEmpty(cellValue))
                                                {
                                                    if (!double.TryParse(cellValue, out quantityRequest))
                                                    {
                                                        ContractValidateDto.Errors.Add($"Quantity {cellValue} notincorrectformat");
                                                    }
                                                    /*var cellV = double.Parse(cellValue, CultureInfo.InvariantCulture);
                                                    var stockQuantity = double.Parse(ContractValidateDto.StockQuantity ?? "0", CultureInfo.InvariantCulture);
                                                    if (cellV > stockQuantity)
                                                    {
                                                        ContractValidateDto.Errors.Add($"Quantity {cellValue} > {stockQuantity}");
                                                        ContractValidateDto.Quantity = cellValue.Replace(',', '.');

                                                    }*/
                                                    else
                                                    {
                                                        ContractValidateDto.Quantity = cellValue.Replace(',', '.');

                                                    }
                                                }
                                            }
                                            catch
                                            {
                                                ContractValidateDto.Errors.Add($"Quantity {cellValue} notincorrectformat");
                                            }
                                            ContractValidateDto.Quantity = cellValue.Replace(',', '.');

                                            break;

                                        case "discountPercent":
                                            var a = cellValue.Replace(',', '.');
                                            double dp;
                                            if (!String.IsNullOrEmpty(a))
                                            {
                                                if (!double.TryParse(a, out dp))
                                                {
                                                    ContractValidateDto.Errors.Add($"discountPercent {a} notincorrectformat");

                                                }
                                                else
                                                {
                                                    ContractValidateDto.DiscountPercent = dp.ToString();
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
                                                        ContractValidateDto.TaxCategoryId = taxcheck.Id.ToString();
                                                        ContractValidateDto.Tax = taxcheck.Name;
                                                        ContractValidateDto.TaxCode = taxcheck.Code;
                                                        ContractValidateDto.TaxRate = taxcheck.Rate.ToString();
                                                    }
                                                    else
                                                    {
                                                        ContractValidateDto.Errors.Add("tax incorrect");
                                                        ContractValidateDto.TaxCategoryId = cellValue;
                                                    }
                                                }
                                                else
                                                {
                                                    ContractValidateDto.TaxCategoryId = null;
                                                }
                                            }
                                            catch (Exception ex)
                                            {
                                                ContractValidateDto.Errors.Add("" + ex + "");
                                            }

                                            break;
                                        case "note":
                                            ContractValidateDto.Note = cellValue;
                                            break;
                                        default:
                                            break;
                                    }
                                }
                            }
                        }

                        if (!String.IsNullOrEmpty(ContractValidateDto.ProductCode))
                        {
                            result.Add(ContractValidateDto);
                        }
                    }
                }
            }
        }
        catch (Exception ex)
        {
        }
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
}
