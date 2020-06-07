using Learun.Loger;
using Learun.Util;
using Learun.Util.Operat;
using System;
using System.Collections.Generic;
using System.Linq;
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

        protected virtual ActionResult FailNoPrice(string info)
        {
            return Content(new ResParameter { code = ResponseCode.NoMoney, info = info }.ToJson());
        }

        protected virtual ActionResult FailException(Exception ex)
        {
            if (ex.InnerException.IsEmpty())
            {
                if (ex.Message.Contains("账户余额不足"))
                    return FailNoPrice(ex.Message);
                else
                    return Fail(ex.Message);
            }
            else
                return Fail(ex.InnerException.Message);
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