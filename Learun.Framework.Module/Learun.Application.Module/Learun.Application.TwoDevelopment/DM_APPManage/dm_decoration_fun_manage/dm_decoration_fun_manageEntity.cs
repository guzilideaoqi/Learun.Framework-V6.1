using Learun.Util;
using System;
using System.ComponentModel.DataAnnotations.Schema;
namespace Learun.Application.TwoDevelopment.DM_APPManage

{
    /// <summary>
    /// 版 本 Learun-ADMS V6.1.6.0 力软敏捷开发框架
    /// Copyright (c) 2013-2017 上海力软信息技术有限公司
    /// 创 建：超级管理员
    /// 日 期：2021-03-03 10:53
    /// 描 述：装修模块分类
    /// </summary>
    public class dm_decoration_fun_manageEntity 
    {
        #region 实体成员
        /// <summary>
        /// id
        /// </summary>
        /// <returns></returns>
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]        [Column("ID")]
        public int? id { get; set; }
        /// <summary>
        /// 功能名称
        /// </summary>
        /// <returns></returns>
        [Column("FUN_NAME")]
        public string fun_name { get; set; }
        /// <summary>
        /// 功能类型  1=原生  2=多麦  3=站内H5 4=淘宝官方活动
        /// </summary>
        /// <returns></returns>
        [Column("FUN_TYPE")]
        public int? fun_type { get; set; }
        /// <summary>
        /// 功能对应参数值(对应跳转)
        /// </summary>
        /// <returns></returns>
        [Column("FUN_PARAM")]
        public string fun_param { get; set; }
        /// <summary>
        /// 功能描述
        /// </summary>
        /// <returns></returns>
        [Column("FUN_REMARK")]
        public string fun_remark { get; set; }
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

