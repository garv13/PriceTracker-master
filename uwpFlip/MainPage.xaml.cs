using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Threading.Tasks;
using Windows.ApplicationModel.Background;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.Storage;
using Windows.UI.Popups;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x409

namespace uwpFlip
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainPage : Page
    {
        public MainPage()
        {
            this.InitializeComponent();
            listLoad();
        }

        public StorageFolder localFolder;
        List<string> urlList = new List<string>();
        Product p = new Product();
        private async Task fileWrite()
        {
            try
            {
                var contentsToWriteToFile = JsonConvert.SerializeObject(p);
                urlList.Add(contentsToWriteToFile);
                localFolder = ApplicationData.Current.LocalFolder;
                StorageFile sampleFile = await localFolder.GetFileAsync("dataFile.txt");
                await FileIO.WriteLinesAsync(sampleFile, urlList);

            }
            catch (Exception ef)
            {
                try
                {
                    StorageFile sampleFile = await localFolder.CreateFileAsync("dataFile.txt",
                    CreationCollisionOption.ReplaceExisting);

                }
                catch (Exception efe)
                {

                }
            }
        }
        private async void RegisterBtn_Click(object sender, RoutedEventArgs e)
        {
            uint i = 30;
            i = uint.Parse(comboBox.SelectedValue.ToString());
            string myTaskName = "FirstTask";

            int f = 0;
            // check if task is already registered
            foreach (var cur in BackgroundTaskRegistration.AllTasks)
                if (cur.Value.Name == myTaskName)
                {
                    await (new MessageDialog("matlab kushi mil gyi dobara dabake... chalo ek barr aur dabao ab....")).ShowAsync();
                    f = 1;

                }



            if (f == 0)
            {
                // Windows Phone app must call this to use trigger types (see MSDN)
                var allowed = await BackgroundExecutionManager.RequestAccessAsync();
                // register a new task
                if ((allowed == BackgroundAccessStatus.AllowedMayUseActiveRealTimeConnectivity) || (allowed == BackgroundAccessStatus.AllowedWithAlwaysOnRealTimeConnectivity))
                {
                    BackgroundTaskBuilder taskBuilder = new BackgroundTaskBuilder { Name = "FirstTask", TaskEntryPoint = "MyTask.Class1" };
                    taskBuilder.SetTrigger(new TimeTrigger(i, false));
                    BackgroundTaskRegistration myFirstTask = taskBuilder.Register();

                    await (new MessageDialog("Task registered")).ShowAsync();
                }
                else
                {
                    await (new MessageDialog("Tumhare pc se na ho payega register ")).ShowAsync();
                }
            }
        }
        private void listLoad()
        {
            List<string> t = new List<string>();
            t.Add("15");
            t.Add("30");
            t.Add("45");
            t.Add("60");
            comboBox.DataContext = t;
        }
        private async Task urlAdd(string url)
        {
            if (radioButtonFlip.IsChecked == true)
                p.url = "http://www.akshaygupta.xyz/flipkart?url=" + url;
            else
                p.url = "http://www.akshaygupta.xyz/snapdeal?url=" + url;
            try
            {
                HttpClient cl = new HttpClient();
                string str = await cl.GetStringAsync(p.url);
                str = str.Remove(0, 4);
                p.price = int.Parse(str);
                p.time = DateTime.Now;
                await fileWrite();
                await (new MessageDialog("add kar diya bc")).ShowAsync();



            }
            catch (Exception ex)
            {

                await (new MessageDialog("url me http kon lagayega ya net kon on karega ")).ShowAsync();
            }
        }
        private async void button_Click(object sender, RoutedEventArgs e)
        {
            await urlAdd(textBox.Text);
            textBox.Text = "";
            textBox.Focus(FocusState.Keyboard);

        }

        private void webButton_Click(object sender, RoutedEventArgs e)
        {
            mainGrid.Visibility = Visibility.Collapsed;
            webViewGrid.Visibility = Visibility.Visible;
            Uri u;
            if (radioButtonFlip.IsChecked == true)
                u = new Uri("http://www.flipkart.com");
            else
                u = new Uri("http://www.snapdeal.com");

            MyWebview.Navigate(u);

        }

        private void backWebview_Click(object sender, RoutedEventArgs e)
        {
            MyWebview.GoBack();
        }

        private void returnWebview_Click(object sender, RoutedEventArgs e)
        {
            mainGrid.Visibility = Visibility.Visible;
            webViewGrid.Visibility = Visibility.Collapsed;
        }

        private async void selectWebview_Click_1(object sender, RoutedEventArgs e)
        {
            string str = MyWebview.Source.ToString();
            await urlAdd(str);
            mainGrid.Visibility = Visibility.Visible;
            webViewGrid.Visibility = Visibility.Collapsed;
        }

        private void MyWebview_NavigationCompleted(WebView sender, WebViewNavigationCompletedEventArgs args)
        {
            selectWebview.Visibility = Visibility.Visible;
        }
    }
}