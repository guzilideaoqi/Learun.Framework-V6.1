using Learun.Util;
using System;
using System.ComponentModel.DataAnnotations.Schema;
namespace Learun.Application.TwoDevelopment.DM_APPManage

{
    /// <summary>
    /// 版 本 Learun-ADMS V6.1.6.0 力软敏捷开发框架
    /// Copyright (c) 2013-2017 上海力软信息技术有限公司
    /// 创 建：超级管理员
    /// 日 期：2020-06-13 15:08
    /// 描 述：直播房间列表
    /// </summary>
    public class dm_meetinglistEntity 
    {
        #region 实体成员
        /// <summary>
        /// id
        /// </summary>
        /// <returns></returns>
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]        [Column("ID")]
        public int? id { get; set; }
        /// <summary>
        /// 房间主题
        /// </summary>
        /// <returns></returns>
        [Column("SUBJECT")]
        public string subject { get; set; }
        /// <summary>
        /// 房间ID
        /// </summary>
        /// <returns></returns>
        [Column("MEETING_ID")]
        public string meeting_id { get; set; }
        /// <summary>
        /// 房间编号
        /// </summary>
        /// <returns></returns>
        [Column("MEETING_CODE")]
        public string meeting_code { get; set; }
        /// <summary>
        /// 密码
        /// </summary>
        /// <returns></returns>
        [Column("PASSWORD")]
        public string password { get; set; }
        /// <summary>
        /// 会议主持人用户ID
        /// </summary>
        /// <returns></returns>
        [Column("HOSTS")]
        public string hosts { get; set; }
        /// <summary>
        /// 邀请的参会者
        /// </summary>
        /// <returns></returns>
        [Column("PARTICIPANTS")]
        public string participants { get; set; }
        /// <summary>
        /// 会议开始时间戳（单位秒）。
        /// </summary>
        /// <returns></returns>
        [Column("START_TIME")]
        public DateTime start_time { get; set; }
        /// <summary>
        /// 会议结束时间戳（单位秒）。
        /// </summary>
        /// <returns></returns>
        [Column("END_TIME")]
        public DateTime end_time { get; set; }
        /// <summary>
        /// 加入会议　URL（点击链接直接加入会议）。
        /// </summary>
        /// <returns></returns>
        [Column("JOIN_URL")]
        public string join_url { get; set; }
        /// <summary>
        /// 会议的配置，可为缺省配置。
        /// </summary>
        /// <returns></returns>
        [Column("SETTINGS")]
        public string settings { get; set; }
        /// <summary>
        /// 加入会议的图片
        /// </summary>
        /// <returns></returns>
        [Column("JOIN_IMAGE")]
        public string join_image { get; set; }
        /// <summary>
        /// 创建时间
        /// </summary>
        /// <returns></returns>
        [Column("CREATETIME")]
        public DateTime? createtime { get; set; }
        /// <summary>
        /// 修改时间
        /// </summary>
        /// <returns></returns>
        [Column("UPDATETIME")]
        public DateTime? updatetime { get; set; }

        /// <summary>
        /// 用户ID
        /// </summary>
        [Column("USER_ID")]
        public int user_id { get; set; }

        /// <summary>
        /// 封面图片
        /// </summary>
        [Column("PAGE_IMAGE")]
        public string page_image { get; set; }
        #endregion

        #region 扩展操作
        /// <summary>
        /// 新增调用
        /// </summary>
        public void Create()
        {
        }
        /// <summary>
        /// 编辑调用
        /// </summary>
        /// <param name="keyValue"></param>
        public void Modify(int? keyValue)
        {
            this.id = keyValue;
        }
        #endregion
    }
}

