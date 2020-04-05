using Learun.Util;
using System.Collections.Generic;

namespace Learun.Application.TwoDevelopment.Hyg_RobotModule
{
    /// <summary>
    /// 版 本 Learun-ADMS V6.1.6.0 力软敏捷开发框架
    /// Copyright (c) 2013-2017 上海力软信息技术有限公司
    /// 创 建：超级管理员
    /// 日 期：2019-11-19 20:34
    /// 描 述：应用商信息设置
    /// </summary>
    public interface Application_SettingIBLL
    {
        #region 获取数据

        /// <summary>
        /// 获取列表数据
        /// <summary>
        /// <returns></returns>
        IEnumerable<s_application_settingEntity> GetList( string queryJson );
        /// <summary>
        /// 获取列表分页数据
        /// <param name="pagination">分页参数</param>
        /// <summary>
        /// <returns></returns>
        IEnumerable<s_application_settingEntity> GetPageList(Pagination pagination, string queryJson);
        /// <summary>
        /// 获取实体数据
        /// <param name="keyValue">主键</param>
        /// <summary>
        /// <returns></returns>
        s_application_settingEntity GetEntity(string keyValue);
        /// <summary>
        /// 获取应用商配置信息
        /// </summary>
        /// <param name="applicationId"></param>
        /// <returns></returns>
        s_application_settingEntity GetEntityByApplicationId(string applicationId = "");
        #endregion

        #region 提交数据

        /// <summary>
        /// 删除实体数据
        /// <param name="keyValue">主键</param>
        /// <summary>
        /// <returns></returns>
        void DeleteEntity(string keyValue);
        /// <summary>
        /// 保存实体数据（新增、修改）
        /// <param name="keyValue">主键</param>
        /// <summary>
        /// <returns></returns>
        void SaveEntity(string keyValue, s_application_settingEntity entity);
        #endregion

    }
}
