using HarmonyLib;
using MGSC;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QM_ShowUpgradeResources
{
    [HarmonyPatch(typeof(TooltipFactory), nameof(TooltipFactory.AddPriceBlock))]
    public static class Tooltip_SetPriceBlock_Patch
    {
        public static void Postfix(TooltipFactory __instance, string itemId)
        {
            if (!Plugin.NeededResourceData.GetRequiresItem(itemId, out int count)) return;

            int inventoryCount = ItemInteractionSystem.Count(__instance._state.Get<Mercenaries>(), __instance.MagnumCargo, itemId);

            __instance._tooltip._count.text = $"Need: {count}".WrapInColor(Colors.Yellow) + " - " +
                inventoryCount.ToString().WrapInColor(Colors.AltGreen);

            Localization.ActualizeFontAndSize(__instance._tooltip._count);
        }

    }
}
