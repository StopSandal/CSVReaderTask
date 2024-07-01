using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.DependencyInjection;
using CSVReaderTask.Helpers;
using Microsoft.Extensions.Configuration;
using System.CodeDom;
using System.IO;
using System.Windows.Forms;
using CSVReaderTask.EF;
using Microsoft.EntityFrameworkCore;

namespace CSVReaderTask
{
    public class Program
    {
        private const string SettingsFilePath = "appsettings.json";
        public static IConfiguration Config { get; private set; }
        [STAThread]
        public async static void Main()
        {
            Config = new ConfigurationBuilder()
                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                .Build();

            var host = Host.CreateDefaultBuilder()
                .ConfigureServices(services =>
                {
                    services.RegisterServices();
                })
                .Build();
            using (var scope = host.Services.CreateScope())
            {
                using var context = scope.ServiceProvider.GetRequiredService<CSVContext>();
                await context.Database.MigrateAsync();
            }
            var app = host.Services.GetService<App>();

 
            app?.Run();
        }
    }
}
