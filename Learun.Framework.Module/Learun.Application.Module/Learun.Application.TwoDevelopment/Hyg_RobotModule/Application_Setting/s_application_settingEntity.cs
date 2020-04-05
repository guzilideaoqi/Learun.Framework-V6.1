using Learun.Util;
using System;
using System.ComponentModel.DataAnnotations.Schema;
namespace Learun.Application.TwoDevelopment.Hyg_RobotModule

{
    /// <summary>
    /// 版 本 Learun-ADMS V6.1.6.0 力软敏捷开发框架
    /// Copyright (c) 2013-2017 上海力软信息技术有限公司
    /// 创 建：超级管理员
    /// 日 期：2019-11-19 20:34
    /// 描 述：应用商信息设置
    /// </summary>
    public class s_application_settingEntity 
    {
        #region 实体成员
        /// <summary>
        /// 配置表ID
        /// </summary>
        /// <returns></returns>
        [Column("F_SETTINGID")]
        public string F_SettingId { get; set; }
        /// <summary>
        /// 应用ID
        /// </summary>
        /// <returns></returns>
        [Column("F_APPLICATIONID")]
        public string F_ApplicationId { get; set; }
        /// <summary>
        /// 联盟账户ID
        /// </summary>
        /// <returns></returns>
        [Column("F_TB_ACCOUNTID")]
        public string F_TB_AccountId { get; set; }
        /// <summary>
        /// 淘宝AppKey
        /// </summary>
        /// <returns></returns>
        [Column("F_TB_APPKEY")]
        public string F_TB_AppKey { get; set; }
        /// <summary>
        /// 淘宝AppSecret
        /// </summary>
        /// <returns></returns>
        [Column("F_TB_SECRET")]
        public string F_TB_Secret { get; set; }
        /// <summary>
        /// 淘宝SessionKey
        /// </summary>
        /// <returns></returns>
        [Column("F_TB_SESSIONKEY")]
        public string F_TB_SessionKey { get; set; }
        /// <summary>
        /// 淘宝授权到期时间
        /// </summary>
        /// <returns></returns>
        [Column("F_TB_AUTHORENDTIME")]
        public DateTime? F_TB_AuthorEndTime { get; set; }
        /// <summary>
        /// 京东联盟账户ID
        /// </summary>
        /// <returns></returns>
        [Column("F_JD_ACCOUNTID")]
        public string F_JD_AccountId { get; set; }
        /// <summary>
        /// 京东AppKey
        /// </summary>
        /// <returns></returns>
        [Column("F_JD_APPKEY")]
        public string F_JD_AppKey { get; set; }
        /// <summary>
        /// 京东Secret
        /// </summary>
        /// <returns></returns>
        [Column("F_JD_SECRET")]
        public string F_JD_Secret { get; set; }
        /// <summary>
        /// 京东SessionKey
        /// </summary>
        /// <returns></returns>
        [Column("F_JD_SESSIONKEY")]
        public string F_JD_SessionKey { get; set; }
        /// <summary>
        /// 多多进宝账户ID
        /// </summary>
        /// <returns></returns>
        [Column("F_PDD_ACCOUNTID")]
        public string F_PDD_AccountId { get; set; }
        /// <summary>
        /// 拼多多ClientID
        /// </summary>
        /// <returns></returns>
        [Column("F_PDD_CLIENTID")]
        public string F_PDD_ClientID { get; set; }
        /// <summary>
        /// 拼多多ClientSecret
        /// </summary>
        /// <returns></returns>
        [Column("F_PDD_CLIENTSECRET")]
        public string F_PDD_ClientSecret { get; set; }
        /// <summary>
        /// 应用商名称
        /// </summary>
        /// <returns></returns>
        [Column("F_APPLICATIONNAME")]
        public string F_ApplicationName { get; set; }
        /// <summary>
        /// 应用商Logo
        /// </summary>
        /// <returns></returns>
        [Column("F_APPLICATIONLOGO")]
        public string F_ApplicationLogo { get; set; }
        /// <summary>
        /// 公司名称
        /// </summary>
        /// <returns></returns>
        [Column("F_COMPANYNAME")]
        public string F_CompanyName { get; set; }
        /// <summary>
        /// 联系方式
        /// </summary>
        /// <returns></returns>
        [Column("F_TELEPHONE")]
        public string F_Telephone { get; set; }
        /// <summary>
        /// QQ号
        /// </summary>
        /// <returns></returns>
        [Column("F_OICQ")]
        public string F_OICQ { get; set; }
        /// <summary>
        /// 微信号
        /// </summary>
        /// <returns></returns>
        [Column("F_WECHAT")]
        public string F_WeChat { get; set; }
        /// <summary>
        /// 描述信息
        /// </summary>
        /// <returns></returns>
        [Column("F_DESCRIPTION")]
        public string F_Description { get; set; }
        /// <summary>
        /// 创建日期
        /// </summary>
        /// <returns></returns>
        [Column("F_CREATEDATE")]
        public DateTime? F_CreateDate { get; set; }
        /// <summary>
        /// 创建用户主键
        /// </summary>
        /// <returns></returns>
        [Column("F_CREATEUSERID")]
        public string F_CreateUserId { get; set; }
        /// <summary>
        /// 创建用户
        /// </summary>
        /// <returns></returns>
        [Column("F_CREATEUSERNAME")]
        public string F_CreateUserName { get; set; }
        /// <summary>
        /// 修改日期
        /// </summary>
        /// <returns></returns>
        [Column("F_MODIFYDATE")]
        public DateTime? F_ModifyDate { get; set; }
        /// <summary>
        /// 修改用户主键
        /// </summary>
        /// <returns></returns>
        [Column("F_MODIFYUSERID")]
        public string F_ModifyUserId { get; set; }
        /// <summary>
        /// 修改用户
        /// </summary>
        /// <returns></returns>
        [Column("F_MODIFYUSERNAME")]
        public string F_ModifyUserName { get; set; }
        #endregion

        #region 扩展操作
        /// <summary>
        /// 新增调用
        /// </summary>
        public void Create()
        {
            this.F_SettingId = Guid.NewGuid().ToString();
            this.F_CreateDate = DateTime.Now;
            UserInfo userInfo = LoginUserInfo.Get();
            this.F_CreateUserId = userInfo.userId;
            this.F_CreateUserName = userInfo.realName;
        }
        /// <summary>
        /// 编辑调用
        /// </summary>
        /// <param name="keyValue"></param>
        public void Modify(string keyValue)
        {
            this.F_SettingId = keyValue;
            this.F_ModifyDate = DateTime.Now;
            UserInfo userInfo = LoginUserInfo.Get();
            if (userInfo != null) {
                this.F_ModifyUserId = userInfo.userId;
                this.F_ModifyUserName = userInfo.realName;
            }
        }
        #endregion
    }
}

