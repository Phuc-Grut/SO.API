using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VFi.Domain.SO.Models;
using VFi.NetDevPack.Data;
using VFi.NetDevPack.Queries;

namespace VFi.Domain.SO.Interfaces;

public interface IPaymentInvoiceRepository : IRepository<PaymentInvoice>
{
    Task<PaymentInvoice> GetByCode(string code);
    Task<IEnumerable<PaymentInvoice>> Filter(Dictionary<string, object> filter, int? top);
    Task<(IEnumerable<PaymentInvoice>, int, IEnumerable<PaymentInvoice>)> Filter(string? keyword, int? status, IFopRequest request);
    void Update(IEnumerable<PaymentInvoice> items);
    void Add(IEnumerable<PaymentInvoice> items);
    void Remove(IEnumerable<PaymentInvoice> t);
    void Changelocked(PaymentInvoice t);
}
