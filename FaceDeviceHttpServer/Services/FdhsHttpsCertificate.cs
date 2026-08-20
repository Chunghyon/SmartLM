using System.Net;
using System.Net.NetworkInformation;
using System.Net.Sockets;
using System.Security.Cryptography;
using System.Security.Cryptography.X509Certificates;

namespace FaceDeviceHttpPcServer.Services;

public static class FdhsHttpsCertificate
{
    public const string PfxPassword = "SmartLM-FDHS";

    public static string CertDirectory
    {
        get
        {
            var docs = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
            if (string.IsNullOrWhiteSpace(docs))
                docs = Environment.GetFolderPath(Environment.SpecialFolder.UserProfile);
            return Path.Combine(docs, "SmartLM_Data", "certs");
        }
    }

    public static string PfxPath => Path.Combine(CertDirectory, "fdhs.pfx");
    public static string CerPath => Path.Combine(CertDirectory, "fdhs.cer");

    public static X509Certificate2 Ensure()
    {
        Directory.CreateDirectory(CertDirectory);
        var cerPath = Path.Combine(CertDirectory, "fdhs.cer");
        if (File.Exists(PfxPath))
        {
            try
            {
                return X509CertificateLoader.LoadPkcs12FromFile(
                    PfxPath, PfxPassword, X509KeyStorageFlags.Exportable);
            }
            catch
            {
                // 재발급
            }
        }

        var cert = CreateSelfSigned();
        File.WriteAllBytes(PfxPath, cert.Export(X509ContentType.Pfx, PfxPassword));
        File.WriteAllBytes(cerPath, cert.Export(X509ContentType.Cert));
        return cert;
    }

    static X509Certificate2 CreateSelfSigned()
    {
        using var rsa = RSA.Create(2048);
        var req = new CertificateRequest(
            "CN=SmartLM FDHS",
            rsa,
            HashAlgorithmName.SHA256,
            RSASignaturePadding.Pkcs1);

        req.CertificateExtensions.Add(new X509BasicConstraintsExtension(false, false, 0, false));
        req.CertificateExtensions.Add(new X509KeyUsageExtension(
            X509KeyUsageFlags.DigitalSignature | X509KeyUsageFlags.KeyEncipherment, true));
        req.CertificateExtensions.Add(new X509EnhancedKeyUsageExtension(
            new OidCollection { new Oid("1.3.6.1.5.5.7.3.1") }, true));

        var san = new SubjectAlternativeNameBuilder();
        san.AddDnsName("localhost");
        san.AddIpAddress(IPAddress.Loopback);
        san.AddIpAddress(IPAddress.IPv6Loopback);
        try
        {
            var host = Dns.GetHostName();
            if (!string.IsNullOrWhiteSpace(host))
                san.AddDnsName(host);
        }
        catch { }

        foreach (var ip in LocalIPv4())
            san.AddIpAddress(ip);
        req.CertificateExtensions.Add(san.Build());

        var created = req.CreateSelfSigned(DateTimeOffset.UtcNow.AddDays(-1), DateTimeOffset.UtcNow.AddYears(10));
        return X509CertificateLoader.LoadPkcs12(
            created.Export(X509ContentType.Pfx, PfxPassword),
            PfxPassword,
            X509KeyStorageFlags.Exportable | X509KeyStorageFlags.PersistKeySet);
    }

    static IEnumerable<IPAddress> LocalIPv4()
    {
        var list = new List<IPAddress>();
        try
        {
            foreach (var nic in NetworkInterface.GetAllNetworkInterfaces())
            {
                if (nic.OperationalStatus != OperationalStatus.Up) continue;
                if (nic.NetworkInterfaceType == NetworkInterfaceType.Loopback) continue;
                foreach (var ua in nic.GetIPProperties().UnicastAddresses)
                {
                    if (ua.Address.AddressFamily == AddressFamily.InterNetwork)
                        list.Add(ua.Address);
                }
            }
        }
        catch { }
        return list;
    }
}
