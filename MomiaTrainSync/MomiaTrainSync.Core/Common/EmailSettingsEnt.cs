using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MomiaTrainSync.Core.Common
{
    public class EmailSettings
    {
        public string FromAddress { get; set; } = string.Empty;
        public string SmtpServer { get; set; } = string.Empty;
        public int Port { get; set; }
        public bool EnableSSL { get; set; }
        public string SenderName { get; set; } = string.Empty;
        public string SenderEmail { get; set; } = string.Empty; // usuario SMTP
        public string SenderPassword { get; set; } = string.Empty; // contraseña SMTP
    }
}
