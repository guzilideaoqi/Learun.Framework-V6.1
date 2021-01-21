using Learun.DataBase.Repository;
using Learun.Util;
using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using System.Data;
using Learun.Cache.Base;
using Learun.Cache.Factory;

namespace Learun.Application.TwoDevelopment.DM_APPManage
{
    public class DM_UserRelationService : RepositoryFactory
    {
        private string fieldSql;
        private ICache redisCache = CacheFactory.CaChe();

        public DM_UserRelationService()
        {
            fieldSql = "    t.id,    t.user_id,    t.parent_id,    t.partners_id,    t.createtime,    t.createcode";
        }

        public IEnumerable<dm_user_relationEntity> GetList(string queryJson)
        {
            try
            {
                StringBuilder strSql = new StringBuilder();
                strSql.Append("SELECT ");
                strSql.Append(fieldSql);
                strSql.Append(" FROM dm_user_relation t ");
                return BaseRepository("dm_data").FindList<dm_user_relationEntity>(strSql.ToString());
            }
            catch (Exception ex)
            {
                if (ex is ExceptionEx)
                {
                    throw;
                }
                throw ExceptionEx.ThrowServiceException(ex);
            }
        }

        public IEnumerable<dm_user_relationEntity> GetPageList(Pagination pagination, string queryJson)
        {
            try
            {
                StringBuilder strSql = new StringBuilder();
                strSql.Append("SELECT ");
                strSql.Append(fieldSql);
                strSql.Append(" FROM dm_user_relation t ");
                return BaseRepository("dm_data").FindList<dm_user_relationEntity>(strSql.ToString(), pagination);
            }
            catch (Exception ex)
            {
                if (ex is ExceptionEx)
                {
                    throw;
                }
                throw ExceptionEx.ThrowServiceException(ex);
            }
        }

        public dm_user_relationEntity GetEntity(int? keyValue)
        {
            try
            {
                return BaseRepository("dm_data").FindEntity<dm_user_relationEntity>(keyValue);
            }
            catch (Exception ex)
            {
                if (ex is ExceptionEx)
                {
                    throw;
                }
                throw ExceptionEx.ThrowServiceException(ex);
            }
        }

        public dm_user_relationEntity GetEntityByUserID(int? id)
        {
            try
            {
                string cacheKey = "UserRelation_" + id;
                dm_user_relationEntity dm_User_RelationEntity = redisCache.Read<dm_user_relationEntity>(cacheKey, 7L);
                if (dm_User_RelationEntity.IsEmpty())
                {
                    dm_User_RelationEntity = BaseRepository("dm_data").FindEntity((dm_user_relationEntity t) => t.user_id == id);
                    if (!dm_User_RelationEntity.IsEmpty())
                    {
                        redisCache.Write(cacheKey, dm_User_RelationEntity, DateTime.Now.AddSeconds(30), 7L);
                    }
                }
                return dm_User_RelationEntity;
            }
            catch (Exception ex)
            {
                if (ex is ExceptionEx)
                {
                    throw;
                }
                throw ExceptionEx.ThrowServiceException(ex);
            }
        }

        public void DeleteEntity(int? keyValue)
        {
            try
            {
                BaseRepository("dm_data").Delete((dm_user_relationEntity t) => t.id == keyValue);
            }
            catch (Exception ex)
            {
                if (ex is ExceptionEx)
                {
                    throw;
                }
                throw ExceptionEx.ThrowServiceException(ex);
            }
        }

        public void SaveEntity(int? keyValue, dm_user_relationEntity entity)
        {
            try
            {
                if (keyValue > 0)
                {
                    entity.Modify(keyValue);
                    BaseRepository("dm_data").Update(entity);
                }
                else
                {
                    entity.Create();
                    BaseRepository("dm_data").Insert(entity);
                }
            }
            catch (Exception ex)
            {
                if (ex is ExceptionEx)
                {
                    throw;
                }
                throw ExceptionEx.ThrowServiceException(ex);
            }
        }

        #region ��ȡ�û���ϵ
        /// <summary>
        /// ���ϻ�ȡ��ϵ
        /// </summary>
        public IEnumerable<dm_user_relationEntity> GetParentRelation(int? user_id)
        {
            try
            {
                return BaseRepository("dm_data").FindList<dm_user_relationEntity>("select * from dm_user_relation where FIND_IN_SET(user_id,getParList(" + user_id + "));");
            }
            catch (Exception ex)
            {
                if (ex is ExceptionEx)
                {
                    throw;
                }
                throw ExceptionEx.ThrowServiceException(ex);
            }
        }

        /// <summary>
        /// ���»�ȡ��ϵ
        /// </summary>
        /// <param name="user_id"></param>
        /// <returns></returns>
        public IEnumerable<dm_user_relationEntity> GetChildRelation(int user_id)
        {
            try
            {
                return BaseRepository("dm_data").FindList<dm_user_relationEntity>("select * from dm_user_relation where FIND_IN_SET(user_id,getChildList(" + user_id + "));");
            }
            catch (Exception ex)
            {
                if (ex is ExceptionEx)
                {
                    throw;
                }
                throw ExceptionEx.ThrowServiceException(ex);
            }
        }
        #endregion

        #region ��ȡֱ���¼�
        public int GetMyChildCount(int user_id)
        {
            return int.Parse(BaseRepository("dm_data").FindObject("select count(1) from dm_user_relation r LEFT JOIN dm_user u on r.user_id=u.id where u.userlevel!=0 and r.parent_id=" + user_id).ToString());
        }
        #endregion

        #region ��ȡ������˿
        public int GetMySonChildCount(int user_id)
        {
            return int.Parse(BaseRepository("dm_data").FindObject("select count(1) from dm_user_relation where parent_id in(select user_id from dm_user_relation where parent_id=" + user_id + ")").ToString());
        }
        #endregion

        #region ��ȡ�Ŷӷ�˿
        public int GetPartnersChildCount(int partners_id)
        {
            return int.Parse(BaseRepository("dm_data").FindObject("select count(1) from dm_user_relation where partners_id=" + partners_id).ToString());
        }
        #endregion

        #region ��ȡ�ҵ�ֱ����˿����
        /// <summary>
        /// ��ȡ�ҵ�ֱ����˿����
        /// </summary>
        /// <param name="user_id">�û�ID</param>
        /// <param name="PageNo">ҳ��</param>
        /// <param name="PageSize">ÿҳ��ʾ����</param>
        /// <returns></returns>
        public DataTable GetMyChildDetail(int user_id, int PageNo, int PageSize)
        {
            try
            {
                string queryChildSql = "select u.nickname,u.headpic,insert(u.phone, 4, 4, '****') as phone,u.userlevel,u.createtime,ur.user_id,ur.ordercount,ur.ReviceTaskCount taskcount,ur.partners_id from dm_user_relation ur left join dm_user u on ur.user_id=u.id where ur.parent_id=" + user_id;
                return BaseRepository("dm_data").FindTable(queryChildSql, new Pagination { page = PageNo, rows = PageSize, sidx = "createtime", sord = "desc" });
            }
            catch (Exception ex)
            {
                if (ex is ExceptionEx)
                {
                    throw;
                }
                throw ExceptionEx.ThrowServiceException(ex);
            }
        }
        #endregion

        #region ��ȡ�ҵĶ�����˿����
        /// <summary>
        /// ��ȡ������˿
        /// </summary>
        /// <param name="user_id">�û�ID</param>
        /// <param name="PageNo">ҳ��</param>
        /// <param name="PageSize">ÿҳ��ʾ����</param>
        /// <returns></returns>
        public DataTable GetMySonChildDetail(int user_id, int PageNo, int PageSize)
        {
            try
            {
                string querySonChildSql = "select u.nickname,u.headpic,insert(u.phone, 4, 4, '****') as phone,u.userlevel,u.createtime,ur.user_id,ur.ordercount,ur.taskcount,ur.partners_id from dm_user_relation ur left join dm_user u on ur.user_id=u.id where ur.parent_id in(select user_id from dm_user_relation where parent_id=" + user_id + ")";
                return BaseRepository("dm_data").FindTable(querySonChildSql, new Pagination { page = PageNo, rows = PageSize, sidx = "createtime", sord = "desc" });
            }
            catch (Exception ex)
            {
                if (ex is ExceptionEx)
                {
                    throw;
                }
                throw ExceptionEx.ThrowServiceException(ex);
            }
        }
        #endregion

        #region ��ȡ�ҵ��Ŷӷ�˿����
        /// <summary>
        /// ��ȡ�ҵ��Ŷӷ�˿����
        /// </summary>
        /// <param name="partners_id">�ϻ���ID</param>
        /// <param name="PageNo">ҳ��</param>
        /// <param name="PageSize">ÿҳ��ʾ����</param>
        /// <returns></returns>
        public DataTable GetPartnersChildDetail(int? partners_id, int PageNo, int PageSize)
        {
            try
            {
                string queryPartnersChildSql = "select u.nickname,u.headpic,insert(u.phone, 4, 4, '****') as phone,u.userlevel,u.createtime,ur.user_id,ur.ordercount,ur.taskcount,ur.partners_id from dm_user_relation ur left join dm_user u on ur.user_id=u.id where ur.partners_id=" + partners_id;
                return BaseRepository("dm_data").FindTable(queryPartnersChildSql, new Pagination { page = PageNo, rows = PageSize, sidx = "createtime", sord = "desc" });
            }
            catch (Exception ex)
            {
                if (ex is ExceptionEx)
                {
                    throw;
                }
                throw ExceptionEx.ThrowServiceException(ex);
            }
        }
        #endregion

        #region ��ȡЧ������
        /// <summary>
        /// ��ȡЧ�����汨��
        /// </summary>
        /// <param name="User_ID">�û�ID</param>
        /// <returns></returns>
        public dm_user_relationEntity GetIncomeReport(int User_ID)
        {
            try
            {
                return GetEntityByUserID(User_ID);
            }
            catch (Exception ex)
            {
                if (ex is ExceptionEx)
                {
                    throw;
                }
                throw ExceptionEx.ThrowServiceException(ex);
            }
        }
        #endregion

        #region ���Ļ�Ա�ϼ�������������Ա��ͳ������
        /// <summary>
        /// ���Ļ�Ա�ϼ�������������Ա��ͳ������
        /// </summary>
        /// <param name="UserID">��Ҫ�����ϼ��Ļ�ԱID</param>
        /// <param name="ParentID">��Ҫ���õ��Ļ�ԱID</param>
        public void UpdateUserParent(int UserID, int ParentID)
        {
            try
            {
                dm_user_relationEntity dm_User_RelationEntity = GetEntityByUserID(UserID);
                if (dm_User_RelationEntity.IsEmpty())
                    throw new Exception("��ǰ��Ա��Ϣ�쳣!");
                dm_user_relationEntity dm_User_ParentRelationEntity = GetEntityByUserID(UserID);
                if (dm_User_ParentRelationEntity.IsEmpty())
                    throw new Exception("�ϼ���Ա��Ϣ�쳣!");

                if (dm_User_RelationEntity.parent_id == ParentID)
                    throw new Exception("��ǰ�û������ڸ������ˣ������ظ�����!");

                if (dm_User_ParentRelationEntity.parent_id == UserID)
                    throw new Exception("�û���ϵ�޷�˫���!");

                if (dm_User_RelationEntity.partners_id != dm_User_ParentRelationEntity.partners_id)
                    throw new Exception("��ǰ���ù�ϵ����ͬһ�Ŷ�,�޷�����!");

                dm_User_RelationEntity.parent_id = ParentID;
                dm_User_RelationEntity.Modify(UserID);
                this.BaseRepository("dm_data").Update(dm_User_RelationEntity);

                #region ����ͳ����Ϣ

                #endregion
            }
            catch (Exception ex)
            {
                if (ex is ExceptionEx)
                {
                    throw;
                }
                throw ExceptionEx.ThrowServiceException(ex);
            }
        }
        #endregion
    }
}
