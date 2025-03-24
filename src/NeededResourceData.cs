using MGSC;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QM_ShowUpgradeResources
{
    /// <summary>
    /// Handles the data for resources that are needed for the Magnum upgrades.
    /// </summary>
    public class NeededResourceData
    {
        /// <summary>
        /// The items that are needed.
        /// Key is the item ID.  The value is the count of items needed.
        /// </summary>
        private Dictionary<string, ItemCount> NeededItems { get; set; } = new Dictionary<string, ItemCount>();

        public event EventHandler<DataUpdateArgs> OnDataUpdated = null;


        /// <summary>
        /// Causes the system to reload the data.
        /// </summary>
        public void Update()
        {
            NeededItems = GetNeededResources();
            OnDataUpdated?.Invoke(this, new DataUpdateArgs(this));

            //ItemInteractionSystem.Count(_state.Get<Mercenaries>(), MagnumCargo, itemId);
            //AddItemIconWithQuantity(0, itemId, available, record.UpgradePrice.Count((string s) => s == itemId));

        }

        /// <summary>
        /// Returns true if an unlocked upgrade requires this item.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public bool UnpurchasedUpgradesRequiresItem(string id, out int count)
        {
            if (string.IsNullOrEmpty(id) || !NeededItems.TryGetValue(id, out ItemCount itemCount))
            {
                count = 0;
                return false;
            }

            count = itemCount.UpgradeCount;
            return count > 0;
        }

        public bool RequiresItemAfterShipInventory(string id)
        {
            return GetRequiresItem(id, true, out _);
        }
        
        /// <summary>
        /// Returns the number of this resource that is needed to complete upgrades that is not 
        /// in the ship's inventory.
        /// </summary>
        /// <param name="id"></param>
        /// <param name="shipOnly"></param>
        /// <param name="count"></param>
        /// <returns></returns>
        public bool GetRequiresItem(string id, bool shipOnly, out int count)
        {
            count = 0;

            if (string.IsNullOrEmpty(id) || !NeededItems.TryGetValue(id, out ItemCount itemCount))
            {
                return false;
            }

            count = itemCount.RemainingCount(shipOnly);

            return count > 0;
        }

        /// <summary>
        /// Returns the list of resources that are required to upgrade the remaining 
        /// Magnum perks.
        /// </summary>
        /// <returns></returns>
        private static Dictionary<string, ItemCount> GetNeededResources()
        {
            MagnumProgression progression = Plugin.State.Get<MagnumProgression>();
            IEnumerable<MagnumPerkRecord> perks = Data.MagnumPerks.Records;

            //Mercs that are not in in a raid or cloning.
            //  Not sure why the game omits mercs that are cloning.  Maybe to prevent a null ref?


            List<Mercenary> availableMercsList = Plugin.State.Get<Mercenaries>().Values
                .Where(x => x.State != MercenaryState.Cloning &&
                    x.State != MercenaryState.InRaid)
                .ToList();


            //The count code only uses the Values property.
            Mercenaries availableMercs = new Mercenaries();
            availableMercs.Values = availableMercsList;

            var neededResources =
                perks
                    .Where(x => x.Enabled == true && !progression.IsPerkPurchased(x.Id))
                    .SelectMany(x => x.UpgradePrice)
                    .GroupBy(x => x)
                    .Select(x => new ItemCount()
                    {
                        ItemId = x.Key,
                        UpgradeCount = x.Count(),       
                        CountInAllInventory =
                                ItemInteractionSystem
                                    .Count(
                                        Plugin.State.Get<Mercenaries>(),
                                        Plugin.State.Get<MagnumCargo>(), x.Key),
                        CountInOnlyShip =
                                ItemInteractionSystem
                                    .Count(
                                        availableMercs,  //Not cloning or in raid.
                                        Plugin.State.Get<MagnumCargo>(), x.Key),
                    }
                    )
               .ToDictionary(x => x.ItemId, x => x);

            return neededResources;
        }

    }
}
