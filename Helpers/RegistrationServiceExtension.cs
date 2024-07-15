﻿using CSVReaderTask.EF;
using CSVReaderTask.Helpers.Interfaces;
using CSVReaderTask.Models.ViewModels;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Configuration;
using CSVReaderTask.Helpers.Dialogs;

namespace CSVReaderTask.Helpers
{
    /// <summary>
    /// Extension methods for registering services and dependencies in the application's dependency injection container.
    /// </summary>
    internal static class RegistrationServiceExtension
    {
        private const string DatabaseConnectionPath = "DefaultConnection";
        /// <summary>
        /// Registers services, database contexts, windows, view models, and other dependencies in the application.
        /// </summary>
        internal static IServiceCollection RegisterServices(this IServiceCollection serviceCollection)
        {
            //services
            serviceCollection.AddScoped<ICSVReader, CSVReader>();
            serviceCollection.AddScoped<IUnitOfWork, UnitOfWork>();
            serviceCollection.AddScoped<IXMLPersonExport, XMLPersonExporter>();
            serviceCollection.AddScoped<IExcelExport, ExcelExporter>();
            serviceCollection.AddScoped<IInitializeOnStartService, InitializeOnStartService>();
            serviceCollection.AddScoped<IMainWindowService, MainWindowService>();
            serviceCollection.AddScoped<ISaveDialog, SaveDialog>();
            serviceCollection.AddScoped<IOpenDialog, OpenDialog>();
            serviceCollection.AddScoped<IFileDialog, FileDialog>();
            serviceCollection.AddSingleton<IMessageDialog, MessageDialog>();
            serviceCollection.AddSingleton<ILocalizationService, LocalizationService>();

            serviceCollection.AddScoped<IDBPersonSaveAsync,DBPersonSaveAsync>();

            serviceCollection.AddDbContext<CSVContext>(options =>
            {
                options.UseSqlServer(Program.Config.GetConnectionString(DatabaseConnectionPath),
                    options => options.EnableRetryOnFailure(
                        1,
                        TimeSpan.FromSeconds(1),
                        []));
             
            }, ServiceLifetime.Transient);

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
