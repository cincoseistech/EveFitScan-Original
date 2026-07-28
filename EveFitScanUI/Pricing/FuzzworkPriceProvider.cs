using System;
using System.Collections.Generic;
using System.Globalization;
using System.Net.Http;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;
using EveFitScan.Core;

namespace EveFitScanUI.Pricing;

public sealed class FuzzworkPriceProvider : IPriceProvider
{
	private const string AggregatesUrl = "https://market.fuzzwork.co.uk/aggregates/?station=60003760&types=";

	private const int ChunkSize = 100;

	private readonly FitScanProcessor _processor;

	private readonly HttpClient _http;

	public FuzzworkPriceProvider(FitScanProcessor processor, HttpClient http = null)
	{
		_processor = processor ?? throw new ArgumentNullException("processor");
		_http = http ?? new HttpClient
		{
			Timeout = TimeSpan.FromSeconds(30.0)
		};
	}

	public async Task<IReadOnlyDictionary<string, double>> GetPricesAsync(IEnumerable<string> itemNames, CancellationToken cancellationToken)
	{
		Dictionary<int, string> nameByTypeId = new Dictionary<int, string>();
		foreach (string name in itemNames)
		{
			if (_processor.TryResolveTypeId(name, out var typeId) && !nameByTypeId.ContainsKey(typeId))
			{
				nameByTypeId[typeId] = name;
			}
		}
		Dictionary<string, double> result = new Dictionary<string, double>(StringComparer.Ordinal);
		if (nameByTypeId.Count == 0)
		{
			return result;
		}
		List<int> typeIds = new List<int>(nameByTypeId.Keys);
		for (int i = 0; i < typeIds.Count; i += 100)
		{
			List<int> chunk = typeIds.GetRange(i, Math.Min(100, typeIds.Count - i));
			string url = "https://market.fuzzwork.co.uk/aggregates/?station=60003760&types=" + string.Join(",", chunk);
			using HttpResponseMessage response = await _http.GetAsync(url, cancellationToken).ConfigureAwait(continueOnCapturedContext: false);
			response.EnsureSuccessStatusCode();
			using JsonDocument doc = JsonDocument.Parse(await response.Content.ReadAsStringAsync().ConfigureAwait(continueOnCapturedContext: false));
			foreach (JsonProperty prop in doc.RootElement.EnumerateObject())
			{
				if (int.TryParse(prop.Name, out var typeId2) && nameByTypeId.TryGetValue(typeId2, out var name2))
				{
					double sellMin = GetNestedDouble(prop.Value, "sell", "min");
					double buyMax = GetNestedDouble(prop.Value, "buy", "max");
					if (!(sellMin <= 0.0) || !(buyMax <= 0.0))
					{
						result[name2] = 0.5 * (sellMin + buyMax);
						name2 = null;
					}
				}
			}
		}
		return result;
	}

	private static double GetNestedDouble(JsonElement root, string side, string field)
	{
		if (!root.TryGetProperty(side, out var value))
		{
			return 0.0;
		}
		if (!value.TryGetProperty(field, out var value2))
		{
			return 0.0;
		}
		if (value2.ValueKind == JsonValueKind.Number && value2.TryGetDouble(out var value3))
		{
			return value3;
		}
		if (value2.ValueKind == JsonValueKind.String && double.TryParse(value2.GetString(), NumberStyles.Float, CultureInfo.InvariantCulture, out value3))
		{
			return value3;
		}
		return 0.0;
	}
}
