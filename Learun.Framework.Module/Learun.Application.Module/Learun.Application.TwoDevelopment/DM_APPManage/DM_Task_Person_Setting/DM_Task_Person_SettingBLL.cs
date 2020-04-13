using Learun.Util;
using System;
using System.Collections.Generic;

namespace Learun.Application.TwoDevelopment.DM_APPManage
{
    /// <summary>
    /// 版 本 Learun-ADMS V6.1.6.0 力软敏捷开发框架
    /// Copyright (c) 2013-2017 上海力软信息技术有限公司
    /// 创 建：超级管理员
    /// 日 期：2020-04-10 13:55
    /// 描 述：进度任务设置
    /// </summary>
    public class DM_Task_Person_SettingBLL : DM_Task_Person_SettingIBLL
    {
        private DM_Task_Person_SettingService dM_Task_Person_SettingService = new DM_Task_Person_SettingService();

        #region 获取数据

        /// <summary>
        /// 获取列表数据
        /// <summary>
        /// <returns></returns>
        public IEnumerable<dm_task_person_settingEntity> GetList(string queryJson)
        {
            try
            {
                return dM_Task_Person_SettingService.GetList(queryJson);
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
        public IEnumerable<dm_task_person_settingEntity> GetPageList(Pagination pagination, string queryJson)
        {
            try
            {
                return dM_Task_Person_SettingService.GetPageList(pagination, queryJson);
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
        public dm_task_person_settingEntity GetEntity(int keyValue)
        {
            try
            {
                return dM_Task_Person_SettingService.GetEntity(keyValue);
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
                dM_Task_Person_SettingService.DeleteEntity(keyValue);
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
        public void SaveEntity(int keyValue, dm_task_person_settingEntity entity)
        {
            try
            {
                dM_Task_Person_SettingService.SaveEntity(keyValue, entity);
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

        #region 获取个人任务进度
        /// <summary>
        /// 获取个人任务进度
        /// </summary>
        /// <param name="user_id"></param>
        /// <param name="appid"></param>
        /// <returns></returns>
        public IEnumerable<dm_task_person_settingEntity> GetPersonProcess(int user_id, string appid)
        {
            try
            {
                return dM_Task_Person_SettingService.GetPersonProcess(user_id, appid);
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

        #region 领取任务
        /// <summary>
        /// 领取任务
        /// </summary>
        /// <param name="user_id"></param>
        /// <param name="task_id"></param>
        public void ReceiveAwards(int user_id, int? task_id)
        {
            try
            {
                dM_Task_Person_SettingService.ReceiveAwards(user_id, task_id);
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

        #region 获取升级合伙人任务
        /// <summary>
        /// 获取合伙人任务
        /// </summary>
        /// <param name="user_id"></param>
        /// <param name="appid"></param>
        /// <returns></returns>
        public IEnumerable<dm_task_person_settingEntity> GetPartnersProcess(int user_id, string appid)
        {
            try
            {
                return dM_Task_Person_SettingService.GetPartnersProcess(user_id, appid);
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

        #region 申请成为合伙人
        /// <summary>
        /// 申请成为合伙人
        /// </summary>
        /// <param name="user_id"></param>
        public void ApplyPartners(int user_id, string appid)
        {
            try
            {
                dM_Task_Person_SettingService.ApplyPartners(user_id, appid);
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
