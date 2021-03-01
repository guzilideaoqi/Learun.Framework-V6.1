using Learun.Application.TwoDevelopment.DM_APPManage;
using Learun.Util;
using System.Collections.Generic;
using System.Data;
using System.Web.Mvc;

namespace Learun.Application.Web.Areas.DM_APPManage.Controllers
{
    public class DM_IntergralChangeRecordController : MvcControllerBase
    {
        private DM_IntergralChangeRecordIBLL dM_IntergralChangeRecordIBLL = new DM_IntergralChangeRecordBLL();

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
        public ActionResult SendExpressNumber()
        {
            return View();
        }

        [HttpGet]
        [AjaxOnly(false)]
        public ActionResult GetList(string queryJson)
        {
            IEnumerable<dm_intergralchangerecordEntity> data = dM_IntergralChangeRecordIBLL.GetList(queryJson);
            return Success(data);
        }

        [HttpGet]
        [AjaxOnly(false)]
        public ActionResult GetPageList(string pagination, string queryJson)
        {
            Pagination paginationobj = pagination.ToObject<Pagination>();
            DataTable data = dM_IntergralChangeRecordIBLL.GetIntegralGoodRecord(paginationobj, queryJson);
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
            dm_intergralchangerecordEntity data = dM_IntergralChangeRecordIBLL.GetEntity(keyValue);
            return Success(data);
        }

        [HttpPost]
        [AjaxOnly(false)]
        public ActionResult DeleteForm(int keyValue)
        {
            dM_IntergralChangeRecordIBLL.DeleteEntity(keyValue);
            return Success("删除成功！");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [AjaxOnly(false)]
        public ActionResult SaveForm(int keyValue, dm_intergralchangerecordEntity entity)
        {
            dM_IntergralChangeRecordIBLL.SaveEntity(keyValue, entity);
            return Success("保存成功！");
        }
    }
}
