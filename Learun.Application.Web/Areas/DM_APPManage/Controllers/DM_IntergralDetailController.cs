using Learun.Application.TwoDevelopment.DM_APPManage;
using Learun.Util;
using System.Collections.Generic;
using System.Web.Mvc;

namespace Learun.Application.Web.Areas.DM_APPManage.Controllers
{
	public class DM_IntergralDetailController : MvcControllerBase
	{
		private DM_IntergralDetailIBLL dM_IntergralDetailIBLL = new DM_IntergralDetailBLL();

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
			IEnumerable<dm_intergraldetailEntity> data = dM_IntergralDetailIBLL.GetList(queryJson);
			return Success(data);
		}

		[HttpGet]
		[AjaxOnly(false)]
		public ActionResult GetPageList(string pagination, string queryJson)
		{
			Pagination paginationobj = pagination.ToObject<Pagination>();
			IEnumerable<dm_intergraldetailEntity> data = dM_IntergralDetailIBLL.GetPageList(paginationobj, queryJson);
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
		public ActionResult GetFormData(int? keyValue)
		{
			dm_intergraldetailEntity data = dM_IntergralDetailIBLL.GetEntity(keyValue);
			return Success(data);
		}

		[HttpPost]
		[AjaxOnly(false)]
		public ActionResult DeleteForm(int? keyValue)
		{
			dM_IntergralDetailIBLL.DeleteEntity(keyValue);
			return Success("删除成功！");
		}

		[HttpPost]
		[ValidateAntiForgeryToken]
		[AjaxOnly(false)]
		public ActionResult SaveForm(int? keyValue, dm_intergraldetailEntity entity)
		{
			dM_IntergralDetailIBLL.SaveEntity(keyValue, entity);
			return Success("保存成功！");
		}
	}
}
