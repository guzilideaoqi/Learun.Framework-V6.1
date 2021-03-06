﻿using Hyg.Common.OtherTools;
using Learun.Application.TwoDevelopment.Common;
using Learun.Application.TwoDevelopment.DM_APPManage;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Learun.Application.Web.Controllers
{
    [HandlerLogin(Util.FilterMode.Ignore)]
    public class DLM_PageController : MvcControllerBase
    {
        DM_UserIBLL dM_UserIBLL = new DM_UserBLL();
        DM_Task_ReviceIBLL dM_Task_ReviceIBLL = new DM_Task_ReviceBLL();
        DM_TaskIBLL dM_TaskIBLL = new DM_TaskBLL();
        dm_activity_manageIBLL dm_Activity_ManageIBLL = new dm_activity_manageBLL();
        dm_activity_recordIBLL dm_Activity_RecordIBLL = new dm_activity_recordBLL();
        // GET: DLM_Page
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult CloseAccount()
        {
            return View();
        }

        public ActionResult CopyInviteCode(string InviteCode)
        {
            ViewBag.InviteCode = InviteCode;
            return View();
        }

        public ActionResult ActivityPage(string token, string appid, string platform, string version)
        {
            /*
             * 1、随机生成金额分配给对应用户，金额区间26.5~28.2
             * 2、生成用户和任务的关联信息，用于校验任务的状态(一个用户同时接受多个任务  并对任务进行编号)
             */
            dm_userEntity dm_UserEntity = CacheHelper.ReadUserInfoByToken(token);
            if (!dm_UserEntity.IsEmpty())
            {
                dm_activity_manageEntity dm_Activity_ManageEntity = dm_Activity_ManageIBLL.GetActivityInfo();

                if (!dm_Activity_ManageEntity.IsEmpty())
                {
                    ViewBag.ActivityRemark = dm_Activity_ManageEntity.ActivityRemark;

                    dm_activity_recordEntity dm_Activity_RecordEntity = dm_Activity_RecordIBLL.GetEntityByUserID((int)dm_UserEntity.id, dm_Activity_ManageEntity.f_id);

                    if (!dm_Activity_RecordEntity.IsEmpty())
                    {
                        dm_UserEntity = dM_UserIBLL.GetEntity(dm_UserEntity.id);
                    }
                    else
                    {
                        dm_UserEntity = dM_UserIBLL.JoinActivity((int)dm_UserEntity.id);
                        dm_Activity_RecordEntity = new dm_activity_recordEntity();
                    }

                    ViewBag.ActivityPrice = dm_UserEntity.activityprice;

                    ViewBag.MyActivityInfo = dm_Activity_RecordEntity;
                }

            }
            else
            {
                ViewBag.MyActivityInfo = new dm_activity_recordEntity();
            }


            ViewBag.Token = token;
            ViewBag.AppID = appid;
            ViewBag.Platform = platform;
            ViewBag.Version = version;

            return View();
        }

        [HttpGet]
        [AjaxOnly(false)]
        public ActionResult GetRandActivityTaskList(string token)
        {
            dm_userEntity dm_UserEntity = CacheHelper.ReadUserInfoByToken(token);
            DataTable dm_TaskEntities = new DataTable();
            if (!dm_UserEntity.IsEmpty())
            {
                dm_TaskEntities = dM_TaskIBLL.GetRandActivityTaskList((int)dm_UserEntity.id);
            }
            else
            {
                return Fail("用户信息异常!");
            }
            return Success(dm_TaskEntities);
        }

        [HttpPost]
        [AjaxOnly(false)]
        public ActionResult ReviceActivityTask(string token, string taskids)
        {
            dm_userEntity dm_UserEntity = CacheHelper.ReadUserInfoByToken(token);
            if (!dm_UserEntity.IsEmpty())
            {
                dM_Task_ReviceIBLL.ReviceActivityTask(taskids.Split(','), (int)dm_UserEntity.id);
            }
            return Success("领取成功！");
        }
    }
}