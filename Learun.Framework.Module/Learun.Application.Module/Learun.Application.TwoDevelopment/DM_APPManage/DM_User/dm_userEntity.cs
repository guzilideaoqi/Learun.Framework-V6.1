using Learun.Cache.Factory;
using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace Learun.Application.TwoDevelopment.DM_APPManage
{
	public class dm_userEntity
	{
		[DatabaseGenerated(DatabaseGeneratedOption.Identity)]
		[Column("ID")]
		public int? id
		{
			get;
			set;
		}

		[Column("REALNAME")]
		public string realname
		{
			get;
			set;
		}

		[Column("IDENTITYCARD")]
		public string identitycard
		{
			get;
			set;
		}

		[Column("ISREAL")]
		public int? isreal
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

		[Column("TOKEN")]
		public string token
		{
			get;
			set;
		}

		[Column("PWD")]
		public string pwd
		{
			get;
			set;
		}

		[Column("NICKNAME")]
		public string nickname
		{
			get;
			set;
		}

		[Column("ACCOUNTPRICE")]
		public decimal? accountprice
		{
			get;
			set;
		}

		[Column("INVITECODE")]
		public string invitecode
		{
			get;
			set;
		}

		[Column("PARTNERS")]
		public int? partners
		{
			get;
			set;
		}

		[Column("PARTNERSSTATUS")]
		public int? partnersstatus
		{
			get;
			set;
		}

		[Column("TB_PID")]
		public string tb_pid
		{
			get;
			set;
		}

		[Column("TB_RELATIONID")]
		public int? tb_relationid
		{
			get;
			set;
		}

		[Column("TB_ORDERRELATIONID")]
		public int? tb_orderrelationid
		{
			get;
			set;
		}

		[Column("JD_PID")]
		public string jd_pid
		{
			get;
			set;
		}

		[Column("PDD_PID")]
		public string pdd_pid
		{
			get;
			set;
		}

		[Column("USERLEVEL")]
		public int? userlevel
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

		[Column("INTEGRAL")]
		public int? integral
		{
			get;
			set;
		}

		public void Create()
		{
			userlevel = 0;
			createtime = DateTime.Now;
			isreal = 0;
			accountprice = default(decimal);
		}

		public void Modify(int? keyValue)
		{
			id = keyValue;
			int? num = keyValue;
			string cacheKey = "UserInfo" + num.ToString();
			CacheFactory.CaChe().Remove(cacheKey, 7L);
		}
	}
}
