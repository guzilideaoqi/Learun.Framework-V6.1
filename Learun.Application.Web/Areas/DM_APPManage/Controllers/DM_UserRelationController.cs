using Learun.Application.TwoDevelopment.DM_APPManage;
using Learun.Util;
using System.Collections.Generic;
using System.Web.Mvc;

namespace Learun.Application.Web.Areas.DM_APPManage.Controllers
{
    public class DM_UserRelationController : MvcControllerBase
    {
        private DM_UserRelationIBLL dM_UserRelationIBLL = new DM_UserRelationBLL();

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
            IEnumerable<dm_user_relationEntity> data = dM_UserRelationIBLL.GetList(queryJson);
            return Success(data);
        }

        [HttpGet]
        [AjaxOnly(false)]
        public ActionResult GetPageList(string pagination, string queryJson)
        {
            Pagination paginationobj = pagination.ToObject<Pagination>();
            IEnumerable<dm_user_relationEntity> data = dM_UserRelationIBLL.GetPageList(paginationobj, queryJson);
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
            dm_user_relationEntity data = dM_UserRelationIBLL.GetEntity(keyValue);
            return Success(data);
        }

        [HttpGet]
        [AjaxOnly(false)]
        public ActionResult GetFormDataByUserID(int? keyValue)
        {
            dm_user_relationEntity data = dM_UserRelationIBLL.GetEntityByUserID(keyValue);
            return Success(data);
        }

        [HttpPost]
        [AjaxOnly(false)]
        public ActionResult DeleteForm(int? keyValue)
        {
            dM_UserRelationIBLL.DeleteEntity(keyValue);
            return Success("删除成功！");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [AjaxOnly(false)]
        public ActionResult SaveForm(int? keyValue, dm_user_relationEntity entity)
        {
            dM_UserRelationIBLL.SaveEntity(keyValue, entity);
            return Success("保存成功！");
        }

        #region 更改会员上级并重置统计信息
        [HttpPost]
        [AjaxOnly(false)]
        public ActionResult UpdateUserParent(int UserID, int ParentID)
        {
            try
            {
                dM_UserRelationIBLL.UpdateUserParent(UserID, ParentID);

                return Success("修改成功!");
            }
            catch (System.Exception ex)
            {
                return FailException(ex);
            }

        }
        #endregion

        #region 重置会员统计信息
        [HttpPost]
        [AjaxOnly(false)]
        public ActionResult ResetUserStatistic(int UserID)
        {
            try
            {
                dM_UserRelationIBLL.ResetUserStatistic(UserID);
                return Success("重置成功!");
            }
            catch (System.Exception ex)
            {
                return FailException(ex);
            }
        }
        #endregion
    }
}
