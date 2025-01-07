using System.Linq;
using System.Runtime;
using VFi.Application.SO.DTOs;
using VFi.Domain.SO.Interfaces;
using VFi.Domain.SO.Interfaces.Extented;
using VFi.Domain.SO.Models;
using VFi.Infra.SO.Repository;
using VFi.NetDevPack.FopExpression;
using VFi.NetDevPack.Queries;

namespace VFi.Application.SO.Queries;

public class CustomerExPagingQuery : FopQuery, IQuery<PagedResult<List<CustomerExDto>>>
{
    public CustomerExPagingQuery(string? keyword, int? type, int? status, string filter, string order, int pageNumber, int pageSize)
    {
        Keyword = keyword;
        Filter = filter;
        Order = order;
        PageNumber = pageNumber;
        PageSize = pageSize;
        Type = type;
        Status = status;
    }
    public string? Keyword { get; set; }
    public int? Type { get; set; }
    public int? Status { get; set; }
}

public class GetMyInfo : IQuery<MyInfoDto>
{
    public GetMyInfo()
    {
    }

    public GetMyInfo(Guid accountId)
    {
        AccountId = accountId;
    }

    public Guid AccountId { get; set; }
}
public class GetBidCreditSetup : IQuery<BidCreditSetupDto>
{
    public GetBidCreditSetup()
    {
    }

    public GetBidCreditSetup(Guid accountId)
    {
        AccountId = accountId;
    }

    public Guid AccountId { get; set; }
}
public class CustomerPricePuchaseByAccountIdQuery : IQuery<IEnumerable<CustomerPricePuchaseDto>>
{
    public CustomerPricePuchaseByAccountIdQuery()
    {
    }

    public CustomerPricePuchaseByAccountIdQuery(
        Guid accountId,
        string? purchaseGroupCode,
        decimal? price)
    {
        AccountId = accountId;
        PurchaseGroupCode = purchaseGroupCode;
        Price = price;
    }

    public Guid AccountId { get; set; }
    public string? PurchaseGroupCode { get; set; }
    public decimal? Price { get; set; }
}
public class CustomerFinaceQuery : IQuery<CustomerFinaceDto>
{
    public CustomerFinaceQuery()
    {
    }
    public Guid Id { get; set; }
}
public class CustomerAuctionQuery : IQuery<CustomerAuctionDto>
{
    public CustomerAuctionQuery()
    {
    }
    public Guid Id { get; set; }
}
public class CustomerExQueryHandler :
                                         IQueryHandler<CustomerPricePuchaseByAccountIdQuery, IEnumerable<CustomerPricePuchaseDto>>,
                                         IQueryHandler<GetMyInfo, MyInfoDto>, IQueryHandler<CustomerFinaceQuery, CustomerFinaceDto>,
                                         IQueryHandler<CustomerExPagingQuery, PagedResult<List<CustomerExDto>>>,
                                         IQueryHandler<GetBidCreditSetup, BidCreditSetupDto>
{
    private readonly IBidRepository _bidRepository;
    private readonly ISOExtProcedures _procedureRepository;
    private readonly ICustomerRepository _customerRepository;
    private readonly IEmployeeRepository _employeeRepository;
    private readonly ICustomerGroupRepository _customerGroupRepository;
    private readonly ICustomerSourceRepository _customerSourceRepository;
    private readonly IBusinessRepository _businessRepository;
    private readonly IGroupEmployeeRepository _groupEmployeeRepository;
    private readonly ICustomerGroupMappingRepository _groupMapping;
    private readonly ICustomerBusinessMappingRepository _businessMapping;
    private readonly IPriceListRepository _priceListRepository;
    private readonly IPriceListCrossRepository _priceListCrossRepository;
    private readonly IPriceListPurchaseRepository _priceListPurchaseRepository;
    public CustomerExQueryHandler(
                                ICustomerRepository CustomerRespository,
                                IEmployeeRepository EmployeeRepository,
                                ICustomerGroupRepository CustomerGroupRepository,
                                ICustomerSourceRepository CustomerSourceRepository,
                                IBusinessRepository BusinessRepository,
                                IGroupEmployeeRepository GroupEmployeeRepository,
                                ICustomerGroupMappingRepository GroupMapping,
                                ICustomerBusinessMappingRepository BusinessMapping,
                                IPriceListRepository priceListRepository,
                                IPriceListCrossRepository priceListCrossRepository,
                                IPriceListPurchaseRepository priceListPurchaseRepository,
                                ISOExtProcedures procedure,
                                IBidRepository bidRepository)
    {
        _customerRepository = CustomerRespository;
        _employeeRepository = EmployeeRepository;
        _customerGroupRepository = CustomerGroupRepository;
        _customerSourceRepository = CustomerSourceRepository;
        _businessRepository = BusinessRepository;
        _groupEmployeeRepository = GroupEmployeeRepository;
        _groupMapping = GroupMapping;
        _businessMapping = BusinessMapping;
        _priceListRepository = priceListRepository;
        _procedureRepository = procedure;
        _priceListCrossRepository = priceListCrossRepository;
        _priceListPurchaseRepository = priceListPurchaseRepository;
        _bidRepository = bidRepository;
    }

    public async Task<PagedResult<List<CustomerExDto>>> Handle(CustomerExPagingQuery request, CancellationToken cancellationToken)
    {
        var response = new PagedResult<List<CustomerExDto>>();

        var fopRequest = FopExpressionBuilder<Customer>.Build(request.Filter, request.Order, request.PageNumber, request.PageSize);
        var filter = new Dictionary<string, object>();
        var filterListBox = new Dictionary<string, object>();
        if (request.Status != null)
            filterListBox.Add("status", request.Status.ToString());
        if (request.Type != null)
            filterListBox.Add("type", request.Type.ToString());
        var (datas, count) = await _customerRepository.Filter(request.Keyword, filterListBox, fopRequest);
        var employee = await _employeeRepository.GetAll();
        var customerGroup = await _customerGroupRepository.GetAll();
        var customerSource = await _customerSourceRepository.GetAll();
        var business = await _businessRepository.GetAll();
        var GroupEmployee = await _groupEmployeeRepository.GetAll();
        var priceListCrossAll = await _priceListCrossRepository.GetAll();
        var priceListPurchaseAll = await _priceListPurchaseRepository.GetAll();
        string priceListPurchaseDefault = "";

        priceListPurchaseDefault = priceListPurchaseAll.FirstOrDefault(x => x.Default.HasValue && x.Default.Value)?.Name;
        //var priceListPurchase = await 
        // var count = await _customerRepository.FilterCount(request.Keyword, request.Filter);
        //var Customers = await _customerRepository.Filter(request.Keyword, request.Filter, request.PageSize, request.PageIndex);
        var data = datas.Select(obj => new CustomerExDto()
        {
            Id = obj.Id,
            Code = obj.Code,
            Name = obj.Name,
            CustomerGroup = obj.CustomerGroup,
            Groups = String.Join(", ", obj.CustomerGroupMapping.Select(x => x.CustomerGroup.Name).ToList()),
            BidActive = obj.BidActive,
            BidQuantity = obj.BidQuantity,
            TranActive = obj.TranActive,
            PriceListPurchaseName = obj.PriceListPurchaseName, //obj.PriceListPurchaseId.HasValue? priceListPurchaseAll.FirstOrDefault(x=>x.Id== obj.PriceListPurchaseId.Value)?.Name : priceListPurchaseDefault,
            //Groups = obj.CustomerGroupMapping.Select(x => x.CustomerGroup.Name).,
            CustomerSourceName = obj.CustomerSourceId != null ? customerSource.FirstOrDefault(x => x.Id == obj.CustomerSourceId)?.Name : null,
            Phone = obj.Phone,
            Email = obj.Email,
            Website = obj.Website,
            TaxCode = obj.TaxCode,
            BusinessSector = obj.BusinessSector,
            CompanySize = obj.CompanySize,
            Capital = obj.Capital,
            EstablishedDate = obj.EstablishedDate,
            EmployeeId = obj.EmployeeId,
            EmployeeName = (obj.EmployeeId != null) ? employee.FirstOrDefault(x => x.AccountId == obj.EmployeeId)?.Name : null,
            GroupEmployeeId = obj.GroupEmployeeId,
            GroupEmployeeName = obj.GroupEmployeeId != null ? GroupEmployee.FirstOrDefault(x => x.Id == obj.GroupEmployeeId)?.Name : null,
            Address = obj.Address,
            Status = obj.Status,
            CreatedDate = obj.CreatedDate,
            Type = obj.Type,
            Year = obj.Year,
            Gender = obj.Gender,
            Country = obj.Country,
            Province = obj.Province,
            District = obj.District,
            Ward = obj.Ward,
            Tags = obj.Tags,
            Note = obj.Note,
            PriceListId = obj.PriceListId,
            PriceListName = obj.PriceListName,
            Currency = obj.Currency,
            CurrencyName = obj.CurrencyName,
            AccountId = obj.AccountId,
            AccountEmail = obj.AccountEmail,
            AccountPhone = obj.AccountPhone,
            AccountEmailVerified = obj.AccountEmailVerified,
            AccountPhoneVerified = obj.AccountPhoneVerified,
            AccountUsername = obj.AccountUsername,
            AccountCreatedDate = obj.AccountCreatedDate,
            Revenue = obj.Revenue,
            RevenueMonth = obj.RevenueMonth

        }).ToList();
        response.Items = data;
        response.TotalCount = count;
        response.PageNumber = request.PageNumber;
        response.PageSize = request.PageSize;
        return response;
    }

    //public async Task<IEnumerable<CustomerDto>> Handle(CustomerQueryAll request, CancellationToken cancellationToken)
    //{
    //    var Customers = await _customerRepository.GetAll();
    //    var employee = await _employeeRepository.GetAll();
    //    var result = Customers.Select(obj => new CustomerDto()
    //    {
    //        Id = obj.Id,
    //        Code = obj.Code,
    //        Name = obj.Name,
    //        CustomerGroup = obj.CustomerGroup,
    //        Phone = obj.Phone,
    //        Email = obj.Email,
    //        EmployeeId = obj.EmployeeId,
    //        EmployeeName = obj.EmployeeId != null ? employee.FirstOrDefault(x => x.Id == obj.EmployeeId)?.Name : null,
    //        GroupEmployeeId = obj.GroupEmployeeId,
    //        Address = obj.Address,
    //        Status = obj.Status,
    //        CreatedDate = obj.CreatedDate,
    //    });
    //    return result;
    //}

    public async Task<IEnumerable<CustomerPricePuchaseDto>> Handle(CustomerPricePuchaseByAccountIdQuery request, CancellationToken cancellationToken)
    {
        var obj = await _procedureRepository.SP_GET_PRICE_PUCHASE_BY_ACCOUNT_IDAsync(request.AccountId, request.PurchaseGroupCode, request.Price);

        if (obj == null)
        {
            return new List<CustomerPricePuchaseDto>();
        }

        var result = obj.Select(x => new CustomerPricePuchaseDto
        {
            Id = x.Id,
            PriceListPurchaseId = x.PriceListPurchaseId,
            PriceListPurchase = x.PriceListPurchase,
            PurchaseGroupCode = x.PurchaseGroupCode,
            BuyFee = x.BuyFee,
            BuyFeeMin = x.BuyFeeMin,
            BuyFeeFix = x.BuyFeeFix,
            Currency = x.Currency,
            CalculatePrice = x.CalculatePrice
        });
        return result;
    }

    public async Task<MyInfoDto> Handle(GetMyInfo request, CancellationToken cancellationToken)
    {
        var obj = await _procedureRepository.SP_GET_MY_INFOAsync(request.AccountId);

        var result = obj.Select(x => new MyInfoDto()
        {
            Id = x.Id,
            AccountId = x.AccountId,
            Code = x.Code,
            Name = x.Name,
            Image = x.Image,
            Level = x.Level,
            BidActive = x.BidActive,
            BidQuantity = x.BidQuantity,
            IdStatus = x.IdStatus,
            IdName = x.IdName,
            IdNumber = x.IdNumber,
            IdImage1 = x.IdImage1,
            IdImage2 = x.IdImage2,
            TranActive = x.TranActive,
            Cash = x.Cash,
            CashHold = x.CashHold,
            CashHoldBid = x.CashHoldBid
        }).FirstOrDefault(new MyInfoDto());
        if (result.BidActive.HasValue && result.BidActive.Value)
        {
            var bidinfo = await _bidRepository.CreditInfo(request.AccountId.ToString());
            if (bidinfo.Status)
            {
                var bidcredit = bidinfo.Data;
                result.TotalBidCredit = bidcredit.TotalBidCredit;
                result.TotalBidLastTimeCredit = bidcredit.TotalBidLastTimeCredit;
                result.TotalOrderPending = bidcredit.TotalOrderPending;
                result.MaxBid = bidcredit.MaxBid;
                result.TotalCredit = bidcredit.TotalCredit;
                result.CreditAvailable = bidcredit.CreditAvailable;
                result.IsMaxBid = bidcredit.IsMaxBid;
            }
        }
        return result;
    }

    public async Task<CustomerFinaceDto> Handle(CustomerFinaceQuery request, CancellationToken cancellationToken)
    {
        var result = new CustomerFinaceDto();
        var customer = await _customerRepository.GetFullById(request.Id);
        result.Id = customer.Id;
        result.CustomerName = customer.Name;
        result.CustomerId = customer.Id;
        result.PriceListId = customer.PriceListId;
        result.PriceListName = customer.PriceListName;
        result.PriceListPurchaseName = customer.PriceListPurchaseName;
        result.PriceListPurchaseId = customer.PriceListPurchaseId;
        result.Currency = customer.Currency;
        result.CurrencyId = customer.CurrencyId;
        result.CurrencyName = customer.CurrencyName;
        result.PriceListCross = customer.CustomerPriceListCross.Select(x => x.PriceListCrossId).ToList();
        result.CustomerPriceListCross = customer.CustomerPriceListCross.Select(x => new CustomerPriceListCrossDto()
        {
            Id = x.Id,
            CustomerId = x.CustomerId,
            PriceListCrossId = x.PriceListCrossId,
            PriceListCrossName = x.PriceListCrossName,
            RouterShipping = x.RouterShipping,
            RouterShippingId = x.RouterShippingId
        }).ToList();
        result.DebtLimit = customer.DebtLimit;
        return result;
    }

    public async Task<BidCreditSetupDto> Handle(GetBidCreditSetup request, CancellationToken cancellationToken)
    {
        var obj = await _procedureRepository.SP_GET_BID_CREDIT_SETUPAsync(request.AccountId);

        var result = obj.Select(x => new BidCreditSetupDto()
        {
            Id = x.Id,
            AccountId = x.AccountId,
            BidActive = x.BidActive,
            BidQuantity = x.BidQuantity,
            OrderPending = x.OrderPending
        }).FirstOrDefault(new BidCreditSetupDto());
        return result;
    }
}
