using System.Text.Json;

namespace SdeConverter;

static class Jsonl
{
    public static IEnumerable<JsonElement> ReadObjects(string path)
    {
        using var reader = new StreamReader(path);
        string? line;
        while ((line = reader.ReadLine()) != null)
        {
            if (string.IsNullOrWhiteSpace(line))
                continue;
            using var doc = JsonDocument.Parse(line);
            yield return doc.RootElement.Clone();
        }
    }

    public static string? GetEn(JsonElement element, string propertyName)
    {
        if (!element.TryGetProperty(propertyName, out var prop))
            return null;
        if (prop.ValueKind == JsonValueKind.Object && prop.TryGetProperty("en", out var en))
            return en.GetString();
        if (prop.ValueKind == JsonValueKind.String)
            return prop.GetString();
        return null;
    }

    public static int GetInt(JsonElement element, string name, int defaultValue = 0)
    {
        if (!element.TryGetProperty(name, out var prop))
            return defaultValue;
        return prop.ValueKind switch
        {
            JsonValueKind.Number => prop.GetInt32(),
            JsonValueKind.String => int.TryParse(prop.GetString(), out var i) ? i : defaultValue,
            _ => defaultValue,
        };
    }

    public static int? GetNullableInt(JsonElement element, string name)
    {
        if (!element.TryGetProperty(name, out var prop) || prop.ValueKind == JsonValueKind.Null)
            return null;
        return prop.ValueKind switch
        {
            JsonValueKind.Number => prop.GetInt32(),
            JsonValueKind.String => int.TryParse(prop.GetString(), out var i) ? i : null,
            _ => null,
        };
    }

    public static bool GetBool(JsonElement element, string name, bool defaultValue = false)
    {
        if (!element.TryGetProperty(name, out var prop))
            return defaultValue;
        return prop.ValueKind switch
        {
            JsonValueKind.True => true,
            JsonValueKind.False => false,
            _ => defaultValue,
        };
    }

    public static float GetFloat(JsonElement element, string name, float defaultValue = 0f)
    {
        if (!element.TryGetProperty(name, out var prop) || prop.ValueKind != JsonValueKind.Number)
            return defaultValue;
        return prop.GetSingle();
    }
}
