using System;
using System.Diagnostics;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using Xunit;

public class E2ETests
{
    [Fact(Timeout = 180000)]
    public void RunProgram_CI_Mode_Passes()
    {
        // Resolve absolute path to the Factorial project from the test assembly directory
        string projectPath = Path.GetFullPath(Path.Combine(AppContext.BaseDirectory, "..", "..", "..", "..", "Factorial.csproj"));
        var psi = new ProcessStartInfo
        {
            FileName = "dotnet",
            Arguments = $"run --project \"{projectPath}\" -- --ci",
            RedirectStandardOutput = true,
            RedirectStandardError = true,
            UseShellExecute = false,
            CreateNoWindow = true
        };

        using var proc = Process.Start(psi);
        Assert.NotNull(proc);

        string output = proc.StandardOutput.ReadToEnd();
        string err = proc.StandardError.ReadToEnd();
        proc.WaitForExit(120000);

        Assert.Equal(0, proc.ExitCode);
        Assert.Contains("ALL TESTS PASSED", output);
        // If it failed, include output for debugging
        if (proc.ExitCode != 0)
        {
            throw new Exception($"Process failed with exit {proc.ExitCode}\nSTDOUT:\n{output}\nSTDERR:\n{err}");
        }
    }
}
