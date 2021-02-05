using Dapper;
using Learun.DataBase.Repository;
using Learun.Util;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;

namespace Learun.Application.TwoDevelopment.DM_APPManage
{
    /// <summary>
    /// 版 本 Learun-ADMS V6.1.6.0 力软敏捷开发框架
    /// Copyright (c) 2013-2017 上海力软信息技术有限公司
    /// 创 建：超级管理员
    /// 日 期：2021-02-05 15:33
    /// 描 述：活动管理
    /// </summary>
    public class dm_activity_manageService : RepositoryFactory
    {
        #region 构造函数和属性

        private string fieldSql;
        public dm_activity_manageService()
        {
            fieldSql = @"
                t.f_id,
                t.APP_RedPaper_Image,
                t.APP_RedPaper_Text,
                t.APP_Rock_RedPaper_Image,
                t.APP_To_ActivityUrl,
                t.ActivityTitle,
                t.ActivityType,
                t.ActivityStartTime,
                t.ActivityEndTime,
                t.ActivityCode,
                t.ActivityStatus,
                t.ActivityRemark,
                t.InitRedPaper_MinPrice,
                t.InitRedPaper_MaxPrice,
                t.RewardPrice,
                t.CreateTime,
                t.UpdateTime
            ";
        }
        #endregion

        #region 获取数据

        /// <summary>
        /// 获取列表数据
        /// <summary>
        /// <returns></returns>
        public IEnumerable<dm_activity_manageEntity> GetList(string queryJson)
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
                strSql.Append(" FROM dm_activity_manage t ");
                return this.BaseRepository("dm_data").FindList<dm_activity_manageEntity>(strSql.ToString());
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
        public IEnumerable<dm_activity_manageEntity> GetPageList(Pagination pagination, string queryJson)
        {
            try
            {
                var strSql = new StringBuilder();
                strSql.Append("SELECT ");
                strSql.Append(fieldSql);
                strSql.Append(" FROM dm_activity_manage t ");
                return this.BaseRepository("dm_data").FindList<dm_activity_manageEntity>(strSql.ToString(), pagination);
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
        public dm_activity_manageEntity GetEntity(string keyValue)
        {
            try
            {
                return this.BaseRepository("dm_data").FindEntity<dm_activity_manageEntity>(keyValue);
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

        public dm_activity_manageEntity GetActivityInfo()
        {
            try
            {
                DateTime currentTime = DateTime.Now;
                IEnumerable<dm_activity_manageEntity> dm_Activity_ManageEntities = this.BaseRepository("dm_data").FindList<dm_activity_manageEntity>(t => t.ActivityStartTime < currentTime && currentTime < t.ActivityEndTime && t.ActivityStatus == 1);
                if (dm_Activity_ManageEntities.Count() == 0)
                    return null;
                else
                    return dm_Activity_ManageEntities.FirstOrDefault();
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
        public void DeleteEntity(string keyValue)
        {
            try
            {
                this.BaseRepository("dm_data").Delete<dm_activity_manageEntity>(t => t.f_id == keyValue);
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
        public void SaveEntity(string keyValue, dm_activity_manageEntity entity)
        {
            try
            {
                if (!string.IsNullOrEmpty(keyValue))
                {
                    entity.Modify(keyValue);
                    this.BaseRepository("dm_data").Update(entity);
                }
                else
                {
                    dm_activity_manageEntity dm_Activity_ManageEntity = GetActivityInfo();
                    if (dm_Activity_ManageEntity.IsEmpty())
                    {
                        entity.Create();
                        this.BaseRepository("dm_data").Insert(entity);
                    }
                    else
                    {
                        throw new Exception("当前存在正在进行中的任务,无法创建新的任务，请在进行中的任务结束后再来创建!");
                    }
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

    }
}
