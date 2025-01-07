using VFi.Domain.SO.Models.Bids;
using VFi.NetDevPack.Queries;

namespace VFi.Domain.SO.Interfaces;

public interface IMyRepository
{
    Task DepositPushNotify(Guid accountId, decimal amount, string code, string description);
}
