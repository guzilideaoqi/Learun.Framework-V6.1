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
using System.ComponentModel;

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
                SmsVerifityMsg smsVerifityMsg = JsonConvert.JsonDeserialize<SmsVerifityMsg>(httpResponse.Content);
                throw new Exception(GetErrorMessage(smsVerifityMsg.error));
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
                SmsVerifityMsg smsVerifityMsg = JsonConvert.JsonDeserialize<SmsVerifityMsg>(httpResponse.Content);
                if (!smsVerifityMsg.is_valid)
                    throw new Exception(GetErrorMessage(smsVerifityMsg.error));
                return smsVerifityMsg.is_valid;
                //throw new Exception("短信接口验证错误" + httpResponse.Content);
                //return false;
            }
            else
            {
                SmsVerifityMsg smsVerifityMsg = JsonConvert.JsonDeserialize<SmsVerifityMsg>(httpResponse.Content);
                if (!smsVerifityMsg.is_valid)
                    throw new Exception(GetErrorMessage(smsVerifityMsg.error));
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

        static string GetErrorMessage(SmsMessageContent smsMessageContent)
        {
            string msg = "短信接口异常,请退出应用重新操作!";
            switch (smsMessageContent.code)
            {
                case SmsMessageErrorCode.Success:
                    break;
                case SmsMessageErrorCode.MissingAuth:
                    msg = "auth为空";
                    break;
                case SmsMessageErrorCode.AuthFailed:
                    msg = "auth鉴权失败";
                    break;
                case SmsMessageErrorCode.MissingBody:
                    msg = "body为空";
                    break;
                case SmsMessageErrorCode.MissingMobile:
                    msg = "手机号码为空";
                    break;
                case SmsMessageErrorCode.MissingTempID:
                    msg = "模版ID为空";
                    break;
                case SmsMessageErrorCode.InvalidMobile:
                    msg = "手机号码无效";
                    break;
                case SmsMessageErrorCode.InvalidBody:
                    msg = "body无效";
                    break;
                case SmsMessageErrorCode.NoSmsCodeAuth:
                    msg = "未开通短信业务";
                    break;
                case SmsMessageErrorCode.OutOfFreq:
                    msg = "发送超频";
                    break;
                case SmsMessageErrorCode.InvalidCode:
                    msg = "验证码无效";
                    break;
                case SmsMessageErrorCode.ExpiredCode:
                    msg = "验证码过期";
                    break;
                case SmsMessageErrorCode.VerifiedCode:
                    msg = "验证码已验证";
                    break;
                case SmsMessageErrorCode.InvalidTempID:
                    msg = "模版ID无效";
                    break;
                case SmsMessageErrorCode.NoMoney:
                    msg = "可发短信余量不足";
                    break;
                case SmsMessageErrorCode.MissingCode:
                    msg = "验证码为空";
                    break;
                case SmsMessageErrorCode.ApiNotFound:
                    msg = "API不存在";
                    break;
                case SmsMessageErrorCode.MediaNotSupported:
                    msg = "媒体类型不支持";
                    break;
                case SmsMessageErrorCode.RequestMethodNotSupport:
                    msg = "请求方法不支持";
                    break;
                case SmsMessageErrorCode.ServerError:
                    msg = "服务端异常";
                    break;
                case SmsMessageErrorCode.TemplateAuditing:
                    msg = "模板审核中";
                    break;
                case SmsMessageErrorCode.TemplateNotPass:
                    msg = "模板审核未通过";
                    break;
                case SmsMessageErrorCode.ParametersNotAllReplaced:
                    msg = "模板中参数未全部替换";
                    break;
                case SmsMessageErrorCode.ParametersIsEmpty:
                    msg = "参数为空";
                    break;
                case SmsMessageErrorCode.UnsubscribedMobile:
                    msg = "手机号码已退订";
                    break;
                case SmsMessageErrorCode.WrongTemplateType:
                    msg = "该API不支持此模版类型";
                    break;
                case SmsMessageErrorCode.WrongMsgID:
                    msg = "Msg_id无效";
                    break;
                case SmsMessageErrorCode.InvalidSendTime:
                    msg = "发送时间为空或在当前时间之前";
                    break;
                case SmsMessageErrorCode.InvalidScheduleID:
                    msg = "schedule_id无效";
                    break;
                case SmsMessageErrorCode.InvalidScheduleStatus:
                    msg = "定时短信已发送或已删除，无法再修改";
                    break;
                case SmsMessageErrorCode.RecipientsIsEmpty:
                    msg = "recipients为空";
                    break;
                case SmsMessageErrorCode.TooMuchRecipients:
                    msg = "recipients短信接收者数量超过1000";
                    break;
                case SmsMessageErrorCode.RepeatSend:
                    msg = "重复发送";
                    break;
                case SmsMessageErrorCode.IllegalIP:
                    msg = "请求IP不合法";
                    break;
                case SmsMessageErrorCode.AppInBlack:
                    msg = "应用被列为黑名单";
                    break;
                case SmsMessageErrorCode.HasBlackWord:
                    msg = "短信内容存在敏感词汇";
                    break;
                case SmsMessageErrorCode.InvalidContentLength:
                    msg = "短信内容长度错误，文本短信长度不超过350个字，语音短信验证码长度4～8数字";
                    break;
                case SmsMessageErrorCode.InvalidCodeType:
                    msg = "语音验证码内容错误，验证码仅支持数字";
                    break;
                case SmsMessageErrorCode.InvalidVoiceLanguageType:
                    msg = "语音验证码播报语言类型错误";
                    break;
                case SmsMessageErrorCode.InvalidTtlValue:
                    msg = "验证码有效期错误";
                    break;
                case SmsMessageErrorCode.TemplateIsEmpty:
                    msg = "模板内容为空";
                    break;
                case SmsMessageErrorCode.TemplateTooLong:
                    msg = "模板内容过长，含签名长度限制为350字符";
                    break;
                case SmsMessageErrorCode.TemplateParameterInvalid:
                    msg = "模板参数无效";
                    break;
                case SmsMessageErrorCode.RemarkTooLong:
                    msg = "备注内容过长，长度限制为500字符";
                    break;
                case SmsMessageErrorCode.SignatureNotSet:
                    msg = "该应用未设置签名，请先设置签名";
                    break;
                case SmsMessageErrorCode.ModifyTemplateNotAllow:
                    msg = "该模版不支持修改，仅状态为审核不通过的模板支持修改";
                    break;
                case SmsMessageErrorCode.TemplateContainsSpecialSymbol:
                    msg = "模板不能含有特殊符号";
                    break;
                case SmsMessageErrorCode.SpecialTemplateParameterNeedExtraRemarkForConfirmation:
                    msg = "模板中存在链接变量，请在 remark 参数中填写链接以报备，避免短信发送时因进入人工审核而导致发送延迟";
                    break;
                case SmsMessageErrorCode.ContentContainsSpecialSymbol:
                    msg = "短信正文不能含有特殊符号";
                    break;
                case SmsMessageErrorCode.InvalidImage:
                    msg = "图片不合法";
                    break;
                case SmsMessageErrorCode.InvalidSignID:
                    msg = "签名ID不合法";
                    break;
                case SmsMessageErrorCode.OtherSignaturesInTheAudit:
                    msg = "已经存在其他待审核的签名，不能提交";
                    break;
                case SmsMessageErrorCode.InvalidSignature:
                    msg = "签名内容不合法";
                    break;
                case SmsMessageErrorCode.DefaultSignatureCannotBeDeleted:
                    msg = "默认签名不能删除";
                    break;
                case SmsMessageErrorCode.PullOutOfFrequency:
                    msg = "超频，API调用频率：单个appKey5秒/次";
                    break;
                case SmsMessageErrorCode.PullNotAllow:
                    msg = "禁止拉取，建议：请排查当前是否正在使用回调的形式获取数据";
                    break;
                case SmsMessageErrorCode.InvalidAccount:
                    msg = "短信开发者账号冻结，请联系技术支持";
                    break;
                default:
                    break;
            }

            return msg;
        }
    }

    public class SmsMsg
    {
        public string msg_id { get; set; }
    }

    public class SmsVerifityMsg
    {
        public bool is_valid { get; set; }

        public SmsMessageContent error { get; set; }
    }

    public class SmsMessageContent
    {
        public SmsMessageErrorCode code { get; set; }

        public string message { get; set; }
    }

    public enum SmsMessageErrorCode
    {
        /// <summary>
        /// 请求成功
        /// </summary>
        [Description("请求成功")]
        Success = 50000,
        /// <summary>
        /// auth 为空
        /// </summary>
        [Description("auth为空")]
        MissingAuth = 50001,
        /// <summary>
        /// auth 鉴权失败
        /// </summary>
        [Description("auth鉴权失败")]
        AuthFailed = 50002,
        /// <summary>
        /// body 为空
        /// </summary>
        [Description("body为空")]
        MissingBody = 50003,
        /// <summary>
        /// 手机号码为空
        /// </summary>
        [Description("手机号码为空")]
        MissingMobile = 50004,
        /// <summary>
        /// 模版ID 为空
        /// </summary>
        [Description("模版ID为空")]
        MissingTempID = 50005,
        /// <summary>
        /// 手机号码无效
        /// </summary>
        [Description("手机号码无效")]
        InvalidMobile = 50006,
        /// <summary>
        /// body 无效
        /// </summary>
        [Description("body无效")]
        InvalidBody = 50007,
        /// <summary>
        /// 未开通短信业务
        /// </summary>
        [Description("未开通短信业务")]
        NoSmsCodeAuth = 50008,
        /// <summary>
        /// 发送超频
        /// </summary>
        [Description("发送超频")]
        OutOfFreq = 50009,
        /// <summary>
        /// 验证码无效
        /// </summary>
        [Description("验证码无效")]
        InvalidCode = 50010,
        /// <summary>
        /// 验证码过期
        /// </summary>
        [Description("验证码过期")]
        ExpiredCode = 50011,
        /// <summary>
        /// 验证码已验证
        /// </summary>
        [Description("验证码已验证")]
        VerifiedCode = 50012,
        /// <summary>
        /// 模版ID无效
        /// </summary>
        [Description("模版ID无效")]
        InvalidTempID = 50013,
        /// <summary>
        /// 可发短信余量不足
        /// </summary>
        [Description("可发短信余量不足")]
        NoMoney = 50014,
        /// <summary>
        /// 验证码为空
        /// </summary>
        [Description("验证码为空")]
        MissingCode = 50015,
        /// <summary>
        /// API 不存在
        /// </summary>
        [Description("API不存在")]
        ApiNotFound = 50016,
        /// <summary>
        /// 媒体类型不支持
        /// </summary>
        [Description("媒体类型不支持")]
        MediaNotSupported = 50017,
        /// <summary>
        /// 请求方法不支持
        /// </summary>
        [Description("请求方法不支持")]
        RequestMethodNotSupport = 50018,
        /// <summary>
        /// 服务端异常
        /// </summary>
        [Description("服务端异常")]
        ServerError = 50019,
        /// <summary>
        /// 模板审核中
        /// </summary>
        [Description("模板审核中")]
        TemplateAuditing = 50020,
        /// <summary>
        /// 模板审核未通过
        /// </summary>
        [Description("模板审核未通过")]
        TemplateNotPass = 50021,
        /// <summary>
        /// 模板中参数未全部替换
        /// </summary>
        [Description("模板中参数未全部替换")]
        ParametersNotAllReplaced = 50022,
        /// <summary>
        /// 参数为空
        /// </summary>
        [Description("参数为空")]
        ParametersIsEmpty = 50023,
        /// <summary>
        /// 手机号码已退订
        /// </summary>
        [Description("手机号码已退订")]
        UnsubscribedMobile = 50024,
        /// <summary>
        /// 该 API 不支持此模版类型
        /// </summary>
        [Description("该API不支持此模版类型")]
        WrongTemplateType = 50025,
        /// <summary>
        /// Msg_id无效
        /// </summary>
        [Description("Msg_id无效")]
        WrongMsgID = 50026,
        /// <summary>
        /// 发送时间为空或在当前时间之前
        /// </summary>
        [Description("发送时间为空或在当前时间之前")]
        InvalidSendTime = 50027,
        /// <summary>
        /// schedule_id 无效
        /// </summary>
        [Description("schedule_id无效")]
        InvalidScheduleID = 50028,
        /// <summary>
        /// 定时短信已发送或已删除，无法再修改
        /// </summary>
        [Description("定时短信已发送或已删除，无法再修改")]
        InvalidScheduleStatus = 50029,
        /// <summary>
        /// recipients 为空
        /// </summary>
        [Description("recipients为空")]
        RecipientsIsEmpty = 50030,
        /// <summary>
        /// recipients短信接收者数量超过1000
        /// </summary>
        [Description("recipients短信接收者数量超过1000")]
        TooMuchRecipients = 50031,
        /// <summary>
        /// 重复发送
        /// </summary>
        [Description("重复发送")]
        RepeatSend = 50034,
        /// <summary>
        /// 请求IP不合法
        /// </summary>
        [Description("请求IP不合法")]
        IllegalIP = 50035,
        /// <summary>
        /// 应用被列为黑名单
        /// </summary>
        [Description("应用被列为黑名单")]
        AppInBlack = 50036,
        /// <summary>
        /// 短信内容存在敏感词汇
        /// </summary>
        [Description("短信内容存在敏感词汇")]
        HasBlackWord = 50037,
        /// <summary>
        /// 短信内容长度错误，文本短信长度不超过350个字，语音短信验证码长度4～8数字
        /// </summary>
        [Description("短信内容长度错误，文本短信长度不超过350个字，语音短信验证码长度4～8数字")]
        InvalidContentLength = 50038,
        /// <summary>
        /// 语音验证码内容错误，验证码仅支持数字
        /// </summary>
        [Description("语音验证码内容错误，验证码仅支持数字")]
        InvalidCodeType = 50039,
        /// <summary>
        /// 语音验证码播报语言类型错误
        /// </summary>
        [Description("语音验证码播报语言类型错误")]
        InvalidVoiceLanguageType = 50040,
        /// <summary>
        /// 验证码有效期错误
        /// </summary>
        [Description("验证码有效期错误")]
        InvalidTtlValue = 50041,
        /// <summary>
        /// 模板内容为空
        /// </summary>
        [Description("模板内容为空")]
        TemplateIsEmpty = 50042,
        /// <summary>
        /// 模板内容过长，含签名长度限制为350字符
        /// </summary>
        [Description("模板内容过长，含签名长度限制为350字符")]
        TemplateTooLong = 50043,
        /// <summary>
        /// 模板参数无效
        /// </summary>
        [Description("模板参数无效")]
        TemplateParameterInvalid = 50044,
        /// <summary>
        /// 备注内容过长，长度限制为500字符
        /// </summary>
        [Description("备注内容过长，长度限制为500字符")]
        RemarkTooLong = 50045,
        /// <summary>
        /// 该应用未设置签名，请先设置签名
        /// </summary>
        [Description("该应用未设置签名，请先设置签名")]
        SignatureNotSet = 50046,
        /// <summary>
        /// 该模版不支持修改，仅状态为审核不通过的模板支持修改
        /// </summary>
        [Description("该模版不支持修改，仅状态为审核不通过的模板支持修改")]
        ModifyTemplateNotAllow = 50047,
        /// <summary>
        /// 模板不能含有特殊符号，如【】
        /// </summary>
        [Description("模板不能含有特殊符号")]
        TemplateContainsSpecialSymbol = 50052,
        /// <summary>
        /// 模板中存在链接变量，请在 remark 参数中填写链接以报备，避免短信发送时因进入人工审核而导致发送延迟
        /// </summary>
        [Description("模板中存在链接变量，请在 remark 参数中填写链接以报备，避免短信发送时因进入人工审核而导致发送延迟")]
        SpecialTemplateParameterNeedExtraRemarkForConfirmation = 50053,
        /// <summary>
        /// 短信正文不能含有特殊符号，如【】
        /// </summary>
        [Description("短信正文不能含有特殊符号")]
        ContentContainsSpecialSymbol = 50054,
        /// <summary>
        /// 图片不合法
        /// </summary>
        [Description("图片不合法")]
        InvalidImage = 50101,
        /// <summary>
        /// 签名ID不合法
        /// </summary>
        [Description("签名ID不合法")]
        InvalidSignID = 50102,
        /// <summary>
        /// 已经存在其他待审核的签名，不能提交
        /// </summary>
        [Description("已经存在其他待审核的签名，不能提交")]
        OtherSignaturesInTheAudit = 50103,
        /// <summary>
        /// 签名内容不合法
        /// </summary>
        [Description("签名内容不合法")]
        InvalidSignature = 50104,
        /// <summary>
        /// 默认签名不能删除
        /// </summary>
        [Description("默认签名不能删除")]
        DefaultSignatureCannotBeDeleted = 50105,
        /// <summary>
        /// 超频，API 调用频率：单个 appKey 5 秒/次
        /// </summary>
        [Description("超频，API调用频率：单个appKey5秒/次")]
        PullOutOfFrequency = 50201,
        /// <summary>
        /// 禁止拉取，建议：请排查当前是否正在使用回调的形式获取数据
        /// </summary>
        [Description("禁止拉取，建议：请排查当前是否正在使用回调的形式获取数据")]
        PullNotAllow = 50202,
        /// <summary>
        /// 短信开发者账号冻结，请联系技术支持
        /// </summary>
        [Description("短信开发者账号冻结，请联系技术支持")]
        InvalidAccount = 50301
    }
}
