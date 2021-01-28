using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace OrderService.Model
{
    public class CommonOrderEntity
    {
        /// <summary>
        /// 记录ID
        /// </summary>
        public string id { get; set; }
        /// <summary>
        /// appid
        /// </summary>
        public string appid { get; set; }
        /// <summary>
        /// 订单编号(父订单编号)
        /// </summary>
        public string order_sn { get; set; }
        /// <summary>
        /// 子订单编号
        /// </summary>
        public string sub_order_sn { get; set; }
        /// <summary>
        /// 商品ID
        /// </summary>
        public string origin_id { get; set; }
        /// <summary>
        /// 大平台类型:1=淘宝和天猫,3=京东,4=拼多多
        /// </summary>
        public int type_big { get; set; }
        /// <summary>
        /// 详细平台类型:1=淘宝,2=天猫,3=京东,4=拼多多
        /// </summary>
        public int type { get; set; }
        /// <summary>
        /// 订单类型1=天猫2=淘宝3=聚划算
        /// </summary>
        public int order_type { get; set; }
        /// <summary>
        /// 商品标题
        /// </summary>
        public string title { get; set; }
        /// <summary>
        /// 订单原始状态
        /// </summary>
        public int? order_status { get; set; }
        /// <summary>
        /// 返佣状态:0=未返,1=已返
        /// </summary>
        public int rebate_status { get; set; }
        /// <summary>
        /// 图片地址
        /// </summary>
        public string image { get; set; }
        /// <summary>
        /// 商品数量
        /// </summary>
        public int? product_num { get; set; }
        /// <summary>
        /// 商品价格
        /// </summary>
        public decimal? product_price { get; set; }
        /// <summary>
        /// 商品实付金额
        /// </summary>
        public decimal? payment_price { get; set; }
        /// <summary>
        /// 结算预估佣金
        /// </summary>
        public decimal? estimated_effect { get; set; }
        /// <summary>
        /// 收入比率
        /// </summary>
        public decimal? income_ratio { get; set; }
        /// <summary>
        /// 付款预估佣金
        /// </summary>
        public decimal? estimated_income { get; set; }
        /// <summary>
        /// 佣金比例
        /// </summary>
        public decimal? commission_rate { get; set; }
        /// <summary>
        /// 实际结算佣金金额
        /// </summary>
        public decimal? commission_amount { get; set; }
        /// <summary>
        /// 补贴比例
        /// </summary>
        public string subsidy_ratio { get; set; }
        /// <summary>
        /// 补贴金额
        /// </summary>
        public decimal? subsidy_amount { get; set; }
        /// <summary>
        /// 补贴类型
        /// </summary>
        public string subsidy_type { get; set; }
        /// <summary>
        /// 订单创建时间
        /// </summary>
        public string order_createtime { get; set; }
        /// <summary>
        /// 订单结算时间
        /// </summary>
        public string order_settlement_at { get; set; }
        /// <summary>
        /// 订单付款时间
        /// </summary>
        public string order_pay_time { get; set; }
        /// <summary>
        /// 订单成团时间
        /// </summary>
        public string order_group_success_time { get; set; }
        /// <summary>
        /// 记录创建时间
        /// </summary>
        public DateTime createtime { get; set; }
        /// <summary>
        /// 订单修改时间
        /// </summary>
        public DateTime updatetime { get; set; }
        /// <summary>
        /// 店铺名称
        /// </summary>
        public string shopname { get; set; }
        /// <summary>
        /// 类目名称
        /// </summary>
        public string category_name { get; set; }
        /// <summary>
        /// 来源媒体名称
        /// </summary>
        public string media_name { get; set; }
        /// <summary>
        /// 媒体ID
        /// </summary>
        public string media_id { get; set; }
        /// <summary>
        /// 广告位名称
        /// </summary>
        public string pid_name { get; set; }
        /// <summary>
        /// 广告位ID
        /// </summary>
        public string pid { get; set; }
        /// <summary>
        /// 渠道ID
        /// </summary>
        public string relation_id { get; set; }
        /// <summary>
        /// 会员id
        /// </summary>
        public string special_id { get; set; }
        /// <summary>
        /// 维权状态
        /// </summary>
        public int? protection_status { get; set; }
        /// <summary>
        /// 订单来源  1同步服务  2后台
        /// </summary>
        public int insert_type { get; set; }
        /// <summary>
        /// 本站订单归类状态: 0=未处理,1=付款,2=订单结算,3=失效,4=订单已返利
        /// </summary>
        public int order_type_new { get; set; }
        /// <summary>
        /// 订单创建日期
        /// </summary>
        public int order_create_date { get; set; }
        /// <summary>
        /// 订单创建月份
        /// </summary>
        public int order_create_month { get; set; }
        /// <summary>
        /// 订单确认收货日期
        /// </summary>
        public int order_receive_date { get; set; }
        /// <summary>
        /// 订单确认收货月份
        /// </summary>
        public int order_receive_month { get; set; }
    }
}
