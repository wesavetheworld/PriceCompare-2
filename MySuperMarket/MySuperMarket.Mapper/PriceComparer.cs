using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using MySuperMarket.ClassLibrary;

namespace MySuperMarket.Mapper
{
    public class PriceComparer
    {
        public SortedQueue<Product> GetMostExpansive(Dictionary<string, Product> shoppingCartDic)
        {
            var queue = new SortedQueue<Product>(3);
            foreach (var item in shoppingCartDic)
            {
                if (item.Value.CompareTo(queue.First) >= 0)
                {
                    queue.Add(item.Value);
                }
            }
            return queue;
        }

        public SortedQueue<Product> GetCheapest(Dictionary<string, Product> shoppingCartDic)
        {
            var queue = new SortedQueue<Product>(3);
            foreach (var item in shoppingCartDic)
            {
                if (item.Value.CompareTo(queue.Last) <= 0)
                {
                    queue.Add(item.Value);
                }
            }
            return queue;
        }

        // accepts an query product and a stores dictionary, returns a list of products matching to query, from all stores
        public List<Product> SearchForProduct(string queryProduct, Dictionary<Store, Dictionary<string, Product>> allStoresDic)
        {
            var productList = new List<Product>(); // perhaps more efficient to do a list of strings
            foreach (var store in allStoresDic)
            {
                foreach (var productKV in store.Value)
                {
                    if (productKV.Value.Name.Contains(queryProduct) && !productList.Contains(productKV.Value))
                    {
                        productList.Add(productKV.Value);
                    }
                }
            }
            productList.Sort((x, y) => string.Compare(x.Name, y.Name));
            return productList;
        }

        // accepts an array of querried products and a stores dictionary, returns a list of dictionaries, each representing the bag in a different store
        public List<Cart> GetCartPrices(List<string> queryProductsArray, Dictionary<Store, Dictionary<string, Product>> allStoresDic)
        {
            var cartsList = new List<Cart>();
            var storeCartList = new List<Product>();

            foreach (var store in allStoresDic)
            {
                foreach (var queryProduct in queryProductsArray)
                {
                    foreach (var productKV in store.Value)
                    {
                        if (productKV.Value.Name.Contains(queryProduct))
                        {
                            storeCartList.Add(productKV.Value); // need to signal when product is unavailable
                        }
                    }
                }
                cartsList.Add(new Cart(new Store(store.Key), new List<Product>(storeCartList)));
                storeCartList.Clear();
            }
            return cartsList;
        }

        // accepts all cart and returns the cheapest one
        public Dictionary<string, List<Product>> CheckCheapestCart(Dictionary<string, List<Product>> allCarts)
        {
            var cheapestCart = new Dictionary<string, List<Product>>(); // this is wastefull, find a better way to mark a name/cart pair
            decimal currentCheapestSum = SumProducts(allCarts.First().Value);
            decimal iteratedSum;
            foreach (var cart in allCarts)
            {
                iteratedSum = SumProducts(cart.Value);
                if (iteratedSum < currentCheapestSum)
                {
                    currentCheapestSum = iteratedSum;
                    cheapestCart.Clear(); // wastefull
                    cheapestCart.Add(cart.Key, cart.Value); // again, this is wastefull, think of a better way!
                }
                currentCheapestSum = iteratedSum < currentCheapestSum ? iteratedSum : currentCheapestSum;
            }
            return cheapestCart;
        }

        public decimal SumProducts(ICollection<Product> collection)
        {
            decimal sum = 0;
            foreach (var item in collection)
            {
                sum += item.PricePerUOM;
            }
            return sum;
        }
    }
}
