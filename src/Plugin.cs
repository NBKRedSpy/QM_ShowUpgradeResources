using HarmonyLib;
using MGSC;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace QM_ShowUpgradeResources
{
    public static class Plugin
    {

        public static ConfigDirectories ConfigDirectories = new ConfigDirectories();

        public static ModConfig Config { get; private set; }
        internal static McmConfiguration McmConfiguration { get; private set; }

        public static Logger Logger = new Logger();
        public static State State { get; private set; }

        public static NeededResourceData NeededResourceData { get; private set; } = new NeededResourceData();

        [Hook(ModHookType.AfterConfigsLoaded)]
        public static void AfterConfig(IModContext context)
        {
            Directory.CreateDirectory(ConfigDirectories.AllModsConfigFolder);
            ConfigDirectories = new ConfigDirectories();
            Directory.CreateDirectory(ConfigDirectories.ModPersistenceFolder);

            Config = new ModConfig(ConfigDirectories.ConfigPath).LoadConfig();

            State = context.State;
            new Harmony("NBKRedSpy_" + ConfigDirectories.ModAssemblyName).PatchAll();
        }


        /// <summary>
        /// Handles the optional MCM configuration if the user has MCM installed.  
        /// </summary>
        /// <param name="modContext"></param>
        [Hook(ModHookType.MainMenuStarted)]
        public static void MainMenuStarted(ModContext modContext)
        {

            //Note - this is a workaround as the game does not support an "optional dependency"
            //  and we cannot guarantee that MCM will be loaded before this mod.

            //At worst this will be invoked on each main menu return, but won't be a big deal.
            if (McmConfiguration != null) return;

            McmConfiguration = McmConfiguration.TryConfigure(modContext.State, Config);
        }

        [Hook(ModHookType.DungeonStarted)]
        public static void DungeonStarted(IModContext context)
        {
            NeededResourceData.Update();
        }

        [Hook(ModHookType.SpaceStarted)]
        public static void SpaceStarted(IModContext context)
        {
            NeededResourceData.Update();
        }
       
    }
}
