using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace MySuperMarket.ClassLibrary
{
    public class Store
    {
        public string ChainId { get; private set; }
        public string ChainName { get; private set; }
        public string StoreId { get; private set; }
        [Key]
        public string FullId { get; private set; }
        public string Location { get; private set; }

        public Store(string chainID, string chainName, string storeID, string location)
        {
            ChainId = chainID;
            ChainName = chainName;
            StoreId = storeID;
            FullId = chainID + "-" + storeID;
            Location = location;
        }

        public Store(string chainID, string chainName, string storeID) : this(chainID, chainName, storeID, "לא ידוע")
        {
        }

        public Store(Store store)
        {
            ChainId = store.ChainId;
            ChainName = store.ChainName;
            StoreId = store.StoreId;
            FullId = store.FullId;
            Location = store.Location;
        }



        public override bool Equals(object obj)
        {
            if (obj == null || GetType() != obj.GetType())
            {
                return false;
            }
            var storeObj = obj as Store;
            return FullId.Equals(storeObj.FullId);
        }

        public override int GetHashCode()
        {
            return FullId.GetHashCode();
        }
    }
}
