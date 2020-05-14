using Learun.Application.TwoDevelopment.DM_APPManage;
using Learun.Util;
using System.Collections.Generic;
using System.Web.Mvc;

namespace Learun.Application.Web.Areas.DM_APPManage.Controllers
{
    public class DM_OrderController : MvcControllerBase
    {
        private DM_OrderIBLL dM_OrderIBLL = (DM_OrderIBLL)(object)new DM_OrderBLL();

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
        public ActionResult SyncOrder()
        {
            return View();
        }

        [HttpGet]
        [AjaxOnly(false)]
        public ActionResult GetList(string queryJson)
        {
            IEnumerable<dm_orderEntity> data = dM_OrderIBLL.GetList(queryJson);
            return Success(data);
        }

        [HttpGet]
        [AjaxOnly(false)]
        public ActionResult GetPageList(string pagination, string queryJson)
        {
            Pagination paginationobj = pagination.ToObject<Pagination>();
            IEnumerable<dm_orderEntity> data = dM_OrderIBLL.GetPageList(paginationobj, queryJson);
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
            dm_orderEntity data = dM_OrderIBLL.GetEntity(keyValue);
            return Success(data);
        }

        [HttpPost]
        [AjaxOnly(false)]
        public ActionResult DeleteForm(string keyValue)
        {
            dM_OrderIBLL.DeleteEntity(keyValue);
            return Success("删除成功！");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [AjaxOnly(false)]
        public ActionResult SaveForm(string keyValue, dm_orderEntity entity)
        {
            dM_OrderIBLL.SaveEntity(keyValue, entity);
            return Success("保存成功！");
        }

        [HttpPost]
        [AjaxOnly(false)]
        public ActionResult SyncOrder(int plaform, int timetype, int status, string startTime, string endTime)
        {
            int effectCount = dM_OrderIBLL.SyncOrder(plaform, timetype, status, startTime, endTime);
            return Success("本次执行完成,共同步" + effectCount + "条数据!");
        }
    }
}
