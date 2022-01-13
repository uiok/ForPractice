using JobManagerWeb.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace JobManagerWeb.Service
{
    public class MessageTransService
    {
        WorkRecordEntities _db = new WorkRecordEntities();

        public string TranslateWord(string code)
        {
            var result = "";
            result = _db.MessageConfig.Where(a => a.code == code)
                                           .Select(b => b.message)
                                           .DefaultIfEmpty("")
                                           .FirstOrDefault();
            return result;
        }
    }
}