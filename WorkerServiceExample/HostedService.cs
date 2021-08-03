using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace WorkerServiceExample
{
    public class HostedService : IHostedService
    {
        private readonly ILogger<HostedService> _logger;
        private bool isExecute = true;

        private Task _executingTask;
        private CancellationTokenSource _cts;
        public HostedService(IServiceProvider services,
           ILogger<HostedService> logger)
        {
            Services = services;
            _logger = logger;
        }
        public IServiceProvider Services { get; }

        public Task StartAsync(CancellationToken cancellationToken)
        {
            // Create a linked token so we can trigger cancellation outside of this token's cancellation
            _cts = CancellationTokenSource.CreateLinkedTokenSource(cancellationToken);

            // Store the task we're executing
            _executingTask = ExecuteAsync(_cts.Token);

            // If the task is completed then return it, otherwise it's running
            return _executingTask.IsCompleted ? _executingTask : Task.CompletedTask;

        }

        public async Task StopAsync(CancellationToken cancellationToken)
        {
            _logger.LogInformation(
                "Service Hosted Service is stopping.");
            // Stop called without start
            if (_executingTask == null)
            {
                return;
            }

            // Signal cancellation to the executing method
            _cts.Cancel();

            // Wait until the task completes or the stop token triggers
            await Task.WhenAny(_executingTask, Task.Delay(-1, cancellationToken));

            // Throw if cancellation triggered
            cancellationToken.ThrowIfCancellationRequested();
        }

        // Derived classes should override this and execute a long running method until 
        // cancellation is requested
        private async  Task ExecuteAsync(CancellationToken cancellationToken)
        {
            _logger.LogInformation(
                "Service Hosted Service running.");

            await DoWork(cancellationToken);
        }


     
        private async Task DoWork(CancellationToken stoppingToken)
        {
            _logger.LogInformation(
                "Service Hosted Service is working.");
            using (var scope = Services.CreateScope())
            {
                var scopedProcessingService =
                    scope.ServiceProvider
                        .GetRequiredService<IScopedProcessingService>();


                while (!stoppingToken.IsCancellationRequested)
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
                    await Task.Yield();

                    scopedProcessingService.DoWork();

                    //min task 10000 milisSecond
                    await Task.Delay(60000, stoppingToken);
                }
            }
        }

    }
}
