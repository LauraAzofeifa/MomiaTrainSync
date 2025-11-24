using System;
using System.Collections.Generic;
using System.Text;

namespace MomiaTrainSync.Domain.Common
{
    public abstract class BaseSoftDelete
    {
        public bool Estado { get; set; } = true;
    }
}
