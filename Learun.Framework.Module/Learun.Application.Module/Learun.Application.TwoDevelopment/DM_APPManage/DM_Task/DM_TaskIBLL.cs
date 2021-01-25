using Learun.Util;
using System.Collections.Generic;
using System.Data;

namespace Learun.Application.TwoDevelopment.DM_APPManage
{
    /// <summary>
    /// 版 本 Learun-ADMS V6.1.6.0 力软敏捷开发框架
    /// Copyright (c) 2013-2017 上海力软信息技术有限公司
    /// 创 建：超级管理员
    /// 日 期：2020-04-16 16:01
    /// 描 述：任务中心
    /// </summary>
    public interface DM_TaskIBLL
    {
        #region 获取数据

        /// <summary>
        /// 获取列表数据
        /// <summary>
        /// <returns></returns>
        IEnumerable<dm_taskEntity> GetList( string queryJson );
        /// <summary>
        /// 获取列表分页数据
        /// <param name="pagination">分页参数</param>
        /// <summary>
        /// <returns></returns>
        IEnumerable<dm_taskEntity> GetPageList(Pagination pagination, string queryJson);

        /// <summary>
        /// 获取列表分页数据
        /// </summary>
        /// <param name="pagination"></param>
        /// <param name="queryJson"></param>
        /// <returns></returns>
        DataTable GetPageListByDataTable(Pagination pagination, string queryJson, bool IsApi = false);
        /// <summary>
        /// 获取实体数据
        /// <param name="keyValue">主键</param>
        /// <summary>
        /// <returns></returns>
        dm_taskEntity GetEntity(int keyValue);

        /// <summary>
        /// 增加任务详情拓展方法(解决实体中不包含发单人信息)
        /// </summary>
        /// <returns></returns>
        Dictionary<string, object> GetTaskDetail(int? id);
        #endregion

        #region 提交数据

        /// <summary>
        /// 删除实体数据
        /// <param name="keyValue">主键</param>
        /// <summary>
        /// <returns></returns>
        void DeleteEntity(int keyValue);
        /// <summary>
        /// 保存实体数据（新增、修改）
        /// <param name="keyValue">主键</param>
        /// <summary>
        /// <returns></returns>
        void SaveEntity(int keyValue, dm_taskEntity entity);
        /// <summary>
        /// 后台审核发布任务
        /// </summary>
        /// <param name="id"></param>
        void CheckTaskByWeb(int id);

        /// <summary>
        /// 下架任务
        /// </summary>
        /// <param name="id"></param>
        void DownTask(int id);
        #endregion


        #region Method Extend
        /// <summary>
        /// 电脑端发布任务
        /// </summary>
        /// <param name="entity"></param>
        void ReleaseTaskByWeb(dm_taskEntity entity);
        /// <summary>
        /// 发布任务
        /// </summary>
        /// <param name="entity"></param>
        void ReleaseTask(dm_taskEntity entity);
        /// <summary>
        /// 取消发布任务
        /// </summary>
        /// <param name="task_id"></param>
        void CancelByReleasePerson(int task_id);
        /// <summary>
        /// 修改任务权重值
        /// </summary>
        /// <param name="task_id">任务ID</param>
        /// <param name="sort_value">权重值</param>
        void UpdateSortValue(int task_id, int sort_value);
        #endregion
    }
}
