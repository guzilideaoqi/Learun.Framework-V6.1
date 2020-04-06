using Learun.Application.TwoDevelopment.DM_APPManage;
using Learun.Application.Web.App_Start._01_Handler;
using Learun.Cache.Base;
using Learun.Cache.Factory;
using Learun.Util;
using System;
using System.Web.Mvc;

namespace Learun.Application.Web.Controllers.DM_APIControl
{
	public class DM_TaskCenterController : MvcAPIControllerBase
	{
		private ICache redisCache = CacheFactory.CaChe();

		private DM_ReadTaskIBLL dM_ReadTaskIBLL = new DM_ReadTaskBLL();

		private DM_BaseSettingIBLL dM_BaseSettingIBLL = new DM_BaseSettingBLL();

		public ActionResult GetTaskType()
		{
			return View();
		}

		public ActionResult GetTaskList()
		{
			return View();
		}

		public ActionResult ReleaseTask()
		{
			return View();
		}

		public ActionResult CancelBySendPerson()
		{
			return View();
		}

		public ActionResult CancelByRevicePerson()
		{
			return View();
		}

		public ActionResult ReviceTask()
		{
			return View();
		}

		public ActionResult SubmitMeans()
		{
			return View();
		}

		public ActionResult AuditTask()
		{
			return View();
		}

		public ActionResult GetMyReleaseTask()
		{
			return View();
		}

		public ActionResult GetMyReviceTask()
		{
			return View();
		}

        #region 获取阅赚任务
        public ActionResult GetReadEarnTaskList(int PageNo, int PageSize)
		{
			try
			{
				string appid = CheckAPPID();
				return Success("获取成功", new
				{
					list = dM_ReadTaskIBLL.GetPageListByCache(new Pagination
					{
						page = PageNo,
						rows = PageSize
					}, appid)
				});
			}
			catch (Exception ex)
			{
				return Fail(ex.Message);
			}
		}
        #endregion

        #region 增加任务点击次数
        public ActionResult AddClickReadEarnTask(int id)
		{
			try
			{
				string appid = CheckAPPID();
				dm_basesettingEntity dm_BasesettingEntity = dM_BaseSettingIBLL.GetEntityByCache(appid);
				Random random = new Random();
				int clickCount = random.Next(dm_BasesettingEntity.readtask_min.ToInt(), dm_BasesettingEntity.readtask_max.ToInt());
				dM_ReadTaskIBLL.AddClickReadEarnTask(id, clickCount, appid);
				return Success("点击成功", new
				{
					ClickCount = clickCount
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
