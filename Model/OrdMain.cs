using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Jinsftpweb.Model
{
    public class OrdMain
    {

        public string ID
        {
            get;
            set;
        }
        public string OrdID
        {
            get;
            set;
        }
        public string OrdHdID
        {
            get;
            set;
        }
        public string OrdType
        {
            get;
            set;
        }
        public string Address1
        {
            get;
            set;
        }
        public string Postal
        {
            get;
            set;
        }
        public string Tel
        {
            get;
            set;
        }
        public string Memo
        {
            get;
            set;
        }
        public DateTime Created
        {
            get;
            set;
        }
        public string SalesOfficeCode
        {
            get;
            set;
        }
        public string SalesOfficeName
        {
            get;
            set;
        }

        public OrdRX SubRX
        {
            get;
            set;
        }

        public List<OrdST> SubST
        {
            get;
            set;
        }

        public OrdZ SubZ
        {
            get;
            set;
        }

        public OrdZMain SubZMain
        {
            get;
            set;
        }

        public string ECode { get; set; }

        public bool F_Reject { get; set; }
    }
}