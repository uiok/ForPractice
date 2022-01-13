using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace JobManagerWeb.App_Start
{
    public class ApplicationParmenterLimitConfig
    {
        private static object _JobSheetApplicationLock = new Object();

        public static Object GetJobFlag()
        {
            return _JobSheetApplicationLock;
        }
    }
}