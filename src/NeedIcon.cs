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

        private bool NeedsItem { get; set; }

        public GameObject ImageContainer { get; set; }

        public void Refresh()
        {
            NeedsItem = Plugin.NeededResourceData.GetRequiresItem(ItemId);
            ImageContainer.SetActive(NeedsItem);
        }

        public NeedIcon()
        {
            if(NeedsIcon == null)
            {
                
                string assemblyPath = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
                Utils.LoadSprite(Path.Combine(assemblyPath, "Up-Arrow-64.png"), out NeedsIcon);
            }

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
