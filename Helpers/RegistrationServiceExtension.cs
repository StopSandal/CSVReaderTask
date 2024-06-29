using CSVReaderTask.EF;
using CSVReaderTask.Helpers.Interfaces;
using CSVReaderTask.Models.ViewModels;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CSVReaderTask.Helpers
{
    internal static class RegistrationServiceExtension
    {
        const string CONNECTION_STRING = "Server=DESKTOP-UAUG3OJ;Database=CSVApp;Trusted_Connection=True;TrustServerCertificate=True;";
        internal static IServiceCollection RegistrateServices(this IServiceCollection serviceCollection)
        {
            //services
            serviceCollection.AddScoped<ICSVReader, CSVReader>();
            serviceCollection.AddScoped<IUnitOfWork, UnitOfWork>();
            serviceCollection.AddScoped<IXMLPersonExport, XMLPersonExporter>();
            serviceCollection.AddScoped<IExcelExport, ExcelExporter>();
            serviceCollection.AddScoped<IMainWindowService, MainWindowService>();
            serviceCollection.AddDbContext<CSVContext>(options =>
            {
                options.UseSqlServer(CONNECTION_STRING);
            });

            //windows
            serviceCollection.AddSingleton<MainWindow>();

            //VM
            serviceCollection.AddSingleton<FilterVM>();

            //apps
            serviceCollection.AddSingleton<App>();

            return serviceCollection;
        }
    }
}
