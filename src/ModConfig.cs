using MGSC;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using QM_ShowUpgradeResources.Mcm;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace QM_ShowUpgradeResources
{
    public class ModConfig : PersistentConfig<ModConfig>
    {
        public ModConfig()
        {
        }

        public ModConfig(string configPath) : base(configPath)
        {

        }

        /// <summary>
        /// When enabled, adds a delta to the item info for upgrade resources. Useful when deciding to trade items 
        /// or hold onto a resource for higher priority upgrades
        /// </summary>
        public bool ShowTotalNeededResources { get; set; } = false;

        public void Save(string configPath)
        {
            string json = JsonConvert.SerializeObject(this, SerializerSettings);
            File.WriteAllText(Plugin.ConfigDirectories.ConfigPath, json);
        }
    }
}
