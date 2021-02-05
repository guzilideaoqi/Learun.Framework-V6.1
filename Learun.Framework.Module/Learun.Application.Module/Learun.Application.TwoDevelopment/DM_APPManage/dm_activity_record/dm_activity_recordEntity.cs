using Learun.Util;
using System;
using System.ComponentModel.DataAnnotations.Schema;
namespace Learun.Application.TwoDevelopment.DM_APPManage

{
    /// <summary>
    /// 版 本 Learun-ADMS V6.1.6.0 力软敏捷开发框架
    /// Copyright (c) 2013-2017 上海力软信息技术有限公司
    /// 创 建：超级管理员
    /// 日 期：2021-02-05 14:26
    /// 描 述：活动领取记录
    /// </summary>
    public class dm_activity_recordEntity 
    {
        #region 实体成员
        /// <summary>
        /// 记录id
        /// </summary>
        /// <returns></returns>
        [Column("F_ID")]
        public string f_id { get; set; }

        /// <summary>
        /// 用户ID
        /// </summary>
        [Column("USER_ID")]
        public int? user_id { get; set; }
        /// <summary>
        /// 活动编号
        /// </summary>
        /// <returns></returns>
        [Column("ACTIVITY_CODE")]
        public string activity_code { get; set; }
        /// <summary>
        /// 领取活动时间
        /// </summary>
        /// <returns></returns>
        [Column("CREATETIME")]
        public DateTime? createtime { get; set; }
        /// <summary>
        /// 完成时间
        /// </summary>
        /// <returns></returns>
        [Column("FINISHTIME")]
        public DateTime? finishtime { get; set; }
        /// <summary>
        /// 邀请人数
        /// </summary>
        /// <returns></returns>
        [Column("INVITENUM")]
        public int? invitenum { get; set; }

        /// <summary>
        /// 初始活动金额
        /// </summary>
        [Column("INITACTIVITYPRICE")]
        public decimal? initactivityprice { get; set; }
        /// <summary>
        /// 活动类型  0=初始有红包加做任务   1=初始无红包加做任务加邀请人
        /// </summary>
        /// <returns></returns>
        [Column("ACTIVITY_TYPE")]
        public int? activity_type { get; set; }
        #endregion

        #region 扩展操作
        /// <summary>
        /// 新增调用
        /// </summary>
        public void Create()
        {
            this.f_id = Guid.NewGuid().ToString();
        }
        /// <summary>
        /// 编辑调用
        /// </summary>
        /// <param name="keyValue"></param>
        public void Modify(string keyValue)
        {
            this.f_id = keyValue;
        }
        #endregion
    }
}

