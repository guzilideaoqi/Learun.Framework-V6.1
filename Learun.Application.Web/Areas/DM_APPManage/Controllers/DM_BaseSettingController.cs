using Learun.Application.TwoDevelopment.DM_APPManage;
using Learun.Util;
using System.Collections.Generic;
using System.Web.Mvc;

namespace Learun.Application.Web.Areas.DM_APPManage.Controllers
{
	public class DM_BaseSettingController : MvcControllerBase
	{
		private DM_BaseSettingIBLL dM_BaseSettingIBLL = new DM_BaseSettingBLL();

		[HttpGet]
		public ActionResult Index()
		{
			return View();
		}

		[HttpGet]
		public ActionResult Form()
		{
			return View();
		}

		[HttpGet]
		[AjaxOnly(false)]
		public ActionResult GetList(string queryJson)
		{
			IEnumerable<dm_basesettingEntity> data = dM_BaseSettingIBLL.GetList(queryJson);
			return Success(data);
		}

		[HttpGet]
		[AjaxOnly(false)]
		public ActionResult GetPageList(string pagination, string queryJson)
		{
			Pagination paginationobj = pagination.ToObject<Pagination>();
			IEnumerable<dm_basesettingEntity> data = dM_BaseSettingIBLL.GetPageList(paginationobj, queryJson);
			var jsonData = new
			{
				rows = data,
				total = paginationobj.total,
				page = paginationobj.page,
				records = paginationobj.records
			};
			return Success(jsonData);
		}

		[HttpGet]
		[AjaxOnly(false)]
		public ActionResult GetFormData(string keyValue)
		{
			dm_basesettingEntity data = dM_BaseSettingIBLL.GetEntity(keyValue);
			return Success(data);
		}

		[HttpPost]
		[AjaxOnly(false)]
		public ActionResult DeleteForm(string keyValue)
		{
			dM_BaseSettingIBLL.DeleteEntity(keyValue);
			return Success("删除成功！");
		}

		[HttpPost]
		[AjaxOnly(false)]
		public ActionResult SaveForm(string keyValue, dm_basesettingEntity entity)
		{
			dM_BaseSettingIBLL.SaveEntity(keyValue, entity);
			return Success("保存成功！");
		}
	}
}
