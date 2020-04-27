using Learun.Util;
using System.Collections.Generic;
using System.Data;

namespace Learun.Application.TwoDevelopment.DM_APPManage
{
    /// <summary>
    /// 版 本 Learun-ADMS V6.1.6.0 力软敏捷开发框架
    /// Copyright (c) 2013-2017 上海力软信息技术有限公司
    /// 创 建：超级管理员
    /// 日 期：2020-04-16 16:03
    /// 描 述：任务接受记录
    /// </summary>
    public interface DM_Task_ReviceIBLL
    {
        #region 获取数据

        /// <summary>
        /// 获取列表数据
        /// <summary>
        /// <returns></returns>
        IEnumerable<dm_task_reviceEntity> GetList(string queryJson);
        /// <summary>
        /// 获取列表分页数据
        /// <param name="pagination">分页参数</param>
        /// <summary>
        /// <returns></returns>
        IEnumerable<dm_task_reviceEntity> GetPageList(Pagination pagination, string queryJson);

        /// <summary>
        /// 获取列表分页数据
        /// </summary>
        /// <param name="pagination"></param>
        /// <param name="queryJson"></param>
        /// <returns></returns>
        DataTable GetPageListByDataTable(Pagination pagination, string queryJson);
        /// <summary>
        /// 获取实体数据
        /// <param name="keyValue">主键</param>
        /// <summary>
        /// <returns></returns>
        dm_task_reviceEntity GetEntity(int keyValue);
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
        void SaveEntity(int keyValue, dm_task_reviceEntity entity);
        #endregion

        #region Method Extend
        /// <summary>
        /// 获取任务接受列表
        /// </summary>
        /// <param name="task_id"></param>
        /// <returns></returns>
        IEnumerable<dm_task_reviceEntity> GetReviceListByTaskID(int task_id);

        /// <summary>
        /// 接受任务
        /// </summary>
        /// <param name="dm_Task_ReviceEntity"></param>
        void ReviceTask(dm_task_reviceEntity dm_Task_ReviceEntity);

        /// <summary>
        /// 提交资料
        /// </summary>
        /// <param name="dm_Task_ReviceEntity"></param>
        void SubmitMeans(dm_task_reviceEntity dm_Task_ReviceEntity);

        /// <summary>
        /// 取消任务
        /// </summary>
        /// <param name="revice_id"></param>
        void CancelByRevicePerson(int revice_id);

        /// <summary>
        /// 审核任务
        /// </summary>
        /// <param name="revice_id"></param>
        void AuditTask(int revice_id);

        /// <summary>
        /// 我的接受任务
        /// </summary>
        /// <param name="user_id"></param>
        /// <param name="pagination"></param>
        /// <returns></returns>
        DataTable GetMyReviceTask(int user_id, Pagination pagination);

        /// <summary>
        /// 获取任务接受记录详情
        /// </summary>
        /// <param name="user_id"></param>
        /// <param name="task_id"></param>
        /// <returns></returns>
        dm_task_reviceEntity GetReviceEntity(int user_id, int task_id);
        #endregion
    }
}
