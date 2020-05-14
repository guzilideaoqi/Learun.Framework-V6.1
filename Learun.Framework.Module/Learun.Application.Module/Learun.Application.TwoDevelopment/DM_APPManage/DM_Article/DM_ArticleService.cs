using Dapper;
using Learun.DataBase.Repository;
using Learun.Util;
using System;
using System.Collections.Generic;
using System.Data;
using System.Text;
using System.Linq;
using Learun.Cache.Base;
using Learun.Cache.Factory;

namespace Learun.Application.TwoDevelopment.DM_APPManage
{
    /// <summary>
    /// 版 本 Learun-ADMS V6.1.6.0 力软敏捷开发框架
    /// Copyright (c) 2013-2017 上海力软信息技术有限公司
    /// 创 建：超级管理员
    /// 日 期：2020-04-06 21:10
    /// 描 述：文案管理
    /// </summary>
    public class DM_ArticleService : RepositoryFactory
    {
        private ICache redisCache = CacheFactory.CaChe();
        #region 构造函数和属性

        private string fieldSql;

        private string childSql;
        public DM_ArticleService()
        {
            fieldSql = @"
                t.id,
                t.title,
                t.content,
                t.parentid,
                t.createtime,
                t.updatetime,
                t.sort,
t.a_image
            ";

            childSql = @"                t.id,
                t.title";
        }
        #endregion

        #region 获取数据

        /// <summary>
        /// 获取列表数据
        /// <summary>
        /// <returns></returns>
        public IEnumerable<dm_articleEntity> GetList(string queryJson)
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
                strSql.Append(" FROM dm_article t where 1=1");

                if (!queryParam["appid"].IsEmpty())
                {
                    strSql.Append(" and t.appid='" + queryParam["appid"].ToString() + "'");
                }

                if (!queryParam["keyword"].IsEmpty())
                {
                    strSql.Append(" and t.title like '%" + queryParam["keyword"].ToString() + "%'");
                }

                return this.BaseRepository("dm_data").FindList<dm_articleEntity>(strSql.ToString());
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
        public IEnumerable<dm_articleEntity> GetPageList(Pagination pagination, string queryJson)
        {
            try
            {
                var strSql = new StringBuilder();
                strSql.Append("SELECT ");
                strSql.Append(fieldSql);
                strSql.Append(" FROM dm_article t ");
                return this.BaseRepository("dm_data").FindList<dm_articleEntity>(strSql.ToString(), pagination);
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
        public dm_articleEntity GetEntity(int keyValue)
        {
            try
            {
                return this.BaseRepository("dm_data").FindEntity<dm_articleEntity>(keyValue);
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
                this.BaseRepository("dm_data").Delete<dm_articleEntity>(t => t.id == keyValue);

                redisCache.Remove("ArticleList", 7);
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
        public void SaveEntity(int keyValue, dm_articleEntity entity)
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

                redisCache.Remove("ArticleList", 7);
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

        #region 获取文章子类
        /// <summary>
        /// 获取文章子类
        /// </summary>
        /// <param name="appid">平台id</param>
        /// <param name="ModeType">父级id</param>
        /// <returns></returns>
        public IEnumerable<dm_articleEntity> GetChildrenArticle(string appid, int ModeType)
        {
            try
            {
                IEnumerable<dm_articleEntity> dm_ArticleEntities = redisCache.Read<IEnumerable<dm_articleEntity>>("ArticleList", 7);

                if (dm_ArticleEntities == null)
                {
                    dm_ArticleEntities = GetList("{\"appid\":\"" + appid + "\"}");
                    if (dm_ArticleEntities != null)
                        redisCache.Write("ArticleList", dm_ArticleEntities, 7);
                }

                return dm_ArticleEntities.Where(t => t.parentid == ModeType);
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

        #region 获取文章详情
        /// <summary>
        /// 获取文章详情
        /// </summary>
        /// <param name="appid">平台id</param>
        /// <param name="id">文章id</param>
        /// <returns></returns>
        public dm_articleEntity GetArticleDetail(string appid, int id)
        {
            try
            {
                IEnumerable<dm_articleEntity> dm_ArticleEntities = redisCache.Read<IEnumerable<dm_articleEntity>>("ArticleList", 7);

                if (dm_ArticleEntities == null)
                {
                    dm_ArticleEntities = GetList("{\"appid\":\"" + appid + "\"}");
                    if (dm_ArticleEntities != null)
                        redisCache.Write("ArticleList", dm_ArticleEntities, 7);
                }

                return dm_ArticleEntities.Where(t => t.id == id).FirstOrDefault();
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
