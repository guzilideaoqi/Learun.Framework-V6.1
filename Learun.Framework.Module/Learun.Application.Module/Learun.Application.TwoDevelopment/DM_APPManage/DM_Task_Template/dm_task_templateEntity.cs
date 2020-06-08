using Learun.Util;
using System;
using System.ComponentModel.DataAnnotations.Schema;
namespace Learun.Application.TwoDevelopment.DM_APPManage

{
    /// <summary>
    /// 版 本 Learun-ADMS V6.1.6.0 力软敏捷开发框架
    /// Copyright (c) 2013-2017 上海力软信息技术有限公司
    /// 创 建：超级管理员
    /// 日 期：2020-04-16 16:06
    /// 描 述：任务模板
    /// </summary>
    public class dm_task_templateEntity
    {
        #region 实体成员
        /// <summary>
        /// id
        /// </summary>
        /// <returns></returns>
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Column("ID")]
        public int? id { get; set; }
        /// <summary>
        /// task_title
        /// </summary>
        /// <returns></returns>
        [Column("TASK_TITLE")]
        public string task_title { get; set; }
        /// <summary>
        /// 任务类型
        /// </summary>
        /// <returns></returns>
        [Column("TASK_TYPE")]
        public int? task_type { get; set; }
        /// <summary>
        /// 任务描述
        /// </summary>
        /// <returns></returns>
        [Column("TASK_DESCRIPTION")]
        public string task_description { get; set; }
        /// <summary>
        /// 任务操作
        /// </summary>
        /// <returns></returns>
        [Column("TASK_OPERATE")]
        public string task_operate { get; set; }
        /// <summary>
        /// 需求人数
        /// </summary>
        /// <returns></returns>
        [Column("NEEDCOUNT")]
        public int? needcount { get; set; }
        /// <summary>
        /// 总佣金
        /// </summary>
        /// <returns></returns>
        [Column("SINGLECOMMISSION")]
        public decimal? singlecommission { get; set; }
        /// <summary>
        /// createtime
        /// </summary>
        /// <returns></returns>
        [Column("CREATETIME")]
        public DateTime? createtime { get; set; }
        /// <summary>
        /// 用户ID
        /// </summary>
        /// <returns></returns>
        [Column("USER_ID")]
        public int? user_id { get; set; }

        /// <summary>
        /// 任务限制时间
        /// </summary>
        [Column("TASK_TIME_LIMIT")]
        public int? task_time_limit { get; set; }
        #endregion

        #region 扩展操作
        /// <summary>
        /// 新增调用
        /// </summary>
        public void Create()
        {
            this.createtime = DateTime.Now;
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

