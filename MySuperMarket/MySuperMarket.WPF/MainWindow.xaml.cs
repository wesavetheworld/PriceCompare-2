using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using MySuperMarket.ClassLibrary;
using MySuperMarket.Mapper;
using MySuperMarket.DataLayer;

namespace MySuperMarket.WPF
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        //string _url1 = "http://pricesprodpublic.blob.core.windows.net/price/Price7290027600007-005-201607262230.gz?sv=2014-02-14&sr=b&sig=jtl9iT0UKc0fO1xqDK2bsAkorLUrpr4YFTGrPnfBqdo%3D&se=2016-08-15T07%3A21%3A13Z&sp=r";
        //string _file1 = "test.gz";
        string _filePath1 = @"C:\Users\Code Value User\Source\Repos\PriceCompare\MySuperMarket\MySuperMarket.ClassLibrary\bin\Debug\resources1";

        //string _url2 = "http://www.ybitan.co.il/upload/PriceFull7290725900003_093_201608150628.zip";
        //string _file2 = "test.gz";
        //string _filePath2 = @"C:\Users\Code Value User\Source\Repos\MySuperMarket\MySuperMarketWPF\MySuperMarketClassLibary\bin\Debug\resources2";

        Dictionary<Store, Dictionary<string, Product>> _storesDic;
        List<Cart> _cartPricesList;
        List<string> _userInput = new List<string>();
        PriceComparer comparer = new PriceComparer();
        DataMiner dataMiner = new DataMiner();
        DataOrganizer organizer = new DataOrganizer();
        string _userSearchItem = "";
        //EntityUploader entityUploader = new EntityUploader();

        public MainWindow()
        {
            InitializeComponent();
        }

        private void getDataButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                _storesDic = organizer.GetStoresDictionary(_filePath1);
                //entityUploader.UploadStoresDicToDB(_storesDic);
            }
            catch (Exception ext)
            {
                searchListBox.Items.Add(ext.Message);
                //throw ext;
            }
        }

        private void compareButton_Click(object sender, RoutedEventArgs e)
        {

            _cartPricesList = new List<Cart>();
            if (_storesDic != null && _userInput.Any())
            {
                _cartPricesList = comparer.GetCartPrices(_userInput, _storesDic);
            }
        }

        private void searchListBox_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            var selectedItem = searchListBox.SelectedItem.ToString();
            if (selectedItem != null && !_userInput.Contains(selectedItem))
            {
                _userInput.Add(selectedItem);
                chosenItemsListBox.Items.Add(selectedItem);
            }
        }

        private void inputTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (_userSearchItem != null)
            {
                _userSearchItem = ""; //redundent
            }
            _userSearchItem = inputTextBox.Text.ToString();
        }

        private void loadButton_Click(object sender, RoutedEventArgs e)
        {
            if (_cartPricesList != null && _cartPricesList.Any())
            {
                resultListBox.Items.Clear();
                foreach (var cart in _cartPricesList)
                {
                    resultListBox.Items.Add(cart);
                }
            }
        }

        private void clearSelectionButton_Click(object sender, RoutedEventArgs e)
        {
            if (chosenItemsListBox.Items != null && _userInput.Any())
            {
                _userInput.Clear();
                chosenItemsListBox.Items.Clear();
            }
        }

        private void searchButton_Click(object sender, RoutedEventArgs e)
        {
            searchListBox.Items.Clear();
            var productsList = new List<Product>();
            if (_storesDic != null && _userSearchItem != "")
            {
                productsList = comparer.SearchForProduct(_userSearchItem, _storesDic);
                if (productsList.Any())
                {
                    foreach (var item in productsList)
                    {
                        searchListBox.Items.Add(item.Name);
                    }
                }
            }
        }

        private void resultListBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            expendedCartListBox.Items.Clear();
            var selectedItem = resultListBox.SelectedItem;
            if (selectedItem != null)
            {
                if (selectedItem.GetType() == typeof(Cart))
                {
                    var cart = selectedItem as Cart;
                    foreach (var item in cart.Products)
                    {
                        expendedCartListBox.Items.Add($"{item.Name} - {item.PricePerUOM} ₪");
                    }
                }
            }
        }
    }
}
