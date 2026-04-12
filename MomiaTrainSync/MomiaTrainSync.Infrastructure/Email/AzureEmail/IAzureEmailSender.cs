using System;
using System.Collections.Generic;
using System.Text;

namespace MomiaTrainSync.Infrastructure.Email.AzureEmail
{
    public interface IAzureEmailSender
    {
        Task SendAsync(string to, string subject, string htmlBody);
    }
}
