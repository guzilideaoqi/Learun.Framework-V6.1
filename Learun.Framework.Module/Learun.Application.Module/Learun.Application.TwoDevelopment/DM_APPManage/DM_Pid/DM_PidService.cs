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
    /// 日 期：2020-04-07 07:57
    /// 描 述：pid管理
    /// </summary>
    public class DM_PidService : RepositoryFactory
    {
        #region 构造函数和属性

        private string fieldSql;
        public DM_PidService()
        {
            fieldSql = @"
                t.id,
                t.pid,
                t.pidname,
                t.pids,
                t.type,
                t.createtime,
                t.usestate,
                t.user_id
            ";
        }
        #endregion

        #region 获取数据

        /// <summary>
        /// 获取列表数据
        /// <summary>
        /// <returns></returns>
        public IEnumerable<dm_pidEntity> GetList(string queryJson)
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
                strSql.Append(" FROM dm_pid t ");
                return this.BaseRepository("dm_data").FindList<dm_pidEntity>(strSql.ToString());
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
        public IEnumerable<dm_pidEntity> GetPageList(Pagination pagination, string queryJson)
        {
            try
            {
                var strSql = new StringBuilder();
                strSql.Append("SELECT ");
                strSql.Append(fieldSql);
                strSql.Append(" FROM dm_pid t ");
                return this.BaseRepository("dm_data").FindList<dm_pidEntity>(strSql.ToString(), pagination);
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
        public dm_pidEntity GetEntity(int keyValue)
        {
            try
            {
                return this.BaseRepository("dm_data").FindEntity<dm_pidEntity>(keyValue);
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
                this.BaseRepository("dm_data").Delete<dm_pidEntity>(t => t.id == keyValue);
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
        public void SaveEntity(int keyValue, dm_pidEntity entity)
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

        #region 自动分配京东PID
        /// <summary>
        /// 自动分配京东pid
        /// </summary>
        public dm_userEntity AutoAssignJDPID(dm_userEntity dm_UserEntity)
        {
            IRepository db = null;
            try
            {
                dm_pidEntity dm_PidEntity = this.BaseRepository("dm_data").FindEntity<dm_pidEntity>("select * from dm_pid where usestate=0 and type=2 limit 1",null);
                if (dm_PidEntity == null)
                    throw new Exception("无可用京东PID，请联系客服!");
                string site_id = dm_PidEntity.pids.Split('_')[1];
                dm_UserEntity.jd_site = site_id;
                dm_UserEntity.jd_pid = dm_PidEntity.pid;

                dm_PidEntity.user_id = dm_UserEntity.id;
                dm_PidEntity.usestate = 1;
                dm_PidEntity.usetime = DateTime.Now;

                db = this.BaseRepository("dm_data").BeginTrans();
                db.Update(dm_UserEntity);
                db.Update(dm_PidEntity);

                db.Commit();

                return dm_UserEntity;
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

        #region 自动分配拼多多PID
        /// <summary>
        /// 自动分配拼多多pid
        /// </summary>
        public dm_userEntity AutoAssignPDDPID(dm_userEntity dm_UserEntity)
        {
            IRepository db = null;
            try
            {
                dm_pidEntity dm_PidEntity = this.BaseRepository("dm_data").FindEntity<dm_pidEntity>("select * from dm_pid where usestate=0 and type=3 limit 1",null);
                if (dm_PidEntity == null)
                    throw new Exception("无可用拼多多PID，请联系客服!");
                dm_UserEntity.pdd_pid = dm_PidEntity.pid;

                dm_PidEntity.user_id = dm_UserEntity.id;
                dm_PidEntity.usestate = 1;
                dm_PidEntity.usetime = DateTime.Now;

                db = this.BaseRepository("dm_data").BeginTrans();
                db.Update(dm_UserEntity);
                db.Update(dm_PidEntity);

                db.Commit();

                return dm_UserEntity;
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
