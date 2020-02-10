using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using backend.core.Data;
using backend.data.Infrastructure;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using NLog.Web;

namespace backend
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var logger = NLog.Web.NLogBuilder.ConfigureNLog("nlog.config").GetCurrentClassLogger();
            logger.Debug("init main");
            var host = CreateHostBuilder(args).Build();
            DataBaseInitializer.InitialDataBase<ApplicationDbContext>(host, ApplicationDbSeed.InitialApplicationDataBase);
            host.Run();
            NLog.LogManager.Shutdown();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseStartup<Startup>().ConfigureLogging(logging =>
                    {
                        logging.ClearProviders();
                        logging.SetMinimumLevel(LogLevel.Trace);
                    }).UseNLog();
                });
    }
}
