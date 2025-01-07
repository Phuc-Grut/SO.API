using VFi.Application.SO.DTOs;
using VFi.Domain.SO.Interfaces;
using VFi.Domain.SO.Models;
using VFi.NetDevPack.FopExpression;
using VFi.NetDevPack.Queries;


namespace VFi.Application.SO.Queries;

public class StorePriceListQueryById : IQuery<StorePriceListDto>
{
    public StorePriceListQueryById()
    {
    }

    public StorePriceListQueryById(Guid id)
    {
        Id = id;
    }

    public Guid Id { get; set; }
}
public class StorePriceListQueryAll : IQuery<IEnumerable<StorePriceListDto>>
{
    public StorePriceListQueryAll()
    {
    }
}

public class StorePriceListQueryComboBox : IQuery<IEnumerable<ComboBoxDto>>
{
    public StorePriceListQueryComboBox(int? status)
    {
        Status = status;
    }
    public int? Status { get; set; }
}
public class StorePriceListQueryCheckCode : IQuery<bool>
{

    public StorePriceListQueryCheckCode(string code)
    {
        Code = code;
    }

    public string Code { get; set; }
}

public class StorePriceListPagingQuery : FopQuery, IQuery<PagedResult<List<StorePriceListDto>>>
{
    public StorePriceListPagingQuery(string filter, string order, int pageNumber, int pageSize)
    {
        Filter = filter;
        Order = order;
        PageNumber = pageNumber;
        PageSize = pageSize;
    }
    public string Filter { get; set; }
    public string Order { get; set; }
    public int PageNumber { get; set; }
    public int PageSize { get; set; }
}
public class StorePriceListQueryHandler : IQueryHandler<StorePriceListQueryComboBox, IEnumerable<ComboBoxDto>>,
                                         IQueryHandler<StorePriceListQueryAll, IEnumerable<StorePriceListDto>>,
                                         IQueryHandler<StorePriceListQueryCheckCode, bool>,
                                         IQueryHandler<StorePriceListQueryById, StorePriceListDto>,
                                         IQueryHandler<StorePriceListPagingQuery, PagedResult<List<StorePriceListDto>>>
{
    private readonly IStorePriceListRepository _StorePriceListRepository;
    private readonly IEmployeeRepository _EmployeeRepository;
    private readonly ICustomerGroupRepository _CustomerGroupRepository;
    public StorePriceListQueryHandler(IStorePriceListRepository StorePriceListRespository, IEmployeeRepository employeeRepository, ICustomerGroupRepository customerGroupRepository)
    {
        _StorePriceListRepository = StorePriceListRespository;
        _EmployeeRepository = employeeRepository;
        _CustomerGroupRepository = customerGroupRepository;
    }
    public async Task<bool> Handle(StorePriceListQueryCheckCode request, CancellationToken cancellationToken)
    {
        var data = await _StorePriceListRepository.GetByCode(request.Code);
        if (data is null)
        {
            return false;
        }
        return true;
    }

    public async Task<StorePriceListDto> Handle(StorePriceListQueryById request, CancellationToken cancellationToken)
    {
        var StorePriceList = await _StorePriceListRepository.GetById(request.Id);
        var result = new StorePriceListDto()
        {
            Id = StorePriceList.Id,
            StoreId = StorePriceList.StoreId,
            PriceListId = StorePriceList.PriceListId,
            PriceListName = StorePriceList.PriceListName,
            Default = StorePriceList.Default,
            CreatedDate = StorePriceList.CreatedDate.Value,
            UpdatedDate = StorePriceList.UpdatedDate,
            CreatedBy = StorePriceList.CreatedBy,
            UpdatedBy = StorePriceList.UpdatedBy,
            DisplayOrder = StorePriceList.DisplayOrder,
            CreatedByName = StorePriceList.CreatedByName,
            UpdatedByName = StorePriceList.UpdatedByName,
        };
        return result;
    }

    public async Task<PagedResult<List<StorePriceListDto>>> Handle(StorePriceListPagingQuery request, CancellationToken cancellationToken)
    {
        var response = new PagedResult<List<StorePriceListDto>>();

        var fopRequest = FopExpressionBuilder<StorePriceList>.Build(request.Filter, request.Order, request.PageNumber, request.PageSize);

        var (StorePriceLists, count) = await _StorePriceListRepository.Filter(fopRequest);

        var data = StorePriceLists.Select(StorePriceList => new StorePriceListDto()
        {
            Id = StorePriceList.Id,
            StoreId = StorePriceList.StoreId,
            PriceListId = StorePriceList.PriceListId,
            PriceListName = StorePriceList.PriceListName,
            Default = StorePriceList.Default,
            CreatedDate = StorePriceList.CreatedDate,
            UpdatedDate = StorePriceList.UpdatedDate,
            CreatedBy = StorePriceList.CreatedBy,
            UpdatedBy = StorePriceList.UpdatedBy,
            DisplayOrder = StorePriceList.DisplayOrder,
            CreatedByName = StorePriceList.CreatedByName,
            UpdatedByName = StorePriceList.UpdatedByName,

        }).OrderBy(x => x.DisplayOrder).ToList();
        response.Items = data;
        response.TotalCount = count;
        response.PageNumber = request.PageNumber;
        response.PageSize = request.PageSize;
        return response;
    }

    public async Task<IEnumerable<StorePriceListDto>> Handle(StorePriceListQueryAll request, CancellationToken cancellationToken)
    {
        var StorePriceLists = await _StorePriceListRepository.GetAll();

        var result = StorePriceLists.Select(StorePriceList => new StorePriceListDto()
        {
            Id = StorePriceList.Id,
            StoreId = StorePriceList.StoreId,
            PriceListId = StorePriceList.PriceListId,
            PriceListName = StorePriceList.PriceListName,
            Default = StorePriceList.Default,
            CreatedDate = StorePriceList.CreatedDate.Value,
            UpdatedDate = StorePriceList.UpdatedDate,
            CreatedBy = StorePriceList.CreatedBy,
            UpdatedBy = StorePriceList.UpdatedBy,
            DisplayOrder = StorePriceList.DisplayOrder,
            CreatedByName = StorePriceList.CreatedByName,
            UpdatedByName = StorePriceList.UpdatedByName,

        });
        return result;
    }

    public async Task<IEnumerable<ComboBoxDto>> Handle(StorePriceListQueryComboBox request, CancellationToken cancellationToken)
    {

        var StorePriceLists = await _StorePriceListRepository.GetListCbx(request.Status);
        var result = StorePriceLists.Select(x => new ComboBoxDto()
        {
            Key = x.CreatedByName,
            Value = x.Id,
            Label = x.CreatedByName
        });
        return result;
    }
}
