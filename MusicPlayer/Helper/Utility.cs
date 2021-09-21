using System.Diagnostics;
using Newtonsoft.Json;
using Windows.System;
using Windows.UI.Notifications;

namespace MusicPlayer
{
    public static class Utility
    {
        public static string ToJson(this object obj)
        {
            return JsonConvert.SerializeObject(obj);
        }

        public static T ToObject<T>(this string s)
        {
            return JsonConvert.DeserializeObject<T>(s);
        }

        public static string IntToSecond(int timecd)
        {
            int phut = timecd / 60;
            int giay = timecd % 60;
            string thoigian = phut + ":" + giay;
            return thoigian;
        }

        public static void ShowToast(string msg, string subMsg = null)
        {
            if (subMsg == null)
                subMsg = GetMemoryUsageText();

            Debug.WriteLine(msg + "\n" + subMsg);


            var toastXml = ToastNotificationManager.GetTemplateContent(ToastTemplateType.ToastText02);

            var toastTextElements = toastXml.GetElementsByTagName("text");
            toastTextElements[0].AppendChild(toastXml.CreateTextNode(msg));
            toastTextElements[1].AppendChild(toastXml.CreateTextNode(subMsg));

            var toast = new ToastNotification(toastXml);
            ToastNotificationManager.CreateToastNotifier().Show(toast);
        }

        private static string GetMemoryUsageText()
        {
            return string.Format("[Memory: Level={0}, Usage={1}K, Target={2}K]", MemoryManager.AppMemoryUsageLevel, MemoryManager.AppMemoryUsage / 1024, MemoryManager.AppMemoryUsageLimit / 1024);
        }
    }
}
