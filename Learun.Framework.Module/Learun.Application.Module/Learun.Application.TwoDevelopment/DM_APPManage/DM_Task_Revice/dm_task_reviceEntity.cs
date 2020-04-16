using Learun.Util;
using System;
using System.ComponentModel.DataAnnotations.Schema;
namespace Learun.Application.TwoDevelopment.DM_APPManage

{
    /// <summary>
    /// 版 本 Learun-ADMS V6.1.6.0 力软敏捷开发框架
    /// Copyright (c) 2013-2017 上海力软信息技术有限公司
    /// 创 建：超级管理员
    /// 日 期：2020-04-16 16:03
    /// 描 述：任务接受记录
    /// </summary>
    public class dm_task_reviceEntity 
    {
        #region 实体成员
        /// <summary>
        /// id
        /// </summary>
        /// <returns></returns>
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]        [Column("ID")]
        public int? id { get; set; }
        /// <summary>
        /// 任务id
        /// </summary>
        /// <returns></returns>
        [Column("TASK_ID")]
        public int? task_id { get; set; }
        /// <summary>
        /// 用户id
        /// </summary>
        /// <returns></returns>
        [Column("USER_ID")]
        public int? user_id { get; set; }
        /// <summary>
        /// 接受时间
        /// </summary>
        /// <returns></returns>
        [Column("REVICE_TIME")]
        public DateTime? revice_time { get; set; }
        /// <summary>
        /// 接收状态 1进行中  2待审核  3已完成  4取消任务
        /// </summary>
        /// <returns></returns>
        [Column("STATUS")]
        public int? status { get; set; }
        /// <summary>
        /// 审核时间
        /// </summary>
        /// <returns></returns>
        [Column("CHECK_TIME")]
        public DateTime? check_time { get; set; }
        /// <summary>
        /// 提交时间
        /// </summary>
        /// <returns></returns>
        [Column("SUBMIT_TIME")]
        public DateTime? submit_time { get; set; }
        /// <summary>
        /// 提交数据
        /// </summary>
        /// <returns></returns>
        [Column("SUBMIT_DATA")]
        public string submit_data { get; set; }
        /// <summary>
        /// 创建时间
        /// </summary>
        /// <returns></returns>
        [Column("CREATETIME")]
        public DateTime? createtime { get; set; }
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

