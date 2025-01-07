using VFi.Application.SO.DTOs;
using VFi.Domain.SO.Interfaces;
using VFi.NetDevPack.Queries;

namespace VFi.Application.SO.Queries;





public class ServiceAddCheckoutQueryListBox : IQuery<IEnumerable<ServiceAddCheckoutDto>>
{
    public ServiceAddCheckoutQueryListBox(string? keyword, int? status)
    {
        Keyword = keyword;
        Status = status;
    }
    public string? Keyword { get; set; }
    public int? Status { get; set; }
}



public class ServiceAddExQueryHandler :
                                         IQueryHandler<ServiceAddCheckoutQueryListBox, IEnumerable<ServiceAddCheckoutDto>>
{
    private readonly IServiceAddRepository _repository;
    public ServiceAddExQueryHandler(IServiceAddRepository serviceAddRespository)
    {
        _repository = serviceAddRespository;
    }
    public async Task<bool> Handle(ServiceAddQueryCheckExist request, CancellationToken cancellationToken)
    {
        return await _repository.CheckExistById(request.Id);
    }

    public async Task<IEnumerable<ServiceAddCheckoutDto>> Handle(ServiceAddCheckoutQueryListBox request, CancellationToken cancellationToken)
    {
        var filter = new Dictionary<string, object>();
        if (request.Status != null)
            filter.Add("status", request.Status.ToString());
        filter.Add("tags", "PAY-1");
        var serviceAdds = await _repository.Filter(request.Keyword, filter);
        var result = serviceAdds.Select(x => new ServiceAddCheckoutDto()
        {
            Code = x.Code,
            Id = x.Id,
            Name = x.Name,
            CalculationMethod = x.CalculationMethod,
            Currency = x.Currency,
            Description = x.Description,
            DisplayOrder = x.DisplayOrder,
            MaxPrice = x.MaxPrice,
            MinPrice = x.MinPrice,
            Price = x.Price,
            PriceSyntax = x.PriceSyntax
        });
        return result;
    }
}
