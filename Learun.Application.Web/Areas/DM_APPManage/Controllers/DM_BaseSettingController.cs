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

                PushPayload pushPayload = PushObject_All_All_Alert(ALERT, plaform);

                client.SendPush(pushPayload);
            }
            catch (System.Exception ex)
            {
                throw;
            }
        }
        #endregion


        #region 推送模板
        Platform GetPlaform(int plaform)
        {
            Platform plaInfo = null;
            switch (plaform)
            {
                case 0:
                    plaInfo = Platform.all();
                    break;
                case 1:
                    plaInfo = Platform.ios();
                    break;
                case 2:
                    plaInfo = Platform.android();
                    break;
            }
            return plaInfo;
        }
        public PushPayload PushObject_All_All_Alert(string ALERT, int plaform)
        {
            PushPayload pushPayload = new PushPayload()
            {
                platform = GetPlaform(plaform),
                audience = Audience.all(),
                notification = new Notification().setAlert(ALERT)
            };
            return pushPayload;
        }

        public static PushPayload PushObject_all_alia_alert(string ALERT)
        {
            PushPayload pushPayload_alias = new PushPayload()
            {
                platform = Platform.android(),
                audience = Audience.s_alias("alias1"),
                notification = new Notification().setAlert(ALERT)
            };
            return pushPayload_alias;
        }

        public static PushPayload PushObject_all_alias_alert(string ALERT)
        {
            PushPayload pushPayload_alias = new PushPayload()
            {
                platform = Platform.android()
            };
            string[] alias = new string[] { "alias1", "alias2", "alias3" };
            pushPayload_alias.audience = Audience.s_alias(alias);
            pushPayload_alias.notification = new Notification().setAlert(ALERT);
            return pushPayload_alias;
        }

        public static PushPayload PushObject_registrationId(string ALERT)
        {
            PushPayload pushPayload = new PushPayload()
            {
                platform = Platform.all()
            };
            string[] rId = new string[] { "registrationId1" };
            pushPayload.audience = Audience.s_registrationId(rId);
            pushPayload.notification = new Notification().setAlert(ALERT);
            return pushPayload;
        }

        public static PushPayload PushObject_Android_Tag_AlertWithTitle(string ALERT, string TITLE)
        {
            PushPayload pushPayload = new PushPayload()
            {
                platform = Platform.android(),
                audience = Audience.s_tag("tag1"),
                notification = Notification.android(ALERT, TITLE)
            };
            return pushPayload;
        }

        public static PushPayload PushObject_android_and_ios()
        {
            PushPayload pushPayload = new PushPayload()
            {
                platform = Platform.android_ios()
            };
            pushPayload.audience = Audience.s_tag("tag1");

            var notification = new Notification().setAlert("alert content");
            notification.AndroidNotification = new AndroidNotification().setTitle("Android Title");
            notification.IosNotification = new IosNotification();
            notification.IosNotification.incrBadge(1);
            notification.IosNotification.setMutableContent(true);
            notification.IosNotification.AddExtra("extra_key", "extra_value");
            pushPayload.notification = notification.Check();
            return pushPayload;
        }

        public static PushPayload PushObject_android_with_options()
        {
            PushPayload pushPayload = new PushPayload()
            {
                platform = Platform.android_ios()
            };
            pushPayload.audience = Audience.s_registrationId();

            AndroidNotification androidnotification = new AndroidNotification();
            androidnotification.setAlert("Push Object android with options");
            androidnotification.setBuilderID(3);
            androidnotification.setStyle(1);
            androidnotification.setAlert_type(1);
            androidnotification.setBig_text("big text content");
            androidnotification.setInbox("JSONObject");
            androidnotification.setBig_pic_path("picture url");
            androidnotification.setPriority(0);
            androidnotification.setCategory("category str");

            var notification = new Notification().setAlert("alert content");
            notification.AndroidNotification = androidnotification;
            notification.IosNotification = new IosNotification();
            notification.IosNotification.incrBadge(1);
            notification.IosNotification.AddExtra("extra_key", "extra_value");
            pushPayload.notification = notification.Check();
            return pushPayload;
        }

        public static PushPayload PushObjectWithExtrasAndMessage(string MSG_CONTENT)
        {
            PushPayload pushPayload = new PushPayload()
            {
                platform = Platform.android_ios(),
                audience = Audience.all()
            };

            pushPayload.notification = new Notification()
            {
                IosNotification = new IosNotification()
                    .setAlert("sound default")
                    .setBadge(5)
                    .setSound("default")
                    .AddExtra("from", "JPush")
            };
            pushPayload.message = Message.content(MSG_CONTENT);
            return pushPayload;
        }

        public static PushPayload PushObject_ios_alert_json(string MSG_CONTENT)
        {
            PushPayload pushPayload = new PushPayload()
            {
                platform = Platform.all(),
                audience = Audience.all()
            };

            Hashtable alert = new Hashtable
            {
                ["title"] = "JPush Title",
                ["subtitle"] = "JPush Subtitle",
                ["body"] = "JPush Body"
            };

            pushPayload.notification = new Notification()
            {
                IosNotification = new IosNotification()
                    .setAlert(alert)
                    .setBadge(5)
                    .setSound("happy")
                    .AddExtra("from", "JPush")
            };
            pushPayload.message = Message.content(MSG_CONTENT);
            return pushPayload;
        }

        public static PushPayload PushObject_ios_audienceMore_messageWithExtras(string MSG_CONTENT)
        {
            var pushPayload = new PushPayload()
            {
                platform = Platform.android_ios(),
                audience = Audience.s_tag("tag1", "tag2"),
                message = Message.content(MSG_CONTENT).AddExtras("from", "JPush")
            };
            return pushPayload;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public static PushPayload PushObject_apns_production_options(string MSG_CONTENT)
        {
            var pushPayload = new PushPayload()
            {
                platform = Platform.android_ios(),
                audience = Audience.s_tag("tag1", "tag2"),
                message = Message.content(MSG_CONTENT).AddExtras("from", "JPush")
            };
            pushPayload.options.apns_production = false;
            return pushPayload;
        }

        public static PushPayload PushSendSmsMessage(string ALERT, string SMSMESSAGE, int DELAY_TIME = 1)
        {
            var pushPayload = new PushPayload()
            {
                platform = Platform.all(),
                audience = Audience.all(),
                notification = new Notification().setAlert(ALERT)
            };

            SmsMessage sms_message = new SmsMessage();
            sms_message.setContent(SMSMESSAGE);
            sms_message.setDelayTime(DELAY_TIME);//发送短信的延时

            pushPayload.sms_message = sms_message;
            return pushPayload;
        }
        #endregion
    }
}
