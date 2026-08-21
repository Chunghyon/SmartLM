using OpenCvSharp;

namespace FaceDeviceHttpPcServer.Services;

public static class FacePhotoValidator
{
    public static (bool ok, int faceCount, string message) ValidateJpeg(byte[] jpeg)
    {
        if (jpeg == null || jpeg.Length < 100)
            return (false, 0, "사진 데이터가 없습니다.");

        var cascadePath = FindCascade();
        if (string.IsNullOrEmpty(cascadePath) || !File.Exists(cascadePath))
            return (false, 0, "얼굴 검사 파일(haarcascade)을 찾을 수 없습니다.");

        try
        {
            using var mat = Cv2.ImDecode(jpeg, ImreadModes.Color);
            if (mat.Empty())
                return (false, 0, "사진을 열 수 없습니다.");

            using var gray = new Mat();
            Cv2.CvtColor(mat, gray, ColorConversionCodes.BGR2GRAY);
            Cv2.EqualizeHist(gray, gray);

            using var cascade = new CascadeClassifier(cascadePath);
            if (cascade.Empty())
                return (false, 0, "얼굴 검사기를 불러오지 못했습니다.");

            var faces = cascade.DetectMultiScale(
                gray,
                scaleFactor: 1.1,
                minNeighbors: 5,
                flags: HaarDetectionTypes.ScaleImage,
                minSize: new OpenCvSharp.Size(80, 80));

            if (faces.Length == 0)
                return (false, 0, "얼굴을 찾지 못했습니다. 가이드 안에 얼굴이 크게 오도록 조정하세요.");
            if (faces.Length > 1)
                return (false, faces.Length, $"얼굴이 {faces.Length}개 감지되었습니다. 한 사람만 나오게 조정하세요.");
            return (true, 1, "얼굴 1개 확인");
        }
        catch (Exception ex)
        {
            return (false, 0, "얼굴 검사 실패: " + ex.Message);
        }
    }

    static string? FindCascade()
    {
        var names = new[]
        {
            Path.Combine(AppContext.BaseDirectory, "Resources", "haarcascade_frontalface_default.xml"),
            Path.Combine(AppContext.BaseDirectory, "haarcascade_frontalface_default.xml"),
            Path.Combine(Directory.GetCurrentDirectory(), "Resources", "haarcascade_frontalface_default.xml")
        };
        return names.FirstOrDefault(File.Exists);
    }
}
