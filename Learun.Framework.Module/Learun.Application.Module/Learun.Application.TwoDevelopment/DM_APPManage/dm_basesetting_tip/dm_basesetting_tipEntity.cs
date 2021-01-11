using Learun.Util;
using System;
using System.ComponentModel.DataAnnotations.Schema;
namespace Learun.Application.TwoDevelopment.DM_APPManage

{
    /// <summary>
    /// 版 本 Learun-ADMS V6.1.6.0 力软敏捷开发框架
    /// Copyright (c) 2013-2017 上海力软信息技术有限公司
    /// 创 建：超级管理员
    /// 日 期：2021-01-11 11:36
    /// 描 述：明细说明
    /// </summary>
    public class dm_basesetting_tipEntity 
    {
        #region 实体成员
        /// <summary>
        /// id
        /// </summary>
        /// <returns></returns>
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]        [Column("ID")]
        public int? id { get; set; }
        /// <summary>
        /// 站长id
        /// </summary>
        /// <returns></returns>
        [Column("APPID")]
        public string appid { get; set; }
        /// <summary>
        /// task_do
        /// </summary>
        /// <returns></returns>
        [Column("TASK_DO_TIP")]
        public string task_do_tip { get; set; }
        /// <summary>
        /// 任务(一级)
        /// </summary>
        /// <returns></returns>
        [Column("TASK_ONE_TIP")]
        public string task_one_tip { get; set; }
        /// <summary>
        /// 任务(二级)
        /// </summary>
        /// <returns></returns>
        [Column("TASK_TWO_TIP")]
        public string task_two_tip { get; set; }
        /// <summary>
        /// 任务(一级团队)
        /// </summary>
        /// <returns></returns>
        [Column("TASK_PARNERS_ONE_TIP")]
        public string task_parners_one_tip { get; set; }
        /// <summary>
        /// 任务(二级团队)
        /// </summary>
        /// <returns></returns>
        [Column("TASK_PARNERS_TWO_TIP")]
        public string task_parners_two_tip { get; set; }
        /// <summary>
        /// 购物人提示
        /// </summary>
        /// <returns></returns>
        [Column("SHOP_PAY_TIP")]
        public string shop_pay_tip { get; set; }
        /// <summary>
        /// 购物(一级)
        /// </summary>
        /// <returns></returns>
        [Column("SHOP_ONE_TIP")]
        public string shop_one_tip { get; set; }
        /// <summary>
        /// 购物(二级)
        /// </summary>
        /// <returns></returns>
        [Column("SHOP_TWO_TIP")]
        public string shop_two_tip { get; set; }
        /// <summary>
        /// 购物(一级团队)
        /// </summary>
        /// <returns></returns>
        [Column("SHOP_PARNERS_ONE_TIP")]
        public string shop_parners_one_tip { get; set; }
        /// <summary>
        /// 购物(二级团队)
        /// </summary>
        /// <returns></returns>
        [Column("SHOP_PARNERS_TWO_TIP")]
        public string shop_parners_two_tip { get; set; }
        /// <summary>
        /// 开通代理(一级)
        /// </summary>
        /// <returns></returns>
        [Column("OPENGENT_ONE_TIP")]
        public string opengent_one_tip { get; set; }
        /// <summary>
        /// 开通代理(二级)
        /// </summary>
        /// <returns></returns>
        [Column("OPENGENT_TWO_TIP")]
        public string opengent_two_tip { get; set; }
        /// <summary>
        /// 开通代理(三级)
        /// </summary>
        /// <returns></returns>
        [Column("OPENGENT_THREE_TIP")]
        public string opengent_three_tip { get; set; }
        /// <summary>
        /// 开通代理(一级合伙人)
        /// </summary>
        /// <returns></returns>
        [Column("OPENGENT_PARNERS_ONE_TIP")]
        public string opengent_parners_one_tip { get; set; }
        /// <summary>
        /// 开通代理(二级合伙人)
        /// </summary>
        /// <returns></returns>
        [Column("OPENGENT_PARNERS_TWO_TIP")]
        public string opengent_parners_two_tip { get; set; }

        /// <summary>
        /// 升级代理(一级)
        /// </summary>
        [Column("UPGRADEGENT_ONE_TIP")]
        public string upgradegent_one_tip { get; set; }

        /// <summary>
        /// 升级代理(二级)
        /// </summary>
        [Column("UPGRADEGENT_TWO_TIP")]
        public string upgradegent_two_tip { get; set; }

        /// <summary>
        /// 升级代理(三级)
        /// </summary>
        [Column("UPGRADEGENT_THREE_TIP")]
        public string upgradegent_three_tip { get; set; }

        /// <summary>
        /// 升级代理(一级合伙人)
        /// </summary>
        [Column("UPGRADEGENT_PARNERS_ONE_TIP")]
        public string upgradegent_parners_one_tip { get; set; }

        /// <summary>
        /// 升级代理(二级合伙人)
        /// </summary>
        [Column("UPGRADEGENT_PARNERS_TWO_TIP")]
        public string upgradegent_parners_two_tip { get; set; }

        /// <summary>
        /// 其他提示
        /// </summary>
        [Column("OTHER_TIP")]
        public string other_tip { get; set; }
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

