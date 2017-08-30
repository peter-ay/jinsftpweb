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
        private string ftpUserID;
        private string ftpPassword;

        private string ftpServerFolder_order;
        private string ftpServerFolder_confirmation;
        private string ftpServerFolder_shipping;

        private string localFolder_order = HttpRuntime.AppDomainAppPath + @"xml\order";
        private string localFolder_confirm = HttpRuntime.AppDomainAppPath + @"xml\confirmation";
        private string localFolder_shipping = HttpRuntime.AppDomainAppPath + @"xml\shipping";

        public Jins()
        {
            this.ftpServerIP = "106.15.90.89:9022";
            this.ftpServerFolder_order = "order";
            this.ftpServerFolder_confirmation = "confirmation";
            this.ftpServerFolder_shipping = "shipping";
            this.ftpUserID = "hko";
            this.ftpPassword = "1QY388ZB";
        }

        public int GetXMLFiles()
        {
            FTPLib.FTP ftp = new FTPLib.FTP();
            string[] fileList = null;
            string fileName = "";
            int countGetFiles = 0;
            while (true)
            {
                fileList = ftp.GetFileList(this.ftpServerIP, this.ftpServerFolder_order, this.ftpUserID, this.ftpPassword);
                if (null == fileList) break;
                fileName = fileList[0];
                JinsPub.OrdID = fileName.Substring(0, fileName.LastIndexOf("."));
                ftp.DownloadFile(this.ftpServerIP, this.ftpServerFolder_order, this.ftpUserID, this.ftpPassword, fileName, localFolder_order, fileName);
                File.Copy(Path.Combine(localFolder_order, fileName), Path.Combine(localFolder_order + @"\backup", fileName), true);
                var fullPath = localFolder_order + @"\" + fileName;
                var model = Jinsxml.ConvertXMLtoOrdModel(fullPath);
                Jinsdb.AddOrd(model);
                FileInfo fi = new FileInfo(fullPath);
                fi.Delete();
                ftp.DeleteFile(this.ftpServerIP, this.ftpServerFolder_order, fileName, this.ftpUserID, this.ftpPassword);
                countGetFiles++;
            }
            return countGetFiles;
        }

        public int UploadConfirmFiles()
        {
            return this.UploadXMLFiles(this.localFolder_confirm, this.ftpServerFolder_order);
        }

        private int UploadXMLFiles(string fromLocalFolder, string toFTPServerFolder)
        {
            FTPLib.FTP ftp = new FTPLib.FTP();
            DirectoryInfo folder = new DirectoryInfo(fromLocalFolder);
            FileInfo[] fileList = null;
            string fileName = "";
            string fullFileName = "";
            var count = 0;
            while (true)
            {
                fileList = folder.GetFiles("*.xml");
                if (fileList.Count() == 0)
                    break;
                fileName = fileList[0].Name;
                JinsPub.OrdID = fileName;
                fullFileName = fileList[0].FullName;
                ftp.UploadFile(ftpServerIP, toFTPServerFolder, fullFileName, ftpUserID, ftpPassword);
                fileList[0].Delete();
                count++;
            }
            return count;
        }

        public int UploadShippingFiles()
        {
            return this.UploadXMLFiles(this.localFolder_shipping, this.ftpServerFolder_order);
        }



    }
}