namespace FaceDeviceHttpPcServer.Services;

public sealed class DeviceCommandJob
{
    public string Id { get; set; } = Guid.NewGuid().ToString("N");
    public string DeviceSn { get; set; } = string.Empty;
    public string Type { get; set; } = string.Empty;
    public string Status { get; set; } = "Pending"; // Pending, Success, Failed
    public string? Message { get; set; }
    public DateTime CreatedAtUtc { get; set; } = DateTime.UtcNow;
    public DateTime? CompletedAtUtc { get; set; }
}

public sealed class DeviceCommandTracker
{
    private readonly object _sync = new();
    private readonly Dictionary<string, DeviceCommandJob> _jobs = new();

    public DeviceCommandJob Start(string deviceSn, string type, string? message = null)
    {
        var job = new DeviceCommandJob
        {
            DeviceSn = deviceSn,
            Type = type,
            Message = message ?? "단말기 Keepalive 대기 중"
        };
        lock (_sync)
            _jobs[job.Id] = job;
        return job;
    }

    public DeviceCommandJob? Get(string id)
    {
        lock (_sync)
            return _jobs.TryGetValue(id, out var job) ? Clone(job) : null;
    }

    public void CompleteLatest(string deviceSn, string type, bool success, string message)
    {
        lock (_sync)
        {
            var job = _jobs.Values
                .Where(j => j.DeviceSn == deviceSn && j.Type == type && j.Status == "Pending")
                .OrderByDescending(j => j.CreatedAtUtc)
                .FirstOrDefault();
            if (job == null)
                return;
            job.Status = success ? "Success" : "Failed";
            job.Message = message;
            job.CompletedAtUtc = DateTime.UtcNow;
        }
    }

    private static DeviceCommandJob Clone(DeviceCommandJob j) => new()
    {
        Id = j.Id,
        DeviceSn = j.DeviceSn,
        Type = j.Type,
        Status = j.Status,
        Message = j.Message,
        CreatedAtUtc = j.CreatedAtUtc,
        CompletedAtUtc = j.CompletedAtUtc
    };
}
