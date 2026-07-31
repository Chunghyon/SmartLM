using System.Security.Cryptography;
using System.Text;

namespace FaceDeviceDesktopClient;

/// <summary>
/// 단말기 웹 UI에 인증하여 사용자 사진을 가져오는 서비스.
/// 로그인 알고리즘: MD5( hash + password + hash ).toUpperCase()
/// </summary>
public static class DevicePhotoService
{
    private static readonly HttpClient _httpClient = new HttpClient { Timeout = TimeSpan.FromSeconds(10) };

    /// <summary>
    /// 지정된 단말기 IP 주소들에서 userId에 해당하는 사진 바이트를 가져옵니다.
    /// 여러 단말기를 순서대로 시도하여 처음으로 성공한 결과를 반환합니다.
    /// </summary>
    /// <param name="deviceIpAddresses">시도할 단말기 IP 주소 목록</param>
    /// <param name="userId">사용자 ID</param>
    /// <param name="password">단말기 관리자 비밀번호 (기본값: "0000")</param>
    /// <returns>JPEG 이미지 바이트 또는 null</returns>
    public static async Task<byte[]?> FetchUserPhotoAsync(
        IEnumerable<string> deviceIpAddresses,
        string userId,
        string password = "0000")
    {
        foreach (var ip in deviceIpAddresses)
        {
            try
            {
                var baseUrl = $"http://{ip}:80";
                var photoBytes = await FetchUserPhotoFromDeviceAsync(baseUrl, userId, password);
                if (photoBytes != null)
                    return photoBytes;
            }
            catch
            {
                // 다음 단말기로 시도
            }
        }
        return null;
    }

    /// <summary>
    /// 단일 단말기에서 사진을 가져옵니다.
    /// </summary>
    private static async Task<byte[]?> FetchUserPhotoFromDeviceAsync(
        string baseUrl,
        string userId,
        string password)
    {
        // 1. 로그인 (JWT 토큰 획득)
        var token = await LoginAsync(baseUrl, password);
        if (token == null) return null;

        // 2. 사람 목록에서 해당 userId의 photo 경로 검색
        var photoPath = await GetPhotoPathAsync(baseUrl, token, userId);
        if (string.IsNullOrEmpty(photoPath)) return null;

        // 3. 사진 파일 다운로드 (인증 불필요 ? 직접 접근 가능)
        var photoUrl = $"{baseUrl}{photoPath}";
        var response = await _httpClient.GetAsync(photoUrl);
        if (!response.IsSuccessStatusCode) return null;

        var bytes = await response.Content.ReadAsByteArrayAsync();
        return bytes.Length > 100 ? bytes : null;
    }

    /// <summary>
    /// POST /api/User/Login ? 단말기 로그인 및 Bearer 토큰 반환
    /// </summary>
    private static async Task<string?> LoginAsync(string baseUrl, string password)
    {
        var sHash = Guid.NewGuid().ToString().ToUpper();
        var sMessage = sHash + password + sHash;
        var cipher = ComputeMd5Upper(sMessage);

        var payload = new { password = cipher, Hash = sHash };
        var content = new StringContent(
            System.Text.Json.JsonSerializer.Serialize(payload),
            Encoding.UTF8,
            "application/json");

        var response = await _httpClient.PostAsync($"{baseUrl}/api/User/Login", content);
        if (!response.IsSuccessStatusCode) return null;

        var json = await response.Content.ReadAsStringAsync();
        var doc = System.Text.Json.JsonDocument.Parse(json);
        var root = doc.RootElement;

        if (!root.TryGetProperty("result", out var resultProp) || !resultProp.GetBoolean())
            return null;

        if (!root.TryGetProperty("content", out var contentProp)) return null;
        if (!contentProp.TryGetProperty("token", out var tokenProp)) return null;
        return tokenProp.GetString();
    }

    /// <summary>
    /// POST /api/People/Search ? Bearer 토큰으로 인증하여 userId에 해당하는 사진 경로 반환
    /// </summary>
    private static async Task<string?> GetPhotoPathAsync(string baseUrl, string token, string userId)
    {
        var payload = new { PageIndex = 1, PageSize = 200 };
        var request = new HttpRequestMessage(HttpMethod.Post, $"{baseUrl}/api/People/Search")
        {
            Content = new StringContent(
                System.Text.Json.JsonSerializer.Serialize(payload),
                Encoding.UTF8,
                "application/json")
        };
        request.Headers.Authorization =
            new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);

        var response = await _httpClient.SendAsync(request);
        if (!response.IsSuccessStatusCode) return null;

        var json = await response.Content.ReadAsStringAsync();
        var doc = System.Text.Json.JsonDocument.Parse(json);
        var root = doc.RootElement;

        if (!root.TryGetProperty("result", out var resultProp) || !resultProp.GetBoolean())
            return null;

        if (!root.TryGetProperty("content", out var contentProp)) return null;
        if (!contentProp.TryGetProperty("DataList", out var listProp)) return null;

        foreach (var person in listProp.EnumerateArray())
        {
            var uid = person.TryGetProperty("UserID", out var uidProp) ? uidProp.GetString() : null;
            if (string.Equals(uid, userId, StringComparison.OrdinalIgnoreCase))
            {
                return person.TryGetProperty("photo", out var photoProp) ? photoProp.GetString() : null;
            }
        }

        return null;
    }

    private static string ComputeMd5Upper(string input)
    {
        var bytes = Encoding.UTF8.GetBytes(input);
        var hash = MD5.HashData(bytes);
        return Convert.ToHexString(hash).ToUpper();
    }
}
