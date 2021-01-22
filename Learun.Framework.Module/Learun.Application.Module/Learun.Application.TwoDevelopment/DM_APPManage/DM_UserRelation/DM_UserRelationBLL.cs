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

        public dm_user_relationEntity GetEntityByUserID(int? id)
        {
            try
            {
                return dM_UserRelationService.GetEntityByUserID(id);
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

        #region ��ȡ�û���ϵ
        /// <summary>
        /// ���ϻ�ȡ�û���ϵ
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
        /// ���»�ȡ�û���ϵ
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


        #region ��ȡֱ����˿����
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

        #region ��ȡ������˿����
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

        #region ��ȡ�Ŷӷ�˿����
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

        #region ��ȡЧ�����汨��
        public dm_user_relationEntity GetIncomeReport(int User_ID)
        {
            try
            {
                return dM_UserRelationService.GetIncomeReport(User_ID);
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

        #region ���Ļ�Ա�ϼ�������ͳ����Ϣ
        public void UpdateUserParent(int UserID, int ParentID)
        {
            try
            {
                dM_UserRelationService.UpdateUserParent(UserID, ParentID);
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

        #region �����û�ͳ����Ϣ
        public void ResetUserStatistic(int UserID)
        {
            try
            {
                dM_UserRelationService.ResetUserStatistic(UserID);
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
