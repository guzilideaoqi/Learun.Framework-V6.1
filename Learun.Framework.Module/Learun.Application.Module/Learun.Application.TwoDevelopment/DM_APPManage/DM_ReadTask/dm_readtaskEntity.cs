using Learun.Util;
using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace Learun.Application.TwoDevelopment.DM_APPManage
{
	public class dm_readtaskEntity
	{
		[DatabaseGenerated(DatabaseGeneratedOption.Identity)]
		[Column("ID")]
		public int? id
		{
			get;
			set;
		}

		[Column("TASKTITLE")]
		public string tasktitle
		{
			get;
			set;
		}

		[Column("TASKREMARK")]
		public string taskremark
		{
			get;
			set;
		}

		[Column("TASKIMAGE")]
		public string taskimage
		{
			get;
			set;
		}

		[Column("TASKURL")]
		public string taskurl
		{
			get;
			set;
		}

		[Column("PEOPLECOUNT")]
		public int? peoplecount
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

		[Column("UPDATETIME")]
		public DateTime? updatetime
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

        /// <summary>
        /// 是否为审核模式 1=是 0=否
        /// </summary>
        [Column("ISCHECKMODE")]
        public int? ischeckmode { get; set; }


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
