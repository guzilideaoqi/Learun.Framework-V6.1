using Learun.Application.TwoDevelopment.DM_APPManage;
using Learun.Application.TwoDevelopment.Hyg_RobotModule;
using Learun.Util;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Learun.Application.Web.Controllers
{
    /// <summary>
    /// 版 本 Learun-ADMS V6.1.6.0 力软敏捷开发框架
    /// Copyright (c) 2013-2017 上海力软信息技术有限公司
    /// 创建人：胡亚广
    /// 日 期：2017.03.09
    /// 描 述：错误页控制器
    /// </summary>
    [HandlerLogin(FilterMode.Ignore)]
    public class TBUserInfoControllerController : MvcControllerBase
    {
        //private Application_SettingIBLL application_SettingIBLL = new Application_SettingBLL();
        private DM_BaseSettingIBLL dm_BaseSettingIBLL = new DM_BaseSettingBLL();
        [HttpGet]
        public ActionResult CallBack(string access_token,string token_type,string expires_in,string refresh_token,string state) {
            if (state.StartsWith("hyg")) {
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
                dm_basesettingEntity dm_BasesettingEntity = new dm_basesettingEntity();
                dm_BasesettingEntity.appid = userid;
                dm_BasesettingEntity.tb_sessionkey = access_token;
                dm_BasesettingEntity.tb_authorendtime= DateTime.Now.AddMonths(1);

                dm_BaseSettingIBLL.SaveEntity(userid, dm_BasesettingEntity);
            }
            return Success("授权成功!");
        }
    }
}