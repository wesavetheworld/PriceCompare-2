using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using System.IO;
using MySuperMarket.ClassLibrary;

namespace MySuperMarket.Mapper
{
    public class DataOrganizer
    {
        DataMiner miner = new DataMiner();

        // accepts a directoy path, returns a stores dictionary
        public Dictionary<Store, Dictionary<string, Product>> GetStoresDictionary(string gzDirectory)
        {
            var xDocArray = CreateStoresXdocArray(gzDirectory);
            var storesDic = CreateStoresDictionary(xDocArray);
            return storesDic;
        }

        // accept a directory path, returns an array of xdocs, for each gz file in directory
        private List<XDocument> CreateStoresXdocArray(string path) // this is dangerous, you need to handle other files, invalid files, in the directory
        {
            XDocument xDoc;
            var list = new List<XDocument>();
            if (Directory.Exists(path) && Directory.GetFiles(path).Any())
            {
                foreach (var file in Directory.GetFiles(path))
                {
                    xDoc = miner.UnzipFileToXDocument(file); // need to accept gz OR xml, and check for duplicates!
                    list.Add(xDoc);
                }
            }
            else
            {
                throw new FileNotFoundException(); // wrap caller with try catch
            }
            return list;
        }

        // accept single store product xml, returns dictionary of products
        private Dictionary<string, Product> CreateProductsDictionary(XDocument xDoc)
        {
            var tempXEl = xDoc.Root.Element("ChainId") ?? xDoc.Root.Element("ChainID");
            var chainID = tempXEl.Value ?? "-1"; // this will only work once, the second time an exception will be thrown
            var chainName = GetChainName(chainID);
            tempXEl = xDoc.Root.Element("StoreId") ?? xDoc.Root.Element("StoreID");
            var storeID = tempXEl.Value;

            var elements =
                from el in xDoc.Root.Descendants("ItemName")
                select el.Parent;

            var dictionary = new Dictionary<string, Product>();
            string name, unitOfMeasurement;
            string id;
            decimal price;
            foreach (var item in elements)
            {
                name = item.Element("ItemName").Value;
                if (!dictionary.ContainsKey(name))
                {
                    if (!decimal.TryParse(item.Element("ItemPrice").Value, out price))
                    {
                        price = -1;
                    }

                    id = item.Element("ItemCode").Value;
                    tempXEl = item.Element("UnitQty") ?? item.Element("Quantity");
                    unitOfMeasurement = tempXEl.Value;
                    dictionary.Add(name, new Product(id, name, price, unitOfMeasurement, chainName, storeID));
                }
            }
            return dictionary;
        }
        // accept chain Id, returns chain name
        private string GetChainName(string id)
        {
            string name;
            switch (id)
            {
                case "7290700100008":
                    name = "כלבו חצי חינם";
                    break;
                case "7290633800006":
                    name = "קו-אופ";
                    break;
                case "":
                    name = "קו-אופ";
                    break;
                case "7290492000005":
                    name = "דור אלון";
                    break;
                case "7290055755557":
                    name = "עדן טבע מרקט";
                    break;
                case "7290876100000":
                    name = "פרש מרקט";
                    break;
                case "7290785400000":
                    name = "קשת טעמים";
                    break;
                case "7290661400001":
                    name = "מחסני השוק";
                    break;
                case "7290058179503":
                    name = "מחסני להב";
                    break;
                case "7290055700007":
                    name = "מגה";
                    break;
                case "7290103152017":
                    name = "אושר עד";
                    break;
                case "7290058140886":
                    name = "רמי לוי";
                    break;
                case "7290027600007":
                    name = "שופרסל";
                    break;
                case "7290873900009":
                    name = "סופר דוש";
                    break;
                case "7290803800003":
                    name = "סופרשוק יוחננוף";
                    break;
                case "7290873255550":
                    name = "טיב טעם";
                    break;
                case "7290696200003":
                    name = "ויקטורי";
                    break;
                case "7290725900003":
                    name = "יינות ביתן";
                    break;
                default:
                    name = "לא ידוע";
                    break;
            }
            return name;
        }

        // accept array of store product xml, return a dictionary of stores
        private Dictionary<Store, Dictionary<string, Product>> CreateStoresDictionary(List<XDocument> storesList)
        {
            var storesDictionary = new Dictionary<Store, Dictionary<string, Product>>();
            Dictionary<string, Product> internalDic;
            Store tempStore;
            XElement tempXEl;
            string chainID, chainName;
            foreach (var store in storesList)
            {
                tempXEl = store.Root.Element("ChainId") ?? store.Root.Element("ChainID");
                chainID = tempXEl.Value ?? "-1"; // this will only work once, the second time an exception will be thrown
                tempXEl = store.Root.Element("StoreId") ?? store.Root.Element("StoreID");
                var storeID = tempXEl.Value;
                chainName = GetChainName(chainID);
                internalDic = CreateProductsDictionary(store);
                tempStore = new Store(chainID, chainName, storeID);
                if (!storesDictionary.ContainsKey(tempStore))
                {
                    storesDictionary.Add(tempStore, internalDic);
                }
            }
            return storesDictionary;
        }
    }
}
