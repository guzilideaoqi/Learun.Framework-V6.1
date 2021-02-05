using Learun.Application.TwoDevelopment.DM_APPManage;
using Learun.Util;
using System.Web.Mvc;

namespace Learun.Application.Web.Areas.DM_APPManage.Controllers
{
    /// <summary>
    /// 版 本 Learun-ADMS V6.1.6.0 力软敏捷开发框架
    /// Copyright (c) 2013-2017 上海力软信息技术有限公司
    /// 创 建：超级管理员
    /// 日 期：2020-04-16 16:01
    /// 描 述：任务中心
    /// </summary>
    public class DM_TaskController : MvcControllerBase
    {
        private DM_TaskIBLL dM_TaskIBLL = new DM_TaskBLL();

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
        public ActionResult LookReviceDetail()
        {
            return View();
        }

        /// <summary>
        /// 发布任务
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public ActionResult PubTask()
        {
            return View();
        }

        /// <summary>
        /// 获取任务详情
        /// </summary>
        /// <param name="TaskID"></param>
        /// <returns></returns>
        [HttpGet]
        public ActionResult LookTaskDetail(string TaskID)
        {
            return View();
        }

        [HttpGet]
        public ActionResult RebutTask() {
            return View();
        }

        [HttpGet]
        public ActionResult CheckReviceDetail() {
            return View();
        }

        [HttpGet]
        public ActionResult RebutReviceTask()
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
            var data = dM_TaskIBLL.GetList(queryJson);
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
            var data = dM_TaskIBLL.GetPageList(paginationobj, queryJson);
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
        /// 获取列表分页数据
        /// </summary>
        /// <param name="pagination"></param>
        /// <param name="queryJson"></param>
        /// <returns></returns>
        [HttpGet]
        [AjaxOnly]
        public ActionResult GetPageListByDataTable(string pagination, string queryJson)
        {
            Pagination paginationobj = pagination.ToObject<Pagination>();
            var data = dM_TaskIBLL.GetPageListByDataTable(paginationobj, queryJson);
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
            var data = dM_TaskIBLL.GetEntity(keyValue);
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
            dM_TaskIBLL.DeleteEntity(keyValue);
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
        public ActionResult SaveForm(int keyValue, dm_taskEntity entity)
        {
            dM_TaskIBLL.SaveEntity(keyValue, entity);
            return Success("保存成功！");
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
        public ActionResult PubNewTask(int keyValue, dm_taskEntity entity)
        {
            dM_TaskIBLL.ReleaseTaskByWeb(entity);
            return Success("发布成功！");
        }

        /// <summary>
        /// 后台审核发布任务
        /// </summary>
        /// <param name="keyValue"></param>
        /// <param name="entity"></param>
        /// <returns></returns>
        [HttpPost]
        [AjaxOnly(false)]
        public ActionResult CheckTaskByWeb(int keyValue)
        {
            dM_TaskIBLL.CheckTaskByWeb(keyValue);
            return Success("审核成功！");
        }

        /// <summary>
        /// 后台审核发布任务
        /// </summary>
        /// <param name="keyValue"></param>
        /// <param name="entity"></param>
        /// <returns></returns>
        [HttpPost]
        [AjaxOnly(false)]
        public ActionResult DownTask(int keyValue)
        {
            dM_TaskIBLL.DownTask(keyValue);
            return Success("任务下架成功！");
        }

        [HttpPost]
        [AjaxOnly(false)]
        public ActionResult UpdateSortValue(int task_id, int sort_value)
        {
            dM_TaskIBLL.UpdateSortValue(task_id, sort_value);
            return Success("修改成功!");
        }

        [HttpPost]
        [AjaxOnly(false)]
        public ActionResult RebutTaskByWeb(int id, string remark)
        {
            dM_TaskIBLL.RebutTaskByWeb(id, remark);
            return Success("驳回成功!");
        }
        #endregion

    }
}
