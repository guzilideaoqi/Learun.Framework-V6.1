using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace Learun.Application.TwoDevelopment.DM_APPManage
{
    public class dm_user_relationEntity
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Column("ID")]
        public int? id
        {
            get;
            set;
        }

        [Column("USER_ID")]
        public int? user_id
        {
            get;
            set;
        }
        /// <summary>
        /// 上级用户id
        /// </summary>
        [Column("PARENT_ID")]
        public int parent_id
        {
            get;
            set;
        }
        /// <summary>
        /// 上级用户昵称
        /// </summary>
        [Column("PARENT_NICKNAME")]
        public string parent_nickname {
            get;set;
        }

        [Column("PARTNERS_ID")]
        public int? partners_id
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

        [Column("CREATECODE")]
        public string createcode
        {
            get;
            set;
        }

        /// <summary>
        /// 订单数
        /// </summary>
        [Column("ORDERCOUNT")]
        public int ordercount { get; set; }

        /// <summary>
        /// 任务数
        /// </summary>
        [Column("TASKCOUNT")]
        public int taskcount { get; set; }

        /// <summary>
        /// 任务举报数
        /// </summary>
        [Column("TASKREPORTCOUNT")]
        public int taskreportcount { get; set; }

        /// <summary>
        /// 本月佣金预估
        /// </summary>
        [Column("CURRENTMONTHEFFECT")]
        public decimal? CurrentMonthEffect { get; set; }

        /// <summary>
        /// 本月收货预估
        /// </summary>
        /// <returns></returns>
        [Column("CURRENTMONTHRECEIVEEFFECT")]
        public decimal? CurrentMonthReceiveEffect { get; set; }
        /// <summary>
        /// 上月收货预估
        /// </summary>
        /// <returns></returns>
        [Column("UPMONTHRECEIVEEFFECT")]
        public decimal? UpMonthReceiveEffect { get; set; }
        /// <summary>
        /// 今日收货预估
        /// </summary>
        /// <returns></returns>
        [Column("TODAYREVICEEFFECT")]
        public decimal? TodayReviceEffect { get; set; }
        /// <summary>
        /// 昨日收货预估
        /// </summary>
        /// <returns></returns>
        [Column("YESTODAYREVICEEFFECT")]
        public decimal? YesTodayReviceEffect { get; set; }
        /// <summary>
        /// 本月付款金额
        /// </summary>
        /// <returns></returns>
        [Column("CURRENTMONTHPAYMENTPRICE")]
        public decimal? CurrentMonthPayMentPrice { get; set; }
        /// <summary>
        /// 上月付款金额
        /// </summary>
        /// <returns></returns>
        [Column("UPMONTHPAYMENTPRICE")]
        public decimal? UpMonthPayMentPrice { get; set; }
        /// <summary>
        /// 今日付款金额
        /// </summary>
        /// <returns></returns>
        [Column("TODAYPAYMENTPRICE")]
        public decimal? TodayPayMentPrice { get; set; }
        /// <summary>
        /// YesTodayPayMentPrice
        /// </summary>
        /// <returns></returns>
        [Column("YESTODAYPAYMENTPRICE")]
        public decimal? YesTodayPayMentPrice { get; set; }
        /// <summary>
        /// 本月订单数
        /// </summary>
        /// <returns></returns>
        [Column("CURRENTMONTHORDERCOUNT")]
        public int? CurrentMonthOrderCount { get; set; }
        /// <summary>
        /// 上月订单数
        /// </summary>
        /// <returns></returns>
        [Column("UPMONTHORDERCOUNT")]
        public int? UpMonthOrderCount { get; set; }
        /// <summary>
        /// 今日订单数
        /// </summary>
        /// <returns></returns>
        [Column("TODAYORDERCOUNT")]
        public int? TodayOrderCount { get; set; }
        /// <summary>
        /// 昨日订单数
        /// </summary>
        /// <returns></returns>
        [Column("YESTODAYORDERCOUNT")]
        public int? YesTodayOrderCount { get; set; }
        /// <summary>
        /// 本月任务佣金
        /// </summary>
        /// <returns></returns>
        [Column("CURRENTMONTHREVICETASKPRICE")]
        public decimal? CurrentMonthReviceTaskPrice { get; set; }
        /// <summary>
        /// 上月任务佣金
        /// </summary>
        /// <returns></returns>
        [Column("UPMONTHREVICETASKPRICE")]
        public decimal? UpMonthReviceTaskPrice { get; set; }
        /// <summary>
        /// 今日任务佣金
        /// </summary>
        /// <returns></returns>
        [Column("TODAYREVICETASKPRICE")]
        public decimal? TodayReviceTaskPrice { get; set; }
        /// <summary>
        /// 昨日任务佣金
        /// </summary>
        /// <returns></returns>
        [Column("YESTODAYREVICETASKPRICE")]
        public decimal? YesTodayReviceTaskPrice { get; set; }
        /// <summary>
        /// 当月接受任务数
        /// </summary>
        /// <returns></returns>
        [Column("CURRENTMONTHREVICETASKCOUNT")]
        public int? CurrentMonthReviceTaskCount { get; set; }
        /// <summary>
        /// 上月接受任务数
        /// </summary>
        /// <returns></returns>
        [Column("UPMONTHREVICETASKCOUNT")]
        public int? UpMonthReviceTaskCount { get; set; }
        /// <summary>
        /// 今日接受任务数
        /// </summary>
        /// <returns></returns>
        [Column("TODAYREVICETASKCOUNT")]
        public int? TodayReviceTaskCount { get; set; }
        /// <summary>
        /// 昨日接受任务数
        /// </summary>
        /// <returns></returns>
        [Column("YESTODAYREVICETASKCOUNT")]
        public int? YesTodayReviceTaskCount { get; set; }
        /// <summary>
        /// 接受任务总数
        /// </summary>
        /// <returns></returns>
        [Column("REVICETASKCOUNT")]
        public int? ReviceTaskCount { get; set; }

        public void Create()
        {
        }

        public void Modify(int? keyValue)
        {
            id = keyValue;
        }
    }
}
