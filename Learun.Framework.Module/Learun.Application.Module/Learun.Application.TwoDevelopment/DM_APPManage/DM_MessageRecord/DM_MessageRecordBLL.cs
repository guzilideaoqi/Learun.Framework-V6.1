using Learun.Util;
using System;
using System.Collections.Generic;

namespace Learun.Application.TwoDevelopment.DM_APPManage
{
    public class DM_MessageRecordBLL : DM_MessageRecordIBLL
    {
        private DM_MessageRecordService dM_MessageRecordService = new DM_MessageRecordService();

        public IEnumerable<dm_messagerecordEntity> GetList(string queryJson)
        {
            try
            {
                return dM_MessageRecordService.GetList(queryJson);
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

        public IEnumerable<dm_messagerecordEntity> GetPageList(Pagination pagination, string queryJson)
        {
            try
            {
                return dM_MessageRecordService.GetPageList(pagination, queryJson);
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

        public dm_messagerecordEntity GetEntity(int? keyValue)
        {
            try
            {
                return dM_MessageRecordService.GetEntity(keyValue);
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

        public void DeleteEntity(int? keyValue)
        {
            try
            {
                dM_MessageRecordService.DeleteEntity(keyValue);
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

        public void SaveEntity(int? keyValue, dm_messagerecordEntity entity)
        {
            try
            {
                dM_MessageRecordService.SaveEntity(keyValue, entity);
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

        #region 未读消息转已读
        /// <summary>
        /// 未读消息转已读(通过用户ID)
        /// </summary>
        /// <param name="user_id"></param>
        public void MessageToReadByUserID(int user_id)
        {
            try
            {
                dM_MessageRecordService.MessageToReadByUserID(user_id);
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
        /// 未读消息转已读(通过消息ID)
        /// </summary>
        /// <param name="id"></param>
        public void MessageToReadByID(int id)
        {
            try
            {
                dM_MessageRecordService.MessageToReadByID(id);
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
        #endregion
    }
}
