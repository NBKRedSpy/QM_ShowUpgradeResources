using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace QM_ShowUpgradeResources
{
    public class DataUpdateArgs : EventArgs
    {
        public NeededResourceData NeededResourceData { get; set; }

        public DataUpdateArgs(NeededResourceData neededResourceData)
        {
            NeededResourceData = neededResourceData;
        }
    }
}
