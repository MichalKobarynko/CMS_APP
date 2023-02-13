using LoggingService;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Serilog;
using Serilog.Filters;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace CMS_APP
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var host = CreateHostBuilder(args).Build();

            using (var scope = host.Services.CreateScope())
            {
                try
                {
                    int zero = 0;
                    int res = 100 / zero;
                }
                catch(DivideByZeroException ex)
                {
                    Log.Error("B³¹d podczas dzielenia przez zero {0} {1} {2} {3}", ex.Message, ex.StackTrace, ex.InnerException, ex.Source);
                }
            }

            host.Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .UseSerilog((hostingContext, loggerConfiguration) => loggerConfiguration
                   .Filter.ByExcluding(Matching.FromSource("Microsoft.AspNetCore.SpaServices"))
                   .Enrich.FromLogContext()
                   .Enrich.WithProperty("Application", "CMS_CORE_NG")
                   .Enrich.WithProperty("MachineName", Environment.MachineName)
                   .Enrich.WithProperty("CurrentManagedThreadId", Environment.CurrentManagedThreadId)
                   .Enrich.WithProperty("OSVersion", Environment.OSVersion)
                   .Enrich.WithProperty("Version", Environment.Version)
                   .Enrich.WithProperty("UserName", Environment.UserName)
                   .Enrich.WithProperty("ProcessId", Process.GetCurrentProcess().Id)
                   .Enrich.WithProperty("ProcessName", Process.GetCurrentProcess().ProcessName)
                   .WriteTo.File(formatter: new CustomTextFormatter(),
                                path: Path.Combine(hostingContext.HostingEnvironment.ContentRootPath + $"/Logs/", $"load_error_{DateTime.Now:yyyyMMdd}.txt"))
                   .ReadFrom.Configuration(hostingContext.Configuration))
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseStartup<Startup>();
                });
    }
}
