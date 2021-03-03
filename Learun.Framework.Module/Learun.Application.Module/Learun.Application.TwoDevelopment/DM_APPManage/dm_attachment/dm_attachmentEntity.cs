using Learun.Util;
using System;
using System.ComponentModel.DataAnnotations.Schema;
namespace Learun.Application.TwoDevelopment.DM_APPManage

{
    /// <summary>
    /// 版 本 Learun-ADMS V6.1.6.0 力软敏捷开发框架
    /// Copyright (c) 2013-2017 上海力软信息技术有限公司
    /// 创 建：超级管理员
    /// 日 期：2021-03-03 10:49
    /// 描 述：附件管理
    /// </summary>
    public class dm_attachmentEntity 
    {
        #region 实体成员
        /// <summary>
        /// id
        /// </summary>
        /// <returns></returns>
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]        [Column("ID")]
        public int? id { get; set; }
        /// <summary>
        /// 文件地址
        /// </summary>
        /// <returns></returns>
        [Column("FILE_URL")]
        public string file_url { get; set; }
        /// <summary>
        /// file_size
        /// </summary>
        /// <returns></returns>
        [Column("FILE_SIZE")]
        public string file_size { get; set; }
        /// <summary>
        /// 文件名称
        /// </summary>
        /// <returns></returns>
        [Column("FILE_NAME")]
        public string file_name { get; set; }
        /// <summary>
        /// file_custom_name
        /// </summary>
        /// <returns></returns>
        [Column("FILE_CUSTOM_NAME")]
        public string file_custom_name { get; set; }
        /// <summary>
        /// 文件类型  1=图片  2=视频  3=文件  4=其他
        /// </summary>
        /// <returns></returns>
        [Column("FILE_TYPE")]
        public int? file_type { get; set; }
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

