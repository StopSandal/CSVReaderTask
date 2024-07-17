using CSVReaderTask.Helpers;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

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
