using Learun.Util;
using System;
using System.Collections.Generic;
using System.Data;

namespace Learun.Application.TwoDevelopment.DM_APPManage
{
    public class DM_IntergralChangeRecordBLL : DM_IntergralChangeRecordIBLL
    {
        private DM_IntergralChangeRecordService dM_IntergralChangeRecordService = new DM_IntergralChangeRecordService();

        public IEnumerable<dm_intergralchangerecordEntity> GetList(string queryJson)
        {
            try
            {
                return dM_IntergralChangeRecordService.GetList(queryJson);
            }
            catch (Exception ex)
            {
                if (ex is ExceptionEx)
                {
                    throw;
                }
                throw ExceptionEx.ThrowBusinessException(ex);
            }
        }

        public IEnumerable<dm_intergralchangerecordEntity> GetPageList(Pagination pagination, string queryJson)
        {
            try
            {
                return dM_IntergralChangeRecordService.GetPageList(pagination, queryJson);
            }
            catch (Exception ex)
            {
                if (ex is ExceptionEx)
                {
                    throw;
                }
                throw ExceptionEx.ThrowBusinessException(ex);
            }
        }

        public dm_intergralchangerecordEntity GetEntity(int keyValue)
        {
            try
            {
                return dM_IntergralChangeRecordService.GetEntity(keyValue);
            }
            catch (Exception ex)
            {
                if (ex is ExceptionEx)
                {
                    throw;
                }
                throw ExceptionEx.ThrowBusinessException(ex);
            }
        }

        public void DeleteEntity(int keyValue)
        {
            try
            {
                dM_IntergralChangeRecordService.DeleteEntity(keyValue);
            }
            catch (Exception ex)
            {
                if (ex is ExceptionEx)
                {
                    throw;
                }
                throw ExceptionEx.ThrowBusinessException(ex);
            }
        }

        public void SaveEntity(int keyValue, dm_intergralchangerecordEntity entity)
        {
            try
            {
                dM_IntergralChangeRecordService.SaveEntity(keyValue, entity);
            }
            catch (Exception ex)
            {
                if (ex is ExceptionEx)
                {
                    throw;
                }
                throw ExceptionEx.ThrowBusinessException(ex);
            }
        }

        public void ApplyChangeGood(dm_intergralchangerecordEntity dm_IntergralchangerecordEntity, dm_userEntity dm_UserEntity)
        {
            try
            {
                dM_IntergralChangeRecordService.ApplyChangeGood(dm_IntergralchangerecordEntity, dm_UserEntity);
            }
            catch (Exception ex)
            {
                if (ex is ExceptionEx)
                {
                    throw;
                }
                throw ExceptionEx.ThrowBusinessException(ex);
            }
        }

        /// <summary>
        /// �ҵĻ��ֶһ���¼
        /// </summary>
        /// <param name="UserID"></param>
        /// <param name="pagination"></param>
        /// <returns></returns>
        public DataTable GetMyIntegralGoodRecord(int UserID, Pagination pagination)
        {
            try
            {
                return dM_IntergralChangeRecordService.GetMyIntegralGoodRecord(UserID, pagination);
            }
            catch (Exception ex)
            {
                if (ex is ExceptionEx)
                {
                    throw;
                }
                throw ExceptionEx.ThrowBusinessException(ex);
            }
        }

        /// <summary>
        /// ���ֶһ���¼
        /// </summary>
        /// <param name="pagination"></param>
        /// <param name="queryJson"></param>
        /// <returns></returns>
        public DataTable GetIntegralGoodRecord(Pagination pagination, string queryJson)
        {
            try
            {
                return dM_IntergralChangeRecordService.GetIntegralGoodRecord(pagination, queryJson);
            }
            catch (Exception ex)
            {
                if (ex is ExceptionEx)
                {
                    throw;
                }
                throw ExceptionEx.ThrowBusinessException(ex);
            }
        }

    }
}
