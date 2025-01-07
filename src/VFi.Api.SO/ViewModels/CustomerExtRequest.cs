using System.Xml.Linq;
using Microsoft.AspNetCore.Mvc;
using VFi.Domain.SO.Models;
using VFi.NetDevPack.Domain;

namespace VFi.Api.SO.ViewModels;

public class CustomerSignupRequest
{
    public int? Type { get; set; }
    public string? Source { get; set; }
    public string? Image { get; set; }

    public string? Name { get; set; }
    public string? Phone { get; set; }
    public string? Email { get; set; }
    public string? Country { get; set; }

    public string? BusinessSector { get; set; }
    public string? CompanyPhone { get; set; }
    public string? CompanyName { get; set; }
    public string? CompanySize { get; set; }
    public decimal? Capital { get; set; }
    public string? Note { get; set; }

    public int? Gender { get; set; }
    public int? Year { get; set; }
    public int? Month { get; set; }
    public int? Day { get; set; }

    public int? Status { get; set; }
    public Guid? AccountId { get; set; }
    public string? AccountUsername { get; set; }
    public string? AccountEmail { get; set; }
    public bool? AccountEmailVerified { get; set; }
    public string? AccountPhone { get; set; }
    public bool? AccountPhoneVerified { get; set; }



}
public class CustomerAccountEditRequest
{
    public Guid AccountId { get; set; }
    public string? Image { get; set; }

    public string? Name { get; set; }
    public string? Phone { get; set; }
    public string? Email { get; set; }

    public int? Gender { get; set; }
    public int? Year { get; set; }
    public int? Month { get; set; }
    public int? Day { get; set; }

}
public class CustomerImageEditRequest
{
    public Guid AccountId { get; set; }
    public string? Image { get; set; }

}
public class CustomerIdentityEditRequest
{
    public Guid AccountId { get; set; }

    public string? IdName { get; set; }

    public string? IdNumber { get; set; }
    public DateTime? IdDate { get; set; }
    public string? IdIssuer { get; set; }
    public string? IdImage1 { get; set; }
    public string? IdImage2 { get; set; }

}
public class ActiveBidRequest
{
    public Guid Id { get; set; }

    public int BidQuantity { get; set; }

}
public class ActiveBidByAccountRequest
{
    public Guid AccountId { get; set; }

    public int BidQuantity { get; set; }

}

public class UpdateBidQuantityRequest
{
    public Guid Id { get; set; }

    public int BidQuantity { get; set; }

}
public class UpdateBidQuantityByAccountRequest
{
    public Guid AccountId { get; set; }

    public int BidQuantity { get; set; }

}
public class DeactiveBidRequest
{
    public Guid Id { get; set; }


}
public class DeactiveBidByAccountRequest
{
    public Guid AccountId { get; set; }

}
public class ActiveBidHoldRequest
{
    public Guid Id { get; set; }

    public int BidQuantity { get; set; }
    public decimal Amount { get; set; }
    public string WalletCode { get; set; }

}
public class ActiveBidHoldByAccountRequest
{
    public Guid AccountId { get; set; }

    public int BidQuantity { get; set; }
    public decimal Amount { get; set; }
    public string WalletCode { get; set; }

}



public class UpBidQuantityHoldRequest
{
    public Guid Id { get; set; }

    public int BidQuantity { get; set; }
    public decimal Amount { get; set; }
    public string WalletCode { get; set; }

}
public class UpBidQuantityHoldByAccountRequest
{
    public Guid AccountId { get; set; }

    public int BidQuantity { get; set; }
    public decimal Amount { get; set; }
    public string WalletCode { get; set; }

}
public class DeactiveBidHoldRequest
{
    public Guid Id { get; set; }
    public string WalletCode { get; set; }


}
public class DeactiveBidHoldByAccountRequest
{
    public Guid AccountId { get; set; }
    public string WalletCode { get; set; }

}
