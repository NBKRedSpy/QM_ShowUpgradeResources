using HarmonyLib;
using MGSC;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QM_ShowUpgradeResources.Patches
{
    /// <summary>
    /// Gets the ID of the  item's icon.  This is used by the TooltipFactory patch as the id is not available at that level.
    /// This allows the AddPriceBlock patch to not need to be more contained and resiliant to changes.
    /// </summary>
    [HarmonyPatch(typeof(TooltipItemIcon), nameof(TooltipItemIcon.Initialize))]
    internal class TooltipItemIcon_Initialize_Patch
    {
        public static string ItemId { get; private set; }

        public static void Prefix(string itemId)
        {
            ItemId = itemId;
        }
    }
}
