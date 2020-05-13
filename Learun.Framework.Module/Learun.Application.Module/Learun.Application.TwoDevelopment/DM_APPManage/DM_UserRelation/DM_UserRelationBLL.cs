using Learun.Util;
using System;
using System.Collections.Generic;
using System.Data;

namespace Learun.Application.TwoDevelopment.DM_APPManage
{
    public class DM_UserRelationBLL : DM_UserRelationIBLL
    {
        private DM_UserRelationService dM_UserRelationService = new DM_UserRelationService();

        public IEnumerable<dm_user_relationEntity> GetList(string queryJson)
        {
            try
            {
                return dM_UserRelationService.GetList(queryJson);
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

        public IEnumerable<dm_user_relationEntity> GetPageList(Pagination pagination, string queryJson)
        {
            try
            {
                return dM_UserRelationService.GetPageList(pagination, queryJson);
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

        public dm_user_relationEntity GetEntity(int? keyValue)
        {
            try
            {
                return dM_UserRelationService.GetEntity(keyValue);
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
                dM_UserRelationService.DeleteEntity(keyValue);
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

        public void SaveEntity(int? keyValue, dm_user_relationEntity entity)
        {
            try
            {
                dM_UserRelationService.SaveEntity(keyValue, entity);
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

        #region 获取用户关系
        /// <summary>
        /// 向上获取用户关系
        /// </summary>
        /// <param name="user_id"></param>
        /// <returns></returns>
        public IEnumerable<dm_user_relationEntity> GetParentRelation(int user_id)
        {
            try
            {
                return dM_UserRelationService.GetParentRelation(user_id);
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
        /// 向下获取用户关系
        /// </summary>
        /// <param name="user_id"></param>
        /// <returns></returns>
        public IEnumerable<dm_user_relationEntity> GetChildRelation(int user_id)
        {
            try
            {
                return dM_UserRelationService.GetChildRelation(user_id);
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


        #region 获取直属粉丝详情
        public DataTable GetMyChildDetail(int user_id, int PageNo, int PageSize)
        {
            try
            {
                return dM_UserRelationService.GetMyChildDetail(user_id, PageNo, PageSize);
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

        #region 获取二级粉丝详情
        public DataTable GetMySonChildDetail(int user_id, int PageNo, int PageSize)
        {
            try
            {
                return dM_UserRelationService.GetMySonChildDetail(user_id, PageNo, PageSize);
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

        #region 获取团队粉丝详情
        public DataTable GetPartnersChildDetail(int? partners_id, int PageNo, int PageSize)
        {
            try
            {
                return dM_UserRelationService.GetPartnersChildDetail(partners_id, PageNo, PageSize);
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

        #region 获取效果收益报表
        public dm_user_relationEntity GetIncomeReport(int User_ID)
        {
            try
            {
               return dM_UserRelationService.GetIncomeReport(User_ID);
            }
            catch (Exception ex)
            {
                if(ex is ExceptionEx)
                {
                    throw;
                }
                throw ExceptionEx.ThrowBusinessException(ex);
            }
        }
        #endregion
    }
}
