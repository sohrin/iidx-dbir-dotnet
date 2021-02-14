using System;
using System.IO;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using NLog;
using Dbir.Dto;
using Dbir.Repository;

namespace dbir
{
    class Program
    {
        private static Logger logger = LogManager.GetCurrentClassLogger();

        static void Main(string[] args)
        {
            logger.Debug("Program.Main() BEGIN.");

            // appsettings.jsonファイルの読み込み
            IConfiguration configuration = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json", true, true)
                .Build();

            //setup our DI
            var serviceProvider = new ServiceCollection()
                .AddLogging()
                .AddSingleton(configuration)
                .AddScoped<IRepository<MusicMst>, MusicMstRepository>()
                .AddScoped<ScrapingMusicDataService>()
                .BuildServiceProvider();

            var service = serviceProvider.GetService<ScrapingMusicDataService>();
            service.Execute();

            logger.Debug("Program.Main() END.");
        }
    }
}
