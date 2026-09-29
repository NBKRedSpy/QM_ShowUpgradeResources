using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HarmonyLib;
using MGSC;

namespace QM_ShowUpgradeResources
{
    [HarmonyPatch(typeof(MagnumModuleWindow), nameof(MagnumModuleWindow.Hide))]
    public static class MagnumModuleWindow_Hide_Patch
    {
        public static void Prefix()
        {
            Plugin.NeededResourceData.Update();
        }
    }
}
