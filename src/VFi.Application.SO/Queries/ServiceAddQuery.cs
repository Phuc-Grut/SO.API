using Consul;
using MassTransit.Components;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using VFi.Application.SO.DTOs;
using VFi.Domain.SO.Interfaces;
using VFi.Domain.SO.Models;
using VFi.NetDevPack.Queries;

namespace VFi.Application.SO.Queries;


public class ServiceAddQueryListBox : IQuery<IEnumerable<ServiceAddListBoxDto>>
{
    public ServiceAddQueryListBox(string? keyword, int? status)
    {
        Keyword = keyword;
        Status = status;
    }
    public string? Keyword { get; set; }
    public int? Status { get; set; }
}
public class ServiceAddQueryCheckExist : IQuery<bool>
{

    public ServiceAddQueryCheckExist(Guid id)
    {
        Id = id;
    }
    public Guid Id { get; set; }
}
public class ServiceAddQueryById : IQuery<ServiceAddDto>
{
    public ServiceAddQueryById()
    {
    }

    public ServiceAddQueryById(Guid serviceAddId)
    {
        ServiceAddId = serviceAddId;
    }

    public Guid ServiceAddId { get; set; }
}
public class ServiceAddPagingQuery : ListQuery, IQuery<PagingResponse<ServiceAddDto>>
{
    public ServiceAddPagingQuery(string? keyword, int? status, int pageSize, int pageIndex) : base(pageSize, pageIndex)
    {
        Keyword = keyword;
        Status = status;
    }

    public ServiceAddPagingQuery(string? keyword, int? status, int pageSize, int pageIndex, SortingInfo[] sortingInfos) : base(pageSize, pageIndex, sortingInfos)
    {
        Keyword = keyword;
        Status = status;
    }

    public string? Keyword { get; set; }
    public int? Status { get; set; }
}

public class ServiceAddQueryHandler : IQueryHandler<ServiceAddQueryListBox, IEnumerable<ServiceAddListBoxDto>>,
                                         IQueryHandler<ServiceAddQueryCheckExist, bool>,
                                         IQueryHandler<ServiceAddQueryById, ServiceAddDto>,
                                         IQueryHandler<ServiceAddPagingQuery, PagingResponse<ServiceAddDto>>
{
    private readonly IServiceAddRepository _serviceAddRepository;
    public ServiceAddQueryHandler(IServiceAddRepository serviceAddRespository)
    {
        _serviceAddRepository = serviceAddRespository;
    }
    public async Task<bool> Handle(ServiceAddQueryCheckExist request, CancellationToken cancellationToken)
    {
        return await _serviceAddRepository.CheckExistById(request.Id);
    }

    public async Task<ServiceAddDto> Handle(ServiceAddQueryById request, CancellationToken cancellationToken)
    {
        var serviceAdd = await _serviceAddRepository.GetById(request.ServiceAddId);
        var result = new ServiceAddDto()
        {
            Id = serviceAdd.Id,
            Code = serviceAdd.Code,
            Name = serviceAdd.Name,
            Description = serviceAdd.Description,
            CalculationMethod = serviceAdd.CalculationMethod,
            Price = serviceAdd.Price,
            PriceSyntax = serviceAdd.PriceSyntax,
            MinPrice = serviceAdd.MinPrice,
            MaxPrice = serviceAdd.MaxPrice,
            PayLater = serviceAdd.PayLater,
            Status = serviceAdd.Status,
            Tags = serviceAdd.Tags,
            Currency = serviceAdd.Currency,
            CurrencyName = serviceAdd.CurrencyName,
            DisplayOrder = serviceAdd.DisplayOrder,
            CreatedBy = serviceAdd.CreatedBy,
            CreatedDate = serviceAdd.CreatedDate,
            UpdatedBy = serviceAdd.UpdatedBy,
            UpdatedDate = serviceAdd.UpdatedDate,
            CreatedByName = serviceAdd.CreatedByName,
            UpdatedByName = serviceAdd.UpdatedByName
        };
        return result;
    }

    public async Task<PagingResponse<ServiceAddDto>> Handle(ServiceAddPagingQuery request, CancellationToken cancellationToken)
    {
        var response = new PagingResponse<ServiceAddDto>();
        var count = await _serviceAddRepository.FilterCount(request.Keyword, request.Status);
        var serviceAdds = await _serviceAddRepository.Filter(request.Keyword, request.Status, request.PageSize, request.PageIndex);
        var data = serviceAdds.Select(serviceAdd => new ServiceAddDto()
        {
            Id = serviceAdd.Id,
            Code = serviceAdd.Code,
            Name = serviceAdd.Name,
            Description = serviceAdd.Description,
            CalculationMethod = serviceAdd.CalculationMethod,
            Price = serviceAdd.Price,
            PriceSyntax = serviceAdd.PriceSyntax,
            MinPrice = serviceAdd.MinPrice,
            MaxPrice = serviceAdd.MaxPrice,
            PayLater = serviceAdd.PayLater,
            Status = serviceAdd.Status,
            Tags = serviceAdd.Tags,
            Currency = serviceAdd.Currency,
            CurrencyName = serviceAdd.CurrencyName,
            DisplayOrder = serviceAdd.DisplayOrder,
            CreatedBy = serviceAdd.CreatedBy,
            CreatedDate = serviceAdd.CreatedDate,
            UpdatedBy = serviceAdd.UpdatedBy,
            UpdatedDate = serviceAdd.UpdatedDate,
            CreatedByName = serviceAdd.CreatedByName,
            UpdatedByName = serviceAdd.UpdatedByName
        });
        response.Items = data;
        response.Total = count;
        response.Count = count;
        response.PageIndex = request.PageIndex;
        response.PageSize = request.PageSize;
        response.Successful();
        return response;
    }

    public async Task<IEnumerable<ServiceAddListBoxDto>> Handle(ServiceAddQueryListBox request, CancellationToken cancellationToken)
    {

        var serviceAdds = await _serviceAddRepository.GetListListBox(request.Keyword, request.Status);
        var result = serviceAdds.Select(x => new ServiceAddListBoxDto()
        {
            Key = x.Code,
            Value = x.Id,
            Label = x.Name,
            Currency = x.Currency,
            CurrencyName = x.CurrencyName,
            Price = x.Price
        });
        return result;
    }
}
