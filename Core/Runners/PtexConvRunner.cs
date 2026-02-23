using DspicoThemeForms.Core.Constants;
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
            if (string.IsNullOrEmpty(arguments))
            {
                Log("No arguments provided for ptexconv.");
                return false;
            }

            string toolsDir = PathHelper.GetToolsDirectory();
            if (string.IsNullOrEmpty(toolsDir))
            {
                Log("Tools directory not found.");
                return false;
            }

            Log("Running ptexconv with arguments: " + arguments);
            ProcessStartInfo psi = new()
            {
                FileName = FilesContants.CmdExeName,
                WorkingDirectory = toolsDir,
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
            Log("Exception while running ptexconv: " + ex.Message);
            return false;
        }
    }
}

