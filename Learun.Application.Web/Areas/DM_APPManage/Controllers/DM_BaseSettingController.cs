using cn.jpush.api.push.mode;
using Learun.Application.TwoDevelopment.DM_APPManage;
using Learun.Util;
using System.Collections.Generic;
using System.Web.Mvc;
using cn.jpush.api;
using cn.jpush.api.push.notification;
using System.Collections;
using Hyg.Common.DTKTools.DTKModel;
using Learun.Cache.Base;
using Learun.Cache.Factory;
using System;
using Learun.Application.TwoDevelopment.Common;

namespace Learun.Application.Web.Areas.DM_APPManage.Controllers
{
    public class DM_BaseSettingController : MvcControllerBase
    {
        private DM_BaseSettingIBLL dM_BaseSettingIBLL = new DM_BaseSettingBLL();
        private dm_basesetting_tipIBLL dM_BaseSetting_tipIBLL = new dm_basesetting_tipBLL();
        private ICache redisCache = CacheFactory.CaChe();

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
        public ActionResult JPushManage()
        {
            return View();
        }

        [HttpGet]
        [AjaxOnly(false)]
        public ActionResult GetList(string queryJson)
        {
            IEnumerable<dm_basesettingEntity> data = dM_BaseSettingIBLL.GetList(queryJson);
            return Success(data);
        }

        [HttpGet]
        [AjaxOnly(false)]
        public ActionResult GetPageList(string pagination, string queryJson)
        {
            Pagination paginationobj = pagination.ToObject<Pagination>();
            IEnumerable<dm_basesettingEntity> data = dM_BaseSettingIBLL.GetPageList(paginationobj, queryJson);
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
            dm_basesettingEntity data = dM_BaseSettingIBLL.GetEntity(keyValue);

            dm_basesetting_tipEntity dm_Basesetting_TipEntity = null;
            if (!data.IsEmpty())
            {
                dm_Basesetting_TipEntity = dM_BaseSetting_tipIBLL.GetEntityByAppID(data.appid);
            }

            return Success(new { BaseSetting = data, BaseSetting_Tip = dm_Basesetting_TipEntity });
        }

        [HttpGet]
        [AjaxOnly(false)]
        public ActionResult GetGoodTypeByCache()
        {
            try
            {
                string cacheKey = "SuperCategory";
                List<CategoryItem> categoryItems = redisCache.Read<List<CategoryItem>>(cacheKey, 7L);
                return Success("获取成功", categoryItems);
            }
            catch (Exception ex)
            {
                return FailException(ex);
            }
        }

        [HttpPost]
        [AjaxOnly(false)]
        public ActionResult DeleteForm(string keyValue)
        {
            dM_BaseSettingIBLL.DeleteEntity(keyValue);
            return Success("删除成功！");
        }

        [HttpPost]
        [AjaxOnly(false)]
        public ActionResult SaveForm(string keyValue, dm_basesettingEntity entity, dm_basesetting_tipEntity dm_Basesetting_TipEntity)
        {
            dM_BaseSettingIBLL.SaveEntity(keyValue, entity);

            dM_BaseSetting_tipIBLL.SaveEntityByAppID(entity.appid, dm_Basesetting_TipEntity);

            return Success("保存成功！");
        }


        [HttpPost]
        [AjaxOnly(false)]
        public ActionResult ExcutePush(string ALERT, string TITLE, string MSG_CONTENT, int Plaform = 0)
        {
            try
            {
                PushObject_All_All_Alert(ALERT, TITLE, MSG_CONTENT, Plaform);
                return Success("推送成功！");
            }
            catch (System.Exception ex)
            {
                return Fail(ex.Message);
            }
        }

        #region 推送消息
        public void PushObject_All_All_Alert(string ALERT, string TITLE, string MSG_CONTENT, int plaform = 0)
        {
            try
            {
                UserInfo userInfo = LoginUserInfo.Get();

                dm_basesettingEntity dm_BasesettingEntity = dM_BaseSettingIBLL.GetEntityByCache(userInfo.companyId);

                JPushClient client = new JPushClient(dm_BasesettingEntity.jg_appkey, dm_BasesettingEntity.jg_appsecret);

                PushPayload pushPayload = JPushClientHelper.PushObject_All_All_Alert(ALERT, plaform);

                client.SendPush(pushPayload);
            }
            catch (System.Exception ex)
            {
                throw;
            }
        }
        #endregion
    }
}
