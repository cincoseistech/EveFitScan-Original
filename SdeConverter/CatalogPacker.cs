using System.Text.Json;
using EveFitScan.Core;
using EveFitScan.Core.Catalog;
using MessagePack;

namespace SdeConverter;

static class CatalogPacker
{
    public static void Write(string msgpackPath, string metaPath, int buildNumber, string releaseDate,
        IReadOnlyList<ShipDescription> ships, IReadOnlyList<ModuleDescription> modules)
    {
        var catalog = new CatalogFile
        {
            BuildNumber = buildNumber,
            ReleaseDate = releaseDate ?? "",
            Ships = new List<ShipDto>(ships.Count),
            Modules = new List<ModuleDto>(modules.Count),
        };

        foreach (var s in ships)
        {
            catalog.Ships.Add(new ShipDto
            {
                Name = s.Name,
                TypeId = s.TypeId,
                HighSlots = s.HighSlots,
                MedSlots = s.MedSlots,
                LowSlots = s.LowSlots,
                RigSlots = s.RigSlots,
                SubsystemSlots = s.SubsystemSlots,
                ShieldHp = s.ShieldHp,
                ShieldHpMultiplier = s.ShieldHpMultiplier,
                ShieldResistEm = s.ShieldResistEm,
                ShieldResistThermal = s.ShieldResistThermal,
                ShieldResistKinetic = s.ShieldResistKinetic,
                ShieldResistExplosive = s.ShieldResistExplosive,
                ArmorHp = s.ArmorHp,
                ArmorHpMultiplier = s.ArmorHpMultiplier,
                ArmorResistEm = s.ArmorResistEm,
                ArmorResistThermal = s.ArmorResistThermal,
                ArmorResistKinetic = s.ArmorResistKinetic,
                ArmorResistExplosive = s.ArmorResistExplosive,
                HullHp = s.HullHp,
                HullHpMultiplier = s.HullHpMultiplier,
                HullResistEm = s.HullResistEm,
                HullResistThermal = s.HullResistThermal,
                HullResistKinetic = s.HullResistKinetic,
                HullResistExplosive = s.HullResistExplosive,
                OverheatingBonus = s.OverheatingBonus,
            });
        }

        foreach (var m in modules)
        {
            var slot = ResolveSlot(m.Slot, m.Name);
            var effects = new List<ModuleEffectDto>();
            foreach (var attr in m.Attributes)
            {
                if (!TryMapAttribute(attr.Key, out var layer, out var effect))
                    continue;
                foreach (var activeEntry in attr.Value)
                {
                    effects.Add(new ModuleEffectDto
                    {
                        Layer = (int)layer,
                        Effect = (int)effect,
                        Value = activeEntry.Value.Value,
                        Active = (int)MapActive(activeEntry.Key),
                        StackGroup = activeEntry.Value.StackGroup,
                    });
                }
            }

            catalog.Modules.Add(new ModuleDto
            {
                Name = m.Name,
                TypeId = m.TypeId,
                Slot = (int)slot,
                OverloadBonus = m.OverloadBonus,
                ShipTypeId = m.ShipTypeId,
                Effects = effects,
            });
        }

        Directory.CreateDirectory(Path.GetDirectoryName(msgpackPath)!);
        var bytes = MessagePackSerializer.Serialize(catalog);
        File.WriteAllBytes(msgpackPath, bytes);

        var meta = new
        {
            buildNumber,
            releaseDate,
            ships = ships.Count,
            modules = modules.Count,
            generatedUtc = DateTime.UtcNow.ToString("O"),
        };
        File.WriteAllText(metaPath, JsonSerializer.Serialize(meta, new JsonSerializerOptions { WriteIndented = true }));
    }

    static ShipModel.SLOT ResolveSlot(ModuleSlot slot, string moduleName)
    {
        if (slot == ModuleSlot.HighPower) return ShipModel.SLOT.HIGH;
        if (slot == ModuleSlot.MediumPower) return ShipModel.SLOT.MEDIUM;
        if (slot == ModuleSlot.LowPower) return ShipModel.SLOT.LOW;
        if (slot == ModuleSlot.Rig) return ShipModel.SLOT.RIG;
        if (slot == ModuleSlot.Subsystem)
        {
            if (moduleName.Contains(" Core - ")) return ShipModel.SLOT.SUB_CORE;
            if (moduleName.Contains(" Defensive - ")) return ShipModel.SLOT.SUB_DEFENSIVE;
            if (moduleName.Contains(" Offensive - ")) return ShipModel.SLOT.SUB_OFFENSIVE;
            if (moduleName.Contains(" Propulsion - ")) return ShipModel.SLOT.SUB_PROPULSION;
        }
        throw new InvalidOperationException($"Unknown slot for module '{moduleName}' ({slot}).");
    }

    static ShipModel.ACTIVE MapActive(ModuleActive active) => active switch
    {
        ModuleActive.Passive => ShipModel.ACTIVE.PASSIVE,
        ModuleActive.Active => ShipModel.ACTIVE.ACTIVE,
        ModuleActive.AssaultPassive => ShipModel.ACTIVE.ASSAULT_PASSIVE,
        ModuleActive.AssaultActive => ShipModel.ACTIVE.ASSAULT_ACTIVE,
        _ => throw new ArgumentOutOfRangeException(nameof(active), active, null),
    };

    static bool TryMapAttribute(ModuleAttributes attr, out ShipModel.LAYER layer, out ShipModel.EFFECT effect)
    {
        switch (attr)
        {
            case ModuleAttributes.Polarized:
                layer = ShipModel.LAYER.NONE; effect = ShipModel.EFFECT.POLARIZED; return true;
            case ModuleAttributes.ShieldEmResist:
                layer = ShipModel.LAYER.SHIELD; effect = ShipModel.EFFECT.EM; return true;
            case ModuleAttributes.ShieldThermalResist:
                layer = ShipModel.LAYER.SHIELD; effect = ShipModel.EFFECT.THERMAL; return true;
            case ModuleAttributes.ShieldKineticResist:
                layer = ShipModel.LAYER.SHIELD; effect = ShipModel.EFFECT.KINETIC; return true;
            case ModuleAttributes.ShieldExplosiveResist:
                layer = ShipModel.LAYER.SHIELD; effect = ShipModel.EFFECT.EXPLOSIVE; return true;
            case ModuleAttributes.ArmorEmResist:
                layer = ShipModel.LAYER.ARMOR; effect = ShipModel.EFFECT.EM; return true;
            case ModuleAttributes.ArmorThermalResist:
                layer = ShipModel.LAYER.ARMOR; effect = ShipModel.EFFECT.THERMAL; return true;
            case ModuleAttributes.ArmorKineticResist:
                layer = ShipModel.LAYER.ARMOR; effect = ShipModel.EFFECT.KINETIC; return true;
            case ModuleAttributes.ArmorExplosiveResist:
                layer = ShipModel.LAYER.ARMOR; effect = ShipModel.EFFECT.EXPLOSIVE; return true;
            case ModuleAttributes.HullEmResist:
                layer = ShipModel.LAYER.HULL; effect = ShipModel.EFFECT.EM; return true;
            case ModuleAttributes.HullThermalResist:
                layer = ShipModel.LAYER.HULL; effect = ShipModel.EFFECT.THERMAL; return true;
            case ModuleAttributes.HullKineticResist:
                layer = ShipModel.LAYER.HULL; effect = ShipModel.EFFECT.KINETIC; return true;
            case ModuleAttributes.HullExplosiveResist:
                layer = ShipModel.LAYER.HULL; effect = ShipModel.EFFECT.EXPLOSIVE; return true;
            case ModuleAttributes.ShieldBonusAdd:
                layer = ShipModel.LAYER.SHIELD; effect = ShipModel.EFFECT.ADD; return true;
            case ModuleAttributes.ArmorBonusAdd:
                layer = ShipModel.LAYER.ARMOR; effect = ShipModel.EFFECT.ADD; return true;
            case ModuleAttributes.HullBonusAdd:
                layer = ShipModel.LAYER.HULL; effect = ShipModel.EFFECT.ADD; return true;
            case ModuleAttributes.ShieldBonusMultiply:
                layer = ShipModel.LAYER.SHIELD; effect = ShipModel.EFFECT.MULTIPLY; return true;
            case ModuleAttributes.ArmorBonusMultiply:
                layer = ShipModel.LAYER.ARMOR; effect = ShipModel.EFFECT.MULTIPLY; return true;
            case ModuleAttributes.HullBonusMultiply:
                layer = ShipModel.LAYER.HULL; effect = ShipModel.EFFECT.MULTIPLY; return true;
            case ModuleAttributes.HighSlots:
                layer = ShipModel.LAYER.NONE; effect = ShipModel.EFFECT.HIGH_SLOTS; return true;
            case ModuleAttributes.MediumSlots:
                layer = ShipModel.LAYER.NONE; effect = ShipModel.EFFECT.MEDIUM_SLOTS; return true;
            case ModuleAttributes.LowSlots:
                layer = ShipModel.LAYER.NONE; effect = ShipModel.EFFECT.LOW_SLOTS; return true;
            case ModuleAttributes.ShieldHardenersOverloadBonus:
                layer = ShipModel.LAYER.SHIELD; effect = ShipModel.EFFECT.OVERHEATING; return true;
            case ModuleAttributes.ArmorHardenersOverloadBonus:
                layer = ShipModel.LAYER.ARMOR; effect = ShipModel.EFFECT.OVERHEATING; return true;
            default:
                layer = default;
                effect = default;
                return false;
        }
    }
}
