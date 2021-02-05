using Learun.Util;
using System;
using System.Collections.Generic;

namespace Learun.Application.TwoDevelopment.DM_APPManage
{
    /// <summary>
    /// 版 本 Learun-ADMS V6.1.6.0 力软敏捷开发框架
    /// Copyright (c) 2013-2017 上海力软信息技术有限公司
    /// 创 建：超级管理员
    /// 日 期：2021-02-05 15:33
    /// 描 述：活动管理
    /// </summary>
    public class dm_activity_manageBLL : dm_activity_manageIBLL
    {
        private dm_activity_manageService dm_activity_manageService = new dm_activity_manageService();

        #region 获取数据

        /// <summary>
        /// 获取列表数据
        /// <summary>
        /// <returns></returns>
        public IEnumerable<dm_activity_manageEntity> GetList( string queryJson )
        {
            try
            {
                return dm_activity_manageService.GetList(queryJson);
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
        public IEnumerable<dm_activity_manageEntity> GetPageList(Pagination pagination, string queryJson)
        {
            try
            {
                return dm_activity_manageService.GetPageList(pagination, queryJson);
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
        public dm_activity_manageEntity GetEntity(string keyValue)
        {
            try
            {
                return dm_activity_manageService.GetEntity(keyValue);
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
        /// 获取当前活动中的任务
        /// </summary>
        /// <returns></returns>
        public dm_activity_manageEntity GetActivityInfo() {
            try
            {
                return dm_activity_manageService.GetActivityInfo();
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
        public void DeleteEntity(string keyValue)
        {
            try
            {
                dm_activity_manageService.DeleteEntity(keyValue);
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
        public void SaveEntity(string keyValue, dm_activity_manageEntity entity)
        {
            try
            {
                dm_activity_manageService.SaveEntity(keyValue, entity);
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
