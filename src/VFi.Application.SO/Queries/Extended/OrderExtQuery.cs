using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;
using AutoMapper.Execution;
using Consul;
using MassTransit.Transports;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Microsoft.IdentityModel.Tokens;
using VFi.Application.SO.DTOs;
using VFi.Domain.SO.Interfaces;
using VFi.Domain.SO.Models;
using VFi.NetDevPack.Data;
using VFi.NetDevPack.FopExpression;
using VFi.NetDevPack.Queries;
using VFi.NetDevPack.Utilities;

namespace VFi.Application.SO.Queries;



public class OrderCountByCustomerId : IQuery<Dictionary<int, int>>
{
    public OrderCountByCustomerId(Guid customerId)
    {
        CustomerId = customerId;
    }
    public Guid CustomerId { get; set; }
}
public class TopOrderByCustomerIdQuery : IQuery<IEnumerable<TopOrderDto>>
{
    public TopOrderByCustomerIdQuery(Guid customerId)
    {
        CustomerId = customerId;
    }
    public Guid CustomerId { get; set; }
}


public class OrderExtQueryHandler : IQueryHandler<TopOrderByCustomerIdQuery, IEnumerable<TopOrderDto>>,
                                         IQueryHandler<OrderCountByCustomerId, Dictionary<int, int>>
{
    private readonly IOrderRepository _OrderRepository;
    private readonly IPIMRepository _PIMRepository;
    private readonly ISOExtProcedures _repository;
    public OrderExtQueryHandler(IOrderRepository OrderRespository, IPIMRepository PIMRepository,
        ISOExtProcedures sOContextProcedures)
    {
        _OrderRepository = OrderRespository;
        _PIMRepository = PIMRepository;
        _repository = sOContextProcedures;
    }


    public async Task<Dictionary<int, int>> Handle(OrderCountByCustomerId request, CancellationToken cancellationToken)
    {
        var dbresult = await _repository.SP_GET_ORDER_COUNTERAsync(request.CustomerId);
        var result = new Dictionary<int, int>();
        foreach (var item in dbresult)
        {
            result.Add(item.Status, item.Count);
        }
        return result;
    }

    public async Task<IEnumerable<TopOrderDto>> Handle(TopOrderByCustomerIdQuery request, CancellationToken cancellationToken)
    {
        var dbresult = await _repository.SP_GET_MY_TOP_ORDERAsync(request.CustomerId);

        return dbresult.Select(x => new TopOrderDto()
        {
            Id = x.Id,
            Code = x.Code,
            OrderDate = x.OrderDate,
            Description = x.Description,
            Image = x.Image,
            Total = x.Total,
            Paid = x.Paid,
            Status = x.Status,
            PaymentStatus = x.PaymentStatus,
            UpdatedDate = x.UpdatedDate
        });
    }
}
