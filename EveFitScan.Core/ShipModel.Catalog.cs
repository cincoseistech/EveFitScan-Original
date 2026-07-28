using System.Collections.Generic;
using EveFitScan.Core.Catalog;

namespace EveFitScan.Core
{
    public partial class ShipModel
    {
        private List<ShipDescription> m_ShipDescriptions;
        private List<ModuleDescription> m_ModuleDescriptions;
        private static readonly object CatalogLock = new object();
        private static CatalogFile s_CachedCatalog;

        public IReadOnlyList<ShipDescription> ShipDescriptions
        {
            get
            {
                EnsureCatalogLoaded();
                return m_ShipDescriptions;
            }
        }

        public IReadOnlyList<ModuleDescription> ModuleDescriptions
        {
            get
            {
                EnsureCatalogLoaded();
                return m_ModuleDescriptions;
            }
        }

        public int CatalogBuildNumber
        {
            get
            {
                EnsureCatalogLoaded();
                return s_CachedCatalog != null ? s_CachedCatalog.BuildNumber : 0;
            }
        }

        internal void SetCatalogData(List<ShipDescription> ships, List<ModuleDescription> modules)
        {
            m_ShipDescriptions = ships;
            m_ModuleDescriptions = modules;
        }

        /// <summary>
        /// Replace the shared catalog cache and reload this model's indexes.
        /// </summary>
        public void ReloadCatalog(CatalogFile catalog = null)
        {
            lock (CatalogLock)
            {
                s_CachedCatalog = catalog ?? CatalogLoader.LoadPreferred();
                ClearCatalogIndexes();
                CatalogLoader.ApplyTo(this, s_CachedCatalog);
            }
        }

        public static void InvalidateCachedCatalog()
        {
            lock (CatalogLock)
            {
                s_CachedCatalog = null;
            }
        }

        private void ClearCatalogIndexes()
        {
            m_ShipDescriptions = null;
            m_ModuleDescriptions = null;
            m_ShipNameToIndex = null;
            m_ShipTypeIDToIndex = null;
            m_ModuleNameToIndex = null;
            m_ModuleTypeIDToIndex = null;
            m_ShipNamesSorted = null;
        }

        private void EnsureCatalogLoaded()
        {
            if (m_ShipDescriptions != null && m_ModuleDescriptions != null)
                return;

            lock (CatalogLock)
            {
                if (m_ShipDescriptions != null && m_ModuleDescriptions != null)
                    return;

                if (s_CachedCatalog == null)
                    s_CachedCatalog = CatalogLoader.LoadPreferred();

                CatalogLoader.ApplyTo(this, s_CachedCatalog);
            }
        }
    }
}
