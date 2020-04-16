using Learun.Util;
using System;
using System.ComponentModel.DataAnnotations.Schema;
namespace Learun.Application.TwoDevelopment.DM_APPManage

{
    /// <summary>
    /// 版 本 Learun-ADMS V6.1.6.0 力软敏捷开发框架
    /// Copyright (c) 2013-2017 上海力软信息技术有限公司
    /// 创 建：超级管理员
    /// 日 期：2020-04-16 16:02
    /// 描 述：任务举报记录
    /// </summary>
    public class dm_task_reportEntity 
    {
        #region 实体成员
        /// <summary>
        /// id
        /// </summary>
        /// <returns></returns>
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]        [Column("ID")]
        public int? id { get; set; }
        /// <summary>
        /// 举报用户
        /// </summary>
        /// <returns></returns>
        [Column("USER_ID")]
        public int? user_id { get; set; }
        /// <summary>
        /// 举报任务id
        /// </summary>
        /// <returns></returns>
        [Column("TASK_ID")]
        public int? task_id { get; set; }
        /// <summary>
        /// 举报内容
        /// </summary>
        /// <returns></returns>
        [Column("REPORT_CONTENT")]
        public string report_content { get; set; }
        /// <summary>
        /// 举报时间
        /// </summary>
        /// <returns></returns>
        [Column("REPORT_TIME")]
        public DateTime? report_time { get; set; }
        /// <summary>
        /// 平台id
        /// </summary>
        /// <returns></returns>
        [Column("APPID")]
        public string appid { get; set; }
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

