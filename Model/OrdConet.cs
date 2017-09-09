using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Jinsftpweb.Model
{
    public class OrdConet
    {
        public string ID { get; set; }

        public string BCode1 { get; set; }

        public string BCode2 { get; set; }

        public bool F_Read { get; set; }

        public bool F_Confirm { get; set; }

        public int F_Shipping { get; set; }

        public bool F_Reject { get; set; }

        public bool F_Err_Convert { get; set; }
    }
}