using Learun.Application.TwoDevelopment.DM_APPManage;
using Learun.Util;
using System.Collections.Generic;
using System.Data;
using System.Web.Mvc;

namespace Learun.Application.Web.Areas.DM_APPManage.Controllers
{
    public class DM_UserController : MvcControllerBase
    {
        private DM_UserIBLL dM_UserIBLL = new DM_UserBLL();

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
        public ActionResult LookUserDetail()
        {
            return View();
        }

        [HttpGet]
        public ActionResult UpdateAccountPrice()
        {
            return View();
        }

        [HttpGet]
        [AjaxOnly(false)]
        public ActionResult GetList(string queryJson)
        {
            IEnumerable<dm_userEntity> data = dM_UserIBLL.GetList(queryJson);
            return Success(data);
        }

        [HttpGet]
        [AjaxOnly(false)]
        public ActionResult GetPageList(string pagination, string queryJson)
        {
            Pagination paginationobj = pagination.ToObject<Pagination>();
            IEnumerable<dm_userEntity> data = dM_UserIBLL.GetPageList(paginationobj, queryJson);
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
        public ActionResult GetPageListByDataTable(string pagination, string queryJson)
        {
            Pagination paginationobj = pagination.ToObject<Pagination>();
            DataTable data = dM_UserIBLL.GetPageListByDataTable(paginationobj, queryJson);
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
            dm_userEntity data = dM_UserIBLL.GetEntity(keyValue);
            return Success(data);
        }

        [HttpPost]
        [AjaxOnly(false)]
        public ActionResult DeleteForm(int keyValue)
        {
            dM_UserIBLL.DeleteEntity(keyValue);
            return Success("删除成功！");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [AjaxOnly(false)]
        public ActionResult SaveForm(int keyValue, dm_userEntity entity)
        {
            dM_UserIBLL.SaveEntity(keyValue, entity);
            return Success("保存成功！");
        }

        [HttpGet]
        [AjaxOnly(false)]
        public ActionResult GetChildUser(int userid)
        {
            IEnumerable<dm_userEntity> data = dM_UserIBLL.GetChildUser(userid);
            return Success(data);
        }

        /// <summary>
        /// 修改账户余额
        /// </summary>
        /// <param name="keyValue"></param>
        /// <param name="entity"></param>
        /// <returns></returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        [AjaxOnly(false)]
        public ActionResult UpdateAccountPrice(int User_ID, decimal UpdatePrice, int UpdateType, string Remark)
        {
            dM_UserIBLL.UpdateAccountPrice(User_ID, UpdatePrice, UpdateType, Remark);
            return Success("修改成功！");
        }

        /// <summary>
        /// 获取统计数据
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [AjaxOnly(false)]
        public ActionResult GetStaticData()
        {
            var data = new
            {
                StaticData1 = dM_UserIBLL.GetStaticData1(),
                StaticData2 = dM_UserIBLL.GetStaticData2(),
                StaticData3 = dM_UserIBLL.GetStaticData3(),
                StaticData4 = dM_UserIBLL.GetStaticData4(),
            };
            return Success(data);
        }
    }
}
