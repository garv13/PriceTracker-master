using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Windows.ApplicationModel.Background;
using Windows.Storage;
using Windows.UI.Notifications;
using NotificationsExtensions.Toasts;
namespace MyTask
{
    public sealed class Class1 : IBackgroundTask 
    {
        StorageFolder localFolder;
        Product p = new Product();
        List<string> pName = new List<string>();
        public async void Run(IBackgroundTaskInstance taskInstance)
        {
            bool flag = false;
            List<char> pri = new List<char>();
            BackgroundTaskDeferral tr = taskInstance.GetDeferral();
            try
            {
                localFolder = ApplicationData.Current.LocalFolder;
                StorageFile sampleFile = await localFolder.GetFileAsync("dataFile.txt");
                StorageFile adsFile = await localFolder.GetFileAsync("addCheckFile.txt");
                string date = await FileIO.ReadTextAsync(adsFile);
                DateTime d = DateTime.Parse(date);
                var v = DateTime.Now;
                TimeSpan ct = v - d; 
                if(ct.TotalDays >= 7)
                {
                   
                        ToastVisual visual = new ToastVisual()
                        {
                            TitleText = new ToastText()
                            {
                                Text = "Looks like you aren't purchasing new products"
                            }
                        };
                        ToastContent toastContent = new ToastContent()
                        {
                            Visual = visual
                        };

                        var toast = new ToastNotification(toastContent.GetXml());

                        toast.ExpirationTime = DateTime.Now.AddHours(1);

                        ToastNotificationManager.CreateToastNotifier().Show(toast);
                        await FileIO.WriteTextAsync(adsFile, DateTime.Today.ToString());
                }
                IList<string> y = new List<string>();
                y= await FileIO.ReadLinesAsync(sampleFile);
                HttpClient cl = new HttpClient();
                foreach (string item in y)
                {
                    p = JsonConvert.DeserializeObject<Product>(item);
                    string str = await cl.GetStringAsync(p.url);                
                    char[] arr = str.ToCharArray();
                    pri.Clear();
                    foreach (char p in arr)
                    {
                        if (char.IsDigit(p) || p =='.')
                        {
                            pri.Add(p);
                        }
                    }
                    str = string.Join("", pri.ToArray());
                    float i = float.Parse(str);
                    if (i < p.price)    //change for testing
                    {
                        pName.Add(p.name);
                        flag = true;
                    }
                }
            }
            catch (Exception)
            {
              
            }
            if (flag)
            {
                try
                {
                    for (int i = 0; i < pName.Count; i++)
                    {


                        ToastVisual visual = new ToastVisual()
                        {
                            TitleText = new ToastText()
                            {
                                Text = "Price Changed"
                            }
                        };
                        ToastContent toastContent = new ToastContent()
                        {
                            Visual = visual
                        };

                        var toast = new ToastNotification(toastContent.GetXml());

                        toast.ExpirationTime = DateTime.Now.AddHours(1);

                        ToastNotificationManager.CreateToastNotifier().Show(toast);
                    }
                }
                catch (Exception)
                {
                }
            }
            tr.Complete();
        }
    }
}
