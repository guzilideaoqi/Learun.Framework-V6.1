using Learun.Application.TwoDevelopment.DM_APPManage;
using Learun.Application.Web.App_Start._01_Handler;
using Learun.Cache.Base;
using Learun.Cache.Factory;
using Learun.Util;
using System;
using System.Web.Mvc;

namespace Learun.Application.Web.Controllers.DM_APIControl
{
	public class DM_ExtendOperateController : MvcAPIControllerBase
	{
		private ICache redisCache = CacheFactory.CaChe();

		private DM_AnnouncementIBLL dm_AnnouncementIBLL = new DM_AnnouncementBLL();

		private DM_BannerIBLL dm_BannerIBLL = new DM_BannerBLL();

		private DM_BaseSettingIBLL dm_BaseSettingIBLL = new DM_BaseSettingBLL();

        #region 获取平台设置
        public ActionResult GetPlaformSetting()
		{
			try
			{
				string appid = CheckAPPID();
				return Success("获取成功", dm_BaseSettingIBLL.GetEntityByCache(appid));
			}
			catch (Exception ex)
			{
				return Fail(ex.Message);
			}
		}
        #endregion

        #region 回去公告列表
        public ActionResult GetAnnouncementList()
		{
			try
			{
				string appid = CheckAPPID();
				return SuccessList("获取成功", dm_AnnouncementIBLL.GetPageListByCache(new Pagination
				{
					page = 1,
					rows = 50
				}, appid));
			}
			catch (Exception ex)
			{
				return Fail(ex.Message);
			}
		}
        #endregion

        #region 获取banner图列表
        public ActionResult GetBannerList(int type)
		{
			try
			{
				string appid = CheckAPPID();
				return SuccessList("获取成功", dm_BannerIBLL.GetPageListByCache(new Pagination
				{
					page = 1,
					rows = 50
				}, type, appid));
			}
			catch (Exception ex)
			{
				return Fail(ex.Message);
			}
		}
        #endregion

        #region 获取公用的配置信息
        public ActionResult GetCommonSetting()
		{
			try
			{
				string appid = CheckAPPID();
				dm_basesettingEntity dm_BasesettingEntity = dm_BaseSettingIBLL.GetEntityByCache(appid);
				return Success("获取成功", new
				{
					isAppStorePreview = ((base.Request.Headers["version"].ToString() == dm_BasesettingEntity.previewversion) ? 1 : 0)
				});
			}
			catch (Exception ex)
			{
				return Fail(ex.Message);
			}
		}
        #endregion

        public string CheckAPPID()
		{
			if (base.Request.Headers["appid"].IsEmpty())
			{
				throw new Exception("缺少参数appid");
			}
			return base.Request.Headers["appid"].ToString();
		}
	}
}
