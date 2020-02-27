using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;

namespace ReservoirDevs.Common.Extensions.Logging
{
    // ReSharper disable once InconsistentNaming
    public static class ILoggerExtensions
    {
        public static IDisposable CreateScope<TClass>(this ILogger<TClass> logger, string method, IDictionary<string, string> data = null)
        {
            data = data ?? new Dictionary<string, string>();

            data.Add("class", typeof(TClass).FullName);
            data.Add("method", method);

            return logger.BeginScope(data);
        }
    }
}