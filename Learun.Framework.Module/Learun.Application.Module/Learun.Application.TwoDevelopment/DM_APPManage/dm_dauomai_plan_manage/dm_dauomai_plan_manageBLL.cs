using Hyg.Common.DuoMaiTools.DuoMaiRequest;
using Hyg.Common.DuoMaiTools.DuoMaiResponse;
using Learun.Util;
using System;
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
    public class dm_dauomai_plan_manageBLL : dm_dauomai_plan_manageIBLL
    {
        private dm_dauomai_plan_manageService dm_dauomai_plan_manageService = new dm_dauomai_plan_manageService();

        #region 获取数据

        /// <summary>
        /// 获取列表数据
        /// <summary>
        /// <returns></returns>
        public IEnumerable<dm_dauomai_plan_manageEntity> GetList(string queryJson)
        {
            try
            {
                return dm_dauomai_plan_manageService.GetList(queryJson);
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
        public IEnumerable<dm_dauomai_plan_manageEntity> GetPageList(Pagination pagination, string queryJson)
        {
            try
            {
                return dm_dauomai_plan_manageService.GetPageList(pagination, queryJson);
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
        public dm_dauomai_plan_manageEntity GetEntity(int? keyValue)
        {
            try
            {
                return dm_dauomai_plan_manageService.GetEntity(keyValue);
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
        public void DeleteEntity(int? keyValue)
        {
            try
            {
                dm_dauomai_plan_manageService.DeleteEntity(keyValue);
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
        public void SaveEntity(int? keyValue, dm_dauomai_plan_manageEntity entity)
        {
            try
            {
                dm_dauomai_plan_manageService.SaveEntity(keyValue, entity);
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

        #region 同步推广计划
        public void SyncPlanList(Query_CPS_Stores_PlansRequest query_CPS_Stores_PlansRequest)
        {
            try
            {
                dm_dauomai_plan_manageService.SyncPlanList(query_CPS_Stores_PlansRequest);
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

        #region 激活功能
        public void StartPlan(int plan_id)
        {
            try
            {
                dm_dauomai_plan_manageService.StartPlan(plan_id);
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

        #region 停止使用
        public void StopPlan(int plan_id)
        {
            try
            {
                dm_dauomai_plan_manageService.StopPlan(plan_id);
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

        #region 推广转链
        public CPS_Convert_LinkResponse ConvertLink(int plan_id, int user_id)
        {
            try
            {
                return dm_dauomai_plan_manageService.ConvertLink(plan_id, user_id);
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
