using System;
using System.Collections.Generic;
using System.Text;

namespace MomiaTrainSync.Core.Helpers
{
    public static class ValidationHelper
    {
        public static List<string> ValidationRequired(params (string name, object? value)[] fields)
        {
            var missing = new List<string>();

            foreach (var (name, value) in fields)
            {
                if (value == null)
                {
                    missing.Add(name);
                    continue;
                }

                switch (value)
                {
                    case string str when string.IsNullOrWhiteSpace(str):
                        missing.Add(name);
                        break;

                    case int i when i <= 0:
                        missing.Add(name);
                        break;

                    case byte b when b <= 0:
                        missing.Add(name);
                        break;

                    case DateOnly d when d == default:
                        missing.Add(name);
                        break;

                    case DateTime dt when dt == default:
                        missing.Add(name);
                        break;
                }
            }

            return missing;
        }
    }
}
