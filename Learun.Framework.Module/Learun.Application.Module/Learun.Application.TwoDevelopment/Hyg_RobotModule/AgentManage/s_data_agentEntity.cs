using Learun.Util;
using System;
using System.ComponentModel.DataAnnotations.Schema;
namespace Learun.Application.TwoDevelopment.Hyg_RobotModule

{
    /// <summary>
    /// 版 本 Learun-ADMS V6.1.6.0 力软敏捷开发框架
    /// Copyright (c) 2013-2017 上海力软信息技术有限公司
    /// 创 建：超级管理员
    /// 日 期：2019-11-19 21:15
    /// 描 述：代理商管理
    /// </summary>
    public class s_data_agentEntity 
    {
        #region 实体成员
        /// <summary>
        /// 代理商ID
        /// </summary>
        /// <returns></returns>
        [Column("F_AGENTID")]
        public string F_AgentId { get; set; }
        /// <summary>
        /// 代理商账户
        /// </summary>
        /// <returns></returns>
        [Column("F_ACCOUNT")]
        public string F_Account { get; set; }
        /// <summary>
        /// 代理商密码
        /// </summary>
        /// <returns></returns>
        [Column("F_PASSWORD")]
        public string F_Password { get; set; }
        /// <summary>
        /// 代理商昵称
        /// </summary>
        /// <returns></returns>
        [Column("F_NICKNAME")]
        public string F_NickName { get; set; }
        /// <summary>
        /// 允许使用开始时间
        /// </summary>
        /// <returns></returns>
        [Column("F_ALLOWSTARTTIME")]
        public DateTime? F_AllowStartTime { get; set; }
        /// <summary>
        /// 允许使用结束时间
        /// </summary>
        /// <returns></returns>
        [Column("F_ALLOWENDTIME")]
        public DateTime? F_AllowEndTime { get; set; }
        /// <summary>
        /// 拼多多佣金比例
        /// </summary>
        /// <returns></returns>
        [Column("F_PDD_COMISSIONRATE")]
        public decimal? F_PDD_ComissionRate { get; set; }
        /// <summary>
        /// 京东佣金比例
        /// </summary>
        /// <returns></returns>
        [Column("F_JD_COMISSIONRATE")]
        public decimal? F_JD_ComissionRate { get; set; }
        /// <summary>
        /// 淘宝佣金比例
        /// </summary>
        /// <returns></returns>
        [Column("F_TB_COMISSIONRATE")]
        public decimal? F_TB_ComissionRate { get; set; }
        /// <summary>
        /// 淘宝渠道ID
        /// </summary>
        /// <returns></returns>
        [Column("F_TB_RELATIONID")]
        public string F_TB_RelationId { get; set; }
        /// <summary>
        /// 淘宝PID
        /// </summary>
        /// <returns></returns>
        [Column("F_TB_PID")]
        public string F_TB_PID { get; set; }
        /// <summary>
        /// 京东PID
        /// </summary>
        /// <returns></returns>
        [Column("F_JD_PID")]
        public string F_JD_PID { get; set; }
        /// <summary>
        /// 拼多多PID
        /// </summary>
        /// <returns></returns>
        [Column("F_PDD_PID")]
        public string F_PDD_PID { get; set; }
        /// <summary>
        /// 应用商ID
        /// </summary>
        /// <returns></returns>
        [Column("F_APPLICATIONID")]
        public string F_ApplicationId { get; set; }
        /// <summary>
        /// 有效标志
        /// </summary>
        /// <returns></returns>
        [Column("F_ENABLEDMARK")]
        public int? F_EnabledMark { get; set; }
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
            this.F_AgentId = Guid.NewGuid().ToString();
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
            this.F_AgentId = keyValue;
            this.F_ModifyDate = DateTime.Now;
            UserInfo userInfo = LoginUserInfo.Get();
            this.F_ModifyUserId = userInfo.userId;
            this.F_ModifyUserName = userInfo.realName;
        }
        #endregion
    }
}

