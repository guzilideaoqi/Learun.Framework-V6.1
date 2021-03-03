using Learun.Application.TwoDevelopment.Common;
using Learun.Application.TwoDevelopment.DM_APPManage;
using Learun.Util;
using System.Web;
using System.Web.Mvc;

namespace Learun.Application.Web.Areas.DM_APPManage.Controllers
{
    /// <summary>
    /// 版 本 Learun-ADMS V6.1.6.0 力软敏捷开发框架
    /// Copyright (c) 2013-2017 上海力软信息技术有限公司
    /// 创 建：超级管理员
    /// 日 期：2021-03-03 10:49
    /// 描 述：附件管理
    /// </summary>
    public class dm_attachmentController : MvcControllerBase
    {
        private dm_attachmentIBLL dm_attachmentIBLL = new dm_attachmentBLL();
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

        /// <summary>
        /// 附件管理
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public ActionResult AttachMentManage()
        {
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
            var data = dm_attachmentIBLL.GetList(queryJson);
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
            var data = dm_attachmentIBLL.GetPageList(paginationobj, queryJson);
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
        public ActionResult GetFormData(int? keyValue)
        {
            var data = dm_attachmentIBLL.GetEntity(keyValue);
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
        public ActionResult DeleteForm(int? keyValue)
        {
            dm_attachmentIBLL.DeleteEntity(keyValue);
            return Success("删除成功！");
        }
        /// <summary>
        /// 保存实体数据（新增、修改）
        /// <param name="keyValue">主键</param>
        /// <summary>
        /// <returns></returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        [AjaxOnly]
        public ActionResult SaveForm(int? keyValue, dm_attachmentEntity entity)
        {
            dm_attachmentIBLL.SaveEntity(keyValue, entity);
            return Success("保存成功！");
        }
        #endregion

        [HttpPost]
        public ActionResult UploadFile(int keyValue, dm_attachmentEntity entity)
        {
            HttpFileCollection files = System.Web.HttpContext.Current.Request.Files;
            if (files.Count > 0)
            {
                if (files[0].ContentLength == 0 || string.IsNullOrEmpty(files[0].FileName))
                {
                    return HttpNotFound();
                }
                UserInfo userInfo = LoginUserInfo.Get();
                HttpPostedFile httpPostedFile = files[0];
                entity.file_url = OSSHelper.PutObject(dM_BaseSettingIBLL.GetEntityByCache(userInfo.companyId), "", httpPostedFile);
                entity.file_size = httpPostedFile.ContentLength.ToString();
                entity.file_name = httpPostedFile.FileName;
                entity.file_type = 1;
            }
            dm_attachmentIBLL.SaveEntity(keyValue, entity);
            return Success("保存成功。");
        }
    }
}
