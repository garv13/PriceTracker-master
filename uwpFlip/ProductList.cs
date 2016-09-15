using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml.Media.Imaging;

namespace uwpFlip
{
    class ProductList
    {
        public string name { get; set; }
        public string price { get; set; }

        public string current_Price { get; set; }
        public BitmapImage bitmapImage { get; set; }
    }
}
