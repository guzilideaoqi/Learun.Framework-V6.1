using Learun.Application.TwoDevelopment.Hyg_RobotModule;
using System.Data.Entity.ModelConfiguration;

namespace  Learun.Application.Mapping
{
    /// <summary>
    /// 版 本 Learun-ADMS V6.1.6.0 力软敏捷开发框架
    /// Copyright (c) 2013-2017 上海力软信息技术有限公司
    /// 创 建：超级管理员
    /// 日 期：2019-11-22 22:10
    /// 描 述：拼多多订单列表
    /// </summary>
    public class order_pddMap : EntityTypeConfiguration<order_pddEntity>
    {
        public order_pddMap()
        {
            #region 表、主键
            //表
            this.ToTable("ORDER_PDD");
            //主键
            this.HasKey(t => t.order_sn);
            #endregion

            #region 配置关系
            #endregion
        }
    }
}

