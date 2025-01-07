using DocumentFormat.OpenXml.Packaging;
using DocumentFormat.OpenXml.Spreadsheet;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using VFi.Application.SO.DTOs;
using VFi.Domain.SO.Interfaces;
using VFi.Domain.SO.Models;
using VFi.NetDevPack.FopExpression;
using VFi.NetDevPack.Queries;

namespace VFi.Application.SO.Queries;


public class ProductionOrderQueryComboBox : IQuery<IEnumerable<ComboBoxDto>>
{
    public ProductionOrderQueryComboBox(int? status)
    {
        Status = status;
    }
    public int? Status { get; set; }
}
public class ProductionOrderQueryCheckCode : IQuery<bool>
{

    public ProductionOrderQueryCheckCode(string code)
    {
        Code = code;
    }

    public string Code { get; set; }
}
public class ProductionOrderQueryById : IQuery<ProductionOrderDto>
{
    public ProductionOrderQueryById()
    {
    }

    public ProductionOrderQueryById(Guid areaId)
    {
        ProductionOrderId = areaId;
    }

    public Guid ProductionOrderId { get; set; }
}
public class ProductionOrderQueryByCode : IQuery<ProductionOrderDto>
{
    public ProductionOrderQueryByCode()
    {
    }

    public ProductionOrderQueryByCode(string code)
    {
        Code = code;
    }

    public string Code { get; set; }
}

public class ProductionOrderPagingFilterQuery : FopQuery, IQuery<PagedResult<List<ProductionOrderDto>>>
{
    public ProductionOrderPagingFilterQuery(string keyword, int? status, string filter, string order, int pageNumber, int pageSize)
    {
        Keyword = keyword;
        Status = status;
        Filter = filter;
        Order = order;
        PageNumber = pageNumber;
        PageSize = pageSize;
    }
}
public class ProductionOrderExportTemplateQuery : IQuery<MemoryStream>
{
    public ProductionOrderExportTemplateQuery(int type)
    {
        Type = type;
    }
    public int Type { get; set; }
}
public class ValidateProductionOrdersQuery : IQuery<List<ProductionOrdersValidateDto>>
{
    public ValidateProductionOrdersQuery(IFormFile file, string sheetId, int headerRow, List<ValidateField> listField)
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
public class ProductionOrderQueryHandler : IQueryHandler<ProductionOrderQueryById, ProductionOrderDto>,
                                         IQueryHandler<ProductionOrderQueryByCode, ProductionOrderDto>,
                                         IQueryHandler<ProductionOrderPagingFilterQuery, PagedResult<List<ProductionOrderDto>>>,
    IQueryHandler<ProductionOrderExportTemplateQuery, MemoryStream>, IQueryHandler<ValidateProductionOrdersQuery, List<ProductionOrdersValidateDto>>
{
    private readonly IProductionOrderRepository _ProductionOrderRepository;
    private readonly IProductionOrdersDetailRepository _ProductionOrdersDetailRepository;
    private readonly IExportTemplateRepository _exportTemplateRepository;
    private readonly IPIMRepository _pimRepository;
    public ProductionOrderQueryHandler(
        IProductionOrderRepository ProductionOrderRespository,
        IProductionOrdersDetailRepository ProductionOrdersDetailRepository, IExportTemplateRepository exportTemplateRepository, IPIMRepository PimRepository
     )
    {
        _ProductionOrderRepository = ProductionOrderRespository;
        _exportTemplateRepository = exportTemplateRepository;
        _ProductionOrdersDetailRepository = ProductionOrdersDetailRepository;
        _pimRepository = PimRepository;
    }

    public async Task<ProductionOrderDto> Handle(ProductionOrderQueryById request, CancellationToken cancellationToken)
    {
        var Parent = new ProductionOrder();
        var data = await _ProductionOrderRepository.GetById(request.ProductionOrderId);
        var listProductionOrdersDetail = await _ProductionOrdersDetailRepository.GetAll(request.ProductionOrderId);
        var result = new ProductionOrderDto()
        {
            Id = data.Id,
            Code = data.Code,
            Note = data.Note,
            Status = data.Status,
            RequestDate = data.RequestDate,
            CustomerId = data.CustomerId,
            CustomerCode = data.CustomerCode,
            CustomerName = data.CustomerName,
            Email = data.Email,
            Phone = data.Phone,
            Address = data.Address,
            EmployeeId = data.EmployeeId,
            EmployeeCode = data.EmployeeCode,
            EmployeeName = data.EmployeeName,
            DateNeed = data.DateNeed,
            OrderId = data.OrderId,
            OrderNumber = data.OrderNumber,
            SaleEmployeeId = data.SaleEmployeeId,
            SaleEmployeeCode = data.SaleEmployeeCode,
            SaleEmployeeName = data.SaleEmployeeName,
            Type = data.Type,
            EstimateDate = data.EstimateDate,
            File = JsonConvert.DeserializeObject<List<FileDto>>(string.IsNullOrEmpty(data.File) ? "" : data.File),
            CreatedBy = data.CreatedBy,
            CreatedDate = data.CreatedDate,
            UpdatedBy = data.UpdatedBy,
            UpdatedDate = data.UpdatedDate,
            CreatedByName = data.CreatedByName,
            UpdatedByName = data.UpdatedByName,
            ListProductionOrdersDetail = listProductionOrdersDetail.Select(x => new ProductionOrdersDetailDto()
            {
                Id = x.Id,
                OrderId = x.OrderId,
                OrderCode = x.OrderCode,
                ProductionOrdersId = x.ProductionOrdersId,
                OrderProductId = x.OrderProductId,
                ProductId = x.ProductId,
                ProductCode = x.ProductCode,
                ProductName = x.ProductName,
                ProductImage = x.ProductImage,
                Sku = x.Sku,
                Gtin = x.Gtin,
                Origin = x.Origin,
                UnitType = x.UnitType,
                UnitCode = x.UnitCode,
                UnitName = x.UnitName,
                Quantity = x.Quantity,
                Note = x.Note,
                EstimatedDeliveryQuantity = x.EstimatedDeliveryQuantity,
                DeliveryDate = x.DeliveryDate,
                IsWorkOrdered = x.IsWorkOrdered,
                ProductionOrdersCode = x.ProductionOrdersCode,
                DisplayOrder = x.DisplayOrder,
                CreatedBy = x.CreatedBy,
                CreatedDate = x.CreatedDate,
                CreatedByName = x.CreatedByName,
            }).OrderBy(x => x.DisplayOrder).ToList()
        };
        return result;
    }
    public async Task<ProductionOrderDto> Handle(ProductionOrderQueryByCode request, CancellationToken cancellationToken)
    {
        var data = await _ProductionOrderRepository.GetByCode(request.Code);
        var result = new ProductionOrderDto()
        {
            Id = data.Id,
            Code = data.Code,
            Note = data.Note,
            Status = data.Status,
            RequestDate = data.RequestDate,
            CustomerId = data.CustomerId,
            CustomerCode = data.CustomerCode,
            CustomerName = data.CustomerName,
            Email = data.Email,
            Phone = data.Phone,
            Address = data.Address,
            EmployeeId = data.EmployeeId,
            EmployeeCode = data.EmployeeCode,
            EmployeeName = data.EmployeeName,
            DateNeed = data.DateNeed,
            OrderId = data.OrderId,
            OrderNumber = data.OrderNumber,
            SaleEmployeeId = data.SaleEmployeeId,
            SaleEmployeeCode = data.SaleEmployeeCode,
            SaleEmployeeName = data.SaleEmployeeName,
            Type = data.Type,
            EstimateDate = data.EstimateDate,
            CreatedBy = data.CreatedBy,
            CreatedDate = data.CreatedDate,
            UpdatedBy = data.UpdatedBy,
            UpdatedDate = data.UpdatedDate,
            CreatedByName = data.CreatedByName,
            UpdatedByName = data.UpdatedByName
        };
        return result;
    }

    public async Task<PagedResult<List<ProductionOrderDto>>> Handle(ProductionOrderPagingFilterQuery request, CancellationToken cancellationToken)
    {
        var response = new PagedResult<List<ProductionOrderDto>>();
        var fopRequest = FopExpressionBuilder<ProductionOrder>.Build(request.Filter, request.Order, request.PageNumber, request.PageSize);
        var filter = new Dictionary<string, object>();
        if (request.Status != null)
            filter.Add("status", request.Status.ToString());
        var (datas, count) = await _ProductionOrderRepository.Filter(request.Keyword, filter, fopRequest);
        var result = datas.Select(item =>
        {
            return new ProductionOrderDto()
            {
                Id = item.Id,
                Code = item.Code,
                Note = item.Note,
                Status = item.Status,
                RequestDate = item.RequestDate,
                CustomerId = item.CustomerId,
                CustomerCode = item.CustomerCode,
                CustomerName = item.CustomerName,
                Email = item.Email,
                Phone = item.Phone,
                Address = item.Address,
                EmployeeId = item.EmployeeId,
                EmployeeCode = item.EmployeeCode,
                EmployeeName = item.EmployeeName,
                DateNeed = item.DateNeed,
                OrderNumber = item.OrderNumber,
                SaleEmployeeId = item.SaleEmployeeId,
                SaleEmployeeCode = item.SaleEmployeeCode,
                SaleEmployeeName = item.SaleEmployeeName,
                Type = item.Type,
                EstimateDate = item.EstimateDate,
                CreatedBy = item.CreatedBy,
                CreatedDate = item.CreatedDate,
                CreatedByName = item.CreatedByName,
            };
        }
        ).ToList();
        response.Items = result;
        response.TotalCount = count;
        response.PageNumber = request.PageNumber;
        response.PageSize = request.PageSize;
        return response;
    }

    public async Task<MemoryStream> Handle(ProductionOrderExportTemplateQuery request, CancellationToken cancellationToken)
    {
        MemoryStream memoryStream = new();

        if (request.Type == 1)
        {
            var contentBytes = await _exportTemplateRepository.GetTemplate("MAU_IMPORT_CHI_TIET_YEU_CAU_DU_TOAN");


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

        }
        else if (request.Type == 2)
        {
            var contentBytes = await _exportTemplateRepository.GetTemplate("MAU_IMPORT_CHI_TIET_YEU_CAU_SAN_XUAT");


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

    public async Task<List<ProductionOrdersValidateDto>> Handle(ValidateProductionOrdersQuery request, CancellationToken cancellationToken)
    {
        List<ProductionOrdersValidateDto> result = new List<ProductionOrdersValidateDto>();
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
                        ProductionOrdersValidateDto ProductionOrdersValidateDto = new ProductionOrdersValidateDto()
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
                                                ProductionOrdersValidateDto.Errors.Add("productCode invalid");
                                            }
                                            else
                                            {
                                                ProductionOrdersValidateDto.ProductCode = cellValue;
                                            }

                                            break;
                                        case "productName":
                                            if (!String.IsNullOrEmpty(cellValue) && String.IsNullOrEmpty(ProductionOrdersValidateDto.ProductCode))
                                            {
                                                ProductionOrdersValidateDto.ProductName = cellValue;
                                                ProductionOrdersValidateDto.Errors.Add($"ProductName {cellValue} invalid");

                                            }
                                            break;
                                        case "unitCode":
                                            if (String.IsNullOrEmpty(cellValue))
                                            {
                                                ProductionOrdersValidateDto.Errors.Add("unitCode invalid");
                                            }
                                            else
                                            {
                                                ProductionOrdersValidateDto.UnitCode = cellValue;

                                            }
                                            break;
                                        case "unitName":
                                            if (!String.IsNullOrEmpty(cellValue) && String.IsNullOrEmpty(ProductionOrdersValidateDto.UnitCode))
                                            {
                                                ProductionOrdersValidateDto.UnitName = cellValue;
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
                                                        ProductionOrdersValidateDto.Errors.Add("Quantityrequest notincorrectformat");
                                                    }
                                                }
                                            }
                                            catch
                                            {
                                                ProductionOrdersValidateDto.Errors.Add("Quantityrequest notincorrectformat");
                                            }
                                            ProductionOrdersValidateDto.QuantityRequest = cellValue.Replace(',', '.');

                                            break;

                                        case "note":
                                            ProductionOrdersValidateDto.Note = cellValue;

                                            break;
                                        default:
                                            break;
                                    }
                                }
                            }
                        }

                        if (!String.IsNullOrEmpty(ProductionOrdersValidateDto.ProductCode))
                        {
                            result.Add(ProductionOrdersValidateDto);
                        }
                    }
                }
            }
        }
        catch (Exception ex)
        {
        }
        var getlistunti = await _pimRepository.GetUnitPaging(1, 500, null, null);
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
                        //a.StockQuantity = getP.StockQuantity;
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
