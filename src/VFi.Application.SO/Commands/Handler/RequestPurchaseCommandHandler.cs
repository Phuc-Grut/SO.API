using FluentValidation.Results;
using MediatR;
using Microsoft.CodeAnalysis;
using VFi.Domain.SO.Enums;
using VFi.Domain.SO.Interfaces;
using VFi.Domain.SO.Models;
using VFi.NetDevPack.Context;
using VFi.NetDevPack.Messaging;

namespace VFi.Application.SO.Commands.Handler;

internal class RequestPurchaseCommandHandler : CommandHandler,
    IRequestHandler<AddRequestPurchaseCommand, ValidationResult>,
    IRequestHandler<DeleteRequestPurchaseCommand, ValidationResult>,
    IRequestHandler<RequestPurchaseProcessCommand, ValidationResult>,
    IRequestHandler<RequestPurchaseDuplicateCommand, ValidationResult>,
    IRequestHandler<EditRequestPurchaseCommand, ValidationResult>,
    IRequestHandler<RequestPurchasePurchaseCommand, ValidationResult>,
    IRequestHandler<POPurchaseProductCommand, ValidationResult>,
    IRequestHandler<DeleteOrderRequestPurchaseCommand, ValidationResult>,
    IRequestHandler<UpdatePurchaseQtyCommand, ValidationResult>
{
    private readonly IRequestPurchaseRepository _repository;
    private readonly IRequestPurchaseProductRepository _requestPurchaseProductRepository;
    private readonly IContextUser _context;
    private readonly IPORepository _PORepository;
    private readonly IEventRepository _eventRepository;
    private readonly IOrderRepository _orderRepository;
    private readonly IOrderProductRepository _orderProductRepository;

    public RequestPurchaseCommandHandler(
        IContextUser contextUser,
        IRequestPurchaseRepository RequestPurchaseRepository,
        IRequestPurchaseProductRepository RequestPurchaseProductRepository,
        IPORepository PORepository,
        IOrderRepository orderRepository,
        IOrderProductRepository orderProductRepository,
        IEventRepository eventRepository
        )
    {
        _repository = RequestPurchaseRepository;
        _requestPurchaseProductRepository = RequestPurchaseProductRepository;
        _context = contextUser;
        _PORepository = PORepository;
        _eventRepository = eventRepository;
        _orderRepository = orderRepository;
        _orderProductRepository = orderProductRepository;
    }
    public void Dispose()
    {
        _repository.Dispose();
    }

    public async Task<ValidationResult> Handle(AddRequestPurchaseCommand request, CancellationToken cancellationToken)
    {
        if (!request.IsValid(_repository))
            return request.ValidationResult;
        var createName = _context.UserClaims.FullName;
        var createdBy = _context.GetUserId();
        var createdDate = DateTime.Now;
        var requestPurchase = new RequestPurchase
        {
            Id = request.Id,
            Code = request.Code,
            RequestBy = request.RequestBy,
            RequestByName = request.RequestByName,
            RequestByEmail = request.RequestByEmail,
            RequestDate = request.RequestDate,
            CurrencyCode = request.CurrencyCode,
            CurrencyName = request.CurrencyName,
            Calculation = request.Calculation,
            ExchangeRate = request.ExchangeRate,
            Proposal = request.Proposal,
            Note = request.Note,
            ApproveDate = request.ApproveDate,
            ApproveBy = request.ApproveBy,
            ApproveByName = request.ApproveByName,
            ApproveComment = request.ApproveComment,
            Status = request.Status,
            QuantityApproved = request.QuantityApproved,
            QuantityRequest = request.QuantityRequest,
            OrderId = request.OrderId,
            OrderCode = request.OrderCode,
            CreatedDate = createdDate,
            CreatedBy = createdBy,
            CreatedByName = createName,
            File = request.File
        };
        _repository.Add(requestPurchase);
        var result = await Commit(_repository.UnitOfWork);
        if (!result.IsValid)
            return result;

        var list = new List<RequestPurchaseProduct>();
        if (request.Detail?.Count > 0)
        {
            var i = 1;
            foreach (var x in request.Detail)
            {
                list.Add(new RequestPurchaseProduct()
                {
                    Id = Guid.NewGuid(),
                    RequestPurchaseId = x.RequestPurchaseId,
                    OrderId = x.OrderId,
                    OrderCode = x.OrderCode,
                    OrderProductId = x.OrderProductId,
                    ProductId = x.ProductId,
                    ProductCode = x.ProductCode,
                    ProductName = x.ProductName,
                    ProductImage = x.ProductImage,
                    SourceLink = x.SourceLink,
                    ShippingFee = x.ShippingFee,
                    Origin = x.Origin,
                    UnitCode = x.UnitCode,
                    UnitName = x.UnitName,
                    UnitType = x.UnitType,
                    QuantityRequest = x.QuantityRequest,
                    QuantityApproved = x.QuantityApproved,
                    UnitPrice = x.UnitPrice,
                    Currency = x.Currency,
                    DeliveryDate = x.DeliveryDate,
                    PriorityLevel = x.PriorityLevel,
                    Status = x.Status,
                    StatusPurchase = (int)RequestPurchaseStatus.Pending,
                    Note = x.Note,
                    BidUsername = x.BidUsername,
                    VendorCode = x.VendorCode,
                    VendorName = x.VendorName,
                    DisplayOrder = i
                });
                i++;
            }
            _requestPurchaseProductRepository.Add(list);
            _ = await CommitNoCheck(_requestPurchaseProductRepository.UnitOfWork);
        }
        return result;
    }

    public async Task<ValidationResult> Handle(DeleteRequestPurchaseCommand request, CancellationToken cancellationToken)
    {
        if (!request.IsValid())
            return request.ValidationResult;
        var RequestPurchase = new RequestPurchase
        {
            Id = request.Id
        };

        _repository.Remove(RequestPurchase);
        return await Commit(_repository.UnitOfWork);
    }
    public async Task<ValidationResult> Handle(DeleteOrderRequestPurchaseCommand request, CancellationToken cancellationToken)
    {
        if (!await request.IsValidAsync(_repository))
            return request.ValidationResult;
		
        var requestPurchase = await _repository.GetRemoveOrderId(request.Id, request.OrderId);
        var orderProductsToRemove = requestPurchase.RequestPurchaseProduct
            .Where(x => x.OrderId == request.OrderId)
            .ToList();

        if (!requestPurchase.RequestPurchaseProduct.Any())
        {
            _repository.Remove(requestPurchase);
        }
        else
        {
            var updateName = _context.UserClaims.FullName;
            var updatedBy = _context.GetUserId();
            requestPurchase.UpdatedBy = updatedBy;
            requestPurchase.UpdatedByName = updateName;
            requestPurchase.UpdatedDate = DateTime.UtcNow;
            _requestPurchaseProductRepository.Remove(orderProductsToRemove);
            var removeCommitResult = await Commit(_requestPurchaseProductRepository.UnitOfWork);
            _repository.Update(requestPurchase);
        }

        return await Commit(_repository.UnitOfWork);
    }

    public async Task<ValidationResult> Handle(RequestPurchaseProcessCommand request, CancellationToken cancellationToken)
    {
        var item = await _repository.GetById(request.Id);
        if (item is null)
        {
            return new ValidationResult() { Errors = new List<ValidationFailure>() { new ValidationFailure("id", "RequestPurchase is not exist") } };
        }
        item.Status = request.Status;
        item.ApproveBy = _context.GetUserId();
        item.ApproveByName = _context.UserClaims.FullName;
        item.ApproveComment = request.ApproveComment;
        item.ApproveDate = DateTime.Now;
        if (request.Status == 1)
        {
            var obj = new PORequestPurchase
            {
                Code = item.Code,
                PurchaseTypeCode = "TM",
                RequestBy = (Guid)item.RequestBy,
                RequestByName = item.RequestByName,
                CurrencyCode = item.CurrencyCode,
                CurrencyName = item.CurrencyName,
                ExchangeRate = item.ExchangeRate,
                Proposal = item.Proposal,
                Note = item.Note,
                Status = request.POStatus,
                RequestByEmail = item.RequestByEmail,
                RequestDate = item.RequestDate,
                Calculation = item.Calculation,
                ApproveDate = item.ApproveDate,
                ApproveBy = item.ApproveBy,
                ApproveByName = item.ApproveByName,
                ApproveComment = item.ApproveComment,
                CreatedBy = item.CreatedBy,
                CreatedDate = item.CreatedDate,
                UpdatedBy = item.UpdatedBy,
                UpdatedDate = item.UpdatedDate,
                CreatedByName = item.CreatedByName,
                UpdatedByName = item.UpdatedByName,
                StatusPurchase = item.StatusPurchase,
                File = item.File,
                ListDetail = item.RequestPurchaseProduct.Select(x => new PORequestPurchaseProduct()
                {
                    ProductId = x.ProductId.Value,
                    ProductCode = x.ProductCode,
                    ProductName = x.ProductName,
                    ProductImage = x.ProductImage,
                    Origin = x.Origin,
                    UnitType = x.UnitType,
                    UnitCode = x.UnitCode,
                    UnitName = x.UnitName,
                    UnitPrice = x.UnitPrice,
                    Currency = x.Currency,
                    DeliveryDate = x.DeliveryDate,
                    PriorityLevel = x.PriorityLevel,
                    VendorCode = x.VendorCode,
                    VendorName = x.VendorName,
                    Status = x.Status,
                    QuantityRequest = x.QuantityRequest,
                    Note = x.Note,
                    QuantityApproved = x.QuantityApproved,
                    PurchaseRequestId = request.Id,
                    PurchaseRequestDetailId = request.Id, // không có trong db
                    SourceLink = x.SourceLink,
                    ShippingFee = x.ShippingFee,
                    DisplayOrder = x.DisplayOrder,
                    CreatedBy = item.CreatedBy,
                    CreatedDate = item.CreatedDate,
                    CreatedByName = item.CreatedByName,
                    StatusPurchase = x.StatusPurchase,
                    QuantityPurchased = (decimal?)x.QuantityPurchased,
                }).ToList()
            };
            var result = await _PORepository.AddExt(obj);
            if (result.IsValid)
            {
                item.PurchaseRequestCode = result.RuleSetsExecuted[0];
            }
            var list = new List<RequestPurchaseProduct>();
            var details = item.RequestPurchaseProduct;
            if (details != null)
            {
                var sumQtyRq = details.Sum(x => x.QuantityRequest);
                var sumQtyAp = details.Select(x => new { QuantityApproved = x.QuantityApproved == 0 ? x.QuantityRequest : x.QuantityApproved }).Sum(x => x.QuantityApproved);
                item.QuantityRequest = sumQtyRq;
                item.QuantityApproved = sumQtyAp;
                foreach (var detail in details)
                {
                    if (detail.QuantityApproved == 0 || detail.QuantityApproved == null)
                    {
                        detail.QuantityApproved = detail.QuantityRequest;
                        list.Add(detail);
                    }
                }
            }
            item.RequestPurchaseProduct = list;
        }
        _repository.Update(item);
        var rs = await Commit(_repository.UnitOfWork);
        return rs;
    }

    public async Task<ValidationResult> Handle(EditRequestPurchaseCommand request, CancellationToken cancellationToken)
    {
        if (!request.IsValid())
            return request.ValidationResult;
        var updateName = _context.UserClaims.FullName;
        var updatedBy = _context.GetUserId();
        var updatedDate = DateTime.Now;
        var pr = await _repository.GetById(request.Id);
        pr.RequestBy = request.RequestBy;
        pr.RequestByName = request.RequestByName;
        pr.RequestByEmail = request.RequestByEmail;
        pr.RequestDate = request.RequestDate;
        pr.CurrencyCode = request.CurrencyCode;
        pr.Calculation = request.Calculation;
        pr.ExchangeRate = request.ExchangeRate;
        pr.Proposal = request.Proposal;
        pr.Note = request.Note;
        pr.ApproveDate = request.ApproveDate;
        pr.ApproveBy = request.ApproveBy;
        pr.ApproveByName = request.ApproveByName;
        pr.ApproveComment = request.ApproveComment;
        pr.Status = request.Status;
        pr.QuantityRequest = request.QuantityRequest;
        pr.QuantityApproved = request.QuantityApproved;
        pr.OrderId = request.OrderId;
        pr.OrderCode = request.OrderCode;
        pr.UpdatedDate = updatedDate;
        pr.UpdatedBy = updatedBy;
        pr.UpdatedByName = updateName;
        pr.File = request.File;

        if (request.Delete?.Count > 0)
        {
            var list = new List<RequestPurchaseProduct>();
            foreach (var d in request.Delete)
            {
                var item = await _requestPurchaseProductRepository.GetById(d.Id);
                if (item is not null)
                {
                    list.Add(item);
                }
            }
            _requestPurchaseProductRepository.Remove(list);
            _ = await CommitNoCheck(_requestPurchaseProductRepository.UnitOfWork);
        }

        var listAdd = new List<RequestPurchaseProduct>();
        var listUpdate = new List<RequestPurchaseProduct>();
        if (request.Detail?.Count > 0)
        {
            var i = 1;
            foreach (var u in request.Detail)
            {
                var item = pr.RequestPurchaseProduct.Where(x => x.Id == u.Id).FirstOrDefault();
                if (item is not null)
                {
                    item.OrderId = u.OrderId;
                    item.OrderCode = u.OrderCode;
                    item.OrderProductId = u.OrderProductId;
                    item.ProductId = u.ProductId;
                    item.ProductCode = u.ProductCode;
                    item.ProductName = u.ProductName;
                    item.ProductImage = u.ProductImage;
                    item.Origin = u.Origin;
                    item.UnitCode = u.UnitCode;
                    item.UnitName = u.UnitName;
                    item.UnitType = u.UnitType;
                    item.QuantityRequest = u.QuantityRequest;
                    item.QuantityApproved = u.QuantityApproved;
                    item.QuantityPurchased = u.QuantityPurchased;
                    item.StatusPurchase = u.StatusPurchase;
                    item.UnitPrice = u.UnitPrice;
                    item.Currency = u.Currency;
                    item.DeliveryDate = u.DeliveryDate;
                    item.PriorityLevel = u.PriorityLevel;
                    item.Status = u.Status;
                    item.Note = u.Note;
                    item.VendorCode = u.VendorCode;
                    item.VendorName = u.VendorName;
                    item.DisplayOrder = i;
                    item.OrderProduct = null;

                    listUpdate.Add(item);
                }
                else
                {
                    listAdd.Add(new RequestPurchaseProduct()
                    {
                        Id = Guid.NewGuid(),
                        RequestPurchaseId = request.Id,
                        OrderId = u.OrderId,
                        OrderCode = u.OrderCode,
                        OrderProductId = u.OrderProductId,
                        ProductId = u.ProductId,
                        ProductCode = u.ProductCode,
                        ProductName = u.ProductName,
                        ProductImage = u.ProductImage,
                        Origin = u.Origin,
                        UnitCode = u.UnitCode,
                        UnitName = u.UnitName,
                        UnitType = u.UnitType,
                        QuantityRequest = u.QuantityRequest,
                        QuantityApproved = u.QuantityApproved,
                        QuantityPurchased = u.QuantityPurchased,
                        StatusPurchase = u.StatusPurchase,
                        UnitPrice = u.UnitPrice,
                        Currency = u.Currency,
                        DeliveryDate = u.DeliveryDate,
                        PriorityLevel = u.PriorityLevel,
                        Status = u.Status,
                        Note = u.Note,
                        VendorCode = u.VendorCode,
                        VendorName = u.VendorName,
                        DisplayOrder = i
                    });
                }
                i++;
            }
            if (listAdd.Count > 0)
            {
                _requestPurchaseProductRepository.Add(listAdd);
                _ = await CommitNoCheck(_requestPurchaseProductRepository.UnitOfWork);
            }
            if (listUpdate.Count > 0)
            {
                _requestPurchaseProductRepository.Update(listUpdate);
                _ = await CommitNoCheck(_requestPurchaseProductRepository.UnitOfWork);
            }
        }
        _repository.Update(pr);
        var result = await Commit(_repository.UnitOfWork);
        if (!result.IsValid)
            return result;

        return result;
    }

    public async Task<ValidationResult> Handle(RequestPurchaseDuplicateCommand request, CancellationToken cancellationToken)
    {
        if (!request.IsValid(_repository))
            return request.ValidationResult;
        var actionBy = _context.GetUserId();
        var actionByName = _context.UserClaims.FullName;
        var actionDate = DateTime.Now;
        var item = await _repository.GetById(request.RequestPurchaseId);
        item.Id = request.Id;
        item.Code = request.Code;
        item.Status = 0;
        item.QuantityApproved = 0;
        item.RequestBy = actionBy;
        item.RequestByName = actionByName;
        item.RequestDate = actionDate;
        item.RequestByEmail = _context.UserClaims.Email;
        item.CreatedBy = actionBy;
        item.CreatedByName = actionByName;
        item.CreatedDate = actionDate;

        var list = new List<RequestPurchaseProduct>();
        if (item.RequestPurchaseProduct.Any())
        {
            foreach (var x in item.RequestPurchaseProduct)
            {
                x.Id = Guid.NewGuid();
                x.RequestPurchaseId = request.Id;
                x.QuantityApproved = 0;
                x.QuantityPurchased = 0;
                list.Add(x);
            }
            item.RequestPurchaseProduct = list;
        }
        _repository.Add(item);
        return await Commit(_repository.UnitOfWork);
    }

    public async Task<ValidationResult> Handle(RequestPurchasePurchaseCommand request, CancellationToken cancellationToken)
    {
        var item = await _repository.GetById(request.Id);

        if (item is null)
        {
            return new ValidationResult() { Errors = new List<ValidationFailure>() { new ValidationFailure("id", "Quotation is not exist") } };
        }
        var obj = new RequestPurchase
        {
            Id = request.Id,
            Podate = DateTime.Now,
            Postatus = request.POStatus
        };
        _repository.Purchase(obj);
        return await Commit(_repository.UnitOfWork);
    }

    public async Task<ValidationResult> Handle(POPurchaseProductCommand request, CancellationToken cancellationToken)
    {
        var listPOCode = request?.ListUpdate?.Select(x => x.PurchaseRequestCode).Distinct()?.ToString();
        var item = await _repository.CheckPO(listPOCode);

        if (item != null)
        {
            var list = new List<RequestPurchaseProduct>();
            if (item.RequestPurchaseProduct?.Count > 0)
            {
                foreach (var x in item.RequestPurchaseProduct)
                {
                    x.StatusPurchase = request.ListUpdate?.Where(y => y.ProductId == x.ProductId && y.UnitType == x.UnitType && y.UnitCode == x.UnitCode).Select(y => y.StatusPurchase).FirstOrDefault();
                    x.QuantityApproved = (x.QuantityApproved ?? 0) + Convert.ToInt32(request.ListUpdate?.Where(y => y.ProductId == x.ProductId && y.UnitType == x.UnitType && y.UnitCode == x.UnitCode).Sum(y => y.QuantityApproved));
                    x.QuantityPurchased = x.QuantityPurchased + request.ListUpdate?.Where(y => y.ProductId == x.ProductId && y.UnitType == x.UnitType && y.UnitCode == x.UnitCode).Sum(y => y.Quantity);
                    list.Add(x);
                }
            }
            _requestPurchaseProductRepository.Update(list);
        }
        return await Commit(_requestPurchaseProductRepository.UnitOfWork);
    }

    public async Task<ValidationResult> Handle(UpdatePurchaseQtyCommand request, CancellationToken cancellationToken)
    {
        var dataPO = await _PORepository.GetQuantityPuchased(1, request.PurchaseRequestCode);
        var item = await _repository.GetById(request.Id);

        if (item != null)
        {
            var list = new List<RequestPurchaseProduct>();
            if (item.RequestPurchaseProduct?.Count > 0)
            {
                foreach (var x in item.RequestPurchaseProduct)
                {
                    x.QuantityApproved = dataPO?.Where(y => y.ProductId == x.ProductId && y.UnitType == x.UnitType && y.UnitCode == x.UnitCode).Sum(y => y.QuantityApproved) ?? 0;
                    x.QuantityPurchased = dataPO?.Where(y => y.ProductId == x.ProductId && y.UnitType == x.UnitType && y.UnitCode == x.UnitCode).Sum(y => y.QuantityPurchased) ?? 0;
                    list.Add(x);
                }
            }
            _requestPurchaseProductRepository.Update(list);
        }
        return await Commit(_requestPurchaseProductRepository.UnitOfWork);
    }

    public async Task<ValidationResult> Handle(RequestPurchaseAddOrdersCommand request, CancellationToken cancellationToken)
    {
        if (!await request.IsValidAsync(_repository, _requestPurchaseProductRepository, _orderRepository))
        {
            return request.ValidationResult;
        }

        var newProducts = new List<RequestPurchaseProduct>();

        var orderIdsToFetch = request.OrderIds.ToList();
        var orders = await _orderRepository.GetByIds(orderIdsToFetch);
        var orderProducts = await _orderProductRepository.GetByOrderIds(orderIdsToFetch);

        foreach (var orderProduct in orderProducts)
        {
            var order = orders.FirstOrDefault(o => o.Id == orderProduct.OrderId);
            var newProduct = new RequestPurchaseProduct
            {
                Id = Guid.NewGuid(),
                RequestPurchaseId = request.Id,
                OrderId = orderProduct.OrderId,
                OrderCode = order.Code,
                OrderProductId = orderProduct.Id,
                ProductId = orderProduct.ProductId,
                ProductCode = orderProduct.ProductCode,
                ProductName = orderProduct.ProductName,
                ProductImage = orderProduct.ProductImage,
                SourceLink = orderProduct.SourceLink,
                Origin = orderProduct.Origin,
                UnitCode = orderProduct.UnitCode,
                UnitName = orderProduct.UnitName,
                UnitType = orderProduct.UnitType,
                QuantityRequest = (double)orderProduct.Quantity,
                QuantityApproved = request.QuantityApproved,
                UnitPrice = orderProduct.UnitPrice,
                Note = orderProduct.Note,
                Currency = order.Currency,
                ShippingFee = order?.OrderServiceAdd?.Where(x => x.ServiceAddName == "Vận chuyển nội địa")
                        .Sum(x => x.Price) ?? 0,
                BidUsername = orderProduct.BidUsername,
                DisplayOrder = orderProduct.DisplayOrder
            };
            newProducts.Add(newProduct);
        }
        if (newProducts.Any())
        {
            _requestPurchaseProductRepository.Add(newProducts);
            return await Commit(_requestPurchaseProductRepository.UnitOfWork);
        }
        else
        {
            return new ValidationResult();
        }
    }
}
