using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace Learun.Application.TwoDevelopment.DM_APPManage
{
	public class dm_accountdetailEntity
	{
		[DatabaseGenerated(DatabaseGeneratedOption.Identity)]
		[Column("ID")]
		public int? id
		{
			get;
			set;
		}

		[Column("CURRENTVALUE")]
		public decimal? currentvalue
		{
			get;
			set;
		}

		[Column("STEPVALUE")]
		public decimal? stepvalue
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

		[Column("TITLE")]
		public string title
		{
			get;
			set;
		}

        /// <summary>
        /// 账户余额变更来源 类型 1订单佣金  2一级粉丝订单  3二级粉丝订单  4团队订单  5下级团队订单  6下级开通代理  7下下级开通代理 8团队成员  9下级团队成员 10进度任务增加 11申请提现 12发布任务 13取消发布任务 14自己做任务  15下级做任务  16下下级做任务 17团队成员做任务  18下级团队成员做任务 19 余额手动调整增加  20余额手动调整减少  21充值  22三级开通代理
        /// </summary>
		[Column("TYPE")]
		public int? type
		{
			get;
			set;
		}

		[Column("REMARK")]
		public string remark
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
        /// 1加号  2减号
        /// </summary>
        [Column("PROFITLOSS")]
        public int profitLoss { get; set; }


        public void Create()
		{
			this.createtime = DateTime.Now;
		}

		public void Modify(int? keyValue)
		{
			id = keyValue;
		}
	}
}
