using Dapper;
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
    /// 日 期：2020-10-28 10:06
    /// 描 述：官推文案
    /// </summary>
    public class dm_friend_circleService : RepositoryFactory
    {
        #region 构造函数和属性

        private string fieldSql;
        public dm_friend_circleService()
        {
            fieldSql = @"
                t.id,
                t.t_content,
                t.t_type,
                t.t_images,
                t.t_sort,
t.t_status,
                t.createcode,
                t.createtime
            ";
        }
        #endregion

        #region 获取数据

        /// <summary>
        /// 获取列表数据
        /// <summary>
        /// <returns></returns>
        public IEnumerable<dm_friend_circleEntity> GetList(string queryJson)
        {
            try
            {
                //参考写法
                //var queryParam = queryJson.ToJObject();
                // 虚拟参数
                //var dp = new DynamicParameters(new { });
                //dp.Add("startTime", queryParam["StartTime"].ToDate(), DbType.DateTime);
                var strSql = new StringBuilder();
                strSql.Append("SELECT ");
                strSql.Append(fieldSql);
                strSql.Append(" FROM dm_friend_circle t ");
                return this.BaseRepository("dm_data").FindList<dm_friend_circleEntity>(strSql.ToString());
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
        public IEnumerable<dm_friend_circleEntity> GetPageList(Pagination pagination, string queryJson)
        {
            try
            {
                var queryParam = queryJson.ToJObject();
                var strSql = new StringBuilder();
                strSql.Append("SELECT ");
                strSql.Append(fieldSql);
                strSql.Append(" FROM dm_friend_circle t where 1=1 ");

                if (!queryParam["t_type"].IsEmpty())
                {
                    strSql.Append(" and t_type='" + queryParam["t_type"].ToString() + "'");
                }

                if (!queryParam["t_status"].IsEmpty())
                {
                    strSql.Append(" and t_status='" + queryParam["t_status"].ToString() + "'");
                }

                return this.BaseRepository("dm_data").FindList<dm_friend_circleEntity>(strSql.ToString(), pagination);
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
        public dm_friend_circleEntity GetEntity(int keyValue)
        {
            try
            {
                return this.BaseRepository("dm_data").FindEntity<dm_friend_circleEntity>(keyValue);
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
                this.BaseRepository("dm_data").Delete<dm_friend_circleEntity>(t => t.id == keyValue);
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
        public void SaveEntity(int keyValue, dm_friend_circleEntity entity)
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

        #region API EXTEND METHOD
        /// <summary>
        /// 获取官推任务
        /// </summary>
        /// <returns></returns>
        public IEnumerable<dm_friend_circleEntity> GetCircleByGovernment(Pagination pagination, string appid)
        {
            try
            {

                return this.BaseRepository("dm_data").FindList<dm_friend_circleEntity>(t => t.appid == appid && t.t_type == 1 && t.t_status == 2, pagination);
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
        /// 获取普通任务
        /// </summary>
        /// <param name="pagination"></param>
        /// <param name="appid"></param>
        /// <returns></returns>
        public DataTable GetCircleByGeneral(Pagination pagination, string appid)
        {
            try
            {
                DataTable dataTable = this.BaseRepository("dm_data").FindTable("select  f.*,u.nickname,u.headpic from dm_friend_circle f left join dm_user u on f.createcode=u.id where f.t_type=0 and f.t_status=1 and f.appid='" + appid + "'", pagination);
                return dataTable;
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
        /// 获取单条哆米圈任务
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public dm_friend_circleEntity GetSingleCircle(int id)
        {
            try
            {
                dm_friend_circleEntity dm_Friend_CircleEntity = this.BaseRepository("dm_data").FindEntity<dm_friend_circleEntity>(id);
                return dm_Friend_CircleEntity;
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
        /// 获取我的哆米圈
        /// </summary>
        /// <param name="pagination"></param>
        /// <param name="User_ID"></param>
        /// <returns></returns>
        public DataTable GetMyCircle(Pagination pagination, string User_ID)
        {
            try
            {
                DataTable dataTable = this.BaseRepository("dm_data").FindTable("select  f.*,u.nickname,u.headpic from dm_friend_circle f left join dm_user u on f.createcode=u.id where u.id=" + User_ID, pagination);
                return dataTable;
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
        /// 发布哆米圈文章
        /// </summary>
        /// <param name="AppID">站长id</param>
        /// <param name="Content">文章内容</param>
        /// <param name="Images">图片信息</param>
        /// <param name="User_ID">用户信息</param>
        public void PubCircle(string AppID, string Content, string Images, string User_ID)
        {
            try
            {
                dm_friend_circleEntity dm_Friend_CircleEntity = new dm_friend_circleEntity();
                dm_Friend_CircleEntity.createcode = User_ID;
                dm_Friend_CircleEntity.t_type = 0;
                dm_Friend_CircleEntity.t_status = 0;
                dm_Friend_CircleEntity.appid = AppID;
                dm_Friend_CircleEntity.t_content = Content;
                dm_Friend_CircleEntity.t_images = Images;
                dm_Friend_CircleEntity.Create();

                SaveEntity(0, dm_Friend_CircleEntity);
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
