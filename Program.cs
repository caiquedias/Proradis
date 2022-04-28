using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using ProradisEx1.Data.Dto;
using ProradisEx1.Interface;
using ProradisEx1.Service;
using System;
using System.Collections.Generic;

namespace ProradisEx1
{
    class Program
    {
        static void Main(string[] args)
        {
            var host = CreateDefaultBuilder().Build();

            using IServiceScope serviceScope = host.Services.CreateScope();
            IServiceProvider provider = serviceScope.ServiceProvider;
            var workerInstance = provider.GetRequiredService<IProcessDataService<CityPayloadDto, ResponseDto>>();

            var logger = provider.GetService<ILoggerFactory>()
                .CreateLogger<Program>();

            logger.LogDebug("Starting application");

            workerInstance.Process();
            host.Run();

            logger.LogDebug("All done!");
        }

        static IHostBuilder CreateDefaultBuilder()
        {
            return Host.CreateDefaultBuilder()
                .ConfigureAppConfiguration(app =>
                {
                    app.AddJsonFile("appsettings.json");
                })
                .ConfigureServices(services =>
                {
                    services
                    .AddScoped<IProcessDataService<CityPayloadDto, ResponseDto>, ProcessDataService>()
                    .AddScoped<ISendRequestService<CityPayloadDto, ResponseDto>, SendRequestService>()
                    .AddLogging(configure => configure.AddConsole())
                    .Configure<LoggerFilterOptions>(options => options.MinLevel = LogLevel.Information)
                    .BuildServiceProvider();
                });
        }
    }
}
