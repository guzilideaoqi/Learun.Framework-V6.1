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
    /// 日 期：2020-04-14 17:35
    /// 描 述：提现申请记录
    /// </summary>
    public class DM_Apply_CashRecordBLL : DM_Apply_CashRecordIBLL
    {
        private DM_Apply_CashRecordService dM_Apply_CashRecordService = new DM_Apply_CashRecordService();

        #region 获取数据

        /// <summary>
        /// 获取列表数据
        /// <summary>
        /// <returns></returns>
        public IEnumerable<dm_apply_cashrecordEntity> GetList(string queryJson)
        {
            try
            {
                return dM_Apply_CashRecordService.GetList(queryJson);
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
        public IEnumerable<dm_apply_cashrecordEntity> GetPageList(Pagination pagination, string queryJson)
        {
            try
            {
                return dM_Apply_CashRecordService.GetPageList(pagination, queryJson);
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
        /// 获取提现记录
        /// </summary>
        /// <param name="pagination"></param>
        /// <param name="queryJson"></param>
        /// <returns></returns>
        public DataTable GetPageListByDataTable(Pagination pagination, string queryJson)
        {
            try
            {
                return dM_Apply_CashRecordService.GetPageListByDataTable(pagination, queryJson);
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
        public dm_apply_cashrecordEntity GetEntity(int keyValue)
        {
            try
            {
                return dM_Apply_CashRecordService.GetEntity(keyValue);
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
                dM_Apply_CashRecordService.DeleteEntity(keyValue);
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
        public void SaveEntity(int keyValue, dm_apply_cashrecordEntity entity)
        {
            try
            {
                dM_Apply_CashRecordService.SaveEntity(keyValue, entity);
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

        #region 申请提现
        public void ApplyAccountCash(int user_id, decimal price, string remark, string appid)
        {
            try
            {
                dM_Apply_CashRecordService.ApplyAccountCash(user_id, price, remark,appid);
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

        #region 审核提现记录
        public void CheckApplyCashRecord(int id, int paytype)
        {
            try
            {
                dM_Apply_CashRecordService.CheckApplyCashRecord(id, paytype);
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

        public void CheckApplyCashRecordByAli(dm_apply_cashrecordEntity dm_Apply_CashrecordEntity) {
            try
            {
                dM_Apply_CashRecordService.CheckApplyCashRecordByAli(dm_Apply_CashrecordEntity);
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

        #region 获取我的提现记录
        public IEnumerable<dm_apply_cashrecordEntity> GetMyCashRecord(int user_id, Pagination pagination)
        {
            try
            {
                return dM_Apply_CashRecordService.GetMyCashRecord(user_id, pagination);
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
