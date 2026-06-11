namespace FaceDeviceHttpPcServer.Services;

/// <summary>
/// 싱글턴 이벤트 브로커. HTTP 미들웨어 → WinForms UI 로그 창으로 메시지를 전달합니다.
/// </summary>
public sealed class LogHub
{
    public static readonly LogHub Instance = new();

    private LogHub() { }

    /// <summary>새 로그 줄이 생성될 때 발생합니다. UI 스레드에서 구독하세요.</summary>
    public event Action<LogEntry>? EntryAdded;

    public void Post(LogEntry entry) => EntryAdded?.Invoke(entry);

    public void Info(string message, string? detail = null) =>
        Post(new LogEntry(LogLevel.Info, message, detail));

    public void Warn(string message, string? detail = null) =>
        Post(new LogEntry(LogLevel.Warn, message, detail));

    public void Error(string message, string? detail = null) =>
        Post(new LogEntry(LogLevel.Error, message, detail));

    public void Request(string method, string path, int status, string? body = null) =>
        Post(new LogEntry(LogLevel.Request, $"{method} {path}  →  {status}", body));
}

public enum LogLevel { Info, Warn, Error, Request }

public sealed record LogEntry(
    LogLevel Level,
    string Message,
    string? Detail = null,
    DateTimeOffset Timestamp = default)
{
    public DateTimeOffset Timestamp { get; init; } =
        Timestamp == default ? DateTimeOffset.Now : Timestamp;
}
