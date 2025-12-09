using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MomiaTrainSync.Core.Common
{
    public class EmailSettings
    {
        public string ConnectionString { get; set; } = string.Empty;
        public string SenderAddress { get; set; } = string.Empty;   // Ej: DoNotReply@xxxx.azurecomm.net
    }

}
