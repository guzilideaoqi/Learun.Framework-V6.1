using Dapper;
using Learun.Cache.Base;
using Learun.Cache.Factory;
using Learun.DataBase.Repository;
using Learun.Util;
using System;
using System.Collections.Generic;
using System.Data;
using System.Text;
using System.Linq;
using Learun.Application.TwoDevelopment.Common;

namespace Learun.Application.TwoDevelopment.DM_APPManage
{
    /// <summary>
    /// 版 本 Learun-ADMS V6.1.6.0 力软敏捷开发框架
    /// Copyright (c) 2013-2017 上海力软信息技术有限公司
    /// 创 建：超级管理员
    /// 日 期：2020-04-16 16:08
    /// 描 述：任务类型
    /// </summary>
    public class DM_Task_TypeService : RepositoryFactory
    {
        private ICache redisCache = CacheFactory.CaChe();

        #region 构造函数和属性

        private string fieldSql;
        public DM_Task_TypeService()
        {
            fieldSql = @"
                t.id,
                t.name,
                t.image,
                t.status,
                t.createtime,
                t.appid
            ";
        }
        #endregion

        #region 获取数据

        /// <summary>
        /// 获取列表数据
        /// <summary>
        /// <returns></returns>
        public IEnumerable<dm_task_typeEntity> GetList(string queryJson)
        {
            try
            {
                var queryParam = queryJson.ToJObject();

                string appid = queryParam["appid"].ToString();
                string cacheKey = "TaskType" + appid;

                IEnumerable<dm_task_typeEntity> dm_Task_TypeEntities = redisCache.Read<IEnumerable<dm_task_typeEntity>>(cacheKey, 7);

                if (dm_Task_TypeEntities == null)
                {
                    //参考写法
                    // 虚拟参数
                    //var dp = new DynamicParameters(new { });
                    //dp.Add("startTime", queryParam["StartTime"].ToDate(), DbType.DateTime);
                    var strSql = new StringBuilder();
                    strSql.Append("SELECT ");
                    strSql.Append(fieldSql);
                    strSql.Append(" FROM dm_task_type t ");

                    if (!appid.IsEmpty())
                    {
                        strSql.Append(" where t.appid='" + appid + "'");
                    }
                    dm_Task_TypeEntities = this.BaseRepository("dm_data").FindList<dm_task_typeEntity>(strSql.ToString());

                    if (dm_Task_TypeEntities.Count() > 0)
                    {
                        redisCache.Write<IEnumerable<dm_task_typeEntity>>(cacheKey, dm_Task_TypeEntities, 7);
                    }
                }

                return dm_Task_TypeEntities;
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
        public IEnumerable<dm_task_typeEntity> GetPageList(Pagination pagination, string queryJson)
        {
            try
            {
                var strSql = new StringBuilder();
                strSql.Append("SELECT ");
                strSql.Append(fieldSql);
                strSql.Append(" FROM dm_task_type t ");
                return this.BaseRepository("dm_data").FindList<dm_task_typeEntity>(strSql.ToString(), pagination);
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
        public dm_task_typeEntity GetEntity(int keyValue)
        {
            try
            {
                return this.BaseRepository("dm_data").FindEntity<dm_task_typeEntity>(keyValue);
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
                this.BaseRepository("dm_data").Delete<dm_task_typeEntity>(t => t.id == keyValue);

                #region 清除缓存
                UserInfo userInfo = LoginUserInfo.Get();
                string cacheKey = "TaskType" + userInfo.companyId;
                redisCache.Read(cacheKey, 7);
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

        /// <summary>
        /// 保存实体数据（新增、修改）
        /// <param name="keyValue">主键</param>
        /// <summary>
        /// <returns></returns>
        public void SaveEntity(int keyValue, dm_task_typeEntity entity)
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

                #region 清除缓存
                UserInfo userInfo = LoginUserInfo.Get();
                string cacheKey = "TaskType" + userInfo.companyId;
                redisCache.Read(cacheKey, 7);
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

    }
}
