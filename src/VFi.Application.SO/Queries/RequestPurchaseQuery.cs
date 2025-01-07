using System.Globalization;
using DocumentFormat.OpenXml.Bibliography;
using DocumentFormat.OpenXml.Packaging;
using DocumentFormat.OpenXml.Spreadsheet;
using MassTransit.Internals;
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

public class RequestPurchaseQueryComboBox : IQuery<IEnumerable<ComboBoxDto>>
{
    public RequestPurchaseQueryComboBox(int? status)
    {
        Status = status;
    }
    public int? Status { get; set; }
}

public class RequestPurchaseQueryById : IQuery<RequestPurchaseDto>
{
    public RequestPurchaseQueryById()
    {
    }

    public RequestPurchaseQueryById(Guid id)
    {
        RequestPurchaseId = id;
    }

    public Guid RequestPurchaseId { get; set; }
}

public class RemoveOrderFromRequestPurchase : IQuery<RequestPurchaseDto>
{
    public RemoveOrderFromRequestPurchase(Guid requestPurchaseId, Guid? orderId)
    {
        RequestPurchaseId = requestPurchaseId;
        OrderId = orderId;
    }
    public Guid RequestPurchaseId { get; set; }
    public Guid? OrderId { get; set; }
}

public class RequestPurchasePagingQuery : FopQuery, IQuery<PagedResult<List<RequestPurchaseDto>>>
{
    public RequestPurchasePagingQuery(string? keyword, int? status, string? employeeId, string? filter, string? order, int pageNumber, int pageSize)
    {
        Keyword = keyword;
        Status = status;
        Filter = filter;
        Order = order;
        PageNumber = pageNumber;
        PageSize = pageSize;
        EmployeeId = employeeId;
    }
    public string? EmployeeId { get; set; }
}
public class RequestPurchaseExportTemplateQuery : IQuery<MemoryStream>
{
    public RequestPurchaseExportTemplateQuery()
    {

    }

}

public class ValidateExcelRequestPurchaseQuery : IQuery<List<RequestPurchaseValidateDto>>
{
    public ValidateExcelRequestPurchaseQuery(IFormFile file, string sheetId, int headerRow, List<ValidateField> listField)
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

public class RequestPurchaseQuery : IQueryHandler<RequestPurchaseQueryComboBox, IEnumerable<ComboBoxDto>>,
                                        IQueryHandler<RequestPurchaseQueryById, RequestPurchaseDto>,
                                        IQueryHandler<RequestPurchasePagingQuery, PagedResult<List<RequestPurchaseDto>>>,
                                    IQueryHandler<RequestPurchaseExportTemplateQuery, MemoryStream>,
                                    IQueryHandler<RemoveOrderFromRequestPurchase, RequestPurchaseDto>,
    IQueryHandler<ValidateExcelRequestPurchaseQuery, List<RequestPurchaseValidateDto>>
{
    private readonly IPIMRepository _pimRepository;
    private readonly IRequestPurchaseRepository _repository;
    private readonly IExportTemplateRepository _exportTemplateRepository;
    public RequestPurchaseQuery(IPIMRepository PimRepository, IRequestPurchaseRepository respository, IExportTemplateRepository exportTemplateRepository)
    {
        _pimRepository = PimRepository;
        _repository = respository;
        _exportTemplateRepository = exportTemplateRepository;
    }

    public async Task<RequestPurchaseDto> Handle(RequestPurchaseQueryById request, CancellationToken cancellationToken)
    {
        var obj = await _repository.GetById(request.RequestPurchaseId);
        var dataInventory = new List<INVENTORY_DETAIL_BY_LISTID>();
        var listId = new List<string>();
        var listProductId = new List<Guid?>();
        var i = 1;
        var source = obj.RequestPurchaseProduct.Select(x => x.ProductId).Distinct();
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
            var inventory = await _pimRepository.GetInventoryDetail(o);
            foreach (var item in inventory)
            {
                var rs = new INVENTORY_DETAIL_BY_LISTID()
                {
                    Id = item.Id,
                    WarehouseId = item.WarehouseId,
                    Code = item.Code,
                    Name = item.Name,
                    ProductId = item.ProductId,
                    StockQuantity = item.StockQuantity,
                    ReservedQuantity = item.ReservedQuantity,
                    PlannedQuantity = item.PlannedQuantity
                };
                dataInventory.Add(rs);
            }
        }
        var result = new RequestPurchaseDto()
        {
            Id = obj.Id,
            Code = obj.Code,
            RequestBy = obj.RequestBy,
            RequestByName = obj.RequestByName,
            RequestByEmail = obj.RequestByEmail,
            RequestDate = obj.RequestDate,
            CurrencyCode = obj.CurrencyCode,
            CurrencyName = obj.CurrencyName,
            Calculation = obj.Calculation,
            ExchangeRate = obj.ExchangeRate,
            Proposal = obj.Proposal,
            Note = obj.Note,
            ApproveDate = obj.ApproveDate,
            ApproveBy = obj.ApproveBy,
            ApproveByName = obj.ApproveByName,
            ApproveComment = obj.ApproveComment,
            QuantityRequest = obj.QuantityRequest,
            QuantityApproved = obj.QuantityApproved,
            StatusPurchase = obj.StatusPurchase,
            QuantityPurchased = obj.RequestPurchaseProduct.Sum(x => x.QuantityPurchased),
            Status = obj.Status,
            PurchaseRequestCode = obj.PurchaseRequestCode,
            OrderId = obj.OrderId,
            OrderCode = obj.OrderCode,
            Podate = obj.Podate,
            Postatus = obj.Postatus,
            CreatedBy = obj.CreatedBy,
            CreatedDate = obj.CreatedDate,
            UpdatedBy = obj.UpdatedBy,
            UpdatedDate = obj.UpdatedDate,
            CreatedByName = obj.CreatedByName,
            UpdatedByName = obj.UpdatedByName,
            File = JsonConvert.DeserializeObject<List<FileDto>>(string.IsNullOrEmpty(obj.File) ? "" : obj.File),
            Details = obj.RequestPurchaseProduct.Select(x => new RequestPurchaseProductDto()
            {
                Id = x.Id,
                RequestPurchaseId = x.RequestPurchaseId,
                OrderId = x.OrderId,
                OrderCode = x.OrderCode,
                OrderProductId = x.OrderProductId,
                ProductId = x.ProductId.Value,
                ProductCode = x.ProductCode,
                ProductName = x.ProductName,
                ProductImage = x.ProductImage,
                Origin = x.Origin,
                UnitType = x.UnitType,
                UnitCode = x.UnitCode,
                UnitName = x.UnitName,
                StockQuantity = dataInventory.Where(y => y.ProductId == x.ProductId).Sum(y => y.StockQuantity),
                QuantityRequest = x.QuantityRequest.Value,
                QuantityApproved = x.QuantityApproved,
                QuantityPurchased = x.QuantityPurchased,
                UnitPrice = x.UnitPrice,
                Currency = x.Currency,
                DeliveryDate = x.DeliveryDate,
                PriorityLevel = x.PriorityLevel,
                VendorCode = x.VendorCode,
                VendorName = x.VendorName,
                Status = x.Status,
                Note = x.Note,
                StatusPurchase = x.StatusPurchase,
                DisplayOrder = x.DisplayOrder
            }).OrderBy(x => x.DisplayOrder).ToList()
        };
        return result;
    }

    public async Task<RequestPurchaseDto> Handle(RemoveOrderFromRequestPurchase request, CancellationToken cancellationToken)
    {
        var obj = await _repository.GetById(request.RequestPurchaseId);
        var dataInventory = new List<INVENTORY_DETAIL_BY_LISTID>();
        var listId = new List<string>();
        var listProductId = new List<Guid?>();
        var i = 1;
        var source = obj.RequestPurchaseProduct.Select(x => x.ProductId).Distinct();
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
            var inventory = await _pimRepository.GetInventoryDetail(o);
            foreach (var item in inventory)
            {
                var rs = new INVENTORY_DETAIL_BY_LISTID()
                {
                    Id = item.Id,
                    WarehouseId = item.WarehouseId,
                    Code = item.Code,
                    Name = item.Name,
                    ProductId = item.ProductId,
                    StockQuantity = item.StockQuantity,
                    ReservedQuantity = item.ReservedQuantity,
                    PlannedQuantity = item.PlannedQuantity
                };
                dataInventory.Add(rs);
            }
        }
        var result = new RequestPurchaseDto()
        {
            Id = obj.Id,
            Code = obj.Code,
            RequestBy = obj.RequestBy,
            RequestByName = obj.RequestByName,
            RequestByEmail = obj.RequestByEmail,
            RequestDate = obj.RequestDate,
            CurrencyCode = obj.CurrencyCode,
            CurrencyName = obj.CurrencyName,
            Calculation = obj.Calculation,
            ExchangeRate = obj.ExchangeRate,
            Proposal = obj.Proposal,
            Note = obj.Note,
            ApproveDate = obj.ApproveDate,
            ApproveBy = obj.ApproveBy,
            ApproveByName = obj.ApproveByName,
            ApproveComment = obj.ApproveComment,
            QuantityRequest = obj.QuantityRequest,
            QuantityApproved = obj.QuantityApproved,
            StatusPurchase = obj.StatusPurchase,
            QuantityPurchased = obj.RequestPurchaseProduct.Sum(x => x.QuantityPurchased),
            Status = obj.Status,
            PurchaseRequestCode = obj.PurchaseRequestCode,
            OrderId = obj.OrderId,
            OrderCode = obj.OrderCode,
            Podate = obj.Podate,
            Postatus = obj.Postatus,
            CreatedBy = obj.CreatedBy,
            CreatedDate = obj.CreatedDate,
            UpdatedBy = obj.UpdatedBy,
            UpdatedDate = obj.UpdatedDate,
            CreatedByName = obj.CreatedByName,
            UpdatedByName = obj.UpdatedByName,
            File = JsonConvert.DeserializeObject<List<FileDto>>(string.IsNullOrEmpty(obj.File) ? "" : obj.File),
            Details = obj.RequestPurchaseProduct.Select(x => new RequestPurchaseProductDto()
            {
                Id = x.Id,
                RequestPurchaseId = x.RequestPurchaseId,
                OrderId = x.OrderId,
                OrderCode = x.OrderCode,
                OrderProductId = x.OrderProductId,
                ProductId = x.ProductId.Value,
                ProductCode = x.ProductCode,
                ProductName = x.ProductName,
                ProductImage = x.ProductImage,
                Origin = x.Origin,
                UnitType = x.UnitType,
                UnitCode = x.UnitCode,
                UnitName = x.UnitName,
                StockQuantity = dataInventory.Where(y => y.ProductId == x.ProductId).Sum(y => y.StockQuantity),
                QuantityRequest = x.QuantityRequest.Value,
                QuantityApproved = x.QuantityApproved,
                QuantityPurchased = x.QuantityPurchased,
                UnitPrice = x.UnitPrice,
                Currency = x.Currency,
                DeliveryDate = x.DeliveryDate,
                PriorityLevel = x.PriorityLevel,
                VendorCode = x.VendorCode,
                VendorName = x.VendorName,
                Status = x.Status,
                Note = x.Note,
                StatusPurchase = x.StatusPurchase,
                DisplayOrder = x.DisplayOrder
            }).OrderBy(x => x.DisplayOrder).ToList()
        };
        return result;
    }

    public async Task<PagedResult<List<RequestPurchaseDto>>> Handle(RequestPurchasePagingQuery request, CancellationToken cancellationToken)
    {
        var response = new PagedResult<List<RequestPurchaseDto>>();
        var fopRequest = FopExpressionBuilder<RequestPurchase>.Build(request.Filter, request.Order, request.PageNumber, request.PageSize);
        var filter = new Dictionary<string, object>();
        if (request.Status != null)
            filter.Add("status", request.Status.ToString());
        if (!string.IsNullOrEmpty(request.EmployeeId))
            filter.Add("employeeId", request.EmployeeId);
        var (datas, count) = await _repository.Filter(request.Keyword, filter, fopRequest);

        var data = datas.Select(a => new RequestPurchaseDto()
        {
            Id = a.Id,
            Code = a.Code,
            RequestBy = a.RequestBy,
            RequestByName = a.RequestByName,
            RequestByEmail = a.RequestByEmail,
            RequestDate = a.RequestDate,
            CurrencyCode = a.CurrencyCode,
            CurrencyName = a.CurrencyName,
            ExchangeRate = a.ExchangeRate,
            Proposal = a.Proposal,
            Note = a.Note,
            Status = a.Status,
            CreatedBy = a.CreatedBy,
            CreatedDate = a.CreatedDate,
            UpdatedBy = a.UpdatedBy,
            UpdatedDate = a.UpdatedDate,
            CreatedByName = a.CreatedByName,
            UpdatedByName = a.UpdatedByName,
            QuantityRequest = a.RequestPurchaseProduct.Sum(x => x.QuantityRequest),
            QuantityApproved = a.RequestPurchaseProduct.Sum(x => x.QuantityApproved),
            StatusPurchase = a.StatusPurchase,
            QuantityPurchased = a.RequestPurchaseProduct.Sum(x => x.QuantityPurchased),
            OrderId = a.OrderId,
            OrderCode = a.OrderCode,
            Podate = a.Podate,
            Postatus = a.Postatus,
            PurchaseRequestCode = a.PurchaseRequestCode
        }).ToList();
        response.Items = data;
        response.TotalCount = count;
        response.PageNumber = request.PageNumber;
        response.PageSize = request.PageSize;
        return response;
    }

    public async Task<IEnumerable<ComboBoxDto>> Handle(RequestPurchaseQueryComboBox request, CancellationToken cancellationToken)
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

    public async Task<MemoryStream> Handle(RequestPurchaseExportTemplateQuery request, CancellationToken cancellationToken)
    {
        var contentBytes = await _exportTemplateRepository.GetTemplate("MAU_IMPORT_CHI_TIET_DE_NGHI_MUA");

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
    public async Task<List<RequestPurchaseValidateDto>> Handle(ValidateExcelRequestPurchaseQuery request, CancellationToken cancellationToken)
    {
        List<RequestPurchaseValidateDto> result = new List<RequestPurchaseValidateDto>();
        try
        {
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
                        RequestPurchaseValidateDto RequestPurchaseValidateDto = new RequestPurchaseValidateDto()
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
                                                RequestPurchaseValidateDto.Errors.Add("productCode invalid");
                                            }
                                            else
                                            {
                                                RequestPurchaseValidateDto.ProductCode = cellValue;

                                            }
                                            break;
                                        case "productName":
                                            if (!String.IsNullOrEmpty(cellValue) && String.IsNullOrEmpty(RequestPurchaseValidateDto.ProductCode))
                                            {
                                                RequestPurchaseValidateDto.ProductName = cellValue;
                                                RequestPurchaseValidateDto.Errors.Add("ProductCode invalid");

                                            }
                                            break;
                                        case "unitCode":
                                            if (String.IsNullOrEmpty(cellValue))
                                            {
                                                RequestPurchaseValidateDto.Errors.Add("unitCode invalid");
                                            }
                                            else
                                            {
                                                RequestPurchaseValidateDto.UnitCode = cellValue;
                                            }
                                            break;
                                        case "unitName":
                                            if (!String.IsNullOrEmpty(cellValue) && String.IsNullOrEmpty(RequestPurchaseValidateDto.UnitCode))
                                            {
                                                RequestPurchaseValidateDto.UnitName = cellValue;
                                            }
                                            break;
                                        case "quantityRequest":
                                            try
                                            {
                                                double quantityRequest;
                                                if (!String.IsNullOrEmpty(cellValue))
                                                {
                                                    if (!double.TryParse(cellValue, out quantityRequest))
                                                    {
                                                        RequestPurchaseValidateDto.Errors.Add($"Quantityrequest {cellValue} notincorrectformat");
                                                    }
                                                    /* var cellV = double.Parse(cellValue.Replace(',', '.'), CultureInfo.InvariantCulture);
                                                     var stockQuantity = double.Parse(RequestPurchaseValidateDto.StockQuantity.Replace(',', '.') ?? "0", CultureInfo.InvariantCulture);
                                                     if (cellV > stockQuantity)
                                                     {
                                                         RequestPurchaseValidateDto.Errors.Add($"Quantityrequest {cellValue} lớn hơn stockQuantity {stockQuantity}");
                                                         RequestPurchaseValidateDto.QuantityRequest = cellValue.Replace(',', '.');

                                                     }*/
                                                }
                                            }
                                            catch
                                            {
                                                RequestPurchaseValidateDto.Errors.Add("Quantityrequest notincorrectformat");
                                            }
                                            RequestPurchaseValidateDto.QuantityRequest = cellValue.Replace(',', '.');

                                            break;
                                        case "unitPrice":
                                            try
                                            {
                                                double amount;
                                                if (!String.IsNullOrEmpty(cellValue))
                                                {
                                                    if (!double.TryParse(cellValue, out amount))
                                                    {
                                                        RequestPurchaseValidateDto.Errors.Add($"UnitPriceExpected {cellValue} notincorrectformat");
                                                    }
                                                }
                                            }

                                            catch
                                            {
                                                RequestPurchaseValidateDto.Errors.Add("UnitPriceExpected notincorrectformat");
                                            }

                                            RequestPurchaseValidateDto.UnitPrice = cellValue.Replace(',', '.');
                                            ;

                                            break;
                                        case "deliveryDate":
                                            try
                                            {
                                                double excelDateValue;
                                                DateTime excelDate;
                                                if (String.IsNullOrEmpty(cellValue))
                                                {
                                                    RequestPurchaseValidateDto.Errors.Add("");

                                                }
                                                else if (Double.TryParse(cellValue, out excelDateValue))
                                                {
                                                    RequestPurchaseValidateDto.DeliveryDate = DateTime.FromOADate(excelDateValue);
                                                }
                                                else if (DateTime.TryParseExact(cellValue, "dd/MM/yyyy", CultureInfo.InvariantCulture, DateTimeStyles.None, out excelDate))
                                                {
                                                    RequestPurchaseValidateDto.DeliveryDate = excelDate;
                                                }
                                                else
                                                {
                                                    RequestPurchaseValidateDto.Errors.Add($"DeliveryDate {cellValue} notincorrectformat");
                                                }
                                            }
                                            catch
                                            {
                                                RequestPurchaseValidateDto.Errors.Add($"DeliveryDate {cellValue} notincorrectformat");

                                            }
                                            /*
                                              if (!string.IsNullOrEmpty(cellValue))
                                              {
                                                  double excelDateValue;
                                                  DateTime? excelDate;
                                                  if (Double.TryParse(cellValue, out excelDateValue))
                                                  {
                                                      RequestPurchaseValidateDto.DeliveryDate = DateTime.FromOADate(excelDateValue);
                                                  }
                                                  else
                                                  {
                                                      RequestPurchaseValidateDto.DeliveryDate = DateTime.FromOADate(excelDateValue);
                                                      RequestPurchaseValidateDto.Errors.Add($"DeliveryDate {cellValue} notincorrectformat");

                                                  }
                                              }
                                              else
                                              {
                                                  RequestPurchaseValidateDto.DeliveryDate = null;
                                                  RequestPurchaseValidateDto.Errors.Add($"DeliveryDate {cellValue} notincorrectformat");
                                              }*/

                                            break;
                                        case "priorityLevel":
                                            RequestPurchaseValidateDto.PriorityLevel = cellValue != null && cellValue != "" ? cellValue == "Gấp" ? 1 : 0 : null;


                                            break;
                                        case "note":
                                            RequestPurchaseValidateDto.Note = cellValue;

                                            break;
                                        default:
                                            break;
                                    }
                                }
                            }
                        }

                        if (!String.IsNullOrEmpty(RequestPurchaseValidateDto.ProductCode))
                        {
                            result.Add(RequestPurchaseValidateDto);
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
