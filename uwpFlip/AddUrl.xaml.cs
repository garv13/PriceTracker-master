using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Runtime.InteropServices;
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
using Windows.UI.Xaml.Navigation;

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=234238

namespace uwpFlip
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class AddUrl : Page
    {
        public AddUrl()
        {
            this.InitializeComponent();
        }

        string name = null;
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

        private async void Image_Tapped(object sender, TappedRoutedEventArgs e)
        {
            await(new MessageDialog("Not Available now :(:( Will be added soon!!!!")).ShowAsync();
            //var add = "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/42.0.2311.135 Safari/537.36 Edge/12.246";
            //ChangeUserAgent(add);
            //MyWebview.Navigate(new Uri("http://flipkart.com", UriKind.Absolute));

            //frstScreen.Visibility = Visibility.Collapsed;
            //flip.Visibility = Visibility.Visible;
        }

        private void Image_Tapped_1(object sender, TappedRoutedEventArgs e)
        {
            var add = "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/42.0.2311.135 Safari/537.36 Edge/12.246";
            ChangeUserAgent(add);
            MyWebview1.Navigate(new Uri("http://snapdeal.com", UriKind.Absolute));
            frstScreen.Visibility = Visibility.Collapsed;
            snap.Visibility = Visibility.Visible;
        }

        private void MyWebview_NavigationCompleted(WebView sender, WebViewNavigationCompletedEventArgs args)
        {
            CommandBar1.Visibility = Visibility.Visible;
        }

        Product p = new Product();
        public StorageFolder localFolder;
        List<string> urlList = new List<string>();
        List<char> pri = new List<char>();

        private async Task urlAdd(string url, int i)
        {
            if (url.Contains("flipkart"))
                p.url = "http://www.akshaygupta.xyz/flipkart?url=" + url;
            else if (url.Contains("snapdeal"))
                p.url = "http://www.akshaygupta.xyz/snapdeal?url=" + url;
            else if (url.Contains("amazon.com"))
                p.url = "http://www.akshaygupta.xyz/amazon/usa?url=" + url;
            else if (url.Contains("amazon.co.in"))
                p.url = "http://www.akshaygupta.xyz/amazon/india?url=" + url;
            else if (url.Contains("amazon"))
                p.url = "http://www.akshaygupta.xyz/amazon/uk?url=" + url;
            else if (url.Contains("jabong"))
                p.url = "http://www.akshaygupta.xyz/jabong?url=" + url;

            try
            {
                HttpClient cl = new HttpClient();
                string str = await cl.GetStringAsync(p.url);
               // int j = str.IndexOf(';');
               // char[] name = str.ToCharArray(0, j - 1);
                char[] arr = str.ToCharArray();
                foreach (char p in arr)
                {
                    if(char.IsDigit(p))
                    {
                        pri.Add(p);
                    }
                }
                str = string.Join("", pri.ToArray());
                p.price = int.Parse(str);
                p.name = name;
                p.time = DateTime.Now;
                await fileWrite();
                await (new MessageDialog("URL added successfully")).ShowAsync();
                Frame.Navigate(typeof(Home));
            }
            catch (Exception)
            {

                await (new MessageDialog("URL wrong or net not working try again")).ShowAsync();
            }
        }

        private async Task fileWrite()
        {
            try
            {
                var contentsToWriteToFile = JsonConvert.SerializeObject(p);
                urlList.Add(contentsToWriteToFile);
                localFolder = ApplicationData.Current.LocalFolder;
                StorageFile sampleFile = await localFolder.GetFileAsync("dataFile.txt");
                await FileIO.AppendLinesAsync(sampleFile, urlList);
            }
            catch (FileNotFoundException)
            {
                try
                {
                    StorageFile sampleFile = await localFolder.CreateFileAsync("dataFile.txt",
                    CreationCollisionOption.ReplaceExisting);
                    await FileIO.AppendLinesAsync(sampleFile, urlList);

                }
                catch (Exception)
                {

                }
            }
        }
        private async void Add_Click(object sender, RoutedEventArgs e)
        {
            string str;
          
            if (snap.Visibility == Visibility.Collapsed)
            {
                name = textBoxN.Text;
                if (name == "")
                {
                    await (new MessageDialog("enter name of product...")).ShowAsync();
                    textBoxN.Focus(FocusState.Pointer);
                    return;
                }
                if (textBox.Text.Length > MyWebview.Source.OriginalString.Length)
                    str = textBox.Text;
                else
                    str = MyWebview.Source.OriginalString;
                await urlAdd(str, 1);
            }

            else
            {
                name = textBox2.Text;
                if (name == "")
                {
                    await (new MessageDialog("enter name of product...")).ShowAsync();
                    textBox2.Focus(FocusState.Pointer);
                    return;
                }
                if (textBox1.Text.Length > MyWebview1.Source.OriginalString.Length)
                    str = textBox1.Text;
                else
                    str = MyWebview1.Source.OriginalString;
                await urlAdd(str, 2);
            }
        }

        private void back1_Click(object sender, RoutedEventArgs e)
        {
            frstScreen.Visibility = Visibility.Visible;
            snap.Visibility = Visibility.Collapsed;
            flip.Visibility = Visibility.Collapsed;
        }


        [DllImport("urlmon.dll", CharSet = CharSet.Ansi)]
        private static extern int UrlMkSetSessionOption(int dwOption, string pBuffer, int dwBufferLength, int dwReserved);

        const int URLMON_OPTION_USERAGENT = 0x10000001;
        public void ChangeUserAgent(string Agent)
        {
            UrlMkSetSessionOption(URLMON_OPTION_USERAGENT, Agent, Agent.Length, 0);
        }

        private void Button_Tapped(object sender, TappedRoutedEventArgs e)
        {
            var add = "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/42.0.2311.135 Safari/537.36 Edge/12.246";
            ChangeUserAgent(add);
            MyWebview.Navigate(new Uri("http://jabong.com", UriKind.Absolute));
            frstScreen.Visibility = Visibility.Collapsed;
            flip.Visibility = Visibility.Visible;

        }

        private void Button_Tapped_2(object sender, TappedRoutedEventArgs e)
        {
            var add = "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/42.0.2311.135 Safari/537.36 Edge/12.246";
            ChangeUserAgent(add);
            MyWebview.Navigate(new Uri("http://amazon.com", UriKind.Absolute));
            frstScreen.Visibility = Visibility.Collapsed;
            flip.Visibility = Visibility.Visible;
        }

        private void Button_Tapped_3(object sender, TappedRoutedEventArgs e)
        {
            var add = "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/42.0.2311.135 Safari/537.36 Edge/12.246";
            ChangeUserAgent(add);
            MyWebview.Navigate(new Uri("http://amazon.co.in", UriKind.Absolute));
            frstScreen.Visibility = Visibility.Collapsed;
            flip.Visibility = Visibility.Visible;
        }

        private void Button_Tapped_4(object sender, TappedRoutedEventArgs e)
        {
            var add = "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/42.0.2311.135 Safari/537.36 Edge/12.246";
            ChangeUserAgent(add);
            MyWebview.Navigate(new Uri("http://amazon.it", UriKind.Absolute));
            frstScreen.Visibility = Visibility.Collapsed;
            flip.Visibility = Visibility.Visible;
        }

        private void Button_Tapped_5(object sender, TappedRoutedEventArgs e)
        {
            var add = "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/42.0.2311.135 Safari/537.36 Edge/12.246";
            ChangeUserAgent(add);
            MyWebview.Navigate(new Uri("http://amazon.co.uk", UriKind.Absolute));
            frstScreen.Visibility = Visibility.Collapsed;
            flip.Visibility = Visibility.Visible;
        }

        private void Button_Tapped_6(object sender, TappedRoutedEventArgs e)
        {
            var add = "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/42.0.2311.135 Safari/537.36 Edge/12.246";
            ChangeUserAgent(add);
            MyWebview.Navigate(new Uri("http://amazon.de", UriKind.Absolute));
            frstScreen.Visibility = Visibility.Collapsed;
            flip.Visibility = Visibility.Visible;
        }

        private void Button_Tapped_7(object sender, TappedRoutedEventArgs e)
        {
            var add = "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/42.0.2311.135 Safari/537.36 Edge/12.246";
            ChangeUserAgent(add);
            MyWebview.Navigate(new Uri("http://amazon.co.jp", UriKind.Absolute));
            frstScreen.Visibility = Visibility.Collapsed;
            flip.Visibility = Visibility.Visible;
        }
    }
}
