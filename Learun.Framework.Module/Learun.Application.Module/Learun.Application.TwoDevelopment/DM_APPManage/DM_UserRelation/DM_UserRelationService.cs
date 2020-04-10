using Learun.DataBase.Repository;
using Learun.Util;
using System;
using System.Collections.Generic;
using System.Text;

namespace Learun.Application.TwoDevelopment.DM_APPManage
{
    public class DM_UserRelationService : RepositoryFactory
    {
        private string fieldSql;

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
                return BaseRepository("dm_data").FindEntity((dm_user_relationEntity t) => t.user_id == id);
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

        #region 获取用户关系
        /// <summary>
        /// 向上获取关系
        /// </summary>
        public IEnumerable<dm_user_relationEntity> GetParentRelation(int user_id)
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
        /// 向下获取关系
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
    }
}
