using SdeConverter;

static class Entry
{
    static async Task<int> Main(string[] args)
    {
        var command = "update";
        var skipDownload = false;
        string? cacheDir = null;

        foreach (var arg in args)
        {
            if (arg is "update" or "help" or "--help" or "-h")
            {
                if (arg is "help" or "--help" or "-h")
                {
                    PrintUsage();
                    return 0;
                }
                command = arg;
            }
            else if (arg == "--skip-download")
            {
                skipDownload = true;
            }
            else if (arg.StartsWith("--cache-dir=", StringComparison.Ordinal))
            {
                cacheDir = arg["--cache-dir=".Length..];
            }
            else
            {
                Console.Error.WriteLine($"Unknown argument: {arg}");
                PrintUsage();
                return 1;
            }
        }

        if (command != "update")
        {
            PrintUsage();
            return 1;
        }

        try
        {
            var repoRoot = FindRepoRoot();
            cacheDir ??= Path.Combine(repoRoot, ".sde-cache");
            var dataDir = Path.Combine(repoRoot, "EveFitScan.Core", "Data");
            var msgpackOut = Path.Combine(dataDir, "fitscan-catalog.msgpack");
            var metaOut = Path.Combine(dataDir, "fitscan-catalog.meta.json");

            var result = await CatalogUpdater.UpdateAsync(cacheDir, msgpackOut, metaOut, skipDownload);
            if (result.Warnings.Count > 0)
            {
                Console.WriteLine($"Warnings: {result.Warnings.Count}");
                foreach (var warning in result.Warnings)
                    Console.WriteLine("  " + warning);
            }
            return 0;
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine(ex.ToString());
            return 1;
        }
    }

    static void PrintUsage()
    {
        Console.WriteLine("SdeConverter — regenerate EveFitScan.Core MessagePack catalog from the EVE JSONL SDE");
        Console.WriteLine();
        Console.WriteLine("Usage:");
        Console.WriteLine("  dotnet run --project SdeConverter -- update [--skip-download] [--cache-dir=PATH]");
        Console.WriteLine();
        Console.WriteLine("  update            Download SDE if needed, then write EveFitScan.Core/Data/fitscan-catalog.msgpack");
        Console.WriteLine("  --skip-download   Use existing .sde-cache/extract only");
        Console.WriteLine("  --cache-dir=PATH  Override cache directory (default: <repo>/.sde-cache)");
        Console.WriteLine();
        Console.WriteLine("Rebuild EveFitScan.Core afterwards so the embedded catalog is refreshed.");
        Console.WriteLine("End users can also update from FitScan Settings (writes to LocalAppData).");
    }

    static string FindRepoRoot()
    {
        var dir = new DirectoryInfo(AppContext.BaseDirectory);
        while (dir != null)
        {
            if (File.Exists(Path.Combine(dir.FullName, "EveFitScan.sln")) &&
                Directory.Exists(Path.Combine(dir.FullName, "EveFitScan.Core")))
            {
                return dir.FullName;
            }
            dir = dir.Parent;
        }

        var cwd = Directory.GetCurrentDirectory();
        if (File.Exists(Path.Combine(cwd, "EveFitScan.sln")))
            return cwd;
        var parent = Directory.GetParent(cwd)?.FullName;
        if (parent != null && File.Exists(Path.Combine(parent, "EveFitScan.sln")))
            return parent;

        throw new InvalidOperationException("Could not locate EveFitScan.sln / EveFitScan.Core from process path.");
    }
}
