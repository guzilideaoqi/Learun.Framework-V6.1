using Learun.Util;
using System;
using System.ComponentModel.DataAnnotations.Schema;
namespace Learun.Application.TwoDevelopment.DM_APPManage

{
    /// <summary>
    /// 版 本 Learun-ADMS V6.1.6.0 力软敏捷开发框架
    /// Copyright (c) 2013-2017 上海力软信息技术有限公司
    /// 创 建：超级管理员
    /// 日 期：2021-03-03 10:50
    /// 描 述：多麦计划
    /// </summary>
    public class dm_dauomai_plan_manageEntity 
    {
        #region 实体成员
        /// <summary>
        /// f_id
        /// </summary>
        /// <returns></returns>
        [Column("ID")]
        public int? id { get; set; }
        /// <summary>
        /// 推广计划ID
        /// </summary>
        /// <returns></returns>
        [Column("ADS_ID")]
        public int? ads_id { get; set; }
        /// <summary>
        /// 推广计划名称
        /// </summary>
        /// <returns></returns>
        [Column("ADS_NAME")]
        public string ads_name { get; set; }
        /// <summary>
        /// 所属商场名称
        /// </summary>
        /// <returns></returns>
        [Column("STORE_NAME")]
        public string store_name { get; set; }
        /// <summary>
        /// 活动类型：0 web 1 wap 3 ROI 4 小程序
        /// </summary>
        /// <returns></returns>
        [Column("CHANNEL")]
        public int? channel { get; set; }
        /// <summary>
        /// RD有效期
        /// </summary>
        /// <returns></returns>
        [Column("RDDAYS")]
        public string rddays { get; set; }
        /// <summary>
        /// 普通佣金说明，具体佣金说明及政策请访问
        /// </summary>
        /// <returns></returns>
        [Column("COMMISSION")]
        public string commission { get; set; }
        /// <summary>
        /// 当category=1 海外商家时，此字段表示计划允许的跟单地区
        /// </summary>
        /// <returns></returns>
        [Column("CATEGORY_AREA")]
        public string category_area { get; set; }
        /// <summary>
        /// 商家类型：0 国内商家 1 海外商家 2 跨境电商
        /// </summary>
        /// <returns></returns>
        [Column("CATEGORY")]
        public string category { get; set; }
        /// <summary>
        /// 审核方式 1人工审核 2自动通过
        /// </summary>
        /// <returns></returns>
        [Column("APPLY_MODE")]
        public string apply_mode { get; set; }
        /// <summary>
        /// 计划开始时间，格式:yyyy-MM-dd HH:mm:ss
        /// </summary>
        /// <returns></returns>
        [Column("STIME")]
        public DateTime? stime { get; set; }
        /// <summary>
        /// 计划截止时间，格式:yyyy-MM-dd HH:mm:ss
        /// </summary>
        /// <returns></returns>
        [Column("ETIME")]
        public DateTime? etime { get; set; }
        /// <summary>
        /// 默认url
        /// </summary>
        /// <returns></returns>
        [Column("URL")]
        public string url { get; set; }
        /// <summary>
        /// 计划logo
        /// </summary>
        /// <returns></returns>
        [Column("ADS_LOGO")]
        public string ads_logo { get; set; }
        /// <summary>
        /// 计划状态：0 待提交 1 待审核 2 已拒绝 3 运行中 4 修改待审核 7 已终止 8已挂起
        /// </summary>
        /// <returns></returns>
        [Column("STATUS")]
        public string status { get; set; }
        /// <summary>
        /// 申请状态：1审核通过，0未审核通过，-1未申请
        /// </summary>
        /// <returns></returns>
        [Column("ADS_APPLY_STATUS")]
        public string ads_apply_status { get; set; }
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
            this.updatetime = DateTime.Now;
        }
        #endregion
    }
}

