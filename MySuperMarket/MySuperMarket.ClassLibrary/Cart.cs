using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MySuperMarket.ClassLibrary
{
    public class Cart
    {
        [Key]
        public string Id { get; private set; }
        public Store Store { get; private set; }
        public List<Product> Products { get; private set; }
        public decimal Sum { get; private set; }

        public Cart(Store store, List<Product> products)
        {
            Store = store;
            Id = Store.FullId + Sum.ToString(); // not the smartest way to represent this
            Products = products;
            Sum = GetSum(products);
        }

        private decimal GetSum(List<Product> products)
        {
            decimal sum = 0;
            foreach (var item in products)
            {
                sum += item.PricePerUOM;
            }
            return sum;
        }

        public override string ToString()
        {
            string str = $"{Store.ChainName}, סניף {Store.StoreId} סכום כולל לסל: {Sum} ₪";
            return str.ToString(); // redundent, but should keep for safe measure
        }
    }
}
