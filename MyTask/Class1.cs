﻿using Newtonsoft.Json;
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
                IList<string> y = new List<string>();
                y= await FileIO.ReadLinesAsync(sampleFile);
                HttpClient cl = new HttpClient();
                foreach (string item in y)
                {
                    p = JsonConvert.DeserializeObject<Product>(item);
                    string str = await cl.GetStringAsync(p.url);
                    int j = str.IndexOf(';');
                    char[] name = str.ToCharArray(0, j - 1);
                    char[] arr = str.ToCharArray(j, str.Length-j);
                    pri.Clear();
                    foreach (char p in arr)
                    {
                        if (char.IsDigit(p))
                        {
                            pri.Add(p);
                        }
                    }
                    str = string.Join("", pri.ToArray());
                    int i = int.Parse(str);
                    if (i < p.price)
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
