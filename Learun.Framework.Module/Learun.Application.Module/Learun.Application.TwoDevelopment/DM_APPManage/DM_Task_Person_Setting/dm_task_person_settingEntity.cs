using Learun.Util;
using System;
using System.ComponentModel.DataAnnotations.Schema;
namespace Learun.Application.TwoDevelopment.DM_APPManage

{
    /// <summary>
    /// 版 本 Learun-ADMS V6.1.6.0 力软敏捷开发框架
    /// Copyright (c) 2013-2017 上海力软信息技术有限公司
    /// 创 建：超级管理员
    /// 日 期：2020-04-10 13:55
    /// 描 述：进度任务设置
    /// </summary>
    public class dm_task_person_settingEntity 
    {
        #region 实体成员
        /// <summary>
        /// id
        /// </summary>
        /// <returns></returns>
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]        [Column("ID")]
        public int? id { get; set; }
        /// <summary>
        /// 任务标题
        /// </summary>
        /// <returns></returns>
        [Column("TITLE")]
        public string title { get; set; }
        /// <summary>
        /// 任务描述
        /// </summary>
        /// <returns></returns>
        [Column("REMARK")]
        public string remark { get; set; }
        /// <summary>
        /// 任务类型  1每日签到任务  2邀请粉丝任务 3团队粉丝任务  4购物任务  5团队购物任务
        /// </summary>
        /// <returns></returns>
        [Column("S_TYPE")]
        public int? s_type { get; set; }
        /// <summary>
        /// 所需人数
        /// </summary>
        /// <returns></returns>
        [Column("NEEDCOUNT")]
        public int? needcount { get; set; }
        /// <summary>
        /// 是否为合伙人任务  0非合伙人   1合伙人
        /// </summary>
        /// <returns></returns>
        [Column("ISPARTNERS")]
        public int? ispartners { get; set; }
        /// <summary>
        /// isenabled
        /// </summary>
        /// <returns></returns>
        [Column("ISENABLED")]
        public int? isenabled { get; set; }
        /// <summary>
        /// 创建时间
        /// </summary>
        /// <returns></returns>
        [Column("CREATETIME")]
        public DateTime? createtime { get; set; }
        /// <summary>
        /// 修改时间
        /// </summary>
        /// <returns></returns>
        [Column("UPDATETIME")]
        public DateTime? updatetime { get; set; }
        /// <summary>
        /// 平台id
        /// </summary>
        /// <returns></returns>
        [Column("APPID")]
        public string appid { get; set; }
        /// <summary>
        /// 奖励类型  0积分奖励  1余额奖励 
        /// </summary>
        /// <returns></returns>
        [Column("REWARDTYPE")]
        public int? rewardtype { get; set; }
        /// <summary>
        /// 奖励数量  积分/余额
        /// </summary>
        /// <returns></returns>
        [Column("REWARDCOUNT")]
        public decimal? rewardcount { get; set; }
        /// <summary>
        /// 完成数量
        /// </summary>
        [Column("FINISHCOUNT")]
        public int finishcount { get; set; }
        /// <summary>
        /// 完成状态 0未完成  1已完成待领取  2已领取
        /// </summary>
        [Column("FINISHSTATUS")]
        public int finishstatus { get; set; }
        #endregion

        #region 扩展操作
        /// <summary>
        /// 新增调用
        /// </summary>
        public void Create()
        {
        }
        /// <summary>
        /// 编辑调用
        /// </summary>
        /// <param name="keyValue"></param>
        public void Modify(int? keyValue)
        {
            this.id = keyValue;
        }
        #endregion
    }
}

