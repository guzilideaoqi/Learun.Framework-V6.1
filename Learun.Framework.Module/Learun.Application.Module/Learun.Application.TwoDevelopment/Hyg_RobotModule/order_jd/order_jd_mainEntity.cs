using Learun.Util;
using System;
using System.ComponentModel.DataAnnotations.Schema;
namespace Learun.Application.TwoDevelopment.Hyg_RobotModule

{
    /// <summary>
    /// 版 本 Learun-ADMS V6.1.6.0 力软敏捷开发框架
    /// Copyright (c) 2013-2017 上海力软信息技术有限公司
    /// 创 建：超级管理员
    /// 日 期：2019-11-26 20:21
    /// 描 述：京东订单主表
    /// </summary>
    public class order_jd_mainEntity 
    {
        #region 实体成员
        /// <summary>
        /// 订单完成时间(时间戳，毫秒)
        /// </summary>
        /// <returns></returns>
        [Column("FINISHTIME")]
        public string finishTime { get; set; }
        /// <summary>
        /// 下单设备(1:PC,2:无线)
        /// </summary>
        /// <returns></returns>
        [Column("ORDEREMT")]
        public int? orderEmt { get; set; }
        /// <summary>
        /// 订单ID
        /// </summary>
        /// <returns></returns>
        [Column("ORDERID")]
        public string orderId { get; set; }
        /// <summary>
        /// 下单时间(时间戳，毫秒)
        /// </summary>
        /// <returns></returns>
        [Column("ORDERTIME")]
        public string orderTime { get; set; }
        /// <summary>
        /// 父单的订单ID，仅当发生订单拆分时返回， 0：未拆分，有值则表示此订单为子订单
        /// </summary>
        /// <returns></returns>
        [Column("PARENTID")]
        public string parentId { get; set; }
        /// <summary>
        /// 订单维度预估结算时间,不建议使用，可以用订单行sku维度paymonth字段参考，（格式：yyyyMMdd），0：未结算，订单'预估结算时间'仅供参考。账号未通过资质审核或订单发生售后，会影响订单实际结算时间。
        /// </summary>
        /// <returns></returns>
        [Column("PAYMONTH")]
        public string payMonth { get; set; }
        /// <summary>
        /// 下单用户是否为PLUS会员 0：否，1：是
        /// </summary>
        /// <returns></returns>
        [Column("PLUS")]
        public int? plus { get; set; }
        /// <summary>
        /// 订单维度商家ID，不建议使用，可以用订单行sku维度popId参考
        /// </summary>
        /// <returns></returns>
        [Column("POPID")]
        public string popId { get; set; }
        /// <summary>
        /// 推客的联盟ID
        /// </summary>
        /// <returns></returns>
        [Column("UNIONID")]
        public string unionId { get; set; }
        /// <summary>
        /// 订单维度的推客生成推广链接时传入的扩展字段，不建议使用，可以用订单行sku维度ext1参考,（需要联系运营开放白名单才能拿到数据）
        /// </summary>
        /// <returns></returns>
        [Column("EXT1")]
        public string ext1 { get; set; }
        /// <summary>
        /// 订单维度的有效码，不建议使用，可以用订单行sku维度validCode参考,（-1：未知,2.无效-拆单,3.无效-取消,4.无效-京东帮帮主订单,5.无效-账号异常,6.无效-赠品类目不返佣,7.无效-校园订单,8.无效-企业订单,9.无效-团购订单,10.无效-开增值税专用发票订单,11.无效-乡村推广员下单,12.无效-自己推广自己下单,13.无效-违规订单,14.无效-来源与备案网址不符,15.待付款,16.已付款,17.已完成,18.已结算（5.9号不再支持结算状态回写展示）,19.无效-佣金比例为0）注：自2018/7/13起，自己推广自己下单已经允许返佣，故12无效码仅针对历史数据有效
        /// </summary>
        /// <returns></returns>
        [Column("VALIDCODE")]
        public int? validCode { get; set; }
        #endregion

        #region 扩展操作
        /// <summary>
        /// 新增调用
        /// </summary>
        public void Create()
        {
            this.orderId = Guid.NewGuid().ToString();
        }
        /// <summary>
        /// 编辑调用
        /// </summary>
        /// <param name="keyValue"></param>
        public void Modify(string keyValue)
        {
            this.orderId = keyValue;
        }
        #endregion
    }
}

