using Learun.DataBase.Repository;
using Learun.Util;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Learun.Application.TwoDevelopment.DM_APPManage
{
    public class DM_IntergralDetailService : RepositoryFactory
    {
        private string fieldSql;

        public DM_IntergralDetailService()
        {
            fieldSql = "    t.id,    t.currentvalue,    t.stepvalue,    t.user_id,    t.title,    t.type,    t.remark,    t.createtime,    t.createcode,t.profitLoss";
        }

        public IEnumerable<dm_intergraldetailEntity> GetList(string queryJson)
        {
            try
            {
                StringBuilder strSql = new StringBuilder();
                strSql.Append("SELECT ");
                strSql.Append(fieldSql);
                strSql.Append(" FROM dm_intergraldetail t ");
                return BaseRepository("dm_data").FindList<dm_intergraldetailEntity>(strSql.ToString());
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

        public IEnumerable<dm_intergraldetailEntity> GetPageList(Pagination pagination, string queryJson)
        {
            try
            {
                var param = queryJson.ToJObject();
                StringBuilder strSql = new StringBuilder();
                strSql.Append("SELECT ");
                strSql.Append(fieldSql);
                strSql.Append(" FROM dm_intergraldetail t where 1=1");

                if (!param["type"].IsEmpty())
                {
                    strSql.Append(" and type='" + param["type"].ToString() + "'");
                }

                if (!param["user_id"].IsEmpty())
                {
                    strSql.Append(" and user_id='" + param["user_id"].ToString() + "'");
                }

                return BaseRepository("dm_data").FindList<dm_intergraldetailEntity>(strSql.ToString(), pagination);
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

        public dm_intergraldetailEntity GetEntity(int? keyValue)
        {
            try
            {
                return BaseRepository("dm_data").FindEntity<dm_intergraldetailEntity>(keyValue);
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
                BaseRepository("dm_data").Delete((dm_intergraldetailEntity t) => t.id == keyValue);
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

        public void SaveEntity(int? keyValue, dm_intergraldetailEntity entity)
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

        public dm_intergraldetailEntity GetLastSignData(int user_id)
        {
            return (from t in BaseRepository("dm_data").IQueryable((dm_intergraldetailEntity t) => t.user_id == (int?)user_id && t.type == 2)
                    orderby t.createtime descending
                    select t).Take(1).FirstOrDefault();
        }
    }
}
