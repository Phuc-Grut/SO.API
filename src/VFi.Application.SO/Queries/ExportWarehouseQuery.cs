using System.Globalization;
using Consul;
using DocumentFormat.OpenXml.Drawing.Diagrams;
using DocumentFormat.OpenXml.Office2010.Excel;
using DocumentFormat.OpenXml.Packaging;
using DocumentFormat.OpenXml.Spreadsheet;
using Microsoft.AspNetCore.Http;
using Microsoft.CodeAnalysis;
using Newtonsoft.Json;
using VFi.Application.SO.DTOs;
using VFi.Domain.SO.Interfaces;
using VFi.Domain.SO.Models;
using VFi.Infra.SO.Repository;
using VFi.NetDevPack.Data;
using VFi.NetDevPack.FopExpression;
using VFi.NetDevPack.Queries;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;


namespace VFi.Application.SO.Queries;

public class ExportWarehouseQueryById : IQuery<ExportWarehouseDto>
{
    public ExportWarehouseQueryById()
    {
    }

    public ExportWarehouseQueryById(Guid id)
    {
        Id = id;
    }

    public Guid Id { get; set; }
}

public class ExportWarehouseQueryComboBox : IQuery<IEnumerable<ComboBoxDto>>
{
    public ExportWarehouseQueryComboBox(int? status)
    {
        Status = status;
    }
    public int? Status { get; set; }
}
public class ExportWarehouseQueryCheckCode : IQuery<bool>
{

    public ExportWarehouseQueryCheckCode(string code)
    {
        Code = code;
    }

    public string Code { get; set; }
}

public class ExportWarehousePagingQuery : FopQuery, IQuery<PagedResult<List<ExportWarehouseDto>>>
{
    public ExportWarehousePagingQuery(string? keyword, Guid? customerId, int? status, string? employeeId, string filter, string order, int pageNumber, int pageSize)
    {
        Keyword = keyword;
        CustomerId = customerId;
        Status = status;
        Filter = filter;
        Order = order;
        PageNumber = pageNumber;
        PageSize = pageSize;
        EmployeeId = employeeId;
    }
    public string? EmployeeId { get; set; }
    public Guid? CustomerId { get; set; }
}
public class ExportWarehouseExportTemplateQuery : IQuery<MemoryStream>
{
    public ExportWarehouseExportTemplateQuery()
    {

    }

}
public class ValidateExcelExportWarehouseQuery : IQuery<List<ExportWarehouseValidateDto>>
{
    public ValidateExcelExportWarehouseQuery(IFormFile file, string sheetId, int headerRow, List<ValidateField> listField)
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
public class ExportWarehouseQueryHandler : IQueryHandler<ExportWarehouseQueryComboBox, IEnumerable<ComboBoxDto>>,
                                         IQueryHandler<ExportWarehouseQueryCheckCode, bool>,
                                         IQueryHandler<ExportWarehouseQueryById, ExportWarehouseDto>,
                                         IQueryHandler<ExportWarehousePagingQuery, PagedResult<List<ExportWarehouseDto>>>,
    IQueryHandler<ExportWarehouseExportTemplateQuery, MemoryStream>,
    IQueryHandler<ValidateExcelExportWarehouseQuery, List<ExportWarehouseValidateDto>>
{
    private readonly IExportWarehouseRepository _ExportWarehouseRepository;
    private readonly IExportWarehouseProductRepository _ExportWarehouseProductRepository;
    private readonly IExportTemplateRepository _exportTemplateRepository;
    private readonly IPIMRepository _pimRepository;
    private readonly IExportWarehouseRepository _repository;

    public ExportWarehouseQueryHandler(IExportWarehouseRepository ExportWarehouseRespository, IExportWarehouseProductRepository exportWarehouseProductRepository, IExportTemplateRepository exportTemplateRepository, IPIMRepository PimRepository, IExportWarehouseRepository repository)
    {
        _ExportWarehouseRepository = ExportWarehouseRespository;
        _ExportWarehouseProductRepository = exportWarehouseProductRepository;
        _exportTemplateRepository = exportTemplateRepository;
        _repository = repository;
        _pimRepository = PimRepository;
    }
    public async Task<bool> Handle(ExportWarehouseQueryCheckCode request, CancellationToken cancellationToken)
    {
        var data = await _ExportWarehouseRepository.GetByCode(request.Code);
        if (data is null)
        {
            return false;
        }
        return true;
    }

    public async Task<ExportWarehouseDto> Handle(ExportWarehouseQueryById request, CancellationToken cancellationToken)
    {

        var obj = await _ExportWarehouseRepository.GetById(request.Id);
        var dataInventory = new List<INVENTORY_DETAIL_BY_LISTID>();
        var listId = new List<string>();
        var listProductId = new List<Guid?>();
        var i = 1;
        var source = obj.ExportWarehouseProduct.Select(x => x.ProductId).Distinct();
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

        var result = new ExportWarehouseDto()
        {
            Id = obj.Id,
            Code = obj.Code,
            OrderId = obj.OrderId,
            OrderCode = obj.OrderCode,
            CustomerId = obj.CustomerId,
            CustomerCode = obj.CustomerCode,
            CustomerName = obj.CustomerName,
            Description = obj.Description,
            WarehouseId = obj.WarehouseId,
            WarehouseCode = obj.WarehouseCode,
            WarehouseName = obj.WarehouseName,
            DeliveryStatus = obj.DeliveryStatus,
            DeliveryName = obj.WarehouseName,
            DeliveryAddress = obj.DeliveryAddress,
            DeliveryCountry = obj.DeliveryCountry,
            DeliveryProvince = obj.DeliveryProvince,
            DeliveryDistrict = obj.DeliveryDistrict,
            DeliveryWard = obj.DeliveryWard,
            DeliveryNote = obj.DeliveryNote,
            EstimatedDeliveryDate = obj.EstimatedDeliveryDate,
            DeliveryMethodId = obj.DeliveryMethodId,
            DeliveryMethodCode = obj.DeliveryMethodCode,
            DeliveryMethodName = obj.DeliveryMethodName,
            ShippingMethodId = obj.ShippingMethodId,
            ShippingMethodCode = obj.ShippingMethodCode,
            ShippingMethodName = obj.ShippingMethodName,
            Status = obj.Status,
            StatusExport = obj.StatusExport,
            Note = obj.Note,
            RequestByName = obj.RequestByName,
            RequestDate = obj.RequestDate,
            RequestBy = obj.RequestBy,
            ApproveDate = obj.ApproveDate,
            ApproveBy = obj.ApproveBy,
            ApproveComment = obj.ApproveComment,
            FulfillmentRequestCode = obj.FulfillmentRequestCode,
            CreatedBy = obj.CreatedBy,
            CreatedDate = obj.CreatedDate,
            UpdatedBy = obj.UpdatedBy,
            UpdatedDate = obj.UpdatedDate,
            CreatedByName = obj.CreatedByName,
            UpdatedByName = obj.UpdatedByName,
            TypeDiscount = obj.Order?.TypeDiscount,
            DiscountRate = obj.Order?.DiscountRate,
            TypeCriteria = obj.Order?.TypeCriteria,
            AmountDiscount = obj.Order?.AmountDiscount,
            File = JsonConvert.DeserializeObject<List<FileDto>>(string.IsNullOrEmpty(obj.File) ? "" : obj.File),
            Details = obj.ExportWarehouseProduct.Select(x => new ExportWarehouseProductDto()
            {
                Id = x.Id,
                OrderProductId = x.OrderProductId,
                OrderId = x.OrderId,
                OrderCode = x.OrderCode,
                ExportWarehouseId = x.ExportWarehouseId,
                ProductId = x.ProductId,
                ProductCode = x.ProductCode,
                ProductName = x.ProductName,
                ProductImage = x.ProductImage,
                WarehouseCode = x.WarehouseCode,
                WarehouseName = x.WarehouseName,
                UnitCode = x.UnitCode,
                UnitName = x.UnitName,
                StockQuantity = !String.IsNullOrEmpty(x.WarehouseCode) ? dataInventory.Where(y => y.ProductId == x.ProductId && y.Code == x.WarehouseCode).FirstOrDefault()?.StockQuantity : dataInventory.Where(y => y.ProductId == x.ProductId).Sum(y => y.StockQuantity),
                QuantityRequest = x.QuantityRequest,
                QuantityExported = x.QuantityExported,
                Note = x.Note,
                DisplayOrder = x.DisplayOrder

            }).OrderBy(x => x.DisplayOrder).ToList()
        };

        return result;
    }

    public async Task<PagedResult<List<ExportWarehouseDto>>> Handle(ExportWarehousePagingQuery request, CancellationToken cancellationToken)
    {
        var response = new PagedResult<List<ExportWarehouseDto>>();

        var fopRequest = FopExpressionBuilder<ExportWarehouse>.Build(request.Filter, request.Order, request.PageNumber, request.PageSize);
        var filter = new Dictionary<string, object>();
        if (request.Status != null)
            filter.Add("status", request.Status.ToString());
        if (!string.IsNullOrEmpty(request.EmployeeId))
            filter.Add("employeeId", request.EmployeeId);
        if (request.CustomerId != null)
            filter.Add("customerId", request.CustomerId);
        var (ExportWarehouses, count) = await _ExportWarehouseRepository.Filter(request.Keyword, filter, fopRequest);
        var data = ExportWarehouses.Select(obj => new ExportWarehouseDto()
        {
            Id = obj.Id,
            Code = obj.Code,
            OrderId = obj.OrderId,
            OrderCode = obj.OrderCode,
            CustomerId = obj.CustomerId,
            CustomerCode = obj.CustomerCode,
            CustomerName = obj.CustomerName,
            Description = obj.Description,
            WarehouseId = obj.WarehouseId,
            WarehouseCode = obj.WarehouseCode,
            WarehouseName = obj.WarehouseName,
            DeliveryStatus = obj.DeliveryStatus,
            DeliveryName = obj.WarehouseName,
            DeliveryAddress = obj.DeliveryAddress,
            DeliveryCountry = obj.DeliveryCountry,
            DeliveryProvince = obj.DeliveryProvince,
            DeliveryDistrict = obj.DeliveryDistrict,
            DeliveryWard = obj.DeliveryWard,
            DeliveryNote = obj.DeliveryNote,
            EstimatedDeliveryDate = obj.EstimatedDeliveryDate,
            DeliveryMethodId = obj.DeliveryMethodId,
            DeliveryMethodName = obj.DeliveryMethodName,
            ShippingMethodId = obj.ShippingMethodId,
            ShippingMethodName = obj.ShippingMethodName,
            Status = obj.Status,
            StatusExport = obj.StatusExport,
            Note = obj.Note,
            ApproveComment = obj.ApproveComment,
            RequestDate = obj.RequestDate,
            RequestByName = obj.RequestByName,
            CreatedBy = obj.CreatedBy,
            CreatedDate = obj.CreatedDate,
            UpdatedBy = obj.UpdatedBy,
            UpdatedDate = obj.UpdatedDate,
            CreatedByName = obj.CreatedByName,
            UpdatedByName = obj.UpdatedByName,
            Details = obj.ExportWarehouseProduct.Select(product => new ExportWarehouseProductDto
            {
                Id = product.Id,
                ProductId = product.ProductId,
                ProductCode = product.ProductCode,
                ProductName = product.ProductName,
                ProductImage = product.ProductImage,
                UnitCode = product.UnitCode,
                UnitName = product.UnitName,
                QuantityRequest = product.QuantityRequest,
            }).ToList()
        }).ToList();
        response.Items = data;
        response.TotalCount = count;
        response.PageNumber = request.PageNumber;
        response.PageSize = request.PageSize;
        return response;
    }

    public async Task<IEnumerable<ComboBoxDto>> Handle(ExportWarehouseQueryComboBox request, CancellationToken cancellationToken)
    {

        var ExportWarehouses = await _ExportWarehouseRepository.GetListCbx(request.Status);
        //var Contacts = await _ContactsRepository.GetListCbx(request.Status);
        var result = ExportWarehouses.Select(x => new ComboBoxDto()
        {
            Key = x.Code,
            Value = x.Id,
            Label = x.Code
        });
        return result;
    }

    public async Task<MemoryStream> Handle(ExportWarehouseExportTemplateQuery request, CancellationToken cancellationToken)
    {
        var contentBytes = await _exportTemplateRepository.GetTemplate("MAU_IMPORT_CHI_TIET_DE_NGHI_XUAT");

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

    public async Task<List<ExportWarehouseValidateDto>> Handle(ValidateExcelExportWarehouseQuery request, CancellationToken cancellationToken)
    {
        List<ExportWarehouseValidateDto> result = new List<ExportWarehouseValidateDto>();
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
                        ExportWarehouseValidateDto ExportWarehouseValidateDto = new ExportWarehouseValidateDto()
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
                                                ExportWarehouseValidateDto.Errors.Add($"productCode {cellValue} invalid");
                                            }
                                            else
                                            {
                                                ExportWarehouseValidateDto.ProductCode = cellValue;
                                            }

                                            break;
                                        case "productName":
                                            if (!String.IsNullOrEmpty(cellValue) && String.IsNullOrEmpty(ExportWarehouseValidateDto.ProductCode))
                                            {
                                                ExportWarehouseValidateDto.ProductName = cellValue;
                                                ExportWarehouseValidateDto.Errors.Add($"ProductName {cellValue} invalid");
                                            }
                                            break;
                                        case "unitCode":
                                            if (String.IsNullOrEmpty(cellValue))
                                            {
                                                ExportWarehouseValidateDto.Errors.Add($"unitCode {cellValue} invalid");
                                            }
                                            else
                                            {
                                                ExportWarehouseValidateDto.UnitCode = cellValue;
                                            }
                                            break;
                                        case "unitName":
                                            if (!String.IsNullOrEmpty(cellValue) && String.IsNullOrEmpty(ExportWarehouseValidateDto.UnitCode))
                                            {
                                                ExportWarehouseValidateDto.UnitName = cellValue;
                                            }
                                            break;

                                        case "quantityRequest":
                                            ExportWarehouseValidateDto.QuantityRequest = cellValue.Replace(',', '.');
                                            try
                                            {
                                                double quantityRequest;
                                                if (!String.IsNullOrEmpty(cellValue))
                                                {
                                                    if (!double.TryParse(cellValue, out quantityRequest))
                                                    {
                                                        ExportWarehouseValidateDto.Errors.Add($"Quantityrequest {cellValue} notincorrectformat");
                                                    }
                                                    /*var cellV = double.Parse(cellValue, CultureInfo.InvariantCulture);
                                                    var stockQuantity = double.Parse(ExportWarehouseValidateDto.StockQuantity ?? "0", CultureInfo.InvariantCulture);
                                                    if (cellV > stockQuantity)
                                                    {
                                                        ExportWarehouseValidateDto.Errors.Add($"Quantityrequest {cellValue} > {stockQuantity}");
                                                        ExportWarehouseValidateDto.QuantityRequest = cellValue.Replace(',', '.');

                                                    }*/
                                                    else
                                                    {
                                                        ExportWarehouseValidateDto.QuantityRequest = cellValue.Replace(',', '.');

                                                    }
                                                }
                                            }
                                            catch
                                            {
                                                ExportWarehouseValidateDto.Errors.Add($"Quantityrequest {cellValue} notincorrectformat");
                                            }

                                            break;

                                        case "note":
                                            ExportWarehouseValidateDto.Note = cellValue;

                                            break;
                                        default:
                                            break;
                                    }
                                }
                            }
                        }

                        if (!String.IsNullOrEmpty(ExportWarehouseValidateDto.ProductCode))
                        {
                            result.Add(ExportWarehouseValidateDto);
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
