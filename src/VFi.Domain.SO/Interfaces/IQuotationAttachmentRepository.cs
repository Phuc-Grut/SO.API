using VFi.Domain.SO.Models;
using VFi.NetDevPack.Data;

namespace VFi.Domain.SO.Interfaces;

public interface IQuotationAttachmentRepository : IRepository<QuotationAttachment>
{
    Task<IEnumerable<QuotationAttachment>> Filter(Guid id, int pagesize, int pageindex);
    Task<int> FilterCount(Guid id);
    Task<IEnumerable<QuotationAttachment>> GetAll(Guid id);
}
