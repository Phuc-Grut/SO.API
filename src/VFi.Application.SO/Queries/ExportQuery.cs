using Consul;
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

public class ExportQueryById : IQuery<ExportDto>
{
    public ExportQueryById()
    {
    }

    public ExportQueryById(Guid id)
    {
        Id = id;
    }

    public Guid Id { get; set; }
}

public class ExportQueryComboBox : IQuery<IEnumerable<ComboBoxDto>>
{
    public ExportQueryComboBox(int? status)
    {
        Status = status;
    }
    public int? Status { get; set; }
}
public class ExportQueryCheckCode : IQuery<bool>
{

    public ExportQueryCheckCode(string code)
    {
        Code = code;
    }

    public string Code { get; set; }
}

public class ExportPagingQuery : FopQuery, IQuery<PagedResult<List<ExportDto>>>
{
    public ExportPagingQuery(string? keyword, string filter, string order, int pageNumber, int pageSize)
    {
        Filter = filter;
        Order = order;
        PageNumber = pageNumber;
        PageSize = pageSize;
        Keyword = keyword;
    }
}
// lấy dữ liệu ,khai báo
public class ExportQueryHandler : IQueryHandler<ExportQueryComboBox, IEnumerable<ComboBoxDto>>,
                                         IQueryHandler<ExportQueryCheckCode, bool>,
                                         IQueryHandler<ExportQueryById, ExportDto>,
                                         IQueryHandler<ExportPagingQuery, PagedResult<List<ExportDto>>>
{
    private readonly IExportRepository _ExportRepository;
    private readonly IExportProductRepository _ExportProductRepository;
    private readonly IExportWarehouseProductRepository _ExportWarehouseProductRepository;
    private readonly IPIMRepository _pimRepository;

    public ExportQueryHandler(IExportRepository ExportRespository, IExportProductRepository exportProductRepository, IExportWarehouseProductRepository exportWarehouseProductRepository, IPIMRepository PimRepository)
    {
        _ExportRepository = ExportRespository;
        _ExportProductRepository = exportProductRepository;
        _ExportWarehouseProductRepository = exportWarehouseProductRepository;
        _pimRepository = PimRepository;
    }
    public async Task<bool> Handle(ExportQueryCheckCode request, CancellationToken cancellationToken)
    {
        var data = await _ExportRepository.GetByCode(request.Code);
        if (data is null)
        {
            return false;
        }
        return true;
    }

    public async Task<ExportDto> Handle(ExportQueryById request, CancellationToken cancellationToken)
    {
        var obj = await _ExportRepository.GetById(request.Id);
        var filter = new Dictionary<string, object>();
        var exportWarehouseProductId = String.Join(",", obj.ExportProducts?.Where(x => x.ExportWarehouseProductId != null).Select(g => g.ExportWarehouseProductId.ToString()).ToArray());
        filter.Add("id", exportWarehouseProductId);
        var detailExport = await _ExportWarehouseProductRepository.Filter(filter);
        var inventory = new List<INVENTORY_DETAIL_BY_LISTID>();
        var productPrice = new List<SP_GET_PRODUCT_PRICE_BY_LISTID>();
        var listId = new List<string>();
        var listProductId = new List<Guid?>();
        var i = 1;
        var source = obj.ExportProducts.Select(x => x.ProductId).Distinct();
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
            var dataInventory = await _pimRepository.GetInventoryDetail(o);
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
        }
        var result = new ExportDto()
        {
            Id = obj.Id,
            ExportWarehouseId = obj.ExportWarehouseId,
            Code = obj.Code,
            ExportDate = obj.ExportDate,
            EmployeeId = obj.EmployeeId,
            EmployeeCode = obj.EmployeeCode,
            EmployeeName = obj.EmployeeName,
            Status = obj.Status,
            TotalQuantity = obj.TotalQuantity,
            Note = obj.Note,
            File = JsonConvert.DeserializeObject<List<FileDto>>(string.IsNullOrEmpty(obj.File) ? "" : obj.File),
            ApproveBy = obj.ApproveBy,
            ApproveDate = obj.ApproveDate,
            ApproveByName = obj.ApproveByName,
            ApproveComment = obj.ApproveComment,
            CreatedBy = obj.CreatedBy,
            CreatedDate = obj.CreatedDate,
            UpdatedBy = obj.UpdatedBy,
            UpdatedDate = obj.UpdatedDate,
            CreatedByName = obj.CreatedByName,
            UpdatedByName = obj.UpdatedByName,
            Details = obj.ExportProducts.Select(x => new ExportProductDto()
            {
                Id = x.Id,
                ExportId = x.ExportId,
                ExportWarehouseProductId = x.ExportWarehouseProductId,
                ProductId = x.ProductId,
                ProductCode = x.ProductCode,
                ProductName = x.ProductName,
                ProductImage = x.ProductImage,
                Origin = x.Origin,
                UnitType = x.UnitType,
                UnitCode = x.UnitCode,
                UnitName = x.UnitName,
                StockQuantity = inventory.Where(y => y.ProductId == x.ProductId).Sum(y => y.StockQuantity),
                QuantityRequest = detailExport.Where(f => f.Id == x.ExportWarehouseProductId).Sum(f => f.QuantityRequest),
                Quantity = x.Quantity,
                Note = x.Note,
                DisplayOrder = x.DisplayOrder

            }).OrderBy(x => x.DisplayOrder).ToList()
        };

        return result;
    }

    public async Task<PagedResult<List<ExportDto>>> Handle(ExportPagingQuery request, CancellationToken cancellationToken)
    {
        var response = new PagedResult<List<ExportDto>>();

        var fopRequest = FopExpressionBuilder<Export>.Build(request.Filter, request.Order, request.PageNumber, request.PageSize);

        var (Export, count) = await _ExportRepository.Filter(request.Keyword, fopRequest);

        var data = Export.Select(obj => new ExportDto()
        {
            Id = obj.Id,
            ExportWarehouseId = obj.ExportWarehouseId,
            Code = obj.Code,
            ExportDate = obj.ExportDate,
            EmployeeId = obj.EmployeeId,
            EmployeeCode = obj.EmployeeCode,
            EmployeeName = obj.EmployeeName,
            Status = obj.Status,
            TotalQuantity = obj.TotalQuantity,
            Note = obj.Note,
            ApproveBy = obj.ApproveBy,
            ApproveDate = obj.ApproveDate,
            ApproveByName = obj.ApproveByName,
            ApproveComment = obj.ApproveComment,
            CreatedBy = obj.CreatedBy,
            CreatedDate = obj.CreatedDate,
            UpdatedBy = obj.UpdatedBy,
            UpdatedDate = obj.UpdatedDate,
            CreatedByName = obj.CreatedByName,
            UpdatedByName = obj.UpdatedByName,

        }).ToList();
        response.Items = data;
        response.TotalCount = count;
        response.PageNumber = request.PageNumber;
        response.PageSize = request.PageSize;
        return response;
    }

    public async Task<IEnumerable<ComboBoxDto>> Handle(ExportQueryComboBox request, CancellationToken cancellationToken)
    {

        var Export = await _ExportRepository.GetListCbx(request.Status);
        var result = Export.Select(x => new ComboBoxDto()
        {
            Key = x.Code,
            Value = x.Id,
            Label = x.Code
        });
        return result;
    }
}
