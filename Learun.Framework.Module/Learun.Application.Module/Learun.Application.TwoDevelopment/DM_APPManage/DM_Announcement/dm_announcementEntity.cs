using Learun.Util;
using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace Learun.Application.TwoDevelopment.DM_APPManage
{
	public class dm_announcementEntity
	{
		[DatabaseGenerated(DatabaseGeneratedOption.Identity)]
		[Column("ID")]
		public int? id
		{
			get;
			set;
		}

		[Column("A_TITLE")]
		public string a_title
		{
			get;
			set;
		}

		[Column("A_CONTENT")]
		public string a_content
		{
			get;
			set;
		}

		[Column("A_IMAGE")]
		public string a_image
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
