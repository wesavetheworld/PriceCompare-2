using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MySuperMarket.ClassLibrary;
using MySuperMarket.Mapper;

namespace MySuperMarket.DataLayer
{
    public class EntityUploader
    {
        Dictionary<Store, Dictionary<string, Product>> _storesDic;
        List<string> _userInput = new List<string>();
        PriceComparer comparer = new PriceComparer();
        DataMiner dataMiner = new DataMiner();
        DataOrganizer organizer = new DataOrganizer();

        public void UploadStoresDicToDB(Dictionary<Store, Dictionary<string, Product>> storesDic)
        {
            foreach (var store in storesDic)
            {
                using (var context = new MySuperMarketContext())
                {
                    context.Stores.Add(store.Key);
                    foreach (var product in store.Value)
                    {
                        context.Products.Add(product.Value);
                    }
                    context.SaveChanges();
                }
            }
        }

        public void UpdateDataBase()
        {

        }

        public void ClearDataBase()
        {

        }

        public void DeleteEntryFromDataBase()
        {

        }

        public object GetDataFromDataBase()
        {
            return new object();
        }
    }
}
