using Learun.Util;
using System;
using System.ComponentModel.DataAnnotations.Schema;
namespace Learun.Application.TwoDevelopment.DM_APPManage

{
    /// <summary>
    /// 版 本 Learun-ADMS V6.1.6.0 力软敏捷开发框架
    /// Copyright (c) 2013-2017 上海力软信息技术有限公司
    /// 创 建：超级管理员
    /// 日 期：2021-01-05 13:46
    /// 描 述：版本管理
    /// </summary>
    public class dm_versionEntity 
    {
        #region 实体成员
        /// <summary>
        /// 主键
        /// </summary>
        /// <returns></returns>
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]        [Column("ID")]
        public int? id { get; set; }
        /// <summary>
        /// 版本号
        /// </summary>
        /// <returns></returns>
        [Column("APP_VERSION")]
        public string App_Version { get; set; }
        /// <summary>
        /// 下载地址
        /// </summary>
        /// <returns></returns>
        [Column("APP_DOWNLOAD")]
        public string App_DownLoad { get; set; }
        /// <summary>
        /// 更新日志
        /// </summary>
        /// <returns></returns>
        [Column("APP_UPDATEREMARK")]
        public string App_UpdateRemark { get; set; }

        /// <summary>
        /// 应用名称
        /// </summary>
        [Column("APP_NAME")]
        public string App_Name { get; set; }

        /// <summary>
        /// 所属平台  1=安卓  2=ios
        /// </summary>
        /// <returns></returns>
        [Column("APP_PLAFORM")]
        public int? App_Plaform { get; set; }
        /// <summary>
        /// 创建时间
        /// </summary>
        /// <returns></returns>
        [Column("CREATETIME")]
        public DateTime? CreateTime { get; set; }
        /// <summary>
        /// 修改时间
        /// </summary>
        /// <returns></returns>
        [Column("UPDATETIME")]
        public DateTime? UpdateTime { get; set; }
        #endregion

        #region 扩展操作
        /// <summary>
        /// 新增调用
        /// </summary>
        public void Create()
        {
            this.CreateTime = DateTime.Now;
        }
        /// <summary>
        /// 编辑调用
        /// </summary>
        /// <param name="keyValue"></param>
        public void Modify(int? keyValue)
        {
            this.id = keyValue;
            this.UpdateTime = DateTime.Now;
        }
        #endregion
    }
}

