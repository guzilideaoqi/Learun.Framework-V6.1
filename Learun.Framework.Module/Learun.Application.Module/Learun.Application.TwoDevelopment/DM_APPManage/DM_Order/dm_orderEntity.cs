using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace Learun.Application.TwoDevelopment.DM_APPManage
{
	public class dm_orderEntity
	{
		[Column("ID")]
		public string id
		{
			get;
			set;
		}

		[Column("APPID")]
		public string appid
		{
			get;
			set;
		}

		[Column("ORDER_SN")]
		public string order_sn
		{
			get;
			set;
		}

		[Column("SUB_ORDER_SN")]
		public string sub_order_sn
		{
			get;
			set;
		}

		[Column("ORIGIN_ID")]
		public string origin_id
		{
			get;
			set;
		}
        /// <summary>
        /// 大平台类型:1=淘宝和天猫,3=京东,4=拼多多
        /// </summary>
		[Column("TYPE_BIG")]
		public int? type_big
		{
			get;
			set;
		}

		[Column("TYPE")]
		public int? type
		{
			get;
			set;
		}

		[Column("ORDER_TYPE")]
		public int? order_type
		{
			get;
			set;
		}

		[Column("TITLE")]
		public string title
		{
			get;
			set;
		}

		[Column("ORDER_STATUS")]
		public int? order_status
		{
			get;
			set;
		}

		[Column("REBATE_STATUS")]
		public int? rebate_status
		{
			get;
			set;
		}

		[Column("IMAGE")]
		public string image
		{
			get;
			set;
		}

		[Column("PRODUCT_NUM")]
		public int? product_num
		{
			get;
			set;
		}

		[Column("PRODUCT_PRICE")]
		public decimal? product_price
		{
			get;
			set;
		}

		[Column("PAYMENT_PRICE")]
		public decimal? payment_price
		{
			get;
			set;
		}

		[Column("ESTIMATED_EFFECT")]
		public decimal estimated_effect
		{
			get;
			set;
		}

		[Column("INCOME_RATIO")]
		public decimal? income_ratio
		{
			get;
			set;
		}

		[Column("ESTIMATED_INCOME")]
		public decimal? estimated_income
		{
			get;
			set;
		}

		[Column("COMMISSION_RATE")]
		public decimal? commission_rate
		{
			get;
			set;
		}

		[Column("COMMISSION_AMOUNT")]
		public decimal? commission_amount
		{
			get;
			set;
		}

		[Column("SUBSIDY_RATIO")]
		public decimal? subsidy_ratio
		{
			get;
			set;
		}

		[Column("SUBSIDY_AMOUNT")]
		public decimal? subsidy_amount
		{
			get;
			set;
		}

		[Column("SUBSIDY_TYPE")]
		public int? subsidy_type
		{
			get;
			set;
		}

		[Column("ORDER_CREATETIME")]
		public DateTime? order_createtime
		{
			get;
			set;
		}

		[Column("ORDER_SETTLEMENT_AT")]
		public DateTime? order_settlement_at
		{
			get;
			set;
		}

		[Column("ORDER_PAY_TIME")]
		public DateTime? order_pay_time
		{
			get;
			set;
		}

		[Column("ORDER_GROUP_SUCCESS_TIME")]
		public DateTime? order_group_success_time
		{
			get;
			set;
		}

		[Column("CREATETIME")]
		public DateTime? createtime
		{
			get;
			set;
		}

		[Column("UPDATETIME")]
		public DateTime? updatetime
		{
			get;
			set;
		}

		[Column("SHOPNAME")]
		public string shopname
		{
			get;
			set;
		}

		[Column("CATEGORY_NAME")]
		public string category_name
		{
			get;
			set;
		}

		[Column("MEDIA_NAME")]
		public string media_name
		{
			get;
			set;
		}

		[Column("MEDIA_ID")]
		public string media_id
		{
			get;
			set;
		}

		[Column("PID_NAME")]
		public string pid_name
		{
			get;
			set;
		}

		[Column("PID")]
		public string pid
		{
			get;
			set;
		}

		[Column("RELATION_ID")]
		public string relation_id
		{
			get;
			set;
		}

		[Column("SPECIAL_ID")]
		public string special_id
		{
			get;
			set;
		}

		[Column("PROTECTION_STATUS")]
		public int? protection_status
		{
			get;
			set;
		}

		[Column("INSERT_TYPE")]
		public int? insert_type
		{
			get;
			set;
		}
        /// <summary>
        /// 本站订单归类状态: 0=未处理,1=付款,2=收货未结,3=失效,4=结算至余额
        /// </summary>
		[Column("ORDER_TYPE_NEW")]
		public int? order_type_new
		{
			get;
			set;
		}

		[Column("ORDER_CREATE_DATE")]
		public int? order_create_date
		{
			get;
			set;
		}

		[Column("ORDER_CREATE_MONTH")]
		public int? order_create_month
		{
			get;
			set;
		}

		[Column("ORDER_RECEIVE_DATE")]
		public int? order_receive_date
		{
			get;
			set;
		}

		[Column("ORDER_RECEIVE_MONTH")]
		public int? order_receive_month
		{
			get;
			set;
		}

		[Column("USERID")]
		public int? userid
		{
			get;
			set;
		}

		public void Create()
		{
			id = Guid.NewGuid().ToString();
		}

		public void Modify(string keyValue)
		{
			id = keyValue;
		}
	}
}
