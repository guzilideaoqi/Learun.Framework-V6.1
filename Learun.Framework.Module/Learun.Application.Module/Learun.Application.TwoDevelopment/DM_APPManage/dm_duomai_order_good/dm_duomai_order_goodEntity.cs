using Learun.Util;
using System;
using System.ComponentModel.DataAnnotations.Schema;
namespace Learun.Application.TwoDevelopment.DM_APPManage

{
    /// <summary>
    /// 版 本 Learun-ADMS V6.1.6.0 力软敏捷开发框架
    /// Copyright (c) 2013-2017 上海力软信息技术有限公司
    /// 创 建：超级管理员
    /// 日 期：2021-03-06 11:16
    /// 描 述：dm_duomai_order商品
    /// </summary>
    public class dm_duomai_order_goodEntity 
    {
        #region 实体成员
        /// <summary>
        /// id
        /// </summary>
        /// <returns></returns>
        [Column("ID")]
        public string id { get; set; }
        /// <summary>
        /// 商品类目，以实际商家平台结果为准
        /// </summary>
        /// <returns></returns>
        [Column("GOODS_CATE")]
        public string goods_cate { get; set; }
        /// <summary>
        /// 商品类目名称，有可能是数字标识、文字说明，以实际商家平台结果为准
        /// </summary>
        /// <returns></returns>
        [Column("GOODS_CATE_NAME")]
        public string goods_cate_name { get; set; }
        /// <summary>
        /// 商品编号
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
        /// 商品单价
        /// </summary>
        /// <returns></returns>
        [Column("GOODS_PRICE")]
        public decimal? goods_price { get; set; }
        /// <summary>
        /// 商品件数
        /// </summary>
        /// <returns></returns>
        [Column("GOODS_TA")]
        public string goods_ta { get; set; }
        /// <summary>
        /// 订单金额，例：1.00
        /// </summary>
        /// <returns></returns>
        [Column("ORDERS_PRICE")]
        public decimal? orders_price { get; set; }
        /// <summary>
        /// 商家平台原始订单状态描述，有可能是英文、中文、数值等，以实际结果为准
        /// </summary>
        /// <returns></returns>
        [Column("ORDER_STATUS")]
        public string order_status { get; set; }
        /// <summary>
        /// 预估站长佣金，非结算站长佣金，例：1.00
        /// </summary>
        /// <returns></returns>
        [Column("ORDER_COMMISSION")]
        public decimal? order_commission { get; set; }
        /// <summary>
        /// 订单号
        /// </summary>
        /// <returns></returns>
        [Column("ORDER_SN")]
        public string order_sn { get; set; }
        /// <summary>
        /// 父订单号/子订单号
        /// </summary>
        /// <returns></returns>
        [Column("PARENT_ORDER_SN")]
        public string parent_order_sn { get; set; }
        /// <summary>
        /// 商品图片地址
        /// </summary>
        /// <returns></returns>
        [Column("GOODS_IMG")]
        public string goods_img { get; set; }
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

