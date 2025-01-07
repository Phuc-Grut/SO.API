using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using VFi.Application.SO.Queries.Extended;

namespace VFi.Api.SO.Controllers;

public partial class WalletTransactionController
{
    [HttpGet("get-by-account")]
    public async Task<IActionResult> GetByAccountId(Guid account, string? wallet, string? keyword, string? type, int? size, int? page)
    {
        if (wallet.IsNullOrEmpty())
        { wallet = ""; }
        if (keyword.IsNullOrEmpty())
        { keyword = ""; }
        if (type.IsNullOrEmpty())
        { type = ""; }
        if (!size.HasValue)
            size = 10;
        if (!page.HasValue)
            page = 1;
        var query = new WalletTransactionByAccountQuery()
        {
            AccountId = account,
            Wallet = wallet,
            Keyword = keyword,
            Type = type,
            PageSize = size.Value,
            PageNumber = page.Value,
            Status = 1
        };
        var result = await _mediator.Send(query);
        return Ok(result);
    }
}
