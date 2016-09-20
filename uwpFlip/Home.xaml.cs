using Microsoft.AdMediator.Core.Models;
using Microsoft.Advertising.WinRT.UI;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Threading.Tasks;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.Storage;
using Windows.System;
using Windows.UI.Popups;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Media.Imaging;
using Windows.UI.Xaml.Navigation;

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=234238

namespace uwpFlip
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class Home : Page
    {
        public Home()
        {
            this.InitializeComponent();
        }

        StorageFolder localFolder;
        Product p = new Product();
        ProductList p1 = new ProductList();
        List<ProductList> items = new List<ProductList>();
        List<ProductList> items1 = new List<ProductList>();
        List<ProductList> itemsDelet = new List<ProductList>();
        InterstitialAd MyVideoAd = new InterstitialAd();
        string MyAppId;
        string MyAdUnitId;
        protected override async void OnNavigatedTo(NavigationEventArgs e)
        {
            Loaded += Home_Loaded;
            await refresh();   
        }

        private void Home_Loaded(object sender, RoutedEventArgs e)
        {
            AdMediator_24D058.AdSdkTimeouts[AdSdkNames.MicrosoftAdvertising] = TimeSpan.FromSeconds(10);
            AdMediator_6351B3.AdSdkTimeouts[AdSdkNames.MicrosoftAdvertising] = TimeSpan.FromSeconds(10);
            bool isHardwareButtonsAPIPresent =
                Windows.Foundation.Metadata.ApiInformation.IsTypePresent("Windows.Phone.UI.Input.HardwareButtons");
            if (isHardwareButtonsAPIPresent)
            {
                AdMediator_6351B3.Width = 640;
                AdMediator_24D058.Width = 640;

                AdMediator_6351B3.Height = 100;
                AdMediator_24D058.Height = 100;


                MyAppId = "	7b01e63c-94b9-461a-a6b1-0f84540e8a9c";
                MyAdUnitId = "11644027";
            }
            else
            {
                MyAppId = "fbbb7779-4ffd-4c42-92ad-526be5070406";
                MyAdUnitId = "11644017";
            }
            MyVideoAd.AdReady += MyVideoAd_AdReady;
            MyVideoAd.ErrorOccurred += MyVideoAd_ErrorOccurred;
            MyVideoAd.Completed += MyVideoAd_Completed;
            MyVideoAd.Cancelled += MyVideoAd_Cancelled;
            MyVideoAd.RequestAd(AdType.Video, MyAppId, MyAdUnitId);

            if ((InterstitialAdState.Ready) == (MyVideoAd.State))
            {
                MyVideoAd.Show();
            }

        }

        void MyVideoAd_AdReady(object sender, object e)
        {

            //AdMediator_6351B3.Pause();
            //AdMediator_24D058.Pause();
            if ((InterstitialAdState.Ready) == (MyVideoAd.State))
            {
                MyVideoAd.Show();
            }
        }

        void MyVideoAd_ErrorOccurred(object sender, AdErrorEventArgs e)
        {
            AdMediator_6351B3.Resume();
            AdMediator_24D058.Resume();
        }

        void MyVideoAd_Completed(object sender, object e)
        {
            AdMediator_24D058.Resume();
            AdMediator_6351B3.Resume();
        }

        void MyVideoAd_Cancelled(object sender, object e)
        {
            AdMediator_6351B3.Resume();
            AdMediator_24D058.Resume();
        }
        private async Task refresh()
        {
            List<char> pri = new List<char>();

            try
            {
                localFolder = ApplicationData.Current.LocalFolder;
                StorageFile sampleFile = await localFolder.CreateFileAsync("dataFile.txt", CreationCollisionOption.OpenIfExists);
                IList<string> y = new List<string>();
                y = await FileIO.ReadLinesAsync(sampleFile);
                string change = File.ReadAllText(sampleFile.Path);
                if(y.Count == 0)
                {
                    await (new MessageDialog("No Item added")).ShowAsync();
                    return;

                }
                HttpClient cl = new HttpClient();
                foreach(string item in y)
                {
                    p = JsonConvert.DeserializeObject<Product>(item);
                    string str = await cl.GetStringAsync(p.url);
                    p1.price = p.price.ToString();
                    if (str.Length>1000)
                    {
                        p1.current_Price = "Present Price: " + "out of stock";
                        goto lol;
                    }

                 //   int j = str.IndexOf(';');
                    char[] arr = str.ToCharArray();
                    pri.Clear();
                    foreach (char p in arr)
                    {
                        if (char.IsDigit(p) || p == '.')
                        {
                            pri.Add(p);
                        }
                    }
                    str = string.Join("", pri.ToArray());
                    float i = float.Parse(str);

                    p1.current_Price = i.ToString();
                    //if (i < p.price)
                    //{
                    //    p.price = i;
                    //}

                    
                    lol:
                    p1.name = p.name;

                    if (p.url.Contains("snapdeal"))
                    {
                        p1.price = "Rs. " + p.price.ToString();
                        p1.current_Price = "Present Price: " + "Rs. " + p1.current_Price;
                        p1.bitmapImage = new BitmapImage(new Uri(this.BaseUri, "/Assets/snapdeal.png"));
                        items.Add(p1);
                    }
                    else if (p.url.Contains("flipkart"))
                    {
                        p1.price = "Rs. " + p.price.ToString();
                        p1.current_Price = "Present Price: " + "Rs. " + p1.current_Price;
                        p1.bitmapImage = new BitmapImage(new Uri(this.BaseUri, "/Assets/Flipkart.png"));
                        items.Add(p1);
                    }
                    else if (p.url.Contains("amazon.com"))
                    {
                        p1.price = "$ " + p.price.ToString();
                        p1.current_Price = "Present Price: " + "$ " + p1.current_Price;
                        p1.bitmapImage = new BitmapImage(new Uri(this.BaseUri, "/Assets/amazon_usa.jpg"));
                        items.Add(p1);
                    }
                    else if (p.url.Contains("amazon.in"))
                    {
                        p1.price ="Rs. " + p.price.ToString();
                        p1.current_Price = "Present Price: " + "Rs. " + p1.current_Price;
                        p1.bitmapImage = new BitmapImage(new Uri(this.BaseUri, "/Assets/amazon_india.jpg"));
                        items.Add(p1);
                    }
                    else if (p.url.Contains("amazon.co.uk"))
                    {
                        p1.price ="GBP " + p.price.ToString();
                        p1.current_Price = "Present Price: " + "GBP " + p1.current_Price;
                        p1.bitmapImage = new BitmapImage(new Uri(this.BaseUri, "/Assets/amazon_uk.jpg"));
                        items.Add(p1);
                    }
                    else if (p.url.Contains("amazon.de"))
                    {
                        p1.price ="EUR " + p.price.ToString();
                        p1.current_Price = "Present Price: " + "EUR " + p1.current_Price;
                        p1.bitmapImage = new BitmapImage(new Uri(this.BaseUri, "/Assets/amazon_germany.jpg"));
                        items.Add(p1);
                    }
                    else if (p.url.Contains("amazon.it"))
                    {
                        p1.price ="EUR " + p.price.ToString();
                        p1.current_Price = "Present Price: " + "EUR " + p1.current_Price;
                        p1.bitmapImage = new BitmapImage(new Uri(this.BaseUri, "/Assets/amazon_italy.jpg"));
                        items.Add(p1);
                    }
                    else if (p.url.Contains("amazon.co.jp"))
                    {
                        p1.price = "YEN " + p.price.ToString();
                        p1.current_Price = "Present Price: " + "YEN " + p1.current_Price;
                        p1.bitmapImage = new BitmapImage(new Uri(this.BaseUri, "/Assets/amazon_japan.jpg"));
                        items.Add(p1);
                    }
                    else if (p.url.Contains("jabong"))
                    {
                        p1.price ="Rs. " + p.price.ToString();
                        p1.current_Price = "Present Price: " + "Rs. " + p1.current_Price;
                        p1.bitmapImage = new BitmapImage(new Uri(this.BaseUri, "/Assets/JABONG.jpeg"));
                        items.Add(p1);
                    }
                    p1 = new ProductList();
                }

                eventF.DataContext = items;
            //    eventS.DataContext = items1;
                await (new MessageDialog("Prices Updated")).ShowAsync();

            }
            catch (Exception)
            {

                await (new MessageDialog("Prices can't be updated now .Please try again later")).ShowAsync();
            }
        }

        private void HamburgerButton_Click(object sender, RoutedEventArgs e)
        {
            MySplitView.IsPaneOpen = !MySplitView.IsPaneOpen;
        }

        private void MenuButton1_Click(object sender, RoutedEventArgs e)
        {
            Frame.Navigate(typeof(Home));
        }

        private void MenuButton2_Click(object sender, RoutedEventArgs e)
        {
            Frame.Navigate(typeof(Settings));
        }

        private void MenuButton3_Click(object sender, RoutedEventArgs e)
        {
            Frame.Navigate(typeof(Help));
        }

        private async void MenuButton4_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                await Launcher.LaunchUriAsync(new Uri(string.Format("ms-windows-store:REVIEW?PFN={0}", Windows.ApplicationModel.Package.Current.Id.FamilyName)));
            }
            catch(Exception es)
            {
                await (new MessageDialog("Can't review now please try again later")).ShowAsync();
            }
        }

        private void MenuButton5_Click(object sender, RoutedEventArgs e)
        {
            Frame.Navigate(typeof(About));
        }

        private async void refresh_Click(object sender, RoutedEventArgs e)
        {
            await refresh();
        }
   
        private void Add_Click(object sender, RoutedEventArgs e)
        {
            Frame.Navigate(typeof(AddUrl));
        }
        private async void DeleteBut_Click(object sender, RoutedEventArgs e)
        {
            var dialog = new MessageDialog("Are you sure?");
            dialog.Title = "Really?";
            dialog.Commands.Add(new UICommand { Label = "Ok", Id = 0 });
            dialog.Commands.Add(new UICommand { Label = "Cancel", Id = 1 });
            var res = await dialog.ShowAsync();

            if ((int)res.Id == 0)
            {

                var tag = (sender as Button).Tag;
                string nam = tag.ToString();
                List<string> urlList = new List<string>();

                try
                {
                    localFolder = ApplicationData.Current.LocalFolder;
                    StorageFile sampleFile = await localFolder.GetFileAsync("dataFile.txt");
                    IList<string> y = new List<string>();
                    y = await FileIO.ReadLinesAsync(sampleFile);
                    string change = File.ReadAllText(sampleFile.Path);
                    foreach (string item in y)
                    {
                        p = JsonConvert.DeserializeObject<Product>(item);

                        if (!(string.Equals(p.name, nam)))
                        {
                            var contentsToWriteToFile = JsonConvert.SerializeObject(p);
                            urlList.Add(contentsToWriteToFile);
                        }
                    }
                    await FileIO.WriteLinesAsync(sampleFile, urlList);
                    await (new MessageDialog("Deleted Successfully ")).ShowAsync();
                    Frame.Navigate(typeof(Home));
                }

                catch (Exception)
                {
                    await (new MessageDialog("Can't Delete now please try again later")).ShowAsync();
                }
            }
        }
    }
}
