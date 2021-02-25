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
        public ActionResult SetLevel()
        {
            return View();
        }

        [HttpGet]
        public ActionResult SelectUser()
        {
            return View();
        }

        [HttpGet]
        public ActionResult SetInviteCode()
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

        [HttpPost]
        [AjaxOnly(false)]
        public ActionResult UpdateUserInfo(int keyValue, dm_userEntity entity)
        {
            dM_UserIBLL.SaveEntity(keyValue, entity);
            return Success("修改成功！");
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
            try
            {
                dM_UserIBLL.UpdateAccountPrice(User_ID, UpdatePrice, UpdateType, Remark);
                return Success("修改成功！");
            }
            catch (System.Exception ex)
            {
                return Fail(ex.InnerException.Message);
            }
        }

        /// <summary>
        /// 自定义用户邀请码
        /// </summary>
        /// <param name="keyValue"></param>
        /// <param name="entity"></param>
        /// <returns></returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        [AjaxOnly(false)]
        public ActionResult SetInviteCode(int User_ID, string InviteCode)
        {
            try
            {
                dM_UserIBLL.SetInviteCode(User_ID, InviteCode);
                return Success("自定义邀请码设置成功!");
            }
            catch (System.Exception ex)
            {
                return Fail(ex.InnerException.Message);
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [AjaxOnly(false)]
        public ActionResult SetUserLevel(string userids, int user_level)
        {
            try
            {
                dM_UserIBLL.SetUserLevel(userids, user_level);
                return Success("修改成功！");
            }
            catch (System.Exception ex)
            {
                return Fail(ex.InnerException.Message);
            }
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


        /// <summary>
        /// 清除淘宝授权
        /// </summary>
        /// <param name="User_ID"></param>
        /// <returns></returns>
        [HttpPost]
        [AjaxOnly(false)]
        public ActionResult Clear_TB_Relation_Auth(int User_ID)
        {
            try
            {
                dM_UserIBLL.Clear_TB_Relation_Auth(User_ID);

                return Success("授权清除成功!");
            }
            catch (System.Exception ex)
            {
                return Fail(ex.InnerException.Message);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        [HttpPost]
        [AjaxOnly(false)]
        public ActionResult ClearShareImage()
        {
            try
            {
                int fileCount = dM_UserIBLL.ClearShareImage();

                return Success("操作成功,当前共清空" + fileCount + "条数据!");
            }
            catch (System.Exception ex)
            {
                return Fail(ex.InnerException.Message);
            }
        }

        [HttpPost]
        [AjaxOnly(false)]
        public ActionResult NoCheckDataStatistic() {
            try
            {
                return Success(dM_UserIBLL.NoCheckDataStatistic());
            }
            catch (System.Exception ex)
            {
                return Fail(ex.InnerException.Message);
            }
        }
    }
}
