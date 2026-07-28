using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using MessagePack;

namespace EveFitScan.Core.Catalog
{
    public static class CatalogLoader
    {
        public const string ResourceName = "EveFitScan.Core.Data.fitscan-catalog.msgpack";

        /// <summary>
        /// Optional user-updated catalog path. Used when the file exists.
        /// </summary>
        public static string UserCatalogPath { get; set; }

        public static CatalogFile LoadEmbedded()
        {
            var assembly = typeof(CatalogLoader).GetTypeInfo().Assembly;
            using (var stream = assembly.GetManifestResourceStream(ResourceName))
            {
                if (stream == null)
                    throw new InvalidOperationException("Embedded catalog resource not found: " + ResourceName);
                return MessagePackSerializer.Deserialize<CatalogFile>(stream);
            }
        }

        public static CatalogFile LoadFromFile(string path)
        {
            using (var stream = File.OpenRead(path))
                return MessagePackSerializer.Deserialize<CatalogFile>(stream);
        }

        public static bool HasUserCatalog()
        {
            return !string.IsNullOrEmpty(UserCatalogPath) && File.Exists(UserCatalogPath);
        }

        public static CatalogFile LoadPreferred()
        {
            if (HasUserCatalog())
                return LoadFromFile(UserCatalogPath);
            return LoadEmbedded();
        }

        public static int GetActiveBuildNumber()
        {
            try
            {
                return LoadPreferred().BuildNumber;
            }
            catch
            {
                return 0;
            }
        }

        public static bool IsUsingUserCatalog()
        {
            return HasUserCatalog();
        }

        public static void ApplyTo(ShipModel model, CatalogFile catalog)
        {
            var ships = new List<ShipModel.ShipDescription>(catalog.Ships.Count);
            foreach (var s in catalog.Ships)
            {
                ships.Add(new ShipModel.ShipDescription(
                    s.Name, s.TypeId, s.HighSlots, s.MedSlots, s.LowSlots, s.RigSlots, s.SubsystemSlots,
                    s.ShieldHp, s.ShieldHpMultiplier, s.ShieldResistEm, s.ShieldResistThermal, s.ShieldResistKinetic, s.ShieldResistExplosive,
                    s.ArmorHp, s.ArmorHpMultiplier, s.ArmorResistEm, s.ArmorResistThermal, s.ArmorResistKinetic, s.ArmorResistExplosive,
                    s.HullHp, s.HullHpMultiplier, s.HullResistEm, s.HullResistThermal, s.HullResistKinetic, s.HullResistExplosive,
                    s.OverheatingBonus));
            }

            var modules = new List<ShipModel.ModuleDescription>(catalog.Modules.Count);
            foreach (var m in catalog.Modules)
            {
                var md = new ShipModel.ModuleDescription(
                    m.Name, m.TypeId, (ShipModel.SLOT)m.Slot, m.OverloadBonus, m.ShipTypeId);
                if (m.Effects != null)
                {
                    foreach (var e in m.Effects)
                    {
                        md.AddEffect(
                            (ShipModel.LAYER)e.Layer,
                            (ShipModel.EFFECT)e.Effect,
                            e.Value,
                            (ShipModel.ACTIVE)e.Active,
                            e.StackGroup);
                    }
                }
                modules.Add(md);
            }

            model.SetCatalogData(ships, modules);
        }
    }
}
