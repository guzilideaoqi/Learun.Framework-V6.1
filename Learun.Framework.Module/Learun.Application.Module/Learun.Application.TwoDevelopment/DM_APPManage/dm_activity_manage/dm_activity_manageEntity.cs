using Learun.Util;
using System;
using System.ComponentModel.DataAnnotations.Schema;
namespace Learun.Application.TwoDevelopment.DM_APPManage

{
    /// <summary>
    /// 版 本 Learun-ADMS V6.1.6.0 力软敏捷开发框架
    /// Copyright (c) 2013-2017 上海力软信息技术有限公司
    /// 创 建：超级管理员
    /// 日 期：2021-02-05 15:33
    /// 描 述：活动管理
    /// </summary>
    public class dm_activity_manageEntity 
    {
        #region 实体成员
        /// <summary>
        /// 主键ID
        /// </summary>
        /// <returns></returns>
        [Column("F_ID")]
        public string f_id { get; set; }
        /// <summary>
        /// 红包背景图片
        /// </summary>
        /// <returns></returns>
        [Column("APP_REDPAPER_IMAGE")]
        public string APP_RedPaper_Image { get; set; }
        /// <summary>
        /// 红包上的文本信息
        /// </summary>
        /// <returns></returns>
        [Column("APP_REDPAPER_TEXT")]
        public string APP_RedPaper_Text { get; set; }
        /// <summary>
        /// 摇晃红包图片地址
        /// </summary>
        /// <returns></returns>
        [Column("APP_ROCK_REDPAPER_IMAGE")]
        public string APP_Rock_RedPaper_Image { get; set; }
        /// <summary>
        /// 开红包跳转地址
        /// </summary>
        /// <returns></returns>
        [Column("APP_TO_ACTIVITYURL")]
        public string APP_To_ActivityUrl { get; set; }
        /// <summary>
        /// 前端刘海栏显示文本
        /// </summary>
        /// <returns></returns>
        [Column("ACTIVITYTITLE")]
        public string ActivityTitle { get; set; }
        /// <summary>
        /// 活动类型  0=初始有红包加做任务   1=初始无红包加做任务加邀请人
        /// </summary>
        /// <returns></returns>
        [Column("ACTIVITYTYPE")]
        public int? ActivityType { get; set; }
        /// <summary>
        /// 活动开始时间
        /// </summary>
        /// <returns></returns>
        [Column("ACTIVITYSTARTTIME")]
        public DateTime? ActivityStartTime { get; set; }
        /// <summary>
        /// 活动结束时间
        /// </summary>
        /// <returns></returns>
        [Column("ACTIVITYENDTIME")]
        public DateTime? ActivityEndTime { get; set; }
        /// <summary>
        /// 活动编号
        /// </summary>
        /// <returns></returns>
        [Column("ACTIVITYCODE")]
        public string ActivityCode { get; set; }
        /// <summary>
        /// 活动状态   1=进行中的活动  0=活动已暂停
        /// </summary>
        /// <returns></returns>
        [Column("ACTIVITYSTATUS")]
        public int? ActivityStatus { get; set; }
        /// <summary>
        /// 活动描述
        /// </summary>
        /// <returns></returns>
        [Column("ACTIVITYREMARK")]
        public string ActivityRemark { get; set; }
        /// <summary>
        /// 初始红包最小金额
        /// </summary>
        /// <returns></returns>
        [Column("INITREDPAPER_MINPRICE")]
        public decimal? InitRedPaper_MinPrice { get; set; }
        /// <summary>
        /// 初始红包最大金额
        /// </summary>
        /// <returns></returns>
        [Column("INITREDPAPER_MAXPRICE")]
        public decimal? InitRedPaper_MaxPrice { get; set; }
        /// <summary>
        /// 奖励金额  ActivityType=1时使用该字段
        /// </summary>
        /// <returns></returns>
        [Column("REWARDPRICE")]
        public decimal? RewardPrice { get; set; }
        /// <summary>
        /// 创建时间
        /// </summary>
        /// <returns></returns>
        [Column("CREATETIME")]
        public DateTime? CreateTime { get; set; }
        /// <summary>
        /// 修改时间
        /// </summary>
        /// <returns></returns>
        [Column("UPDATETIME")]
        public DateTime? UpdateTime { get; set; }
        #endregion

        #region 扩展操作
        /// <summary>
        /// 新增调用
        /// </summary>
        public void Create()
        {
            this.f_id = Guid.NewGuid().ToString();
            this.CreateTime = DateTime.Now;
        }
        /// <summary>
        /// 编辑调用
        /// </summary>
        /// <param name="keyValue"></param>
        public void Modify(string keyValue)
        {
            this.f_id = keyValue;
            this.UpdateTime = DateTime.Now;
        }
        #endregion
    }
}

