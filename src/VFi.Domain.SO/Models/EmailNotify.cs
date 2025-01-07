using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VFi.Domain.SO.Models;

public class SendEmail
{
    public string SenderCode { get; set; }
    public string SenderName { get; set; }
    public string Subject { get; set; }
    public string From { get; set; }
    public string To { get; set; }
    public string CC { get; set; }
    public string BCC { get; set; }
    public string Body { get; set; }
}

public class EmailNotify : SendEmail
{
    public string Order { get; set; }
    public string TemplateCode { get; set; }
}

public class CampaignSendEmail : SendEmail
{
    public string Campaign { get; set; }
}
