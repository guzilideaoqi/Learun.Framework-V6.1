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
        /// ��Ϣ���� 1��ͨ���� 2�ӵ���ȡ������ 3�������ͨ��  4���񲵻�֪ͨ
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
        /// �Ƿ��Ѷ�  0δ��  1�Ѷ�
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
