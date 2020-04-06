using Learun.Util;
using System;
using System.ComponentModel.DataAnnotations.Schema;
namespace Learun.Application.TwoDevelopment.DM_APPManage

{
    /// <summary>
    /// 版 本 Learun-ADMS V6.1.6.0 力软敏捷开发框架
    /// Copyright (c) 2013-2017 上海力软信息技术有限公司
    /// 创 建：超级管理员
    /// 日 期：2020-04-07 07:57
    /// 描 述：pid管理
    /// </summary>
    public class dm_pidEntity 
    {
        #region 实体成员
        /// <summary>
        /// id
        /// </summary>
        /// <returns></returns>
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]        [Column("ID")]
        public int? id { get; set; }
        /// <summary>
        /// pid或渠道id
        /// </summary>
        /// <returns></returns>
        [Column("PID")]
        public string pid { get; set; }
        /// <summary>
        /// pid名称
        /// </summary>
        /// <returns></returns>
        [Column("PIDNAME")]
        public string pidname { get; set; }
        /// <summary>
        /// 完整pid
        /// </summary>
        /// <returns></returns>
        [Column("PIDS")]
        public string pids { get; set; }
        /// <summary>
        /// 类型  1淘宝  2京东  3拼多多
        /// </summary>
        /// <returns></returns>
        [Column("TYPE")]
        public int? type { get; set; }
        /// <summary>
        /// 创建时间
        /// </summary>
        /// <returns></returns>
        [Column("CREATETIME")]
        public DateTime? createtime { get; set; }
        /// <summary>
        /// 使用状态 0未使用  1已使用
        /// </summary>
        /// <returns></returns>
        [Column("USESTATE")]
        public int? usestate { get; set; }
        /// <summary>
        /// 用户ID
        /// </summary>
        /// <returns></returns>
        [Column("USER_ID")]
        public int? user_id { get; set; }
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

