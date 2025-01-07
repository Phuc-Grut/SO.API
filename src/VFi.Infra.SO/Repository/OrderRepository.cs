using Microsoft.EntityFrameworkCore;
using VFi.Domain.SO.Enums;
using VFi.Domain.SO.Interfaces;
using VFi.Domain.SO.Models;
using VFi.Domain.SO.Models.Extented.OrderCross;
using VFi.Infra.SO.Context;
using VFi.NetDevPack.Data;
using VFi.NetDevPack.Exceptions;
using VFi.NetDevPack.Queries;

namespace VFi.Infra.SO.Repository;

public class OrderRepository : IOrderRepository
{
    protected readonly SqlCoreContext Db;
    protected readonly DbSet<Order> DbSet;
    public IUnitOfWork UnitOfWork => Db;
    //public OrderRepository(IServiceProvider serviceProvider)
    //{
    //    var scope = serviceProvider.CreateScope();
    //    Db = scope.ServiceProvider.GetRequiredService<SqlCoreContext>();
    //    DbSet = Db.Set<Order>();
    //}
    public OrderRepository(SqlCoreContext context)
    {
        Db = context;
        DbSet = Db.Set<Order>();
    }

    public async Task<bool> ExistId(Guid id)
    {
        return await DbSet.FirstOrDefaultAsync(x => x.Id.Equals(id)) != default;
    }

    public void Add(Order Order)
    {
        DbSet.Add(Order);
    }

    public void Dispose()
    {
        Db.Dispose();
    }

    public async Task<IEnumerable<Order>> GetAll()
    {
        return await DbSet.ToListAsync();
    }

    public async Task<Order> GetById(Guid id)
    {
        return await DbSet
            .Include(x => x.Customer)
            .Include(x => x.OrderProduct)
            .Include(x => x.OrderServiceAdd)
            .Include(x => x.PaymentInvoice)
            .Include(x => x.OrderTracking)
            .Include(x => x.OrderInvoice)
            .FirstOrDefaultAsync(x => x.Id == id);
    }

    public async Task<Order> GetByIdWithCustomerAndProducts(Guid id)
    {
        return await DbSet
            .Include(x => x.Customer)
            .Include(x => x.OrderProduct)
            .FirstOrDefaultAsync(x => x.Id == id);
    }

    public async Task<IEnumerable<Order>> GetByIds(IList<Guid> ids)
    {
        return await DbSet
            .Include(x => x.Customer)
            .Include(x => x.OrderProduct)
            .Include(x => x.OrderServiceAdd)
            .Include(x => x.PaymentInvoice)
            .Include(x => x.OrderTracking)
            .Include(x => x.OrderInvoice)
            .Where(x => ids.Contains(x.Id))
            .ToListAsync();
    }
    public async Task<IEnumerable<Order>> GetByIdsWithCustomerId(IList<Guid> ids)
    {
        return await DbSet
            .Include(x => x.Customer)
            .Where(x => ids.Contains(x.Id))
            .ToListAsync();
    }
    public async Task<IEnumerable<Order>> GetByCodes(IList<string> codes)
    {
        return await DbSet
            .Include(x => x.Customer)
            .Where(x => codes.Contains(x.Code))
            .ToListAsync();
    }
    public void Remove(Order Order)
    {
        DbSet.Remove(Order);
    }

    public void Update(Order Order)
    {
        DbSet.Update(Order);
    }

    public async Task<Order> GetByCode(string code)
    {
        return await DbSet.FirstOrDefaultAsync(x => x.Code.Equals(code));
    }




    public async Task<IEnumerable<Order>> GetByCode(Guid customerId, IList<string> code)
    {
        return await DbSet
            .Where(x => x.CustomerId.Equals(customerId) && code.Contains(x.Code))
            .ToListAsync();
    }


    public async Task<(IEnumerable<Order>, int)> Filter(string? keyword, Dictionary<string, object> filter, IFopRequest request)
    {
        if (!string.IsNullOrEmpty(keyword))
        {
            keyword = keyword.Trim();
        }
        var query = DbSet
            .Include(x => x.Quotation)
            .Include(x => x.Contract)
            .Include(x => x.OrderProduct)
            .Include(x => x.OrderServiceAdd)
            .Include(x => x.PaymentInvoice)
            .AsQueryable();

        var keywordTracking = keyword?.Replace("-", "");

        if (!string.IsNullOrEmpty(keyword))
            query = query.Where(x => x.CustomerName.Contains(keyword)
            || x.Code.Contains(keyword)
            || x.Description.Contains(keyword)
            || x.Note.Contains(keyword)
            || (!string.IsNullOrEmpty(x.DomesticTracking) && x.DomesticTracking.Replace("-", "").Contains(keywordTracking!))
            || (x.OrderProduct != null && x.OrderProduct.Any(detail =>
                detail.SourceLink.Contains(keyword)
                || detail.ProductName.Contains(keyword)
                || (!string.IsNullOrEmpty(detail.SellerId) && detail.SellerId.Equals(keyword))
                || (!string.IsNullOrEmpty(detail.BidUsername) && detail.BidUsername.Equals(keyword))
                ))
            );

        foreach (var item in filter)
        {
            if (item.Key.Equals("status"))
            {
                query = query.Where(x => x.Status == int.Parse(item.Value + ""));
            }
            if (item.Key.Equals("customerId"))
            {
                query = query.Where(x => x.CustomerId.Equals(item.Value));
            }
            if (item.Key.Equals("employeeId"))
            {
                query = query.Where(x => x.AccountId == new Guid(item.Value + ""));
            }
            if (item.Key.Equals("orderType"))
            {
                query = query.Where(x => x.OrderType == item.Value.ToString());
            }
            if (item.Key.Equals("domesticStatus"))
            {
                query = query.Where(x => x.DomesticStatus.HasValue && x.DomesticStatus.Value.ToString() == item.Value.ToString());
            }
        }
        var (filtered, totalCount) = query.ApplyFop(request);
        return (await filtered.ToListAsync(), totalCount);
    }


    public async Task<(IEnumerable<Order>, int)> FilterCustom(string? keyword, List<string>? codes, List<string>? tracking, Dictionary<string, object> filter, IFopRequest request)
    {
        if (!string.IsNullOrEmpty(keyword))
        {
            keyword = keyword.Trim();
        }
        var query = DbSet
            .Include(x => x.OrderProduct)
            .Include(x => x.OrderServiceAdd)
            .Include(x => x.PaymentInvoice)
            .AsQueryable();

        var trackings = tracking?
            .Where(x => !string.IsNullOrEmpty(x)).ToArray();
        var orderCodes = codes?
            .Where(x => !string.IsNullOrEmpty(x)).ToArray();

        if (!string.IsNullOrEmpty(keyword))
            query = query.Where(x => x.CustomerName.Contains(keyword)
            || x.Code.Contains(keyword)
            || x.Description.Contains(keyword)
            || x.Note.Contains(keyword)
            || (!string.IsNullOrEmpty(x.InternalNote) && x.InternalNote.Contains(keyword))
            || (!string.IsNullOrEmpty(x.DomesticTracking)
                && x.DomesticTracking.Replace("-", "").Contains(keyword.Replace("-", "")))
            || (x.OrderProduct != null && x.OrderProduct.Any(detail =>
                detail.SourceLink.Contains(keyword)
                || detail.ProductName.Contains(keyword)
                || (!string.IsNullOrEmpty(detail.SellerId) && detail.SellerId.Equals(keyword))
                || (!string.IsNullOrEmpty(detail.BidUsername) && detail.BidUsername.Equals(keyword))
                ))
            );

        if (trackings != null && trackings.Count() > 1)
        {
            query = query.Where(x => !string.IsNullOrEmpty(x.DomesticTracking)
                && trackings.Contains(x.DomesticTracking.Replace("-", "").Trim()));
        }

        if (orderCodes != null && orderCodes.Count() > 1)
        {
            query = query.Where(x => orderCodes.Contains(x.Code));
        }

        foreach (var item in filter)
        {
            if (item.Key.Equals("status"))
            {
                query = query.Where(x => x.Status == int.Parse(item.Value + ""));
            }
            if (item.Key.Equals("customerId"))
            {
                query = query.Where(x => x.CustomerId.Equals(item.Value));
            }
            if (item.Key.Equals("employeeId"))
            {
                query = query.Where(x => x.AccountId == new Guid(item.Value + ""));
            }
            if (item.Key.Equals("orderType"))
            {
                query = query.Where(x => x.OrderType == item.Value.ToString());
            }
            if (item.Key.Equals("domesticStatus"))
            {
                if (item.Value.ToString() == "-1")
                {
                    query = query.Where(x => !x.DomesticStatus.HasValue || string.IsNullOrEmpty(x.DomesticTracking));
                }
                else
                {
                    query = query.Where(x => x.DomesticStatus.HasValue && x.DomesticStatus.Value.ToString() == item.Value.ToString());
                }

            }
        }
        var (filtered, totalCount) = query.ApplyFop(request);
        return (await filtered.ToListAsync(), totalCount);
    }


    public async Task<IEnumerable<Order>> GetListListBox(string? keyword, Dictionary<string, object> filter, int pagesize, int pageindex)
    {
        var query = DbSet.AsQueryable();
        if (!string.IsNullOrEmpty(keyword))
        {
            query = query.Where(x => x.Code.Contains(keyword) || x.CustomerName.Contains(keyword) || x.AccountName.Contains(keyword));
        }
        foreach (var item in filter)
        {
            if (item.Key.Equals("status"))
            {
                var list = (item.Value + "").Split(",").ToList();
                query = query.Where(x => list.Contains(x.Status.ToString()));
            }
            if (item.Key.Equals("diferenceStatus"))
            {
                var list = (item.Value + "").Split(",").ToList();
                query = query.Where(x => !list.Contains(x.Status.ToString()));
            }
            if (item.Key.Equals("orderType"))
            {
                var list = (item.Value + "").Split(",").ToList();
                query = query.Where(x => list.Contains(x.OrderType));
            }
            if (item.Key.Equals("orderDate"))
            {
                var date = item.Value != "" ? Convert.ToDateTime(item.Value) : DateTime.Now;
                query = query.Where(x => x.OrderDate >= date);
            }
            if (item.Key.Equals("customerId"))
            {
                query = query.Where(x => x.CustomerId == new Guid(item.Value.ToString()));
            }
            if (item.Key.Equals("warehouseId"))
            {
                query = query.Include(x => x.OrderProduct).Where(x => x.OrderProduct.Select(y => y.WarehouseId).Contains((Guid)item.Value));
            }
            if (item.Key.Equals("isContract") && item.Value == "0")
            {
                var listContract = Db.Contract.Where(x => x.OrderId != null).Select(y => y.OrderId).ToList();
                query = query.Where(x => !listContract.Contains(x.Id));
            }
            if (item.Key.Equals("isQuotation") && item.Value == "0")
            {
                var listQuotation = Db.Quotation.Where(x => x.SaleOrderId != null).Select(y => y.SaleOrderId).ToList();
                query = query.Where(x => !listQuotation.Contains(x.Id));
            }
            if (item.Key.Equals("statusExport") && item.Value == "0")
            {
                query = query.Where(x => x.Status != 0 && x.Status != 9).Include(x => x.OrderProduct).Where(x => x.OrderProduct.Any(y => y.Quantity > (y.QuantityExported ?? 0)));
            }
            if (item.Key.Equals("statusPurchase") && item.Value == "0")
            {
                query = query.Where(x => x.Status != 9).Include(x => x.OrderProduct).Where(x => x.OrderProduct.Any(y => y.Quantity > (y.QuantityPurchased ?? 0)));
            }
            if (item.Key.Equals("statusReturn") && item.Value == "0")
            {
                query = query.Include(x => x.OrderProduct).Where(x => x.OrderProduct.Any(y => y.Quantity > (y.QuantityReturned ?? 0)));
            }
            if (item.Key.Equals("statusSales") && item.Value == "0")
            {
                query = query.Include(x => x.OrderProduct).Where(x => x.OrderProduct.Any(y => y.Quantity > (y.QuantitySales ?? 0)));
            }
            if (item.Key.Equals("fromDate"))
            {
                var date = item.Value != "" ? Convert.ToDateTime(item.Value) : DateTime.Now;
                query = query.Where(x => x.OrderDate >= date);
            }
            if (item.Key.Equals("toDate"))
            {
                var date = item.Value != "" ? Convert.ToDateTime(item.Value).Add(new TimeSpan(23, 59, 59)) : DateTime.Now;
                query = query.Where(x => x.OrderDate <= date);
            }
            if (item.Key.Equals("currency"))
            {
                query = query.Where(x => x.Currency.Equals(item.Value));
            }
            if (item.Key.Equals("statusProduction") && item.Value == "0")
            {
                query = query.Include(x => x.OrderProduct).Where(x => x.OrderProduct.Any(y => (double?)y.Quantity > (y.QuantityProductioned ?? 0)));
            }

            if (item.Key.Equals("includeProduct") && item.Value == "1")
            {
                query = query.Include(x => x.OrderProduct);
            }
        }
        return await query.OrderByDescending(x => x.CreatedDate).Skip((pageindex - 1) * pagesize).Take(pagesize).ToListAsync();
    }
    public void Approve(Order t)
    {
        Db.Entry(t).Property(x => x.Status).IsModified = true;
        Db.Entry(t).Property(x => x.ApproveDate).IsModified = true;
        Db.Entry(t).Property(x => x.ApproveBy).IsModified = true;
        Db.Entry(t).Property(x => x.ApproveByName).IsModified = true;
        Db.Entry(t).Property(x => x.ApproveComment).IsModified = true;
    }

    public async Task<IEnumerable<Order>> GetReference(Dictionary<string, object> filter)
    {
        var query = DbSet.Include(x => x.OrderProduct).AsQueryable();
        foreach (var item in filter)
        {
            if (item.Key.Equals("status"))
            {
                var list = (item.Value + "").Split(",").ToList();
                query = query.Where(x => list.Contains(x.Status.ToString()));
            }
            if (item.Key.Equals("diferenceStatus"))
            {
                var list = (item.Value + "").Split(",").ToList();
                query = query.Where(x => !list.Contains(x.Status.ToString()));
            }
            if (item.Key.Equals("contractId"))
            {
                query = query.Where(x => x.ContractId.Equals(item.Value));
            }
            if (item.Key.Equals("productId"))
            {
                query = query.Where(x => x.OrderProduct.Any(x => x.ProductId.Equals(item.Value)));
            }
        }
        return await query.ToListAsync();
    }

    public async Task<IEnumerable<Order>> GetDataReport(Dictionary<string, object> filter)
    {
        var query = DbSet.Include(x => x.OrderProduct).ThenInclude(x => x.ReturnOrderProduct).AsQueryable();
        foreach (var item in filter)
        {
            if (item.Key.Equals("status"))
            {
                query = query.Where(x => x.Status == Convert.ToInt32(item.Value));
            }
            if (item.Key.Equals("diferenceStatus"))
            {
                query = query.Where(x => x.Status != Convert.ToInt32(item.Value));
            }
            if (item.Key.Equals("listCustomer") && !string.IsNullOrEmpty(item.Value + ""))
            {
                var listCustomer = ((string)item.Value).Split(",");
                query = query.Where(x => listCustomer.Contains(x.CustomerCode));
            }
            if (item.Key.Equals("listEmployee"))
            {
                var listEmployee = ((string)item.Value).Split(",");
                query = query.Where(x => listEmployee.Contains(x.AccountId.ToString()));
            }
            if (item.Key.Equals("fromDate"))
            {
                query = query.Where(x => x.OrderDate >= Convert.ToDateTime(item.Value));
            }
            if (item.Key.Equals("toDate"))
            {
                query = query.Where(x => x.OrderDate <= Convert.ToDateTime(item.Value).Add(new TimeSpan(23, 59, 59)));
            }
        }
        return await query.ToListAsync();
    }
    public void UploadFile(Order t)
    {
        Db.Entry(t).Property(x => x.File).IsModified = true;
    }
    public async Task<Order?> GetByCode(Guid customerId, string code)
    {
        return await DbSet
            .Include(x => x.OrderProduct)
            .Include(x => x.OrderServiceAdd)
            .Include(x => x.PaymentInvoice)
            .Include(x => x.OrderTracking)
            .SingleOrDefaultAsync(x => x.CustomerId.Equals(customerId) && x.Code.Equals(code));
    }

    public async Task<IEnumerable<Order>> Filter(Guid accountId)
    {
        var query = DbSet.AsQueryable();

        query = query.Where(x => accountId == x.AccountId);

        return await query.ToListAsync();
    }

    public async Task<IEnumerable<Order>> Filter(Dictionary<string, object> filter)
    {
        var query = DbSet.AsQueryable();

        foreach (var item in filter)
        {
            if (item.Key.Equals("accountId"))
            {
                query = query.Where(x => x.AccountId == (Guid)item.Value);
            }
            if (item.Key.Equals("groupEmployeeId"))
            {
                query = query.Where(x => x.GroupEmployeeId == (Guid)item.Value);
            }
        }
        return await query.ToListAsync();
    }
    public bool CheckUsing(Guid id)
    {
        return !(
            Db.Contract.Any(x => x.OrderId.Equals(id)) // hợp đồng
            || (Db.ExportWarehouseProduct.Select(x => (Guid)x.OrderProductId).Intersect(Db.OrderProduct.Where(y => y.OrderId == id).Select(y => y.Id)).Any())
            || (Db.RequestPurchaseProduct.Select(x => (Guid)x.OrderProductId).Intersect(Db.OrderProduct.Where(y => y.OrderId == id).Select(y => y.Id)).Any())
            || (Db.ReturnOrderProduct.Select(x => (Guid)x.OrderProductId).Intersect(Db.OrderProduct.Where(y => y.OrderId == id).Select(y => y.Id)).Any())
            || (Db.SalesDiscountProduct.Select(x => (Guid)x.OrderProductId).Intersect(Db.OrderProduct.Where(y => y.OrderId == id).Select(y => y.Id)).Any())
            );
    }
    public async Task<IEnumerable<OrderInformation>> GetInfoByCodes(Guid customerId, IList<string> code)
    {
        return await DbSet
            .Where(x => x.CustomerId.Equals(customerId) && code.Contains(x.Code))
            .Select(x => new OrderInformation
            {
                Code = x.Code,
                CreatedDate = x.CreatedDate,
                PaymentExpiryDate = x.PaymentExpiryDate,
                Status = x.Status,
            }).ToListAsync();
    }

    public async Task<IEnumerable<OrderInformation>> GetAuctionUnpaid(Guid customerId)
    {
        var pendingStatus = (int)OrderStatus.PendingConfirm;
        return await DbSet
            .Where(x => x.OrderType == "AUC" && x.CustomerId.Equals(customerId) && x.Status == pendingStatus && x.PaymentInvoice.Sum(x => x.Amount) <= 0)
            .Select(x => new OrderInformation
            {
                Code = x.Code,
                CreatedDate = x.CreatedDate,
                PaymentExpiryDate = x.PaymentExpiryDate,
                Status = x.Status,
            }).ToListAsync();
    }

    public async Task<IEnumerable<Order>> GetAuctionWithoutDomesticTracking(DateTime fromDate, DateTime toDate, int? top = 50)
    {
        var purchasedStatus = (int)OrderStatus.Purchased;
        return await DbSet
            .Include(x => x.OrderProduct)
            .Where(x => x.OrderType == "AUC"
                && x.Status == purchasedStatus
                && x.CreatedDate >= fromDate
                && x.CreatedDate <= toDate
                && string.IsNullOrEmpty(x.DomesticTracking)
            )
            .OrderBy(x => x.CreatedDate)
            .Take(top ?? 50)
            .ToListAsync();
    }


    public async Task<IEnumerable<Order>> GetMercariWithoutDomesticTracking(DateTime fromDate, DateTime toDate, int? top = 50)
    {
        var purchasedStatus = (int)OrderStatus.Purchased;
        return await DbSet
            .Include(x => x.OrderProduct)
            .Where(x => (x.OrderType == "CROSS" || x.OrderType == "BARGAIN")
                && x.Status == purchasedStatus
                && x.CreatedDate >= fromDate
                && x.CreatedDate <= toDate
                && string.IsNullOrEmpty(x.DomesticTracking)
                && x.OrderProduct.Any(x => x.SourceLink.Contains("https://jp.mercari.com/item/"))
            )
            .OrderBy(x => x.CreatedDate)
            .Take(top ?? 50)
            .ToListAsync();
    }


    public async Task<IEnumerable<Order>> GetOrderWithDomesticTracking(DateTime fromDate, DateTime toDate, int? top = 50)
    {
        var purchasedStatus = (int)OrderStatus.Purchased;
        return await DbSet
            .Include(x => x.OrderProduct)
            .Where(x => x.Status == purchasedStatus
                && x.CreatedDate >= fromDate
                && x.CreatedDate <= toDate
                && (!x.DomesticStatus.HasValue || x.DomesticStatus.Value != 10)
                && !string.IsNullOrEmpty(x.DomesticTracking)
                && !string.IsNullOrEmpty(x.DomesticCarrier)
            )
            .OrderBy(x => x.UpdatedDate)
            .ThenBy(x => x.CreatedDate)
            .Take(top ?? 50)
            .ToListAsync();
    }
}
