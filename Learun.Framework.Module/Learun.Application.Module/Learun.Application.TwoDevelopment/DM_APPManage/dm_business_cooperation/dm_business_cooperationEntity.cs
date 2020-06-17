using Learun.Util;
using System;
using System.ComponentModel.DataAnnotations.Schema;
namespace Learun.Application.TwoDevelopment.DM_APPManage

{
    /// <summary>
    /// 版 本 Learun-ADMS V6.1.6.0 力软敏捷开发框架
    /// Copyright (c) 2013-2017 上海力软信息技术有限公司
    /// 创 建：超级管理员
    /// 日 期：2020-06-17 16:20
    /// 描 述：商务合作
    /// </summary>
    public class dm_business_cooperationEntity 
    {
        #region 实体成员
        /// <summary>
        /// id
        /// </summary>
        /// <returns></returns>
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]        [Column("ID")]
        public int? id { get; set; }
        /// <summary>
        /// user_id
        /// </summary>
        /// <returns></returns>
        [Column("USER_ID")]
        public int? user_id { get; set; }
        /// <summary>
        /// 公司名称
        /// </summary>
        /// <returns></returns>
        [Column("COMPANYNAME")]
        public string companyname { get; set; }
        /// <summary>
        /// 用户名称
        /// </summary>
        /// <returns></returns>
        [Column("USERNAME")]
        public string username { get; set; }
        /// <summary>
        /// 部门名称
        /// </summary>
        /// <returns></returns>
        [Column("DEPARTMENTNAME")]
        public string departmentname { get; set; }
        /// <summary>
        /// 联系电话
        /// </summary>
        /// <returns></returns>
        [Column("LINK_PHONE")]
        public string link_phone { get; set; }
        /// <summary>
        /// 描述信息
        /// </summary>
        /// <returns></returns>
        [Column("REMARK")]
        public string remark { get; set; }
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

