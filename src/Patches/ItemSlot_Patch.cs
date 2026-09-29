using HarmonyLib;
using MGSC;
using QM_ShowUpgradeResources;
using System;
using UnityEngine;

namespace PrototypeMod
{
    [HarmonyPatch(typeof(ItemSlot), nameof(ItemSlot.Initialize))]
    public static class ItemSlot_Patch
    {
        public static void Prefix(ItemSlot __instance, BasePickupItem item, ItemStorage itemStorage)
        {
            AddResourceCount(__instance, item?.Id);
        }

        public static void AddResourceCount(ItemSlot __instance, string id)
        {
            NeedIcon component = __instance.gameObject.GetComponent<NeedIcon>();
            
            if (component == null)
            {
                component = __instance.gameObject.AddComponent<NeedIcon>();
            }

            component.ItemId = id;
            component.Refresh(Plugin.State.Get<DungeonGameMode>() != null);
        }
    }



    [HarmonyPatch(typeof(ItemSlot), nameof(ItemSlot.InitializeEmpty))]
    public static class ItemSlotPatch2
    {
        public static void Prefix(ItemSlot __instance)
        {
            ItemSlot_Patch.AddResourceCount(__instance, string.Empty);
        }
    }


}