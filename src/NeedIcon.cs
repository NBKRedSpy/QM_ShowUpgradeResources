using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;

namespace QM_ShowUpgradeResources
{
    public class NeedIcon : MonoBehaviour
    {

        private static Sprite NeedsIcon;

        public string ItemId { get; set; }

        private bool NeedsResource { get; set; }

        public GameObject ImageContainer { get; set; }

        /// <summary>
        /// Updates the icon status.
        /// </summary>
        /// <param name="isInRaid">If in raid, only includes that resources that are in the ship,
        /// Not the merc in the current raid</param>
        public void Refresh(bool isInRaid)
        {
            //When in raid, only use the inventory in the ship to determine if the resource upgrade amount
            //  is satisfied.  The purpose is prevent the user from choosing to drop an item because they thought
            //  they had enough inventory.  The tooltip is what handles the actual amount including inventory.
            if (isInRaid)
            {
                NeedsResource = Plugin.NeededResourceData.RequiresItemAfterShipInventory(ItemId);
            }
            else
            {
                NeedsResource = Plugin.NeededResourceData.UnpurchasedUpgradesRequiresItem(ItemId, out _);
            }

            ImageContainer.SetActive(NeedsResource);

        }

        static NeedIcon()
        {
            string assemblyPath = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
            Utils.LoadSprite(Path.Combine(assemblyPath, "Up-Arrow-64.png"), out NeedsIcon);
        }

        public NeedIcon()
        {
            RectTransform slotRect = transform.parent.GetComponent<RectTransform>();

            transform.SetParent(slotRect);

            ImageContainer = new GameObject("NeedIcon");

            ImageContainer.transform.SetParent(transform);

            Image image = ImageContainer.AddComponent<Image>();
            image.sprite = NeedsIcon;

            RectTransform rect = ImageContainer.GetComponent<RectTransform>();
            rect.anchoredPosition = new Vector2(-5f, -5f);
            rect.anchorMin = Vector2.one;
            rect.anchorMax = Vector2.one;
            rect.sizeDelta = new Vector2(16, 16);
        }
    }
}
