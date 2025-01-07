using DocumentFormat.OpenXml.Packaging;
using DocumentFormat.OpenXml.Spreadsheet;
using Microsoft.AspNetCore.Http;
using VFi.Application.SO.DTOs;
using VFi.Domain.SO.Interfaces;
using VFi.Domain.SO.Models;
using VFi.NetDevPack.FopExpression;
using VFi.NetDevPack.Queries;

namespace VFi.Application.SO.Queries;


public class PriceListQueryComboBox : IQuery<IEnumerable<PriceListListBoxDto>>
{
    public PriceListQueryComboBox(PriceListParams queryParams)
    {
        QueryParams = queryParams;
    }
    public PriceListParams QueryParams { get; set; }
}

public class PriceListQueryById : IQuery<PriceListDto>
{
    public PriceListQueryById()
    {
    }

    public PriceListQueryById(Guid id)
    {
        PriceListId = id;
    }

    public Guid PriceListId { get; set; }
}

public class PriceListPagingQuery : FopQuery, IQuery<PagedResult<List<PriceListDto>>>
{
    public PriceListPagingQuery(string? keyword, int? status, string filter, string order, int pageNumber, int pageSize)
    {
        Keyword = keyword;
        Filter = filter;
        Order = order;
        PageNumber = pageNumber;
        PageSize = pageSize;
        Status = status;
    }
}
public class PriceListExportTemplateQuery : IQuery<MemoryStream>
{
    public PriceListExportTemplateQuery()
    { }
}
public class ValidateExcelPriceListQuery : IQuery<List<PriceListValidateDto>>
{
    public ValidateExcelPriceListQuery(IFormFile file, string sheetId, int headerRow, List<ValidateField> listField)
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
public class PriceListQuery : IQueryHandler<PriceListQueryComboBox, IEnumerable<PriceListListBoxDto>>,
                                        IQueryHandler<PriceListQueryById, PriceListDto>,
                                         IQueryHandler<PriceListExportTemplateQuery, MemoryStream>,
                                         IQueryHandler<PriceListPagingQuery, PagedResult<List<PriceListDto>>>,
    IQueryHandler<ValidateExcelPriceListQuery, List<PriceListValidateDto>>
{
    private readonly IPriceListRepository _repository;
    private readonly IPIMRepository _PIMRepository;
    private readonly IExportTemplateRepository _exportTemplateRepository;
    public PriceListQuery(IPriceListRepository respository,
                    IExportTemplateRepository exportTemplateRepository,
        IPIMRepository PIMRepository)
    {
        _repository = respository;
        _PIMRepository = PIMRepository;
        _exportTemplateRepository = exportTemplateRepository;
    }

    public async Task<PriceListDto> Handle(PriceListQueryById request, CancellationToken cancellationToken)
    {
        var data = await _repository.GetById(request.PriceListId);
        var productPrice = await _PIMRepository.GetProductPrice(string.Join(",", data.PriceListDetail.Select(x => x.ProductId).ToArray()));
        var result = new PriceListDto()
        {
            Id = data.Id,
            Code = data.Code,
            Name = data.Name,
            Description = data.Description,
            Status = data.Status,
            StartDate = data.StartDate,
            EndDate = data.EndDate,
            Currency = data.Currency,
            CurrencyName = data.CurrencyName,
            DisplayOrder = data.DisplayOrder,
            CreatedDate = data.CreatedDate,
            UpdatedDate = data.UpdatedDate,
            CreatedBy = data.CreatedBy,
            UpdatedBy = data.UpdatedBy,
            CreatedByName = data.CreatedByName,
            UpdatedByName = data.UpdatedByName,
            PriceListDetail = data.PriceListDetail.Select(x => new PriceListDetailDto()
            {
                Id = x.Id,
                PriceListId = x.PriceListId,
                ProductId = x.ProductId,
                ProductCode = x.ProductCode,
                ProductName = x.ProductName,
                ProductPrice = productPrice.Where(y => y.Id == x.ProductId).FirstOrDefault()?.Price,
                ProductCurrency = productPrice.Where(y => y.Id == x.ProductId).FirstOrDefault()?.Currency,
                UnitType = x.UnitType,
                UnitCode = x.UnitCode,
                UnitName = x.UnitName,
                CurrencyCode = x.CurrencyCode,
                CurrencyName = x.CurrencyName,
                QuantityMin = x.QuantityMin,
                Type = x.Type,
                FixPrice = x.FixPrice,
                TypeDiscount = x.TypeDiscount,
                DiscountRate = x.DiscountRate,
                DiscountValue = x.DiscountValue,
                DisplayOrder = x.DisplayOrder,
                CreatedDate = x.CreatedDate,
                UpdatedDate = x.UpdatedDate,
                CreatedBy = x.CreatedBy,
                UpdatedBy = x.UpdatedBy,
                CreatedByName = x.CreatedByName,
                UpdatedByName = x.UpdatedByName
            }).OrderBy(x => x.DisplayOrder).ToList()
        };
        return result;
    }

    public async Task<PagedResult<List<PriceListDto>>> Handle(PriceListPagingQuery request, CancellationToken cancellationToken)
    {
        var response = new PagedResult<List<PriceListDto>>();
        var fopRequest = FopExpressionBuilder<PriceList>.Build(request.Filter, request.Order, request.PageNumber, request.PageSize);
        var filter = new Dictionary<string, object>();
        var filterListBox = new Dictionary<string, object>();
        if (request.Status != null)
            filterListBox.Add("status", request.Status.ToString());
        var (datas, count) = await _repository.Filter(request.Keyword, filterListBox, fopRequest);
        var data = datas.Select(x => new PriceListDto()
        {
            Id = x.Id,
            Code = x.Code,
            Name = x.Name,
            Description = x.Description,
            StartDate = x.StartDate,
            EndDate = x.EndDate,
            Status = x.Status,
            Currency = x.Currency,
            CurrencyName = x.CurrencyName,
        }).ToList();
        response.Items = data;
        response.TotalCount = count;
        response.PageNumber = request.PageNumber;
        response.PageSize = request.PageSize;
        return response;
    }

    public async Task<IEnumerable<PriceListListBoxDto>> Handle(PriceListQueryComboBox request, CancellationToken cancellationToken)
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
        if (request.QueryParams.ProductId != null)
        {
            filter.Add("productId", request.QueryParams.ProductId);
        }
        if (request.QueryParams.Quantity != null)
        {
            filter.Add("quantity", request.QueryParams.Quantity);
        }
        if (request.QueryParams.ProductId != null && request.QueryParams.Quantity != null)
        {
            filter.Add("quantity_productId", request.QueryParams.ProductId);
        }
        if (request.QueryParams.Quantity != null)
        {
            filter.Add("quantity", request.QueryParams.Quantity);
        }
        if (!string.IsNullOrEmpty(request.QueryParams.Currency))
        {
            filter.Add("currency", request.QueryParams.Currency);
        }
        var priceList = await _repository.GetListCbx(filter);
        var result = priceList.Select(x => new PriceListListBoxDto()
        {
            Key = x.Code,
            Value = x.Id,
            Label = x.Name,
            Currency = x.Currency,
            CurrencyName = x.CurrencyName,
            Details = x.PriceListDetail?.Select(x => new PriceListDetailDto()
            {
                Id = x.Id,
                PriceListId = x.PriceListId,
                ProductId = x.ProductId,
                ProductCode = x.ProductCode,
                ProductName = x.ProductName,
                UnitType = x.UnitType,
                UnitCode = x.UnitCode,
                UnitName = x.UnitName,
                CurrencyCode = x.CurrencyCode,
                CurrencyName = x.CurrencyName,
                QuantityMin = x.QuantityMin,
                Type = x.Type,
                FixPrice = x.FixPrice,
                TypeDiscount = x.TypeDiscount,
                DiscountRate = x.DiscountRate,
                DiscountValue = x.DiscountValue
            }).OrderBy(x => x.DisplayOrder).ToList()
        });
        return result;
    }
    public async Task<MemoryStream> Handle(PriceListExportTemplateQuery request, CancellationToken cancellationToken)
    {
        var contentBytes = await _exportTemplateRepository.GetTemplate("MAU_IMPORT_CHI_TIET_BANG_GIA");

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
    public async Task<List<PriceListValidateDto>> Handle(ValidateExcelPriceListQuery request, CancellationToken cancellationToken)
    {
        List<PriceListValidateDto> result = new List<PriceListValidateDto>();
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
                        PriceListValidateDto PriceListValidateDto = new PriceListValidateDto()
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
                                                PriceListValidateDto.Errors.Add($"productCode {cellValue} invalid");
                                            }
                                            else
                                            {
                                                PriceListValidateDto.ProductCode = cellValue;
                                            }
                                            break;
                                        case "productName":
                                            if (!String.IsNullOrEmpty(cellValue) && String.IsNullOrEmpty(PriceListValidateDto.ProductCode))
                                            {
                                                PriceListValidateDto.Errors.Add($"productName {cellValue} invalid");
                                                PriceListValidateDto.ProductName = cellValue;
                                            }
                                            break;

                                        case "type":
                                            PriceListValidateDto.Type = cellValue != null && cellValue != "" ? (cellValue == "Thiết lập giá" ? 1 : 0) : null;
                                            break;

                                        case "fixPrice":
                                            PriceListValidateDto.FixPrice = cellValue.Replace(',', '.');
                                            ;
                                            try
                                            {
                                                double amount;
                                                if (!String.IsNullOrEmpty(cellValue))
                                                {
                                                    if (!double.TryParse(cellValue, out amount))
                                                    {
                                                        PriceListValidateDto.Errors.Add($"FixPrice {cellValue} notincorrectformat");
                                                    }
                                                }
                                                else
                                                {
                                                    PriceListValidateDto.FixPrice = cellValue.Replace(',', '.');
                                                    ;
                                                }

                                            }

                                            catch
                                            {
                                                PriceListValidateDto.Errors.Add("FixPrice notincorrectformat");
                                            }



                                            break;
                                        case "quantityMin":
                                            PriceListValidateDto.QuantityMin = cellValue.Replace(',', '.');
                                            try
                                            {
                                                double quantityRequest;
                                                if (!String.IsNullOrEmpty(cellValue))
                                                {
                                                    if (!double.TryParse(cellValue, out quantityRequest))
                                                    {
                                                        PriceListValidateDto.Errors.Add($"QuantityMin {cellValue} notincorrectformat");
                                                    }
                                                    else
                                                    {
                                                        PriceListValidateDto.QuantityMin = cellValue.Replace(',', '.');

                                                    }
                                                }
                                            }
                                            catch
                                            {
                                                PriceListValidateDto.Errors.Add($"QuantityMin {cellValue} notincorrectformat");
                                            }


                                            break;

                                        default:
                                            break;
                                    }
                                }
                            }
                        }

                        if (!String.IsNullOrEmpty(PriceListValidateDto.ProductCode))
                        {
                            result.Add(PriceListValidateDto);
                        }
                    }
                }
            }
        }
        catch (Exception ex)
        {
        }
        var getlistunti = await _PIMRepository.GetUnitPaging(1, 500, null, null);
        var resultCode = result.Where(x => !string.IsNullOrEmpty(x.ProductCode)).Select(x => x.ProductCode);
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
                        a.UnitCode = getP.UnitCode;
                        a.UnitName = getP.UnitName;
                        a.ProductPrice = getP.Price;
                        a.CurrencyCode = getP.Currency;
                        /*var unit = getlistunti.FirstOrDefault(x => x.Code.Equals(a.UnitCode) && x.GroupUnitCode == a.UnitType);
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

                        }*/
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
                        a.Errors.Add($"ProductCode {a.ProductCode} invalid");
                    }
                }
            }
        }
        return result;
    }
}
