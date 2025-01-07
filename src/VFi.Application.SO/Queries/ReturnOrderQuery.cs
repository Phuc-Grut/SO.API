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
using VFi.NetDevPack.Context;
using VFi.NetDevPack.FopExpression;
using VFi.NetDevPack.Queries;

namespace VFi.Application.SO.Queries;


public class ReturnOrderQueryComboBox : IQuery<IEnumerable<ComboBoxDto>>
{
    public ReturnOrderQueryComboBox(int? status)
    {
        Status = status;
    }
    public int? Status { get; set; }
}

public class ReturnOrderQueryById : IQuery<ReturnOrderDto>
{
    public ReturnOrderQueryById()
    {
    }

    public ReturnOrderQueryById(Guid id)
    {
        ReturnOrderId = id;
    }

    public Guid ReturnOrderId { get; set; }
}

public class ReturnOrderPagingQuery : FopQuery, IQuery<PagedResult<List<ReturnOrderDto>>>
{
    public ReturnOrderPagingQuery(string? keyword, int? status, string? employeeId, string? filter, string? order, int pageNumber, int pageSize)
    {
        Keyword = keyword;
        Status = status;
        EmployeeId = employeeId;
        Filter = filter;
        Order = order;
        PageNumber = pageNumber;
        PageSize = pageSize;
    }
    public string? EmployeeId { get; set; }
}

public class ReturnOrderExportTemplateQuery : IQuery<MemoryStream>
{
    public ReturnOrderExportTemplateQuery()
    {

    }

}
public class ValidateExcelReturnOrderQuery : IQuery<List<ReturnOrderValidateDto>>
{
    public ValidateExcelReturnOrderQuery(IFormFile file, string sheetId, int headerRow, List<ValidateField> listField)
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
public class ReturnOrderQuery : IQueryHandler<ReturnOrderQueryComboBox, IEnumerable<ComboBoxDto>>,
                                        IQueryHandler<ReturnOrderQueryById, ReturnOrderDto>,
                                        IQueryHandler<ReturnOrderPagingQuery, PagedResult<List<ReturnOrderDto>>>,
    IQueryHandler<ReturnOrderExportTemplateQuery, MemoryStream>,
    IQueryHandler<ValidateExcelReturnOrderQuery, List<ReturnOrderValidateDto>>
{
    private readonly IReturnOrderRepository _repository;
    private readonly IExportTemplateRepository _exportTemplateRepository;
    private readonly IPIMRepository _pimRepository;

    public ReturnOrderQuery(
                            IReturnOrderRepository respository,
                            IExportTemplateRepository exportTemplateRepository,
                            IPIMRepository PimRepository)
    {
        _repository = respository;
        _exportTemplateRepository = exportTemplateRepository;
        _pimRepository = PimRepository;

    }

    public async Task<ReturnOrderDto> Handle(ReturnOrderQueryById request, CancellationToken cancellationToken)
    {
        var data = await _repository.GetById(request.ReturnOrderId);
        var result = new ReturnOrderDto()
        {
            Id = data.Id,
            Code = data.Code,
            CustomerId = data.CustomerId,
            CustomerCode = data.CustomerCode,
            CustomerName = data.CustomerName,
            OrderId = data.OrderId,
            OrderCode = data.OrderCode,
            Address = data.Address,
            Country = data.Country,
            Province = data.Province,
            District = data.District,
            Ward = data.Ward,
            Description = data.Description,
            Status = data.Status,
            WarehouseCode = data.WarehouseCode,
            WarehouseName = data.WarehouseName,
            WarehouseId = data.WarehouseId,
            ReturnDate = data.ReturnDate,
            AccountId = data.AccountId,
            AccountName = data.AccountName,
            CurrencyId = data.CurrencyId,
            Currency = data.Currency,
            CurrencyName = data.CurrencyName,
            Calculation = data.Calculation,
            ExchangeRate = data.ExchangeRate,
            TypeDiscount = data.TypeDiscount,
            DiscountRate = data.DiscountRate,
            TypeCriteria = data.TypeCriteria,
            AmountDiscount = data.AmountDiscount,
            PaymentStatus = data.PaymentStatus,
            ApproveBy = data.ApproveBy,
            ApproveDate = data.ApproveDate,
            ApproveByName = data.ApproveByName,
            ApproveComment = data.ApproveComment,
            File = JsonConvert.DeserializeObject<List<FileDto>>(string.IsNullOrEmpty(data.File) ? "" : data.File),
            CreatedByName = data.CreatedByName,
            UpdatedByName = data.UpdatedByName,
            CreatedBy = data.CreatedBy,
            CreatedDate = data.CreatedDate,
            UpdatedBy = data.UpdatedBy,
            UpdatedDate = data.UpdatedDate,
            ReturnOrderProduct = data.ReturnOrderProduct.Select(x => new ReturnOrderProductDto()
            {
                Id = x.Id,
                ReturnOrderId = x.ReturnOrderId,
                OrderProductId = x.OrderProductId,
                ProductId = x.ProductId,
                ProductCode = x.ProductCode,
                ProductName = x.ProductName,
                QuantityReturn = x.QuantityReturn,
                UnitPrice = x.UnitPrice,
                UnitType = x.UnitType,
                UnitCode = x.UnitCode,
                UnitName = x.UnitName,
                QuantityRemain = (x.OrderProduct?.Quantity ?? 0) - (x.OrderProduct?.QuantityReturned ?? 0),
                DiscountAmountDistribution = x.DiscountAmountDistribution,
                DiscountType = x.DiscountType,
                DiscountPercent = x.DiscountPercent,
                AmountDiscount = x.AmountDiscount,
                TaxRate = x.TaxRate,
                Tax = x.Tax,
                TaxCode = x.TaxCode,
                ReasonId = x.ReasonId,
                ReasonName = x.ReasonName,
                WarehouseId = x.WarehouseId,
                WarehouseName = x.WarehouseName,
                OrderId = x.OrderProduct?.Order?.Id,
                OrderCode = x.OrderProduct?.Order?.Code,
                DisplayOrder = x.DisplayOrder,
                CreatedBy = x.CreatedBy,
                CreatedDate = x.CreatedDate,
                UpdatedBy = x.UpdatedBy,
                UpdatedDate = x.UpdatedDate,
                CreatedByName = x.CreatedByName,
                UpdatedByName = x.UpdatedByName
            }).OrderBy(x => x.DisplayOrder).ToList(),
            PaymentInvoice = data.PaymentInvoice.Select(x => new PaymentInvoiceDto()
            {
                Id = x.Id,
                Type = x.Type,
                Code = x.Code,
                ReturnOrderId = x.ReturnOrderId,
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
                Bank = x.BankName + " - " + x.BankNumber,
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
                CreatedBy = x.CreatedBy,
                CreatedDate = x.CreatedDate,
                UpdatedBy = x.UpdatedBy,
                UpdatedDate = x.UpdatedDate,
                UpdatedByName = x.UpdatedByName,
                CreatedByName = x.CreatedByName,
                CustomerId = x.CustomerId,
                CustomerName = x.CustomerName
            }).ToList()
        };
        return result;
    }

    public async Task<PagedResult<List<ReturnOrderDto>>> Handle(ReturnOrderPagingQuery request, CancellationToken cancellationToken)
    {
        var response = new PagedResult<List<ReturnOrderDto>>();

        var fopRequest = FopExpressionBuilder<ReturnOrder>.Build(request.Filter, request.Order, request.PageNumber, request.PageSize);
        var filter = new Dictionary<string, object>();
        if (request.Status != null)
            filter.Add("status", request.Status.ToString());
        if (!string.IsNullOrEmpty(request.EmployeeId))
            filter.Add("employeeId", request.EmployeeId);
        var (datas, count) = await _repository.Filter(request.Keyword, filter, fopRequest);

        var data = datas.Select(item => new ReturnOrderDto()
        {
            Id = item.Id,
            Code = item.Code,
            CustomerId = item.CustomerId,
            CustomerCode = item.CustomerCode,
            CustomerName = item.CustomerName,
            OrderId = item.OrderId,
            OrderCode = item.OrderCode,
            Address = item.Address,
            Country = item.Country,
            Province = item.Province,
            District = item.District,
            Ward = item.Ward,
            Description = item.Description,
            Status = item.Status,
            WarehouseCode = item.WarehouseCode,
            WarehouseName = item.WarehouseName,
            WarehouseId = item.WarehouseId,
            ReturnDate = item.ReturnDate,
            AccountId = item.AccountId,
            AccountName = item.AccountName,
            CurrencyId = item.CurrencyId,
            Currency = item.Currency,
            CurrencyName = item.CurrencyName,
            Calculation = item.Calculation,
            ExchangeRate = item.ExchangeRate,
            ApproveBy = item.ApproveBy,
            ApproveDate = item.ApproveDate,
            ApproveByName = item.ApproveByName,
            ApproveComment = item.ApproveComment,
            CreatedByName = item.CreatedByName,
            UpdatedByName = item.UpdatedByName,
            CreatedBy = item.CreatedBy,
            CreatedDate = item.CreatedDate,
            UpdatedBy = item.UpdatedBy,
            UpdatedDate = item.UpdatedDate,
        }).ToList();
        response.Items = data;
        response.TotalCount = count;
        response.PageNumber = request.PageNumber;
        response.PageSize = request.PageSize;
        return response;
    }

    public async Task<IEnumerable<ComboBoxDto>> Handle(ReturnOrderQueryComboBox request, CancellationToken cancellationToken)
    {
        var priceList = await _repository.GetListCbx(request.Status);
        var result = priceList.Select(x => new ComboBoxDto()
        {
            Key = x.Code,
            Value = x.Id,
            Label = x.Code
        });
        return result;
    }

    public async Task<MemoryStream> Handle(ReturnOrderExportTemplateQuery request, CancellationToken cancellationToken)
    {
        var contentBytes = await _exportTemplateRepository.GetTemplate("MAU_IMPORT_CHI_TIET_TRA_LAI_BAN");

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
    public async Task<List<ReturnOrderValidateDto>> Handle(ValidateExcelReturnOrderQuery request, CancellationToken cancellationToken)
    {
        List<ReturnOrderValidateDto> result = new List<ReturnOrderValidateDto>();
        try
        {
            var taxlist = await _pimRepository.GetListBox();

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
                        ReturnOrderValidateDto ReturnOrderValidateDto = new ReturnOrderValidateDto()
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
                                                ReturnOrderValidateDto.Errors.Add($"productCode {cellValue} invalid");
                                            }
                                            else
                                            {
                                                ReturnOrderValidateDto.ProductCode = cellValue;
                                            }


                                            break;
                                        case "productName":
                                            if (!String.IsNullOrEmpty(cellValue) && String.IsNullOrEmpty(ReturnOrderValidateDto.ProductCode))
                                            {
                                                ReturnOrderValidateDto.Errors.Add($"productName {cellValue} invalid");
                                                ReturnOrderValidateDto.ProductName = cellValue;
                                            }
                                            break;
                                        case "unitCode":
                                            if (String.IsNullOrEmpty(cellValue))
                                            {
                                                ReturnOrderValidateDto.Errors.Add($"unitCode {cellValue} invalid");
                                            }
                                            else
                                            {
                                                ReturnOrderValidateDto.UnitCode = cellValue;
                                            }
                                            break;
                                        case "unitName":
                                            if (!String.IsNullOrEmpty(cellValue) && String.IsNullOrEmpty(ReturnOrderValidateDto.UnitCode))
                                            {
                                                ReturnOrderValidateDto.UnitName = cellValue;
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
                                                        ReturnOrderValidateDto.Errors.Add($"UnitPrice {cellValue} notincorrectformat");
                                                    }
                                                }
                                            }

                                            catch
                                            {
                                                ReturnOrderValidateDto.Errors.Add("UnitPrice notincorrectformat");
                                            }

                                            ReturnOrderValidateDto.UnitPrice = cellValue.Replace(',', '.');
                                            ;

                                            break;
                                        case "quantityReturn":
                                            try
                                            {
                                                double quantityRequest;
                                                if (!String.IsNullOrEmpty(cellValue))
                                                {
                                                    if (!double.TryParse(cellValue, out quantityRequest))
                                                    {
                                                        ReturnOrderValidateDto.Errors.Add($"QuantityReturn {cellValue} notincorrectformat");
                                                    }
                                                    /*var cellV = double.Parse(cellValue, CultureInfo.InvariantCulture);
                                                    var stockQuantity = double.Parse(ReturnOrderValidateDto.StockQuantity ?? "0", CultureInfo.InvariantCulture);
                                                    if (cellV > stockQuantity) {
                                                        ReturnOrderValidateDto.Errors.Add($"Quantityrequest {cellValue} > {stockQuantity}");
                                                        ReturnOrderValidateDto.QuantityRequest = cellValue.Replace(',', '.');

                                                    }*/
                                                }
                                            }
                                            catch
                                            {
                                                ReturnOrderValidateDto.Errors.Add($"QuantityReturn {cellValue} notincorrectformat");
                                            }
                                            ReturnOrderValidateDto.QuantityReturn = cellValue.Replace(',', '.');

                                            break;

                                        case "discountPercent":
                                            var a = cellValue.Replace(',', '.');
                                            double dp;
                                            if (!String.IsNullOrEmpty(a))
                                            {
                                                if (!double.TryParse(a, out dp))
                                                {
                                                    ReturnOrderValidateDto.Errors.Add($"discountPercent {a} notincorrectformat");

                                                }
                                                else
                                                {
                                                    ReturnOrderValidateDto.DiscountPercent = dp.ToString();
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
                                                        ReturnOrderValidateDto.TaxCategoryId = taxcheck.Id.ToString();
                                                        ReturnOrderValidateDto.Tax = taxcheck.Name;
                                                        ReturnOrderValidateDto.TaxCode = taxcheck.Code;
                                                        ReturnOrderValidateDto.TaxRate = taxcheck.Rate.ToString();
                                                    }
                                                    else
                                                    {
                                                        ReturnOrderValidateDto.Errors.Add("tax incorrect");
                                                        ReturnOrderValidateDto.TaxCategoryId = cellValue;
                                                    }
                                                }
                                                else
                                                {
                                                    ReturnOrderValidateDto.TaxCategoryId = null;
                                                }
                                            }
                                            catch (Exception ex)
                                            {
                                                ReturnOrderValidateDto.Errors.Add("" + ex + "");
                                            }

                                            break;
                                        case "reasonName":
                                            if (!string.IsNullOrEmpty(cellValue))
                                            {
                                                /*var getReason = await _reason.GetByName(cellValue);

                                                if (getReason != null)
                                                {
                                                    ReturnOrderValidateDto.ReasonName = cellValue;
                                                    ReturnOrderValidateDto.ReasonId = getReason?.Id;

                                                }
                                                else
                                                {
                                                    var item = new Reason
                                                    {
                                                        Id = Guid.NewGuid(),
                                                        Code = "",
                                                        Name = cellValue,
                                                        Status = 1,
                                                        CreatedDate = DateTime.Now,
                                                        CreatedBy = _context.GetUserId(),
                                                        CreatedByName = _context.UserClaims.FullName
                                                    };

                                                    _reason.Add(item);
                                                }*/
                                                ReturnOrderValidateDto.ReasonName = cellValue;

                                            }
                                            else
                                            {
                                                ReturnOrderValidateDto.ReasonName = null;
                                                ReturnOrderValidateDto.ReasonId = null;
                                            }


                                            break;
                                        default:
                                            break;
                                    }
                                }
                            }
                        }

                        if (!String.IsNullOrEmpty(ReturnOrderValidateDto.ProductCode))
                        {
                            result.Add(ReturnOrderValidateDto);
                        }
                    }
                }
            }
        }
        catch (Exception ex)
        {
        }
        var getlistunti = await _pimRepository.GetUnitPaging(1, 10000, null, null);
        var resultCode = result.Where(x => !string.IsNullOrEmpty(x.ProductCode)).Select(x => x.ProductCode);
        if (resultCode is not null && resultCode.Count() > 0)
        {
            var getproduct = await _pimRepository.GetProductByListCode(resultCode.ToList());// get code true
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
