using Learun.Util;
using System;
using System.ComponentModel.DataAnnotations.Schema;
namespace Learun.Application.TwoDevelopment.DM_APPManage

{
    /// <summary>
    /// 版 本 Learun-ADMS V6.1.6.0 力软敏捷开发框架
    /// Copyright (c) 2013-2017 上海力软信息技术有限公司
    /// 创 建：超级管理员
    /// 日 期：2020-04-10 13:52
    /// 描 述：套餐模板
    /// </summary>
    public class dm_alipay_templateEntity 
    {
        #region 实体成员
        /// <summary>
        /// id
        /// </summary>
        /// <returns></returns>
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]        [Column("ID")]
        public int? id { get; set; }
        /// <summary>
        /// 套餐名称
        /// </summary>
        /// <returns></returns>
        [Column("NAME")]
        public string name { get; set; }
        /// <summary>
        /// 套餐原价
        /// </summary>
        /// <returns></returns>
        [Column("GOODPRICE")]
        public decimal goodprice { get; set; }
        /// <summary>
        /// 套餐优惠价
        /// </summary>
        /// <returns></returns>
        [Column("FINISHPRICE")]
        public decimal finishprice { get; set; }
        /// <summary>
        /// 是否为活动状态  0否  1是
        /// </summary>
        /// <returns></returns>
        [Column("ISACTIVE")]
        public int? isactive { get; set; }
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
        /// 平台ID
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

