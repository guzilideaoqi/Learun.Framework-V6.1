using Learun.Cache.Base;
using Learun.Cache.Factory;
using Learun.Cache.Redis;
using Learun.Util;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Text;


namespace Learun.Application.Web.App_Start._01_Handler
{
    public class VerificationAPIAttribute : AuthorizeAttribute
    {
        string[] actionNameList = new string[] { "dm_login", "dm_register", "paycallback", "authorcallback", "callback", "authorresult", "getgoodtypebycache", "getrankinglist", "gettodaygood", "commonsearchgood", "getsuperserachgood", "getdtksearchgood", "getrecommendgoodbytb", "getopgood", "getactivitygoodlist", "gettopiccatalogue", "gettopicgoodlist", "gettbtopiclist", "get_jd_goodlist", "get_jd_searchgoodlist", "get_pdd_goodlist", "getrecommendgoodbypdd", "getgoodimagedetail", "getgooddetailbytb", "getbannerlist", "getgoodsmallcate", "getpersoninfo", "getverification", "resetpwd" };
        private ICache redisCache = CacheFactory.CaChe();
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
            string token = filterContext.HttpContext.Request.Headers["token"];
            string platform = filterContext.HttpContext.Request.Headers["platform"];

            //登录和注册不校验
            if (actionNameList.Contains(ActionName))
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
            else if (filterContext.HttpContext.Request.Headers["timestamp"] == null)
            {
                modelResult.code = ResponseCode.fail;
                modelResult.info = "缺少timestamp参数!";
                filterContext.Result = new ContentResult { Content = modelResult.ToJson() };
                return;
            }
            else if (filterContext.HttpContext.Request.Headers["version"] == null)
            {
                modelResult.code = ResponseCode.fail;
                modelResult.info = "缺少version参数!";
                filterContext.Result = new ContentResult { Content = modelResult.ToJson() };
                return;
            }
            else if (platform == null)
            {
                modelResult.code = ResponseCode.fail;
                modelResult.info = "缺少platform参数!";
                filterContext.Result = new ContentResult { Content = modelResult.ToJson() };
                return;
            }

            #region 校验当前用户是否在线
            string user_id = "";
            if (filterContext.HttpContext.Request.Params.AllKeys.Contains("User_ID"))
            {
                user_id = filterContext.HttpContext.Request.Params["User_ID"];
            }
            if (filterContext.HttpContext.Request.Params.AllKeys.Contains("user_id"))
            {
                user_id = filterContext.HttpContext.Request.Params["user_id"];
            }

            if (!user_id.IsEmpty() && user_id != "0")// && platform == "android"
            {
                if (token.IsEmpty())
                {
                    modelResult.code = ResponseCode.NoLogin;
                    modelResult.info = "请登录后操作!";
                    filterContext.Result = new ContentResult { Content = modelResult.ToJson() };
                    return;
                }
                else
                {
                    string cacheKey = Md5Helper.Hash("SingleUserList" + user_id);
                    string exist_token = redisCache.Read<string>(cacheKey, 7);
                    if (exist_token != token)
                    {
                        modelResult.code = ResponseCode.LoginExpire;
                        modelResult.info = "您的账号在另一台设备登录。如非本人操作，请注意账户安全!";
                        filterContext.Result = new ContentResult { Content = modelResult.ToJson() };
                        return;
                    }
                }
            }
            #endregion
        }
    }
}