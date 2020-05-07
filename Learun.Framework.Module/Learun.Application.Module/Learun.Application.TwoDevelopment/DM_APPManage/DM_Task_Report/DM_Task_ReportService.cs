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
    /// 日 期：2020-04-16 16:02
    /// 描 述：任务举报记录
    /// </summary>
    public class DM_Task_ReportService : RepositoryFactory
    {
        #region 构造函数和属性

        private string fieldSql;
        public DM_Task_ReportService()
        {
            fieldSql = @"
                t.id,
                t.user_id,
                t.task_id,
                t.report_content,
                t.report_time,
                t.appid
            ";
        }
        #endregion

        #region 获取数据

        /// <summary>
        /// 获取列表数据
        /// <summary>
        /// <returns></returns>
        public IEnumerable<dm_task_reportEntity> GetList(string queryJson)
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
                strSql.Append(" FROM dm_task_report t ");
                return this.BaseRepository("dm_data").FindList<dm_task_reportEntity>(strSql.ToString());
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
        public IEnumerable<dm_task_reportEntity> GetPageList(Pagination pagination, string queryJson)
        {
            try
            {
                var strSql = new StringBuilder();
                strSql.Append("SELECT ");
                strSql.Append(fieldSql);
                strSql.Append(" FROM dm_task_report t ");
                return this.BaseRepository("dm_data").FindList<dm_task_reportEntity>(strSql.ToString(), pagination);
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

        public DataTable GetPageListByDataTable(Pagination pagination, string queryJson)
        {
            try
            {
                var queryParam = queryJson.ToJObject();

                var strSql = new StringBuilder();
                strSql.Append("SELECT r.*,u.phone,u.nickname,u.realname,t.task_no,t.task_title from dm_task_report r left join dm_user u on r.user_id = u.id LEFT JOIN dm_task t on r.task_id = t.id ");

                if (queryParam["AppID"].IsEmpty())
                {
                    UserInfo userInfo = LoginUserInfo.Get();
                    strSql.Append(" where r.appid='" + userInfo.companyId + "'");
                }
                else
                {
                    strSql.Append(" where r.appid='" + queryParam["AppID"].ToString() + "'");
                }

                if (!queryParam["txt_phone"].IsEmpty())
                {
                    strSql.Append(" and u.phone like '%" + queryParam["txt_phone"].ToString() + "%'");
                }

                if (!queryParam["txt_nickname"].IsEmpty())
                {
                    strSql.Append(" and u.nickname like '%" + queryParam["txt_nickname"].ToString() + "%'");
                }

                if (!queryParam["txt_realname"].IsEmpty())
                {
                    strSql.Append(" and u.realname like '%" + queryParam["txt_realname"].ToString() + "%'");
                }

                if (!queryParam["txt_task_no"].IsEmpty())
                {
                    strSql.Append(" and t.task_no like '%" + queryParam["txt_task_no"].ToString() + "%'");
                }

                return this.BaseRepository("dm_data").FindTable(strSql.ToString(), pagination);
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
        public dm_task_reportEntity GetEntity(int keyValue)
        {
            try
            {
                return this.BaseRepository("dm_data").FindEntity<dm_task_reportEntity>(keyValue);
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
                this.BaseRepository("dm_data").Delete<dm_task_reportEntity>(t => t.id == keyValue);
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
        public void SaveEntity(int keyValue, dm_task_reportEntity entity)
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

        public void SubmitTaskReport(dm_task_reportEntity entity)
        {
            try
            {
                dm_task_reportEntity old_task_reportEntity = BaseRepository("dm_data").FindEntity<dm_task_reportEntity>(t => t.user_id == entity.user_id && t.task_id == entity.task_id);
                if (!old_task_reportEntity.IsEmpty())
                    throw new Exception("该任务已经举报过了，请勿重复提交!");
                entity.Create();
                this.BaseRepository("dm_data").Insert(entity);
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
