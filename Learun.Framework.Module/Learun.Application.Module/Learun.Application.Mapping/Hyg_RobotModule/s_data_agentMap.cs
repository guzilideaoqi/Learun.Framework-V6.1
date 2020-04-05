using Learun.Application.TwoDevelopment.Hyg_RobotModule;
using System.Data.Entity.ModelConfiguration;

namespace  Learun.Application.Mapping
{
    /// <summary>
    /// 版 本 Learun-ADMS V6.1.6.0 力软敏捷开发框架
    /// Copyright (c) 2013-2017 上海力软信息技术有限公司
    /// 创 建：超级管理员
    /// 日 期：2019-11-19 21:15
    /// 描 述：代理商管理
    /// </summary>
    public class s_data_agentMap : EntityTypeConfiguration<s_data_agentEntity>
    {
        public s_data_agentMap()
        {
            #region 表、主键
            //表
            this.ToTable("S_DATA_AGENT");
            //主键
            this.HasKey(t => t.F_AgentId);
            #endregion

            #region 配置关系
            #endregion
        }
    }
}

