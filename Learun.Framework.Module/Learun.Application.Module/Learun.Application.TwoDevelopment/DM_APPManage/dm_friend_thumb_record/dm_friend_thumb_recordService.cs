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
    /// 日 期：2020-11-09 10:21
    /// 描 述：点赞记录
    /// </summary>
    public class dm_friend_thumb_recordService : RepositoryFactory
    {
        #region 构造函数和属性

        private string fieldSql;
        public dm_friend_thumb_recordService()
        {
            fieldSql = @"
                t.id,
                t.user_id,
                t.friend_id,
                t.status,
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
        public IEnumerable<dm_friend_thumb_recordEntity> GetList(string queryJson)
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
                strSql.Append(" FROM dm_friend_thumb_record t ");
                return this.BaseRepository("dm_data").FindList<dm_friend_thumb_recordEntity>(strSql.ToString());
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
        public IEnumerable<dm_friend_thumb_recordEntity> GetPageList(Pagination pagination, string queryJson)
        {
            try
            {
                var strSql = new StringBuilder();
                strSql.Append("SELECT ");
                strSql.Append(fieldSql);
                strSql.Append(" FROM dm_friend_thumb_record t ");
                return this.BaseRepository("dm_data").FindList<dm_friend_thumb_recordEntity>(strSql.ToString(), pagination);
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
        public dm_friend_thumb_recordEntity GetEntity(int keyValue)
        {
            try
            {
                return this.BaseRepository("dm_data").FindEntity<dm_friend_thumb_recordEntity>(keyValue);
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
                this.BaseRepository("dm_data").Delete<dm_friend_thumb_recordEntity>(t => t.id == keyValue);
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
        public void SaveEntity(int keyValue, dm_friend_thumb_recordEntity entity)
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

        #region 点赞扩展
        /// <summary>
        /// 点赞
        /// </summary>
        /// <param name="dm_Friend_Thumb_RecordEntity"></param>
        public void ClickPraise(dm_friend_thumb_recordEntity dm_Friend_Thumb_RecordEntity)
        {
            try
            {
                dm_friend_thumb_recordEntity dm_Friend_Thumb_RecordEntity_Exist = this.BaseRepository("dm_data").FindEntity<dm_friend_thumb_recordEntity>(t => t.user_id == dm_Friend_Thumb_RecordEntity.user_id && t.friend_id == dm_Friend_Thumb_RecordEntity.friend_id);
                if (dm_Friend_Thumb_RecordEntity_Exist.IsEmpty())
                {//不存在点赞记录
                    dm_Friend_Thumb_RecordEntity.status = 1;
                    dm_Friend_Thumb_RecordEntity.Create();
                    this.BaseRepository("dm_data").Insert<dm_friend_thumb_recordEntity>(dm_Friend_Thumb_RecordEntity);
                }
                else
                {
                    dm_Friend_Thumb_RecordEntity_Exist.status = 1;
                    dm_Friend_Thumb_RecordEntity_Exist.Modify(dm_Friend_Thumb_RecordEntity_Exist.id);
                    this.BaseRepository("dm_data").Update(dm_Friend_Thumb_RecordEntity_Exist);
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

        /// <summary>
        /// 取消点赞
        /// </summary>
        /// <param name="dm_Friend_Thumb_RecordEntity"></param>
        public void CanclePraise(dm_friend_thumb_recordEntity dm_Friend_Thumb_RecordEntity)
        {
            try
            {
                dm_Friend_Thumb_RecordEntity = this.BaseRepository("dm_data").FindEntity<dm_friend_thumb_recordEntity>(t => t.user_id == dm_Friend_Thumb_RecordEntity.user_id && t.friend_id == dm_Friend_Thumb_RecordEntity.friend_id);
                if (dm_Friend_Thumb_RecordEntity.IsEmpty())
                {
                    throw new Exception("未找到点赞记录");
                }
                else
                {
                    dm_Friend_Thumb_RecordEntity.status = 0;
                    dm_Friend_Thumb_RecordEntity.Modify(dm_Friend_Thumb_RecordEntity.id);
                    this.BaseRepository("dm_data").Update(dm_Friend_Thumb_RecordEntity);
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

        /// <summary>
        /// 获取点赞记录
        /// </summary>
        /// <param name="friend_ids"></param>
        /// <returns></returns>
        public DataTable GetPraiseRecord(List<int> friend_ids)
        {
            try
            {
                return this.BaseRepository("dm_data").FindTable("select ft.friend_id,u.headpic from dm_friend_thumb_record ft LEFT JOIN dm_user u on ft.user_id=u.id where ft.friend_id in (" + string.Join(",", friend_ids) + ") and ft.status=1");
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
        /// 获取我的点赞记录
        /// </summary>
        /// <param name="friend_ids"></param>
        /// <param name="User_ID"></param>
        /// <returns></returns>
        public IEnumerable<dm_friend_thumb_recordEntity> GetPraiseRecord(List<int> friend_ids, int User_ID)
        {
            try
            {
                return this.BaseRepository("dm_data").FindList<dm_friend_thumb_recordEntity>("select friend_id,user_id,status from dm_friend_thumb_record where user_id=" + User_ID + " and friend_id in (" + string.Join(",", friend_ids) + ") and status=1");
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
