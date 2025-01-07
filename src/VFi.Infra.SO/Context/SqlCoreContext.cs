using Microsoft.EntityFrameworkCore;
using VFi.Domain.SO.Models;
using VFi.Domain.SO.Models.Extented;
using VFi.NetDevPack.Data;
using VFi.NetDevPack.Domain;
using VFi.NetDevPack.Mediator;
using VFi.NetDevPack.Messaging;


namespace VFi.Infra.SO.Context;

public sealed class SqlCoreContext : DbContext, IUnitOfWork
{
    private readonly IMediatorHandler _mediatorHandler;

    public SqlCoreContext(DbContextOptions<SqlCoreContext> options, IMediatorHandler mediatorHandler) : base(options)
    {
        _mediatorHandler = mediatorHandler;
        ChangeTracker.QueryTrackingBehavior = QueryTrackingBehavior.NoTracking;
        ChangeTracker.AutoDetectChangesEnabled = false;
    }
    public DbSet<Bank> Bank { get; set; }
    public DbSet<Business> Business { get; set; }
    public DbSet<Campaign> Campaign { get; set; }
    public DbSet<CampaignStatus> CampaignStatus { get; set; }
    public DbSet<CommodityGroup> CommodityGroup { get; set; }
    public DbSet<Contract> Contract { get; set; }
    public DbSet<ContractTerm> ContractTerm { get; set; }
    public DbSet<ContractType> ContractType { get; set; }
    public DbSet<Coupon> Coupon { get; set; }
    public DbSet<CouponCustomer> CouponCustomer { get; set; }
    public DbSet<CouponCustomerGroup> CouponCustomerGroup { get; set; }
    public DbSet<CouponGroup> CouponGroup { get; set; }
    public DbSet<Currency> Currency { get; set; }
    public DbSet<Customer> Customer { get; set; }
    public DbSet<CustomerAddress> CustomerAddress { get; set; }
    public DbSet<CustomerBank> CustomerBank { get; set; }
    public DbSet<CustomerBusinessMapping> CustomerBusinessMapping { get; set; }
    public DbSet<CustomerContact> CustomerContact { get; set; }
    public DbSet<CustomerGroup> CustomerGroup { get; set; }
    public DbSet<CustomerGroupMapping> CustomerGroupMapping { get; set; }
    public DbSet<CustomerPriceListCross> CustomerPriceListCross { get; set; }
    public DbSet<CustomerProfile> CustomerProfile { get; set; }
    public DbSet<CustomerSource> CustomerSource { get; set; }
    public DbSet<DeliveryMethod> DeliveryMethod { get; set; }
    public DbSet<DeliveryProduct> DeliveryProduct { get; set; }
    public DbSet<Employee> Employee { get; set; }
    public DbSet<SalesDiscount> SalesDiscount { get; set; }
    public DbSet<SalesDiscountProduct> SalesDiscountProduct { get; set; }
    public DbSet<ExchangeRate> ExchangeRate { get; set; }
    public DbSet<Expense> Expense { get; set; }
    public DbSet<Export> Export { get; set; }
    public DbSet<ExportProduct> ExportProduct { get; set; }
    public DbSet<ExportWarehouse> ExportWarehouse { get; set; }
    public DbSet<ExportWarehouseProduct> ExportWarehouseProduct { get; set; }
    public DbSet<GroupEmployee> GroupEmployee { get; set; }
    public DbSet<GroupEmployeeMapping> GroupEmployeeMapping { get; set; }
    public DbSet<Lead> Lead { get; set; }
    public DbSet<LeadActivity> LeadActivity { get; set; }
    public DbSet<LeadProfile> LeadProfile { get; set; }
    public DbSet<LeadTopicSuggest> LeadTopicSuggest { get; set; }
    public DbSet<Order> Order { get; set; }
    public DbSet<OrderCost> OrderCost { get; set; }
    public DbSet<OrderExpress> OrderExpress { get; set; }
    public DbSet<OrderExpressDetail> OrderExpressDetail { get; set; }
    public DbSet<OrderFulfillment> OrderFulfillment { get; set; }
    public DbSet<OrderFulfillmentDetail> OrderFulfillmentDetail { get; set; }
    public DbSet<OrderInvoice> OrderInvoice { get; set; }
    public DbSet<OrderProduct> OrderProduct { get; set; }
    public DbSet<OrderServiceAdd> OrderServiceAdd { get; set; }
    public DbSet<OrderTracking> OrderTracking { get; set; }
    public DbSet<PaymentInvoice> PaymentInvoice { get; set; }
    public DbSet<PaymentMethod> PaymentMethod { get; set; }
    public DbSet<PaymentTerm> PaymentTerm { get; set; }
    public DbSet<PaymentTermDetail> PaymentTermDetail { get; set; }
    public DbSet<PostOffice> PostOffice { get; set; }
    public DbSet<PriceList> PriceList { get; set; }
    public DbSet<PriceListCross> PriceListCross { get; set; }
    public DbSet<PriceListCrossDetail> PriceListCrossDetail { get; set; }
    public DbSet<PriceListDetail> PriceListDetail { get; set; }
    public DbSet<PriceListPurchase> PriceListPurchase { get; set; }
    public DbSet<PriceListPurchaseDetail> PriceListPurchaseDetail { get; set; }
    public DbSet<PriceListSurcharge> PriceListSurcharge { get; set; }
    public DbSet<ProductionOrder> ProductionOrder { get; set; }
    public DbSet<ProductionOrdersDetail> ProductionOrdersDetail { get; set; }
    public DbSet<Promotion> Promotion { get; set; }
    public DbSet<PromotionByValue> PromotionByValue { get; set; }
    public DbSet<PromotionCustomer> PromotionCustomer { get; set; }
    public DbSet<PromotionCustomerGroup> PromotionCustomerGroup { get; set; }
    public DbSet<PromotionDiscount> PromotionDiscount { get; set; }
    public DbSet<PromotionGroup> PromotionGroup { get; set; }
    public DbSet<PromotionProduct> PromotionProduct { get; set; }
    public DbSet<PromotionProductBuy> PromotionProductBuy { get; set; }
    public DbSet<PurchaseGroup> PurchaseGroup { get; set; }
    public DbSet<Quotation> Quotation { get; set; }
    public DbSet<QuotationAttachment> QuotationAttachment { get; set; }
    public DbSet<QuotationTerm> QuotationTerm { get; set; }
    public DbSet<Reason> Reason { get; set; }
    public DbSet<Report> Report { get; set; }
    public DbSet<ReportType> ReportType { get; set; }
    public DbSet<RequestPurchase> RequestPurchase { get; set; }
    public DbSet<RequestPurchaseProduct> RequestPurchaseProduct { get; set; }
    public DbSet<RequestQuote> RequestQuote { get; set; }
    public DbSet<ReturnOrder> ReturnOrder { get; set; }
    public DbSet<ReturnOrderProduct> ReturnOrderProduct { get; set; }
    public DbSet<RouteShipping> RouteShipping { get; set; }
    public DbSet<RptSoChiTietBanHang> RptSoChiTietBanHang { get; set; }
    public DbSet<RptTinhHinhThucHienDonBanHang> RptTinhHinhThucHienDonBanHang { get; set; }
    public DbSet<RptTinhHinhThucHienHopDongBan> RptTinhHinhThucHienHopDongBan { get; set; }
    public DbSet<RptTongHopBanHangTheoKhachHang> RptTongHopBanHangTheoKhachHang { get; set; }
    public DbSet<RptTongHopBanHangTheoMatHang> RptTongHopBanHangTheoMatHang { get; set; }
    public DbSet<RptTongHopBanHangTheoNhanVien> RptTongHopBanHangTheoNhanVien { get; set; }
    public DbSet<RptTongHopBanHangTheoNhomKhachHang> RptTongHopBanHangTheoNhomKhachHang { get; set; }
    public DbSet<RptTongHopBanHangTheoKhachHangVaDonBanHang> RptTongHopBanHangTheoKhachHangVaDonBanHang { get; set; }
    public DbSet<RptTongHopBanHangTheoMatHangVaKhachHang> RptTongHopBanHangTheoMatHangVaKhachHang { get; set; }
    public DbSet<SalesChannel> SalesChannel { get; set; }
    public DbSet<Service> Service { get; set; }
    public DbSet<ServiceAdd> ServiceAdd { get; set; }
    public DbSet<ShippingCarrier> ShippingCarrier { get; set; }
    public DbSet<ShippingMethod> ShippingMethod { get; set; }
    public DbSet<Store> Store { get; set; }
    public DbSet<StorePriceList> StorePriceList { get; set; }
    public DbSet<SurchargeGroup> SurchargeGroup { get; set; }
    public DbSet<Transaction> Transaction { get; set; }
    public DbSet<Wallet> Wallet { get; set; }
    public DbSet<WalletTransaction> WalletTransaction { get; set; }
    public DbSet<WalletType> WalletType { get; set; }
    //------------------------
    public DbSet<SP_DB_COUNT_CUSTOMERResult> SP_DB_COUNT_CUSTOMERResult { get; set; }
    public DbSet<SP_DB_OVERVIEWResult> SP_DB_OVERVIEWResult { get; set; }
    public DbSet<SP_DB_SELLING_PRODUCTResult> SP_DB_SELLING_PRODUCTResult { get; set; }
    public DbSet<SP_DB_CUSTMER_SALEResult> SP_DB_CUSTMER_SALEResult { get; set; }
    public DbSet<SP_DB_STORE_SALEResult> SP_DB_STORE_SALEResult { get; set; }
    public DbSet<SP_DB_SALESCHANNEL_SALEResult> SP_DB_SALESCHANNEL_SALEResult { get; set; }
    public DbSet<SP_DB_SALES_PRODUCT_BY_TIME> SP_DB_SALES_PRODUCT_BY_TIME { get; set; }
    public DbSet<SP_DB_CONTRACT> SP_DB_CONTRACT { get; set; }
    public DbSet<SP_DB_EMPLOYEE_EXCELLENT> SP_DB_EMPLOYEE_EXCELLENT { get; set; }
    public DbSet<SP_DB_PAYMENT> SP_DB_PAYMENT { get; set; }
    //-------------------------

    public DbSet<SP_DEACTIVE_BID_WALLETResult> SP_DEACTIVE_BID_WALLETResult { get; set; }
    public DbSet<SP_DEPOSIT_WALLETResult> SP_DEPOSIT_WALLETResult { get; set; }
    public DbSet<SP_HOLD_BID_WALLETResult> SP_HOLD_BID_WALLETResult { get; set; }
    public DbSet<SP_HOLD_WALLETResult> SP_HOLD_WALLETResult { get; set; }
    public DbSet<SP_PAY_ORDER_WALLETResult> SP_PAY_ORDER_WALLETResult { get; set; }
    public DbSet<SP_PAY_WALLETResult> SP_PAY_WALLETResult { get; set; }
    public DbSet<SP_REFUND_HOLD_WALLETResult> SP_REFUND_HOLD_WALLETResult { get; set; }
    public DbSet<SP_REFUND_PAY_ORDER_WALLETResult> SP_REFUND_PAY_ORDER_WALLETResult { get; set; }
    public DbSet<SP_REFUND_PAY_WALLETResult> SP_REFUND_PAY_WALLETResult { get; set; }
    public DbSet<SP_WITHDRAW_WALLETResult> SP_WITHDRAW_WALLETResult { get; set; }
    public DbSet<SP_WITHDRAW_WALLET_APPROVEResult> SP_WITHDRAW_WALLET_APPROVEResult { get; set; }

    public DbSet<SP_ADD_HOLD_BID_WALLETResult> SP_ADD_HOLD_BID_WALLETResult { get; set; }
    public DbSet<SP_REFUND_HOLD_BID_WALLETResult> SP_REFUND_HOLD_BID_WALLETResult { get; set; }
    public DbSet<SP_GET_PRICE_PUCHASE_BY_ACCOUNT_IDResult> SP_GET_PRICE_PUCHASE_BY_ACCOUNT_IDResult { get; set; }
    public DbSet<SP_CREATE_CROSS_ORDERResult> SP_CREATE_CROSS_ORDERResult { get; set; }
    public DbSet<SP_CREATE_INVOICE_PAY_ORDERResult> SP_CREATE_INVOICE_PAY_ORDERResult { get; set; }
    public DbSet<SP_CREATE_EXPRESS_ORDERResult> SP_CREATE_EXPRESS_ORDERResult { get; set; }
    public DbSet<SP_DEPOSIT_FROM_BANKResult> SP_DEPOSIT_FROM_BANKResult { get; set; }
    public DbSet<SP_ORDER_UPDATE_ADDRESSResult> SP_ORDER_UPDATE_ADDRESSResult { get; set; }
    public DbSet<SP_GET_MY_INFOResult> SP_GET_MY_INFOResult { get; set; }
    public DbSet<SP_GET_ORDER_COUNTERResult> SP_GET_ORDER_COUNTERResult { get; set; }
    public DbSet<SP_GET_ORDEREXPRESS_COUNTERResult> SP_GET_ORDEREXPRESS_COUNTERResult { get; set; }
    public DbSet<SP_GET_MY_TOP_ORDERResult> SP_GET_MY_TOP_ORDERResult { get; set; }
    public DbSet<SP_GET_MY_TOP_PAYMENTResult> SP_GET_MY_TOP_PAYMENTResult { get; set; }
    public DbSet<SP_CANCEL_ORDER_AUCTIONResult> SP_CANCEL_ORDER_AUCTIONResult { get; set; }
    public DbSet<SP_GET_BID_CREDIT_SETUPResult> SP_GET_BID_CREDIT_SETUPResult { get; set; }

    public DbSet<SP_CREATE_INVOICE_PAY_ORDER_EXPRESSResult> SP_CREATE_INVOICE_PAY_ORDER_EXPRESSResult { get; set; }
    public DbSet<SP_CREATE_INVOICE_WITHDRAWResult> SP_CREATE_INVOICE_WITHDRAWResult { get; set; }
    public DbSet<SP_ORDER_STATUS_COUNTERResult> SP_ORDER_STATUS_COUNTERResult { get; set; }

    public DbSet<SP_ACCOUNT_REVENUE_LOADResult> SP_ACCOUNT_REVENUE_LOADResult { get; set; }

    public DbSet<SP_ORDER_UPDATE_BID_USERNAMEResult> SP_ORDER_UPDATE_BID_USERNAMEResult { get; set; }
    public DbSet<SP_ORDER_UPDATE_ADD_TRACKINGResult> SP_ORDER_UPDATE_ADD_TRACKINGResult { get; set; }
    public DbSet<SP_ORDER_UPDATE_TRACKINGResult> SP_ORDER_UPDATE_TRACKINGResult { get; set; }
    public DbSet<SP_ORDER_UPDATE_DOMESTIC_DELIVERRY_STATUSResult> SP_ORDER_UPDATE_DOMESTIC_DELIVERRY_STATUSResult { get; set; }
    public DbSet<SP_ORDER_UPDATE_SHIPMENT_ROUTINGResult> SP_ORDER_UPDATE_SHIPMENT_ROUTINGResult { get; set; }
    public DbSet<SP_ORDER_UPDATE_NOTEResult> SP_ORDER_UPDATE_NOTEResult { get; set; }
    public DbSet<SP_ORDER_UPDATE_DOMESTIC_DELIVERYResult> SP_ORDER_UPDATE_DOMESTIC_DELIVERYResult { get; set; }
    public DbSet<SP_ORDER_CROSS_RECALCULATED_PRICEResult> SP_ORDER_CROSS_RECALCULATED_PRICEResult { get; set; }

   

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Ignore<FluentValidation.Results.ValidationResult>();
        modelBuilder.Ignore<NetDevPack.Domain.ValidationResult>();
        modelBuilder.Ignore<Event>();

        //-------------------------------
        modelBuilder.ApplyConfiguration(new Mappings.Configurations.BankConfiguration());
        modelBuilder.ApplyConfiguration(new Mappings.Configurations.BusinessConfiguration());
        modelBuilder.ApplyConfiguration(new Mappings.Configurations.CampaignConfiguration());
        modelBuilder.ApplyConfiguration(new Mappings.Configurations.CampaignStatusConfiguration());
        modelBuilder.ApplyConfiguration(new Mappings.Configurations.CommodityGroupConfiguration());
        modelBuilder.ApplyConfiguration(new Mappings.Configurations.ContractConfiguration());
        modelBuilder.ApplyConfiguration(new Mappings.Configurations.ContractTermConfiguration());
        modelBuilder.ApplyConfiguration(new Mappings.Configurations.ContractTypeConfiguration());
        modelBuilder.ApplyConfiguration(new Mappings.Configurations.CouponConfiguration());
        modelBuilder.ApplyConfiguration(new Mappings.Configurations.CouponCustomerConfiguration());
        modelBuilder.ApplyConfiguration(new Mappings.Configurations.CouponCustomerGroupConfiguration());
        modelBuilder.ApplyConfiguration(new Mappings.Configurations.CouponGroupConfiguration());
        modelBuilder.ApplyConfiguration(new Mappings.Configurations.CurrencyConfiguration());
        modelBuilder.ApplyConfiguration(new Mappings.Configurations.CustomerConfiguration());
        modelBuilder.ApplyConfiguration(new Mappings.Configurations.CustomerAddressConfiguration());
        modelBuilder.ApplyConfiguration(new Mappings.Configurations.CustomerBankConfiguration());
        modelBuilder.ApplyConfiguration(new Mappings.Configurations.CustomerBusinessMappingConfiguration());
        modelBuilder.ApplyConfiguration(new Mappings.Configurations.CustomerContactConfiguration());
        modelBuilder.ApplyConfiguration(new Mappings.Configurations.CustomerGroupConfiguration());
        modelBuilder.ApplyConfiguration(new Mappings.Configurations.CustomerGroupMappingConfiguration());
        modelBuilder.ApplyConfiguration(new Mappings.Configurations.CustomerPriceListCrossConfiguration());
        modelBuilder.ApplyConfiguration(new Mappings.Configurations.CustomerProfileConfiguration());
        modelBuilder.ApplyConfiguration(new Mappings.Configurations.CustomerSourceConfiguration());
        modelBuilder.ApplyConfiguration(new Mappings.Configurations.DeliveryMethodConfiguration());
        modelBuilder.ApplyConfiguration(new Mappings.Configurations.DeliveryProductConfiguration());
        modelBuilder.ApplyConfiguration(new Mappings.Configurations.EmployeeConfiguration());
        modelBuilder.ApplyConfiguration(new Mappings.Configurations.ExchangeRateConfiguration());
        modelBuilder.ApplyConfiguration(new Mappings.Configurations.ExpenseConfiguration());
        modelBuilder.ApplyConfiguration(new Mappings.Configurations.ExportConfiguration());
        modelBuilder.ApplyConfiguration(new Mappings.Configurations.ExportProductConfiguration());
        modelBuilder.ApplyConfiguration(new Mappings.Configurations.ExportWarehouseConfiguration());
        modelBuilder.ApplyConfiguration(new Mappings.Configurations.ExportWarehouseProductConfiguration());
        modelBuilder.ApplyConfiguration(new Mappings.Configurations.GroupEmployeeConfiguration());
        modelBuilder.ApplyConfiguration(new Mappings.Configurations.GroupEmployeeMappingConfiguration());
        modelBuilder.ApplyConfiguration(new Mappings.Configurations.LeadConfiguration());
        modelBuilder.ApplyConfiguration(new Mappings.Configurations.LeadCampaignConfiguration());
        modelBuilder.ApplyConfiguration(new Mappings.Configurations.LeadActivityConfiguration());
        modelBuilder.ApplyConfiguration(new Mappings.Configurations.LeadProfileConfiguration());
        modelBuilder.ApplyConfiguration(new Mappings.Configurations.LeadActivityConfiguration());
        modelBuilder.ApplyConfiguration(new Mappings.Configurations.LeadProfileConfiguration());
        modelBuilder.ApplyConfiguration(new Mappings.Configurations.LeadTopicSuggestConfiguration());
        modelBuilder.ApplyConfiguration(new Mappings.Configurations.OrderConfiguration());
        modelBuilder.ApplyConfiguration(new Mappings.Configurations.OrderCostConfiguration());
        modelBuilder.ApplyConfiguration(new Mappings.Configurations.OrderExpressConfiguration());
        modelBuilder.ApplyConfiguration(new Mappings.Configurations.OrderExpressDetailConfiguration());
        modelBuilder.ApplyConfiguration(new Mappings.Configurations.OrderFulfillmentConfiguration());
        modelBuilder.ApplyConfiguration(new Mappings.Configurations.OrderFulfillmentDetailConfiguration());
        modelBuilder.ApplyConfiguration(new Mappings.Configurations.OrderInvoiceConfiguration());
        modelBuilder.ApplyConfiguration(new Mappings.Configurations.OrderProductConfiguration());
        modelBuilder.ApplyConfiguration(new Mappings.Configurations.OrderServiceAddConfiguration());
        modelBuilder.ApplyConfiguration(new Mappings.Configurations.OrderTrackingConfiguration());
        modelBuilder.ApplyConfiguration(new Mappings.Configurations.PaymentInvoiceConfiguration());
        modelBuilder.ApplyConfiguration(new Mappings.Configurations.PaymentMethodConfiguration());
        modelBuilder.ApplyConfiguration(new Mappings.Configurations.PaymentTermConfiguration());
        modelBuilder.ApplyConfiguration(new Mappings.Configurations.PaymentTermDetailConfiguration());
        modelBuilder.ApplyConfiguration(new Mappings.Configurations.PostOfficeConfiguration());
        modelBuilder.ApplyConfiguration(new Mappings.Configurations.PriceListConfiguration());
        modelBuilder.ApplyConfiguration(new Mappings.Configurations.PriceListCrossConfiguration());
        modelBuilder.ApplyConfiguration(new Mappings.Configurations.PriceListCrossDetailConfiguration());
        modelBuilder.ApplyConfiguration(new Mappings.Configurations.PriceListDetailConfiguration());
        modelBuilder.ApplyConfiguration(new Mappings.Configurations.PriceListPurchaseConfiguration());
        modelBuilder.ApplyConfiguration(new Mappings.Configurations.PriceListPurchaseDetailConfiguration());
        modelBuilder.ApplyConfiguration(new Mappings.Configurations.PriceListSurchargeConfiguration());
        modelBuilder.ApplyConfiguration(new Mappings.Configurations.ProductionOrderConfiguration());
        modelBuilder.ApplyConfiguration(new Mappings.Configurations.ProductionOrdersDetailConfiguration());
        modelBuilder.ApplyConfiguration(new Mappings.Configurations.PromotionConfiguration());
        modelBuilder.ApplyConfiguration(new Mappings.Configurations.PromotionByValueConfiguration());
        modelBuilder.ApplyConfiguration(new Mappings.Configurations.PromotionCustomerConfiguration());
        modelBuilder.ApplyConfiguration(new Mappings.Configurations.PromotionCustomerGroupConfiguration());
        modelBuilder.ApplyConfiguration(new Mappings.Configurations.PromotionDiscountConfiguration());
        modelBuilder.ApplyConfiguration(new Mappings.Configurations.PromotionGroupConfiguration());
        modelBuilder.ApplyConfiguration(new Mappings.Configurations.PromotionProductConfiguration());
        modelBuilder.ApplyConfiguration(new Mappings.Configurations.PromotionProductBuyConfiguration());
        modelBuilder.ApplyConfiguration(new Mappings.Configurations.PurchaseGroupConfiguration());
        modelBuilder.ApplyConfiguration(new Mappings.Configurations.QuotationConfiguration());
        modelBuilder.ApplyConfiguration(new Mappings.Configurations.QuotationAttachmentConfiguration());
        modelBuilder.ApplyConfiguration(new Mappings.Configurations.QuotationTermConfiguration());
        modelBuilder.ApplyConfiguration(new Mappings.Configurations.ReasonConfiguration());
        modelBuilder.ApplyConfiguration(new Mappings.Configurations.ReportConfiguration());
        modelBuilder.ApplyConfiguration(new Mappings.Configurations.ReportTypeConfiguration());
        modelBuilder.ApplyConfiguration(new Mappings.Configurations.RequestPurchaseConfiguration());
        modelBuilder.ApplyConfiguration(new Mappings.Configurations.RequestPurchaseProductConfiguration());
        modelBuilder.ApplyConfiguration(new Mappings.Configurations.RequestQuoteConfiguration());
        modelBuilder.ApplyConfiguration(new Mappings.Configurations.ReturnOrderConfiguration());
        modelBuilder.ApplyConfiguration(new Mappings.Configurations.ReturnOrderProductConfiguration());
        modelBuilder.ApplyConfiguration(new Mappings.Configurations.RouteShippingConfiguration());
        modelBuilder.ApplyConfiguration(new Mappings.Configurations.RptSoChiTietBanHangConfiguration());
        modelBuilder.ApplyConfiguration(new Mappings.Configurations.RptTinhHinhThucHienDonBanHangConfiguration());
        modelBuilder.ApplyConfiguration(new Mappings.Configurations.RptTinhHinhThucHienHopDongBanConfiguration());
        modelBuilder.ApplyConfiguration(new Mappings.Configurations.RptTongHopBanHangTheoKhachHangConfiguration());
        modelBuilder.ApplyConfiguration(new Mappings.Configurations.RptTongHopBanHangTheoMatHangConfiguration());
        modelBuilder.ApplyConfiguration(new Mappings.Configurations.RptTongHopBanHangTheoNhanVienConfiguration());
        modelBuilder.ApplyConfiguration(new Mappings.Configurations.RptTongHopBanHangTheoNhomKhachHangConfiguration());
        modelBuilder.ApplyConfiguration(new Mappings.Configurations.RptTongHopBanHangTheoMatHangVaKhachHangConfiguration());
        modelBuilder.ApplyConfiguration(new Mappings.Configurations.RptTongHopBanHangTheoKhachHangVaDonBanHangConfiguration());
        modelBuilder.ApplyConfiguration(new Mappings.Configurations.SalesChannelConfiguration());
        modelBuilder.ApplyConfiguration(new Mappings.Configurations.ServiceConfiguration());
        modelBuilder.ApplyConfiguration(new Mappings.Configurations.ServiceAddConfiguration());
        modelBuilder.ApplyConfiguration(new Mappings.Configurations.ShippingCarrierConfiguration());
        modelBuilder.ApplyConfiguration(new Mappings.Configurations.ShippingMethodConfiguration());
        modelBuilder.ApplyConfiguration(new Mappings.Configurations.StoreConfiguration());
        modelBuilder.ApplyConfiguration(new Mappings.Configurations.StorePriceListConfiguration());
        modelBuilder.ApplyConfiguration(new Mappings.Configurations.SurchargeGroupConfiguration());
        modelBuilder.ApplyConfiguration(new Mappings.Configurations.TransactionConfiguration());
        modelBuilder.ApplyConfiguration(new Mappings.Configurations.WalletConfiguration());
        modelBuilder.ApplyConfiguration(new Mappings.Configurations.SalesDiscountConfiguration());
        modelBuilder.ApplyConfiguration(new Mappings.Configurations.SalesDiscountProductConfiguration());
        modelBuilder.ApplyConfiguration(new Mappings.Configurations.WalletTransactionConfiguration());
        modelBuilder.ApplyConfiguration(new Mappings.Configurations.WalletTypeConfiguration());
        base.OnModelCreating(modelBuilder);
    }

    public async Task<bool> Commit()
    {
        // Dispatch Domain Events collection. 
        // Choices:
        // A) Right BEFORE committing data (EF SaveChanges) into the DB will make a single transaction including  
        // side effects from the domain event handlers which are using the same DbContext with "InstancePerLifetimeScope" or "scoped" lifetime
        // B) Right AFTER committing data (EF SaveChanges) into the DB will make multiple transactions. 
        // You will need to handle eventual consistency and compensatory actions in case of failures in any of the Handlers. 
        await _mediatorHandler.PublishDomainEvents(this).ConfigureAwait(false);

        // After executing this line all the changes (from the Command Handler and Domain Event Handlers) 
        // performed through the DbContext will be committed

        var success = await SaveChangesAsync() > 0;

        return success;
    }
}

public static class MediatorExtension
{
    public static async Task PublishDomainEvents<T>(this IMediatorHandler mediator, T ctx) where T : DbContext
    {
        var domainEntities = ctx.ChangeTracker
            .Entries<Entity>()
            .Where(x => x.Entity.DomainEvents != null && x.Entity.DomainEvents.Any());

        var domainEvents = domainEntities
            .SelectMany(x => x.Entity.DomainEvents)
            .ToList();

        domainEntities.ToList()
            .ForEach(entity => entity.Entity.ClearDomainEvents());

        var tasks = domainEvents
            .Select(async (domainEvent) =>
            {
                await mediator.PublishEvent(domainEvent);
            });

        await Task.WhenAll(tasks);
    }
}
