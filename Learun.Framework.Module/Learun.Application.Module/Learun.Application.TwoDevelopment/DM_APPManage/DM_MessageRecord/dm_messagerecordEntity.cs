using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace Learun.Application.TwoDevelopment.DM_APPManage
{
	public class dm_messagerecordEntity
	{
		[DatabaseGenerated(DatabaseGeneratedOption.Identity)]
		[Column("ID")]
		public int? id
		{
			get;
			set;
		}

		[Column("MESSAGETITLE")]
		public string messagetitle
		{
			get;
			set;
		}

		[Column("MESSAGECONTENT")]
		public string messagecontent
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
        /// 消息类型 1开通代理
        /// </summary>
		[Column("MESSAGETYPE")]
		public int? messagetype
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
        /// 是否已读  0未读  1已读
        /// </summary>
        [Column("ISREAD")]
        public int isread { get; set; }


        public void Create()
		{
		}

		public void Modify(int? keyValue)
		{
			id = keyValue;
		}
	}
}
