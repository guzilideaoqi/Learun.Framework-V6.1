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
using Learun.Application.TwoDevelopment.Common;
using Learun.Application.TwoDevelopment.DM_APPManage;

namespace Learun.Application.Web.App_Start._01_Handler
{
    public class VerificationAPIAttribute : AuthorizeAttribute
    {
        DM_UserIBLL dM_UserIBLL = new DM_UserBLL();
        string[] actionNameList = new string[] {
            "dm_login",
            "getarticledetail",
            "decodeinvitecode",
            "dm_register",
            "paycallback","testpaycallback",
            "authorcallback",
            "callback",
            "authorresult",
            "getgoodtype",
            "getsubgoodtype",
            "getgoodtypebycache",
            "gettop100",
            "getrankinglist",
            "getsuperserachgood",
            "getdtksearchgood",
            "getrecommendgoodbytb",
            "getopgood",
            "searchsuggestion",
            "getactivitycatalogue",
            "getactivitygoodlist",
            "gettopiccatalogue",
            "gettopicgoodlist",
            "gettbtopiclist",
            "get_jd_goodlist",
            "get_jd_searchgoodlist",
            "get_pdd_catlist",
            "get_pdd_goodlist",
            "getrecommendgoodbypdd",
            "getgoodimagedetail",
            "getcommongooddetail",
            "getgooddetailbytb",
            "getbannerlist",
            "getintergralchangegood",
            "getgoodsmallcate",
            "getverification",
            "resetpwd",
            "getplaformsetting",
            "excutesubcommission","gettasktype","gettasklist","logintokenverify","getcommonsetting","quickregister","dm_loginbyphone","getversionrecord" };
        string[] nosign = new string[] { "getplaformsetting", "excutesubcommission" };
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
            /*签名生成格式*/
            /*md5(md5(appidplatform=androidtimestamp=1611907265000version=1.2.0appid)+"174PYR5Wwtce")  最后转为小写  参数放在header里面  参数名sign*/
            string ActionName = filterContext.RouteData.Values["action"].ToString().ToLower();
            //登录拦截是否忽略
            if (_customMode == FilterMode.Ignore || nosign.Contains(ActionName))
            {
                return;
            }

            var attrNeeds = filterContext.ActionDescriptor.GetCustomAttributes(typeof(NoNeedLoginAttribute), false);
            if (!attrNeeds.IsEmpty() && attrNeeds.Count() > 0)
            {
                //NoNeedLoginAttribute needPass = attrNeeds[0] as NoNeedLoginAttribute;
                return;
            }

            string token = filterContext.HttpContext.Request.Headers["token"];//用户登录token
            string platform = filterContext.HttpContext.Request.Headers["platform"];//平台类型
            string appid = filterContext.HttpContext.Request.Headers["appid"];//appid
            string timestamp = filterContext.HttpContext.Request.Headers["timestamp"];//时间戳
            string version = filterContext.HttpContext.Request.Headers["version"];//版本号
            string sign = filterContext.HttpContext.Request.Headers["sign"];//请求签名
            DateTime currentTime = DateTime.Now;
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

            int version_num = int.Parse(version.Replace(".", ""));
            if ((platform == "ios" && version_num > 103) || (platform == "android" && version_num > 119))
            {
                if (sign != "dlm_hyg")
                {
                    #region 校验签名
                    string signContent = string.Format("{0}platform={1}timestamp={2}version={3}{0}", appid, platform, timestamp, version);
                    string check_sign = Md5Helper.Encrypt(Md5Helper.Encrypt(signContent, 32) + "174PYR5Wwtce", 32).ToLower();
                    if (sign != check_sign)
                    {
                        modelResult.code = ResponseCode.fail;
                        modelResult.info = "签名校验失败!";
                        filterContext.Result = new ContentResult { Content = modelResult.ToJson() };
                        return;
                    }
                    #endregion

                    #region 校验时间戳区间
                    long timestamp_long = long.Parse(timestamp);
                    long startTime = long.Parse(Time.GetTimeStamp(currentTime.AddMinutes(-2), true));
                    long endTime = long.Parse(Time.GetTimeStamp(currentTime.AddMinutes(2), true));
                    if (startTime > timestamp_long || timestamp_long > endTime)
                    {
                        modelResult.code = ResponseCode.fail;
                        modelResult.info = "本地时间和网络时间存在较大差别,请调整后重新使用APP!";
                        filterContext.Result = new ContentResult { Content = modelResult.ToJson() };
                        return;
                    }
                    #endregion

                    #region 校验请求频率
                    string ip = Net.Ip;
                    IP_Limit iP_Limit = CommonConfig.iP_Limits.Where(t => t.IP == ip).FirstOrDefault();//获取IP限制记录
                    if (!iP_Limit.IsEmpty())
                    {
                        double diffTime = (currentTime - iP_Limit.RequestTime).TotalSeconds;//该IP请求时间间隔在1分钟以内  增加接口请求的数量   不在1分钟以内的重新记录IP请求
                        if (diffTime < 60)
                        {
                            if (iP_Limit.RequestCount > 100)
                            {
                                modelResult.code = ResponseCode.fail;
                                modelResult.info = "操作过于频繁，请稍后重试!";
                                filterContext.Result = new ContentResult { Content = modelResult.ToJson() };
                                return;
                            }
                            iP_Limit.RequestCount += 1;
                        }
                        else
                        {
                            iP_Limit.RequestTime = currentTime;
                            iP_Limit.RequestCount = 1;
                        }
                    }
                    else
                    {
                        CommonConfig.iP_Limits.Add(new IP_Limit
                        {
                            IP = ip,
                            RequestCount = 1,
                            RequestTime = currentTime
                        });
                    }
                    CommonConfig.iP_Limits.RemoveAll(t => t.RequestTime < currentTime.AddMinutes(-2));//清空超过2分钟的请求记录
                    #endregion
                }
            }


            #region 屏蔽不需要校验登录的接口
            if (actionNameList.Contains(ActionName))
            {
                return;
            }
            #endregion

            #region 校验当前用户是否在线
            if (token.IsEmpty())
            {
                modelResult.code = ResponseCode.NoLogin;
                modelResult.info = "请登录后操作!";
                filterContext.Result = new ContentResult { Content = modelResult.ToJson() };
                return;
            }
            else
            {
                dm_userEntity dm_UserEntity = CacheHelper.ReadUserInfo(filterContext.HttpContext.Request.Headers);
                if (dm_UserEntity.IsEmpty())
                {

                    dm_UserEntity = dM_UserIBLL.GetUserInfoByToken(token);
                    if (!dm_UserEntity.IsEmpty())
                    {
                        CacheHelper.UpdateUserInfo(dm_UserEntity);
                    }
                    else
                    {
                        string header = string.Format("ActionName={0}&token={1}&platform={2}", ActionName, token, platform);
                        Hyg.Common.OtherTools.LogHelper.WriteDebugLog("测试token", header);

                        modelResult.code = ResponseCode.LoginExpire;
                        modelResult.info = "您的账号在另一台设备登录。如非本人操作，请注意账户安全!";
                        //modelResult.info = "亲，离开太久了,重新登录一下吧!";
                        filterContext.Result = new ContentResult { Content = modelResult.ToJson() };
                        return;
                    }
                }
            }
            #endregion
        }
    }
}