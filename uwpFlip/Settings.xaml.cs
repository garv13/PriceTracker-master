using Microsoft.AdMediator.Core.Models;
using Microsoft.Advertising.WinRT.UI;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Threading.Tasks;
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
using Windows.UI.Xaml.Navigation;

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=234238

namespace uwpFlip
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class Settings : Page
    {
        public Settings()
        {
            this.InitializeComponent();
            Loaded += Settings_Loaded;
            listLoad();
        }
        InterstitialAd MyVideoAd = new InterstitialAd();
        string MyAppId;
        string MyAdUnitId;

        private void Settings_Loaded(object sender, RoutedEventArgs e)
        {
            AdMediator_BFF56C.AdSdkTimeouts[AdSdkNames.MicrosoftAdvertising] = TimeSpan.FromSeconds(10);
            AdMediator_BFF56C1.AdSdkTimeouts[AdSdkNames.MicrosoftAdvertising] = TimeSpan.FromSeconds(10);
            bool isHardwareButtonsAPIPresent =
                Windows.Foundation.Metadata.ApiInformation.IsTypePresent("Windows.Phone.UI.Input.HardwareButtons");
            if (isHardwareButtonsAPIPresent)
            {
                AdMediator_BFF56C1.Width = 640;
                AdMediator_BFF56C.Width = 640;

                AdMediator_BFF56C1.Height = 100;
                AdMediator_BFF56C.Height = 100;


                MyAppId = "	7b01e63c-94b9-461a-a6b1-0f84540e8a9c";
                MyAdUnitId = "11644026";
            }
            else
            {
                MyAppId = "fbbb7779-4ffd-4c42-92ad-526be5070406";
                MyAdUnitId = "11644016";
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
           
            //AdMediator_BFF56C1.Pause();
            //AdMediator_BFF56C.Pause();
            if ((InterstitialAdState.Ready) == (MyVideoAd.State))
            {
                MyVideoAd.Show();
            }
        }

        void MyVideoAd_ErrorOccurred(object sender, AdErrorEventArgs e)
        {
            AdMediator_BFF56C1.Resume();
            AdMediator_BFF56C.Resume();
        }

        void MyVideoAd_Completed(object sender, object e)
        {
            AdMediator_BFF56C.Resume();
            AdMediator_BFF56C1.Resume();
        }

        void MyVideoAd_Cancelled(object sender, object e)
        {
            AdMediator_BFF56C1.Resume();
            AdMediator_BFF56C.Resume();
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
            
            ProgressRing.IsActive = true;
            if(comboBox.SelectedValue==null)
            {
                await (new MessageDialog("please select time")).ShowAsync();
                Frame.Navigate(typeof(Settings));
                return;
            }
            int j = 45;
            j = int.Parse(comboBox.SelectedValue.ToString());
            //if(j==15 || j==30)
            //{
            //    message.Visibility = Visibility.Visible;
            //}
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
                    await (new MessageDialog("Tracking Started")).ShowAsync();
                }                 
            ProgressRing.IsActive = false;
        }
    }
}
