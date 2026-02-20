using Microsoft.Extensions.DependencyInjection;

namespace DspicoThemeForms
{
    internal static class Program
    {
        /// <summary>
        ///  The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            var services = new ServiceCollection();
            services.AddSingleton<IDarkModeFactory, DarkModeFactory>();
            services.AddSingleton<Main>();

            using var serviceProvider = services.BuildServiceProvider();

            // To customize application configuration such as set high DPI settings or default font,
            // see https://aka.ms/applicationconfiguration.
            ApplicationConfiguration.Initialize();
            var mainForm = serviceProvider.GetRequiredService<Main>();
            Application.Run(mainForm);
        }
    }
}
