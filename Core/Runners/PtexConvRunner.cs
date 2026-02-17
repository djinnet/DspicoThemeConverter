using DspicoThemeForms.Core.Helper;
using System.Diagnostics;

namespace DspicoThemeForms.Core.Runners;

public sealed class PtexConvRunner
{
    private string PtexConvPath { get; }
    private Action<string> Log { get; }

    public PtexConvRunner(string ptexConvPath, Action<string> log)
    {
        if (!File.Exists(ptexConvPath))
            throw new FileNotFoundException("ptexconv executable not found at the specified path.", ptexConvPath);

        PtexConvPath = ptexConvPath;
        Log = log;
    }

    public bool Run(string arguments)
    {
        bool HasError = false;
        try
        {
            Log("Running ptexconv with arguments: " + arguments);
            var psi = new ProcessStartInfo
            {
                FileName = "cmd.exe",
                WorkingDirectory = PathHelper.GetToolsDirectory(),
                Arguments = $"/c {PtexConvPath} {arguments}",
                RedirectStandardOutput = true,
                RedirectStandardError = true,
                UseShellExecute = false,
                CreateNoWindow = true
            };

            using var proc = new Process { StartInfo = psi };
            proc.OutputDataReceived += (s, e) =>
            {
                if (!string.IsNullOrEmpty(e.Data))
                {
                    Log(e.Data);
                }
            };

            proc.ErrorDataReceived += (s, e) =>
            {
                if (!string.IsNullOrEmpty(e.Data))
                {
                    Log("ERROR:" + e.Data);
                    HasError = true;
                }
            };

            proc.Start();
            proc.BeginOutputReadLine();
            proc.BeginErrorReadLine();

            proc.WaitForExit();

            if (proc.ExitCode != 0)
            {
                throw new Exception($"ptexconv failed with exit code {proc.ExitCode}");
            }

            return !HasError;
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error running ptexconv: {ex.Message}");
            return false;
        }
    }
}

