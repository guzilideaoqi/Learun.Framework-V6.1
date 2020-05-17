﻿using Learun.Application.TwoDevelopment.Common;
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
    /// 日 期：2020-04-16 16:08
    /// 描 述：任务类型
    /// </summary>
    public class DM_Task_TypeController : MvcControllerBase
    {
        private DM_Task_TypeIBLL dM_Task_TypeIBLL = new DM_Task_TypeBLL();
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
            var data = dM_Task_TypeIBLL.GetList(queryJson);
            return Success(data);
        }

        [HttpGet]
        [AjaxOnly]
        public ActionResult GetListByWeb()
        {
            UserInfo userInfo = LoginUserInfo.Get();
            var data = dM_Task_TypeIBLL.GetList("{\"appid\":\"" + userInfo.companyId + "\"}");
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
            var data = dM_Task_TypeIBLL.GetPageList(paginationobj, queryJson);
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
            var data = dM_Task_TypeIBLL.GetEntity(keyValue);
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
            dM_Task_TypeIBLL.DeleteEntity(keyValue);
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
        public ActionResult SaveForm(int keyValue, dm_task_typeEntity entity)
        {
            dM_Task_TypeIBLL.SaveEntity(keyValue, entity);
            return Success("保存成功！");
        }
        #endregion

        [HttpPost]
        public ActionResult UploadFile(int keyValue, dm_task_typeEntity entity)
        {
            HttpFileCollection files = System.Web.HttpContext.Current.Request.Files;
            if (files.Count > 0)
            {
                HttpPostedFile pic_file = files[0];
                if (pic_file.ContentLength != 0 && !string.IsNullOrEmpty(pic_file.FileName))
                {
                    UserInfo userInfo = LoginUserInfo.Get();
                    entity.image = OSSHelper.PutObject(dM_BaseSettingIBLL.GetEntityByCache(userInfo.companyId), "", files[0]);
                }
            }
            dM_Task_TypeIBLL.SaveEntity(keyValue, entity);
            return Success("保存成功。");
        }
    }
}
