using System.IO.Compression;
using System.Text.Json;

namespace SdeConverter;

sealed class SdeCache
{
    public const string DownloadUrl = "https://developers.eveonline.com/static-data/eve-online-static-data-latest-jsonl.zip";
    public const string LatestMetaUrl = "https://developers.eveonline.com/static-data/tranquility/latest.jsonl";
    public const string ZipFileName = "sde-latest.jsonl.zip";
    public const string ExtractDirName = "extract";

    static readonly string[] RequiredFiles =
    [
        "_sde.jsonl",
        "types.jsonl",
        "marketGroups.jsonl",
        "typeDogma.jsonl",
        "typeBonus.jsonl",
    ];

    readonly Action<string> _log;

    public string CacheDir { get; }
    public string ExtractDir => Path.Combine(CacheDir, ExtractDirName);
    public string ZipPath => Path.Combine(CacheDir, ZipFileName);

    public SdeCache(string cacheDir, Action<string> log = null)
    {
        CacheDir = Path.GetFullPath(cacheDir);
        _log = log ?? Console.WriteLine;
    }

    void Log(string message) => _log(message);

    public bool HasCompleteExtract()
    {
        if (!Directory.Exists(ExtractDir))
            return false;
        foreach (var file in RequiredFiles)
        {
            if (!File.Exists(Path.Combine(ExtractDir, file)))
                return false;
        }
        return true;
    }

    public async Task EnsureAsync(bool skipDownload, CancellationToken ct = default)
    {
        Directory.CreateDirectory(CacheDir);

        if (skipDownload)
        {
            if (!HasCompleteExtract())
                throw new InvalidOperationException($"--skip-download set but required JSONL files are missing under {ExtractDir}");
            var cached = ReadBuildInfo();
            Log($"Using cached SDE extract: {ExtractDir} (build {cached.BuildNumber})");
            return;
        }

        using var http = new HttpClient { Timeout = TimeSpan.FromMinutes(30) };
        var remoteBuild = await TryGetRemoteBuildNumberAsync(http, ct);

        if (HasCompleteExtract())
        {
            var cached = ReadBuildInfo();
            if (remoteBuild is null || remoteBuild.Value == cached.BuildNumber)
            {
                Log($"SDE cache is current (build {cached.BuildNumber}).");
                return;
            }
            Log($"SDE update available: cached={cached.BuildNumber} remote={remoteBuild.Value}");
        }
        else if (remoteBuild is int build)
        {
            Log($"No local SDE cache; remote build {build}.");
        }

        Log($"Downloading SDE from {DownloadUrl} ...");
        await DownloadAsync(http, ct);
        Log($"Extracting to {ExtractDir} ...");
        ExtractZip();
        if (!HasCompleteExtract())
            throw new InvalidOperationException("SDE extract is incomplete after download.");

        var info = ReadBuildInfo();
        Log($"Cached SDE build {info.BuildNumber} ({info.ReleaseDate})");
    }

    async Task<int?> TryGetRemoteBuildNumberAsync(HttpClient http, CancellationToken ct)
    {
        try
        {
            await using var stream = await http.GetStreamAsync(LatestMetaUrl, ct);
            using var reader = new StreamReader(stream);
            string? line;
            while ((line = await reader.ReadLineAsync(ct)) is not null)
            {
                if (string.IsNullOrWhiteSpace(line))
                    continue;
                using var doc = JsonDocument.Parse(line);
                var root = doc.RootElement;
                if (!root.TryGetProperty("_key", out var key))
                    continue;
                var keyText = key.ValueKind == JsonValueKind.String ? key.GetString() : key.ToString();
                if (!string.Equals(keyText, "sde", StringComparison.OrdinalIgnoreCase))
                    continue;
                if (root.TryGetProperty("buildNumber", out var bn) && bn.TryGetInt32(out var build))
                    return build;
            }
        }
        catch (Exception ex)
        {
            Log($"Warning: could not read remote SDE build number ({ex.Message}); will download zip.");
        }
        return null;
    }

    async Task DownloadAsync(HttpClient http, CancellationToken ct)
    {
        using var response = await http.GetAsync(DownloadUrl, HttpCompletionOption.ResponseHeadersRead, ct);
        response.EnsureSuccessStatusCode();
        await using var stream = await response.Content.ReadAsStreamAsync(ct);
        await using var file = File.Create(ZipPath);
        await stream.CopyToAsync(file, ct);
        Log($"Saved {ZipPath} ({new FileInfo(ZipPath).Length:N0} bytes)");
    }

    void ExtractZip()
    {
        if (Directory.Exists(ExtractDir))
            Directory.Delete(ExtractDir, recursive: true);
        Directory.CreateDirectory(ExtractDir);
        ZipFile.ExtractToDirectory(ZipPath, ExtractDir);
    }

    public (int BuildNumber, string ReleaseDate) ReadBuildInfo()
    {
        var path = Path.Combine(ExtractDir, "_sde.jsonl");
        foreach (var el in Jsonl.ReadObjects(path))
        {
            var build = Jsonl.GetInt(el, "buildNumber");
            var release = el.TryGetProperty("releaseDate", out var rd) ? rd.GetString() ?? "" : "";
            return (build, release);
        }
        throw new InvalidOperationException("_sde.jsonl did not contain build info.");
    }
}
