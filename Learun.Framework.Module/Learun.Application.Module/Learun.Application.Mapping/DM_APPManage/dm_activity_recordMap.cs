﻿using Learun.Application.TwoDevelopment.DM_APPManage;
using System.Data.Entity.ModelConfiguration;

namespace  Learun.Application.Mapping
{
    /// <summary>
    /// 版 本 Learun-ADMS V6.1.6.0 力软敏捷开发框架
    /// Copyright (c) 2013-2017 上海力软信息技术有限公司
    /// 创 建：超级管理员
    /// 日 期：2021-02-05 14:26
    /// 描 述：活动领取记录
    /// </summary>
    public class dm_activity_recordMap : EntityTypeConfiguration<dm_activity_recordEntity>
    {
        public dm_activity_recordMap()
        {
            #region 表、主键
            //表
            this.ToTable("DM_ACTIVITY_RECORD");
            //主键
            this.HasKey(t => t.f_id);
            #endregion

            #region 配置关系
            #endregion
        }
    }
}
