namespace SdeConverter;

sealed class ShipDescription
{
    public required string Name { get; init; }
    public required int TypeId { get; init; }
    public required uint HighSlots { get; init; }
    public required uint MedSlots { get; init; }
    public required uint LowSlots { get; init; }
    public required uint RigSlots { get; init; }
    public required uint SubsystemSlots { get; init; }
    public required float ShieldHp { get; init; }
    public required float ShieldHpMultiplier { get; init; }
    public required float ShieldResistEm { get; init; }
    public required float ShieldResistThermal { get; init; }
    public required float ShieldResistKinetic { get; init; }
    public required float ShieldResistExplosive { get; init; }
    public required float ArmorHp { get; init; }
    public required float ArmorHpMultiplier { get; init; }
    public required float ArmorResistEm { get; init; }
    public required float ArmorResistThermal { get; init; }
    public required float ArmorResistKinetic { get; init; }
    public required float ArmorResistExplosive { get; init; }
    public required float HullHp { get; init; }
    public required float HullHpMultiplier { get; init; }
    public required float HullResistEm { get; init; }
    public required float HullResistThermal { get; init; }
    public required float HullResistKinetic { get; init; }
    public required float HullResistExplosive { get; init; }
    public required float OverheatingBonus { get; init; }
}

static class ShipCatalog
{
    public static List<ShipDescription> Build(SdeData data)
    {
        var ships = new List<(string Name, int TypeId)>();
        var shipGroupSet = data.ShipMarketGroupIdsWithTypes.ToHashSet();

        foreach (var marketGroupId in data.ShipMarketGroupIdsWithTypes)
        {
            var inGroup = data.TypesById.Values
                .Where(t => t.MarketGroupId == marketGroupId)
                .OrderBy(t => t.TypeId);
            foreach (var type in inGroup)
                ships.Add((type.Name, type.TypeId));
        }

        // Also include any published ship-group types missed if market groups changed shape
        _ = shipGroupSet;

        Console.WriteLine($"got {ships.Count} ships");
        var result = new List<ShipDescription>(ships.Count);
        foreach (var (name, typeId) in ships)
            result.Add(GetShipDescription(data, name, typeId));
        return result;
    }

    static ShipDescription GetShipDescription(SdeData data, string shipName, int typeId)
    {
        var shipAttributes = GetShipAttributes(data, typeId);
        var shipTraits = GetShipTraits(data, typeId);

        if (shipTraits.ContainsKey(ShipTraits.ShieldResistsPerLevel) && shipTraits.ContainsKey(ShipTraits.ShieldResistsRole))
            throw new InvalidOperationException($"Ship {shipName} has both per-level and role shield resist traits.");
        float traitShieldResonance = 1.0f;
        if (shipTraits.TryGetValue(ShipTraits.ShieldResistsPerLevel, out var traitShieldResists))
            traitShieldResonance = 1.0f - traitShieldResists * 5.0f * 0.01f;
        else if (shipTraits.TryGetValue(ShipTraits.ShieldResistsRole, out traitShieldResists))
            traitShieldResonance = 1.0f - traitShieldResists * 0.01f;

        if (shipTraits.ContainsKey(ShipTraits.ArmorResistsPerLevel) && shipTraits.ContainsKey(ShipTraits.ArmorResistsRole))
            throw new InvalidOperationException($"Ship {shipName} has both per-level and role armor resist traits.");
        float traitArmorResonance = 1.0f;
        if (shipTraits.TryGetValue(ShipTraits.ArmorResistsPerLevel, out var traitArmorResists))
            traitArmorResonance = 1.0f - traitArmorResists * 5.0f * 0.01f;
        else if (shipTraits.TryGetValue(ShipTraits.ArmorResistsRole, out traitArmorResists))
            traitArmorResonance = 1.0f - traitArmorResists * 0.01f;

        shipTraits.TryGetValue(ShipTraits.ShieldHpPercentPerLevel, out var traitShieldHp);
        float traitShieldHpMultiplier = 1.0f + traitShieldHp * 5.0f * 0.01f;

        shipTraits.TryGetValue(ShipTraits.ArmorHpPercentPerLevel, out var traitArmorHp);
        float traitArmorHpMultiplier = 1.0f + traitArmorHp * 5.0f * 0.01f;

        shipTraits.TryGetValue(ShipTraits.HullHpPercentPerLevel, out var traitHullHp);
        float traitHullHpMultiplier = 1.0f + traitHullHp * 5.0f * 0.01f;

        shipTraits.TryGetValue(ShipTraits.OverheatingBonusPercent, out var traitOverheatingBonusPercent);
        float traitOverheatingBonus = traitOverheatingBonusPercent * 0.01f;

        uint subsystemSlots = 0;
        if (shipAttributes.TryGetValue(ShipAttributes.TechLevel, out var techLevel) &&
            shipAttributes.TryGetValue(ShipAttributes.SubsystemHoldCapacity, out var subsystemHoldCapacity) &&
            techLevel == 3.0f && subsystemHoldCapacity > 0.0f)
        {
            subsystemSlots = 4;
        }

        return new ShipDescription
        {
            Name = shipName,
            TypeId = typeId,
            HighSlots = (uint)shipAttributes[ShipAttributes.HighSlots],
            MedSlots = (uint)shipAttributes[ShipAttributes.MedSlots],
            LowSlots = (uint)shipAttributes[ShipAttributes.LowSlots],
            RigSlots = (uint)shipAttributes[ShipAttributes.RigSlots],
            SubsystemSlots = subsystemSlots,
            ShieldHp = shipAttributes[ShipAttributes.ShieldHp],
            ShieldHpMultiplier = traitShieldHpMultiplier,
            ShieldResistEm = 1.0f - shipAttributes[ShipAttributes.ShieldEmResonance] * traitShieldResonance,
            ShieldResistThermal = 1.0f - shipAttributes[ShipAttributes.ShieldThermResonance] * traitShieldResonance,
            ShieldResistKinetic = 1.0f - shipAttributes[ShipAttributes.ShieldKinResonance] * traitShieldResonance,
            ShieldResistExplosive = 1.0f - shipAttributes[ShipAttributes.ShieldExplResonance] * traitShieldResonance,
            ArmorHp = shipAttributes[ShipAttributes.ArmorHp],
            ArmorHpMultiplier = traitArmorHpMultiplier,
            ArmorResistEm = 1.0f - shipAttributes[ShipAttributes.ArmorEmResonance] * traitArmorResonance,
            ArmorResistThermal = 1.0f - shipAttributes[ShipAttributes.ArmorThermResonance] * traitArmorResonance,
            ArmorResistKinetic = 1.0f - shipAttributes[ShipAttributes.ArmorKinResonance] * traitArmorResonance,
            ArmorResistExplosive = 1.0f - shipAttributes[ShipAttributes.ArmorExplResonance] * traitArmorResonance,
            HullHp = shipAttributes[ShipAttributes.HullHp],
            HullHpMultiplier = traitHullHpMultiplier,
            HullResistEm = 1.0f - shipAttributes[ShipAttributes.HullEmResonance],
            HullResistThermal = 1.0f - shipAttributes[ShipAttributes.HullThermResonance],
            HullResistKinetic = 1.0f - shipAttributes[ShipAttributes.HullKinResonance],
            HullResistExplosive = 1.0f - shipAttributes[ShipAttributes.HullExplResonance],
            OverheatingBonus = traitOverheatingBonus,
        };
    }

    static Dictionary<ShipAttributes, float> GetShipAttributes(SdeData data, int typeId)
    {
        var shipAttributes = new Dictionary<ShipAttributes, float>();
        foreach (ShipAttributes attr in Enum.GetValues<ShipAttributes>())
            shipAttributes[attr] = 0.0f;

        foreach (var (attributeId, value) in data.GetAttributes(typeId))
        {
            if (Enum.IsDefined(typeof(ShipAttributes), attributeId))
                shipAttributes[(ShipAttributes)attributeId] = value;
        }
        return shipAttributes;
    }

    static Dictionary<ShipTraits, float> GetShipTraits(SdeData data, int typeId)
    {
        var shipTraits = new Dictionary<ShipTraits, float>();
        foreach (var bonus in data.GetBonuses(typeId))
        {
            var bonusText = bonus.BonusText;
            var skillId = bonus.SkillId;
            var value = bonus.Bonus;

            if (string.Equals(bonusText, "bonus to ship shield hitpoints", StringComparison.OrdinalIgnoreCase))
            {
                shipTraits[ShipTraits.ShieldHpPercentPerLevel] = value;
            }
            else if (string.Equals(bonusText, "bonus to ship armor hitpoints", StringComparison.OrdinalIgnoreCase))
            {
                shipTraits[ShipTraits.ArmorHpPercentPerLevel] = value;
            }
            else if (string.Equals(bonusText, "bonus to ship shield and hull hitpoints", StringComparison.OrdinalIgnoreCase))
            {
                shipTraits[ShipTraits.ShieldHpPercentPerLevel] = value;
                shipTraits[ShipTraits.HullHpPercentPerLevel] = value;
            }
            else if (string.Equals(bonusText, "bonus to ship armor and hull hitpoints", StringComparison.OrdinalIgnoreCase))
            {
                shipTraits[ShipTraits.ArmorHpPercentPerLevel] = value;
                shipTraits[ShipTraits.HullHpPercentPerLevel] = value;
            }
            else if (string.Equals(bonusText, "bonus to all shield resistances", StringComparison.OrdinalIgnoreCase))
            {
                if (skillId > 0)
                    shipTraits[ShipTraits.ShieldResistsPerLevel] = value;
                else
                    shipTraits[ShipTraits.ShieldResistsRole] = value;
            }
            else if (string.Equals(bonusText, "bonus to all armor resistances", StringComparison.OrdinalIgnoreCase))
            {
                if (skillId > 0)
                    shipTraits[ShipTraits.ArmorResistsPerLevel] = value;
                else
                    shipTraits[ShipTraits.ArmorResistsRole] = value;
            }
            else if (bonusText.StartsWith("bonus to the benefits of overheating ", StringComparison.OrdinalIgnoreCase))
            {
                shipTraits[ShipTraits.OverheatingBonusPercent] = value;
            }
        }
        return shipTraits;
    }
}
