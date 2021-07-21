using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;

namespace WorkerServiceExample
{
    public class ScopedProcessingService : IScopedProcessingService
    {
        private readonly ILogger _logger;

        public ScopedProcessingService(ILogger<ScopedProcessingService> logger)
        {
            _logger = logger;
        }

        //handle medthod affter loop
        public void DoWork()
        {
            _logger.LogInformation(
                   "Scoped Processing Service is working at "+DateTime.Now);
        }
    }
}
