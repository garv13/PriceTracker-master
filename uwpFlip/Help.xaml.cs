using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
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
    public sealed partial class Help : Page
    {
        public Help()
        {
            this.InitializeComponent();
            string lol = "Home screen is showed when the app is opened first time.This contains all the products added by you for tracking.Each product tile will contain original price, present price and an option to delete product.";
            string lol1 = "To update the prices of products , user can click the refresh button present at the bottom of the page.\n\nFor adding new product user has to click the addition button present at the bottom of the home page.After that user has to select the website on which the product is present.";
            string lol2 = "The website will be opened on the next page where user has to navigate to the page,where the buy option for the product is visible.Then user has to enter a nickname for the product which is shown on the home page.This name can be anything and may not have any link with the product.\n\n";
            string lol3 = "Once the name is entered the user has to click the Add button present at the bottom of the page,a message will be popped when the product is addede.If an error message is shown then please check the Internet connectivity and if the problem is not resolved , then please send us the product link via  email on axejulius @outlook.com.";
            string lol4 = "We will try to solve the issue as soon as possible.\n\nThe tracking time needs to be selected once when the app is installed and opened for first time. This time can be entered from the preferences page. User can select any suitable time varying from 15 minutes to 2 hours.";
            string lol5 = "Once the time is selected from the list please click the add button present at the bottom of the page, a message will be popped to confirm the selection.";
            string lol6 = "This time can be changed any no. of times by the user depending upon his comfort.\n\nFor any help or issue feel free to contact us at - axejulius@outlook.com.";

            about_text.Text = lol + lol1 + lol2 + lol3 + lol4 + lol5 + lol6;
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
    }
}
