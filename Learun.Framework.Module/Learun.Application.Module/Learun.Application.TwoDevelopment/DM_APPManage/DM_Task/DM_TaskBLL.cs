using Learun.Util;
using System;
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
    public class DM_TaskBLL : DM_TaskIBLL
    {
        private DM_TaskService dM_TaskService = new DM_TaskService();

        #region 获取数据

        /// <summary>
        /// 获取列表数据
        /// <summary>
        /// <returns></returns>
        public IEnumerable<dm_taskEntity> GetList(string queryJson)
        {
            try
            {
                return dM_TaskService.GetList(queryJson);
            }
            catch (Exception ex)
            {
                if (ex is ExceptionEx)
                {
                    throw;
                }
                else
                {
                    throw ExceptionEx.ThrowBusinessException(ex);
                }
            }
        }

        /// <summary>
        /// 获取列表分页数据
        /// <param name="pagination">分页参数</param>
        /// <summary>
        /// <returns></returns>
        public IEnumerable<dm_taskEntity> GetPageList(Pagination pagination, string queryJson)
        {
            try
            {
                return dM_TaskService.GetPageList(pagination, queryJson);
            }
            catch (Exception ex)
            {
                if (ex is ExceptionEx)
                {
                    throw;
                }
                else
                {
                    throw ExceptionEx.ThrowBusinessException(ex);
                }
            }
        }

        public DataTable GetPageListByDataTable(Pagination pagination, string queryJson, bool IsApi = false)
        {
            try
            {
                return dM_TaskService.GetPageListByDataTable(pagination, queryJson, IsApi);
            }
            catch (Exception ex)
            {
                if (ex is ExceptionEx)
                {
                    throw;
                }
                else
                {
                    throw ExceptionEx.ThrowBusinessException(ex);
                }
            }
        }

        /// <summary>
        /// 获取实体数据
        /// <param name="keyValue">主键</param>
        /// <summary>
        /// <returns></returns>
        public dm_taskEntity GetEntity(int keyValue)
        {
            try
            {
                return dM_TaskService.GetEntity(keyValue);
            }
            catch (Exception ex)
            {
                if (ex is ExceptionEx)
                {
                    throw;
                }
                else
                {
                    throw ExceptionEx.ThrowBusinessException(ex);
                }
            }
        }

        /// <summary>
        /// 增加任务详情拓展方法(解决实体中不包含发单人信息)
        /// </summary>
        /// <returns></returns>
        public Dictionary<string, object> GetTaskDetail(int? id)
        {
            try
            {
                return dM_TaskService.GetTaskDetail(id);
            }
            catch (Exception ex)
            {
                if (ex is ExceptionEx)
                {
                    throw;
                }
                else
                {
                    throw ExceptionEx.ThrowBusinessException(ex);
                }
            }
        }

        /// <summary>
        /// 获取随机活动任务
        /// </summary>
        /// <returns></returns>
        public DataTable GetRandActivityTaskList(int user_id) {
            try
            {
                return dM_TaskService.GetRandActivityTaskList(user_id);
            }
            catch (Exception ex)
            {
                if (ex is ExceptionEx)
                {
                    throw;
                }
                else
                {
                    throw ExceptionEx.ThrowBusinessException(ex);
                }
            }
        }
        #endregion

        #region 提交数据

        /// <summary>
        /// 删除实体数据
        /// <param name="keyValue">主键</param>
        /// <summary>
        /// <returns></returns>
        public void DeleteEntity(int keyValue)
        {
            try
            {
                dM_TaskService.DeleteEntity(keyValue);
            }
            catch (Exception ex)
            {
                if (ex is ExceptionEx)
                {
                    throw;
                }
                else
                {
                    throw ExceptionEx.ThrowBusinessException(ex);
                }
            }
        }

        /// <summary>
        /// 保存实体数据（新增、修改）
        /// <param name="keyValue">主键</param>
        /// <summary>
        /// <returns></returns>
        public void SaveEntity(int keyValue, dm_taskEntity entity)
        {
            try
            {
                dM_TaskService.SaveEntity(keyValue, entity);
            }
            catch (Exception ex)
            {
                if (ex is ExceptionEx)
                {
                    throw;
                }
                else
                {
                    throw ExceptionEx.ThrowBusinessException(ex);
                }
            }
        }

        /// <summary>
        /// 后台审核发布任务
        /// </summary>
        /// <param name="id"></param>
        public void CheckTaskByWeb(int id)
        {
            try
            {
                dM_TaskService.CheckTaskByWeb(id);
            }
            catch (Exception ex)
            {
                if (ex is ExceptionEx)
                {
                    throw;
                }
                else
                {
                    throw ExceptionEx.ThrowBusinessException(ex);
                }
            }
        }

        /// <summary>
        /// 下架任务
        /// </summary>
        /// <param name="id"></param>
        public void DownTask(int id) {
            try
            {
                dM_TaskService.DownTask(id);
            }
            catch (Exception ex)
            {
                if (ex is ExceptionEx)
                {
                    throw;
                }
                else
                {
                    throw ExceptionEx.ThrowBusinessException(ex);
                }
            }
        }
        #endregion

        #region Method Extend
        public void ReleaseTaskByWeb(dm_taskEntity entity)
        {
            try
            {
                dM_TaskService.ReleaseTaskByWeb(entity);
            }
            catch (Exception ex)
            {
                if (ex is ExceptionEx)
                {
                    throw;
                }
                else
                {
                    throw ExceptionEx.ThrowBusinessException(ex);
                }
            }
        }
        /// <summary>
        /// 发布任务
        /// </summary>
        /// <param name="entity"></param>
        public void ReleaseTask(dm_taskEntity entity)
        {
            try
            {
                dM_TaskService.ReleaseTask(entity);
            }
            catch (Exception ex)
            {
                if (ex is ExceptionEx)
                {
                    throw;
                }
                else
                {
                    throw ExceptionEx.ThrowBusinessException(ex);
                }
            }
        }
        /// <summary>
        /// 取消发布任务
        /// </summary>
        /// <param name="task_id"></param>
        public void CancelByReleasePerson(int task_id)
        {
            try
            {
                dM_TaskService.CancelByReleasePerson(task_id);
            }
            catch (Exception ex)
            {
                if (ex is ExceptionEx)
                {
                    throw;
                }
                else
                {
                    throw ExceptionEx.ThrowBusinessException(ex);
                }
            }
        }

        /// <summary>
        /// 修改权重值图片
        /// </summary>
        /// <param name="task_id">任务ID</param>
        /// <param name="sort_value">权重值</param>
        public void UpdateSortValue(int task_id, int sort_value) {
            try
            {
                dM_TaskService.UpdateSortValue(task_id, sort_value);
            }
            catch (Exception ex)
            {
                if (ex is ExceptionEx)
                {
                    throw;
                }
                else
                {
                    throw ExceptionEx.ThrowBusinessException(ex);
                }
            }
        }

        /// <summary>
        /// 任务驳回
        /// </summary>
        /// <param name="id"></param>
        /// <param name="remark"></param>
        public void RebutTaskByWeb(int id, string remark) {
            try
            {
                dM_TaskService.RebutTaskByWeb(id, remark);
            }
            catch (Exception ex)
            {
                if (ex is ExceptionEx)
                {
                    throw;
                }
                else
                {
                    throw ExceptionEx.ThrowBusinessException(ex);
                }
            }
        }
        #endregion
    }
}
