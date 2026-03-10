using Serilog;

namespace DspicoThemeForms.Core.Logging;

public static class LoggerSetup
{
    public static void Initialize(RichTextBox logbox)
    {
        Log.Logger = new LoggerConfiguration()
            .MinimumLevel.Verbose()
            .WriteTo.RichTextBox(logbox, outputTemplate: "[{Timestamp:HH:mm:ss} {Level:u5}] {Message:lj}{NewLine}{Exception}")
            .CreateLogger();
    }
}
