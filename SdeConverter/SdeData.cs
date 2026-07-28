using System.Text.Json;
using System.Text.RegularExpressions;

namespace SdeConverter;

sealed class TypeInfo
{
    public required int TypeId { get; init; }
    public required string Name { get; init; }
    public required int GroupId { get; init; }
    public int? MarketGroupId { get; init; }
    public required bool Published { get; init; }
}

sealed class MarketGroupInfo
{
    public required int MarketGroupId { get; init; }
    public required string NameEn { get; init; }
    public required bool HasTypes { get; init; }
    public int? ParentGroupId { get; init; }
}

sealed class BonusEntry
{
    public required int SkillId { get; init; }
    public required float Bonus { get; init; }
    public required string BonusText { get; init; }
}

sealed class SdeData
{
    static readonly Regex HtmlTagRegex = new("<[^>]+>", RegexOptions.Compiled);

    static readonly HashSet<int> SlotEffectIds = [(int)ModuleSlot.LowPower, (int)ModuleSlot.HighPower, (int)ModuleSlot.MediumPower, (int)ModuleSlot.Rig, (int)ModuleSlot.Subsystem];

    static readonly HashSet<int> ShipAttributeIds = Enum.GetValues<ShipAttributes>().Select(a => (int)a).ToHashSet();
    static readonly HashSet<int> ModuleAttributeIds = Enum.GetValues<ModuleAttributesDb>().Select(a => (int)a).ToHashSet();

    public required IReadOnlyDictionary<int, MarketGroupInfo> MarketGroups { get; init; }
    public required IReadOnlyDictionary<int, TypeInfo> TypesById { get; init; }
    public required IReadOnlyDictionary<string, int> TypeIdsByName { get; init; }
    public required IReadOnlyDictionary<int, Dictionary<int, float>> AttributesByType { get; init; }
    public required IReadOnlyDictionary<int, HashSet<int>> EffectIdsByType { get; init; }
    public required IReadOnlyDictionary<int, List<BonusEntry>> BonusesByType { get; init; }
    public required IReadOnlyList<int> ShipMarketGroupIdsWithTypes { get; init; }
    public required int BuildNumber { get; init; }
    public required string ReleaseDate { get; init; }

    public static SdeData Load(SdeCache cache)
    {
        var (build, release) = cache.ReadBuildInfo();
        Console.WriteLine($"SDE buildNumber={build} releaseDate={release}");

        Console.WriteLine("Loading marketGroups.jsonl ...");
        var marketGroups = LoadMarketGroups(Path.Combine(cache.ExtractDir, "marketGroups.jsonl"));
        var shipMarketGroups = CollectShipMarketGroups(marketGroups);
        Console.WriteLine($"Ship market groups with types: {shipMarketGroups.Count}");

        Console.WriteLine("Loading types.jsonl (published only) ...");
        var (typesById, typeIdsByName) = LoadTypes(Path.Combine(cache.ExtractDir, "types.jsonl"));
        Console.WriteLine($"Published types: {typesById.Count}");

        Console.WriteLine("Loading typeDogma.jsonl ...");
        var (attributesByType, effectIdsByType) = LoadTypeDogma(Path.Combine(cache.ExtractDir, "typeDogma.jsonl"), typesById);
        Console.WriteLine($"Types with dogma attributes: {attributesByType.Count}");

        Console.WriteLine("Loading typeBonus.jsonl ...");
        var bonusesByType = LoadTypeBonus(Path.Combine(cache.ExtractDir, "typeBonus.jsonl"));
        Console.WriteLine($"Types with bonuses: {bonusesByType.Count}");

        return new SdeData
        {
            MarketGroups = marketGroups,
            TypesById = typesById,
            TypeIdsByName = typeIdsByName,
            AttributesByType = attributesByType,
            EffectIdsByType = effectIdsByType,
            BonusesByType = bonusesByType,
            ShipMarketGroupIdsWithTypes = shipMarketGroups,
            BuildNumber = build,
            ReleaseDate = release,
        };
    }

    static Dictionary<int, MarketGroupInfo> LoadMarketGroups(string path)
    {
        var result = new Dictionary<int, MarketGroupInfo>();
        foreach (var el in Jsonl.ReadObjects(path))
        {
            var id = Jsonl.GetInt(el, "_key");
            var name = Jsonl.GetEn(el, "name") ?? "";
            result[id] = new MarketGroupInfo
            {
                MarketGroupId = id,
                NameEn = name,
                HasTypes = Jsonl.GetBool(el, "hasTypes"),
                ParentGroupId = Jsonl.GetNullableInt(el, "parentGroupID"),
            };
        }
        return result;
    }

    static List<int> CollectShipMarketGroups(IReadOnlyDictionary<int, MarketGroupInfo> marketGroups)
    {
        var root = marketGroups.Values.FirstOrDefault(g =>
            string.Equals(g.NameEn, "Ships", StringComparison.Ordinal) && g.ParentGroupId is null)
            ?? throw new InvalidOperationException("Root market group 'Ships' with null parentGroupID not found.");

        var result = new List<int>();
        CollectRecursive(root.MarketGroupId);
        return result;

        void CollectRecursive(int marketGroupId)
        {
            foreach (var child in marketGroups.Values.Where(g => g.ParentGroupId == marketGroupId).OrderBy(g => g.MarketGroupId))
            {
                if (child.HasTypes)
                    result.Add(child.MarketGroupId);
                CollectRecursive(child.MarketGroupId);
            }
        }
    }

    static (Dictionary<int, TypeInfo> ById, Dictionary<string, int> ByName) LoadTypes(string path)
    {
        var byId = new Dictionary<int, TypeInfo>();
        var byName = new Dictionary<string, int>(StringComparer.Ordinal);
        foreach (var el in Jsonl.ReadObjects(path))
        {
            if (!Jsonl.GetBool(el, "published"))
                continue;

            var id = Jsonl.GetInt(el, "_key");
            var name = Jsonl.GetEn(el, "name") ?? "";
            var info = new TypeInfo
            {
                TypeId = id,
                Name = name,
                GroupId = Jsonl.GetInt(el, "groupID"),
                MarketGroupId = Jsonl.GetNullableInt(el, "marketGroupID"),
                Published = true,
            };
            byId[id] = info;
            byName[name] = id;
        }
        return (byId, byName);
    }

    static (Dictionary<int, Dictionary<int, float>> Attrs, Dictionary<int, HashSet<int>> Effects) LoadTypeDogma(
        string path,
        IReadOnlyDictionary<int, TypeInfo> publishedTypes)
    {
        var attrs = new Dictionary<int, Dictionary<int, float>>();
        var effects = new Dictionary<int, HashSet<int>>();

        foreach (var el in Jsonl.ReadObjects(path))
        {
            var typeId = Jsonl.GetInt(el, "_key");
            if (!publishedTypes.ContainsKey(typeId))
                continue;

            if (el.TryGetProperty("dogmaAttributes", out var dogmaAttrs) && dogmaAttrs.ValueKind == JsonValueKind.Array)
            {
                Dictionary<int, float>? interesting = null;
                foreach (var attr in dogmaAttrs.EnumerateArray())
                {
                    var attributeId = Jsonl.GetInt(attr, "attributeID");
                    if (!ShipAttributeIds.Contains(attributeId) && !ModuleAttributeIds.Contains(attributeId))
                        continue;
                    interesting ??= new Dictionary<int, float>();
                    interesting[attributeId] = Jsonl.GetFloat(attr, "value");
                }
                if (interesting != null)
                    attrs[typeId] = interesting;
            }

            if (el.TryGetProperty("dogmaEffects", out var dogmaEffects) && dogmaEffects.ValueKind == JsonValueKind.Array)
            {
                HashSet<int>? slotEffects = null;
                foreach (var effect in dogmaEffects.EnumerateArray())
                {
                    var effectId = Jsonl.GetInt(effect, "effectID");
                    if (!SlotEffectIds.Contains(effectId))
                        continue;
                    slotEffects ??= new HashSet<int>();
                    slotEffects.Add(effectId);
                }
                if (slotEffects != null)
                    effects[typeId] = slotEffects;
            }
        }

        return (attrs, effects);
    }

    static Dictionary<int, List<BonusEntry>> LoadTypeBonus(string path)
    {
        var result = new Dictionary<int, List<BonusEntry>>();
        foreach (var el in Jsonl.ReadObjects(path))
        {
            var typeId = Jsonl.GetInt(el, "_key");
            var list = new List<BonusEntry>();

            if (el.TryGetProperty("roleBonuses", out var roleBonuses) && roleBonuses.ValueKind == JsonValueKind.Array)
            {
                foreach (var bonus in roleBonuses.EnumerateArray())
                    TryAddBonus(list, skillId: -1, bonus);
            }

            if (el.TryGetProperty("types", out var types) && types.ValueKind == JsonValueKind.Array)
            {
                foreach (var skillEntry in types.EnumerateArray())
                {
                    var skillId = Jsonl.GetInt(skillEntry, "_key");
                    if (!skillEntry.TryGetProperty("_value", out var values) || values.ValueKind != JsonValueKind.Array)
                        continue;
                    foreach (var bonus in values.EnumerateArray())
                        TryAddBonus(list, skillId, bonus);
                }
            }

            if (list.Count > 0)
                result[typeId] = list;
        }
        return result;
    }

    static void TryAddBonus(List<BonusEntry> list, int skillId, JsonElement bonus)
    {
        var text = StripHtml(Jsonl.GetEn(bonus, "bonusText") ?? "");
        float value = 0f;
        if (bonus.TryGetProperty("bonus", out var b) && b.ValueKind == JsonValueKind.Number)
            value = b.GetSingle();
        list.Add(new BonusEntry { SkillId = skillId, Bonus = value, BonusText = text });
    }

    public static string StripHtml(string text) => HtmlTagRegex.Replace(text, string.Empty);

    public int GetMarketGroupId(string[] groupPath)
    {
        if (groupPath.Length == 0)
            throw new ArgumentException("Group path required.", nameof(groupPath));

        var current = MarketGroups.Values.FirstOrDefault(g =>
            string.Equals(g.NameEn, groupPath[0], StringComparison.Ordinal) && g.ParentGroupId is null)
            ?? throw new InvalidOperationException($"Root market group '{groupPath[0]}' not found.");

        for (var i = 1; i < groupPath.Length; i++)
        {
            var fragment = groupPath[i];
            var next = MarketGroups.Values.FirstOrDefault(g =>
                g.ParentGroupId == current.MarketGroupId &&
                g.NameEn.Contains(fragment, StringComparison.Ordinal))
                ?? throw new InvalidOperationException($"Market group containing '{fragment}' under '{current.NameEn}' not found.");
            current = next;
        }

        return current.MarketGroupId;
    }

    public float GetMaxAttributeValueForMarketGroup(string[] groupPath, ModuleAttributesDb attribute)
    {
        var marketGroupId = GetMarketGroupId(groupPath);
        var attrId = (int)attribute;
        float max = 0f;
        foreach (var type in TypesById.Values)
        {
            if (type.MarketGroupId != marketGroupId)
                continue;
            if (!AttributesByType.TryGetValue(type.TypeId, out var attrs))
                continue;
            if (!attrs.TryGetValue(attrId, out var value))
                continue;
            if (value > max)
                max = value;
        }
        return max;
    }

    public int GetTypeIdByName(string name)
    {
        if (TypeIdsByName.TryGetValue(name, out var id))
            return id;
        throw new InvalidOperationException($"Type not found by name: {name}");
    }

    public Dictionary<int, float> GetAttributes(int typeId)
    {
        return AttributesByType.TryGetValue(typeId, out var attrs)
            ? attrs
            : new Dictionary<int, float>();
    }

    public IReadOnlyList<BonusEntry> GetBonuses(int typeId)
    {
        return BonusesByType.TryGetValue(typeId, out var list) ? list : Array.Empty<BonusEntry>();
    }
}
