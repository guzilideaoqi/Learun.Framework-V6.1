using Learun.DataBase.Repository;
using Learun.Util;
using System;
using System.Collections.Generic;
using System.Text;

namespace Learun.Application.TwoDevelopment.DM_APPManage
{
    public class DM_MessageRecordService : RepositoryFactory
    {
        private string fieldSql;

        public DM_MessageRecordService()
        {
            fieldSql = "    t.id,    t.messagetitle,    t.messagecontent,    t.user_id,    t.messagetype,    t.createtime,    t.createcode";
        }

        public IEnumerable<dm_messagerecordEntity> GetList(string queryJson)
        {
            try
            {
                StringBuilder strSql = new StringBuilder();
                strSql.Append("SELECT ");
                strSql.Append(fieldSql);
                strSql.Append(" FROM dm_messagerecord t ");
                return BaseRepository("dm_data").FindList<dm_messagerecordEntity>(strSql.ToString());
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

        public IEnumerable<dm_messagerecordEntity> GetPageList(Pagination pagination, string queryJson)
        {
            try
            {
                var param = queryJson.ToJObject();

                StringBuilder strSql = new StringBuilder();
                strSql.Append("SELECT ");
                strSql.Append(fieldSql);
                strSql.Append(" FROM dm_messagerecord t where 1=1");

                if (!param["user_id"].IsEmpty())
                {
                    strSql.Append(" and t.user_id=" + param["user_id"].ToString());
                }

                return BaseRepository("dm_data").FindList<dm_messagerecordEntity>(strSql.ToString(), pagination);
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

        public dm_messagerecordEntity GetEntity(int? keyValue)
        {
            try
            {
                return BaseRepository("dm_data").FindEntity<dm_messagerecordEntity>(keyValue);
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
                BaseRepository("dm_data").Delete((dm_messagerecordEntity t) => t.id == keyValue);
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

        public void SaveEntity(int? keyValue, dm_messagerecordEntity entity)
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

        #region 未读消息转已读
        /// <summary>
        /// 未读消息转已读(通过用户ID)
        /// </summary>
        /// <param name="user_id"></param>
        public void MessageToReadByUserID(int user_id)
        {
            try
            {
                BaseRepository("dm_data").ExecuteBySql("update dm_messagerecord isread=1 where user_id=" + user_id);
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
        /// 未读消息转已读(通过消息ID)
        /// </summary>
        public void MessageToReadByID(int id)
        {
            BaseRepository("dm_data").ExecuteBySql("update dm_messagerecord isread=1 where id=" + id);
        }
        #endregion
    }
}
