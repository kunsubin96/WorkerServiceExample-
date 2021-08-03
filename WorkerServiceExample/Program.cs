using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Serilog;

namespace WorkerServiceExample
{
    public class Program
    {
        //public static async Task Main(string[] args)
        //{
        //    Log.Logger = new LoggerConfiguration()
        //                    .MinimumLevel.Debug()
        //                    .MinimumLevel.Override("Microsoft", Serilog.Events.LogEventLevel.Warning)
        //                    .Enrich.FromLogContext()
        //                    .WriteTo.File("LogFile.txt")
        //                    .CreateLogger();

        //    await CreateWebHostBuilder(args).Build()
        //        .RunAsync();
        //}

        public static void Main(string[] args)
        {
            Log.Logger = new LoggerConfiguration()
                            .MinimumLevel.Debug()
                            .MinimumLevel.Override("Microsoft", Serilog.Events.LogEventLevel.Warning)
                            .Enrich.FromLogContext()
                            .WriteTo.File("LogFile.txt")
                            .CreateLogger();


            CreateWebHostBuilder(args).Build().Run();
        }

        public static IWebHostBuilder CreateWebHostBuilder(string[] args) =>
            WebHost.CreateDefaultBuilder(args)
                .UseStartup<Startup>()
                .UseSerilog();
    }
}
