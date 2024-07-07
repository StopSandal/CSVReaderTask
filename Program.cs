using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.DependencyInjection;
using CSVReaderTask.Helpers;
using Microsoft.Extensions.Configuration;
using CSVReaderTask.EF;
using Microsoft.EntityFrameworkCore;
using CSVReaderTask.Helpers.Dialogs;
using CSVReaderTask.Helpers.Interfaces;
using Microsoft.Data.SqlClient;

namespace CSVReaderTask
{
    public class Program
    {
        private const string SettingsFilePath = "appsettings.json";
        public static IConfiguration Config { get; private set; }
        [STAThread]
        public static void Main()
        {
            Config = new ConfigurationBuilder()
                .AddJsonFile(SettingsFilePath, optional: false, reloadOnChange: true)
                .Build();

            var host = Host.CreateDefaultBuilder()
                .ConfigureServices(services =>
                {
                    services.RegisterServices();
                })
                .Build();

            var app = host.Services.GetService<App>();

 
            app?.Run();
        }
    }
}
