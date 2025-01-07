using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Swashbuckle.AspNetCore.Annotations;
using VFi.Api.SO.ViewModels;
using VFi.Application.SO.Commands;
using VFi.Application.SO.Events;
using VFi.Application.SO.Queries;
using VFi.NetDevPack.Mediator;

namespace VFi.Api.SO.Controllers;

public partial class CustomerController : ControllerBase
{
    [HttpGet("customer-revenue-load")]
    [SwaggerOperation(Summary = "Load Revenue")]
    public async Task<IActionResult> CustomerRevenueLoad([FromQuery] CustomerRevenueRequest request, CancellationToken cancellationToken)
    {
        var ev = new CustomerRevenueLoadEvent();
        ev.CustomerId = request.CustomerId;
        ev.AccountId = request.AccountId;
        ev.BackHour = request.BackHour;
        ev.Tenant = _context.Tenant;
        ev.Data = _context.Data;
        ev.Data_Zone = _context.Data_Zone;
        _ = _mediator.PublishEvent(ev, PublishStrategy.ParallelNoWait, cancellationToken);

        return Ok();
    }

    [HttpPost("signupv2")]
    public async Task<IActionResult> SignupV2([FromBody] RegisterRequest request)
    {
        var id = Guid.NewGuid();
        var command = new SignupNoPassCommand(id, request.Name, request.Email, request.Phone);
        var result = await _mediator.SendCommand(command);
        return Ok(result);
    }

    [HttpGet("pagingv1")]
    public async Task<IActionResult> PagingEx([FromQuery] CustomerPagingRequest request)
    {
        var query = new CustomerExPagingQuery(
              request.Keyword ?? "",
              request.Type,
              request.Status,
              request.Filter ?? "",
              request.Order ?? "",
              request.PageNumber,
              request.PageSize
          );
        var result = await _mediator.Send(query);
        return Ok(result);
    }

    [HttpPost("signup")]
    public async Task<IActionResult> Signup([FromBody] CustomerSignupRequest request)
    {
        _logger.LogError(JsonConvert.SerializeObject(request));
        //valid accountid
        if (request.AccountId != null && request.AccountId != Guid.Empty)
        {
            var checkExistAccountId = new CustomerCheckExistAccountId() { AccountId = request.AccountId.Value };
            var exist = await _mediator.Send(checkExistAccountId);
            if (exist)
                return Ok();
        }
        //valid accountemail
        if (!string.IsNullOrEmpty(request.AccountEmail))
        {
        }

        var code = await _mediator.Send(new GetCodeQuery(_codeSyntax.Customer, 1));
        var cmd = new CustomerAddCommand();
        cmd.Id = Guid.NewGuid();
        cmd.Code = code;
        cmd.Type = request.Type;
        cmd.Name = request.Name?.Trim();
        cmd.Phone = request.Phone;
        cmd.Email = request.Email;
        cmd.Country = request.Country;
        cmd.BusinessSector = request.BusinessSector;
        cmd.CompanyName = request.CompanyName;
        cmd.CompanyPhone = request.CompanyPhone;
        cmd.CompanySize = request.CompanySize;
        cmd.Capital = request.Capital;
        cmd.Note = request.Note;
        cmd.Status = request.Status;
        cmd.Gender = request.Gender;
        cmd.Year = request.Year;
        cmd.Month = request.Month;
        cmd.Day = request.Day;
        cmd.Image = request.Image;
        if (!string.IsNullOrEmpty(request.Source))
        {
        }
        cmd.AccountId = request.AccountId;
        cmd.AccountUsername = request.AccountUsername;
        cmd.AccountEmail = request.AccountEmail;
        cmd.AccountEmailVerified = request.AccountEmailVerified;
        cmd.AccountPhone = request.AccountPhone;
        cmd.AccountPhoneVerified = request.AccountPhoneVerified;

        var result = await _mediator.SendCommand(cmd);
        return Ok(cmd);
    }

    [HttpPost("edit-info-by-account")]
    public async Task<IActionResult> EditInfoByAccount([FromBody] CustomerAccountEditRequest request)
    {
        var rs = await _mediator.Send(new CustomerQueryByAccountId(request.AccountId));
        var cmd = new CustomerExEditCommand();
        cmd.Id = rs.Id;
        cmd.Name = request.Name;
        cmd.Phone = request.Phone;
        cmd.Email = request.Email;
        cmd.Gender = request.Gender;
        cmd.Year = request.Year;
        cmd.Month = request.Month;
        cmd.Day = request.Day;
        cmd.Image = request.Image;
        var result = await _mediator.SendCommand(cmd);
        return Ok(result);
    }

    [HttpPost("edit-image-by-account")]
    public async Task<IActionResult> EditImageByAccount([FromBody] CustomerImageEditRequest request)
    {
        var rs = await _mediator.Send(new CustomerIdQueryByAccountId(request.AccountId));
        var cmd = new CustomerImageEditCommand();
        cmd.Id = rs;
        cmd.Image = request.Image;
        var result = await _mediator.SendCommand(cmd);
        return Ok(result);
    }

    [HttpDelete("delete-image-by-account/{accountid}")]
    public async Task<IActionResult> DeleteImageByAccount(Guid accountid)
    {
        var rs = await _mediator.Send(new CustomerIdQueryByAccountId(accountid));
        var cmd = new CustomerImageEditCommand();
        cmd.Id = rs;
        cmd.Image = "";
        var result = await _mediator.SendCommand(cmd);
        return Ok(result);
    }

    [HttpPost("edit-identity-by-account")]
    public async Task<IActionResult> EditIdentityByAccount([FromBody] CustomerIdentityEditRequest request)
    {
        var rs = await _mediator.Send(new CustomerQueryByAccountId(request.AccountId));
        var cmd = new CustomerExIdentityEditCommand();
        cmd.Id = rs.Id;
        cmd.IdName = request.IdName;
        cmd.IdDate = request.IdDate;
        cmd.IdIssuer = request.IdIssuer;
        cmd.IdImage1 = request.IdImage1;
        cmd.IdImage2 = request.IdImage2;
        cmd.IdNumber = request.IdNumber;
        var result = await _mediator.SendCommand(cmd);
        return Ok(result);
    }

    [HttpGet("get-by-account/{id}")]
    public async Task<IActionResult> GetByAccount(Guid id)
    {
        var rs = await _mediator.Send(new CustomerQueryByAccountId(id));
        return Ok(rs);
    }

    [HttpGet("get-by-account-email/{email}")]
    public async Task<IActionResult> GetByAccountEmail(string email)
    {
        var rs = await _mediator.Send(new CustomerQueryByAccountEmail(email));
        return Ok(rs);
    }

    [HttpGet("get-by-account-username/{username}")]
    public async Task<IActionResult> GetByAccountUsername(string username)
    {
        var rs = await _mediator.Send(new CustomerQueryByAccountUsername(username));
        return Ok(rs);
    }

    //------------------------------------------------------------------------------------------

    [HttpGet("get-bid-by-account/{id}")]
    public async Task<IActionResult> GetBidByAccount(Guid id)
    {
        var rs = await _mediator.Send(new CustomerQueryByAccountId(id));
        return Ok(new
        {
            rs.AccountId,
            rs.Code,
            rs.Name,
            rs.BidActive,
            rs.BidQuantity,
            OrderUnpaid = 0,
            rs.Id
        });
    }

    [HttpPost("active-bid")]
    public async Task<IActionResult> ActiveBid([FromBody] ActiveBidRequest request)
    {
        var cmd = new CustomerExActiveBidCommand();
        cmd.Id = request.Id;
        cmd.BidQuantity = request.BidQuantity;
        var result = await _mediator.SendCommand(cmd);
        return Ok(result);
    }

    [HttpPost("active-bid-by-account")]
    public async Task<IActionResult> ActiveBidByAccount([FromBody] ActiveBidByAccountRequest request)
    {
        var customer = await _mediator.Send(new CustomerQueryByAccountId(request.AccountId));
        var cmd = new CustomerExActiveBidCommand();
        cmd.Id = customer.Id;
        cmd.BidQuantity = request.BidQuantity;
        var result = await _mediator.SendCommand(cmd);
        return Ok(result);
    }

    [HttpPost("update-bid-quantity")]
    public async Task<IActionResult> UpdateBidQuantity([FromBody] UpdateBidQuantityRequest request)
    {
        var cmd = new CustomerExEditBidQuantityCommand();
        cmd.Id = request.Id;
        cmd.BidQuantity = request.BidQuantity;
        var result = await _mediator.SendCommand(cmd);
        return Ok(result);
    }

    [HttpPost("update-bid-quantity-by-account")]
    public async Task<IActionResult> UpdateBidQuantity([FromBody] UpdateBidQuantityByAccountRequest request)
    {
        var customer = await _mediator.Send(new CustomerQueryByAccountId(request.AccountId));
        var cmd = new CustomerExEditBidQuantityCommand();
        cmd.Id = customer.Id;
        cmd.BidQuantity = request.BidQuantity;
        var result = await _mediator.SendCommand(cmd);
        return Ok(result);
    }

    [HttpPost("deactive-bid")]
    public async Task<IActionResult> DeactiveBid([FromBody] DeactiveBidRequest request)
    {
        var cmd = new CustomerExDeactiveBidCommand();
        cmd.Id = request.Id;
        var result = await _mediator.SendCommand(cmd);
        return Ok(result);
    }

    [HttpPost("deactive-bid-by-account")]
    public async Task<IActionResult> DeactiveBidByAccount([FromBody] DeactiveBidByAccountRequest request)
    {
        var customer = await _mediator.Send(new CustomerQueryByAccountId(request.AccountId));
        var cmd = new CustomerExDeactiveBidCommand();
        cmd.Id = customer.Id;
        var result = await _mediator.SendCommand(cmd);
        return Ok(result);
    }

    //----------------------
    [HttpPost("active-bid-hold")]
    public async Task<IActionResult> ActiveBidHoldByAccount([FromBody] ActiveBidHoldRequest request)
    {
        var cmd = new CustomerExActiveBidHoldCommand();
        cmd.Id = request.Id;
        cmd.BidQuantity = request.BidQuantity;
        cmd.Amount = request.Amount;
        cmd.WalletCode = request.WalletCode;
        var result = await _mediator.SendCommand(cmd);
        return Ok(result);
    }

    [HttpPost("active-bid-hold-by-account")]
    public async Task<IActionResult> ActiveBidHoldByAccount([FromBody] ActiveBidHoldByAccountRequest request)
    {
        var customer = await _mediator.Send(new CustomerQueryByAccountId(request.AccountId));
        var cmd = new CustomerExActiveBidHoldCommand();
        cmd.Id = customer.Id;
        cmd.BidQuantity = request.BidQuantity;
        cmd.Amount = request.Amount;
        cmd.WalletCode = request.WalletCode;
        var result = await _mediator.SendCommand(cmd);
        return Ok(result);
    }

    //-------------
    [HttpPost("up-bid-quantity-hold")]
    public async Task<IActionResult> UpdateBidQuantityHold([FromBody] UpBidQuantityHoldRequest request)
    {
        var cmd = new CustomerExUpBidQuantityHoldCommand();
        cmd.Id = request.Id;
        cmd.BidQuantity = request.BidQuantity;
        cmd.Amount = request.Amount;
        cmd.WalletCode = request.WalletCode;
        var result = await _mediator.SendCommand(cmd);
        return Ok(result);
    }

    [HttpPost("up-bid-quantity-hold-by-account")]
    public async Task<IActionResult> UpdateBidQuantityHoldByAccount([FromBody] UpBidQuantityHoldByAccountRequest request)
    {
        var customer = await _mediator.Send(new CustomerQueryByAccountId(request.AccountId));
        var cmd = new CustomerExUpBidQuantityHoldCommand();
        cmd.Id = customer.Id;
        cmd.BidQuantity = request.BidQuantity;
        cmd.Amount = request.Amount;
        cmd.WalletCode = request.WalletCode;
        var result = await _mediator.SendCommand(cmd);
        return Ok(result);
    }

    [HttpPost("deactive-bid-hold")]
    public async Task<IActionResult> DeactiveBidHoldByAccount([FromBody] DeactiveBidHoldRequest request)
    {
        var cmd = new CustomerExDeactiveBidHoldCommand();
        cmd.Id = request.Id;
        cmd.WalletCode = request.WalletCode;
        var result = await _mediator.SendCommand(cmd);
        return Ok(result);
    }

    [HttpPost("deactive-bid-hold-by-account")]
    public async Task<IActionResult> DeactiveBidHoldByAccount([FromBody] DeactiveBidHoldByAccountRequest request)
    {
        var customer = await _mediator.Send(new CustomerQueryByAccountId(request.AccountId));
        var cmd = new CustomerExDeactiveBidHoldCommand();
        cmd.Id = customer.Id;
        cmd.WalletCode = request.WalletCode;
        var result = await _mediator.SendCommand(cmd);
        return Ok(result);
    }

    [HttpGet("price-puchase-by-account-id")]
    public async Task<IActionResult> PricePuchaseByAccountId(Guid accountId, string? purchaseGroupCode, decimal? price)
    {
        var customerPricing = await _mediator.Send(new CustomerPricePuchaseByAccountIdQuery(accountId, purchaseGroupCode, price));

        return Ok(customerPricing);
    }

    [HttpGet("get-my-info")]
    public async Task<IActionResult> GetMyInfo(Guid accountId)
    {
        var query = new GetMyInfo(accountId);
        var result = await _mediator.Send(query);
        return Ok(result);
    }

    [HttpGet("get-account-info/{accountId}")]
    public async Task<IActionResult> GetAccountInfo(Guid accountId)
    {
        var query = new GetMyInfo(accountId);
        var result = await _mediator.Send(query);
        return Ok(result);
    }

    [HttpGet("get-bid-credit-setup/{accountId}")]
    public async Task<IActionResult> GetBidCreditSetup(Guid accountId)
    {
        var query = new GetBidCreditSetup(accountId);
        var result = await _mediator.Send(query);
        return Ok(result);
    }

    [HttpPost("get-by-accounts")]
    public async Task<IActionResult> GetByAccounts([FromBody] params Guid[] accountIds)
    {
        var rs = await _mediator.Send(new CustomerQueryByAccountIds(accountIds));
        return Ok(rs);
    }
}
