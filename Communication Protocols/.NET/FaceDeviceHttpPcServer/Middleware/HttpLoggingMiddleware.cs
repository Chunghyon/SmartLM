using FaceDeviceHttpPcServer.Services;
using System.Text;

namespace FaceDeviceHttpPcServer.Middleware;

/// <summary>
/// 모든 HTTP 요청/응답 본문을 캡처하여 LogHub에 전달하는 미들웨어.
/// </summary>
public sealed class HttpLoggingMiddleware(RequestDelegate next)
{
    private static readonly HashSet<string> TextContentTypes =
    [
        "application/json",
        "text/plain",
        "text/html",
        "application/x-www-form-urlencoded"
    ];

    public async Task InvokeAsync(HttpContext context)
    {
        // ── 요청 본문 읽기 ──────────────────────────────────────────
        context.Request.EnableBuffering();
        string? requestBody = null;

        if (context.Request.ContentLength > 0 && IsTextContent(context.Request.ContentType))
        {
            using var reader = new StreamReader(
                context.Request.Body, Encoding.UTF8,
                detectEncodingFromByteOrderMarks: false,
                bufferSize: 4096, leaveOpen: true);

            requestBody = await reader.ReadToEndAsync();
            context.Request.Body.Position = 0;
        }

        // ── 응답 본문 캡처 ──────────────────────────────────────────
        var originalBody = context.Response.Body;
        await using var buffer = new MemoryStream();
        context.Response.Body = buffer;

        try
        {
            await next(context);
        }
        finally
        {
            buffer.Position = 0;
            string? responseBody = null;

            if (IsTextContent(context.Response.ContentType) && buffer.Length > 0)
            {
                using var reader = new StreamReader(buffer, Encoding.UTF8, leaveOpen: true);
                responseBody = await reader.ReadToEndAsync();
                buffer.Position = 0;
            }

            await buffer.CopyToAsync(originalBody);
            context.Response.Body = originalBody;

            // ── 로그 발행 ───────────────────────────────────────────
            var method  = context.Request.Method;
            var path    = context.Request.Path + context.Request.QueryString;
            var status  = context.Response.StatusCode;

            // 요청 상세 (본문 or form 필드 요약)
            var detail = BuildDetail(context, requestBody, responseBody);

            LogHub.Instance.Request(method, path, status, detail);
        }
    }

    // ── helpers ────────────────────────────────────────────────────────────

    private static string? BuildDetail(HttpContext ctx, string? reqBody, string? respBody)
    {
        var sb = new StringBuilder();

        if (!string.IsNullOrWhiteSpace(reqBody))
        {
            sb.AppendLine("▼ Request Body");
            sb.AppendLine(Truncate(reqBody, 800));
        }
        else if (ctx.Request.HasFormContentType)
        {
            sb.AppendLine("▼ Form Fields");
            foreach (var kv in ctx.Request.Form)
                sb.AppendLine($"  {kv.Key} = {Truncate(kv.Value.ToString(), 120)}");
            if (ctx.Request.Form.Files.Count > 0)
                foreach (var f in ctx.Request.Form.Files)
                    sb.AppendLine($"  [FILE] {f.Name}  {f.FileName}  {f.Length:N0} bytes");
        }

        if (!string.IsNullOrWhiteSpace(respBody))
        {
            sb.AppendLine("▼ Response Body");
            sb.AppendLine(Truncate(respBody, 800));
        }

        return sb.Length > 0 ? sb.ToString().TrimEnd() : null;
    }

    private static bool IsTextContent(string? contentType)
    {
        if (string.IsNullOrWhiteSpace(contentType)) return false;
        foreach (var t in TextContentTypes)
            if (contentType.Contains(t, StringComparison.OrdinalIgnoreCase))
                return true;
        return false;
    }

    private static string Truncate(string s, int max) =>
        s.Length <= max ? s : s[..max] + $" … (+{s.Length - max} chars)";
}
