using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MySuperMarket.ClassLibrary
{
    public class Product : IComparable
    {
        [Key]
        public string Id { get; private set; }
        public string Name { get; private set; }
        public decimal PricePerUOM { get; private set; }
        public string UnitOfMeasurement { get; private set; }
        //public enum QtyTypes
        //{
        //    grams,
        //    kilos,
        //    units
        //};
        public string ChainId { get; private set; } // think of a better way to update the cahin id
        public string StoreId { get; private set; }

        public Product(string id, string name, decimal pricePerUOM, string unitOfMeasurement, string chainID, string storeID)
        {
            Id = id;
            Name = name;
            PricePerUOM = pricePerUOM;
            UnitOfMeasurement = unitOfMeasurement;
            ChainId = chainID;
            StoreId = storeID;
        }

        public int CompareTo(object obj) // shouldnt I override this?
        {
            try
            {
                Product otherProduct = obj as Product;
                return PricePerUOM.CompareTo(otherProduct.PricePerUOM);
            }
            catch (InvalidCastException ext)
            {
                Debug.WriteLine($"Unable to cast from object to product. Exception message {ext.Message}");
            }
            return 0;
        }

        public override bool Equals(object obj)
        {
            if (obj == null || obj.GetType() != typeof(Product))
            {
                return false;
            }
            var productObj = obj as Product;
            return Name.Equals(productObj.Name);
        }

        public override int GetHashCode()
        {
            return Name.GetHashCode();
        }
    }
}
