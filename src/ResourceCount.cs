using MGSC;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TMPro;
using UnityEngine;
using UnityEngine.PlayerLoop;

namespace QM_ShowUpgradeResources
{
    class ResourceCount : MonoBehaviour
    {

        const string ComponentName = "ResourceCountText";

        /// <summary>
        /// The ID of the parent item.  Set to null to mean nothing.
        /// </summary>
        public string ItemId { get; set; }
        /// <summary>
        /// The indicator for the number of resources needed.
        /// </summary>
        private TextMeshProUGUI UiText { get; set; }

        private GameObject TextGameObject { get; set; }

        /// <summary>
        /// The number of resources needed for a ship upgrade.
        /// </summary>
        public int NeededCount { get; set; }

        /// <summary>
        /// Updates the need count text
        /// </summary>
        public void Refresh()
        {

            if (string.IsNullOrEmpty(ItemId) || 
                !(Plugin.NeededResourceData.GetRequiresItem(ItemId, out int count)))
            {
                TextGameObject.SetActive(false);
                UiText?.SetText(string.Empty);
                NeededCount = -1;
            }
            else 
            {
                TextGameObject.SetActive(true);
                NeededCount = count;

                //TODO:  Why would this be null?
                if(UiText == null)
                {
                    Debug.LogError("UiText was null");
                }
                UiText?.SetText($"({count})");
            }

        }

        //public void OnDisable()
        //{
        //    TextGameObject.SetActive(false);
        //}

        //public void OnEnable()
        //{
        //    TextGameObject.SetActive(true);
        //}

        public ResourceCount()
        {
            var parent = gameObject.transform.gameObject.GetComponent<RectTransform>();
            transform.SetParent(parent);

            GameObject textGameObject = new GameObject(ComponentName);
            textGameObject.transform.SetParent(transform);

            
            TextGameObject = textGameObject;


            RectTransform rect = transform.GetComponent<RectTransform>();
            rect.anchoredPosition = Vector2.one;
            rect.anchorMin = rect.anchorMax = Vector2.one;

            rect.sizeDelta = new Vector2(16, 16);
            //Bottom right test
            //textGameObject.transform.localPosition = new Vector3(14.2244f, -25f, -1);
            //textGameObject.transform.localPosition = new Vector3(26.6265f, -25, -1);
            //Upper left
            textGameObject.transform.localPosition = new Vector3(14.2244f, 7.9611f, -1);

            TextMeshProUGUI text = textGameObject.gameObject.AddComponent<TextMeshProUGUI>();
            
            //text.autoSizeTextContainer = false;
            //text.enableAutoSizing = false;
            //text.horizontalAlignment = HorizontalAlignmentOptions.Right;
            //text.enableWordWrapping = false;

            text.fontSize = 12f;
            text.fontStyle = FontStyles.Bold;
            //text.lineSpacing = 1;
            //text.alignment = TextAlignmentOptions.Left;
            text.color = Color.white;
            text.outlineColor = Color.black;
            text.raycastTarget = false;
            UiText = text;


            Refresh();
        }
    }
}
