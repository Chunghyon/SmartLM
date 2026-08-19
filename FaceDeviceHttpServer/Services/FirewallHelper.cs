using System.Diagnostics;

namespace FaceDeviceHttpPcServer.Services;

/// <summary>
/// Windows ��ȭ�� ��Ģ�� �����ϴ� ����
/// </summary>
public static class FirewallHelper
{
    /// <summary>
    /// ���� ���ø����̼ǿ� ���� ��ȭ�� ��Ģ�� �ִ��� Ȯ��
    /// </summary>
    public static bool CheckFirewallRule(string ruleName)
    {
        if (!OperatingSystem.IsWindows()) return true;
        try
        {
            var psi = new ProcessStartInfo
            {
                FileName = "netsh",
                Arguments = $"advfirewall firewall show rule name=\"{ruleName}\"",
                RedirectStandardOutput = true,
                UseShellExecute = false,
                CreateNoWindow = true
            };

            using var process = Process.Start(psi);
            if (process == null) return false;

            var output = process.StandardOutput.ReadToEnd();
            process.WaitForExit();

            return output.Contains("��Ģ �̸�:") || output.Contains("Rule Name:");
        }
        catch
        {
            return false;
        }
    }

    /// <summary>
    /// UDP Discovery�� ���� ��ȭ�� ��Ģ �߰� (������ ���� �ʿ�)
    /// </summary>
    public static bool AddUdpFirewallRule(string programPath, string ruleName = "FDDC UDP Discovery")
    {
        if (!OperatingSystem.IsWindows()) return true;
        try
        {
            // ���� ��Ģ�� �ִ��� Ȯ��
            if (CheckFirewallRule(ruleName))
            {
                LogHub.Instance.Info($"��ȭ�� ��Ģ�� �̹� �����մϴ�: {ruleName}");
                return true;
            }

            // �ιٿ�� ��Ģ �߰�
            var psiIn = new ProcessStartInfo
            {
                FileName = "netsh",
                Arguments = $"advfirewall firewall add rule name=\"{ruleName}\" " +
                           $"dir=in action=allow protocol=UDP " +
                           $"program=\"{programPath}\" " +
                           "enable=yes",
                RedirectStandardOutput = true,
                RedirectStandardError = true,
                UseShellExecute = false,
                CreateNoWindow = true,
                Verb = "runas" // ������ ���� ��û
            };

            using var processIn = Process.Start(psiIn);
            if (processIn == null)
            {
                LogHub.Instance.Warn("��ȭ�� ��Ģ �߰� ����: ���μ��� ���� ����");
                return false;
            }

            var outputIn = processIn.StandardOutput.ReadToEnd();
            var errorIn = processIn.StandardError.ReadToEnd();
            processIn.WaitForExit();

            if (processIn.ExitCode == 0)
            {
                LogHub.Instance.Info($"��ȭ�� �ιٿ�� ��Ģ �߰� ����: {ruleName}");
                return true;
            }
            else
            {
                LogHub.Instance.Warn($"��ȭ�� ��Ģ �߰� ����: {errorIn}");
                return false;
            }
        }
        catch (Exception ex)
        {
            LogHub.Instance.Warn($"��ȭ�� ��Ģ �߰� ����: {ex.Message}");
            return false;
        }
    }

    /// <summary>
    /// ��ȭ�� ��Ģ ����
    /// </summary>
    public static bool RemoveFirewallRule(string ruleName)
    {
        try
        {
            var psi = new ProcessStartInfo
            {
                FileName = "netsh",
                Arguments = $"advfirewall firewall delete rule name=\"{ruleName}\"",
                RedirectStandardOutput = true,
                RedirectStandardError = true,
                UseShellExecute = false,
                CreateNoWindow = true,
                Verb = "runas"
            };

            using var process = Process.Start(psi);
            if (process == null) return false;

            var output = process.StandardOutput.ReadToEnd();
            var error = process.StandardError.ReadToEnd();
            process.WaitForExit();

            if (process.ExitCode == 0)
            {
                LogHub.Instance.Info($"��ȭ�� ��Ģ ���� ����: {ruleName}");
                return true;
            }
            else
            {
                LogHub.Instance.Warn($"��ȭ�� ��Ģ ���� ����: {error}");
                return false;
            }
        }
        catch (Exception ex)
        {
            LogHub.Instance.Warn($"��ȭ�� ��Ģ ���� ����: {ex.Message}");
            return false;
        }
    }

    /// <summary>
    /// PowerShell ���ɾ�� ��ȭ�� ��Ģ �߰� (��ü ���)
    /// </summary>
    public static string GetFirewallAddCommand(string programPath)
    {
        return $@"New-NetFirewallRule -DisplayName ""FDDC UDP Discovery"" `
    -Direction Inbound `
    -Protocol UDP `
    -Program ""{programPath}"" `
    -Action Allow";
    }

    /// <summary>
    /// ����ڿ��� �������� ��ȭ���� �����ϵ��� �ȳ��ϴ� �޽���
    /// </summary>
    public static string GetManualFirewallInstructions()
    {
        return @"UDP ��ε�ĳ��Ʈ �˻��� ���� Windows ��ȭ�� ������ �ʿ��մϴ�.

��� 1: PowerShell (������ ����)
---------------------------------------
New-NetFirewallRule -DisplayName ""FDDC UDP Discovery"" `
    -Direction Inbound `
    -Protocol UDP `
    -LocalPort 1024-65535 `
    -Action Allow

��� 2: Windows ��ȭ�� GUI
---------------------------------------
1. ������ �� Windows Defender ��ȭ��
2. ���� ���� �� �ιٿ�� ��Ģ
3. �� ��Ģ �� ���α׷�
4. �� ���α׷� ��� ����: FaceDeviceHttpPcServer.exe
5. ���� ��� �� �Ϸ�

��� 3: netsh ���ɾ�
---------------------------------------
netsh advfirewall firewall add rule name=""FDDC UDP Discovery"" ^
    dir=in action=allow protocol=UDP ^
    program=""C:\Path\To\FaceDeviceHttpPcServer.exe"" ^
    enable=yes";
    }
}
