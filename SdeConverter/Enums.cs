namespace SdeConverter;

enum ShipAttributes
{
    HighSlots = 14,
    MedSlots = 13,
    LowSlots = 12,
    RigSlots = 1154,

    ShieldHp = 263,
    ShieldEmResonance = 271,
    ShieldThermResonance = 274,
    ShieldKinResonance = 273,
    ShieldExplResonance = 272,

    ArmorHp = 265,
    ArmorEmResonance = 267,
    ArmorThermResonance = 270,
    ArmorKinResonance = 269,
    ArmorExplResonance = 268,

    HullHp = 9,
    HullEmResonance = 113,
    HullThermResonance = 110,
    HullKinResonance = 109,
    HullExplResonance = 111,

    TechLevel = 422,
    SubsystemHoldCapacity = 2675,
}

enum ShipTraits
{
    ShieldResistsPerLevel = 1,
    ArmorResistsPerLevel = 2,
    HullResistsPerLevel = 3,

    ShieldHpPercentPerLevel = 4,
    ArmorHpPercentPerLevel = 5,
    HullHpPercentPerLevel = 6,

    OverheatingBonusPercent = 7,

    ShieldResistsRole = 8,
    ArmorResistsRole = 9,
}

enum ModuleSlot
{
    HighPower = 12,
    MediumPower = 13,
    LowPower = 11,
    Rig = 2663,
    Subsystem = 3772,
}

enum ModuleActive
{
    Passive,
    Active,
    AssaultPassive,
    AssaultActive,
}

enum ModuleTraits
{
    ShieldHpPercentPerLevel,
    ArmorHpPercentPerLevel,
    ShieldHardenersOverheatingBonus,
    ArmorHardenersOverheatingBonus,
}

enum ModuleAttributesDb
{
    CapacitorNeeded = 6,
    Duration = 73,

    ShieldEmResonance = 271,
    ShieldThermalResonance = 274,
    ShieldKineticResonance = 273,
    ShieldExplosiveResonance = 272,

    ArmorEmResonance = 267,
    ArmorThermalResonance = 270,
    ArmorKineticResonance = 269,
    ArmorExplosiveResonance = 268,

    HullEmResonance = 974,
    HullThermalResonance = 977,
    HullKineticResonance = 976,
    HullExplosiveResonance = 975,

    EmResistBonus = 984,
    ThermalResistBonus = 987,
    KineticResistBonus = 986,
    ExplosiveResistBonus = 985,

    AllResonances = 2746,

    OverloadHardeningBonus = 1208,

    ShieldCapacityMultiplier = 146,
    ArmorHpMultiplier = 148,
    StructureHpMultiplier = 150,

    ArmorHpBonusAdd = 1159,
    CapacityBonus = 72,
    ShieldCapacity = 263,
    StructureHpBonusAdd = 2688,

    ShieldCapacityBonus = 337,
    ArmorHpBonus = 335,
    HullHpBonus = 327,

    Drawback = 1138,

    ShipType = 1380,
    HighSlots = 1374,
    MediumSlots = 1375,
    LowSlots = 1376,

    Polarized = 1978,
}

enum ModuleAttributes
{
    Polarized,

    ShieldEmResist,
    ShieldThermalResist,
    ShieldKineticResist,
    ShieldExplosiveResist,

    ArmorEmResist,
    ArmorThermalResist,
    ArmorKineticResist,
    ArmorExplosiveResist,

    HullEmResist,
    HullThermalResist,
    HullKineticResist,
    HullExplosiveResist,

    ShieldBonusAdd,
    ArmorBonusAdd,
    HullBonusAdd,

    ShieldBonusMultiply,
    ArmorBonusMultiply,
    HullBonusMultiply,

    ShipType,
    HighSlots,
    MediumSlots,
    LowSlots,

    ShieldHardenersOverloadBonus,
    ArmorHardenersOverloadBonus,
}
