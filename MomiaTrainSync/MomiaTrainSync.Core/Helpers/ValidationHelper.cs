using System;
using System.Collections.Generic;
using System.Text;

namespace MomiaTrainSync.Core.Helpers
{
    public static class ValidationHelper
    {
        public static List<string> ValidationRequired(params (string name, string? value)[] fields)
        {
            return fields
                .Where(f => string.IsNullOrWhiteSpace(f.value))
                .Select(f => f.name)
                .ToList();
        }
    }
}
