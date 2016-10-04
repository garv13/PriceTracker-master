using Microsoft.Advertising.WinRT.UI;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Threading.Tasks;
using VungleSDK;
using Windows.ApplicationModel.Background;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.System;
using Windows.UI.Popups;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;using Windows.System.Profile;

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=234238

namespace uwpFlip
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class Settings : Page
    {
        private int AD_WIDTH = 728;
        private int AD_HEIGHT = 90;
        private const string WAPPLICATIONID = "fbbb7779-4ffd-4c42-92ad-526be5070406";
        private const string WADUNITID = "11644014";
        private const string MAPPLICATIONID = "7b01e63c-94b9-461a-a6b1-0f84540e8a9c";
        private const string MADUNITID = "11644024";

        private const string WAPPLICATIONID1 = "fbbb7779-4ffd-4c42-92ad-526be5070406";
        private const string WADUNITID1 = "11644015";
        private const string MAPPLICATIONID1 = "7b01e63c-94b9-461a-a6b1-0f84540e8a9c";
        private const string MADUNITID1 = "11644025";

        private AdControl myAdControl = null;
        private AdControl myAdControl1 = null;

        private string myAppId = WAPPLICATIONID;
        private string myAdUnitId = WADUNITID;

        private string myAppId1 = WAPPLICATIONID1;
        private string myAdUnitId1 = WADUNITID1;
        bool check = true;
        public Settings()
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

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            check = true;
            Loaded += Settings_Loaded;
            listLoad();
        }
        InterstitialAd MyVideoAd = new InterstitialAd();
  
        VungleAd sdkInstance;
  

        private void Settings_Loaded(object sender, RoutedEventArgs e)
        {
          
            sdkInstance = AdFactory.GetInstance("57e8278aee5b9daa1d000048");
            sdkInstance.OnAdPlayableChanged += SdkInstance_OnAdPlayableChanged;
            if (sdkInstance.AdPlayable)
                SdkInstance_OnAdPlayableChanged(null,null);

        }

        private async void SdkInstance_OnAdPlayableChanged(object sender, AdPlayableEventArgs e)
        {
            if (sdkInstance.AdPlayable && check)
            {
                await sdkInstance.PlayAdAsync(new AdConfig());
                check = false;
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
            catch (Exception es)
            {
                await (new MessageDialog("Can't review now please try again later")).ShowAsync();
            }
        }


        private void MenuButton5_Click(object sender, RoutedEventArgs e)
        {
            Frame.Navigate(typeof(About));
        }

        private void listLoad()
        {
            List<string> t = new List<string>();
            t.Add("15");
            t.Add("30");
            t.Add("45");
            t.Add("60");
            t.Add("90");
            t.Add("120");
            comboBox.DataContext = t;
        }

        private async void refresh_Click(object sender, RoutedEventArgs e)
        {
            Time_Button.IsEnabled = false;
            Time_Button.IsChecked = false;            
            ProgressRing.IsActive = true;
            if(comboBox.SelectedValue==null)
            {
                await (new MessageDialog("Please select time")).ShowAsync();
                Time_Button.IsEnabled = true;
                Frame.Navigate(typeof(Settings));
                return;
            }
            int j = 45;
            j = int.Parse(comboBox.SelectedValue.ToString());
                uint i = uint.Parse(comboBox.SelectedValue.ToString());
                string myTaskName = "priceTask";

                
                // check if task is already registered
                foreach (var cur in BackgroundTaskRegistration.AllTasks)
                    if (cur.Value.Name == myTaskName)
                    {
                        cur.Value.Unregister(true);
                    }
                             
                    // Windows Phone app must call this to use trigger types (see MSDN)
                    var allowed = await BackgroundExecutionManager.RequestAccessAsync();
                // register a new task
                if ((allowed == BackgroundAccessStatus.AllowedMayUseActiveRealTimeConnectivity) || (allowed == BackgroundAccessStatus.AllowedWithAlwaysOnRealTimeConnectivity))
                {
                    BackgroundTaskBuilder taskBuilder = new BackgroundTaskBuilder { Name = "priceTask", TaskEntryPoint = "MyTask.Class1" };
                    taskBuilder.SetTrigger(new TimeTrigger(i, false));
                    BackgroundTaskRegistration myFirstTask = taskBuilder.Register();
                    await (new MessageDialog("Time Changed")).ShowAsync();
                    Time_Button.IsEnabled = true;

            }
            ProgressRing.IsActive = false;
        }
    }
}
