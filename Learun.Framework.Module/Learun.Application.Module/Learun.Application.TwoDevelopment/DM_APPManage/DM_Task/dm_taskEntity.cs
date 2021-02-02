using Learun.Util;
using System;
using System.ComponentModel.DataAnnotations.Schema;
namespace Learun.Application.TwoDevelopment.DM_APPManage

{
    /// <summary>
    /// 版 本 Learun-ADMS V6.1.6.0 力软敏捷开发框架
    /// Copyright (c) 2013-2017 上海力软信息技术有限公司
    /// 创 建：超级管理员
    /// 日 期：2020-04-16 16:01
    /// 描 述：任务中心
    /// </summary>
    public class dm_taskEntity
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
        /// 任务编号
        /// </summary>
        /// <returns></returns>
        [Column("TASK_NO")]
        public string task_no { get; set; }
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
        /// 任务状态 -2待审核 0进行中  1已完成  2已取消 3任务下架
        /// </summary>
        /// <returns></returns>
        [Column("TASK_STATUS")]
        public int? task_status { get; set; }
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
        /// 0pc  1移动
        /// </summary>
        /// <returns></returns>
        [Column("PLAFORM")]
        public int? plaform { get; set; }
        /// <summary>
        /// 排序值
        /// </summary>
        /// <returns></returns>
        [Column("SORT")]
        public decimal sort { get; set; }
        /// <summary>
        /// 创建时间
        /// </summary>
        /// <returns></returns>
        [Column("CREATETIME")]
        public DateTime createtime { get; set; }
        /// <summary>
        /// 总佣金(发布任务所需要的总金额)
        /// </summary>
        /// <returns></returns>
        [Column("TOTALCOMMISSION")]
        public decimal totalcommission { get; set; }

        /// <summary>
        /// 每一单的佣金
        /// </summary>
        [Column("SINGLECOMMISSION")]
        public decimal singlecommission { get; set; }
        /// <summary>
        /// 服务费(每一单的服务费)
        /// </summary>
        /// <returns></returns>
        [Column("SERVICEFEE")]
        public decimal servicefee { get; set; }
        /// <summary>
        /// 初级佣金(每一单的佣金)
        /// </summary>
        /// <returns></returns>
        [Column("JUNIORCOMMISSION")]
        public decimal juniorcommission { get; set; }
        /// <summary>
        /// 高级佣金(每一单的佣金)
        /// </summary>
        /// <returns></returns>
        [Column("SENIORCOMMISSION")]
        public decimal seniorcommission { get; set; }
        /// <summary>
        /// 需求人数
        /// </summary>
        /// <returns></returns>
        [Column("NEEDCOUNT")]
        public int needcount { get; set; }

        /// <summary>
        /// 接受人数
        /// </summary>
        [Column("REVICECOUNT")]
        public int revicecount { get; set; }

        /// <summary>
        /// 完成人数
        /// </summary>
        /// <returns></returns>
        [Column("FINISHCOUNT")]
        public int finishcount { get; set; }
        /// <summary>
        /// 用户ID
        /// </summary>
        /// <returns></returns>
        [Column("USER_ID")]
        public int user_id { get; set; }
        /// <summary>
        /// 平台ID
        /// </summary>
        /// <returns></returns>
        [Column("APPID")]
        public string appid { get; set; }

        /// <summary>
        /// 创建月份
        /// </summary>
        [Column("CREATEMONTH")]
        public int createmonth { get; set; }

        /// <summary>
        /// 创建日期
        /// </summary>
        [Column("CREATEDATE")]
        public int createdate { get; set; }

        [Column("SUBMIT_DATA")]
        public string submit_data { get; set; }

        /// <summary>
        /// 任务取消时间限制
        /// </summary>
        [Column("TASK_TIME_LIMIT")]
        public int task_time_limit { get; set; }

        /// <summary>
        /// 审核模式  1是  0否
        /// </summary>
        [Column("ISCHECKMODE")]
        public int ischeckmode { get; set; }

        /// <summary>
        /// 是否为活动任务  1=是  0=否
        /// </summary>
        [Column("ISACTIVITY")]
        public int isactivity { get; set; }
        #endregion

        #region 扩展操作
        /// <summary>
        /// 新增调用
        /// </summary>
        public void Create()
        {
            this.createtime = DateTime.Now;
            this.finishcount = 0;
            this.plaform = 1;
            this.revicecount = 0;
            this.createmonth = int.Parse(this.createtime.ToString("yyyyMM"));
            this.createdate = int.Parse(this.createtime.ToString("yyyyMMdd"));
            this.ischeckmode = 0;
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

