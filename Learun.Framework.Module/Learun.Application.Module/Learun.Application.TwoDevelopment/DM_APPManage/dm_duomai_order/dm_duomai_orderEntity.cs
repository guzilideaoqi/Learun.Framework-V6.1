using Learun.Util;
using System;
using System.ComponentModel.DataAnnotations.Schema;
namespace Learun.Application.TwoDevelopment.DM_APPManage

{
    /// <summary>
    /// 版 本 Learun-ADMS V6.1.6.0 力软敏捷开发框架
    /// Copyright (c) 2013-2017 上海力软信息技术有限公司
    /// 创 建：超级管理员
    /// 日 期：2021-03-06 11:15
    /// 描 述：dm_duomai_order
    /// </summary>
    public class dm_duomai_orderEntity 
    {
        #region 实体成员
        /// <summary>
        /// id
        /// </summary>
        /// <returns></returns>
        [Column("ID")]
        public string id { get; set; }
        /// <summary>
        /// 计划ID
        /// </summary>
        /// <returns></returns>
        [Column("ADS_ID")]
        public string ads_id { get; set; }
        /// <summary>
        /// 推广位ID
        /// </summary>
        /// <returns></returns>
        [Column("SITE_ID")]
        public string site_id { get; set; }
        /// <summary>
        /// 推广计划链接ID
        /// </summary>
        /// <returns></returns>
        [Column("LINK_ID")]
        public string link_id { get; set; }
        /// <summary>
        /// 反馈ID
        /// </summary>
        /// <returns></returns>
        [Column("EUID")]
        public string euid { get; set; }
        /// <summary>
        /// 订单编号
        /// </summary>
        /// <returns></returns>
        [Column("ORDER_SN")]
        public string order_sn { get; set; }
        /// <summary>
        /// 针对部分联盟存在父订单号
        /// </summary>
        /// <returns></returns>
        [Column("PARENT_ORDER_SN")]
        public string parent_order_sn { get; set; }
        /// <summary>
        /// 订单下单时间，格式:yyyy-MM-dd HH:mm:ss
        /// </summary>
        /// <returns></returns>
        [Column("ORDER_TIME")]
        public DateTime? order_time { get; set; }
        /// <summary>
        /// 订单金额，例：1.00
        /// </summary>
        /// <returns></returns>
        [Column("ORDERS_PRICE")]
        public decimal? orders_price { get; set; }
        /// <summary>
        /// 订单佣金，例：1.00
        /// </summary>
        /// <returns></returns>
        [Column("SITER_COMMISSION")]
        public decimal? siter_commission { get; set; }
        /// <summary>
        /// 多麦联盟结算状态：-1=无效 0=未确认 1=确认 2=结算
        /// </summary>
        /// <returns></returns>
        [Column("STATUS")]
        public string status { get; set; }
        /// <summary>
        /// 确认订单金额，1.00
        /// </summary>
        /// <returns></returns>
        [Column("CONFIRM_PRICE")]
        public decimal? confirm_price { get; set; }
        /// <summary>
        /// 确认订单佣金，1.00
        /// </summary>
        /// <returns></returns>
        [Column("CONFIRM_SITER_COMMISSION")]
        public decimal? confirm_siter_commission { get; set; }
        /// <summary>
        /// 结算时间，格式:yyyy-MM-dd HH:mm:ss
        /// </summary>
        /// <returns></returns>
        [Column("CHARGE_TIME")]
        public DateTime? charge_time { get; set; }
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
            this.id = Guid.NewGuid().ToString();
        }
        /// <summary>
        /// 编辑调用
        /// </summary>
        /// <param name="keyValue"></param>
        public void Modify(string keyValue)
        {
            this.id = keyValue;
        }
        #endregion
    }
}

