using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Jinsftpweb.Model
{
    public class OrdMain
    {

        public string Order_Type
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

    }
}