using Learun.Util;
using System;
using System.ComponentModel.DataAnnotations.Schema;
namespace Learun.Application.TwoDevelopment.DM_APPManage

{
    /// <summary>
    /// 版 本 Learun-ADMS V6.1.6.0 力软敏捷开发框架
    /// Copyright (c) 2013-2017 上海力软信息技术有限公司
    /// 创 建：超级管理员
    /// 日 期：2021-03-03 10:58
    /// 描 述：模块对应功能
    /// </summary>
    public class dm_decoration_template_module_itemEntity
    {
        #region 实体成员
        /// <summary>
        /// id
        /// </summary>
        /// <returns></returns>
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Column("ID")]
        public int? id { get; set; }
        /// <summary>
        /// 模块名称
        /// </summary>
        /// <returns></returns>
        [Column("MODULE_ITEM_NAME")]
        public string module_item_name { get; set; }
        /// <summary>
        /// 图片地址
        /// </summary>
        /// <returns></returns>
        [Column("MODULE_ITEM_IMAGE")]
        public string module_item_image { get; set; }
        /// <summary>
        /// 模块ID
        /// </summary>
        /// <returns></returns>
        [Column("MODULE_FUN_ID")]
        public int? module_fun_id { get; set; }
        /// <summary>
        /// module_sort
        /// </summary>
        /// <returns></returns>
        [Column("MODULE_SORT")]
        public int? module_sort { get; set; }

        /// <summary>
        /// template_module_id
        /// </summary>
        [Column("TEMPLATE_MODULE_ID")]
        public string template_module_id { get; set; }

        /// <summary>
        /// 模板ID
        /// </summary>
        [Column("TEMPLATE_ID")]
        public int template_id { get; set; }

        /// <summary>
        /// createtime
        /// </summary>
        /// <returns></returns>
        [Column("CREATETIME")]
        public DateTime? createtime { get; set; }
        /// <summary>
        /// updatetime
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

