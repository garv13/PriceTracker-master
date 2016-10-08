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
using VungleSDK;
using Windows.ApplicationModel.Core;
using Windows.UI.Core;
using Windows.ApplicationModel.Background;
using Windows.System.Profile;
// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=234238

namespace uwpFlip
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class Home : Page
    {
        private int AD_WIDTH = 728;
        private int AD_HEIGHT = 90;
        private const string WAPPLICATIONID = "fbbb7779-4ffd-4c42-92ad-526be5070406";
        private const string WADUNITID = "11644012";
        private const string MAPPLICATIONID = "7b01e63c-94b9-461a-a6b1-0f84540e8a9c";
        private const string MADUNITID = "11644022";

        private const string WAPPLICATIONID1 = "fbbb7779-4ffd-4c42-92ad-526be5070406";
        private const string WADUNITID1 = "11644013";
        private const string MAPPLICATIONID1 = "7b01e63c-94b9-461a-a6b1-0f84540e8a9c";
        private const string MADUNITID1 = "	11644023";

        private AdControl myAdControl = null;
        private AdControl myAdControl1 = null;
        private string myAppId = WAPPLICATIONID;
        private string myAdUnitId = WADUNITID;

        private string myAppId1 = WAPPLICATIONID1;
        private string myAdUnitId1 = WADUNITID1;

        public Home()
        {
            this.InitializeComponent();

          

            // For mobile device families, use the mobile ad unit info.
            if ("Windows.Mobile" == AnalyticsInfo.VersionInfo.DeviceFamily)
            {
                myAppId = MAPPLICATIONID;
                myAdUnitId = MADUNITID;
                myAppId1 = MAPPLICATIONID1;
                myAdUnitId1 = MADUNITID1;
                AD_HEIGHT = 50;
                AD_WIDTH = 300;
            }

            myAdGrid.Width = AD_WIDTH;
            myAdGrid.Height = AD_HEIGHT;

            myAdGrid1.Width = AD_WIDTH;
            myAdGrid1.Height = AD_HEIGHT;

            // Initialize the AdControl.
            myAdControl = new AdControl();
            myAdControl.ApplicationId = myAppId;
            myAdControl.AdUnitId = myAdUnitId;
            myAdControl.Width = AD_WIDTH;
            myAdControl.Height = AD_HEIGHT;
            myAdControl.IsAutoRefreshEnabled = true;

            myAdGrid.Children.Add(myAdControl);
            myAdControl.IsAutoRefreshEnabled = true;
            myAdControl.AutoRefreshIntervalInSeconds = 5;


            myAdControl1 = new AdControl();
            myAdControl1.ApplicationId = myAppId1;
            myAdControl1.AdUnitId = myAdUnitId1;
            myAdControl1.Width = AD_WIDTH;
            myAdControl1.Height = AD_HEIGHT;
            myAdControl1.IsAutoRefreshEnabled = true;

            myAdGrid1.Children.Add(myAdControl1);
            myAdControl1.IsAutoRefreshEnabled = true;
            myAdControl1.AutoRefreshIntervalInSeconds = 5;

        }

        StorageFolder localFolder;
        Product p = new Product();
        ProductList p1 = new ProductList();
        List<ProductList> items = new List<ProductList>();
        List<ProductList> items1 = new List<ProductList>();
        List<ProductList> itemsDelet = new List<ProductList>();
        InterstitialAd MyVideoAd = new InterstitialAd();
        VungleAd sdkInstance;
        bool check = true;
        protected override async void OnNavigatedTo(NavigationEventArgs e)
        {
            check = true;
            Loaded += Home_Loaded;
            await refresh();   
        }
        private async void Home_Loaded(object sender, RoutedEventArgs e)
        {
            string myTaskName = "priceTask";
            bool flag = true;
            try
            {
                foreach (var cur in BackgroundTaskRegistration.AllTasks)
                    if (cur.Value.Name == myTaskName)
                        flag = false;
                if (flag)
                {
                    var allowed = await BackgroundExecutionManager.RequestAccessAsync();
                    // register a new task
                    if ((allowed == BackgroundAccessStatus.AllowedMayUseActiveRealTimeConnectivity) || (allowed == BackgroundAccessStatus.AllowedWithAlwaysOnRealTimeConnectivity))
                    {
                        BackgroundTaskBuilder taskBuilder = new BackgroundTaskBuilder { Name = "priceTask", TaskEntryPoint = "MyTask.Class1" };
                        taskBuilder.SetTrigger(new TimeTrigger(120, false));
                        BackgroundTaskRegistration myFirstTask = taskBuilder.Register();
                    }
                }
                sdkInstance = AdFactory.GetInstance("57e8278aee5b9daa1d000048");
                sdkInstance.OnAdPlayableChanged += SdkInstance_OnAdPlayableChanged;
                if (sdkInstance.AdPlayable)
                    SdkInstance_OnAdPlayableChanged(this,new AdPlayableEventArgs(true));
            }
            catch (Exception)
            {

            }
        }

        private async void SdkInstance_OnAdPlayableChanged(object sender, AdPlayableEventArgs e)
        {
            if (sdkInstance.AdPlayable && check)
            {
                if(myAdControl.HasAd)
                    myAdControl.Suspend();
                if(myAdControl1.HasAd)
                    myAdControl1.Suspend();
                await sdkInstance.PlayAdAsync(new AdConfig());
                check = false;
            }
            myAdControl.Resume();
            myAdControl1.Resume();
        }
        private async Task refresh()
        {
            No_Item_Text.Visibility = Visibility.Collapsed;
            loading_Bar.IsIndeterminate = true;
            loading_Bar.Visibility = Visibility.Visible;
            Refresh_Button.IsEnabled = false;
            Refresh_Button.IsChecked = false;
           
            List<char> pri = new List<char>();

            try
            {
                localFolder = ApplicationData.Current.LocalFolder;
                StorageFile sampleFile = await localFolder.CreateFileAsync("dataFile.txt", CreationCollisionOption.OpenIfExists);
                StorageFile adsFile = await localFolder.CreateFileAsync("addCheckFile.txt", CreationCollisionOption.OpenIfExists);
                await FileIO.WriteTextAsync(adsFile, DateTime.Now.ToString());

                IList<string> y = new List<string>();
                y = await FileIO.ReadLinesAsync(sampleFile);
                string change = File.ReadAllText(sampleFile.Path);
                if(y.Count == 0)
                {
                    No_Item_Text.Visibility = Visibility.Visible;
                    loading_Bar.Visibility = Visibility.Collapsed;
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
                loading_Bar.Visibility = Visibility.Collapsed;
                Refresh_Button.IsEnabled = true;
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
