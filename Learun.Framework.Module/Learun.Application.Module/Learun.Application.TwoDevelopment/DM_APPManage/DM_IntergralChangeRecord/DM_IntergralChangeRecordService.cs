using Learun.DataBase.Repository;
using Learun.Util;
using System;
using System.Collections.Generic;
using System.Data;
using System.Text;

namespace Learun.Application.TwoDevelopment.DM_APPManage
{
    public class DM_IntergralChangeRecordService : RepositoryFactory
    {
        private string fieldSql;

        public DM_IntergralChangeRecordService()
        {
            fieldSql = "    t.id,    t.user_id,    t.goodid,    t.createtime,    t.updatetime,    t.sendstatus,    t.expresscode,    t.remark,    t.appid,    t.province,    t.city,    t.down,    t.address,    t.phone,    t.username";
        }

        public IEnumerable<dm_intergralchangerecordEntity> GetList(string queryJson)
        {
            try
            {
                StringBuilder strSql = new StringBuilder();
                strSql.Append("SELECT ");
                strSql.Append(fieldSql);
                strSql.Append(" FROM dm_intergralchangerecord t ");
                return BaseRepository("dm_data").FindList<dm_intergralchangerecordEntity>(strSql.ToString());
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

        public IEnumerable<dm_intergralchangerecordEntity> GetPageList(Pagination pagination, string queryJson)
        {
            try
            {
                var queryParam = queryJson.ToJObject();
                StringBuilder strSql = new StringBuilder();
                strSql.Append("SELECT ");
                strSql.Append(fieldSql);
                strSql.Append(" FROM dm_intergralchangerecord t where 1=1");

                if (!queryParam["txt_phone"].IsEmpty())
                    strSql.Append(" and t.phone like '%" + queryParam["txt_phone"].ToString() + "%'");
                if (!queryParam["txt_username"].IsEmpty())
                    strSql.Append(" and t.username like '%" + queryParam["txt_username"].ToString() + "%'");
                if (!queryParam["txt_expresscode"].IsEmpty())
                    strSql.Append(" and t.expresscode like '%" + queryParam["txt_expresscode"].ToString() + "%'");

                return BaseRepository("dm_data").FindList<dm_intergralchangerecordEntity>(strSql.ToString(), pagination);
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

        public dm_intergralchangerecordEntity GetEntity(int keyValue)
        {
            try
            {
                return BaseRepository("dm_data").FindEntity<dm_intergralchangerecordEntity>(keyValue);
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

        public void DeleteEntity(int keyValue)
        {
            try
            {
                BaseRepository("dm_data").Delete((dm_intergralchangerecordEntity t) => t.id == (int?)keyValue);
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

        public void SaveEntity(int keyValue, dm_intergralchangerecordEntity entity)
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

        public void ApplyChangeGood(dm_intergralchangerecordEntity dm_IntergralchangerecordEntity, dm_userEntity dm_UserEntity)
        {
            IRepository db = BaseRepository("dm_data").BeginTrans();
            try
            {
                DM_IntergralChangeGoodService dM_IntergralChangeGoodService = new DM_IntergralChangeGoodService();
                dm_intergralchangegoodEntity dm_IntergralchangegoodEntity = dM_IntergralChangeGoodService.GetEntity(dm_IntergralchangerecordEntity.goodid.ToInt());
                if (dm_IntergralchangegoodEntity == null)
                {
                    throw new Exception("该商品不存在!");
                }
                if (!(dm_UserEntity.integral >= dm_IntergralchangegoodEntity.needintergral))
                {
                    throw new Exception("账户积分不足!");
                }

                dm_UserEntity.integral -= dm_IntergralchangegoodEntity.needintergral;
                dm_intergraldetailEntity dm_IntergraldetailEntity = new dm_intergraldetailEntity()
                {
                    currentvalue = dm_UserEntity.integral,
                    stepvalue = dm_IntergralchangegoodEntity.needintergral,
                    user_id = dm_UserEntity.id,
                    type = 6,
                    title = "兑换商品",
                    remark = "使用积分兑换商品--" + dm_IntergralchangegoodEntity.goodtitle,
                    profitLoss = 2
                };
                dm_IntergraldetailEntity.Create();

                db.Insert(dm_IntergraldetailEntity);
                db.Insert(dm_IntergralchangerecordEntity);
                dm_UserEntity.Modify(dm_UserEntity.id);
                db.Update(dm_UserEntity);
                db.Commit();
            }
            catch (Exception ex)
            {
                db.Rollback();
                if (ex is ExceptionEx)
                {
                    throw;
                }
                throw ExceptionEx.ThrowServiceException(ex);
            }
        }

        #region 获取我的积分兑换记录
        public DataTable GetMyIntegralGoodRecord(int UserID, Pagination pagination)
        {
            try
            {
                StringBuilder strSql = new StringBuilder();
                strSql.Append("select g.id,goodtitle,goodremark,needintergral,goodimage,goodprice,r.createtime from dm_intergralchangerecord r left join dm_intergralchangegood g on r.goodid=g.id where r.user_id=" + UserID);
                return BaseRepository("dm_data").FindTable(strSql.ToString(), pagination);
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

        #region 积分兑换记录
        public DataTable GetIntegralGoodRecord(Pagination pagination, string queryJson)
        {
            try
            {
                var queryParam = queryJson.ToJObject();

                StringBuilder strSql = new StringBuilder();
                strSql.Append("select r.*,g.goodtitle,g.goodremark,g.goodimage from dm_intergralchangerecord r left join dm_intergralchangegood g on r.goodid=g.id where 1=1");

                if (!queryParam["txt_phone"].IsEmpty())
                    strSql.Append(" and t.phone like '%" + queryParam["txt_phone"].ToString() + "%'");
                if (!queryParam["txt_username"].IsEmpty())
                    strSql.Append(" and t.username like '%" + queryParam["txt_username"].ToString() + "%'");
                if (!queryParam["txt_expresscode"].IsEmpty())
                    strSql.Append(" and t.expresscode like '%" + queryParam["txt_expresscode"].ToString() + "%'");

                return BaseRepository("dm_data").FindTable(strSql.ToString(), pagination);
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
