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
        private Dictionary<string, int> NeededItems { get; set; } = new Dictionary<string, int>();

        public event EventHandler<DataUpdateArgs> OnDataUpdated = null;


        /// <summary>
        /// Causes the system to reload the data.
        /// </summary>
        public void Update()
        {
            NeededItems = GetNeededResources();
            OnDataUpdated?.Invoke(this, new DataUpdateArgs(this));
        }

        public bool GetRequiresItem(string id)
        {
            return NeededItems.ContainsKey(id);
        }

        public bool GetRequiresItem(string id, out int count)
        {
            return NeededItems.TryGetValue(id, out count);
        }

        /// <summary>
        /// Returns the list of resources that are required to upgrade the remaining 
        /// Magnum perks.
        /// </summary>
        /// <returns></returns>
        private static Dictionary<string, int> GetNeededResources()
        {
            MagnumProgression progression = Plugin.State.Get<MagnumProgression>();
            IEnumerable<MagnumPerkRecord> perks = Data.MagnumPerks.Records;

            Dictionary<string, int> neededResources = perks.Where(x => x.Enabled == true && !progression.IsPerkPurchased(x.Id))
                .SelectMany(x => x.UpgradePrice)
                .GroupBy(x => x)
                .Select(x => (x.Key, Count: x.Count()))
                .OrderBy(x => x.Key)
                .ToDictionary(x => x.Key, x => x.Count);

            return neededResources;
        }

    }
}
