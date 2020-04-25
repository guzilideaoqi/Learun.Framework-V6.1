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
        /// <summary>
        /// 上级用户id
        /// </summary>
        [Column("PARENT_ID")]
        public int parent_id
        {
            get;
            set;
        }
        /// <summary>
        /// 上级用户昵称
        /// </summary>
        [Column("PARENT_NICKNAME")]
        public string parent_nickname {
            get;set;
        }

        [Column("PARTNERS_ID")]
        public int? partners_id
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
        /// 订单数
        /// </summary>
        [Column("ORDERCOUNT")]
        public int ordercount { get; set; }

        /// <summary>
        /// 任务数
        /// </summary>
        [Column("TASKCOUNT")]
        public int taskcount { get; set; }

        /// <summary>
        /// 任务举报数
        /// </summary>
        [Column("TASKREPORTCOUNT")]
        public int taskreportcount { get; set; }


        public void Create()
        {
        }

        public void Modify(int? keyValue)
        {
            id = keyValue;
        }
    }
}
