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
    public class DM_ExtendOperateController : MvcAPIControllerBase
    {
        private ICache redisCache = CacheFactory.CaChe();

        #region 数据接口层
        private DM_AnnouncementIBLL dm_AnnouncementIBLL = new DM_AnnouncementBLL();//公告接口
        private DM_BannerIBLL dm_BannerIBLL = new DM_BannerBLL();//导航图接口
        #endregion

        #region 获取公告
        public ActionResult GetAnnouncementList()
        {
            try
            {
                string appid = CheckAPPID();
                return Success("获取成功", dm_AnnouncementIBLL.GetPageListByCache(new Pagination { page = 1, rows = 50 }, appid));
            }
            catch (Exception ex)
            {
                return Fail(ex.Message);
            }
        }
        #endregion

        #region 获取导航图片
        /// <summary>
        /// 获取导航图片
        /// </summary>
        /// <param name="type">0首页导航  1任务页导航</param>
        /// <returns></returns>
        public ActionResult GetBannerList(int type)
        {
            try
            {
                string appid = CheckAPPID();
                return Success("获取成功", dm_BannerIBLL.GetPageListByCache(new Pagination { page = 1, rows = 50 },type, appid));
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