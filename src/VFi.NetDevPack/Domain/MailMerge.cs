using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VFi.NetDevPack.Domain;

public class MailMerge
{
    public string Id { get; set; } = "";
    public string Name { get; set; } = "";
    public string Email { get; set; } = "";
    public string Phone { get; set; } = "";
    public string Token { get; set; } = "";
    public string VerifyUrl { get; set; } = "";
    public string Otp { get; set; } = "";

    public string ProductName { get; set; } = "";
    public string PreviewImage { get; set; } = "";
    public string ProductCode { get; set; } = "";
    public string CurrentPrice { get; set; } = "";
    public string BidPrice { get; set; } = "";
    public string BidTime { get; set; } = "";
    public string EndTime { get; set; } = "";
    public string Link { get; set; } = "";
    public string BidTimeSchedule { get; set; } = "";

    public List<EmailItem> Items { get; set; } = new List<EmailItem>();
    public List<EmailItem> Items2 { get; set; } = new List<EmailItem>();
}
public class EmailItem
{
    public string Id { get; set; }
    public string Name { get; set; }
    public string Code { get; set; }
    public string Description { get; set; }
    public string Price { get; set; }
    public string Link { get; set; }
    public string Image { get; set; }
    public string Image1 { get; set; }
    public string Image2 { get; set; }
    public string Image3 { get; set; }
}
