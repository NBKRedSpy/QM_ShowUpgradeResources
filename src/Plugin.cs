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
            //TODO:  Move this to try configure since we are searching now.

            //Note - this is a workaround as the game does not support an "optional dependency".  
            //  The alternative is to make sure the MCM is configured before the main menu is started,
            //  which should always be after all the mods are loaded.
            if (McmConfiguration != null ||
                !State.Get<UserMods>().Values.Any(x => x.UniqueModName == "Crynano_ModConfigMenu"))
            {
                return;
            }

            //Optionally support MCM if the user has MCM installed.  Otherwise the user must edit the config
            //  file directly as needed.
            //Have to configure at the MainMenu to ensure the MCM is not actually required, but it still has to be loaded 
            //  before this mod.
            McmConfiguration = new McmConfiguration(Config);
            McmConfiguration.Configure();
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
