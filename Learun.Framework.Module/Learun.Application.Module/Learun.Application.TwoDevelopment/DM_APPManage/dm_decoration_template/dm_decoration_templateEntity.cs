using Learun.Util;
using System;
using System.ComponentModel.DataAnnotations.Schema;
namespace Learun.Application.TwoDevelopment.DM_APPManage

{
    /// <summary>
    /// 版 本 Learun-ADMS V6.1.6.0 力软敏捷开发框架
    /// Copyright (c) 2013-2017 上海力软信息技术有限公司
    /// 创 建：超级管理员
    /// 日 期：2021-03-03 10:56
    /// 描 述：装修模板
    /// </summary>
    public class dm_decoration_templateEntity 
    {
        #region 实体成员
        /// <summary>
        /// id
        /// </summary>
        /// <returns></returns>
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]        [Column("ID")]
        public int? id { get; set; }
        /// <summary>
        /// template_name
        /// </summary>
        /// <returns></returns>
        [Column("TEMPLATE_NAME")]
        public string template_name { get; set; }
        /// <summary>
        /// 模板备注
        /// </summary>
        /// <returns></returns>
        [Column("TEMPLATE_REMARK")]
        public string template_remark { get; set; }
        /// <summary>
        /// 主色
        /// </summary>
        /// <returns></returns>
        [Column("MAIN_COLOR")]
        public string main_color { get; set; }
        /// <summary>
        /// 辅色
        /// </summary>
        /// <returns></returns>
        [Column("SECONDARY_COLOR")]
        public string secondary_color { get; set; }
        /// <summary>
        /// 模板状态  0=禁用  1=使用中
        /// </summary>
        /// <returns></returns>
        [Column("TEMPLATE_STATUS")]
        public int? template_status { get; set; }
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

