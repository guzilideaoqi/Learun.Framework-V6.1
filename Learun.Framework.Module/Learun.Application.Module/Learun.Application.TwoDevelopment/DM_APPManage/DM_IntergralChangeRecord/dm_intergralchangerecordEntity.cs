using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace Learun.Application.TwoDevelopment.DM_APPManage
{
	public class dm_intergralchangerecordEntity
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

		[Column("GOODID")]
		public int? goodid
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

		[Column("SENDSTATUS")]
		public int? sendstatus
		{
			get;
			set;
		}

		[Column("EXPRESSCODE")]
		public string expresscode
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

		[Column("APPID")]
		public string appid
		{
			get;
			set;
		}

		[Column("PROVINCE")]
		public string province
		{
			get;
			set;
		}

		[Column("CITY")]
		public string city
		{
			get;
			set;
		}

		[Column("DOWN")]
		public string down
		{
			get;
			set;
		}

		[Column("ADDRESS")]
		public string address
		{
			get;
			set;
		}

		[Column("PHONE")]
		public string phone
		{
			get;
			set;
		}

		[Column("USERNAME")]
		public string username
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
			this.updatetime = DateTime.Now;
		}
	}
}
