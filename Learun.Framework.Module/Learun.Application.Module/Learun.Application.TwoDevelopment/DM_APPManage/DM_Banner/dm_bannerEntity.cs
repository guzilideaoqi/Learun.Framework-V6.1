using Learun.Util;
using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace Learun.Application.TwoDevelopment.DM_APPManage
{
	public class dm_bannerEntity
	{
		[DatabaseGenerated(DatabaseGeneratedOption.Identity)]
		[Column("ID")]
		public int? id
		{
			get;
			set;
		}

		[Column("B_TYPE")]
		public int? b_type
		{
			get;
			set;
		}

		[Column("B_TITLE")]
		public string b_title
		{
			get;
			set;
		}

		[Column("B_IMAGE")]
		public string b_image
		{
			get;
			set;
		}

		[Column("B_PARAM")]
		public string b_param
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

		public void Create()
		{
			UserInfo userInfo = LoginUserInfo.Get();
			createtime = DateTime.Now;
			createcode = userInfo.userId;
			appid = userInfo.companyId;
		}

		public void Modify(int? keyValue)
		{
			id = keyValue;
		}
	}
}
