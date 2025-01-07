using Flurl.Http.Configuration;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using VFi.Domain.PO.Interfaces;
using VFi.Domain.SO.Interfaces;
using VFi.Domain.SO.Interfaces.Extented;
using VFi.Infra.PIM.Context;
using VFi.Infra.SO.Consul;
using VFi.Infra.SO.Context;
using VFi.Infra.SO.Context.Extented;
using VFi.Infra.SO.EventSourcing;
using VFi.Infra.SO.MassTransit;
using VFi.Infra.SO.Repository;
using VFi.Infra.SO.Repository.EventSourcing;
using VFi.Infra.SO.Repository.Extentded;
using VFi.Infra.SO.Repository.Extented;
using VFi.NetDevPack;
using VFi.NetDevPack.Context;
using VFi.NetDevPack.Events;
using VFi.NetDevPack.Mediator;

namespace VFi.Infra.SO;

public class StartupApplication : IStartupApplication
{
    public int Priority => 2;
    public bool BeforeConfigure => true;

    public void ConfigureServices(IServiceCollection services, IConfiguration configuration)
    {
        services.AddConsul(configuration);
        var connectionStringPlaceHolder = configuration.GetConnectionString("SOConnection");
        services.AddDbContext<SqlCoreContext>((serviceProvider, dbContextBuilder) =>
        {
            var context = serviceProvider.GetRequiredService<IContextUser>();
            var connectionString = connectionStringPlaceHolder.Replace("{data_zone}", context.Data_Zone).Replace("{data}", context.Data);
            dbContextBuilder.UseSqlServer(connectionString);
        });

        var connectionEventPlaceHolder = configuration.GetConnectionString("SOEventConnection");
        services.AddDbContext<EventStoreSqlContext>((serviceProvider, dbContextBuilder) =>
        {
            var context = serviceProvider.GetRequiredService<IContextUser>();
            var connectionString = connectionStringPlaceHolder.Replace("{data_zone}", context.Data_Zone).Replace("{data}", context.Data);
            dbContextBuilder.UseSqlServer(connectionString);
        });
        //----------------------
        services.AddSingleton<IFlurlClientFactory, PerBaseUrlFlurlClientFactory>();
        services.AddSingleton<MasterApiContext>();
        services.AddSingleton<POApiContext>();
        services.AddSingleton<WMSApiContext>();
        services.AddSingleton<PIMApiContext>();
        services.AddSingleton<MESApiContext>();
        //----------------------
        services.AddTransient<Publisher>();
        services.AddTransient<IMediatorHandler, VFi.Infra.SO.Bus.MediatorHandler>();
        services.AddTransient<IEventStoreRepository, EventStoreSqlRepository>();
        services.AddTransient<IEventStore, SqlEventStore>();
        services.AddTransient<EventStoreSqlContext>();
        // Infra - Data

        services.AddTransient<ICustomerGroupRepository, CustomerGroupRepository>();
        services.AddTransient<ICustomerSourceRepository, CustomerSourceRepository>();
        services.AddTransient<IEmployeeRepository, EmployeeRepository>();
        services.AddTransient<IExpenseRepository, ExpenseRepository>();
        services.AddTransient<IPromotionGroupRepository, PromotionGroupRepository>();
        services.AddTransient<IBankRepository, BankRepository>();
        services.AddTransient<IBusinessRepository, BusinessRepository>();
        services.AddTransient<ICampaignRepository, CampaignRepository>();
        services.AddTransient<ICampaignStatusRepository, CampaignStatusRepository>();
        services.AddTransient<IContractTermRepository, ContractTermRepository>();
        services.AddTransient<IQuotationTermRepository, QuotationTermRepository>();
        services.AddTransient<ICustomerRepository, CustomerRepository>();
        services.AddTransient<IPaymentTermRepository, PaymentTermRepository>();
        services.AddTransient<IPaymentTermDetailRepository, PaymentTermDetailRepository>();
        services.AddTransient<IShippingMethodRepository, ShippingMethodRepository>();
        services.AddTransient<IShippingCarrierRepository, ShippingCarrierRepository>();
        services.AddTransient<IPaymentMethodRepository, PaymentMethodRepository>();
        services.AddTransient<IReasonRepository, ReasonRepository>();
        services.AddTransient<IRequestQuoteRepository, RequestQuoteRepository>();
        services.AddTransient<IPriceListRepository, PriceListRepository>();
        services.AddTransient<IPriceListDetailRepository, PriceListDetailRepository>();
        services.AddTransient<ICustomerAddressRepository, CustomerAddressRepository>();
        services.AddTransient<ICustomerContactRepository, CustomerContactRepository>();
        services.AddTransient<IQuotationRepository, QuotationRepository>();
        services.AddTransient<IOrderCostRepository, OrderCostRepository>();
        services.AddTransient<IQuotationAttachmentRepository, QuotationAttachmentRepository>();
        services.AddTransient<IGroupEmployeeMappingRepository, GroupEmployeeMappingRepository>();
        services.AddTransient<IGroupEmployeeRepository, GroupEmployeeRepository>();
        services.AddTransient<IStoreRepository, StoreRepository>();
        services.AddTransient<ICustomerGroupMappingRepository, CustomerGroupMappingRepository>();
        services.AddTransient<ICustomerBusinessMappingRepository, CustomerBusinessMappingRepository>();
        services.AddTransient<IServiceAddRepository, ServiceAddRepository>();
        services.AddTransient<ILeadRepository, LeadRepository>();
        services.AddTransient<ILeadCampaignRepository, LeadCampaignRepository>();
        services.AddTransient<ILeadActivityRepository, LeadActivityRepository>();
        services.AddTransient<ILeadProfileRepository, LeadProfileRepository>();
        services.AddTransient<IOrderRepository, OrderRepository>();
        services.AddTransient<IOrderProductRepository, OrderProductRepository>();
        services.AddTransient<IOrderServiceAddRepository, OrderServiceAddRepository>();
        services.AddTransient<IOrderTrackingRepository, OrderTrackingRepository>();
        services.AddTransient<ISyntaxCodeRepository, SyntaxCodeRepository>();
        services.AddTransient<IReturnOrderRepository, ReturnOrderRepository>();
        services.AddTransient<IReturnOrderProductRepository, ReturnOrderProductRepository>();
        services.AddTransient<IProductionOrderRepository, ProductionOrderRepository>();
        services.AddTransient<IProductionOrdersDetailRepository, ProductionOrdersDetailRepository>();
        services.AddTransient<IPromotionRepository, PromotionRepository>();
        services.AddTransient<IProductRepository, ProductRepository>();
        services.AddTransient<IUnitRepository, UnitRepository>();
        services.AddTransient<IPromotionByValueRepository, PromotionByValueRepository>();
        services.AddTransient<IPromotionCustomerRepository, PromotionCustomerRepository>();
        services.AddTransient<IPromotionCustomerGroupRepository, PromotionCustomerGroupRepository>();
        services.AddTransient<IContractRepository, ContractRepository>();
        services.AddTransient<ISalesChannelRepository, SalesChannelRepository>();
        services.AddTransient<IStorePriceListRepository, StorePriceListRepository>();
        services.AddTransient<IDeliveryProductRepository, DeliveryProductRepository>();
        services.AddTransient<IDeliveryMethodRepository, DeliveryMethodRepository>();
        services.AddTransient<IContractTypeRepository, ContractTypeRepository>();
        services.AddTransient<IContractTypeRepository, ContractTypeRepository>();
        services.AddTransient<IExportRepository, ExportRepository>();
        services.AddTransient<IExportProductRepository, ExportProductRepository>();
        services.AddTransient<IExportWarehouseRepository, ExportWarehouseRepository>();
        services.AddTransient<IExportWarehouseProductRepository, ExportWarehouseProductRepository>();
        services.AddTransient<IRequestPurchaseRepository, RequestPurchaseRepository>();
        services.AddTransient<IExportTemplateRepository, ExportTemplateRepository>();
        services.AddTransient<IRequestPurchaseProductRepository, RequestPurchaseProductRepository>();
        services.AddTransient<IPaymentInvoiceRepository, PaymentInvoiceRepository>();
        services.AddTransient<ICustomerBankRepository, CustomerBankRepository>();
        services.AddTransient<IPromotionProductRepository, PromotionProductRepository>();
        services.AddTransient<IPromotionProductBuyRepository, PromotionProductBuyRepository>();
        services.AddTransient<IOrderInvoiceRepository, OrderInvoiceRepository>();
        services.AddTransient<IReportRepository, ReportRepository>();
        services.AddTransient<ISalesDiscountRepository, SalesDiscountRepository>();
        services.AddTransient<ISalesDiscountProductRepository, SalesDiscountProductRepository>();
        services.AddTransient<IReportTypeRepository, ReportTypeRepository>();
        services.AddTransient<IRptSoChiTietBanHangRepository, RptSoChiTietBanHangRepository>();
        services.AddTransient<IRptTongHopBanHangTheoKhachHangRepository, RptTongHopBanHangTheoKhachHangRepository>();
        services.AddTransient<IRptTongHopBanHangTheoMatHangRepository, RptTongHopBanHangTheoMatHangRepository>();
        services.AddTransient<IRptTongHopBanHangTheoNhanVienRepository, RptTongHopBanHangTheoNhanVienRepository>();
        services.AddTransient<IRptTinhHinhThucHienDonBanHangRepository, RptTinhHinhThucHienDonBanHangRepository>();
        services.AddTransient<IRptTinhHinhThucHienHopDongBanRepository, RptTinhHinhThucHienHopDongBanRepository>();
        services.AddTransient<IRptTongHopBanHangTheoNhomKhachHangRepository, RptTongHopBanHangTheoNhomKhachHangRepository>();
        services.AddTransient<IRptTongHopBanHangTheoKhachHangVaDonBanHangRepository, RptTongHopBanHangTheoKhachHangVaDonBanHangRepository>();
        services.AddTransient<IRptTongHopBanHangTheoMatHangVaKhachHangRepository, RptTongHopBanHangTheoMatHangVaKhachHangRepository>();
        services.AddTransient<IExchangeRateRepository, ExchangeRateRepository>();
        services.AddTransient<ISOContextProcedures, SOContextProcedures>();
        services.AddTransient<IEmailMasterRepository, EmailMasterRepository>();
        services.AddScoped<IEventRepository, EventRepository>();

        //-----------------------
        services.AddTransient<IPORepository, PORepository>();
        services.AddTransient<IWMSRepository, WMSRepository>();
        services.AddTransient<IPIMRepository, PIMRepository>();
        services.AddTransient<IMESRepository, MESRepository>();
        //----------------------
        services.AddScoped<SqlCoreContext>();
        services.AddRabbitMQ(configuration);

        //-----------------------
        services.AddTransient<ISOExtProcedures, SOExtProcedures>();
        services.AddSingleton<IDApiContext>();
        services.AddSingleton<BidApiContext>();
        services.AddSingleton<SpiderApiContext>();
        services.AddSingleton<MyApiContext>();
        services.AddTransient<IBidRepository, BidRepository>();
        services.AddTransient<ICarrierJapanRepository, CarrierJapanRepository>();
        services.AddTransient<IMyRepository, MyRepository>();
        services.AddTransient<ICustomerPriceListCrossRepository, CustomerPriceListCrossRepository>();
        services.AddTransient<ICommodityGroupRepository, CommodityGroupRepository>();
        services.AddTransient<IOrderExpressDetailRepository, OrderExpressDetailRepository>();
        services.AddTransient<IOrderExpressRepository, OrderExpressRepository>();
        services.AddTransient<IOrderFulfillmentDetailRepository, OrderFulfillmentDetailRepository>();
        services.AddTransient<IOrderFulfillmentRepository, OrderFulfillmentRepository>();
        services.AddTransient<IPartnerRepository, PartnerRepository>();
        services.AddTransient<IPostOfficeRepository, PostOfficeRepository>();
        services.AddTransient<IRouteShippingRepository, RouteShippingRepository>();
        services.AddTransient<IPriceListCrossDetailRepository, PriceListCrossDetailRepository>();
        services.AddTransient<IPriceListCrossRepository, PriceListCrossRepository>();
        services.AddTransient<IPriceListPurchaseDetailRepository, PriceListPurchaseDetailRepository>();
        services.AddTransient<IPriceListPurchaseRepository, PriceListPurchaseRepository>();
        services.AddTransient<IPriceListSurchargeRepository, PriceListSurchargeRepository>();
        services.AddTransient<IPurchaseGroupRepository, PurchaseGroupRepository>();
        services.AddTransient<ISurchargeGroupRepository, SurchargeGroupRepository>();
        services.AddTransient<ITransactionRepository, TransactionRepository>();
        services.AddTransient<IWalletRepository, WalletRepository>();
        services.AddTransient<IWalletTransactionRepository, WalletTransactionRepository>();
        services.AddTransient<IWalletTypeRepository, WalletTypeRepository>();

        services.AddTransient<IAccountRepository, AccountRepository>();


        services.AddTransient<IEmailMasterRepository, EmailMasterRepository>();
    }

    public void Configure(WebApplication application, IWebHostEnvironment webHostEnvironment)
    {
        try
        {
            application.UseConsul(application.Lifetime);
        }
        catch (Exception)
        {
        }
    }
}
