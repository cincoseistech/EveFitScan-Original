using System;
using System.Collections.Generic;
using System.Globalization;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;

namespace EveFitScanUI.Pricing;

public sealed class GoonpraisalPriceProvider : IPriceProvider
{
	private const string AppraisalUrl = "https://appraise.gnf.lt/appraisal.json?market=jita&persist=no";

	private const string UserAgent = "EveFitScan/1.0 (+https://github.com/; donna.hale.eve@gmail.com)";

	private readonly HttpClient _http;

	public GoonpraisalPriceProvider(HttpClient http = null)
	{
		_http = http ?? CreateClient();
	}

	private static HttpClient CreateClient()
	{
		HttpClient httpClient = new HttpClient
		{
			Timeout = TimeSpan.FromSeconds(45.0)
		};
		httpClient.DefaultRequestHeaders.UserAgent.Clear();
		httpClient.DefaultRequestHeaders.TryAddWithoutValidation("User-Agent", "EveFitScan/1.0 (+https://github.com/; donna.hale.eve@gmail.com)");
		httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
		return httpClient;
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
		using (HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Post, "https://appraise.gnf.lt/appraisal.json?market=jita&persist=no"))
		{
			request.Content = new StringContent(body, Encoding.UTF8, "text/plain");
			using HttpResponseMessage response = await _http.SendAsync(request, cancellationToken).ConfigureAwait(continueOnCapturedContext: false);
			response.EnsureSuccessStatusCode();
			using JsonDocument doc = JsonDocument.Parse(await response.Content.ReadAsStringAsync().ConfigureAwait(continueOnCapturedContext: false));
			if (!doc.RootElement.TryGetProperty("appraisal", out var appraisal))
			{
				return result;
			}
			if (!appraisal.TryGetProperty("items", out var items) || items.ValueKind != JsonValueKind.Array)
			{
				return result;
			}
			foreach (JsonElement item in items.EnumerateArray())
			{
				string name2 = GetString(item, "typeName") ?? GetString(item, "name");
				if (!string.IsNullOrEmpty(name2) && item.TryGetProperty("prices", out var prices))
				{
					double sellMin = GetNestedDouble(prices, "sell", "min");
					double buyMax = GetNestedDouble(prices, "buy", "max");
					if (!(sellMin <= 0.0) || !(buyMax <= 0.0))
					{
						result[name2] = 0.5 * (sellMin + buyMax);
						prices = default(JsonElement);
					}
				}
			}
		}
		return result;
	}

	private static string GetString(JsonElement el, string property)
	{
		if (!el.TryGetProperty(property, out var value) || value.ValueKind != JsonValueKind.String)
		{
			return null;
		}
		return value.GetString();
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
