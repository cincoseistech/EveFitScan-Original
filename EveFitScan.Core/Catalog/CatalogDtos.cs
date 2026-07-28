using System.Collections.Generic;
using MessagePack;

namespace EveFitScan.Core.Catalog
{
    [MessagePackObject]
    public sealed class CatalogFile
    {
        [Key(0)]
        public int BuildNumber { get; set; }

        [Key(1)]
        public string ReleaseDate { get; set; }

        [Key(2)]
        public List<ShipDto> Ships { get; set; }

        [Key(3)]
        public List<ModuleDto> Modules { get; set; }
    }

    [MessagePackObject]
    public sealed class ShipDto
    {
        [Key(0)] public string Name { get; set; }
        [Key(1)] public int TypeId { get; set; }
        [Key(2)] public uint HighSlots { get; set; }
        [Key(3)] public uint MedSlots { get; set; }
        [Key(4)] public uint LowSlots { get; set; }
        [Key(5)] public uint RigSlots { get; set; }
        [Key(6)] public uint SubsystemSlots { get; set; }
        [Key(7)] public float ShieldHp { get; set; }
        [Key(8)] public float ShieldHpMultiplier { get; set; }
        [Key(9)] public float ShieldResistEm { get; set; }
        [Key(10)] public float ShieldResistThermal { get; set; }
        [Key(11)] public float ShieldResistKinetic { get; set; }
        [Key(12)] public float ShieldResistExplosive { get; set; }
        [Key(13)] public float ArmorHp { get; set; }
        [Key(14)] public float ArmorHpMultiplier { get; set; }
        [Key(15)] public float ArmorResistEm { get; set; }
        [Key(16)] public float ArmorResistThermal { get; set; }
        [Key(17)] public float ArmorResistKinetic { get; set; }
        [Key(18)] public float ArmorResistExplosive { get; set; }
        [Key(19)] public float HullHp { get; set; }
        [Key(20)] public float HullHpMultiplier { get; set; }
        [Key(21)] public float HullResistEm { get; set; }
        [Key(22)] public float HullResistThermal { get; set; }
        [Key(23)] public float HullResistKinetic { get; set; }
        [Key(24)] public float HullResistExplosive { get; set; }
        [Key(25)] public float OverheatingBonus { get; set; }
    }

    [MessagePackObject]
    public sealed class ModuleDto
    {
        [Key(0)] public string Name { get; set; }
        [Key(1)] public int TypeId { get; set; }
        [Key(2)] public int Slot { get; set; }
        [Key(3)] public float OverloadBonus { get; set; }
        [Key(4)] public int ShipTypeId { get; set; }
        [Key(5)] public List<ModuleEffectDto> Effects { get; set; }
    }

    [MessagePackObject]
    public sealed class ModuleEffectDto
    {
        [Key(0)] public int Layer { get; set; }
        [Key(1)] public int Effect { get; set; }
        [Key(2)] public float Value { get; set; }
        [Key(3)] public int Active { get; set; }
        [Key(4)] public int StackGroup { get; set; }
    }
}
