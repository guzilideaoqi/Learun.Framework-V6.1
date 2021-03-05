using Hyg.Common.DuoMaiTools.DuoMaiRequest;
using Hyg.Common.DuoMaiTools.DuoMaiResponse;
using Learun.Util;
using System.Collections.Generic;

namespace Learun.Application.TwoDevelopment.DM_APPManage
{
    /// <summary>
    /// 版 本 Learun-ADMS V6.1.6.0 力软敏捷开发框架
    /// Copyright (c) 2013-2017 上海力软信息技术有限公司
    /// 创 建：超级管理员
    /// 日 期：2021-03-03 10:50
    /// 描 述：多麦计划
    /// </summary>
    public interface dm_dauomai_plan_manageIBLL
    {
        #region 获取数据

        /// <summary>
        /// 获取列表数据
        /// <summary>
        /// <returns></returns>
        IEnumerable<dm_dauomai_plan_manageEntity> GetList( string queryJson );
        /// <summary>
        /// 获取列表分页数据
        /// <param name="pagination">分页参数</param>
        /// <summary>
        /// <returns></returns>
        IEnumerable<dm_dauomai_plan_manageEntity> GetPageList(Pagination pagination, string queryJson);
        /// <summary>
        /// 获取实体数据
        /// <param name="keyValue">主键</param>
        /// <summary>
        /// <returns></returns>
        dm_dauomai_plan_manageEntity GetEntity(int? keyValue);
        #endregion

        #region 提交数据

        /// <summary>
        /// 删除实体数据
        /// <param name="keyValue">主键</param>
        /// <summary>
        /// <returns></returns>
        void DeleteEntity(int? keyValue);
        /// <summary>
        /// 保存实体数据（新增、修改）
        /// <param name="keyValue">主键</param>
        /// <summary>
        /// <returns></returns>
        void SaveEntity(int? keyValue, dm_dauomai_plan_manageEntity entity);
        #endregion

        #region 同步推广计划
        void SyncPlanList(Query_CPS_Stores_PlansRequest query_CPS_Stores_PlansRequest);
        #endregion

        #region 激活功能
        void StartPlan(int plan_id);
        #endregion

        #region 停止使用
        void StopPlan(int plan_id);
        #endregion

        #region 推广转链
        CPS_Convert_LinkResponse ConvertLink(int plan_id, int user_id);
        #endregion
    }
}
