using System.Collections.Generic;
using System.Threading.Tasks;

namespace Puchase_and_payables.MailHandler.Service
{
    public interface IEmailService
    {
        Task Send(EmailMessage emailMessage);
        List<EmailMessage> ReceiveEmail(int maxCount = 10);
    }
}
