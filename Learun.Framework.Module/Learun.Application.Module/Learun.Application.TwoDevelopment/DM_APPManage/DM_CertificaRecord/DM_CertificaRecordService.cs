using Dapper;
using Learun.Cache.Base;
using Learun.Cache.Factory;
using Learun.DataBase.Repository;
using Learun.Util;
using System;
using System.Collections.Generic;
using System.Data;
using System.Text;

namespace Learun.Application.TwoDevelopment.DM_APPManage
{
    /// <summary>
    /// 版 本 Learun-ADMS V6.1.6.0 力软敏捷开发框架
    /// Copyright (c) 2013-2017 上海力软信息技术有限公司
    /// 创 建：超级管理员
    /// 日 期：2020-04-06 21:08
    /// 描 述：身份证实名
    /// </summary>
    public class DM_CertificaRecordService : RepositoryFactory
    {
        private ICache redisCache = CacheFactory.CaChe();

        #region 构造函数和属性

        private string fieldSql;
        public DM_CertificaRecordService()
        {
            fieldSql = @"
                t.id,
                t.user_id,
                t.realname,
                t.cardno,
                t.facecard,
                t.frontcard,
                t.createtime,
                t.updatetime,
                t.remark,
                t.realstatus
            ";
        }
        #endregion

        #region 获取数据

        /// <summary>
        /// 获取列表数据
        /// <summary>
        /// <returns></returns>
        public IEnumerable<dm_certifica_recordEntity> GetList(string queryJson)
        {
            try
            {
                //参考写法
                var queryParam = queryJson.ToJObject();
                // 虚拟参数
                //var dp = new DynamicParameters(new { });
                //dp.Add("startTime", queryParam["StartTime"].ToDate(), DbType.DateTime);
                var strSql = new StringBuilder();
                strSql.Append("SELECT ");
                strSql.Append(fieldSql);
                strSql.Append(" FROM dm_certifica_record t ");
                if (!queryParam["appid"].IsEmpty())
                {
                    strSql.Append(" where t.appid='" + queryParam["appid"].ToString() + "'");
                }
                return this.BaseRepository("dm_data").FindList<dm_certifica_recordEntity>(strSql.ToString());
            }
            catch (Exception ex)
            {
                if (ex is ExceptionEx)
                {
                    throw;
                }
                else
                {
                    throw ExceptionEx.ThrowServiceException(ex);
                }
            }
        }

        /// <summary>
        /// 获取列表分页数据
        /// <param name="pagination">分页参数</param>
        /// <summary>
        /// <returns></returns>
        public IEnumerable<dm_certifica_recordEntity> GetPageList(Pagination pagination, string queryJson)
        {
            try
            {
                var strSql = new StringBuilder();
                strSql.Append("SELECT ");
                strSql.Append(fieldSql);
                strSql.Append(" FROM dm_certifica_record t ");
                return this.BaseRepository("dm_data").FindList<dm_certifica_recordEntity>(strSql.ToString(), pagination);
            }
            catch (Exception ex)
            {
                if (ex is ExceptionEx)
                {
                    throw;
                }
                else
                {
                    throw ExceptionEx.ThrowServiceException(ex);
                }
            }
        }

        /// <summary>
        /// 获取实体数据
        /// <param name="keyValue">主键</param>
        /// <summary>
        /// <returns></returns>
        public dm_certifica_recordEntity GetEntity(int keyValue)
        {
            try
            {
                return this.BaseRepository("dm_data").FindEntity<dm_certifica_recordEntity>(keyValue);
            }
            catch (Exception ex)
            {
                if (ex is ExceptionEx)
                {
                    throw;
                }
                else
                {
                    throw ExceptionEx.ThrowServiceException(ex);
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
                this.BaseRepository("dm_data").Delete<dm_certifica_recordEntity>(t => t.id == keyValue);
            }
            catch (Exception ex)
            {
                if (ex is ExceptionEx)
                {
                    throw;
                }
                else
                {
                    throw ExceptionEx.ThrowServiceException(ex);
                }
            }
        }

        /// <summary>
        /// 保存实体数据（新增、修改）
        /// <param name="keyValue">主键</param>
        /// <summary>
        /// <returns></returns>
        public void SaveEntity(int keyValue, dm_certifica_recordEntity entity)
        {
            try
            {
                if (keyValue > 0)
                {
                    entity.Modify(keyValue);
                    this.BaseRepository("dm_data").Update(entity);
                }
                else
                {
                    entity.Create();
                    this.BaseRepository("dm_data").Insert(entity);
                }
                #region 清除缓存记录
                string cacheKey = "CertificationRecord" + entity.user_id;
                redisCache.Remove(cacheKey, 7);
                #endregion
            }
            catch (Exception ex)
            {
                if (ex is ExceptionEx)
                {
                    throw;
                }
                else
                {
                    throw ExceptionEx.ThrowServiceException(ex);
                }
            }
        }

        #endregion


        #region 获取实名认证记录
        public dm_certifica_recordEntity GetCertificationRecord(int user_id)
        {
            try
            {
                string cacheKey = "CertificationRecord" + user_id;
                dm_certifica_recordEntity dm_Certifica_RecordEntity = redisCache.Read<dm_certifica_recordEntity>(cacheKey);

                if (dm_Certifica_RecordEntity == null)
                {
                    dm_Certifica_RecordEntity = this.BaseRepository("dm_data").FindEntity<dm_certifica_recordEntity>(t => t.user_id == user_id);

                    if (dm_Certifica_RecordEntity != null)
                    {
                        redisCache.Write<dm_certifica_recordEntity>(cacheKey, dm_Certifica_RecordEntity, DateTime.Now.AddHours(2), 7);
                    }
                }

                return dm_Certifica_RecordEntity;
            }
            catch (Exception ex)
            {
                if (ex is ExceptionEx)
                {
                    throw;
                }
                else
                {
                    throw ExceptionEx.ThrowServiceException(ex);
                }
            }
        }
        #endregion

        #region 审核实名认证
        public void CheckCertificationRecord(dm_certifica_recordEntity entity)
        {
            IRepository db = null;
            try
            {
                if (entity.realstatus == 1)
                {
                    dm_userEntity dm_UserEntity = new dm_userEntity();
                    dm_UserEntity.isreal = 1;
                    dm_UserEntity.id = entity.user_id;
                    dm_UserEntity.realname = entity.realname;
                    dm_UserEntity.frontcard = entity.frontcard;
                    dm_UserEntity.facecard = entity.facecard;

                    db = this.BaseRepository("dm_data").BeginTrans();
                    db.Update(dm_UserEntity);
                    db.Update(entity);

                    db.Commit();
                }
                else if (entity.realstatus == 2)
                {
                    this.BaseRepository("dm_data").Update<dm_certifica_recordEntity>(entity);
                }
            }
            catch (Exception ex)
            {
                if (db != null)
                    db.Rollback();
                if (ex is ExceptionEx)
                {
                    throw;
                }
                else
                {
                    throw ExceptionEx.ThrowServiceException(ex);
                }
            }
        }
        #endregion
    }
}
