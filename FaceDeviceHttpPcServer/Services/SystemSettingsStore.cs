using System.Net;
using System.Net.NetworkInformation;
using System.Net.Sockets;
using System.Xml.Serialization;

namespace FaceDeviceHttpPcServer.Services;

[XmlRoot("FaceDeviceSettings")]
public sealed class SystemSettings
{
    public string ServerUrl { get; set; } = "http://localhost:8100";
    public int RecordRetentionMonths { get; set; } = 12;
}

public sealed class SystemSettingsStore
{
    private static readonly XmlSerializer Serializer = new(typeof(SystemSettings));
    private readonly object _sync = new();
    private readonly string _path;
    private SystemSettings _current;

    public string SettingsFilePath => _path;

    public SystemSettingsStore(string settingsFilePath, IConfiguration config)
    {
        _path = string.IsNullOrWhiteSpace(settingsFilePath)
            ? Path.Combine("App_Data", "FaceDeviceSettings.xml")
            : settingsFilePath;
        var dir = Path.GetDirectoryName(_path);
        if (!string.IsNullOrWhiteSpace(dir))
            Directory.CreateDirectory(dir);

        _current = Load(config);
        Persist();
    }

    public SystemSettings Get()
    {
        lock (_sync)
        {
            return new SystemSettings
            {
                ServerUrl = _current.ServerUrl,
                RecordRetentionMonths = _current.RecordRetentionMonths
            };
        }
    }

    public SystemSettings Save(int? months = null, string? serverUrl = null)
    {
        lock (_sync)
        {
            if (months.HasValue)
            {
                var m = months.Value;
                if (m < 0) m = 0;
                if (m > 120) m = 120;
                _current.RecordRetentionMonths = m;
            }

            if (!string.IsNullOrWhiteSpace(serverUrl))
                _current.ServerUrl = NormalizeUrl(serverUrl);

            Persist();
            return Get();
        }
    }

    public IReadOnlyList<string> GetLocalServerUrls()
    {
        var port = GetPort(_current.ServerUrl);
        var hosts = new List<string> { "localhost", "127.0.0.1" };
        try
        {
            foreach (var nic in NetworkInterface.GetAllNetworkInterfaces())
            {
                if (nic.OperationalStatus != OperationalStatus.Up) continue;
                if (nic.NetworkInterfaceType is NetworkInterfaceType.Loopback) continue;
                foreach (var ua in nic.GetIPProperties().UnicastAddresses)
                {
                    if (ua.Address.AddressFamily != AddressFamily.InterNetwork) continue;
                    if (IPAddress.IsLoopback(ua.Address)) continue;
                    hosts.Add(ua.Address.ToString());
                }
            }
        }
        catch { }

        return hosts.Distinct(StringComparer.OrdinalIgnoreCase)
            .Select(h => $"http://{h}:{port}")
            .ToList();
    }

    private SystemSettings Load(IConfiguration config)
    {
        try
        {
            if (File.Exists(_path))
            {
                using var fs = File.OpenRead(_path);
                if (Serializer.Deserialize(fs) is SystemSettings loaded)
                {
                    if (string.IsNullOrWhiteSpace(loaded.ServerUrl))
                        loaded.ServerUrl = "http://localhost:8100";
                    return loaded;
                }
            }
        }
        catch { }

        return new SystemSettings
        {
            ServerUrl = "http://localhost:8100",
            RecordRetentionMonths = config.GetValue("RecordRetentionMonths", 12)
        };
    }

    private void Persist()
    {
        var tmp = _path + ".tmp";
        using (var fs = File.Create(tmp))
            Serializer.Serialize(fs, _current);
        File.Copy(tmp, _path, overwrite: true);
        File.Delete(tmp);
    }

    private static string NormalizeUrl(string url)
    {
        url = url.Trim();
        if (!url.StartsWith("http://", StringComparison.OrdinalIgnoreCase) &&
            !url.StartsWith("https://", StringComparison.OrdinalIgnoreCase))
            url = "http://" + url;
        return url.TrimEnd('/');
    }

    private static int GetPort(string? url)
    {
        if (Uri.TryCreate(url, UriKind.Absolute, out var uri) && uri.Port > 0)
            return uri.Port;
        return 8100;
    }
}
