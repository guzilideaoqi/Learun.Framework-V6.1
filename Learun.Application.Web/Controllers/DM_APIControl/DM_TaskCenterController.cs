using Learun.Application.TwoDevelopment.DM_APPManage;
using Learun.Application.Web.App_Start._01_Handler;
using Learun.Cache.Base;
using Learun.Cache.Factory;
using Learun.Util;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Learun.Application.Web.Controllers.DM_APIControl
{
    /// <summary>
    /// dm_data任务类API
    /// Copyright (c) 2013-2017 上海力软信息技术有限公司
    /// 创建人：胡亚广
    /// 日 期：2020.03.13
    /// 描 述：
    /// </summary>
    public class DM_TaskCenterController : MvcAPIControllerBase
    {
        #region 基础类库调用
        private ICache redisCache = CacheFactory.CaChe();
        private DM_ReadTaskIBLL dM_ReadTaskIBLL = new DM_ReadTaskBLL();
        private DM_BaseSettingIBLL dM_BaseSettingIBLL = new DM_BaseSettingBLL();
        #endregion

        #region 获取任务类别
        public ActionResult GetTaskType()
        {
            return View();
        }
        #endregion

        #region 获取任务列表
        // GET: DM_TaskCenter
        public ActionResult GetTaskList()
        {
            return View();
        }
        #endregion

        #region 发布任务
        public ActionResult ReleaseTask()
        {
            return View();
        }
        #endregion

        #region 取消任务(发布任务人操作)
        public ActionResult CancelBySendPerson()
        {
            return View();
        }
        #endregion

        #region 取消任务(接受任务人操作,未提交资料之前都可以提交)
        public ActionResult CancelByRevicePerson()
        {
            return View();
        }
        #endregion

        #region 接受任务
        public ActionResult ReviceTask()
        {
            return View();
        }
        #endregion

        #region 提交资料(接受任务人提交)
        public ActionResult SubmitMeans()
        {
            return View();
        }
        #endregion

        #region 任务审核(发布任务人操作)
        public ActionResult AuditTask()
        {
            return View();
        }
        #endregion

        #region 获取我发布的任务
        public ActionResult GetMyReleaseTask()
        {
            return View();
        }
        #endregion

        #region 获取我接受的任务
        public ActionResult GetMyReviceTask()
        {
            return View();
        }
        #endregion

        #region 获取阅赚任务列表
        public ActionResult GetReadEarnTaskList(int PageNo, int PageSize)
        {
            try
            {
                string appid = CheckAPPID();

                return Success("获取成功", dM_ReadTaskIBLL.GetPageListByCache(new Pagination { page = PageNo, rows = PageSize }, appid));
            }
            catch (Exception ex)
            {
                return Fail(ex.Message);
            }
        }
        #endregion

        #region 阅赚任务点击
        public ActionResult AddClickReadEarnTask(int id)
        {
            try
            {
                string appid = CheckAPPID();

                //获取基础配置
                dm_basesettingEntity dm_BasesettingEntity = dM_BaseSettingIBLL.GetEntityByCache(appid);
                Random random = new Random();
                int clickCount = random.Next(dm_BasesettingEntity.readtask_min.ToInt(), dm_BasesettingEntity.readtask_max.ToInt());

                dM_ReadTaskIBLL.AddClickReadEarnTask(id, clickCount,appid);
                return Success("点击成功",clickCount);
            }
            catch (Exception ex)
            {
                return Fail(ex.Message);
            }

        }
        #endregion

        #region 检测头部是否包含APPID
        public string CheckAPPID()
        {
            if (Request.Headers["appid"].IsEmpty())
                throw new Exception("缺少参数appid");
            else
            {
                return Request.Headers["appid"].ToString();
            }
        }
        #endregion
    }
}