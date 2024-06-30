using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.DependencyInjection;
using System;
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
                    services.RegistrateServices();
                })
                .Build();

            var app = host.Services.GetService<App>();

            app?.Run();
        }
    }
}
