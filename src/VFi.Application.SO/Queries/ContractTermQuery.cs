using Microsoft.EntityFrameworkCore.Metadata.Internal;
using VFi.Application.SO.DTOs;
using VFi.Domain.SO.Interfaces;
using VFi.Domain.SO.Models;
using VFi.NetDevPack.FopExpression;
using VFi.NetDevPack.Queries;

namespace VFi.Application.SO.Queries;

public class ContractTermQueryAll : IQuery<IEnumerable<ContractTermDto>>
{
    public ContractTermQueryAll()
    {
    }
}

public class ContractTermQueryComboBox : IQuery<IEnumerable<ComboBoxDto>>
{
    public ContractTermQueryComboBox(int? status)
    {
        Status = status;
    }
    public int? Status { get; set; }
}
public class ContractTermQueryCheckCode : IQuery<bool>
{

    public ContractTermQueryCheckCode(string code)
    {
        Code = code;
    }

    public string Code { get; set; }
}
public class ContractTermQueryById : IQuery<ContractTermDto>
{
    public ContractTermQueryById()
    {
    }

    public ContractTermQueryById(Guid contractTermId)
    {
        ContractTermId = contractTermId;
    }

    public Guid ContractTermId { get; set; }
}
public class ContractTermPagingQuery : FopQuery, IQuery<PagedResult<List<ContractTermDto>>>
{
    public ContractTermPagingQuery(string? keyword, int? status, string filter, string order, int pageNumber, int pageSize)
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

public class ContractTermQueryHandler : IQueryHandler<ContractTermQueryComboBox, IEnumerable<ComboBoxDto>>,
                                         IQueryHandler<ContractTermQueryAll, IEnumerable<ContractTermDto>>,
                                         IQueryHandler<ContractTermQueryCheckCode, bool>,
                                         IQueryHandler<ContractTermQueryById, ContractTermDto>,
                                         IQueryHandler<ContractTermPagingQuery, PagedResult<List<ContractTermDto>>>
{
    private readonly IContractTermRepository _repository;
    public ContractTermQueryHandler(IContractTermRepository contractTermRespository)
    {
        _repository = contractTermRespository;
    }
    public async Task<bool> Handle(ContractTermQueryCheckCode request, CancellationToken cancellationToken)
    {
        var data = await _repository.GetByCode(request.Code);
        if (data is null)
        {
            return false;
        }
        return true;
    }

    public async Task<ContractTermDto> Handle(ContractTermQueryById request, CancellationToken cancellationToken)
    {
        var item = await _repository.GetById(request.ContractTermId);
        var result = new ContractTermDto()
        {
            Id = item.Id,
            Code = item.Code,
            Name = item.Name,
            Description = item.Description,
            Status = item.Status,
            CreatedDate = item.CreatedDate,
            CreatedBy = item.CreatedBy,
            UpdatedDate = item.UpdatedDate,
            UpdatedBy = item.UpdatedBy
        };
        return result;
    }

    public async Task<PagedResult<List<ContractTermDto>>> Handle(ContractTermPagingQuery request, CancellationToken cancellationToken)
    {
        var response = new PagedResult<List<ContractTermDto>>();

        var fopRequest = FopExpressionBuilder<ContractTerm>.Build(request.Filter, request.Order, request.PageNumber, request.PageSize);

        var (datas, count) = await _repository.Filter(request.Keyword, request.Status, fopRequest);
        var data = datas.Select(item => new ContractTermDto()
        {
            Id = item.Id,
            Code = item.Code,
            Name = item.Name,
            Description = item.Description,
            Status = item.Status,
            DisplayOrder = item.DisplayOrder,
            CreatedDate = item.CreatedDate,
            CreatedBy = item.CreatedBy,
            CreatedByName = item.CreatedByName,
            UpdatedByName = item.UpdatedByName,
            UpdatedDate = item.UpdatedDate,
            UpdatedBy = item.UpdatedBy
        }).OrderBy(x => x.DisplayOrder).ToList();
        response.Items = data;
        response.TotalCount = count;
        response.PageNumber = request.PageNumber;
        response.PageSize = request.PageSize;
        return response;
    }

    public async Task<IEnumerable<ContractTermDto>> Handle(ContractTermQueryAll request, CancellationToken cancellationToken)
    {
        var ContractTerms = await _repository.GetAll();
        var result = ContractTerms.Select(ContractTerm => new ContractTermDto()
        {
            Id = ContractTerm.Id,
            Code = ContractTerm.Code,
            Name = ContractTerm.Name,
            Description = ContractTerm.Description,
            Status = ContractTerm.Status,
            CreatedDate = ContractTerm.CreatedDate,
            UpdatedDate = ContractTerm.UpdatedDate,
            CreatedBy = ContractTerm.CreatedBy,
            UpdatedBy = ContractTerm.UpdatedBy
        });
        return result;
    }

    public async Task<IEnumerable<ComboBoxDto>> Handle(ContractTermQueryComboBox request, CancellationToken cancellationToken)
    {

        var ContractTerms = await _repository.GetListCbx(request.Status);
        var result = ContractTerms.Select(x => new ComboBoxDto()
        {
            Key = x.Code,
            Value = x.Id,
            Label = x.Name
        });
        return result;
    }
}
