using Learun.Util;
using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace Learun.Application.TwoDevelopment.DM_APPManage
{
	public class dm_intergralchangegoodEntity
	{
		[DatabaseGenerated(DatabaseGeneratedOption.Identity)]
		[Column("ID")]
		public int? id
		{
			get;
			set;
		}

		[Column("GOODTITLE")]
		public string goodtitle
		{
			get;
			set;
		}

		[Column("GOODREMARK")]
		public string goodremark
		{
			get;
			set;
		}

		[Column("NEEDINTERGRAL")]
		public int? needintergral
		{
			get;
			set;
		}

		[Column("GOODIMAGE")]
		public string goodimage
		{
			get;
			set;
		}

		[Column("GOODPRICE")]
		public decimal? goodprice
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

		[Column("ISEXPRESS")]
		public int? isexpress
		{
			get;
			set;
		}

		[Column("EXPRESSPRICE")]
		public decimal? expressprice
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

		public void Create()
		{
			UserInfo userInfo = LoginUserInfo.Get();
			id = 0;
			createcode = userInfo.nickName;
			createtime = DateTime.Now;
			appid = userInfo.companyId;
		}

		public void Modify(int? keyValue)
		{
			id = keyValue;
		}
	}
}
