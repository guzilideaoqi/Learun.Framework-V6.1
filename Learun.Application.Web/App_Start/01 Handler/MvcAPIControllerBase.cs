using Learun.Application.Base.SystemModule.Log;
using Learun.Application.TwoDevelopment.Common;
using Learun.Loger;
using Learun.Util;
using Learun.Util.Operat;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Web;
using System.Web.Mvc;

namespace Learun.Application.Web.App_Start._01_Handler
{
    [VerificationAPI(FilterMode.Enforce)]
    public class MvcAPIControllerBase : Controller
    {
        #region 日志操作
        /// <summary>
        /// 日志对象实体
        /// </summary>
        private Log _logger;
        /// <summary>
        /// 日志操作
        /// </summary>
        public Log Logger
        {
            get { return _logger ?? (_logger = LogFactory.GetLogger(this.GetType().ToString())); }
        }
        #endregion

        #region 请求响应
        /// <summary>
        /// 返回成功消息
        /// </summary>
        /// <param name="data">数据</param>
        /// <returns></returns>
        protected virtual ActionResult ToJsonResult(object data)
        {
            return Content(data.ToJson());
        }
        /// <summary>
        /// 返回成功消息
        /// </summary>
        /// <param name="info">消息</param>
        /// <returns></returns>
        protected virtual ActionResult Success(string info)
        {
            return Content(new ResParameter { code = ResponseCode.success, info = info, data = new object { } }.ToJson());
        }
        /// <summary>
        /// 返回成功消息
        /// </summary>
        /// <param name="data">数据</param>
        /// <returns></returns>
        protected virtual ActionResult SuccessString(string data)
        {
            return Content(new ResParameter { code = ResponseCode.success, info = "响应成功", data = data }.ToJson());
        }
        /// <summary>
        /// 返回成功数据
        /// </summary>
        /// <param name="data">数据</param>
        /// <returns></returns>
        protected virtual ActionResult Success(object data)
        {
            return Content(new ResParameter { code = ResponseCode.success, info = "响应成功", data = data }.ToJson());
        }
        /// <summary>
        /// 返回成功消息
        /// </summary>
        /// <param name="info">消息</param>
        /// <param name="data">数据</param>
        /// <returns></returns>
        protected virtual ActionResult Success(string info, object data)
        {
            if (data == null)
                data = new { };
            return Content(new ResParameter { code = ResponseCode.success, info = info, data = data }.ToJson());
        }

        /// <summary>
        /// 带操作日志
        /// </summary>
        /// <param name="info"></param>
        /// <returns></returns>
        protected virtual ActionResult Success(string info, string title, OperationType type, string keyValue, string content)
        {
            OperateLogModel operateLogModel = new OperateLogModel();
            operateLogModel.title = title;
            operateLogModel.type = type;
            operateLogModel.url = (string)WebHelper.GetHttpItems("currentUrl");
            operateLogModel.sourceObjectId = keyValue;
            operateLogModel.sourceContentJson = content;

            OperatorHelper.Instance.WriteOperateLog(operateLogModel);

            return Content(new ResParameter { code = ResponseCode.success, info = info, data = new object { } }.ToJson());
        }

        protected virtual ActionResult SuccessList(string info, object data)
        {
            return Content(new ResParameter
            {
                code = ResponseCode.success,
                info = info,
                data = new
                {
                    list = data
                }
            }.ToJson());
        }

        protected virtual ActionResult SuccessList(string info, object data, object Extend)
        {
            return Content(new ResParameter
            {
                code = ResponseCode.success,
                info = info,
                data = new
                {
                    list = data,
                    Extend = Extend
                }
            }.ToJson());
        }

        protected virtual ActionResult NoRelationID(string info)
        {
            return Content(new ResParameter
            {
                code = ResponseCode.NoRelation,
                info = info
            }.ToJson());
        }

        /// <summary>
        /// 返回失败消息
        /// </summary>
        /// <param name="info">消息</param>
        /// <returns></returns>
        protected virtual ActionResult Fail(string info)
        {
            return Content(new ResParameter { code = ResponseCode.fail, info = info }.ToJson());
        }

        protected virtual ActionResult FailNoLogin()
        {
            return Content(new ResParameter { code = ResponseCode.NoLogin, info = "用户未登录!" }.ToJson());
        }

        protected virtual ActionResult FailNoExistUser(object data)
        {
            return Content(new ResParameter { code = ResponseCode.NoExistUser, info = "用户信息不存在，请输入上级邀请码完成注册!", data = data }.ToJson());
        }

        protected virtual ActionResult FailNoPrice(string info)
        {
            return Content(new ResParameter { code = ResponseCode.NoMoney, info = info }.ToJson());
        }

        protected virtual ActionResult FailNoBindAliPay(string info)
        {
            return Content(new ResParameter { code = ResponseCode.NoBindAliPay, info = info }.ToJson());
        }

        protected virtual ActionResult FailNoRealName(string info)
        {
            return Content(new ResParameter { code = ResponseCode.NoRealName, info = info }.ToJson());
        }

        protected virtual ActionResult FailException(Exception ex)
        {
            #region 写入日志
            StackTrace trace = new StackTrace();
            StackFrame frame = trace.GetFrame(1);//1代表上级，2代表上上级，以此类推
            MethodBase method = frame.GetMethod();
            String className = method.ReflectedType.Name;

            Exception Error = ex;
            LogMessage logMessage = new LogMessage();
            logMessage.OperationTime = DateTime.Now;
            logMessage.Url = className + "/" + method.Name;
            logMessage.Class = className;
            logMessage.Ip = Net.Ip;
            logMessage.Host = Net.Host;
            logMessage.Browser = Net.Browser;
            logMessage.RequestParam = GetParamInfo(base.Request.Params);

            if (Error.InnerException == null)
            {
                logMessage.ExceptionInfo = Error.Message;
            }
            else
            {
                logMessage.ExceptionInfo = Error.InnerException.Message;
            }
            logMessage.ExceptionSource = Error.Source;
            logMessage.ExceptionRemark = Error.StackTrace;
            logMessage.UserName = GetOperateUserName(base.Request.Headers);
            string strMessage = new LogFormat().ExceptionFormat(logMessage);

            LogEntity logEntity = new LogEntity();
            logEntity.F_CategoryId = 5;
            logEntity.F_OperateTypeId = ((int)OperationType.Exception).ToString();
            logEntity.F_OperateType = EnumAttribute.GetDescription(OperationType.Exception);
            logEntity.F_OperateAccount = "调用接口";
            logEntity.F_OperateUserId = "api";
            logEntity.F_ExecuteResult = -1;
            logEntity.F_ExecuteResultJson = strMessage;
            logEntity.WriteLog();
            #endregion

            if (ex.InnerException.IsEmpty())
            {
                if (ex.Message.Contains("账户余额不足"))
                    return FailNoPrice(ex.Message);
                else if (ex.Message.Contains("您的账号未实名"))
                {
                    return FailNoRealName(ex.Message);
                }
                else if (ex.Message.Contains("支付宝账号未绑定"))
                {
                    return FailNoBindAliPay(ex.Message);
                }
                else
                    return Fail(ex.Message);
            }
            else
            {
                if (ex.InnerException.Message.Contains("账户余额不足"))
                    return FailNoPrice(ex.InnerException.Message);
                else if (ex.InnerException.Message.Contains("您的账号未实名"))
                {
                    return FailNoRealName(ex.InnerException.Message);
                }
                else if (ex.InnerException.Message.Contains("支付宝账号未绑定"))
                {
                    return FailNoBindAliPay(ex.InnerException.Message);
                }
                else
                    return Fail(ex.InnerException.Message);
            }
        }

        string GetParamInfo(NameValueCollection nameValueCollection)
        {
            string param = "";
            foreach (string item in nameValueCollection.Keys)
            {
                if (!CommonConfig.NoRecordRequestKeyWord.Contains(item))
                {
                    if (!param.IsEmpty())
                        param += "&";
                    param += (item + "=" + nameValueCollection[item]);
                }
            }
            return param;
        }

        string GetOperateUserName(NameValueCollection header)
        {
            string username = "";
            if (!header["platform"].IsEmpty())
            {
                username += header["platform"].ToString();
            }

            if (!header["version"].IsEmpty())
            {
                username += ("(" + header["version"].ToString() + ")");
            }
            return username;
        }

        /// <summary>
        /// 返回失败消息
        /// </summary>
        /// <param name="info">消息</param>
        /// <param name="data">消息</param>
        /// <returns></returns>
        protected virtual ActionResult Fail(string info, object data)
        {
            return Content(new ResParameter { code = ResponseCode.fail, info = info, data = data }.ToJson());
        }
        #endregion
    }
}