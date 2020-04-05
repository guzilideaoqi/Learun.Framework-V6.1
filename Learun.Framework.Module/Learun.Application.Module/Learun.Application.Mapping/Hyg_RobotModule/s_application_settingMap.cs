using Learun.Application.TwoDevelopment.Hyg_RobotModule;
using System.Data.Entity.ModelConfiguration;

namespace  Learun.Application.Mapping
{
    /// <summary>
    /// 版 本 Learun-ADMS V6.1.6.0 力软敏捷开发框架
    /// Copyright (c) 2013-2017 上海力软信息技术有限公司
    /// 创 建：超级管理员
    /// 日 期：2019-11-19 20:34
    /// 描 述：应用商信息设置
    /// </summary>
    public class s_application_settingMap : EntityTypeConfiguration<s_application_settingEntity>
    {
        public s_application_settingMap()
        {
            #region 表、主键
            //表
            this.ToTable("S_APPLICATION_SETTING");
            //主键
            this.HasKey(t => t.F_SettingId);
            #endregion

            #region 配置关系
            #endregion
        }
    }
}

