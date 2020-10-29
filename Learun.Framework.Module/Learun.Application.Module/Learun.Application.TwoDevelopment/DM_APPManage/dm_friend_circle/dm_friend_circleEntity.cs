using Learun.Util;
using System;
using System.ComponentModel.DataAnnotations.Schema;
namespace Learun.Application.TwoDevelopment.DM_APPManage

{
    /// <summary>
    /// 版 本 Learun-ADMS V6.1.6.0 力软敏捷开发框架
    /// Copyright (c) 2013-2017 上海力软信息技术有限公司
    /// 创 建：超级管理员
    /// 日 期：2020-10-28 10:06
    /// 描 述：官推文案
    /// </summary>
    public class dm_friend_circleEntity 
    {
        #region 实体成员
        /// <summary>
        /// 朋友圈文案id
        /// </summary>
        /// <returns></returns>
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]        [Column("ID")]
        public int? id { get; set; }
        /// <summary>
        /// 文案内容
        /// </summary>
        /// <returns></returns>
        [Column("T_CONTENT")]
        public string t_content { get; set; }
        /// <summary>
        /// 文案类型  0普通  1官方
        /// </summary>
        /// <returns></returns>
        [Column("T_TYPE")]
        public int? t_type { get; set; }
        /// <summary>
        /// 图片链接数组
        /// </summary>
        /// <returns></returns>
        [Column("T_IMAGES")]
        public string t_images { get; set; }
        /// <summary>
        /// 排序
        /// </summary>
        /// <returns></returns>
        [Column("T_SORT")]
        public int? t_sort { get; set; }

        /// <summary>
        /// 文案状态  0待审核  1已审核  2已下架
        /// </summary>
        [Column("T_STATUS")]
        public int? t_status { get; set; }
        /// <summary>
        /// 创建人
        /// </summary>
        /// <returns></returns>
        [Column("CREATECODE")]
        public string createcode { get; set; }
        /// <summary>
        /// 创建时间
        /// </summary>
        /// <returns></returns>
        [Column("CREATETIME")]
        public DateTime? createtime { get; set; }

        /// <summary>
        /// 站长id
        /// </summary>
        [Column("APPID")]
        public string appid { get; set; }
        #endregion

        #region 扩展操作
        /// <summary>
        /// 新增调用
        /// </summary>
        public void Create()
        {
            this.createtime = DateTime.Now;

            UserInfo userInfo = LoginUserInfo.Get();
            if (!userInfo.IsEmpty()) {
                this.appid = userInfo.companyId;
            }
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

