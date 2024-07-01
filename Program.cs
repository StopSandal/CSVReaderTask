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
        public static void Main()
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
                try
                {
                    using var context = scope.ServiceProvider.GetRequiredService<CSVContext>();
                    context.Database.Migrate();
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Error occurred while processing database. Contact with administrator. Message: {ex.Message}","StartUp Error", MessageBoxButtons.OK,MessageBoxIcon.Error);
                    return;
                }
            }
            var app = host.Services.GetService<App>();

 
            app?.Run();
        }
    }
}
