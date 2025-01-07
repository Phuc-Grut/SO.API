
using DocumentFormat.OpenXml.Packaging;
using DocumentFormat.OpenXml.Spreadsheet;
using MassTransit.Initializers;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using VFi.Application.SO.DTOs;
using VFi.Application.SO.Ultilities;
using VFi.Domain.SO.Interfaces;
using VFi.Domain.SO.Models;
using VFi.NetDevPack.FopExpression;
using VFi.NetDevPack.Queries;

namespace VFi.Application.SO.Queries;

public class SalesDiscountQueryCheckCode : IQuery<bool>
{

    public SalesDiscountQueryCheckCode(string code)
    {
        Code = code;
    }

    public string Code { get; set; }
}
public class SalesDiscountQueryById : IQuery<SalesDiscountDto>
{
    public SalesDiscountQueryById()
    {
    }

    public SalesDiscountQueryById(Guid id)
    {
        Id = id;
    }

    public Guid Id { get; set; }
}

public class SalesDiscountPagingQuery : FopQuery, IQuery<PagedResult<List<SalesDiscountDto>>>
{
    public SalesDiscountPagingQuery(string? keyword, Guid? customerId, int? status, string? filter, string? order, int pageNumber, int pageSize)
    {
        Keyword = keyword;
        CustomerId = customerId;
        Status = status;
        Filter = filter;
        Order = order;
        PageNumber = pageNumber;
        PageSize = pageSize;
    }
    public Guid? CustomerId { get; set; }

}

public class SalesDiscountExportTemplateQuery : IQuery<MemoryStream>
{
    public SalesDiscountExportTemplateQuery()
    {

    }

}

public class ValidateExcelSalesDiscountQuery : IQuery<List<SalesDiscountValidateDto>>
{
    public ValidateExcelSalesDiscountQuery(IFormFile file, string sheetId, int headerRow, List<ValidateField> listField)
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
public class SalesDiscountQueryHandler :
                                         IQueryHandler<SalesDiscountQueryCheckCode, bool>,
                                         IQueryHandler<SalesDiscountQueryById, SalesDiscountDto>,
                                         IQueryHandler<SalesDiscountPagingQuery, PagedResult<List<SalesDiscountDto>>>,
    IQueryHandler<SalesDiscountExportTemplateQuery, MemoryStream>,
    IQueryHandler<ValidateExcelSalesDiscountQuery, List<SalesDiscountValidateDto>>
{
    private readonly ISalesDiscountRepository _salesDiscountRepository;
    private readonly ISalesDiscountProductRepository _salesDiscountProductRepository;
    private readonly IExportTemplateRepository _exportTemplateRepository;
    private readonly IPIMRepository _pimRepository;
    public SalesDiscountQueryHandler(
        ISalesDiscountRepository salesDiscountRepository,
        ISalesDiscountProductRepository salesDiscountProductRepository, IPIMRepository PimRepository, IExportTemplateRepository exportTemplateRepository
        )
    {
        _salesDiscountRepository = salesDiscountRepository;
        _exportTemplateRepository = exportTemplateRepository;
        _pimRepository = PimRepository;
        _salesDiscountProductRepository = salesDiscountProductRepository;
    }
    public async Task<bool> Handle(SalesDiscountQueryCheckCode request, CancellationToken cancellationToken)
    {
        var data = await _salesDiscountRepository.GetByCode(request.Code);
        if (data is null)
        {
            return false;
        }
        return true;
    }

    public async Task<SalesDiscountDto> Handle(SalesDiscountQueryById request, CancellationToken cancellationToken)
    {
        var item = await _salesDiscountRepository.GetById(request.Id);
        var details = await _salesDiscountProductRepository.GetByParentId(request.Id);
        var _details = details.Select((x, i) =>
        {
            return new SalesDiscountProductDto()
            {
                Id = x.Id,
                SalesDiscountId = x.SalesDiscountId,
                SalesOrderId = x.SalesOrderId,
                SalesOrderCode = x.SalesOrderCode,
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
                TotalAmount = (decimal)(x.Quantity ?? 0) * (x.UnitPrice ?? 0) + (decimal)(x.TaxRate ?? 0) * (decimal)(x.Quantity ?? 0) * (x.UnitPrice ?? 0) / 100,
            };
        }).ToList();

        var result = new SalesDiscountDto()
        {
            Id = item.Id,
            Code = item.Code,
            CustomerId = item.CustomerId,
            CustomerCode = item.CustomerCode,
            CustomerName = item.CustomerName,
            CustomerAddressId = item.CustomerAddressId,
            CustomerAddress = item.CustomerAddress != null
             ? string.Join(", ", new[]
                {
                    item.CustomerAddress.Address,
                    item.CustomerAddress.Ward,
                    item.CustomerAddress.District,
                    item.CustomerAddress.Province,
                    item.CustomerAddress.Country
                }.Where(addressPart => !string.IsNullOrEmpty(addressPart)))
            : "",
            SalesOrderId = item.SalesOrderId,
            SalesOrderCode = item.SalesOrderCode,
            CurrencyId = item.CurrencyId,
            CurrencyCode = item.CurrencyCode,
            CurrencyName = item.CurrencyName,
            ExchangeRate = item.ExchangeRate,
            EmployeeId = item.EmployeeId,
            EmployeeCode = item.EmployeeCode,
            EmployeeName = item.EmployeeName,
            Note = item.Note,
            Status = item.Status,
            DiscountDate = item.DiscountDate,
            PaymentStatus = item.PaymentStatus,
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
            AmountTax = _details.Sum(x => x.AmountTax),
            AmountNoTax = _details.Sum(x => x.AmountNoTax),
            TotalAmount = _details.Sum(x => x.TotalAmount),
            Reference = _details.Select(x => new DocumentDto()
            {
                RefId = x.SalesOrderId,
                RefCode = x.SalesOrderCode,
                RefResourceCode = ResourceCode.DH
            }).GroupBy(y => y.RefId).Select(z => z.First()).ToList(),
            File = JsonConvert.DeserializeObject<List<FileDto>>(string.IsNullOrEmpty(item.File) ? "" : item.File),
            ListDetails = _details,
            TypeDiscount = item.TypeDiscount,
            PaymentInvoice = item.PaymentInvoice.Select(x => new PaymentInvoiceDto()
            {
                Id = x.Id,
                Type = x.Type,
                Code = x.Code,
                SaleDiscountId = x.SaleDiscountId,
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
    public async Task<PagedResult<List<SalesDiscountDto>>> Handle(SalesDiscountPagingQuery request, CancellationToken cancellationToken)
    {
        var response = new PagedResult<List<SalesDiscountDto>>();
        var fopRequest = FopExpressionBuilder<SalesDiscount>.Build(request.Filter, request.Order, request.PageNumber, request.PageSize);
        var (datas, count) = await _salesDiscountRepository.Filter(request.Keyword, request.CustomerId, request.Status, fopRequest);

        var data = datas.Select(item => new SalesDiscountDto()
        {
            Id = item.Id,
            Code = item.Code,
            CustomerId = item.CustomerId,
            CustomerCode = item.CustomerCode,
            CustomerName = item.CustomerName,
            CustomerAddressId = item.CustomerAddressId,
            SalesOrderId = item.SalesOrderId,
            SalesOrderCode = item.SalesOrderCode,
            CurrencyId = item.CurrencyId,
            CurrencyCode = item.CurrencyCode,
            CurrencyName = item.CurrencyName,
            ExchangeRate = item.ExchangeRate,
            EmployeeId = item.EmployeeId,
            EmployeeName = item.EmployeeName,
            EmployeeCode = item.EmployeeCode,
            Note = item.Note,
            Status = item.Status,
            DiscountDate = item.DiscountDate,
        }).ToList();
        response.Items = data;
        response.TotalCount = count;
        response.PageNumber = request.PageNumber;
        response.PageSize = request.PageSize;
        return response;
    }

    public async Task<MemoryStream> Handle(SalesDiscountExportTemplateQuery request, CancellationToken cancellationToken)
    {
        var contentBytes = await _exportTemplateRepository.GetTemplate("CHI_TIET_GIAM_GIA_BAN");

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

    public async Task<List<SalesDiscountValidateDto>> Handle(ValidateExcelSalesDiscountQuery request, CancellationToken cancellationToken)
    {
        List<SalesDiscountValidateDto> result = new List<SalesDiscountValidateDto>();
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
                        SalesDiscountValidateDto SalesDiscountValidateDto = new SalesDiscountValidateDto()
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
                                                SalesDiscountValidateDto.Errors.Add("productCode invalid");
                                            }
                                            else
                                            {
                                                SalesDiscountValidateDto.ProductCode = cellValue;
                                            }

                                            break;
                                        case "productName":
                                            if (!String.IsNullOrEmpty(cellValue) && String.IsNullOrEmpty(SalesDiscountValidateDto.ProductCode))
                                            {
                                                SalesDiscountValidateDto.ProductName = cellValue;
                                                SalesDiscountValidateDto.Errors.Add($"ProductName {cellValue} invalid");
                                            }
                                            break;
                                        case "unitCode":
                                            if (String.IsNullOrEmpty(cellValue))
                                            {
                                                SalesDiscountValidateDto.Errors.Add("unitCode invalid");
                                            }
                                            else
                                            {
                                                /*var unit = getlistunti.FirstOrDefault(a => SalesDiscountValidateDto.ProductCode != null && SalesDiscountValidateDto.UnitType != null && cellValue != null &&
                                                a.GroupUnitCode != null &&
                                                a.GroupUnitCode.ToLower() == SalesDiscountValidateDto.UnitType.ToLower() && cellValue.ToLower() == a.Code.ToLower());
                                                if (unit == null)
                                                {
                                                    SalesDiscountValidateDto.Errors.Add($"unitCode {cellValue} incorrect");
                                                    SalesDiscountValidateDto.UnitCode = cellValue;

                                                }
                                                else
                                                {
                                                    SalesDiscountValidateDto.UnitId = unit.Id;
                                                    SalesDiscountValidateDto.UnitCode = unit.Code;
                                                    SalesDiscountValidateDto.UnitName = unit.Name;
                                                }*/
                                                SalesDiscountValidateDto.UnitCode = cellValue;

                                            }
                                            break;
                                        case "unitName":
                                            SalesDiscountValidateDto.UnitName = cellValue;

                                            if (!String.IsNullOrEmpty(cellValue) && String.IsNullOrEmpty(SalesDiscountValidateDto.UnitCode))
                                            {
                                                SalesDiscountValidateDto.Errors.Add($"Unit name {cellValue} invalid");
                                            }
                                            break;
                                        case "quantity":
                                            SalesDiscountValidateDto.Quantity = cellValue.Replace(',', '.');
                                            double quantityRequest;
                                            if (!String.IsNullOrEmpty(cellValue))
                                            {
                                                if (!double.TryParse(cellValue, out quantityRequest))
                                                {
                                                    SalesDiscountValidateDto.Errors.Add($"Quantity {cellValue} notincorrectformat");
                                                }
                                                /* //var newStockQuantity = SalesDiscountValidateDto.StockQuantity ?? "0";
                                                 var cellV = double.Parse(cellValue.Replace(',', '.'), CultureInfo.InvariantCulture);
                                                 var stockQuantity = double.Parse(SalesDiscountValidateDto.StockQuantity?.Replace(',', '.') ?? "0", CultureInfo.InvariantCulture);
                                                 if (cellV > stockQuantity)
                                                 {
                                                     SalesDiscountValidateDto.Errors.Add($"Quantity {cellValue} lớn hơn stockQuantity {stockQuantity}");
                                                     SalesDiscountValidateDto.Quantity = cellValue.Replace(',', '.');

                                                 }*/
                                                else
                                                {
                                                    SalesDiscountValidateDto.Quantity = cellValue.Replace(',', '.');

                                                }

                                            }

                                            break;
                                        case "unitPrice":
                                            var cv = cellValue.Replace(',', '.');
                                            double amount;
                                            if (!String.IsNullOrEmpty(cv))
                                            {
                                                if (!double.TryParse(cv, out amount))
                                                {
                                                    SalesDiscountValidateDto.Errors.Add($"UnitPriceExpected {cv} notincorrectformat");

                                                }
                                                else
                                                {
                                                    SalesDiscountValidateDto.UnitPrice = amount.ToString();
                                                }
                                            }



                                            break;

                                        case "discountPercent":
                                            var a = cellValue.Replace(',', '.');
                                            double dp;
                                            if (!String.IsNullOrEmpty(a))
                                            {
                                                if (!double.TryParse(a, out dp))
                                                {
                                                    SalesDiscountValidateDto.Errors.Add($"UnitPriceExpected {a} notincorrectformat");

                                                }
                                                else
                                                {
                                                    SalesDiscountValidateDto.DiscountPercent = dp.ToString();
                                                }
                                            }
                                            break;

                                        case "tax":
                                            if (!string.IsNullOrEmpty(cellValue))
                                            {
                                                var taxcheck = taxlist.Where(x => x.Name == cellValue || x.Code == cellValue).FirstOrDefault();
                                                if (taxcheck != null)
                                                {
                                                    SalesDiscountValidateDto.TaxCategoryId = taxcheck.Id.ToString();
                                                    SalesDiscountValidateDto.TaxName = taxcheck.Name;
                                                    SalesDiscountValidateDto.TaxRate = taxcheck.Rate.ToString();
                                                    SalesDiscountValidateDto.TaxCode = taxcheck.Code;
                                                }
                                                else
                                                {
                                                    SalesDiscountValidateDto.Errors.Add("tax incorrect");
                                                    SalesDiscountValidateDto.TaxName = cellValue;
                                                }
                                            }
                                            else
                                            {
                                                SalesDiscountValidateDto.TaxName = null;
                                            }
                                            break;
                                        case "reasonDiscount":
                                            SalesDiscountValidateDto.ReasonDiscount = cellValue;

                                            break;
                                        default:
                                            break;
                                    }
                                }
                            }
                        }

                        if (!String.IsNullOrEmpty(SalesDiscountValidateDto.ProductCode))
                        {
                            result.Add(SalesDiscountValidateDto);
                        }
                    }
                }
            }
        }
        catch (Exception ex)
        {
        }
        var resultCode = result.Where(x => !string.IsNullOrEmpty(x.ProductCode)).Select(x => x.ProductCode);
        var getlistunti = await _pimRepository.GetUnitPaging(1, 10000, null, null);

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
                    var a = result.FirstOrDefault(x => x.ProductCode == i.ProductCode.Trim());
                    var getP = getproduct.FirstOrDefault(x => x.Code == i.ProductCode.Trim());

                    if (a != null)
                    {
                        a.ProductId = getP.Id;
                        a.ProductName = getP.Name;
                        a.ProductCode = getP.Code;
                        a.StockQuantity = getP.StockQuantity;
                        a.UnitType = getP.UnitType;

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
        /* var getUnit = result.Where(x => u.Contains(x.UnitType)).ToList();
         var getUnitE = result.Where(x => !u.Contains(x.UnitType)).ToList();
         if (getUnit != null)
         {
             foreach (var y in getUnit)
             {
                 var unit = result.Where(x => x.UnitCode == y.UnitCode.Trim()).FirstOrDefault();
                 var getU = getlistunti.Where(x => x.Code == y.UnitCode.Trim()).FirstOrDefault();
                 if (unit != null)
                 {
                     unit.UnitCode = getU.Code;
                     unit.UnitName = getU.Name;
                     unit.UnitId = getU.Id;
                 }
             }
         }
         if (getUnitE != null && getUnitE.Count() > 0)
         {
             foreach (var ii in getUnitE)
             {
                 var aa = result.Where(x => x.UnitCode == ii.UnitCode.Trim()).FirstOrDefault();

                 if (aa != null)
                 {
                     aa.Errors.Add($"{aa.UnitCode} invalid");
                 }
             }
         }*/
        return result;
    }
}
