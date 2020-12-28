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
        /// ����  1���û�ע��  2ǩ��  3������ѽ��� 4�������� 5��Ȧ���˵��޻��  6�һ�ʹ�û���
        /// </summary>
        [Column("TYPE")]
		public int? type
		{
			get;
			set;
		}

        /// <summary>
        /// �����޸ķ���  1�Ӻ�  2����
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
