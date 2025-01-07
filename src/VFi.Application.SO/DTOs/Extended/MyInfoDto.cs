using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VFi.Application.SO.DTOs;
public class MyInfoDto
{
    public Guid Id { get; set; }
    public Guid? AccountId { get; set; }
    public string Code { get; set; }
    public string Name { get; set; }
    public string Image { get; set; }
    public string Email { get; set; }
    public int? Level { get; set; }
    public bool? BidActive { get; set; }
    public int? BidQuantity { get; set; }
    public string IdName { get; set; }
    public string IdNumber { get; set; }
    public string IdImage1 { get; set; }
    public string IdImage2 { get; set; }
    public bool? TranActive { get; set; }
    public int? IdStatus { get; set; }
    public decimal? Cash { get; set; }
    public decimal? CashHold { get; set; }
    public decimal? CashHoldBid { get; set; }

    public int TotalBidCredit { get; set; } = 0;
    public int TotalBidLastTimeCredit { get; set; } = 0;
    public int TotalOrderPending { get; set; } = 0;
    public int MaxBid { get; set; } = 0;
    public int TotalCredit { get; set; } = 0;
    public int CreditAvailable { get; set; } = 0;
    public bool IsMaxBid { get; set; }
}
