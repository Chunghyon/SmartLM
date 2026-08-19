using System.Net.Http.Json;

namespace FaceDeviceDesktopClient.Services;

public static class DeviceCommandWaiter
{
    public static async Task<(bool ok, string message)> WaitAsync(
        HttpClient http, IEnumerable<string> jobIds, TimeSpan timeout, CancellationToken ct = default)
    {
        var ids = jobIds.Where(id => !string.IsNullOrWhiteSpace(id)).Distinct().ToList();
        if (ids.Count == 0)
            return (true, "대기할 명령이 없습니다.");

        var deadline = DateTime.UtcNow + timeout;
        var last = "단말기 응답 대기 중...";

        while (DateTime.UtcNow < deadline)
        {
            ct.ThrowIfCancellationRequested();
            var pending = 0;
            var failed = new List<string>();
            var succeeded = new List<string>();

            foreach (var id in ids)
            {
                var job = await http.GetFromJsonAsync<CommandJobDto>($"/admin/command-jobs/{id}", ct);
                if (job == null)
                {
                    pending++;
                    continue;
                }

                last = job.Message ?? last;
                if (string.Equals(job.Status, "Pending", StringComparison.OrdinalIgnoreCase))
                    pending++;
                else if (string.Equals(job.Status, "Failed", StringComparison.OrdinalIgnoreCase))
                    failed.Add(job.Message ?? "실패");
                else
                    succeeded.Add(job.Message ?? "성공");
            }

            if (pending == 0)
            {
                if (failed.Count > 0)
                    return (false, string.Join("\n", failed));
                return (true, succeeded.Count > 0 ? string.Join("\n", succeeded) : "명령이 완료되었습니다.");
            }

            await Task.Delay(500, ct);
        }

        return (false, "단말기 응답 대기 시간이 초과되었습니다.\n" + last);
    }

    private sealed class CommandJobDto
    {
        public string? Id { get; set; }
        public string? Status { get; set; }
        public string? Message { get; set; }
    }
}
