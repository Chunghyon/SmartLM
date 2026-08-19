namespace FaceDeviceHttpPcServer.Services;

public sealed class RecordRetentionCleanupService : BackgroundService
{
    private readonly SystemSettingsStore _settings;
    private readonly MySqlStateStore _store;
    private readonly ILogger<RecordRetentionCleanupService> _logger;

    public RecordRetentionCleanupService(
        SystemSettingsStore settings,
        MySqlStateStore store,
        ILogger<RecordRetentionCleanupService> logger)
    {
        _settings = settings;
        _store = store;
        _logger = logger;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        await Task.Delay(TimeSpan.FromSeconds(15), stoppingToken);
        while (!stoppingToken.IsCancellationRequested)
        {
            try
            {
                RunOnce();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "출입기록 보관기간 정리 실패");
            }

            await Task.Delay(TimeSpan.FromHours(1), stoppingToken);
        }
    }

    private void RunOnce()
    {
        var months = _settings.Get().RecordRetentionMonths;
        if (months <= 0)
        {
            _logger.LogInformation("출입기록 자동 삭제 비활성 (0개월)");
            return;
        }

        var cutoff = DateTime.UtcNow.AddMonths(-months);
        var deleted = _store.DeleteRecordsOlderThan(cutoff);
        if (deleted > 0)
            LogHub.Instance.Info($"[Retention] {months}개월 이전 출입기록 {deleted}건 삭제 (기준 {cutoff:yyyy-MM-dd})");
    }
}
