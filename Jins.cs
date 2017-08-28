using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;

namespace Jinsftpweb
{
    public class Jins
    {
        private string ftpServerIP;
        private string ftpServerFolder_order;
        private string ftpUserID;
        private string ftpPassword;
        private string ftpServerFolder_confirmation;
        private string ftpServerFolder_shipping;
        private string saveFilePath = HttpRuntime.AppDomainAppPath + @"xml\order";

        public Jins()
        {
            this.ftpServerIP = "180.167.68.150";
            this.ftpServerFolder_order = "Output";
            this.ftpServerFolder_confirmation = "confirmation";
            this.ftpServerFolder_shipping = "shipping";
            this.ftpUserID = "UserSino";
            this.ftpPassword = "UserSino010!@#$%^";
        }

        public void GetXMLFiles()
        {
            FTPLib.FTP ftp = new FTPLib.FTP();
            string[] fileList = null;
            string fileName = "";
            while (true)
            {
                fileList = ftp.GetFileList(this.ftpServerIP, this.ftpServerFolder_order, this.ftpUserID, this.ftpPassword);
                if (null == fileList) break;
                fileName = fileList[0];
                ftp.DownloadFile(this.ftpServerIP, this.ftpServerFolder_order, this.ftpUserID, this.ftpPassword, fileName, saveFilePath, fileName);
                File.Copy(Path.Combine(saveFilePath, fileName), Path.Combine(saveFilePath + @"\backup", fileName), true);
                ftp.DeleteFile(this.ftpServerIP, this.ftpServerFolder_order, fileName, this.ftpUserID, this.ftpPassword);
            }
        }
    }
}