using Learun.Util;
using System;
using System.ComponentModel.DataAnnotations.Schema;
namespace Learun.Application.TwoDevelopment.DM_APPManage

{
    /// <summary>
    /// 版 本 Learun-ADMS V6.1.6.0 力软敏捷开发框架
    /// Copyright (c) 2013-2017 上海力软信息技术有限公司
    /// 创 建：超级管理员
    /// 日 期：2020-04-14 17:35
    /// 描 述：提现申请记录
    /// </summary>
    public class dm_apply_cashrecordEntity 
    {
        #region 实体成员
        /// <summary>
        /// 记录id
        /// </summary>
        /// <returns></returns>
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]        [Column("ID")]
        public int? id { get; set; }
        /// <summary>
        /// 用户id
        /// </summary>
        /// <returns></returns>
        [Column("USER_ID")]
        public int? user_id { get; set; }
        /// <summary>
        /// 提现金额
        /// </summary>
        /// <returns></returns>
        [Column("PRICE")]
        public decimal? price { get; set; }

        [Column("CURRENTPRICE")]
        public decimal? currentprice { get; set; }
        /// <summary>
        /// 提现状态  0未审核  1审核成功  2审核驳回
        /// </summary>
        /// <returns></returns>
        [Column("STATUS")]
        public int? status { get; set; }
        /// <summary>
        /// 打款方式  0未打款  1手动打款  2支付宝打款
        /// </summary>
        /// <returns></returns>
        [Column("PAYTYPE")]
        public int? paytype { get; set; }
        /// <summary>
        /// 申请提现备注信息
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

        [Column("CHECKTIME")]
        public DateTime? checktime { get; set; }
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

