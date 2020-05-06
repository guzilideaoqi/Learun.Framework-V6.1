using HYG.CommonHelper.Common;
using Learun.Application.TwoDevelopment.DM_APPManage;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Learun.Util;
using Jiguang.JSMS;
using Jiguang.JSMS.Model;
using System.Net;

namespace Learun.Application.TwoDevelopment.Common
{
    public class CommonSMSHelper
    {
        public static string SendSms(string phone, string appid)
        {
            dm_basesettingEntity dm_BasesettingEntity = new DM_BaseSettingService().GetEntityByCache(appid);
            if (dm_BasesettingEntity.IsEmpty())
                throw new Exception("参数错误!");
            JSMSClient jsmsClient = new JSMSClient(dm_BasesettingEntity.jg_appkey, dm_BasesettingEntity.jg_appsecret);
            HttpResponse httpResponse = jsmsClient.SendCode(phone, int.Parse(dm_BasesettingEntity.sms_template_id), int.Parse(dm_BasesettingEntity.sms_sign_id));
            if (httpResponse.StatusCode != HttpStatusCode.OK)
            {
                throw new Exception(httpResponse.Content);
            }
            else
            {
                SmsMsg smsMsg = JsonConvert.JsonDeserialize<SmsMsg>(httpResponse.Content);
                return smsMsg.msg_id;
            }
        }

        public static bool IsPassVerification(string msgid, string VerificationCode, string appid)
        {
            dm_basesettingEntity dm_BasesettingEntity = new DM_BaseSettingService().GetEntityByCache(appid);
            if (dm_BasesettingEntity.IsEmpty())
                throw new Exception("参数错误!");
            JSMSClient jsmsClient = new JSMSClient(dm_BasesettingEntity.jg_appkey, dm_BasesettingEntity.jg_appsecret);
            HttpResponse httpResponse = jsmsClient.IsCodeValid(msgid, VerificationCode);

            if (httpResponse.StatusCode != HttpStatusCode.OK)
            {
                return false;
            }
            else
            {
                SmsVerifityMsg smsVerifityMsg = JsonConvert.JsonDeserialize<SmsVerifityMsg>(httpResponse.Content);
                return smsVerifityMsg.is_valid;
            }
        }

        ///编码
        static string EncodeBase64(string code_type, string code)
        {
            string encode = "";
            byte[] bytes = Encoding.GetEncoding(code_type).GetBytes(code);
            try
            {
                encode = Convert.ToBase64String(bytes);
            }
            catch
            {
                encode = code;
            }
            return encode;
        }
    }

    public class SmsMsg
    {
        public string msg_id { get; set; }
    }

    public class SmsVerifityMsg
    {
        public bool is_valid { get; set; }
    }
}
