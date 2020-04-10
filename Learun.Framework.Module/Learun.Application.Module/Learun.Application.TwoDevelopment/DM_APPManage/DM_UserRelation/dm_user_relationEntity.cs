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

		[Column("PARENT_ID")]
		public int parent_id
		{
			get;
			set;
		}

		[Column("PARTNERS_ID")]
		public int partners_id
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
		}

		public void Modify(int? keyValue)
		{
			id = keyValue;
		}
	}
}
