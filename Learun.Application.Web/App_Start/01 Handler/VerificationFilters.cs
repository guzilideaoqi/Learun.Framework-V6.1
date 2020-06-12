using Learun.Cache.Redis;
using Learun.Util;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Learun.Application.Web.App_Start._01_Handler
{
    public class VerificationAPIAttribute : AuthorizeAttribute
    {
        private FilterMode _customMode;
        /// <summary>默认构造</summary>
        /// <param name="Mode">认证模式</param>
        public VerificationAPIAttribute(FilterMode Mode)
        {
            _customMode = Mode;
        }

        /// <summary>
        /// 响应前执行登录验证,查看当前用户是否有效 
        /// </summary>
        /// <param name="filterContext"></param>
        public override void OnAuthorization(AuthorizationContext filterContext)
        {
            //登录拦截是否忽略
            if (_customMode == FilterMode.Ignore)
            {
                return;
            }
            string ActionName = filterContext.RouteData.Values["action"].ToString().ToLower();
            //登录和注册不校验
            if (ActionName == "dm_login" || ActionName == "dm_register" || ActionName == "paycallback" || ActionName == "authorcallback" || ActionName == "callback"||ActionName== "authorresult"||ActionName== "getgoodtypebycache")
            {
                return;
            }
            //return Content(new ResParameter { code = ResponseCode.success, info = info, data = new object { } }.ToJson());
            ResParameter modelResult = new ResParameter();
            //参数判断
            if (filterContext.HttpContext.Request.Headers["appid"] == null)
            {
                modelResult.code = ResponseCode.fail;
                modelResult.info = "缺少appid参数!";
                filterContext.Result = new ContentResult { Content = modelResult.ToJson() };
                return;
            }
            else if (filterContext.HttpContext.Request.Headers["token"] == null)
            {
                modelResult.code = ResponseCode.fail;
                modelResult.info = "缺少token参数!";
                filterContext.Result = new ContentResult { Content = modelResult.ToJson() };
                return;
            }
            else if (filterContext.HttpContext.Request.Headers["timestamp"] == null)
            {
                modelResult.code = ResponseCode.fail;
                modelResult.info = "缺少timestamp参数!";
                filterContext.Result = new ContentResult { Content = modelResult.ToJson() };
                return;
            }
        }
    }
}