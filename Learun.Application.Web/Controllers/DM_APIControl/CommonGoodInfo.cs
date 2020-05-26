using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Learun.Application.Web.Controllers.DM_APIControl
{
    public class CommonGoodInfo
    {
        /// <summary>
        /// 商品ID
        /// </summary>
        public string skuid { get; set; }
        /// <summary>
        /// 商品标题
        /// </summary>
        public string title { get; set; }
        /// <summary>
        /// 优惠券金额
        /// </summary>
        public decimal coupon_price { get; set; }
        /// <summary>
        /// 商品原件
        /// </summary>
        public decimal origin_price { get; set; }
        /// <summary>
        /// 券后价
        /// </summary>
        public decimal coupon_after_price { get; set; }
        /// <summary>
        /// 平台  1淘宝 2京东  3拼多多
        /// </summary>
        public int plaform { get; set; }

        public string[] images { get; set; }
        /// <summary>
        /// 优惠券开始时间
        /// </summary>
        public string coupon_start_time { get; set; }
        /// <summary>
        /// 优惠券结束时间
        /// </summary>
        public string coupon_end_time { get; set; }
        /// <summary>
        /// 店铺id
        /// </summary>
        public string shopId { get; set; }
        /// <summary>
        /// 店铺名称
        /// </summary>
        public string shopName { get; set; }
        /// <summary>
        /// 详情图片
        /// </summary>
        public string[] detail_images { get; set; }
        /// <summary>
        /// 月销量
        /// </summary>
        public int month_sales { get; set; }

        /// <summary>
        /// 初级佣金金额
        /// </summary>
        public decimal LevelCommission { get; set; }
        /// <summary>
        /// 高级佣金金额
        /// </summary>
        public decimal SuperCommission { get; set; }
    }
}