using JobManagerWeb.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace JobManagerWeb.ViewModels
{
    public class JobMemberCreateViewModel
    {
        public JobSheet JobSheetModel { get; set; }

        public List<JobDetail> JobDetailModelList { get; set; }

        public string EmpidString { get; set; }
    }
}