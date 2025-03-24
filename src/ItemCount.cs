using MGSC;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Unity.Jobs.LowLevel.Unsafe;

namespace QM_ShowUpgradeResources
{
    public class ItemCount
    {
        /// <summary>
        /// The ID of the item that this record is for.
        /// </summary>
        public string ItemId { get; set; }

        /// <summary>
        /// The count in all inventory, including the mercenary.
        /// </summary>
        public int CountInAllInventory { get; set; }

        /// <summary>
        /// Only the amount that is in the ship's inventory.  Not mercs in raid or being cloned.
        /// </summary>
        public int CountInOnlyShip { get; set; }

        /// <summary>
        /// The total amount of this resource that is needed for all upgrades.  Does not include any inventory.
        /// </summary>
        public int UpgradeCount { get; set; }

        /// <summary>
        /// Returns the number of items needed for an upgrade.
        /// </summary>
        /// <param name="shipOnly">If true, does not include the inventory of mercs that are cloning or in a raid.</param>
        /// <returns>The count of resources that are needed for all upgrades, minus what is in inventory. </returns>
        public int RemainingCount(bool shipOnly)
        {
            int countRequired;

            if (shipOnly)
            {
                countRequired = UpgradeCount - CountInOnlyShip;
            }
            else
            {
                countRequired = UpgradeCount - CountInAllInventory;
            }

            return Math.Max(0, countRequired);
        }

        public ItemCount()
        {
        }
    }
}
