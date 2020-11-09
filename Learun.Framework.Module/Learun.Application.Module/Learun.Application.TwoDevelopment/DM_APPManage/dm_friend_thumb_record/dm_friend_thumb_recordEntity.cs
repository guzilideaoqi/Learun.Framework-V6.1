using Learun.Util;
using System;
using System.ComponentModel.DataAnnotations.Schema;
namespace Learun.Application.TwoDevelopment.DM_APPManage

{
    /// <summary>
    /// 版 本 Learun-ADMS V6.1.6.0 力软敏捷开发框架
    /// Copyright (c) 2013-2017 上海力软信息技术有限公司
    /// 创 建：超级管理员
    /// 日 期：2020-11-09 10:21
    /// 描 述：点赞记录
    /// </summary>
    public class dm_friend_thumb_recordEntity 
    {
        #region 实体成员
        /// <summary>
        /// 记录id
        /// </summary>
        /// <returns></returns>
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]        [Column("ID")]
        public int? id { get; set; }
        /// <summary>
        /// 用户id
        /// </summary>
        /// <returns></returns>
        [Column("USER_ID")]
        public int? user_id { get; set; }
        /// <summary>
        /// 好友id
        /// </summary>
        /// <returns></returns>
        [Column("FRIEND_ID")]
        public int? friend_id { get; set; }
        /// <summary>
        /// 点赞状态  0未点赞  1已点赞
        /// </summary>
        /// <returns></returns>
        [Column("STATUS")]
        public int? status { get; set; }
        /// <summary>
        /// 创建时间
        /// </summary>
        /// <returns></returns>
        [Column("CREATETIME")]
        public DateTime? createtime { get; set; }
        /// <summary>
        /// 更新时间
        /// </summary>
        /// <returns></returns>
        [Column("UPDATETIME")]
        public DateTime? updatetime { get; set; }

        /// <summary>
        /// 头像列表
        /// </summary>
        [Column("HEADPICLIST")]
        public string headpiclist { get; set; }
        #endregion

        #region 扩展操作
        /// <summary>
        /// 新增调用
        /// </summary>
        public void Create()
        {
            this.createtime = DateTime.Now;
        }
        /// <summary>
        /// 编辑调用
        /// </summary>
        /// <param name="keyValue"></param>
        public void Modify(int? keyValue)
        {
            this.id = keyValue;
            this.updatetime = DateTime.Now;
        }
        #endregion
    }
}

