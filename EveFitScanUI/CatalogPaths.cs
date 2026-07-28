using System;
using System.IO;

namespace EveFitScanUI;

public static class CatalogPaths
{
	public static string AppDataDir => Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "EveFitScan");

	public static string CatalogMsgpack => Path.Combine(AppDataDir, "fitscan-catalog.msgpack");

	public static string CatalogMeta => Path.Combine(AppDataDir, "fitscan-catalog.meta.json");

	public static string SdeCacheDir => Path.Combine(AppDataDir, "sde-cache");
}
