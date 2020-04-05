using Learun.Util;
using System;
using System.ComponentModel.DataAnnotations.Schema;
namespace Learun.Application.TwoDevelopment.Hyg_RobotModule

{
    /// <summary>
    /// 版 本 Learun-ADMS V6.1.6.0 力软敏捷开发框架
    /// Copyright (c) 2013-2017 上海力软信息技术有限公司
    /// 创 建：超级管理员
    /// 日 期：2019-11-26 20:23
    /// 描 述：京东订单详情
    /// </summary>
    public class order_jd_detailEntity 
    {
        #region 实体成员
        /// <summary>
        /// id
        /// </summary>
        /// <returns></returns>
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]        [Column("ID")]
        public int? id { get; set; }
        /// <summary>
        /// orderId
        /// </summary>
        /// <returns></returns>
        [Column("ORDERID")]
        public string orderId { get; set; }
        /// <summary>
        /// 实际计算佣金的金额。订单完成后，会将误扣除的运费券金额更正。如订单完成后发生退款，此金额会更新
        /// </summary>
        /// <returns></returns>
        [Column("ACTUALCOSPRICE")]
        public decimal? actualCosPrice { get; set; }
        /// <summary>
        /// 推客获得的实际佣金（实际计佣金额*佣金比例*最终比例）。如订单完成后发生退款，此金额会更新。
        /// </summary>
        /// <returns></returns>
        [Column("ACTUALFEE")]
        public decimal? actualFee { get; set; }
        /// <summary>
        /// 佣金比例
        /// </summary>
        /// <returns></returns>
        [Column("COMMISSIONRATE")]
        public decimal? commissionRate { get; set; }
        /// <summary>
        /// 预估计佣金额，即用户下单的金额(已扣除优惠券、白条、支付优惠、进口税，未扣除红包和京豆)，有时会误扣除运费券金额，完成结算时会在实际计佣金额中更正。如订单完成前发生退款，此金额也会更新
        /// </summary>
        /// <returns></returns>
        [Column("ESTIMATECOSPRICE")]
        public decimal? estimateCosPrice { get; set; }
        /// <summary>
        /// 推客的预估佣金（预估计佣金额*佣金比例*最终比例），如订单完成前发生退款，此金额也会更新
        /// </summary>
        /// <returns></returns>
        [Column("ESTIMATEFEE")]
        public decimal? estimateFee { get; set; }
        /// <summary>
        /// 最终比例（分成比例+补贴比例）
        /// </summary>
        /// <returns></returns>
        [Column("FINALRATE")]
        public decimal? finalRate { get; set; }
        /// <summary>
        /// 一级类目ID
        /// </summary>
        /// <returns></returns>
        [Column("CID1")]
        public string cid1 { get; set; }
        /// <summary>
        /// 商品售后中数量
        /// </summary>
        /// <returns></returns>
        [Column("FROZENSKUNUM")]
        public int? frozenSkuNum { get; set; }
        /// <summary>
        /// 联盟子站长身份标识，格式：子站长ID_子站长网站ID_子站长推广位ID
        /// </summary>
        /// <returns></returns>
        [Column("PID")]
        public string pid { get; set; }
        /// <summary>
        /// 推广位ID,0代表无推广位
        /// </summary>
        /// <returns></returns>
        [Column("POSITIONID")]
        public int? positionId { get; set; }
        /// <summary>
        /// 商品单价
        /// </summary>
        /// <returns></returns>
        [Column("PRICE")]
        public decimal? price { get; set; }
        /// <summary>
        /// 二级类目ID
        /// </summary>
        /// <returns></returns>
        [Column("CID2")]
        public string cid2 { get; set; }
        /// <summary>
        /// 网站ID，0：无网站
        /// </summary>
        /// <returns></returns>
        [Column("SITEID")]
        public string siteId { get; set; }
        /// <summary>
        /// 商品ID
        /// </summary>
        /// <returns></returns>
        [Column("SKUID")]
        public string skuId { get; set; }
        /// <summary>
        /// 商品名称
        /// </summary>
        /// <returns></returns>
        [Column("SKUNAME")]
        public string skuName { get; set; }
        /// <summary>
        /// 商品数量
        /// </summary>
        /// <returns></returns>
        [Column("SKUNUM")]
        public int? skuNum { get; set; }
        /// <summary>
        /// 商品已退货数量
        /// </summary>
        /// <returns></returns>
        [Column("SKURETURNNUM")]
        public int? skuReturnNum { get; set; }
        /// <summary>
        /// 分成比例
        /// </summary>
        /// <returns></returns>
        [Column("SUBSIDERATE")]
        public decimal? subSideRate { get; set; }
        /// <summary>
        /// 补贴比例
        /// </summary>
        /// <returns></returns>
        [Column("SUBSIDYRATE")]
        public decimal? subsidyRate { get; set; }
        /// <summary>
        /// 三级类目ID
        /// </summary>
        /// <returns></returns>
        [Column("CID3")]
        public string cid3 { get; set; }
        /// <summary>
        /// PID所属母账号平台名称（原第三方服务商来源）
        /// </summary>
        /// <returns></returns>
        [Column("UNIONALIAS")]
        public string unionAlias { get; set; }
        /// <summary>
        /// 联盟标签数据（整型的二进制字符串，目前返回16位：0000000000000001。数据从右向左进行，每一位为1表示符合联盟的标签特征，第1位：红包，第2位：组合推广，第3位：拼购，第5位：有效首次购（0000000000011XXX表示有效首购，最终奖励活动结算金额会结合订单状态判断，以联盟后台对应活动效果数据报表https://union.jd.com/active为准）,第9位：礼金，第10位：联盟礼金，第11位：推客礼金。例如：0000000000000001:红包订单，0000000000000010:组合推广订单，0000000000000100:拼购订单，0000000000011000:有效首购，0000000000000111：红包+组合推广+拼购等）
        /// </summary>
        /// <returns></returns>
        [Column("UNIONTAG")]
        public string unionTag { get; set; }
        /// <summary>
        /// 渠道组 1：1号店，其他：京东
        /// </summary>
        /// <returns></returns>
        [Column("UNIONTRAFFICGROUP")]
        public int? unionTrafficGroup { get; set; }
        /// <summary>
        /// sku维度的有效码（-1：未知,2.无效-拆单,3.无效-取消,4.无效-京东帮帮主订单,5.无效-账号异常,6.无效-赠品类目不返佣,7.无效-校园订单,8.无效-企业订单,9.无效-团购订单,10.无效-开增值税专用发票订单,11.无效-乡村推广员下单,12.无效-自己推广自己下单,13.无效-违规订单,14.无效-来源与备案网址不符,15.待付款,16.已付款,17.已完成,18.已结算（5.9号不再支持结算状态回写展示）,19.无效-佣金比例为0）注：自2018/7/13起，自己推广自己下单已经允许返佣，故12无效码仅针对历史数据有效
        /// </summary>
        /// <returns></returns>
        [Column("VALIDCODE")]
        public int? validCode { get; set; }
        /// <summary>
        /// 子联盟ID(需要联系运营开放白名单才能拿到数据)
        /// </summary>
        /// <returns></returns>
        [Column("SUBUNIONID")]
        public string subUnionId { get; set; }
        /// <summary>
        /// 2：同店；3：跨店
        /// </summary>
        /// <returns></returns>
        [Column("TRACETYPE")]
        public int? traceType { get; set; }
        /// <summary>
        /// 订单行维度预估结算时间（格式：yyyyMMdd） ，0：未结算。订单'预估结算时间'仅供参考。账号未通过资质审核或订单发生售后，会影响订单实际结算时间
        /// </summary>
        /// <returns></returns>
        [Column("PAYMONTH")]
        public string payMonth { get; set; }
        /// <summary>
        /// 商家ID。'订单行维度'
        /// </summary>
        /// <returns></returns>
        [Column("POPID")]
        public string popId { get; set; }
        /// <summary>
        /// 推客生成推广链接时传入的扩展字段（需要联系运营开放白名单才能拿到数据）。'订单行维度'
        /// </summary>
        /// <returns></returns>
        [Column("EXT1")]
        public string ext1 { get; set; }
        /// <summary>
        /// 招商团活动id，正整数，为0时表示无活动
        /// </summary>
        /// <returns></returns>
        [Column("CPACTID")]
        public string cpActId { get; set; }
        /// <summary>
        /// 站长角色，1： 推客、 2： 团长
        /// </summary>
        /// <returns></returns>
        [Column("UNIONROLE")]
        public int? unionRole { get; set; }
        /// <summary>
        /// 礼金批次ID
        /// </summary>
        /// <returns></returns>
        [Column("GIFTCOUPONKEY")]
        public string giftCouponKey { get; set; }
        /// <summary>
        /// 礼金分摊金额
        /// </summary>
        /// <returns></returns>
        [Column("GIFTCOUPONOCSAMOUNT")]
        public decimal? giftCouponOcsAmount { get; set; }
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

