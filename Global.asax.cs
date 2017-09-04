using DbHelperSQLLib;
using System;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Net;
using System.Text;
using System.Threading;
using System.Timers;
using System.Web;

namespace Jinsftpweb
{
    public class Global : HttpApplication
    {
        void Application_Start(object sender, EventArgs e)
        {
            //this.t3_Elapsed(null, null);
            //return;
            //定时器
            var t1 = this.CreateTimer(600000);
            t1.Elapsed += t1_Elapsed;
            var t2 = this.CreateTimer(700000);
            t2.Elapsed += t2_Elapsed;
            var t3 = this.CreateTimer(900000);
            t3.Elapsed += t3_Elapsed;
        }
        private System.Timers.Timer CreateTimer(double intervel)
        {
            System.Timers.Timer myTimer1 = new System.Timers.Timer(intervel);
            myTimer1.Enabled = true;
            myTimer1.AutoReset = true;
            return myTimer1;
        }

        void t3_Elapsed(object sender, ElapsedEventArgs e)
        {
            try
            {
                RunGetShippingXMLFiles();
                RunUploadShippingXMLFiles();
            }
            catch (Exception ex)
            {
                Default.html += DateTime.Now.ToShortDateString() + " " + DateTime.Now.ToLongTimeString() + "[Shipping]" + ex.Message + "<br />";
                Jinsdb.AddLog(ex.Message);
            }
        }

        void t2_Elapsed(object sender, ElapsedEventArgs e)
        {
            try
            {
                RunGetConfirmXMLFiles();
                RunUploadConfirmXMLFiles();
            }
            catch (Exception ex)
            {
                Default.html += DateTime.Now.ToShortDateString() + " " + DateTime.Now.ToLongTimeString() + "[Confirm]" + ex.Message + "<br />";
                Jinsdb.AddLog(ex.Message);
            }
        }

        private void t1_Elapsed(object source, ElapsedEventArgs e)
        {
            try
            {
                RunGetXMLFiles();
            }
            catch (Exception ex)
            {
                Default.html += DateTime.Now.ToShortDateString() + " " + DateTime.Now.ToLongTimeString() + "[Getting]" + ex.Message + "<br />";
                Jinsdb.AddLog(ex.Message);
            }
        }

        private void RunGetXMLFiles()
        {
            Jins jins = new Jins();
            var count = jins.GetXMLFiles();
            Default.html += DateTime.Now.ToShortDateString() + " " + DateTime.Now.ToLongTimeString() + " [" + count.ToString() + "] xmlfiles get from server.<br />";
        }

        private void RunUploadConfirmXMLFiles()
        {
            Jins jins = new Jins();
            var count = jins.UploadConfirmFiles();
            Default.html += DateTime.Now.ToShortDateString() + " " + DateTime.Now.ToLongTimeString() + " [" + count.ToString() + "] confirmxmlfiles upload to server.<br />";
        }

        private void RunUploadShippingXMLFiles()
        {
            Jins jins = new Jins();
            var count = jins.UploadShippingFiles();
            Default.html += DateTime.Now.ToShortDateString() + " " + DateTime.Now.ToLongTimeString() + " [" + count.ToString() + "] shippingxmlfiles upload to server.<br />";
        }

        private void RunGetConfirmXMLFiles()
        {
            Jins jins = new Jins();
            var count = jins.GetConfirmFiles();
            Default.html += DateTime.Now.ToShortDateString() + " " + DateTime.Now.ToLongTimeString() + " [" + count.ToString() + "] confirmxmlfiles create to local.<br />";
        }

        private void RunGetShippingXMLFiles()
        {
            Jins jins = new Jins();
            var count = jins.GetShippingFilesRX();
            Default.html += DateTime.Now.ToShortDateString() + " " + DateTime.Now.ToLongTimeString() + " [" + count.ToString() + "] RXshippingxmlfiles create to local.<br />";
            var count2 = jins.GetShippingFilesST();
            Default.html += DateTime.Now.ToShortDateString() + " " + DateTime.Now.ToLongTimeString() + " [" + count2.ToString() + "] STshippingxmlfiles create to local.<br />";
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
