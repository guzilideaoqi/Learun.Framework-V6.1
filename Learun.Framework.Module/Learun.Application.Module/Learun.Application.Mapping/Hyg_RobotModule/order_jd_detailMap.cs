using Learun.Application.TwoDevelopment.Hyg_RobotModule;
using System.Data.Entity.ModelConfiguration;

namespace  Learun.Application.Mapping
{
    /// <summary>
    /// 版 本 Learun-ADMS V6.1.6.0 力软敏捷开发框架
    /// Copyright (c) 2013-2017 上海力软信息技术有限公司
    /// 创 建：超级管理员
    /// 日 期：2019-11-26 20:23
    /// 描 述：京东订单详情
    /// </summary>
    public class order_jd_detailMap : EntityTypeConfiguration<order_jd_detailEntity>
    {
        public order_jd_detailMap()
        {
            #region 表、主键
            //表
            this.ToTable("ORDER_JD_DETAIL");
            //主键
            this.HasKey(t => t.id);
            #endregion

            #region 配置关系
            #endregion
        }
    }
}

