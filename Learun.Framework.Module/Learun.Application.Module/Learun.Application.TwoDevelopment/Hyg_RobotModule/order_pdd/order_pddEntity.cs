using Learun.Util;
using System;
using System.ComponentModel.DataAnnotations.Schema;
namespace Learun.Application.TwoDevelopment.Hyg_RobotModule

{
    /// <summary>
    /// 版 本 Learun-ADMS V6.1.6.0 力软敏捷开发框架
    /// Copyright (c) 2013-2017 上海力软信息技术有限公司
    /// 创 建：超级管理员
    /// 日 期：2019-11-22 22:10
    /// 描 述：拼多多订单列表
    /// </summary>
    public class order_pddEntity 
    {
        #region 实体成员
        /// <summary>
        /// 订单编号
        /// </summary>
        /// <returns></returns>
        [Column("ORDER_SN")]
        public string order_sn { get; set; }
        /// <summary>
        /// 商品id
        /// </summary>
        /// <returns></returns>
        [Column("GOODS_ID")]
        public string goods_id { get; set; }
        /// <summary>
        /// 商品名称
        /// </summary>
        /// <returns></returns>
        [Column("GOODS_NAME")]
        public string goods_name { get; set; }
        /// <summary>
        /// 商品缩略图
        /// </summary>
        /// <returns></returns>
        [Column("GOODS_THUMBNAIL_URL")]
        public string goods_thumbnail_url { get; set; }
        /// <summary>
        /// 商品数量
        /// </summary>
        /// <returns></returns>
        [Column("GOODS_QUANTITY")]
        public int? goods_quantity { get; set; }
        /// <summary>
        /// 商品价格（分）
        /// </summary>
        /// <returns></returns>
        [Column("GOODS_PRICE")]
        public string goods_price { get; set; }
        /// <summary>
        /// 订单价格（分）
        /// </summary>
        /// <returns></returns>
        [Column("ORDER_AMOUNT")]
        public string order_amount { get; set; }
        /// <summary>
        /// 佣金比例 千分比
        /// </summary>
        /// <returns></returns>
        [Column("PROMOTION_RATE")]
        public string promotion_rate { get; set; }
        /// <summary>
        /// 佣金（分）
        /// </summary>
        /// <returns></returns>
        [Column("PROMOTION_AMOUNT")]
        public string promotion_amount { get; set; }
        /// <summary>
        /// 结算批次号
        /// </summary>
        /// <returns></returns>
        [Column("BATCH_NO")]
        public string batch_no { get; set; }
        /// <summary>
        /// 订单状态
        /// </summary>
        /// <returns></returns>
        [Column("ORDER_STATUS")]
        public int? order_status { get; set; }
        /// <summary>
        /// 订单状态描述（ -1 未支付; 0-已支付；1-已成团；2-确认收货；3-审核成功；4-审核失败（不可提现）；5-已经结算；8-非多多进宝商品（无佣金订单）;10-已处罚）
        /// </summary>
        /// <returns></returns>
        [Column("ORDER_STATUS_DESC")]
        public string order_status_desc { get; set; }
        /// <summary>
        /// 订单创建时间（UNIX时间戳）
        /// </summary>
        /// <returns></returns>
        [Column("ORDER_CREATE_TIME")]
        public int? order_create_time { get; set; }
        /// <summary>
        /// 订单支付时间（UNIX时间戳）
        /// </summary>
        /// <returns></returns>
        [Column("ORDER_PAY_TIME")]
        public string order_pay_time { get; set; }
        /// <summary>
        /// 订单成团时间（UNIX时间戳）
        /// </summary>
        /// <returns></returns>
        [Column("ORDER_GROUP_SUCCESS_TIME")]
        public string order_group_success_time { get; set; }
        /// <summary>
        /// 订单确认收货时间（UNIX时间戳）
        /// </summary>
        /// <returns></returns>
        [Column("ORDER_RECEIVE_TIME")]
        public string order_receive_time { get; set; }
        /// <summary>
        /// 订单审核时间（UNIX时间戳）
        /// </summary>
        /// <returns></returns>
        [Column("ORDER_VERIFY_TIME")]
        public string order_verify_time { get; set; }
        /// <summary>
        /// 订单结算时间（UNIX时间戳）
        /// </summary>
        /// <returns></returns>
        [Column("ORDER_SETTLE_TIME")]
        public string order_settle_time { get; set; }
        /// <summary>
        /// 订单最后更新时间（UNIX时间戳）
        /// </summary>
        /// <returns></returns>
        [Column("ORDER_MODIFY_AT")]
        public string order_modify_at { get; set; }
        /// <summary>
        /// 订单类型：0：领券页面， 1： 红包页， 2：领券页， 3： 题页
        /// </summary>
        /// <returns></returns>
        [Column("TYPE")]
        public int? type { get; set; }
        /// <summary>
        /// 成团编号
        /// </summary>
        /// <returns></returns>
        [Column("GROUP_ID")]
        public string group_id { get; set; }
        /// <summary>
        /// 多多客工具id
        /// </summary>
        /// <returns></returns>
        [Column("AUTH_DUO_ID")]
        public string auth_duo_id { get; set; }
        /// <summary>
        /// 招商多多客id
        /// </summary>
        /// <returns></returns>
        [Column("ZS_DUO_ID")]
        public string zs_duo_id { get; set; }
        /// <summary>
        /// 自定义参数
        /// </summary>
        /// <returns></returns>
        [Column("CUSTOM_PARAMETERS")]
        public string custom_parameters { get; set; }
        /// <summary>
        /// CPS_Sign
        /// </summary>
        /// <returns></returns>
        [Column("CPS_SIGN")]
        public string cps_sign { get; set; }
        /// <summary>
        /// 链接最后一次生产时间
        /// </summary>
        /// <returns></returns>
        [Column("URL_LAST_GENERATE_TIME")]
        public string url_last_generate_time { get; set; }
        /// <summary>
        /// 打点时间
        /// </summary>
        /// <returns></returns>
        [Column("POINT_TIME")]
        public string point_time { get; set; }
        /// <summary>
        /// 售后状态：0：无，1：售后中，2：售后完成
        /// </summary>
        /// <returns></returns>
        [Column("RETURN_STATUS")]
        public int? return_status { get; set; }
        /// <summary>
        /// 推广位id
        /// </summary>
        /// <returns></returns>
        [Column("P_ID")]
        public string p_id { get; set; }
        /// <summary>
        /// 是否是 cpa 新用户，1表示是，0表示否
        /// </summary>
        /// <returns></returns>
        [Column("CPA_NEW")]
        public int? cpa_new { get; set; }
        #endregion

        #region 扩展操作
        /// <summary>
        /// 新增调用
        /// </summary>
        public void Create()
        {
            this.order_sn = Guid.NewGuid().ToString();
        }
        /// <summary>
        /// 编辑调用
        /// </summary>
        /// <param name="keyValue"></param>
        public void Modify(string keyValue)
        {
            this.order_sn = keyValue;
        }
        #endregion
    }
}

