using Microsoft.EntityFrameworkCore.Metadata.Internal;
using VFi.Application.SO.DTOs;
using VFi.Domain.SO.Interfaces;
using VFi.Domain.SO.Models;
using VFi.NetDevPack.FopExpression;
using VFi.NetDevPack.Queries;

namespace VFi.Application.SO.Queries;

public class ExpenseQueryAll : IQuery<IEnumerable<ExpenseDto>>
{
    public ExpenseQueryAll()
    {
    }
}

public class ExpenseQueryComboBox : IQuery<IEnumerable<ComboBoxDto>>
{
    public ExpenseQueryComboBox(int? status)
    {
        Status = status;
    }
    public int? Status { get; set; }
}
public class ExpenseQueryCheckCode : IQuery<bool>
{

    public ExpenseQueryCheckCode(string code)
    {
        Code = code;
    }

    public string Code { get; set; }
}
public class ExpenseQueryById : IQuery<ExpenseDto>
{
    public ExpenseQueryById()
    {
    }

    public ExpenseQueryById(Guid expenseId)
    {
        ExpenseId = expenseId;
    }

    public Guid ExpenseId { get; set; }
}
public class ExpensePagingQuery : FopQuery, IQuery<PagedResult<List<ExpenseDto>>>
{
    public ExpensePagingQuery(string? keyword, int? status, string filter, string order, int pageNumber, int pageSize)
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

public class ExpenseQueryHandler : IQueryHandler<ExpenseQueryComboBox, IEnumerable<ComboBoxDto>>,
                                         IQueryHandler<ExpenseQueryAll, IEnumerable<ExpenseDto>>,
                                         IQueryHandler<ExpenseQueryCheckCode, bool>,
                                         IQueryHandler<ExpenseQueryById, ExpenseDto>,
                                         IQueryHandler<ExpensePagingQuery, PagedResult<List<ExpenseDto>>>
{
    private readonly IExpenseRepository _repository;
    public ExpenseQueryHandler(IExpenseRepository expenseRespository)
    {
        _repository = expenseRespository;
    }
    public async Task<bool> Handle(ExpenseQueryCheckCode request, CancellationToken cancellationToken)
    {
        var data = await _repository.GetByCode(request.Code);
        if (data is null)
        {
            return false;
        }
        return true;
    }

    public async Task<ExpenseDto> Handle(ExpenseQueryById request, CancellationToken cancellationToken)
    {
        var item = await _repository.GetById(request.ExpenseId);
        var result = new ExpenseDto()
        {
            Id = item.Id,
            Code = item.Code,
            Name = item.Name,
            Description = item.Description,
            Status = item.Status,
            CreatedDate = item.CreatedDate,
            CreatedBy = item.CreatedBy,
            UpdatedDate = item.UpdatedDate,
            UpdatedBy = item.UpdatedBy,
            CreatedByName = item.CreatedByName,
            DisplayOrder = item.DisplayOrder,
            UpdatedByName = item.UpdatedByName
        };
        return result;
    }

    public async Task<PagedResult<List<ExpenseDto>>> Handle(ExpensePagingQuery request, CancellationToken cancellationToken)
    {
        var response = new PagedResult<List<ExpenseDto>>();

        var fopRequest = FopExpressionBuilder<Expense>.Build(request.Filter, request.Order, request.PageNumber, request.PageSize);

        var (datas, count) = await _repository.Filter(request.Keyword, request.Status, fopRequest);
        var data = datas.Select(item => new ExpenseDto()
        {
            Id = item.Id,
            Code = item.Code,
            Name = item.Name,
            Description = item.Description,
            Status = item.Status,
            CreatedDate = item.CreatedDate,
            CreatedBy = item.CreatedBy,
            UpdatedDate = item.UpdatedDate,
            UpdatedBy = item.UpdatedBy,
            CreatedByName = item.CreatedByName,
            DisplayOrder = item.DisplayOrder,
            UpdatedByName = item.UpdatedByName
        }).ToList();
        response.Items = data;
        response.TotalCount = count;
        response.PageNumber = request.PageNumber;
        response.PageSize = request.PageSize;
        return response;
    }

    public async Task<IEnumerable<ExpenseDto>> Handle(ExpenseQueryAll request, CancellationToken cancellationToken)
    {
        var items = await _repository.GetAll();
        var result = items.Select(item => new ExpenseDto()
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
            DisplayOrder = item.DisplayOrder,
            UpdatedByName = item.UpdatedByName
        });
        return result;
    }

    public async Task<IEnumerable<ComboBoxDto>> Handle(ExpenseQueryComboBox request, CancellationToken cancellationToken)
    {

        var items = await _repository.GetListCbx(request.Status);
        var result = items.Select(x => new ComboBoxDto()
        {
            Key = x.Code,
            Value = x.Id,
            Label = x.Name
        });
        return result;
    }
}
