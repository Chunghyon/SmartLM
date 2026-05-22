Add-Type -TypeDefinition @"
using System;
using System.Runtime.InteropServices;
using System.Text;
public static class U32 {
  public delegate bool EnumWndProc(IntPtr hWnd, IntPtr lParam);
  [DllImport("user32.dll")] public static extern bool EnumWindows(EnumWndProc lpEnumFunc, IntPtr lParam);
  [DllImport("user32.dll")] public static extern bool EnumChildWindows(IntPtr hWndParent, EnumWndProc lpEnumFunc, IntPtr lParam);
  [DllImport("user32.dll", CharSet=CharSet.Unicode)] public static extern int GetWindowText(IntPtr hWnd, StringBuilder lpString, int nMaxCount);
  [DllImport("user32.dll", CharSet=CharSet.Unicode)] public static extern int GetClassName(IntPtr hWnd, StringBuilder lpClassName, int nMaxCount);
  [DllImport("user32.dll")] public static extern bool IsWindowVisible(IntPtr hWnd);
  [DllImport("user32.dll")] public static extern bool GetWindowRect(IntPtr hWnd, out RECT rect);
  [DllImport("user32.dll")] public static extern bool SetForegroundWindow(IntPtr hWnd);
  [DllImport("user32.dll")] public static extern bool SetCursorPos(int X, int Y);
  [DllImport("user32.dll")] public static extern void mouse_event(uint dwFlags, uint dx, uint dy, uint dwData, UIntPtr dwExtraInfo);
  public const uint MOUSEEVENTF_LEFTDOWN = 0x0002;
  public const uint MOUSEEVENTF_LEFTUP = 0x0004;
  [StructLayout(LayoutKind.Sequential)]
  public struct RECT { public int Left; public int Top; public int Right; public int Bottom; }
}
"@

$lastClick = Get-Date "2000-01-01"
while ($true) {
  $targets = New-Object System.Collections.ArrayList
  [U32]::EnumWindows({
    param($h,$p)
    if (-not [U32]::IsWindowVisible($h)) { return $true }
    $t = New-Object System.Text.StringBuilder 512
    $c = New-Object System.Text.StringBuilder 128
    [U32]::GetWindowText($h,$t,512) | Out-Null
    [U32]::GetClassName($h,$c,128) | Out-Null
    if ($c.ToString() -eq 'ThunderRT6FormDC' -and $t.ToString().StartsWith('Pixel:')) {
      [void]$targets.Add($h)
    }
    return $true
  }, [IntPtr]::Zero) | Out-Null

  foreach ($w in $targets) {
    $buttons = New-Object System.Collections.ArrayList
    [U32]::EnumChildWindows($w, {
      param($ch,$p)
      if (-not [U32]::IsWindowVisible($ch)) { return $true }
      $cc = New-Object System.Text.StringBuilder 128
      [U32]::GetClassName($ch,$cc,128) | Out-Null
      if ($cc.ToString() -eq 'ThunderRT6UserControl') {
        $r = New-Object U32+RECT
        [U32]::GetWindowRect($ch,[ref]$r) | Out-Null
        $wth = $r.Right - $r.Left
        $hgt = $r.Bottom - $r.Top
        if ($wth -ge 70 -and $wth -le 120 -and $hgt -ge 20 -and $hgt -le 40) {
          [void]$buttons.Add([PSCustomObject]@{ Left=$r.Left; Top=$r.Top; Right=$r.Right; Bottom=$r.Bottom })
        }
      }
      return $true
    }, [IntPtr]::Zero) | Out-Null

    if ($buttons.Count -ge 2) {
      $rightBtn = $buttons | Sort-Object Right | Select-Object -Last 1
      $now = Get-Date
      if (($now - $lastClick).TotalMilliseconds -gt 800) {
        [U32]::SetForegroundWindow($w) | Out-Null
        $x = [int](($rightBtn.Left + $rightBtn.Right) / 2)
        $y = [int](($rightBtn.Top + $rightBtn.Bottom) / 2)
        [U32]::SetCursorPos($x,$y) | Out-Null
        Start-Sleep -Milliseconds 50
        [U32]::mouse_event([U32]::MOUSEEVENTF_LEFTDOWN,0,0,0,[UIntPtr]::Zero)
        Start-Sleep -Milliseconds 30
        [U32]::mouse_event([U32]::MOUSEEVENTF_LEFTUP,0,0,0,[UIntPtr]::Zero)
        $lastClick = $now
      }
    }
  }

  Start-Sleep -Milliseconds 150
}
