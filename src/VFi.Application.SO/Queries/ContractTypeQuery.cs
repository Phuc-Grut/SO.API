using VFi.Application.SO.DTOs;
using VFi.Domain.SO.Interfaces;
using VFi.Domain.SO.Models;
using VFi.NetDevPack.FopExpression;
using VFi.NetDevPack.Queries;


namespace VFi.Application.SO.Queries;

public class ContractTypeQueryById : IQuery<ContractTypeDto>
{
    public ContractTypeQueryById()
    {
    }

    public ContractTypeQueryById(Guid id)
    {
        Id = id;
    }

    public Guid Id { get; set; }
}
public class ContractTypeQueryAll : IQuery<IEnumerable<ContractTypeDto>>
{
    public ContractTypeQueryAll()
    {
    }
}

public class ContractTypeQueryComboBox : IQuery<IEnumerable<ComboBoxDto>>
{
    public ContractTypeQueryComboBox(int? status)
    {
        Status = status;
    }
    public int? Status { get; set; }
}
public class ContractTypeQueryCheckCode : IQuery<bool>
{

    public ContractTypeQueryCheckCode(string code)
    {
        Code = code;
    }

    public string Code { get; set; }
}

public class ContractTypePagingQuery : FopQuery, IQuery<PagedResult<List<ContractTypeDto>>>
{
    public ContractTypePagingQuery(string? keyword, int? status, string filter, string order, int pageNumber, int pageSize)
    {
        Filter = filter;
        Order = order;
        PageNumber = pageNumber;
        PageSize = pageSize;
        Status = status;
        Keyword = keyword;
    }
    public string Filter { get; set; }
    public string Order { get; set; }
    public int PageNumber { get; set; }
    public int PageSize { get; set; }
    public int? Status { get; set; }
    public string? Keyword { get; set; }

}
// lấy dữ liệu ,khai báo
public class ContractTypeQueryHandler : IQueryHandler<ContractTypeQueryComboBox, IEnumerable<ComboBoxDto>>,
                                         IQueryHandler<ContractTypeQueryAll, IEnumerable<ContractTypeDto>>,
                                         IQueryHandler<ContractTypeQueryCheckCode, bool>,
                                         IQueryHandler<ContractTypeQueryById, ContractTypeDto>,
                                         IQueryHandler<ContractTypePagingQuery, PagedResult<List<ContractTypeDto>>>
{
    private readonly IContractTypeRepository _repository;
    public ContractTypeQueryHandler(IContractTypeRepository contractTypeRespository)
    {
        _repository = contractTypeRespository;
    }
    public async Task<bool> Handle(ContractTypeQueryCheckCode request, CancellationToken cancellationToken)
    {
        var data = await _repository.GetByCode(request.Code);
        if (data is null)
        {
            return false;
        }
        return true;
    }

    public async Task<ContractTypeDto> Handle(ContractTypeQueryById request, CancellationToken cancellationToken)
    {
        var item = await _repository.GetById(request.Id);
        var result = new ContractTypeDto()
        {
            Id = item.Id,
            Code = item.Code,
            Name = item.Name,
            Description = item.Description,
            Status = item.Status,
            CreatedDate = item.CreatedDate,
            UpdatedDate = item.UpdatedDate,
            CreatedBy = item.CreatedBy,
            UpdatedBy = item.UpdatedBy,
            CreatedByName = item.CreatedByName,
            UpdatedByName = item.UpdatedByName,
        };
        return result;
    }

    public async Task<PagedResult<List<ContractTypeDto>>> Handle(ContractTypePagingQuery request, CancellationToken cancellationToken)
    {
        var response = new PagedResult<List<ContractTypeDto>>();

        var fopRequest = FopExpressionBuilder<ContractType>.Build(request.Filter, request.Order, request.PageNumber, request.PageSize);
        var filter = new Dictionary<string, object>();
        if (request.Status != null)
        {
            filter.Add("status", request.Status.ToString());
        }
        var (datas, count) = await _repository.Filter(request.Keyword, filter, fopRequest);

        var data = datas.Select(item => new ContractTypeDto()
        {
            Id = item.Id,
            Code = item.Code,
            Name = item.Name,
            Description = item.Description,
            Status = item.Status,
            CreatedDate = item.CreatedDate,
            UpdatedDate = item.UpdatedDate,
            CreatedBy = item.CreatedBy,
            UpdatedBy = item.UpdatedBy,
            CreatedByName = item.CreatedByName,
            UpdatedByName = item.UpdatedByName,

        }).ToList();
        response.Items = data;
        response.TotalCount = count;
        response.PageNumber = request.PageNumber;
        response.PageSize = request.PageSize;
        return response;
    }

    public async Task<IEnumerable<ContractTypeDto>> Handle(ContractTypeQueryAll request, CancellationToken cancellationToken)
    {
        var items = await _repository.GetAll();

        var result = items.Select(item => new ContractTypeDto()
        {
            Id = item.Id,
            Code = item.Code,
            Name = item.Name,
            Description = item.Description,
            Status = item.Status,
            CreatedDate = item.CreatedDate,
            UpdatedDate = item.UpdatedDate,
            CreatedBy = item.CreatedBy,
            UpdatedBy = item.UpdatedBy,
            CreatedByName = item.CreatedByName,
            UpdatedByName = item.UpdatedByName,

        });
        return result;
    }

    public async Task<IEnumerable<ComboBoxDto>> Handle(ContractTypeQueryComboBox request, CancellationToken cancellationToken)
    {

        var ContractTypes = await _repository.GetListCbx(request.Status);
        var result = ContractTypes.Select(x => new ComboBoxDto()
        {
            Key = x.Code,
            Value = x.Id,
            Label = x.Name
        });
        return result;
    }
}
