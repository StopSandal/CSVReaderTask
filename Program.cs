using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.DependencyInjection;
using CSVReaderTask.Helpers;

namespace CSVReaderTask
{
    public class Program
    {
        [STAThread]
        public static void Main()
        {

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
