/*===================================Copyright© 2021 xxx Ltd. All rights reserved.==============================

文件类名 : JPushClientHelper.cs
创建人员 : Mr.Hu
创建时间 : 2021-02-20 15:02:34 
备注说明 : 

 =====================================End=======================================================*/
using cn.jpush.api;
using cn.jpush.api.push.mode;
using cn.jpush.api.push.notification;
using Hyg.Common.OtherTools;
using Learun.Application.TwoDevelopment.DM_APPManage;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Learun.Application.TwoDevelopment.Common
{
    public enum PushTarget
    {
        User = 1,//单用户
        Partners = 2//合伙人团队
    }

    /// <summary>
    /// JPushClientHelper
    /// </summary>
    public class JPushClientHelper
    {
        #region 推送消息
        /// <summary>
        /// 向用户推送一条消息
        /// </summary>
        /// <param name="appid">平台表示ID</param>
        /// <param name="user_id">用户ID</param>
        /// <param name="title">标题</param>
        /// <param name="content">内容</param>
        public static void SendPush(string appid, string target, string title, string content, PushTarget pushTarget = PushTarget.User)
        {
            Audience audience = null;
            if (pushTarget == PushTarget.User)
            {
                audience = Audience.s_alias(new string[] { target.ToString() });
            }
            else
            {
                audience = Audience.s_tag(new string[] { target.ToString() });
            }

            #region 构造安卓消息模板
            AndroidNotification androidNotification = new AndroidNotification();
            androidNotification.setTitle(title);
            #endregion

            #region 构造IOS消息模板
            IosNotification iosNotification = new IosNotification();
            iosNotification.setBadge(1);
            #endregion

            PushPayload pushPayload = new PushPayload()
            {
                platform = GetPlaform(0),
                audience = audience
            };

            var notification = new Notification().setAlert(content);
            notification.AndroidNotification = androidNotification;
            notification.IosNotification = iosNotification;
            pushPayload.notification = notification.Check();

            //pushPayload.message = Message.content(content);
            SendPush(appid, pushPayload);
        }
        #endregion

        #region 推送极光消息
        static void SendPush(string appid, PushPayload pushPayload)
        {
            dm_basesettingEntity dm_BasesettingEntity = new DM_BaseSettingBLL().GetEntityByCache(appid);
            if (dm_BasesettingEntity.IsEmpty())
            {
                throw new Exception("未获取到极光配置信息!");
            }
            JPushClient client = new JPushClient(dm_BasesettingEntity.jg_appkey, dm_BasesettingEntity.jg_appsecret);
            client.SendPush(pushPayload);
        }
        #endregion

        #region 推送模板(推送demo)
        static Platform GetPlaform(int plaform)
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
        public static PushPayload PushObject_All_All_Alert(string ALERT, int plaform)
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
