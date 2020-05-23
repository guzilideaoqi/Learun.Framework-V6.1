using Learun.Application.TwoDevelopment.Common;
using Learun.Application.TwoDevelopment.DM_APPManage;
using Learun.Util;
using System;
using System.Collections.Generic;
using System.IO;
using System.Web;
using System.Web.Mvc;

namespace Learun.Application.Web.Areas.DM_APPManage.Controllers
{
	public class DM_BannerController : MvcControllerBase
	{
		private DM_BannerIBLL dM_BannerIBLL = new DM_BannerBLL();
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
			IEnumerable<dm_bannerEntity> data = dM_BannerIBLL.GetList(queryJson);
			return Success(data);
		}

		[HttpGet]
		[AjaxOnly(false)]
		public ActionResult GetPageList(string pagination, string queryJson)
		{
			Pagination paginationobj = pagination.ToObject<Pagination>();
			IEnumerable<dm_bannerEntity> data = dM_BannerIBLL.GetPageList(paginationobj, queryJson);
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
		public ActionResult GetFormData(int keyValue)
		{
			dm_bannerEntity data = dM_BannerIBLL.GetEntity(keyValue);
			return Success(data);
		}

		[HttpPost]
		[AjaxOnly(false)]
		public ActionResult DeleteForm(int keyValue)
		{
			dM_BannerIBLL.DeleteEntity(keyValue);
			return Success("删除成功！");
		}

		[HttpPost]
		[ValidateAntiForgeryToken]
		[AjaxOnly(false)]
		public ActionResult SaveForm(int keyValue, dm_bannerEntity entity)
		{
			dM_BannerIBLL.SaveEntity(keyValue, entity);
			return Success("保存成功！");
		}

		[HttpPost]
		public ActionResult UploadFile(int keyValue, dm_bannerEntity entity)
		{
			HttpFileCollection files = System.Web.HttpContext.Current.Request.Files;
			if (files.Count > 0)
			{
				if (files[0].ContentLength == 0 || string.IsNullOrEmpty(files[0].FileName))
				{
					return HttpNotFound();
				}
				UserInfo userInfo = LoginUserInfo.Get();
				/*string FileEextension = Path.GetExtension(files[0].FileName);
				string virtualPath = $"/Resource/GoodImage/{Guid.NewGuid().ToString()}{FileEextension}";
				string fullFileName = base.Server.MapPath("~" + virtualPath);
				string path = Path.GetDirectoryName(fullFileName);
				Directory.CreateDirectory(path);
				files[0].SaveAs(fullFileName);
				entity.b_image = virtualPath;*/

				entity.b_image = OSSHelper.PutObject(dM_BaseSettingIBLL.GetEntityByCache(userInfo.companyId), "", files[0]);
			}
			dM_BannerIBLL.SaveEntity(keyValue, entity);
			return Success("保存成功。");
		}
	}
}
