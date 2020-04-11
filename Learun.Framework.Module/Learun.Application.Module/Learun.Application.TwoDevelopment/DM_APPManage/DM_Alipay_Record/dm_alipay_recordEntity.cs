using Learun.Util;
using System;
using System.ComponentModel.DataAnnotations.Schema;
namespace Learun.Application.TwoDevelopment.DM_APPManage

{
    /// <summary>
    /// 版 本 Learun-ADMS V6.1.6.0 力软敏捷开发框架
    /// Copyright (c) 2013-2017 上海力软信息技术有限公司
    /// 创 建：超级管理员
    /// 日 期：2020-04-10 13:38
    /// 描 述：开通代理记录
    /// </summary>
    public class dm_alipay_recordEntity 
    {
        #region 实体成员
        /// <summary>
        /// 订单开通记录
        /// </summary>
        /// <returns></returns>
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]        [Column("ID")]
        public int? id { get; set; }
        /// <summary>
        /// 用户Id
        /// </summary>
        /// <returns></returns>
        [Column("OUT_TRADE_NO")]
        public string out_trade_no { get; set; }
        /// <summary>
        /// user_id
        /// </summary>
        /// <returns></returns>
        [Column("USER_ID")]
        public int? user_id { get; set; }
        /// <summary>
        /// 支付宝交易订单号
        /// </summary>
        /// <returns></returns>
        [Column("ALIPAY_TRADE_NO")]
        public string alipay_trade_no { get; set; }
        /// <summary>
        /// 生成支付的金额(此处做好保留，防止后期套餐金额修改)
        /// </summary>
        /// <returns></returns>
        [Column("TOTAL_AMOUNT")]
        public decimal? total_amount { get; set; }
        /// <summary>
        /// 支付宝交易状态
        /// </summary>
        /// <returns></returns>
        [Column("ALIPAY_STATUS")]
        public string alipay_status { get; set; }
        /// <summary>
        /// 1初级代理  2高级代理  3升级代理
        /// </summary>
        /// <returns></returns>
        [Column("TEMPLATEID")]
        public int? templateid { get; set; }
        /// <summary>
        /// 支付宝交易创建时间
        /// </summary>
        /// <returns></returns>
        [Column("GMT_CREATE")]
        public string gmt_create { get; set; }
        /// <summary>
        /// 支付时间
        /// </summary>
        /// <returns></returns>
        [Column("GMT_PAYMENT")]
        public string gmt_payment { get; set; }
        /// <summary>
        /// 回调时间
        /// </summary>
        /// <returns></returns>
        [Column("NOTIFY_TIME")]
        public string notify_time { get; set; }
        /// <summary>
        /// 记录创建时间
        /// </summary>
        /// <returns></returns>
        [Column("CREATETIME")]
        public DateTime? createtime { get; set; }
        /// <summary>
        /// 记录修改时间
        /// </summary>
        /// <returns></returns>
        [Column("UPDATETIME")]
        public DateTime? updatetime { get; set; }
        /// <summary>
        /// 支付宝账号id
        /// </summary>
        /// <returns></returns>
        [Column("SELLER_ID")]
        public string seller_id { get; set; }
        /// <summary>
        /// 支付宝回调所得
        /// </summary>
        /// <returns></returns>
        [Column("NOTIFY_ID")]
        public string notify_id { get; set; }
        /// <summary>
        /// 交易信息(套餐名称)
        /// </summary>
        /// <returns></returns>
        [Column("SUBJECT")]
        public string subject { get; set; }
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

