using System.Diagnostics;

namespace FaceDeviceHttpPcServer.Services;

/// <summary>
/// Windows 방화벽 규칙을 관리하는 서비스
/// </summary>
public static class FirewallHelper
{
    /// <summary>
    /// 현재 애플리케이션에 대한 방화벽 규칙이 있는지 확인
    /// </summary>
    public static bool CheckFirewallRule(string ruleName)
    {
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

            return output.Contains("규칙 이름:") || output.Contains("Rule Name:");
        }
        catch
        {
            return false;
        }
    }

    /// <summary>
    /// UDP Discovery를 위한 방화벽 규칙 추가 (관리자 권한 필요)
    /// </summary>
    public static bool AddUdpFirewallRule(string programPath, string ruleName = "FDDC UDP Discovery")
    {
        try
        {
            // 기존 규칙이 있는지 확인
            if (CheckFirewallRule(ruleName))
            {
                LogHub.Instance.Info($"방화벽 규칙이 이미 존재합니다: {ruleName}");
                return true;
            }

            // 인바운드 규칙 추가
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
                Verb = "runas" // 관리자 권한 요청
            };

            using var processIn = Process.Start(psiIn);
            if (processIn == null)
            {
                LogHub.Instance.Warn("방화벽 규칙 추가 실패: 프로세스 시작 실패");
                return false;
            }

            var outputIn = processIn.StandardOutput.ReadToEnd();
            var errorIn = processIn.StandardError.ReadToEnd();
            processIn.WaitForExit();

            if (processIn.ExitCode == 0)
            {
                LogHub.Instance.Info($"방화벽 인바운드 규칙 추가 성공: {ruleName}");
                return true;
            }
            else
            {
                LogHub.Instance.Warn($"방화벽 규칙 추가 실패: {errorIn}");
                return false;
            }
        }
        catch (Exception ex)
        {
            LogHub.Instance.Warn($"방화벽 규칙 추가 실패: {ex.Message}");
            return false;
        }
    }

    /// <summary>
    /// 방화벽 규칙 제거
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
                LogHub.Instance.Info($"방화벽 규칙 제거 성공: {ruleName}");
                return true;
            }
            else
            {
                LogHub.Instance.Warn($"방화벽 규칙 제거 실패: {error}");
                return false;
            }
        }
        catch (Exception ex)
        {
            LogHub.Instance.Warn($"방화벽 규칙 제거 실패: {ex.Message}");
            return false;
        }
    }

    /// <summary>
    /// PowerShell 명령어로 방화벽 규칙 추가 (대체 방법)
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
    /// 사용자에게 수동으로 방화벽을 설정하도록 안내하는 메시지
    /// </summary>
    public static string GetManualFirewallInstructions()
    {
        return @"UDP 브로드캐스트 검색을 위해 Windows 방화벽 설정이 필요합니다.

방법 1: PowerShell (관리자 권한)
---------------------------------------
New-NetFirewallRule -DisplayName ""FDDC UDP Discovery"" `
    -Direction Inbound `
    -Protocol UDP `
    -LocalPort 1024-65535 `
    -Action Allow

방법 2: Windows 방화벽 GUI
---------------------------------------
1. 제어판 → Windows Defender 방화벽
2. 고급 설정 → 인바운드 규칙
3. 새 규칙 → 프로그램
4. 이 프로그램 경로 선택: FaceDeviceHttpPcServer.exe
5. 연결 허용 → 완료

방법 3: netsh 명령어
---------------------------------------
netsh advfirewall firewall add rule name=""FDDC UDP Discovery"" ^
    dir=in action=allow protocol=UDP ^
    program=""C:\Path\To\FaceDeviceHttpPcServer.exe"" ^
    enable=yes";
    }
}
