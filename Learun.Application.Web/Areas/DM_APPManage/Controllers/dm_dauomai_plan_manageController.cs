﻿using Hyg.Common.DuoMaiTools.DuoMaiRequest;
using Learun.Application.TwoDevelopment.DM_APPManage;
using Learun.Util;
using System.Collections.Generic;
using System.Web.Mvc;

namespace Learun.Application.Web.Areas.DM_APPManage.Controllers
{
    /// <summary>
    /// 版 本 Learun-ADMS V6.1.6.0 力软敏捷开发框架
    /// Copyright (c) 2013-2017 上海力软信息技术有限公司
    /// 创 建：超级管理员
    /// 日 期：2021-03-03 10:50
    /// 描 述：多麦计划
    /// </summary>
    public class dm_dauomai_plan_manageController : MvcControllerBase
    {
        private dm_dauomai_plan_manageIBLL dm_dauomai_plan_manageIBLL = new dm_dauomai_plan_manageBLL();

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
        /// 同步推广计划
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public ActionResult SyncPlan()
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
            var data = dm_dauomai_plan_manageIBLL.GetList(queryJson);
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
            var data = dm_dauomai_plan_manageIBLL.GetPageList(paginationobj, queryJson);
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
            var data = dm_dauomai_plan_manageIBLL.GetEntity(keyValue);
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
            dm_dauomai_plan_manageIBLL.DeleteEntity(keyValue);
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
        public ActionResult SaveForm(int? keyValue, dm_dauomai_plan_manageEntity entity)
        {
            dm_dauomai_plan_manageIBLL.SaveEntity(keyValue, entity);
            return Success("保存成功！");
        }
        #endregion

        /// <summary>
        /// 同步推广计划
        /// </summary>
        /// <param name="query_CPS_Stores_PlansRequest"></param>
        /// <returns></returns>
        [HttpPost]
        [AjaxOnly]
        public ActionResult SyncPlanList(Query_CPS_Stores_PlansRequest query_CPS_Stores_PlansRequest)
        {
            dm_dauomai_plan_manageIBLL.SyncPlanList(query_CPS_Stores_PlansRequest);
            return Success("同步成功！");
        }

        [HttpPost]
        [AjaxOnly]
        public ActionResult StartPlan(int keyValue)
        {
            dm_dauomai_plan_manageIBLL.StartPlan(keyValue);
            return Success("激活成功!");
        }

        [HttpPost]
        [AjaxOnly]
        public ActionResult StopPlan(int keyValue)
        {
            dm_dauomai_plan_manageIBLL.StopPlan(keyValue);
            return Success("停用成功,该模块已从装修模板移除,若需要使用此功能，请在激活后重新装修!");
        }
    }
}
