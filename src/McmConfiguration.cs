using HarmonyLib;
using ModConfigMenu;
using ModConfigMenu.Contracts;
using ModConfigMenu.Objects;
using System;
using System.CodeDom;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Xml;
using UnityEngine;
using QM_ShowUpgradeResources.Mcm;
using QM_ShowUpgradeResources;

namespace QM_ShowUpgradeResources
{
    /// <summary>
    /// Responsible for mapping the ModConfig to the MCM configuration menu.  
    /// </summary>
    internal class McmConfiguration : McmConfigurationBase
    {

        public McmConfiguration(ModConfig config) : base (config) { }

        /// <summary>
        /// Registers the mod configuration with the Mod Config Menu API. This method is called when the mod is loaded and allows you to define the configuration options that will be available to users in the mod configuration menu.
        /// </summary>
        public override void Configure()
        {

            ModConfig defaults = new ModConfig();

            ModConfigMenuAPI.RegisterModConfig("QM_ShowUpgradeResources", new List<IConfigValue>()
            {
                //Note that MCM does support color codes, as shown below.

                //-- Restart note for any settings that cannot be immediately applied without restarting the game.
                new ConfigValue("__RestartNote", @"<color=#FF0000>The game must be restarted if any changes are made.</color>" , "Restart"),

                CreateConfigProperty(nameof(ModConfig.ShowTotalNeededResources),
                    "When enabled, adds a delta to the item info for upgrade resources.  Useful when deciding to trade items or" +
                    " hold onto a resource for higher priority upgrades",
                    "Show Total Needed Resources"),
            }, OnSave);
        }
         
    }
}
