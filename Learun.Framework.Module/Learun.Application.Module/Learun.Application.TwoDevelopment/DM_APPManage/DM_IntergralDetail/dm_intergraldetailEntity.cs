using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace Learun.Application.TwoDevelopment.DM_APPManage
{
	public class dm_intergraldetailEntity
	{
		[DatabaseGenerated(DatabaseGeneratedOption.Identity)]
		[Column("ID")]
		public int? id
		{
			get;
			set;
		}

		[Column("CURRENTVALUE")]
		public int? currentvalue
		{
			get;
			set;
		}

		[Column("STEPVALUE")]
		public int? stepvalue
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
        /// 类型  1新用户注册  2签到  3邀请好友奖励 4进度任务 5米圈被人点赞获得  6兑换使用积分
        /// </summary>
        [Column("TYPE")]
		public int? type
		{
			get;
			set;
		}

        /// <summary>
        /// 积分修改符号  1加号  2减号
        /// </summary>
        [Column("PROFITLOSS")]
        public int? profitLoss {
            get;set;
        }

        [Column("REMARK")]
		public string remark
		{
			get;
			set;
		}

		[Column("CREATETIME")]
		public DateTime createtime
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
