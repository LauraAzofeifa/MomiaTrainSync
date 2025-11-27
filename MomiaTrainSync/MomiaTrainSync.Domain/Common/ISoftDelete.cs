using System;
using System.Collections.Generic;
using System.Text;

namespace MomiaTrainSync.Domain.Common
{
    public interface ISoftDelete
    {
        bool Estado { get; set; }
    }
}
