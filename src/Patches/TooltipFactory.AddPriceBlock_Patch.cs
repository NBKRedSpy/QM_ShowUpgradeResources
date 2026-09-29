using HarmonyLib;
using MGSC;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace QM_ShowUpgradeResources
{
    [HarmonyPatch(typeof(TooltipFactory), nameof(TooltipFactory.AddPriceBlock))]
    public static class Tooltip_SetPriceBlock_Patch
    {
        public static void Postfix(TooltipFactory __instance, string itemId)
        {
            if (!Plugin.NeededResourceData.UnpurchasedUpgradesRequiresItem(itemId, out int neededCount))
            {
                return;
            }

            int inventoryCount = ItemInteractionSystem.Count(__instance._state.Get<Mercenaries>(), __instance.MagnumCargo, itemId);

            int surplus =  inventoryCount - neededCount;

            Color neededColor = surplus < 0 ? Colors.White : Colors.AltGreen;

            //Format to use a positive/negative number with colors
            // (-1) 5/4
            // (0) 5/5
            // (+10) 5/10

            string displayText = $"({surplus:+0;-0;0}) {inventoryCount}/{neededCount}".WrapInColor(neededColor);

            __instance._tooltip._count.text = displayText;

            Localization.ActualizeFontAndSize(__instance._tooltip._count);
        }
    }
}
