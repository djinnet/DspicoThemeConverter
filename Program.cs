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
            ServiceCollection services = new();
            services.AddSingleton<IDarkModeFactory, DarkModeFactory>();
            services.AddSingleton<Main>();

            using ServiceProvider serviceProvider = services.BuildServiceProvider();



            // To customize application configuration such as set high DPI settings or default font,
            // see https://aka.ms/applicationconfiguration.
            ApplicationConfiguration.Initialize();
            Main mainForm = serviceProvider.GetRequiredService<Main>();

            Application.Run(mainForm);
        }
    }
}