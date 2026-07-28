using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;

namespace EveFitScanUI.Pricing;

public sealed class JanicePriceProvider : IPriceProvider
{
	private const string PricerUrl = "https://janice.e-351.com/api/rest/v2/pricer?market=2";

	private readonly string _apiKey;

	private readonly HttpClient _http;

	public JanicePriceProvider(string apiKey, HttpClient http = null)
	{
		if (string.IsNullOrWhiteSpace(apiKey))
		{
			throw new ArgumentException("Janice API key is required.", "apiKey");
		}
		_apiKey = apiKey.Trim();
		_http = http ?? new HttpClient
		{
			Timeout = TimeSpan.FromSeconds(30.0)
		};
	}

	public async Task<IReadOnlyDictionary<string, double>> GetPricesAsync(IEnumerable<string> itemNames, CancellationToken cancellationToken)
	{
		List<string> names = new List<string>();
		HashSet<string> seen = new HashSet<string>(StringComparer.Ordinal);
		foreach (string name in itemNames)
		{
			if (!string.IsNullOrWhiteSpace(name) && seen.Add(name))
			{
				names.Add(name);
			}
		}
		Dictionary<string, double> result = new Dictionary<string, double>(StringComparer.Ordinal);
		if (names.Count == 0)
		{
			return result;
		}
		string body = string.Join("\n", names);
		using (HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Post, "https://janice.e-351.com/api/rest/v2/pricer?market=2"))
		{
			request.Headers.TryAddWithoutValidation("X-ApiKey", _apiKey);
			request.Content = new StringContent(body, Encoding.UTF8, "text/plain");
			using HttpResponseMessage response = await _http.SendAsync(request, cancellationToken).ConfigureAwait(continueOnCapturedContext: false);
			response.EnsureSuccessStatusCode();
			using JsonDocument doc = JsonDocument.Parse(await response.Content.ReadAsStringAsync().ConfigureAwait(continueOnCapturedContext: false));
			if (doc.RootElement.ValueKind != JsonValueKind.Array)
			{
				return result;
			}
			foreach (JsonElement item in doc.RootElement.EnumerateArray())
			{
				if (!item.TryGetProperty("itemType", out var itemType) || !itemType.TryGetProperty("name", out var nameEl))
				{
					continue;
				}
				string name2 = nameEl.GetString();
				if (!string.IsNullOrEmpty(name2) && item.TryGetProperty("immediatePrices", out var prices) && prices.TryGetProperty("splitPrice", out var splitEl))
				{
					double split;
					if (splitEl.ValueKind == JsonValueKind.Number)
					{
						split = splitEl.GetDouble();
					}
					else if (!double.TryParse(splitEl.GetString(), out split))
					{
						continue;
					}
					if (split > 0.0)
					{
						result[name2] = split;
					}
					itemType = default(JsonElement);
					nameEl = default(JsonElement);
					prices = default(JsonElement);
					splitEl = default(JsonElement);
				}
			}
		}
		return result;
	}
}
