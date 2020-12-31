using Learun.Application.TwoDevelopment.Common;
using Learun.Application.TwoDevelopment.DM_APPManage;
using Learun.Application.TwoDevelopment.Hyg_RobotModule;
using Learun.Application.Web.App_Start._01_Handler;
using Learun.Loger;
using Learun.Util;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Mvc;
using Top.Api;
using Top.Api.Request;
using Top.Api.Response;
using Top.Api.Util;


namespace Learun.Application.Web.Controllers
{
    /// <summary>
    /// 版 本 Learun-ADMS V6.1.6.0 力软敏捷开发框架
    /// Copyright (c) 2013-2017 上海力软信息技术有限公司
    /// 创建人：胡亚广
    /// 日 期：2017.03.09
    /// 描 述：错误页控制器
    /// </summary>
    public class TBUserInfoControllerController : MvcAPIControllerBase
    {
        //private Application_SettingIBLL application_SettingIBLL = new Application_SettingBLL();
        private DM_BaseSettingIBLL dm_BaseSettingIBLL = new DM_BaseSettingBLL();
        private DM_UserIBLL dm_userIBLL = new DM_UserBLL();

        [HttpGet]
        public ActionResult AuthorResult(string IsSuccess, string ErrorMessage)
        {
            ViewBag.IsSuccess = IsSuccess;
            ViewBag.ErrorMessage = ErrorMessage;
            return View();
        }

        [HttpGet]
        public ActionResult CallBack(string access_token, string token_type, string expires_in, string refresh_token, string state)
        {
            if (state.StartsWith("hyg"))
            {
                string userid = state.Substring(3);
                /*
                 * 机器人的授权配置
                 * s_application_settingEntity s_Application_SettingEntity = application_SettingIBLL.GetEntityByApplicationId(userid);
                s_Application_SettingEntity.F_TB_SessionKey = access_token;
                s_Application_SettingEntity.F_TB_AuthorEndTime = DateTime.Now.AddMonths(1);
                application_SettingIBLL.SaveEntity(s_Application_SettingEntity.F_SettingId, s_Application_SettingEntity);*/

                /*
                * 多米的授权配置
                */
                dm_basesettingEntity dm_BasesettingEntity = dm_BaseSettingIBLL.GetEntityByCache(userid);
                //dm_BasesettingEntity.appid = userid;
                dm_BasesettingEntity.tb_sessionkey = access_token;
                dm_BasesettingEntity.tb_authorendtime = DateTime.Now.AddMonths(1);

                dm_BaseSettingIBLL.SaveEntity(userid, dm_BasesettingEntity);
            }
            else
            {
                string[] allKeys = Request.QueryString.AllKeys;
                Dictionary<string, string> keyValuePairs = new Dictionary<string, string>();

                for (int i = 0; i < allKeys.Length; i++)
                {
                    string name = Request.QueryString[allKeys[i]];
                    keyValuePairs.Add(allKeys[i], name);
                }

                //Log log = LogFactory.GetLogger("workflowapi");
                //log.Error(keyValuePairs.ToJson());
            }
            return Success("授权成功!");
        }

        [HttpGet]
        public ActionResult AuthorCallBack(string code, string state)
        {
            Learun.Loger.Log log = LogFactory.GetLogger("workflowapi");

            try
            {
                int user_id = int.Parse(state);
                dm_userEntity dm_UserEntity = dm_userIBLL.GetEntityByCache(user_id);

                dm_basesettingEntity dm_BasesettingEntity = dm_BaseSettingIBLL.GetEntityByCache(dm_UserEntity.appid);


                WebUtils webUtils = new WebUtils();
                IDictionary<string, string> pout = new Dictionary<string, string>();
                pout.Add("grant_type", "authorization_code");
                pout.Add("client_id", dm_BasesettingEntity.tb_appkey);
                pout.Add("client_secret", dm_BasesettingEntity.tb_appsecret);
                pout.Add("code", code);
                pout.Add("redirect_uri", HttpUtility.UrlEncode(CommonConfig.tb_auth_address));
                string output = webUtils.DoPost("http://oauth.taobao.com/token", pout);

                log.Error(output);

                AuthorInfo authorInfo = JsonConvert.DeserializeObject<AuthorInfo>(output);

                ITopClient client = new DefaultTopClient("http://gw.api.taobao.com/router/rest", dm_BasesettingEntity.tb_appkey, dm_BasesettingEntity.tb_appsecret);
                TbkScPublisherInfoSaveRequest req = new TbkScPublisherInfoSaveRequest();
                req.RelationFrom = "1";
                req.OfflineScene = "1";
                req.OnlineScene = "1";
                req.InviterCode = dm_BasesettingEntity.tb_relation_invitecode;
                req.InfoType = 1L;
                req.Note = "哆来米代理申请";
                req.RegisterInfo = "{\"phoneNumber\":\"18801088599\",\"city\":\"江苏省\",\"province\":\"南京市\",\"location\":\"玄武区花园小区\",\"detailAddress\":\"5号楼3单元101室\"}";
                TbkScPublisherInfoSaveResponse rsp = client.Execute(req, authorInfo.access_token);
                log.Error(rsp.Body);
                log.Error(HttpUtility.UrlDecode(authorInfo.taobao_user_nick));
                if (rsp.Data == null)
                    throw new Exception(rsp.SubErrMsg);
                else
                {
                    string relation_id = rsp.Data.RelationId.ToString();
                    if (dm_userIBLL.NoExistRelationID(relation_id, user_id))
                    {
                        string[] pids = dm_BasesettingEntity.tb_relation_pid.Split('_');
                        dm_UserEntity.tb_pid = pids.Length == 4 ? pids[3] : "";
                        dm_UserEntity.tb_relationid = relation_id;
                        dm_UserEntity.tb_nickname = HttpUtility.UrlDecode(authorInfo.taobao_user_nick);
                        dm_UserEntity.isrelation_beian = 1;
                        dm_userIBLL.SaveEntity(user_id, dm_UserEntity);
                    }
                    else
                    {
                        throw new Exception("当前淘宝账号已在其他账号下备案,请更换账号!");
                    }
                }



                return RedirectToAction("AuthorResult", new { IsSuccess = "授权成功", ErrorMessage = "" });

                //return Success("授权成功", output);
            }
            catch (Exception ex)
            {
                return RedirectToAction("AuthorResult", new { IsSuccess = "授权失败", ErrorMessage = ex.Message });
            }
        }

        [HttpGet]
        public ActionResult AuthorCallBackBaiChuan(string code, string state)
        {
            try
            {
                int user_id = int.Parse(state);
                dm_userEntity dm_UserEntity = dm_userIBLL.GetEntityByCache(user_id);

                dm_basesettingEntity dm_BasesettingEntity = dm_BaseSettingIBLL.GetEntityByCache(dm_UserEntity.appid);

                WebUtils webUtils = new WebUtils();
                IDictionary<string, string> pout = new Dictionary<string, string>();
                pout.Add("grant_type", "authorization_code");
                pout.Add("client_id", "29236073");
                pout.Add("client_secret", "29de7a8560d773736ef5bf568a7961bd");
                pout.Add("code", code);
                pout.Add("redirect_uri", HttpUtility.UrlEncode(CommonConfig.tb_auth_address));
                string output = webUtils.DoPost("http://oauth.taobao.com/token", pout);

                #region 获取出来用户信息
                Learun.Loger.Log log = LogFactory.GetLogger("workflowapi");
                log.Error(output);
                #endregion

                return Success("授权成功", output);
            }
            catch (Exception ex)
            {
                return Fail(ex.Message);
            }
        }
    }

    public class AuthorInfo
    {
        public string access_token { get; set; }
        public string taobao_user_nick { get; set; }
    }
}