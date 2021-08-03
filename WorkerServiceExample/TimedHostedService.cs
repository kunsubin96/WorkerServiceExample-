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
        private IScopedProcessingService scopedProcessingService;
        private bool isExecute = true;

        public TimedHostedService(IServiceProvider services, ILogger<TimedHostedService> logger)
        {
            _logger = logger;
            Services = services;
        }
        public IServiceProvider Services { get; }

        public Task StartAsync(CancellationToken stoppingToken)
        {
            _logger.LogInformation("Hosted Service running.");

            //create service handle
            scopedProcessingService = Services.CreateScope().ServiceProvider.GetRequiredService<IScopedProcessingService>();

            _timer = new Timer(DoWork, null, TimeSpan.Zero,
                TimeSpan.FromMinutes(1));

            return Task.CompletedTask;
        }

        //handle loop 
        private void DoWork(object state)
        {
            //handle call method service
            //at minute
            //if(DateTime.Now.Minute==38&& isExecute)
            //{
            //    scopedProcessingService.DoWork();
            //    isExecute = false;

            //}
            //if (DateTime.Now.Minute == 39)
            //{
            //    isExecute = true;
            //}

            //handle at 24:00
            //if (DateTime.Now.Hour == 0 && isExecute)
            //{
            //    scopedProcessingService.DoWork();
            //    isExecute = false;

            //}
            //if (DateTime.Now.Hour == 1)
            //{
            //    isExecute = true;
            //}

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
