//------------------------------------------------------------------------------
// <auto-generated>
//     這個程式碼是由範本產生。
//
//     對這個檔案進行手動變更可能導致您的應用程式產生未預期的行為。
//     如果重新產生程式碼，將會覆寫對這個檔案的手動變更。
// </auto-generated>
//------------------------------------------------------------------------------

namespace JobManagerWeb.Models
{
    using System;
    using System.Collections.Generic;
    
    public partial class JobRecord
    {
        public string jobcode { get; set; }
        public int seqno { get; set; }
        public Nullable<System.DateTime> startTime { get; set; }
        public Nullable<System.DateTime> endTime { get; set; }
        public string descriptions { get; set; }
        public string attachedFile { get; set; }
        public string datfr { get; set; }
        public string adduser { get; set; }
        public Nullable<System.DateTime> addtime { get; set; }
        public string chguser { get; set; }
        public Nullable<System.DateTime> chgtime { get; set; }
        public string note { get; set; }
    }
}