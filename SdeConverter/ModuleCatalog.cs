namespace SdeConverter;

sealed class ModuleDescription
{
    public required string Name { get; init; }
    public required int TypeId { get; init; }
    public required ModuleSlot Slot { get; init; }
    public required Dictionary<ModuleAttributes, Dictionary<ModuleActive, (float Value, int StackGroup)>> Attributes { get; init; }
    public required float OverloadBonus { get; init; }
    public required int ShipTypeId { get; init; }
}

static class ModuleCatalog
{
    public static List<string> Warnings { get; } = new();

    public static List<ModuleDescription> Build(SdeData data)
    {
        Warnings.Clear();
        var modules = GetModules(data);
        Console.WriteLine($"got {modules.Count} modules");

        var abyssalModules = CreateAbyssalModules(data);
        var result = new List<ModuleDescription>(modules.Count);
        foreach (var module in modules)
        {
            if (abyssalModules.TryGetValue(module.TypeId, out var abyssal))
                result.Add(abyssal);
            else
                result.Add(GetModuleDescription(data, module.Name, module.TypeId, module.GroupId, module.Slot));
        }
        return result;
    }

    sealed record ModuleRef(string Name, int TypeId, int GroupId, ModuleSlot Slot);

    static List<ModuleRef> GetModules(SdeData data)
    {
        var modules = new List<ModuleRef>();
        foreach (var (typeId, effectIds) in data.EffectIdsByType.OrderBy(kv => kv.Key))
        {
            if (!data.TypesById.TryGetValue(typeId, out var type))
                continue;
            if (type.Name.StartsWith("Standup ", StringComparison.Ordinal))
                continue;

            foreach (var effectId in effectIds.OrderBy(id => id))
            {
                if (!Enum.IsDefined(typeof(ModuleSlot), effectId))
                    continue;
                modules.Add(new ModuleRef(type.Name, typeId, type.GroupId, (ModuleSlot)effectId));
            }
        }
        return modules;
    }

    static Dictionary<int, ModuleDescription> CreateAbyssalModules(SdeData data)
    {
        var result = new Dictionary<int, ModuleDescription>();
        void Add(string name, string[] groups, ModuleAttributesDb dbAttr, ModuleAttributes attr, ModuleSlot slot)
        {
            var md = CreateAbyssalModule(data, name, groups, dbAttr, 1.3f, attr, slot);
            result[md.TypeId] = md;
        }

        Add("Small Abyssal Shield Extender", ["Ship Equipment", "Shield", "Shield Extenders", "Small"], ModuleAttributesDb.CapacityBonus, ModuleAttributes.ShieldBonusAdd, ModuleSlot.MediumPower);
        Add("Medium Abyssal Shield Extender", ["Ship Equipment", "Shield", "Shield Extenders", "Medium"], ModuleAttributesDb.CapacityBonus, ModuleAttributes.ShieldBonusAdd, ModuleSlot.MediumPower);
        Add("Large Abyssal Shield Extender", ["Ship Equipment", "Shield", "Shield Extenders", "Large"], ModuleAttributesDb.CapacityBonus, ModuleAttributes.ShieldBonusAdd, ModuleSlot.MediumPower);

        Add("Small Abyssal Armor Plates", ["Ship Equipment", "Armor", "Armor Plates", "200mm Armor Plate"], ModuleAttributesDb.ArmorHpBonusAdd, ModuleAttributes.ArmorBonusAdd, ModuleSlot.LowPower);
        Add("Medium Abyssal Armor Plates", ["Ship Equipment", "Armor", "Armor Plates", "800mm Armor Plate"], ModuleAttributesDb.ArmorHpBonusAdd, ModuleAttributes.ArmorBonusAdd, ModuleSlot.LowPower);
        Add("Large Abyssal Armor Plates", ["Ship Equipment", "Armor", "Armor Plates", "1600mm Armor Plate"], ModuleAttributesDb.ArmorHpBonusAdd, ModuleAttributes.ArmorBonusAdd, ModuleSlot.LowPower);

        return result;
    }

    static ModuleDescription CreateAbyssalModule(
        SdeData data,
        string moduleName,
        string[] marketGroups,
        ModuleAttributesDb dbAttribute,
        float bonus,
        ModuleAttributes attribute,
        ModuleSlot slot)
    {
        var typeId = data.GetTypeIdByName(moduleName);
        var value = data.GetMaxAttributeValueForMarketGroup(marketGroups, dbAttribute);
        var attributes = new Dictionary<ModuleAttributes, Dictionary<ModuleActive, (float, int)>>
        {
            [attribute] = new Dictionary<ModuleActive, (float, int)>
            {
                [ModuleActive.Passive] = (value * bonus, 1),
            },
        };
        return new ModuleDescription
        {
            Name = moduleName,
            TypeId = typeId,
            Slot = slot,
            Attributes = attributes,
            OverloadBonus = 0.0f,
            ShipTypeId = -1,
        };
    }

    static ModuleActive GetActive(bool isOmni, bool isAdc, bool isActive)
    {
        if (isOmni)
            return ModuleActive.AssaultActive;
        if (isAdc)
            return ModuleActive.AssaultPassive;
        return isActive ? ModuleActive.Active : ModuleActive.Passive;
    }

    static ModuleDescription GetModuleDescription(SdeData data, string moduleName, int typeId, int groupId, ModuleSlot slot)
    {
        var moduleAttributesDb = data.GetAttributes(typeId);
        var attributes = new Dictionary<ModuleAttributes, Dictionary<ModuleActive, (float Value, int StackGroup)>>();
        float overloadBonus = 0.0f;
        int shipTypeId = -1;

        bool bActiveModule = false;
        if (moduleAttributesDb.TryGetValue((int)ModuleAttributesDb.CapacitorNeeded, out var capNeeded))
            bActiveModule = capNeeded > 0.0f;
        if (!bActiveModule && moduleAttributesDb.TryGetValue((int)ModuleAttributesDb.Duration, out var duration))
            bActiveModule = duration > 0.0f;

        bool isAdc = moduleAttributesDb.ContainsKey((int)ModuleAttributesDb.AllResonances);

        foreach (var (attrId, attrDbValue) in moduleAttributesDb)
        {
            if (!Enum.IsDefined(typeof(ModuleAttributesDb), attrId))
                continue;
            var attrDb = (ModuleAttributesDb)attrId;

            switch (attrDb)
            {
                case ModuleAttributesDb.Polarized:
                    AddTo(attributes, ModuleAttributes.Polarized, GetActive(false, false, false), 1.0f, 1);
                    break;

                case ModuleAttributesDb.ShieldEmResonance:
                    AddTo(attributes, ModuleAttributes.ShieldEmResist, GetActive(false, isAdc, bActiveModule), 1.0f - attrDbValue, GetStackingGroup(groupId));
                    break;
                case ModuleAttributesDb.ShieldThermalResonance:
                    AddTo(attributes, ModuleAttributes.ShieldThermalResist, GetActive(false, isAdc, bActiveModule), 1.0f - attrDbValue, GetStackingGroup(groupId));
                    break;
                case ModuleAttributesDb.ShieldKineticResonance:
                    AddTo(attributes, ModuleAttributes.ShieldKineticResist, GetActive(false, isAdc, bActiveModule), 1.0f - attrDbValue, GetStackingGroup(groupId));
                    break;
                case ModuleAttributesDb.ShieldExplosiveResonance:
                    AddTo(attributes, ModuleAttributes.ShieldExplosiveResist, GetActive(false, isAdc, bActiveModule), 1.0f - attrDbValue, GetStackingGroup(groupId));
                    break;

                case ModuleAttributesDb.ArmorEmResonance:
                    AddTo(attributes, ModuleAttributes.ArmorEmResist, GetActive(false, isAdc, bActiveModule), 1.0f - attrDbValue, GetStackingGroup(groupId));
                    break;
                case ModuleAttributesDb.ArmorThermalResonance:
                    AddTo(attributes, ModuleAttributes.ArmorThermalResist, GetActive(false, isAdc, bActiveModule), 1.0f - attrDbValue, GetStackingGroup(groupId));
                    break;
                case ModuleAttributesDb.ArmorKineticResonance:
                    AddTo(attributes, ModuleAttributes.ArmorKineticResist, GetActive(false, isAdc, bActiveModule), 1.0f - attrDbValue, GetStackingGroup(groupId));
                    break;
                case ModuleAttributesDb.ArmorExplosiveResonance:
                    AddTo(attributes, ModuleAttributes.ArmorExplosiveResist, GetActive(false, isAdc, bActiveModule), 1.0f - attrDbValue, GetStackingGroup(groupId));
                    break;

                case ModuleAttributesDb.HullEmResonance:
                    AddTo(attributes, ModuleAttributes.HullEmResist, GetActive(false, isAdc, bActiveModule), 1.0f - attrDbValue, GetStackingGroup(groupId));
                    break;
                case ModuleAttributesDb.HullThermalResonance:
                    AddTo(attributes, ModuleAttributes.HullThermalResist, GetActive(false, isAdc, bActiveModule), 1.0f - attrDbValue, GetStackingGroup(groupId));
                    break;
                case ModuleAttributesDb.HullKineticResonance:
                    AddTo(attributes, ModuleAttributes.HullKineticResist, GetActive(false, isAdc, bActiveModule), 1.0f - attrDbValue, GetStackingGroup(groupId));
                    break;
                case ModuleAttributesDb.HullExplosiveResonance:
                    AddTo(attributes, ModuleAttributes.HullExplosiveResist, GetActive(false, isAdc, bActiveModule), 1.0f - attrDbValue, GetStackingGroup(groupId));
                    break;

                case ModuleAttributesDb.EmResistBonus:
                    HandleResistBonus(attributes, moduleName, groupId, bActiveModule, attrDbValue, "EM",
                        ModuleAttributes.ShieldEmResist, ModuleAttributes.ArmorEmResist);
                    break;
                case ModuleAttributesDb.ThermalResistBonus:
                    HandleResistBonus(attributes, moduleName, groupId, bActiveModule, attrDbValue, "THERMAL",
                        ModuleAttributes.ShieldThermalResist, ModuleAttributes.ArmorThermalResist);
                    break;
                case ModuleAttributesDb.KineticResistBonus:
                    HandleResistBonus(attributes, moduleName, groupId, bActiveModule, attrDbValue, "KINETIC",
                        ModuleAttributes.ShieldKineticResist, ModuleAttributes.ArmorKineticResist);
                    break;
                case ModuleAttributesDb.ExplosiveResistBonus:
                    HandleResistBonus(attributes, moduleName, groupId, bActiveModule, attrDbValue, "EM",
                        ModuleAttributes.ShieldExplosiveResist, ModuleAttributes.ArmorExplosiveResist);
                    break;

                case ModuleAttributesDb.AllResonances:
                {
                    var active = GetActive(true, true, true);
                    float resist = 1.0f - attrDbValue;
                    int stackingGroup = GetStackingGroup(groupId);
                    AddTo(attributes, ModuleAttributes.ShieldEmResist, active, resist, stackingGroup);
                    AddTo(attributes, ModuleAttributes.ShieldThermalResist, active, resist, stackingGroup);
                    AddTo(attributes, ModuleAttributes.ShieldKineticResist, active, resist, stackingGroup);
                    AddTo(attributes, ModuleAttributes.ShieldExplosiveResist, active, resist, stackingGroup);
                    AddTo(attributes, ModuleAttributes.ArmorEmResist, active, resist, stackingGroup);
                    AddTo(attributes, ModuleAttributes.ArmorThermalResist, active, resist, stackingGroup);
                    AddTo(attributes, ModuleAttributes.ArmorKineticResist, active, resist, stackingGroup);
                    AddTo(attributes, ModuleAttributes.ArmorExplosiveResist, active, resist, stackingGroup);
                    AddTo(attributes, ModuleAttributes.HullEmResist, active, resist, stackingGroup);
                    AddTo(attributes, ModuleAttributes.HullThermalResist, active, resist, stackingGroup);
                    AddTo(attributes, ModuleAttributes.HullKineticResist, active, resist, stackingGroup);
                    AddTo(attributes, ModuleAttributes.HullExplosiveResist, active, resist, stackingGroup);
                    break;
                }

                case ModuleAttributesDb.OverloadHardeningBonus:
                    if (attrDbValue > 1.0f)
                        overloadBonus = attrDbValue * 0.01f;
                    break;

                case ModuleAttributesDb.ShieldCapacityMultiplier:
                {
                    float bonus = Math.Abs(attrDbValue - 1.0f);
                    if (bonus > 0.01f)
                    {
                        attributes.Add(ModuleAttributes.ShieldBonusMultiply,
                            new Dictionary<ModuleActive, (float, int)> { { GetActive(false, false, false), (attrDbValue, 1) } });
                    }
                    break;
                }

                case ModuleAttributesDb.ArmorHpMultiplier:
                {
                    float bonus = Math.Abs(attrDbValue - 1.0f);
                    if (bonus > 0.01f)
                    {
                        attributes.Add(ModuleAttributes.ArmorBonusMultiply,
                            new Dictionary<ModuleActive, (float, int)> { { GetActive(false, false, false), (attrDbValue, 1) } });
                    }
                    break;
                }

                case ModuleAttributesDb.StructureHpMultiplier:
                {
                    float bonus = Math.Abs(attrDbValue - 1.0f);
                    if (bonus > 0.01f)
                    {
                        attributes.Add(ModuleAttributes.HullBonusMultiply,
                            new Dictionary<ModuleActive, (float, int)> { { GetActive(false, false, false), (attrDbValue, 1) } });
                    }
                    break;
                }

                case ModuleAttributesDb.ArmorHpBonusAdd:
                {
                    float bonus = Math.Abs(attrDbValue);
                    if (bonus > 0.01f)
                    {
                        attributes.Add(ModuleAttributes.ArmorBonusAdd,
                            new Dictionary<ModuleActive, (float, int)> { { GetActive(false, false, false), (attrDbValue, 1) } });
                    }
                    break;
                }

                case ModuleAttributesDb.CapacityBonus:
                case ModuleAttributesDb.ShieldCapacity:
                {
                    float bonus = Math.Abs(attrDbValue);
                    if (bonus > 0.01f)
                    {
                        attributes.Add(ModuleAttributes.ShieldBonusAdd,
                            new Dictionary<ModuleActive, (float, int)> { { GetActive(false, false, false), (attrDbValue, 1) } });
                    }
                    break;
                }

                case ModuleAttributesDb.StructureHpBonusAdd:
                {
                    float bonus = Math.Abs(attrDbValue);
                    if (bonus > 0.01f)
                    {
                        attributes.Add(ModuleAttributes.HullBonusAdd,
                            new Dictionary<ModuleActive, (float, int)> { { GetActive(false, false, false), (attrDbValue, 1) } });
                    }
                    break;
                }

                case ModuleAttributesDb.ShieldCapacityBonus:
                    if (groupId == 774)
                    {
                        float bonus = Math.Abs(attrDbValue);
                        if (bonus > 0.01f)
                        {
                            bonus = 1.0f + 0.01f * bonus;
                            attributes.Add(ModuleAttributes.ShieldBonusMultiply,
                                new Dictionary<ModuleActive, (float, int)> { { GetActive(false, false, false), (bonus, 1) } });
                        }
                    }
                    break;

                case ModuleAttributesDb.ArmorHpBonus:
                    if (groupId == 773)
                    {
                        float bonus = Math.Abs(attrDbValue);
                        if (bonus > 0.01f)
                        {
                            bonus = 1.0f + 0.01f * bonus;
                            attributes.Add(ModuleAttributes.ArmorBonusMultiply,
                                new Dictionary<ModuleActive, (float, int)> { { GetActive(false, false, false), (bonus, 1) } });
                        }
                    }
                    break;

                case ModuleAttributesDb.HullHpBonus:
                    if (groupId == 773)
                    {
                        float bonus = Math.Abs(attrDbValue);
                        if (bonus > 0.01f)
                        {
                            bonus = 1.0f + 0.01f * bonus;
                            attributes.Add(ModuleAttributes.HullBonusMultiply,
                                new Dictionary<ModuleActive, (float, int)> { { GetActive(false, false, false), (bonus, 1) } });
                        }
                    }
                    break;

                case ModuleAttributesDb.Drawback:
                    if (Math.Abs(attrDbValue) > 0.0f)
                    {
                        float drawback = 0.5f * attrDbValue;
                        drawback = 1.0f + 0.01f * drawback;

                        if (moduleName.Contains("Inverted Signal Field Projector") ||
                            moduleName.Contains("Particle Dispersion Augmentor") ||
                            moduleName.Contains("Particle Dispersion Projector") ||
                            moduleName.Contains("Targeting Systems Stabilizer") ||
                            moduleName.Contains("Tracking Diagnostics Subroutines") ||
                            moduleName.Contains("Signal Focusing Kit") ||
                            moduleName.Contains("Ionic Field Projector") ||
                            moduleName.Contains("Targeting System Subcontroller"))
                        {
                            attributes.Add(ModuleAttributes.ShieldBonusMultiply,
                                new Dictionary<ModuleActive, (float, int)> { { GetActive(false, false, false), (drawback, 1) } });
                        }
                        else if (
                            moduleName.Contains("Auxiliary Thrusters") ||
                            moduleName.Contains("Cargohold Optimization") ||
                            moduleName.Contains("Dynamic Fuel Valve") ||
                            moduleName.Contains("Engine Thermal Shielding") ||
                            moduleName.Contains("Low Friction Nozzle Joints") ||
                            moduleName.Contains("Polycarbon Engine Housing"))
                        {
                            attributes.Add(ModuleAttributes.ArmorBonusMultiply,
                                new Dictionary<ModuleActive, (float, int)> { { GetActive(false, false, false), (drawback, 1) } });
                        }
                    }
                    break;

                case ModuleAttributesDb.ShipType:
                    shipTypeId = (int)attrDbValue;
                    break;

                case ModuleAttributesDb.HighSlots:
                    if (attrDbValue > 0.01f)
                    {
                        attributes.Add(ModuleAttributes.HighSlots,
                            new Dictionary<ModuleActive, (float, int)> { { GetActive(false, false, false), (attrDbValue, 1) } });
                    }
                    break;

                case ModuleAttributesDb.MediumSlots:
                    if (attrDbValue > 0.01f)
                    {
                        attributes.Add(ModuleAttributes.MediumSlots,
                            new Dictionary<ModuleActive, (float, int)> { { GetActive(false, false, false), (attrDbValue, 1) } });
                    }
                    break;

                case ModuleAttributesDb.LowSlots:
                    if (attrDbValue > 0.01f)
                    {
                        attributes.Add(ModuleAttributes.LowSlots,
                            new Dictionary<ModuleActive, (float, int)> { { GetActive(false, false, false), (attrDbValue, 1) } });
                    }
                    break;
            }
        }

        if (shipTypeId > 0)
        {
            var moduleTraits = GetModuleTraits(data, typeId);

            if (moduleTraits.TryGetValue(ModuleTraits.ShieldHpPercentPerLevel, out var traitShieldHpPercentPerLevel))
            {
                float bonus = 1.0f + traitShieldHpPercentPerLevel * 0.01f * 5.0f;
                attributes.Add(ModuleAttributes.ShieldBonusMultiply,
                    new Dictionary<ModuleActive, (float, int)> { { GetActive(false, false, false), (bonus, 1) } });
            }

            if (moduleTraits.TryGetValue(ModuleTraits.ArmorHpPercentPerLevel, out var traitArmorHpPercentPerLevel))
            {
                float bonus = 1.0f + traitArmorHpPercentPerLevel * 0.01f * 5.0f;
                attributes.Add(ModuleAttributes.ArmorBonusMultiply,
                    new Dictionary<ModuleActive, (float, int)> { { GetActive(false, false, false), (bonus, 1) } });
            }

            if (moduleTraits.TryGetValue(ModuleTraits.ShieldHardenersOverheatingBonus, out var traitShieldOh))
            {
                float bonus = traitShieldOh * 0.01f * 5.0f;
                attributes.Add(ModuleAttributes.ShieldHardenersOverloadBonus,
                    new Dictionary<ModuleActive, (float, int)> { { GetActive(false, false, false), (bonus, 1) } });
            }

            if (moduleTraits.TryGetValue(ModuleTraits.ArmorHardenersOverheatingBonus, out var traitArmorOh))
            {
                float bonus = traitArmorOh * 0.01f * 5.0f;
                attributes.Add(ModuleAttributes.ArmorHardenersOverloadBonus,
                    new Dictionary<ModuleActive, (float, int)> { { GetActive(false, false, false), (bonus, 1) } });
            }
        }

        return new ModuleDescription
        {
            Name = moduleName,
            TypeId = typeId,
            Slot = slot,
            Attributes = attributes,
            OverloadBonus = overloadBonus,
            ShipTypeId = shipTypeId,
        };
    }

    static void HandleResistBonus(
        Dictionary<ModuleAttributes, Dictionary<ModuleActive, (float, int)>> attributes,
        string moduleName,
        int groupId,
        bool bActiveModule,
        float attrDbValue,
        string label,
        ModuleAttributes shieldAttr,
        ModuleAttributes armorAttr)
    {
        if (attrDbValue >= 0.0f)
            return;

        if (groupId == 77 || groupId == 1700)
            AddResistAttributesWithHeat(attributes, attrDbValue, true, false, shieldAttr);
        else if (groupId == 328 || groupId == 1699)
            AddResistAttributesWithHeat(attributes, attrDbValue, true, false, armorAttr);
        else if (groupId == 295)
            AddResistAttributesWithHeat(attributes, attrDbValue, false, false, shieldAttr);
        else if (groupId == 98 || groupId == 326)
            AddResistAttributesWithHeat(attributes, attrDbValue, false, false, armorAttr);
        else if (groupId == 774)
            AddResistAttributesWithHeat(attributes, attrDbValue, false, true, shieldAttr);
        else if (groupId == 773)
            AddResistAttributesWithHeat(attributes, attrDbValue, false, true, armorAttr);
        else
        {
            var msg = $"Unknown {label} resist module: {moduleName}, groupID={groupId}";
            Console.WriteLine(msg);
            Warnings.Add(msg);
        }
    }

    static Dictionary<ModuleTraits, float> GetModuleTraits(SdeData data, int typeId)
    {
        var moduleTraits = new Dictionary<ModuleTraits, float>();
        foreach (var bonus in data.GetBonuses(typeId))
        {
            if (bonus.SkillId <= 0)
                continue;

            var bonusText = bonus.BonusText;
            var value = bonus.Bonus;

            if (string.Equals(bonusText, "bonus to all shield hitpoints", StringComparison.OrdinalIgnoreCase))
                moduleTraits[ModuleTraits.ShieldHpPercentPerLevel] = value;
            else if (string.Equals(bonusText, "bonus to all armor hitpoints", StringComparison.OrdinalIgnoreCase))
                moduleTraits[ModuleTraits.ArmorHpPercentPerLevel] = value;
            else if (string.Equals(bonusText, "bonus to all armor and shield hitpoints", StringComparison.OrdinalIgnoreCase))
            {
                moduleTraits[ModuleTraits.ArmorHpPercentPerLevel] = value;
                moduleTraits[ModuleTraits.ShieldHpPercentPerLevel] = value;
            }
            else if (string.Equals(bonusText, "bonus to the benefits of overheating shield hardeners", StringComparison.OrdinalIgnoreCase))
                moduleTraits[ModuleTraits.ShieldHardenersOverheatingBonus] = value;
            else if (string.Equals(bonusText, "bonus to the benefits of overheating armor hardeners", StringComparison.OrdinalIgnoreCase))
                moduleTraits[ModuleTraits.ArmorHardenersOverheatingBonus] = value;
            else if (string.Equals(bonusText, "bonus to the benefits of overheating armor and shield hardeners", StringComparison.OrdinalIgnoreCase))
            {
                moduleTraits[ModuleTraits.ArmorHardenersOverheatingBonus] = value;
                moduleTraits[ModuleTraits.ShieldHardenersOverheatingBonus] = value;
            }
        }
        return moduleTraits;
    }

    static void AddTo(
        Dictionary<ModuleAttributes, Dictionary<ModuleActive, (float, int)>> attributes,
        ModuleAttributes attr,
        ModuleActive active,
        float value,
        int stackingGroup)
    {
        if (!attributes.TryGetValue(attr, out var byActive))
        {
            byActive = new Dictionary<ModuleActive, (float, int)>();
            attributes[attr] = byActive;
        }
        byActive[active] = (value, stackingGroup);
    }

    static void AddResistAttributesWithHeat(
        Dictionary<ModuleAttributes, Dictionary<ModuleActive, (float, int)>> attributes,
        float resistValueDb,
        bool bActiveModule,
        bool bRig,
        ModuleAttributes attribute)
    {
        float resistCold = -(0.01f * resistValueDb);
        if (!bActiveModule && !bRig)
            resistCold *= 1.25f;
        AddTo(attributes, attribute, bActiveModule ? ModuleActive.Active : ModuleActive.Passive, resistCold, 1);
    }

    static int GetStackingGroup(int groupId)
    {
        if (groupId == 60 || groupId == 1150 || groupId == 515)
            return 2;
        return 1;
    }
}
