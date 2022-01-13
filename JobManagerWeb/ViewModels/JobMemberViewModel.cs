using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace JobManagerWeb.ViewModels
{
    public class JobMemberViewModel
    {
        public string empid { get; set; }

        public string name { get; set; }

        public int mtype { get; set; }
        public bool flag { get; set; }
    }
}