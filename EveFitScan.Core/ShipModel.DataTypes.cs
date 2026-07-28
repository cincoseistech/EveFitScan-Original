using System;
using System.Collections.Generic;

namespace EveFitScan.Core
{
    public partial class ShipModel
    {
        public enum SLOT { HIGH, MEDIUM, LOW, RIG, SUB_CORE, SUB_DEFENSIVE, SUB_OFFENSIVE, SUB_PROPULSION }
        public enum LAYER { SHIELD, ARMOR, HULL, NONE }
        public enum EFFECT { POLARIZED, EM, THERMAL, KINETIC, EXPLOSIVE, ADD, MULTIPLY, HIGH_SLOTS, MEDIUM_SLOTS, LOW_SLOTS, OVERHEATING }
        public enum ACTIVE { PASSIVE, ACTIVE, ASSAULT_PASSIVE, ASSAULT_ACTIVE }

        public class ShipDescription
        {
            public ShipDescription(
                string Name, int TypeID, uint HighSlots, uint MedSlots, uint LowSlots, uint RigSlots, uint SubsystemSlots,
                float ShieldHP, float ShieldHPMultiplier, float ShieldResistEM, float ShieldResistThermal, float ShieldResistKinetic, float ShieldResistExplosive,
                float ArmorHP, float ArmorHPMultiplier, float ArmorResistEM, float ArmorResistThermal, float ArmorResistKinetic, float ArmorResistExplosive,
                float HullHP, float HullHPMultiplier, float HullResistEM, float HullResistThermal, float HullResistKinetic, float HullResistExplosive,
                float OverheatingBonus)
            {
                m_Name = Name;
                m_TypeID = TypeID;
                m_HighSlots = HighSlots;
                m_MedSlots = MedSlots;
                m_LowSlots = LowSlots;
                m_RigSlots = RigSlots;
                m_SubsystemSlots = SubsystemSlots;
                m_ShieldHP = ShieldHP;
                m_ShieldHPMultiplier = ShieldHPMultiplier;
                m_ShieldResistEM = ShieldResistEM;
                m_ShieldResistThermal = ShieldResistThermal;
                m_ShieldResistKinetic = ShieldResistKinetic;
                m_ShieldResistExplosive = ShieldResistExplosive;
                m_ArmorHP = ArmorHP;
                m_ArmorHPMultiplier = ArmorHPMultiplier;
                m_ArmorResistEM = ArmorResistEM;
                m_ArmorResistThermal = ArmorResistThermal;
                m_ArmorResistKinetic = ArmorResistKinetic;
                m_ArmorResistExplosive = ArmorResistExplosive;
                m_HullHP = HullHP;
                m_HullHPMultiplier = HullHPMultiplier;
                m_HullResistEM = HullResistEM;
                m_HullResistThermal = HullResistThermal;
                m_HullResistKinetic = HullResistKinetic;
                m_HullResistExplosive = HullResistExplosive;
                m_OverheatingBonus = OverheatingBonus;
            }

            public string m_Name;
            public int m_TypeID;
            public uint m_HighSlots;
            public uint m_MedSlots;
            public uint m_LowSlots;
            public uint m_RigSlots;
            public uint m_SubsystemSlots;
            public float m_ShieldHP;
            public float m_ShieldHPMultiplier;
            public float m_ShieldResistEM;
            public float m_ShieldResistThermal;
            public float m_ShieldResistKinetic;
            public float m_ShieldResistExplosive;
            public float m_ArmorHP;
            public float m_ArmorHPMultiplier;
            public float m_ArmorResistEM;
            public float m_ArmorResistThermal;
            public float m_ArmorResistKinetic;
            public float m_ArmorResistExplosive;
            public float m_HullHP;
            public float m_HullHPMultiplier;
            public float m_HullResistEM;
            public float m_HullResistThermal;
            public float m_HullResistKinetic;
            public float m_HullResistExplosive;
            public float m_OverheatingBonus;
        }

        public class ModuleDescription
        {
            public ModuleDescription(string Name, int TypeID, SLOT Slot, float OverloadBonus, int ShipTypeID)
            {
                m_Name = Name;
                m_TypeID = TypeID;
                m_Slot = Slot;
                m_OverloadBonus = OverloadBonus;
                m_ShipTypeID = ShipTypeID;
            }

            public ModuleDescription AddEffect(LAYER Layer, EFFECT Effect, float Value, ACTIVE Active, int StackGroup)
            {
                if (!m_Effects.ContainsKey(Layer))
                    m_Effects.Add(Layer, new Dictionary<EFFECT, Dictionary<ACTIVE, Tuple<float, int>>>());
                if (!m_Effects[Layer].ContainsKey(Effect))
                    m_Effects[Layer].Add(Effect, new Dictionary<ACTIVE, Tuple<float, int>>());
                m_Effects[Layer][Effect][Active] = new Tuple<float, int>(Value, StackGroup);
                return this;
            }

            public string m_Name;
            public int m_TypeID;
            public SLOT m_Slot;
            public float m_OverloadBonus;
            public float m_ShipTypeID;
            public Dictionary<LAYER, Dictionary<EFFECT, Dictionary<ACTIVE, Tuple<float, int>>>> m_Effects =
                new Dictionary<LAYER, Dictionary<EFFECT, Dictionary<ACTIVE, Tuple<float, int>>>>();
        }
    }
}
