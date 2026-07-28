namespace SdeConverter;

public sealed class CatalogUpdateResult
{
    public int BuildNumber { get; init; }
    public string ReleaseDate { get; init; } = "";
    public int ShipCount { get; init; }
    public int ModuleCount { get; init; }
    public string MsgpackPath { get; init; } = "";
    public IReadOnlyList<string> Warnings { get; init; } = Array.Empty<string>();
}

/// <summary>
/// Downloads the EVE JSONL SDE (if needed) and writes a MessagePack catalog.
/// Shared by the CLI and the FitScan Settings UI.
/// </summary>
public static class CatalogUpdater
{
    public static async Task<CatalogUpdateResult> UpdateAsync(
        string cacheDir,
        string msgpackPath,
        string metaPath,
        bool skipDownload = false,
        IProgress<string> progress = null,
        CancellationToken cancellationToken = default)
    {
        if (string.IsNullOrWhiteSpace(cacheDir))
            throw new ArgumentException("Cache directory is required.", nameof(cacheDir));
        if (string.IsNullOrWhiteSpace(msgpackPath))
            throw new ArgumentException("Msgpack path is required.", nameof(msgpackPath));

        void Log(string message)
        {
            progress?.Report(message);
            Console.WriteLine(message);
        }

        var cache = new SdeCache(cacheDir, Log);
        await cache.EnsureAsync(skipDownload, cancellationToken).ConfigureAwait(false);

        Log("Building ship/module catalogs from SDE...");
        var data = SdeData.Load(cache);
        var ships = ShipCatalog.Build(data);
        var modules = ModuleCatalog.Build(data);

        Log($"Writing {msgpackPath}");
        CatalogPacker.Write(msgpackPath, metaPath, data.BuildNumber, data.ReleaseDate, ships, modules);

        var warnings = ModuleCatalog.Warnings.Count > 0
            ? ModuleCatalog.Warnings.ToArray()
            : Array.Empty<string>();

        Log($"Done. ships={ships.Count} modules={modules.Count} buildNumber={data.BuildNumber}");

        return new CatalogUpdateResult
        {
            BuildNumber = data.BuildNumber,
            ReleaseDate = data.ReleaseDate ?? "",
            ShipCount = ships.Count,
            ModuleCount = modules.Count,
            MsgpackPath = msgpackPath,
            Warnings = warnings,
        };
    }
}
