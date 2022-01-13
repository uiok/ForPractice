using JobManagerWeb.App_Start;
using JobManagerWeb.Models;
using JobManagerWeb.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace JobManagerWeb.Service
{
    public class JobReporService
    {
        WorkRecordEntities _db = new WorkRecordEntities();
      
        public List<JobSheet> IndexDataList(JobSheet model)
        {
            var result = new List<JobSheet>();
            if (model.starttime == null && model.endtime == null)
            {
                result =  _db.JobSheet.OrderByDescending(a => a.addtime).Take(10).ToList();
            }
            else
            {
                result =  _db.JobSheet.Where(a => 
                                               ( a.jobcode.Contains(model.jobcode) ||  a.jobtitle.Contains(model.jobtitle) ) &&
                                               (a.starttime >= model.starttime && a.endtime <= model.endtime)
                                       ).ToList();
            }
        
            return result;
        }
        

        public List<JobMemberViewModel> PeopleDataList()
        {
            var result = _db.MemberInfo.Select(a =>
                             new JobMemberViewModel()
                             {
                                 empid = a.empid,
                                 name = a.name,
                                 mtype = a.membertype
                             }
                            ).ToList();
            return result ?? new List<JobMemberViewModel>();

        }


        public string Create(JobMemberCreateViewModel model)
        {
            try
            {
                JobSheet masterModel;
                //新增主檔
                lock (ApplicationParmenterLimitConfig.GetJobFlag())
                {
                    model.JobSheetModel.jobcode = GenerJobeCode();
                    model.JobSheetModel.adduser = "admin"; //暫時用看系統設計
                    model.JobSheetModel.addtime = DateTime.Now;
                    masterModel = _db.JobSheet.Add(model.JobSheetModel);
                    _db.SaveChanges();
                }

                //新增明細
                var index = 0;
                model.JobDetailModelList.ForEach((a) => { a.seqno = index; a.jobcode = masterModel.jobcode; a.adduser = "admin"; a.addtime = DateTime.Now; index++; });
                _db.JobDetail.AddRange(model.JobDetailModelList);

                //新增人員
                _db.JobMember.AddRange(GenerMember(masterModel.jobcode, model.EmpidString));

                var status = _db.SaveChanges();

                if (status <= 0)
                {
                    return "0";
                }

                return "1";
            }
            catch (Exception ex)
            {
                return "0";
            }
      
        }


        public string Edit(JobMemberCreateViewModel model)
        {
            try
            {
                //取出主檔
                var jopbSheetObj = _db.JobSheet.Where(a => a.jobcode == model.JobSheetModel.jobcode).FirstOrDefault();

                //異常資料
                if (jopbSheetObj == null)
                {
                    return "0";
                }
                else
                {
                    jopbSheetObj.jobtitle = model.JobSheetModel.jobtitle;
                    jopbSheetObj.jobtype = model.JobSheetModel.jobtype;
                    jopbSheetObj.starttime = model.JobSheetModel.starttime;
                    jopbSheetObj.endtime = model.JobSheetModel.endtime;
                    jopbSheetObj.chguser = "system"; 
                    jopbSheetObj.chgtime = DateTime.Now;
                    jopbSheetObj.datfr = model.JobSheetModel.datfr;
                }
                
                //取出明細
                var JobDetailList = _db.JobDetail.Where(a => a.jobcode == model.JobSheetModel.jobcode).ToList();
                //刪除明細
                _db.JobDetail.RemoveRange(JobDetailList);
                //新增明細
                var index = 0;
                model.JobDetailModelList.ForEach((a) => { a.seqno = index; a.jobcode = jopbSheetObj.jobcode; a.adduser = "admin"; a.addtime = DateTime.Now; index++; });
                _db.JobDetail.AddRange(model.JobDetailModelList);
                
                //刪除人員
                _db.JobMember.RemoveRange(_db.JobMember.Where(a => a.jobcode == model.JobSheetModel.jobcode));
                //新增人員
                _db.JobMember.AddRange(GenerMember(model.JobSheetModel.jobcode, model.EmpidString));


                var status = _db.SaveChanges();

                if (status <= 0)
                {
                    return "0";
                }

                return "1";
            }
            catch (Exception ex)
            {
                return "0";
            }

        }
        private List<JobMember> GenerMember(string keyString,string memberString)
        {
            var result = new List<JobMember>();
            var dataList = memberString.Split('/').ToList();

            foreach (var item in dataList)
            {
                var tempDataList = item.Split(';').ToList();

                //有問題的資料
                if (tempDataList.Count() != 3)
                {
                    continue;
                }
                var tempModel = new JobMember()
                {
                    jobcode = keyString,
                    empid = tempDataList[0],
                    membertype = int.Parse(tempDataList[1]),
                    name = tempDataList[2]
                };
                result.Add(tempModel);
            }
            return result;
        }

        private string GenerJobeCode()
        {
            //編號格式:類別+五碼流水號
            var result = "A";
            var number = _db.JobSheet.Count() + 1;
            result += number.ToString().PadLeft(4, '0');
            return result;
        }

        /// <summary>
        /// 取主檔
        /// </summary>
        /// <param name="jobcode"></param>
        /// <returns></returns>
        public JobSheet GetJobSheetData(string jobcode)
        {
            var result = _db.JobSheet.Where(a => a.jobcode == jobcode).FirstOrDefault();

            return result ?? new JobSheet();
        }


        public List<JobMemberViewModel> GetMemberData(string jobcode)
        {
            var result = _db.JobMember
                            .Where(a => a.jobcode == jobcode)
                            .Select(b => new JobMemberViewModel()
                            {
                                empid = b.empid,
                                mtype = b.membertype,
                                name = b.name
                            })
                            .ToList();

            return result ?? new List<JobMemberViewModel>();
        }

        public List<JobDetail> GetJobDetailData(string jobcode)
        {
            var result = _db.JobDetail.Where(a => a.jobcode == jobcode).ToList();

            return result ?? new List<JobDetail>();
        }
        public void Delete()
        {

        }
    }
}