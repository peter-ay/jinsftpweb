using System;
using System.IO;
using System.Net;
using System.Threading;
using System.Timers;
using System.Web;

namespace Jinsftpweb
{
    public class Global : HttpApplication
    {
        void Application_Start(object sender, EventArgs e)
        {
            //this.RunTheTask();
            //定时器
            System.Timers.Timer myTimer = new System.Timers.Timer(10000);
            myTimer.Elapsed += new ElapsedEventHandler(myTimer_Elapsed);
            myTimer.Enabled = true;
            myTimer.AutoReset = true;
        }
        private void myTimer_Elapsed(object source, ElapsedEventArgs e)
        {
            try
            {
                RunTheTask();
            }
            catch (Exception ex)
            {
                Default.html += DateTime.Now.ToShortDateString() + " " + DateTime.Now.ToLongTimeString() + ex.Message + "<br />";
            }
        }

        private void RunTheTask()
        {
            Jins jins = new Jins();
            jins.GetXMLFiles();
        }

        void Application_End(object sender, EventArgs e)
        {
            //  Code that runs on application shutdown

            //下面的代码是关键，可解决IIS应用程序池自动回收的问题  
            Thread.Sleep(1000);
            //这里设置你的web地址，可以随便指向你的任意一个aspx页面甚至不存在的页面，目的是要激发Application_Start  
            string url = "http://www.123.com";
            HttpWebRequest myHttpWebRequest = (HttpWebRequest)WebRequest.Create(url);
            HttpWebResponse myHttpWebResponse = (HttpWebResponse)myHttpWebRequest.GetResponse();
            Stream receiveStream = myHttpWebResponse.GetResponseStream();//得到回写的字节流  
        }

        void Application_Error(object sender, EventArgs e)
        {
            // Code that runs when an unhandled error occurs

        }
    }
}
