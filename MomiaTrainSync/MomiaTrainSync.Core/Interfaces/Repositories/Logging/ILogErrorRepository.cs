using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MomiaTrainSync.Core.Interfaces.Repositories.Logging
{
    public interface ILogErrorRepository
    {
        Task AddLogAsync(string origen, Exception exception);
    }
}
