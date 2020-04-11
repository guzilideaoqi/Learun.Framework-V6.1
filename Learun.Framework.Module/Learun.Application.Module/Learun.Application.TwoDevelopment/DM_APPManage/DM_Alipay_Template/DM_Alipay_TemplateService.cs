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

namespace Learun.Application.TwoDevelopment.DM_APPManage
{
    /// <summary>
    /// 版 本 Learun-ADMS V6.1.6.0 力软敏捷开发框架
    /// Copyright (c) 2013-2017 上海力软信息技术有限公司
    /// 创 建：超级管理员
    /// 日 期：2020-04-10 13:52
    /// 描 述：套餐模板
    /// </summary>
    public class DM_Alipay_TemplateService : RepositoryFactory
    {
        private ICache redisCache = CacheFactory.CaChe();

        #region 构造函数和属性

        private string fieldSql;
        public DM_Alipay_TemplateService()
        {
            fieldSql = @"
                t.id,
                t.name,
                t.goodprice,
                t.finishprice,
                t.isactive,
                t.createtime,
                t.updatetime
            ";
        }
        #endregion

        #region 获取数据
        /// <summary>
        /// 获取列表数据
        /// <summary>
        /// <returns></returns>
        public IEnumerable<dm_alipay_templateEntity> GetList(string queryJson)
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
                strSql.Append(" FROM dm_alipay_template t ");

                if (!queryParam["appid"].IsEmpty())
                {
                    strSql.Append(" where appid='" + queryParam["appid"].ToString() + "'");
                }

                return this.BaseRepository("dm_data").FindList<dm_alipay_templateEntity>(strSql.ToString());
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
        public IEnumerable<dm_alipay_templateEntity> GetPageList(Pagination pagination, string queryJson)
        {
            try
            {
                var strSql = new StringBuilder();
                strSql.Append("SELECT ");
                strSql.Append(fieldSql);
                strSql.Append(" FROM dm_alipay_template t ");
                return this.BaseRepository("dm_data").FindList<dm_alipay_templateEntity>(strSql.ToString(), pagination);
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
        public dm_alipay_templateEntity GetEntity(int keyValue)
        {
            try
            {
                return this.BaseRepository("dm_data").FindEntity<dm_alipay_templateEntity>(keyValue);
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
                this.BaseRepository("dm_data").Delete<dm_alipay_templateEntity>(t => t.id == keyValue);

                #region 清除缓存信息
                UserInfo loginUserInfo = LoginUserInfo.Get();
                string cacheKey = "AliPayTemplate" + loginUserInfo.appId;
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

        /// <summary>
        /// 保存实体数据（新增、修改）
        /// <param name="keyValue">主键</param>
        /// <summary>
        /// <returns></returns>
        public void SaveEntity(int keyValue, dm_alipay_templateEntity entity)
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

                #region 清除缓存信息
                UserInfo loginUserInfo = LoginUserInfo.Get();
                string cacheKey = "AliPayTemplate" + loginUserInfo.appId;
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

        #region 从缓存中读取套餐信息
        public IEnumerable<dm_alipay_templateEntity> GetListByCache(string appid)
        {
            try
            {
                string cacheKey = "AliPayTemplate" + appid;
                IEnumerable<dm_alipay_templateEntity> dm_Alipay_TemplateEntities = redisCache.Read<IEnumerable<dm_alipay_templateEntity>>(cacheKey, 7);
                if (dm_Alipay_TemplateEntities.IsEmpty())
                {
                    dm_Alipay_TemplateEntities = GetList("{\"appid\":\"" + appid + "\"}");

                    if (dm_Alipay_TemplateEntities.Count() > 0)
                        redisCache.Write<IEnumerable<dm_alipay_templateEntity>>(cacheKey, dm_Alipay_TemplateEntities, 7);
                }

                return dm_Alipay_TemplateEntities;
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

        public dm_alipay_templateEntity GetEntityByCache(int templateID, string appid)
        {
            try
            {
                dm_alipay_templateEntity dm_Alipay_TemplateEntity = GetListByCache(appid).Where(t => t.id == templateID).FirstOrDefault();

                return dm_Alipay_TemplateEntity;
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
