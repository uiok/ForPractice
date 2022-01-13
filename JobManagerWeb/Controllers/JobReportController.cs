using JobManagerWeb.Models;
using JobManagerWeb.Service;
using JobManagerWeb.ViewModels;
using Newtonsoft.Json;
using PagedList;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace JobManagerWeb.Controllers
{
    public class JobReportController : Controller
    {
        JobReporService _JobReporService = new JobReporService();
        MessageTransService _MessageTransService = new MessageTransService();

        #region 列表頁

        /// <summary>
        /// 首頁
        /// </summary>
        /// <returns></returns>
        public ActionResult Index()
        {
            return View();
        }

        /// <summary>
        /// 查詢功能
        /// </summary>
        /// <param name="page"></param>
        /// <param name="searchTarget"></param>
        /// <returns></returns>
        public ActionResult PartialIndex(int? page, JobSheet model)
        {
            //列出前10筆新的
            var result = _JobReporService.IndexDataList(model);

            //畫面
            int currentPageIndex = page.HasValue ? page.Value : 1;
            ViewBag.page = currentPageIndex.ToString();
           return PartialView("_PartialIndex", result.ToPagedList(currentPageIndex, 10));

        }
        #endregion
        
        #region 新增


        public ActionResult Create()
        {
            return View("Operator");
        }
        
        [HttpPost]
        public ActionResult Create(JobMemberCreateViewModel model)
        {
            try
            {
                var result = _JobReporService.Create(model);
                TempData["SystemMesaage"] = _MessageTransService.TranslateWord(result);
                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }


        #endregion


        #region 編輯


        public ActionResult Edit()
        {
            return View("Operator");
        }


        [HttpPost]
        public ActionResult Edit(JobMemberCreateViewModel model)
        {
            try
            {
                var result = _JobReporService.Edit(model);
                TempData["SystemMesaage"] = _MessageTransService.TranslateWord(result);
                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }
        #endregion


        #region 共用

        /// <summary>
        /// 主檔版型
        /// </summary>
        /// <returns></returns>
        [ChildActionOnly]
        public PartialViewResult PartialTop(string id)
        {
            var actionName = RouteData.Route.GetRouteData(HttpContext).Values["action"].ToString();

            if (actionName == "Create")
            {
                return PartialView("_PartialTopForCreate");
            }
            else if (actionName == "Edit")
            {
                var result = _JobReporService.GetJobSheetData(id);
                TempData["JobMember"] = _JobReporService.GetMemberData(id); 
                return PartialView("_PartialTopForEdit", result);
            }

            return PartialView("Empty");
        }

        /// <summary>
        /// 明細檔版型
        /// </summary>
        /// <returns></returns>
        [ChildActionOnly]
        public PartialViewResult PartialBottom(string id)
        {
            var actionName = RouteData.Route.GetRouteData(HttpContext).Values["action"].ToString();

            if ("Create" == actionName)
            {
                return PartialView("_PartialBottomForCreate");
            }
            else
            {
                var result = _JobReporService.GetJobDetailData(id);
             
                return PartialView("_PartialBottomForEdit", result);
            }
            
        }

        /// <summary>
        /// 新增明細
        /// </summary>
        /// <param name="count"></param>
        /// <returns></returns>
        [HttpPost]
        public PartialViewResult BottomOption( string count)
        {
            ViewBag.count = count;
            return PartialView("_PartialBottomDetail");
        }

        /// <summary>
        /// 人員版型
        /// </summary>
        /// <param name="page"></param>
        /// <param name="model"></param>
        /// <returns></returns>
        public ActionResult PartialPeople(int? page, JobSheet model)
        {
            //畫面
            int currentPageIndex = page.HasValue ? page.Value : 1;
            ViewBag.page = currentPageIndex.ToString();
            var result =   _JobReporService.PeopleDataList();
           
           return PartialView("_PartialPeople", result.ToPagedList(currentPageIndex, 10));

        }


        public JsonResult PeopleData(string id)
        {
            var result = _JobReporService.GetMemberData(id);
            return Json(result);
        }

        #endregion

        [HttpPost]
        public ActionResult Delete(int id, FormCollection collection)
        {
            try
            {
                // TODO: Add delete logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }
        
    }
}
