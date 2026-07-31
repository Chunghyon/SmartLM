using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Threading.Tasks;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Formats.Jpeg;
using SixLabors.ImageSharp.Processing;

namespace DeviceProtocolServer.Utilities;

/// <summary>
/// JPEG 图片转换工具类
/// 基于硬件解码兼容性要求，将任意图片转换为设备端硬解码可接受的 JPEG 格式。
/// 
/// 约束来源: JPEG 硬件解码失败问题实测结论 (2026/06/23)
/// - 必须 baseline，standard Huffman
/// - 3 通道 YCbCr，4:2:0 子采样
/// - 尺寸 [256, 720]，解码上限 1024×1024
/// - 去除 ICC profile 和大块 EXIF
/// - 长宽比不超过 3:1
/// </summary>
public static class JpegConvertHelper
{
    /// <summary>默认 JPEG 质量 (1-100)</summary>
    public const int DefaultQuality = 85;

    /// <summary>建议最小尺寸</summary>
    public const int MinDimension = 256;

    /// <summary>建议最大尺寸</summary>
    public const int MaxDimension = 720;

    /// <summary>硬件解码器最大解码尺寸(不含)</summary>
    public const int DecoderMaxSize = 1024;

    /// <summary>允许的最大长宽比</summary>
    public const double MaxAspectRatio = 3.0;

    /// <summary>
    /// 将图片转换为符合硬件解码要求的 JPEG 文件。
    /// 自动处理: 强制 RGB→YCbCr、尺寸约束、去除元数据、标准 Huffman。
    /// </summary>
    /// <param name="sourcePath">源图片路径</param>
    /// <param name="outputPath">输出 JPEG 路径 (自动添加 .jpg 后缀)</param>
    /// <param name="options">转换选项 (为 null 时使用默认值)</param>
    /// <returns>转换结果，包含是否成功、警告信息等</returns>
    public static ConvertResult Convert(string sourcePath, string outputPath, ConvertOptions? options = null)
    {
        options ??= new ConvertOptions();
        using var stream = File.OpenRead(sourcePath);
        return ConvertFromStream(stream, outputPath, options);
    }

    // ========================================================================
    // byte[] 重载
    // ========================================================================

    /// <summary>
    /// 将 byte[] 图片数据转换并保存到文件。
    /// </summary>
    public static ConvertResult Convert(byte[] sourceData, string outputPath, ConvertOptions? options = null)
    {
        options ??= new ConvertOptions();
        using var ms = new MemoryStream(sourceData);
        return ConvertFromStream(ms, outputPath, options);
    }

    /// <summary>
    /// 将 byte[] 图片数据转换，返回转换后的 byte[]。
    /// </summary>
    public static ConvertResult Convert(byte[] sourceData, ConvertOptions? options = null)
    {
        options ??= new ConvertOptions();
        using var inStream = new MemoryStream(sourceData);
        using var outStream = new MemoryStream();
        return ConvertFromStream(inStream, outStream, options);
    }

    // ========================================================================
    // Stream 重载
    // ========================================================================

    /// <summary>
    /// 从 Stream 读取图片并转换保存到文件。
    /// </summary>
    public static ConvertResult Convert(Stream sourceStream, string outputPath, ConvertOptions? options = null)
    {
        options ??= new ConvertOptions();
        return ConvertFromStream(sourceStream, outputPath, options);
    }

    /// <summary>
    /// 从 Stream 读取图片，转换后写入目标 Stream。
    /// </summary>
    public static ConvertResult Convert(Stream sourceStream, Stream outputStream, ConvertOptions? options = null)
    {
        options ??= new ConvertOptions();
        return ConvertFromStream(sourceStream, outputStream, options);
    }

    // ========================================================================
    // 异步
    // ========================================================================

    /// <summary>
    /// 异步版本 (文件路径): 将图片转换为符合硬件解码要求的 JPEG 文件。
    /// </summary>
    public static async Task<ConvertResult> ConvertAsync(string sourcePath, string outputPath, ConvertOptions? options = null)
    {
        return await Task.Run(() => Convert(sourcePath, outputPath, options));
    }

    /// <summary>
    /// 异步版本 (byte[]): 返回转换后的 byte[]。
    /// </summary>
    public static async Task<ConvertResult> ConvertAsync(byte[] sourceData, ConvertOptions? options = null)
    {
        return await Task.Run(() => Convert(sourceData, options));
    }

    /// <summary>
    /// 异步版本 (Stream → Stream)。
    /// </summary>
    public static async Task<ConvertResult> ConvertAsync(Stream sourceStream, Stream outputStream, ConvertOptions? options = null)
    {
        return await Task.Run(() => Convert(sourceStream, outputStream, options));
    }

    /// <summary>
    /// 批量转换目录中的图片。
    /// </summary>
    /// <param name="inputDir">输入目录</param>
    /// <param name="outputDir">输出目录</param>
    /// <param name="searchPattern">文件匹配模式, e.g. "*.jpg"</param>
    /// <param name="options">转换选项</param>
    /// <returns>每张图的转换结果</returns>
    public static List<ConvertResult> ConvertDirectory(
        string inputDir,
        string outputDir,
        string searchPattern = "*.*",
        ConvertOptions? options = null)
    {
        var results = new List<ConvertResult>();
        if (!Directory.Exists(inputDir))
        {
            results.Add(new ConvertResult
            {
                Success = false,
                Errors = { $"输入目录不存在: {inputDir}" }
            });
            return results;
        }

        Directory.CreateDirectory(outputDir);

        var supportedExtensions = new HashSet<string>(StringComparer.OrdinalIgnoreCase)
        {
            ".jpg", ".jpeg", ".png", ".bmp", ".gif", ".tiff", ".tif", ".webp"
        };

        var files = Directory.GetFiles(inputDir, searchPattern)
            .Where(f => supportedExtensions.Contains(Path.GetExtension(f)))
            .ToList();

        foreach (var file in files)
        {
            var outputPath = Path.Combine(outputDir, Path.GetFileNameWithoutExtension(file) + ".jpg");
            var result = Convert(file, outputPath, options);
            result.SourceFile = file;
            results.Add(result);
        }

        return results;
    }

    /// <summary>
    /// 验证图片是否已满足硬件解码兼容性要求 (仅检查属性，不做转换)。
    /// </summary>
    /// <param name="imagePath">图片路径</param>
    /// <returns>验证结果，列出所有不兼容项</returns>
    public static ValidationResult Validate(string imagePath)
    {
        var result = new ValidationResult();
        try
        {
            using var image = Image.Load(imagePath);

            // 尺寸检查
            if (image.Width < MinDimension || image.Height < MinDimension)
                result.Issues.Add($"尺寸过小: {image.Width}×{image.Height}，建议 ≥{MinDimension}×{MinDimension}");

            if (image.Width >= DecoderMaxSize || image.Height >= DecoderMaxSize)
                result.Issues.Add($"尺寸超过/达到解码上限: {image.Width}×{image.Height}，解码器上限为 {DecoderMaxSize}×{DecoderMaxSize}(不含)");

            double aspectRatio = (double)Math.Max(image.Width, image.Height) / Math.Min(image.Width, image.Height);
            if (aspectRatio > MaxAspectRatio)
                result.Issues.Add($"长宽比过大: {aspectRatio:F2}:1，建议≤{MaxAspectRatio}:1");

            result.IsCompatible = result.Issues.Count == 0;
            result.Width = image.Width;
            result.Height = image.Height;

            // 检查是否为 JPEG 格式
            if (image.Metadata.DecodedImageFormat is JpegFormat)
            {
                result.IsJpeg = true;
                // 注意: ImageSharp 3.x 无法直接检测 Progressive/Optimized Huffman
                // 这些信息需要更底层的 JPEG 解析
            }
        }
        catch (Exception ex)
        {
            result.IsCompatible = false;
            result.Issues.Add($"验证失败: {ex.Message}");
        }
        return result;
    }

    // ========================================================================
    // 核心处理逻辑
    // ========================================================================

    /// <summary>从 Stream 读取→处理→保存到文件。</summary>
    private static ConvertResult ConvertFromStream(Stream sourceStream, string outputPath, ConvertOptions options)
    {
        var result = ProcessImage(sourceStream, options);

        if (!result.Success) return result;

        try
        {
            if (!outputPath.EndsWith(".jpg", StringComparison.OrdinalIgnoreCase) &&
                !outputPath.EndsWith(".jpeg", StringComparison.OrdinalIgnoreCase))
                outputPath += ".jpg";

            result.OutputPath = outputPath;
            result.Image!.Save(outputPath, BuildEncoder(options));
            result.MD5 = ComputeFileMd5(outputPath);
        }
        catch (Exception ex)
        {
            result.Success = false;
            result.Errors.Add(ex.Message);
        }
        finally
        {
            result.Image?.Dispose();
        }

        return result;
    }

    /// <summary>从 Stream 读取→处理→写入 Stream。</summary>
    private static ConvertResult ConvertFromStream(Stream sourceStream, Stream outputStream, ConvertOptions options)
    {
        var result = ProcessImage(sourceStream, options);

        if (!result.Success) return result;

        try
        {
            result.Image!.Save(outputStream, BuildEncoder(options));
            outputStream.Position = 0;

            // 如果 outputStream 是 MemoryStream，提取 byte[]
            if (outputStream is MemoryStream ms)
            {
                result.OutputData = ms.ToArray();
            }

            // 从 stream 计算 MD5
            result.MD5 = ComputeStreamMd5(outputStream);
        }
        catch (Exception ex)
        {
            result.Success = false;
            result.Errors.Add(ex.Message);
        }
        finally
        {
            result.Image?.Dispose();
        }

        return result;
    }

    /// <summary>核心处理: 加载图片、缩放、约束检查。返回带 Image 的 result。</summary>
    private static ConvertResult ProcessImage(Stream sourceStream, ConvertOptions options)
    {
        var result = new ConvertResult();

        try
        {
            var image = Image.Load(sourceStream);

            result.OriginalWidth = image.Width;
            result.OriginalHeight = image.Height;

            var (newWidth, newHeight) = CalculateTargetSize(
                image.Width, image.Height,
                options.MinDimension ?? MinDimension,
                options.MaxDimension ?? MaxDimension,
                options.DecoderMaxSize ?? DecoderMaxSize);

            if (newWidth != image.Width || newHeight != image.Height)
            {
                image.Mutate(x => x.Resize(newWidth, newHeight, KnownResamplers.Lanczos3));
                result.Resized = true;
                result.Warnings.Add($"尺寸已从 {result.OriginalWidth}×{result.OriginalHeight} 缩放至 {newWidth}×{newHeight}");
            }

            result.OutputWidth = newWidth;
            result.OutputHeight = newHeight;

            double aspectRatio = (double)Math.Max(newWidth, newHeight) / Math.Min(newWidth, newHeight);
            if (aspectRatio > (options.MaxAspectRatio ?? MaxAspectRatio))
            {
                result.Warnings.Add(
                    $"长宽比 {aspectRatio:F2}:1 超过建议值 {options.MaxAspectRatio ?? MaxAspectRatio}:1，" +
                    "极端长宽比可能导致硬解码失败");
            }

            if (newWidth >= DecoderMaxSize || newHeight >= DecoderMaxSize)
            {
                result.Warnings.Add(
                    $"尺寸 {newWidth}×{newHeight} 达到/超过解码器上限 {DecoderMaxSize}×{DecoderMaxSize}，" +
                    "建议缩小图片避免解码失败");
            }

            result.Image = image;
            result.Success = true;
        }
        catch (Exception ex)
        {
            result.Success = false;
            result.Errors.Add(ex.Message);
        }

        return result;
    }

    /// <summary>构建 JpegEncoder。</summary>
    private static JpegEncoder BuildEncoder(ConvertOptions options)
    {
        return new JpegEncoder
        {
            Quality = options.Quality ?? DefaultQuality,
            ColorType = JpegEncodingColor.YCbCrRatio420,
            SkipMetadata = options.SkipMetadata ?? true,
            Interleaved = true,
        };
    }

    /// <summary>计算文件的 MD5，返回 32 位小写十六进制字符串。</summary>
    private static string ComputeFileMd5(string filePath)
    {
        using var stream = File.OpenRead(filePath);
        return ComputeStreamMd5(stream);
    }

    /// <summary>计算 Stream 的 MD5，返回 32 位小写十六进制字符串。</summary>
    private static string ComputeStreamMd5(Stream stream)
    {
        var hash = MD5.HashData(stream);
        stream.Position = 0;
        return System.Convert.ToHexString(hash).ToUpperInvariant();
    }

    /// <summary>
    /// 根据硬解码约束计算目标尺寸。
    /// 规则:
    /// 1. 宽高均定位到 [min, max] 区间
    /// 2. 不得超过解码器上限 1024
    /// 3. 保持原始长宽比
    /// </summary>
    private static (int Width, int Height) CalculateTargetSize(
        int origW, int origH, int minDim, int maxDim, int decoderMax)
    {
        int w = origW;
        int h = origH;

        // 如果任一边超过解码器上限，等比缩放到上限以内
        int longer = Math.Max(w, h);
        if (longer >= decoderMax)
        {
            double scale = (decoderMax - 1) / (double)longer;
            w = (int)Math.Round(w * scale);
            h = (int)Math.Round(h * scale);
        }

        // 如果任一边超过建议最大值，等比缩放到 max
        longer = Math.Max(w, h);
        if (longer > maxDim)
        {
            double scale = maxDim / (double)longer;
            w = (int)Math.Round(w * scale);
            h = (int)Math.Round(h * scale);
        }

        // 如果任一边太小，等比放大到 min (保持长宽比)
        int shorter = Math.Min(w, h);
        if (shorter < minDim)
        {
            double scale = minDim / (double)shorter;
            w = (int)Math.Round(w * scale);
            h = (int)Math.Round(h * scale);

            // 放大后仍需检查不超过 decoderMax
            longer = Math.Max(w, h);
            if (longer >= decoderMax)
            {
                double scaleDown = (decoderMax - 1) / (double)longer;
                w = (int)Math.Round(w * scaleDown);
                h = (int)Math.Round(h * scaleDown);
            }
        }

        // 确保最小 1px
        w = Math.Max(1, w);
        h = Math.Max(1, h);

        return (w, h);
    }
}

/// <summary>
/// 转换选项
/// </summary>
public class ConvertOptions
{
    /// <summary>JPEG 质量 (1-100)，默认 85。建议 80-90，不要 ≥95 (避免触发 Optimized Huffman)</summary>
    public int? Quality { get; set; }

    /// <summary>是否跳过元数据 (EXIF/ICC/XMP)，默认 true</summary>
    public bool? SkipMetadata { get; set; }

    /// <summary>
    /// 最小尺寸 (像素)，图片任一边小于此值会被放大。
    /// 默认 256。设为 null 表示不限制。
    /// </summary>
    public int? MinDimension { get; set; }

    /// <summary>
    /// 最大尺寸 (像素)，图片长边超过此值会被缩小。
    /// 默认 720。设为 null 表示不限制。
    /// </summary>
    public int? MaxDimension { get; set; }

    /// <summary>
    /// 解码器最大尺寸上限 (不含)，超过此值强制缩放。
    /// 默认 1024。设为 null 表示不限制。
    /// </summary>
    public int? DecoderMaxSize { get; set; }

    /// <summary>
    /// 允许的最大长宽比，默认 3.0。
    /// 设为 null 表示不检查。
    /// </summary>
    public double? MaxAspectRatio { get; set; }

    /// <summary>
    /// 创建默认选项 (兼容硬解码要求)
    /// </summary>
    public static ConvertOptions Default => new();

    /// <summary>
    /// 仅去除元数据，不做任何尺寸调整
    /// </summary>
    public static ConvertOptions StripMetadataOnly => new()
    {
        MinDimension = null,
        MaxDimension = null,
        DecoderMaxSize = null,
        MaxAspectRatio = null,
    };
}

/// <summary>
/// 单张图片转换结果
/// </summary>
public class ConvertResult
{
    /// <summary>是否转换成功</summary>
    public bool Success { get; set; }

    /// <summary>源文件路径 (批量转换时填充)</summary>
    public string? SourceFile { get; set; }

    /// <summary>输出文件路径 (保存到文件时填充)</summary>
    public string? OutputPath { get; set; }

    /// <summary>转换后的 byte[] (byte[]/Stream 输出时填充)</summary>
    public byte[]? OutputData { get; set; }

    /// <summary>原始宽度</summary>
    public int OriginalWidth { get; set; }

    /// <summary>原始高度</summary>
    public int OriginalHeight { get; set; }

    /// <summary>输出宽度</summary>
    public int OutputWidth { get; set; }

    /// <summary>输出高度</summary>
    public int OutputHeight { get; set; }

    /// <summary>是否进行了缩放</summary>
    public bool Resized { get; set; }

    /// <summary>警告信息</summary>
    public List<string> Warnings { get; set; } = [];

    /// <summary>错误信息</summary>
    public List<string> Errors { get; set; } = [];

    /// <summary>转换后图片的 MD5 值 (32 位大写十六进制)</summary>
    public string? MD5 { get; set; }

    /// <summary>内部持有的 Image 对象 (处理完成后由调用方 Dispose)</summary>
    internal SixLabors.ImageSharp.Image? Image { get; set; }
}

/// <summary>
/// 验证结果
/// </summary>
public class ValidationResult
{
    /// <summary>是否兼容硬解码</summary>
    public bool IsCompatible { get; set; }

    /// <summary>是否为 JPEG 格式</summary>
    public bool IsJpeg { get; set; }

    /// <summary>图片宽度</summary>
    public int Width { get; set; }

    /// <summary>图片高度</summary>
    public int Height { get; set; }

    /// <summary>不兼容项列表</summary>
    public List<string> Issues { get; set; } = [];
}
