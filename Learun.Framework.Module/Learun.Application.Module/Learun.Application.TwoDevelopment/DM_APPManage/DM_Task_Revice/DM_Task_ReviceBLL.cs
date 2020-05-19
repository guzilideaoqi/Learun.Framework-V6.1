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
    /// 日 期：2020-04-16 16:03
    /// 描 述：任务接受记录
    /// </summary>
    public class DM_Task_ReviceBLL : DM_Task_ReviceIBLL
    {
        private DM_Task_ReviceService dM_Task_ReviceService = new DM_Task_ReviceService();

        #region 获取数据

        /// <summary>
        /// 获取列表数据
        /// <summary>
        /// <returns></returns>
        public IEnumerable<dm_task_reviceEntity> GetList(string queryJson)
        {
            try
            {
                return dM_Task_ReviceService.GetList(queryJson);
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
        public IEnumerable<dm_task_reviceEntity> GetPageList(Pagination pagination, string queryJson)
        {
            try
            {
                return dM_Task_ReviceService.GetPageList(pagination, queryJson);
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

        public DataTable GetPageListByDataTable(Pagination pagination, string queryJson) {
            try
            {
                return dM_Task_ReviceService.GetPageListByDataTable(pagination, queryJson);
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
        public dm_task_reviceEntity GetEntity(int keyValue)
        {
            try
            {
                return dM_Task_ReviceService.GetEntity(keyValue);
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
                dM_Task_ReviceService.DeleteEntity(keyValue);
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
        public void SaveEntity(int keyValue, dm_task_reviceEntity entity)
        {
            try
            {
                dM_Task_ReviceService.SaveEntity(keyValue, entity);
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
        /// <summary>
        /// 获取任务接受列表
        /// </summary>
        /// <param name="task_id"></param>
        /// <returns></returns>
        public IEnumerable<dm_task_reviceEntity> GetReviceListByTaskID(int task_id)
        {
            try
            {
                return dM_Task_ReviceService.GetReviceListByTaskID(task_id);
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
        /// 接受任务
        /// </summary>
        /// <param name="dm_Task_ReviceEntity"></param>
        public void ReviceTask(dm_task_reviceEntity dm_Task_ReviceEntity,string appid)
        {
            try
            {
                dM_Task_ReviceService.ReviceTask(dm_Task_ReviceEntity, appid);
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
        /// 提交资料
        /// </summary>
        /// <param name="dm_Task_ReviceEntity"></param>
        public void SubmitMeans(dm_task_reviceEntity dm_Task_ReviceEntity)
        {
            try
            {
                dM_Task_ReviceService.SubmitMeans(dm_Task_ReviceEntity);
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
        /// 取消任务
        /// </summary>
        /// <param name="revice_id"></param>
        public void CancelByRevicePerson(int revice_id)
        {
            try
            {
                dM_Task_ReviceService.CancelByRevicePerson(revice_id);
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
        /// 审核任务
        /// </summary>
        /// <param name="revice_id"></param>
        public void AuditTask(int revice_id)
        {
            try
            {
                dM_Task_ReviceService.AuditTask(revice_id);
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
        /// 我的接受任务
        /// </summary>
        /// <param name="user_id"></param>
        /// <param name="pagination"></param>
        /// <returns></returns>
        public DataTable GetMyReviceTask(int user_id, int TaskStatus, Pagination pagination) {
            try
            {
               return dM_Task_ReviceService.GetMyReviceTask(user_id, TaskStatus, pagination);
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
        /// 获取任务接受记录详情
        /// </summary>
        /// <param name="user_id"></param>
        /// <param name="task_id"></param>
        /// <returns></returns>
        public dm_task_reviceEntity GetReviceEntity(int user_id, int task_id) {
            try
            {
                return dM_Task_ReviceService.GetReviceEntity(user_id, task_id);
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
