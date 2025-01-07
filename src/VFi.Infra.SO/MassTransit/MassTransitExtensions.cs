using MassTransit;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using VFi.Infra.ACC.MassTransit.Consumers;
using VFi.Infra.SO.MassTransit.Consumers;
using VFi.NetDevPack.Configuration;

namespace VFi.Infra.SO.MassTransit;

public static class MassTransitExtensions
{
    public static IServiceCollection AddRabbitMQ(this IServiceCollection services, IConfiguration configuration)
    {

        var queueSettings = configuration.GetSection("RabbitConfig").Get<RabbitConfig>();
        //services.AddSingleton(sp => configuration.GetSection("RabbitConfig").Get<RabbitConfig>());
        if (queueSettings != null)
            if (queueSettings.RabbitEnabled)
            {
                services.AddMassTransit(x =>
                {
                    if (queueSettings.ConsumerEnabled)
                    {
                        // x.AddConsumer<UserEmailEventConsumer>();
                        // x.AddConsumer<UserLogedEventConsumer>(); 
                        x.AddConsumer<EventConsumer>();
                        x.AddConsumer<AddRptSoChiTietBanHangConsumer>();
                        x.AddConsumer<AddRptTinhHinhThucHienDonBanHangConsumer>();
                        x.AddConsumer<AddRptTinhHinhThucHienHopDongBanConsumer>();
                        x.AddConsumer<AddRptTongHopBanHangTheoKhachHangConsumer>();
                        x.AddConsumer<AddRptTongHopBanHangTheoKhachHangVaDonBanHangConsumer>();
                        x.AddConsumer<AddRptTongHopBanHangTheoMatHangConsumer>();
                        x.AddConsumer<AddRptTongHopBanHangTheoMatHangVaKhachHangConsumer>();
                        x.AddConsumer<AddRptTongHopBanHangTheoNhanVienConsumer>();
                        x.AddConsumer<AddRptTongHopBanHangTheoNhomKhachHangConsumer>();
                        x.AddConsumer<FulfillmentRequestAddExtConsumer>();
                        x.AddConsumer<CustomerRevenueLoadConsumer>();
                        x.AddConsumer<PaymentNotifyConsumer>();
                        x.AddConsumer<PurchaseNotifyConsumer>();
                        x.AddConsumer<PurchaseNotifyAllConsumer>();
                        x.AddConsumer<OrderFetchTrackingConsumer>();
                        x.AddConsumer<OrderFetchDomesticDeliveryConsumer>();
                        x.AddConsumer<OrderItemFetchDomesticDeliveryConsumer>();
                        x.AddConsumer<OrderStatusChangedConsumer>();
                        //  x.AddConsumer<FaultConsumer<UserEmailEvent>>();
                    }



                    x.UsingRabbitMq((context, cfg) =>
                    {
                        cfg.Host(queueSettings.RabbitHostName, queueSettings.RabbitVirtualHost, h =>
                        {
                            h.Username(queueSettings.RabbitUsername);
                            h.Password(queueSettings.RabbitPassword);
                        });
                        //cfg.ConfigureEndpoints(context);

                        if (queueSettings.ConsumerEnabled)
                        {
                            //cfg.ReceiveEndpoint("user-email-event", e =>
                            //{
                            //    e.ExchangeType = ExchangeType.Direct;


                            //    //e.ConfigureConsumer<UserEmailEventConsumer>(context, c =>
                            //    //{
                            //    //    //https://masstransit-project.com/usage/exceptions.html#retry-configuration
                            //    //    c.UseMessageRetry(r =>
                            //    //    {
                            //    //      //  r.Interval(2, TimeSpan.FromMilliseconds(1000));
                            //    //        r.Intervals(TimeSpan.FromMilliseconds(3000), TimeSpan.FromMilliseconds(8000), TimeSpan.FromMilliseconds(12000));
                            //    //        r.Ignore<ArgumentNullException>();
                            //    //        r.Ignore<DataException>(x => x.Message.Contains("SQL"));
                            //    //    });
                            //    //});
                            //});



                            //cfg.ReceiveEndpoint("user-email-event_error", e =>
                            //{
                            //    e.ExchangeType = ExchangeType.Direct;
                            //    //e.ConfigureConsumer<FaultConsumer<UserEmailEvent>>(context, c =>
                            //    //{

                            //    //});
                            //});

                            cfg.ReceiveEndpoint("Event", e =>
                            {
                                e.ConfigureConsumer<EventConsumer>(context);
                                e.ConfigureConsumer<AddRptSoChiTietBanHangConsumer>(context);
                                e.ConfigureConsumer<AddRptTinhHinhThucHienDonBanHangConsumer>(context);
                                e.ConfigureConsumer<AddRptTinhHinhThucHienHopDongBanConsumer>(context);
                                e.ConfigureConsumer<AddRptTongHopBanHangTheoKhachHangConsumer>(context);
                                e.ConfigureConsumer<AddRptTongHopBanHangTheoKhachHangVaDonBanHangConsumer>(context);
                                e.ConfigureConsumer<AddRptTongHopBanHangTheoMatHangConsumer>(context);
                                e.ConfigureConsumer<AddRptTongHopBanHangTheoMatHangVaKhachHangConsumer>(context);
                                e.ConfigureConsumer<AddRptTongHopBanHangTheoNhanVienConsumer>(context);
                                e.ConfigureConsumer<AddRptTongHopBanHangTheoNhomKhachHangConsumer>(context);
                            });
                            cfg.ReceiveEndpoint("fulfillmentRequestAddExt-event", e =>
                            {
                                e.ConfigureConsumer<FulfillmentRequestAddExtConsumer>(context);
                                e.PrefetchCount = 1;
                            });
                            cfg.ReceiveEndpoint("CustomerRevenueLoadConsumer-Event", e =>
                            {
                                e.ConfigureConsumer<CustomerRevenueLoadConsumer>(context);
                                e.PrefetchCount = 1;
                            });
                            cfg.ReceiveEndpoint("PaymentNotify-Event", e =>
                            {
                                e.ConfigureConsumer<PaymentNotifyConsumer>(context, c =>
                                {
                                    c.UseMessageRetry(r =>
                                    {
                                        r.Incremental(3, TimeSpan.FromMilliseconds(3000), TimeSpan.FromMilliseconds(10000));
                                    });
                                });
                                //e.ConfigureConsumer<PaymentNotifyConsumer>(context);
                                e.PrefetchCount = 1;
                            });
                            cfg.ReceiveEndpoint("PurchaseNotify-Event", e =>
                            {
                                e.ConfigureConsumer<PurchaseNotifyConsumer>(context, c =>
                                {
                                    c.UseMessageRetry(r =>
                                    {
                                        r.Incremental(3, TimeSpan.FromMilliseconds(3000), TimeSpan.FromMilliseconds(10000));
                                    });
                                });
                                //e.ConfigureConsumer<PurchaseNotifyConsumer>(context);
                                e.PrefetchCount = 1;
                            });

                            cfg.ReceiveEndpoint("PurchaseNotifyAll-Event", e =>
                            {
                                e.ConfigureConsumer<PurchaseNotifyAllConsumer>(context);
                                e.PrefetchCount = 1;
                            });
                            cfg.ReceiveEndpoint("OrderFetchTracking-Event", e =>
                            {
                                e.ConfigureConsumer<OrderFetchTrackingConsumer>(context);
                                e.PrefetchCount = 1;
                            });
                            cfg.ReceiveEndpoint("OrderFetchDomesticDelivery-Event", e =>
                            {
                                e.ConfigureConsumer<OrderFetchDomesticDeliveryConsumer>(context);
                                e.PrefetchCount = 1;
                            });
                            cfg.ReceiveEndpoint("OrderItemFetchDomesticDelivery-Event", e =>
                            {
                                e.ConfigureConsumer<OrderItemFetchDomesticDeliveryConsumer>(context);
                                e.PrefetchCount = 1;
                                e.UseMessageRetry(r => r.Interval(3, TimeSpan.FromSeconds(30)));
                                e.UseRateLimit(1, TimeSpan.FromSeconds(10));
                            });
                            cfg.ReceiveEndpoint("OrderStatusChanged-Event", e =>
                            {
                                e.ConfigureConsumer<OrderStatusChangedConsumer>(context);
                            });
                            //cfg.ReceiveEndpoint(typeof(UserLogedEvent).FullName, e =>
                            //{
                            //    e.ConfigureConsumer<UserLogedEventConsumer>(context);
                            //});
                        }

                    });
                });

                if (queueSettings.PublisherEnabled)
                {
                    // EndpointConvention.Map<UserEmailEvent>(queueSettings.BuildEndPoint("user-email-event")); 
                    // services.AddScoped<IEmailRepository, EmailRepository>();
                }

            }

        //var provider = services.BuildServiceProvider();
        //var busControl = provider.GetRequiredService<IBusControl>();



        return services;
    }

    public static IApplicationBuilder UseRabbitMQ(this IApplicationBuilder app, IHostApplicationLifetime lifetime)
    {


        return app;
    }
}
