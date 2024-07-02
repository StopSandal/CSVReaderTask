using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.DependencyInjection;
using CSVReaderTask.Helpers;
using Microsoft.Extensions.Configuration;
using CSVReaderTask.EF;
using Microsoft.EntityFrameworkCore;
using CSVReaderTask.Helpers.Dialogs;

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
            using (var scope = host.Services.CreateScope())
            {
                try
                {
                    using var context = scope.ServiceProvider.GetRequiredService<CSVContext>();
                    context.Database.Migrate();
                }
                catch (Exception ex)
                {
                    var messageDialog = scope.ServiceProvider.GetRequiredService<MessageDialog>();
                    messageDialog.ShowError($"Unable to start app.\n Error occurred while processing database. Contact with administrator. Message: {ex.Message}");
                    return;
                }
            }
            var app = host.Services.GetService<App>();

 
            app?.Run();
        }
    }
}
