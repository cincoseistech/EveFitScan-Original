using System;
using System.Collections.Generic;

namespace EveFitScan.Core
{
    public static class EhpCalculator
    {
        public static float GetEHP(
            float shieldHP, IReadOnlyDictionary<ShipModel.RESIST, float> shieldResists,
            float armorHP, IReadOnlyDictionary<ShipModel.RESIST, float> armorResists,
            float hullHP, IReadOnlyDictionary<ShipModel.RESIST, float> hullResists,
            IReadOnlyDictionary<ShipModel.RESIST, float> ammo)
        {
            return GetLayerEHP(shieldHP, shieldResists, ammo)
                + GetLayerEHP(armorHP, armorResists, ammo)
                + GetLayerEHP(hullHP, hullResists, ammo);
        }

        public static float GetLayerEHP(
            float layerHP,
            IReadOnlyDictionary<ShipModel.RESIST, float> layerResists,
            IReadOnlyDictionary<ShipModel.RESIST, float> ammo)
        {
            float fullAmmoDamage = 0.0f;
            float appliedDamage = 0.0f;
            foreach (ShipModel.RESIST resist in Enum.GetValues(typeof(ShipModel.RESIST)))
            {
                fullAmmoDamage += ammo[resist];
                appliedDamage += ammo[resist] * (1.0f - layerResists[resist]);
            }
            return layerHP * fullAmmoDamage / appliedDamage;
        }

        public static IReadOnlyDictionary<ShipModel.RESIST, float> AmmoMjolnir
        {
            get { return AmmoProfiles.Mjolnir; }
        }

        public static IReadOnlyDictionary<ShipModel.RESIST, float> AmmoNova
        {
            get { return AmmoProfiles.Nova; }
        }

        public static IReadOnlyDictionary<ShipModel.RESIST, float> AmmoAntimatter
        {
            get { return AmmoProfiles.Antimatter; }
        }

        public static IReadOnlyDictionary<ShipModel.RESIST, float> AmmoVoid
        {
            get { return AmmoProfiles.Void; }
        }

        public static IReadOnlyDictionary<ShipModel.RESIST, float> AmmoMultifreq
        {
            get { return AmmoProfiles.Multifreq; }
        }

        public static IReadOnlyDictionary<ShipModel.RESIST, float> AmmoEMP
        {
            get { return AmmoProfiles.EMP; }
        }

        public static IReadOnlyDictionary<ShipModel.RESIST, float> AmmoFusion
        {
            get { return AmmoProfiles.Fusion; }
        }

        public static IReadOnlyDictionary<ShipModel.RESIST, float> AmmoPhasedPlasma
        {
            get { return AmmoProfiles.PhasedPlasma; }
        }

        public static IReadOnlyDictionary<ShipModel.RESIST, float> AmmoHail
        {
            get { return AmmoProfiles.Hail; }
        }

        public static IReadOnlyDictionary<ShipModel.RESIST, float> AmmoUniform
        {
            get { return AmmoProfiles.Uniform; }
        }

        private static class AmmoProfiles
        {
            public static readonly Dictionary<ShipModel.RESIST, float> Mjolnir = Create(100.0f, 0.0f, 0.0f, 0.0f);
            public static readonly Dictionary<ShipModel.RESIST, float> Nova = Create(0.0f, 0.0f, 0.0f, 100.0f);
            public static readonly Dictionary<ShipModel.RESIST, float> Antimatter = Create(0.0f, 5.0f, 7.0f, 0.0f);
            public static readonly Dictionary<ShipModel.RESIST, float> Void = Create(0.0f, 7.7f, 7.7f, 0.0f);
            public static readonly Dictionary<ShipModel.RESIST, float> Multifreq = Create(7.0f, 5.0f, 0.0f, 0.0f);
            public static readonly Dictionary<ShipModel.RESIST, float> EMP = Create(9.0f, 0.0f, 1.0f, 2.0f);
            public static readonly Dictionary<ShipModel.RESIST, float> Fusion = Create(0.0f, 0.0f, 2.0f, 10.0f);
            public static readonly Dictionary<ShipModel.RESIST, float> PhasedPlasma = Create(0.0f, 10.0f, 2.0f, 0.0f);
            public static readonly Dictionary<ShipModel.RESIST, float> Hail = Create(0.0f, 0.0f, 3.3f, 12.1f);
            public static readonly Dictionary<ShipModel.RESIST, float> Uniform = Create(10.0f, 10.0f, 10.0f, 10.0f);

            private static Dictionary<ShipModel.RESIST, float> Create(float em, float thermal, float kinetic, float explosive)
            {
                return new Dictionary<ShipModel.RESIST, float>
                {
                    { ShipModel.RESIST.EM, em },
                    { ShipModel.RESIST.THERMAL, thermal },
                    { ShipModel.RESIST.KINETIC, kinetic },
                    { ShipModel.RESIST.EXPLOSIVE, explosive }
                };
            }
        }
    }
}
