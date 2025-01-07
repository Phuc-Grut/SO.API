using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VFi.Domain.SO.Models;
using VFi.NetDevPack.Models;

namespace VFi.Domain.SO.Interfaces;

public interface IEmailMasterRepository
{
    Task<IEnumerable<SendConfigCombobox>> GetListboxSendConfig();
    Task<IEnumerable<SendTemplateCombobox>> GetListboxSendTemplate();
    Task<EmailBody> EmailBuilder(string subject, string jBody, string template);
    void EmailNotify(EmailNotify request);
    void SendEmail(SendEmail request);
    void CampaignSendEmail(CampaignSendEmail request);
    Task<List<SendTransactionLog>> GetListSendTransaction(Dictionary<string, string?> filter);
    Task<SendTransaction> GetSendTransactionById(Guid sendTransactionId);
}
