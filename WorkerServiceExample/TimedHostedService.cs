using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace WorkerServiceExample
{
    public class TimedHostedService : IHostedService, IDisposable
    {
        private readonly ILogger<TimedHostedService> _logger;
        private Timer _timer;
        IScopedProcessingService scopedProcessingService;

        public TimedHostedService(IServiceProvider services, ILogger<TimedHostedService> logger)
        {
            _logger = logger;
            Services = services;
        }
        public IServiceProvider Services { get; }

        public Task StartAsync(CancellationToken stoppingToken)
        {
            _logger.LogInformation("Hosted Service running.");

            scopedProcessingService = Services.CreateScope().ServiceProvider.GetRequiredService<IScopedProcessingService>();

            _timer = new Timer(DoWork, null, TimeSpan.Zero,
                TimeSpan.FromMinutes(1));

            return Task.CompletedTask;
        }

        //handle
        private void DoWork(object state)
        {
            //using (var scope = Services.CreateScope())
            //{
            //    var scopedProcessingService =
            //        scope.ServiceProvider
            //            .GetRequiredService<IScopedProcessingService>();
            //    scopedProcessingService.DoWork();
            //};

            scopedProcessingService.DoWork();

        }
        public Task StopAsync(CancellationToken stoppingToken)
        {
            _logger.LogInformation("Hosted Service is stopping.");

            _timer?.Change(Timeout.Infinite, 0);

            return Task.CompletedTask;
        }

        public void Dispose()
        {
            _timer?.Dispose();
        }
    }
}
