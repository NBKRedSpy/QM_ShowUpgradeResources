using HarmonyLib;
using MGSC;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TMPro;
using UnityEngine;

namespace QM_ShowUpgradeResources.Patches.MagnumUpgradeScreen
{
    /// <summary>
    /// Change the the Confirmation Window test to include the needed amount for each resource.
    /// </summary>
    [HarmonyPatch(typeof(TooltipItemIcon), nameof(TooltipItemIcon.ConfigureAvailability))]
    internal class TooltipItemIcon_ConfigureAvailability_Patch
    {
        public static void Postfix(TooltipItemIcon __instance, int available, int required)
        {
            //Doesn't cost much, and we are here anyway.  
            //  Avoids needing to update the confirm dialog amount when an upgrade is purchased.
            Plugin.NeededResourceData.Update();

            if(!Plugin.NeededResourceData.GetRequiresItem(TooltipItemIcon_Initialize_Patch.ItemId, false, 
                out int totalNeededCount))
            {
                return;
            }

            if (totalNeededCount > 0)
            {
                string formattedAmount = $"-{totalNeededCount}".WrapInColor(Colors.LightRed);
                __instance._rightCornerText.richText = true;
                __instance._rightCornerText.SetText($"{required}({available} {formattedAmount})");
                __instance._rightCornerText.fontSize = 6;
            }

        }
    }
}
