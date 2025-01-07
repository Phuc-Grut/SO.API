public class EmailNotifyRequest
{
    public EmailNotifyRequest() { }
    public string SenderCode { get; set; }
    public string SenderName { get; set; }
    public string Subject { get; set; }
    public string From { get; set; }
    public string To { get; set; }
    public string CC { get; set; }
    public string BCC { get; set; }
    public string Body { get; set; }
    public string TemplateCode { get; set; }

}
public class EmailBuilderRequest
{
    public EmailBuilderRequest() { }
    public string Subject { get; set; }
    public string JBody { get; set; }
    public string Template { get; set; }

}
