using Learun.Application.TwoDevelopment.Common;
using Learun.Application.TwoDevelopment.DM_APPManage;
using Learun.Util;
using System;
using System.IO;
using System.Web;
using System.Web.Mvc;

namespace Learun.Application.Web.Areas.DM_APPManage.Controllers
{
    /// <summary>
    /// 版 本 Learun-ADMS V6.1.6.0 力软敏捷开发框架
    /// Copyright (c) 2013-2017 上海力软信息技术有限公司
    /// 创 建：超级管理员
    /// 日 期：2020-04-06 21:10
    /// 描 述：文案管理
    /// </summary>
    [HandlerLogin(FilterMode.Ignore)]
    public class DM_ArticleController : MvcControllerBase
    {
        private DM_ArticleIBLL dM_ArticleIBLL = new DM_ArticleBLL();
        private DM_BaseSettingIBLL dM_BaseSettingIBLL = new DM_BaseSettingBLL();

        #region 视图功能

        /// <summary>
        /// 主页面
        /// <summary>
        /// <returns></returns>
        [HttpGet]
        public ActionResult Index()
        {
            return View();
        }
        /// <summary>
        /// 表单页
        /// <summary>
        /// <returns></returns>
        [HttpGet]
        public ActionResult Form()
        {
            return View();
        }

        [HttpGet]
        public ActionResult LookArticle(int id)
        {
            var data = dM_ArticleIBLL.GetEntity(id);
            ViewBag.PageHtml = data.content;
            ViewBag.Title = data.title;
            return View();
        }
        #endregion

        #region 获取数据

        /// <summary>
        /// 获取列表数据
        /// <summary>
        /// <returns></returns>
        [HttpGet]
        [AjaxOnly]
        public ActionResult GetList(string queryJson)
        {
            var data = dM_ArticleIBLL.GetList(queryJson);
            return Success(data);
        }
        /// <summary>
        /// 获取列表分页数据
        /// <param name="pagination">分页参数</param>
        /// <summary>
        /// <returns></returns>
        [HttpGet]
        [AjaxOnly]
        public ActionResult GetPageList(string pagination, string queryJson)
        {
            Pagination paginationobj = pagination.ToObject<Pagination>();
            var data = dM_ArticleIBLL.GetPageList(paginationobj, queryJson);
            var jsonData = new
            {
                rows = data,
                total = paginationobj.total,
                page = paginationobj.page,
                records = paginationobj.records
            };
            return Success(jsonData);
        }
        /// <summary>
        /// 获取表单数据
        /// <param name="keyValue">主键</param>
        /// <summary>
        /// <returns></returns>
        [HttpGet]
        [AjaxOnly]
        public ActionResult GetFormData(int keyValue)
        {
            var data = dM_ArticleIBLL.GetEntity(keyValue);
            return Success(data);
        }
        #endregion

        #region 提交数据

        /// <summary>
        /// 删除实体数据
        /// <param name="keyValue">主键</param>
        /// <summary>
        /// <returns></returns>
        [HttpPost]
        [AjaxOnly]
        public ActionResult DeleteForm(int keyValue)
        {
            dM_ArticleIBLL.DeleteEntity(keyValue);
            return Success("删除成功！");
        }
        /// <summary>
        /// 保存实体数据（新增、修改）
        /// <param name="keyValue">主键</param>
        /// <summary>
        /// <returns></returns>
 		[HttpPost]
        [ValidateAntiForgeryToken]
        [AjaxOnly(false)]
        [ValidateInput(false)]
        public ActionResult SaveForm(int keyValue, dm_articleEntity entity)
        {
            dM_ArticleIBLL.SaveEntity(keyValue, entity);
            return Success("保存成功！");
        }

        [HttpPost]
        [ValidateInput(false)]
        public ActionResult UploadFile(int keyValue, dm_articleEntity entity)
        {
            entity.content = HttpUtility.UrlDecode(entity.content);
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
                entity.a_image = virtualPath;*/
                string appid = userInfo.IsEmpty() ? "e2b3ec3a-310b-4ab8-aa81-b563ac8f3006" : userInfo.companyId;
                entity.a_image = OSSHelper.PutObject(dM_BaseSettingIBLL.GetEntityByCache(appid), "", files[0]);
            }
            dM_ArticleIBLL.SaveEntity(keyValue, entity);
            return Success("保存成功。");
        }
        #endregion

        /// <summary>
        /// 获取字典分类列表(树结构)
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [AjaxOnly]
        public ActionResult GetClassifyTree()
        {
            var data = dM_ArticleIBLL.GetArticleTree();
            return this.Success(data);
        }

    }
}
