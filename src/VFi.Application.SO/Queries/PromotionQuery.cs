using MediatR;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using VFi.Application.SO.DTOs;
using VFi.Domain.SO.Interfaces;
using VFi.Domain.SO.Models;
using VFi.Infra.SO.Repository;
using VFi.NetDevPack.FopExpression;
using VFi.NetDevPack.Queries;

namespace VFi.Application.SO.Queries;


public class PromotionQueryAll : IQuery<IEnumerable<PromotionDto>>
{
    public PromotionQueryAll()
    {
    }
}

public class PromotionQueryComboBox : IQuery<IEnumerable<ComboBoxDto>>
{
    public PromotionQueryComboBox(int? status)
    {
        Status = status;
    }
    public int? Status { get; set; }
}

public class PromotionQueryById : IQuery<PromotionDto>
{
    public PromotionQueryById()
    {
    }

    public PromotionQueryById(Guid id)
    {
        PromotionId = id;
    }

    public Guid PromotionId { get; set; }
}

public class PromotionPagingFilterQuery : FopQuery, IQuery<PagedResult<List<PromotionDto>>>
{
    public PromotionPagingFilterQuery(string keyword, int? status, string filter, string order, int pageNumber, int pageSize, Guid? promotionGroupId)
    {
        Keyword = keyword;
        Status = status;
        Filter = filter;
        Order = order;
        PageNumber = pageNumber;
        PageSize = pageSize;
        PromotionGroupId = promotionGroupId;
    }
    public Guid? PromotionGroupId { get; set; }
}

public class PromotionQuery : IQueryHandler<PromotionQueryComboBox, IEnumerable<ComboBoxDto>>,
                                        IQueryHandler<PromotionQueryAll, IEnumerable<PromotionDto>>,
                                        IQueryHandler<PromotionQueryById, PromotionDto>,
                                         IQueryHandler<PromotionPagingFilterQuery, PagedResult<List<PromotionDto>>>
{
    private readonly IPromotionRepository _repository;
    private readonly IPromotionCustomerRepository _promotionCustomerRepository;
    private readonly ICustomerRepository _customerRepository;
    private readonly IPromotionCustomerGroupRepository _promotionCustomerGroupRepository;
    private readonly ICustomerGroupRepository _customerGroupRepository;
    public PromotionQuery(
                    IPromotionRepository respository,
                    IPromotionCustomerRepository promotionCustomerRepository,
                    ICustomerRepository customerRepository,
                    IPromotionCustomerGroupRepository promotionCustomerGroupRepository,
                    ICustomerGroupRepository customerGroupRepository
        )
    {
        _repository = respository;
        _promotionCustomerRepository = promotionCustomerRepository;
        _customerRepository = customerRepository;
        _promotionCustomerGroupRepository = promotionCustomerGroupRepository;
        _customerGroupRepository = customerGroupRepository;
    }

    public async Task<PromotionDto> Handle(PromotionQueryById request, CancellationToken cancellationToken)
    {
        var data = await _repository.GetById(request.PromotionId);

        var filter = new Dictionary<string, object>();
        filter.Add("promotionId", request.PromotionId);
        var listGroup = await _promotionCustomerGroupRepository.GetListListBox(filter);
        var groups = await _customerGroupRepository.Filter(listGroup.Select(x => x.CustomerGroupId.Value).ToList());

        var listCustomer = await _promotionCustomerRepository.GetListListBox(filter);
        var customers = await _customerRepository.Filter(listCustomer.Select(x => x.CustomerId).ToList());
        var result = new PromotionDto()
        {
            Id = data.Id,
            PromotionGroupId = data.PromotionGroupId,
            Code = data.Code,
            Name = data.Name,
            Description = data.Description,
            Status = data.Status,
            StartDate = data.StartDate,
            EndDate = data.EndDate,
            StartTime = data.StartTime,
            EndTime = data.EndTime,
            Stores = data.Stores,
            SalesChannel = data.SalesChannel,
            ApplyTogether = data.ApplyTogether,
            ApplyAllCustomer = data.ApplyAllCustomer,
            Type = data.Type,
            PromotionMethod = data.PromotionMethod,
            UsingCode = data.UsingCode,
            ApplyBirthday = data.ApplyBirthday,
            PromotionalCode = data.PromotionalCode,
            IsLimit = data.IsLimit,
            PromotionLimit = data.PromotionLimit,
            Applytax = data.Applytax,
            DisplayType = data.DisplayType,
            PromotionBase = data.PromotionBase,
            ObjectApply = data.ObjectApply,
            Condition = data.Condition,
            Apply = data.Apply,
            CreatedDate = data.CreatedDate,
            CreatedBy = data.CreatedBy,
            UpdatedDate = data.UpdatedDate,
            UpdatedBy = data.UpdatedBy,
            CreatedByName = data.CreatedByName,
            UpdatedByName = data.UpdatedByName,
            ListGroup = listGroup.Select(x => new PromotionCustomerGroupDto()
            {
                Value = x.CustomerGroupId
            }).ToList(),
            Groups = String.Join(", ", groups.ToList().Select(x => x.Name).ToArray()),
            ListCustomer = listCustomer.Select(x => new PromotionCustomerDto()
            {
                CustomerId = x.CustomerId,
                CustomerName = x.Customer.Name
            }).ToList(),
            CustomerName = String.Join(", ", customers.ToList().Select(x => x.Name).ToArray()),
            Details = data?.PromotionByValue.Select(x => new PromotionByValueDto()
            {
                Id = x.Id,
                PromotionId = x.PromotionId,
                ProductId = x.ProductId,
                ProductCode = x.ProductCode,
                ProductName = x.ProductName,
                Type = x.Type,
                MinOrderPrice = x.MinOrderPrice,
                LimitTotalValue = x.LimitTotalValue,
                DiscountPercent = x.DiscountPercent,
                ReduceAmount = x.ReduceAmount,
                FixPrice = x.FixPrice,
                TypeBonus = x.TypeBonus,
                TypeBuy = x.TypeBuy,
                Quantity = x.Quantity,
                QuantityBuy = x.QuantityBuy,
                CreatedDate = x.CreatedDate,
                CreatedBy = x.CreatedBy,
                UpdatedDate = x.UpdatedDate,
                UpdatedBy = x.UpdatedBy,
                CreatedByName = x.CreatedByName,
                UpdatedByName = x.UpdatedByName,
                ProductBonus = x?.PromotionProduct.Select(y => new PromotionProductDto()
                {
                    Id = y.Id,
                    PromotionByValueId = y.PromotionByValueId,
                    ProductId = y.ProductId,
                    ProductCode = y.ProductCode,
                    ProductName = y.ProductName,
                    Quantity = y.Quantity,
                    CreatedDate = y.CreatedDate,
                    CreatedBy = y.CreatedBy,
                    UpdatedDate = y.UpdatedDate,
                    UpdatedBy = y.UpdatedBy,
                    CreatedByName = y.CreatedByName,
                    UpdatedByName = y.UpdatedByName,
                }).ToList(),
                ProductBuy = x?.PromotionProductBuy.Select(y => new PromotionProductBuyDto()
                {
                    Id = y.Id,
                    PromotionByValueId = y.PromotionByValueId,
                    ProductId = y.ProductId,
                    ProductCode = y.ProductCode,
                    ProductName = y.ProductName,
                    Quantity = y.Quantity,
                    CreatedDate = y.CreatedDate,
                    CreatedBy = y.CreatedBy,
                    UpdatedDate = y.UpdatedDate,
                    UpdatedBy = y.UpdatedBy,
                    CreatedByName = y.CreatedByName,
                    UpdatedByName = y.UpdatedByName,
                }).ToList()
            }).ToList()
        };
        return result;
    }

    public async Task<PagedResult<List<PromotionDto>>> Handle(PromotionPagingFilterQuery request, CancellationToken cancellationToken)
    {
        var response = new PagedResult<List<PromotionDto>>();
        var fopRequest = FopExpressionBuilder<Promotion>.Build(request.Filter, request.Order, request.PageNumber, request.PageSize);
        var filter = new Dictionary<string, object>();
        if (request.Status != null)
            filter.Add("status", request.Status.ToString());
        var (datas, count) = await _repository.Filter(request.Keyword, filter, fopRequest, request.PromotionGroupId);
        var result = datas.Select(item =>
        {
            return new PromotionDto()
            {
                Id = item.Id,
                Code = item.Code,
                Name = item.Name,
                StartDate = item.StartDate,
                EndDate = item.EndDate,
                Status = item.Status,
                Description = item.Description
            };
        }
        ).ToList();
        response.Items = result;
        response.TotalCount = count;
        response.PageNumber = request.PageNumber;
        response.PageSize = request.PageSize;
        return response;
    }
    public async Task<IEnumerable<PromotionDto>> Handle(PromotionQueryAll request, CancellationToken cancellationToken)
    {
        var data = await _repository.GetAll();
        var result = data.Select(x => new PromotionDto()
        {
            Id = x.Id,
            Code = x.Code,
            Name = x.Name,
            StartDate = x.StartDate,
            EndDate = x.EndDate,
            Status = x.Status
        });
        return result;
    }

    public async Task<IEnumerable<ComboBoxDto>> Handle(PromotionQueryComboBox request, CancellationToken cancellationToken)
    {
        var Promotion = await _repository.GetListCbx(request.Status);
        var result = Promotion.Select(x => new ComboBoxDto()
        {
            Key = x.Code,
            Value = x.Id,
            Label = x.Name
        });
        return result;
    }
}
