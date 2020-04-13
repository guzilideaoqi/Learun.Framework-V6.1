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
    /// 日 期：2020-04-10 13:55
    /// 描 述：进度任务设置
    /// </summary>
    public class DM_Task_Person_SettingService : RepositoryFactory
    {
        private ICache redisCache = CacheFactory.CaChe();
        private DM_IntergralDetailService dm_IntergralDetailService = new DM_IntergralDetailService();
        private DM_UserRelationService dm_UserRelationService = new DM_UserRelationService();
        private DM_OrderService dm_OrderService = new DM_OrderService();
        #region 构造函数和属性

        private string fieldSql;
        public DM_Task_Person_SettingService()
        {
            fieldSql = @"
                t.id,
                t.title,
                t.remark,
                t.s_type,
                t.needcount,
                t.ispartners,
                t.isenabled,
                t.createtime,
                t.updatetime,
                t.appid,
                t.rewardtype,
                t.rewardcount
            ";
        }
        #endregion

        #region 获取数据

        /// <summary>
        /// 获取列表数据
        /// <summary>
        /// <returns></returns>
        public IEnumerable<dm_task_person_settingEntity> GetList(string queryJson)
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
                strSql.Append(" FROM dm_task_person_setting t where 1=1");
                if (!queryParam["appid"].IsEmpty())
                {
                    strSql.Append(" and appid='" + queryParam["appid"].ToString() + "'");
                }
                return this.BaseRepository("dm_data").FindList<dm_task_person_settingEntity>(strSql.ToString());
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
        /// 缓存中获取任务
        /// </summary>
        /// <param name="appid">平台id</param>
        /// <param name="ispartners">是否为合伙人任务</param>
        /// <returns></returns>
        public IEnumerable<dm_task_person_settingEntity> GetListByCache(string appid, int ispartners)
        {
            string cacheKey = "PersonSetting" + appid;
            IEnumerable<dm_task_person_settingEntity> dm_Task_Person_SettingEntities = redisCache.Read<IEnumerable<dm_task_person_settingEntity>>(cacheKey, 7);
            if (dm_Task_Person_SettingEntities == null)
            {
                dm_Task_Person_SettingEntities = this.BaseRepository("dm_data").FindList<dm_task_person_settingEntity>("select * from dm_task_person_setting t where t.isenabled=1");
                if (dm_Task_Person_SettingEntities.Count() > 0)
                    redisCache.Write(cacheKey, dm_Task_Person_SettingEntities, 7);
            }

            return dm_Task_Person_SettingEntities.Where(t => t.ispartners == ispartners);
        }

        /// <summary>
        /// 获取列表分页数据
        /// <param name="pagination">分页参数</param>
        /// <summary>
        /// <returns></returns>
        public IEnumerable<dm_task_person_settingEntity> GetPageList(Pagination pagination, string queryJson)
        {
            try
            {
                var strSql = new StringBuilder();
                strSql.Append("SELECT ");
                strSql.Append(fieldSql);
                strSql.Append(" FROM dm_task_person_setting t ");
                return this.BaseRepository("dm_data").FindList<dm_task_person_settingEntity>(strSql.ToString(), pagination);
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
        public dm_task_person_settingEntity GetEntity(int keyValue)
        {
            try
            {
                return this.BaseRepository("dm_data").FindEntity<dm_task_person_settingEntity>(keyValue);
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
                this.BaseRepository("dm_data").Delete<dm_task_person_settingEntity>(t => t.id == keyValue);
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
        public void SaveEntity(int keyValue, dm_task_person_settingEntity entity)
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


        #region 获取个人任务进度
        public IEnumerable<dm_task_person_settingEntity> GetPersonProcess(int user_id, string appid)
        {
            try
            {
                IEnumerable<dm_task_person_settingEntity> person_SettingEntities = GetListByCache(appid, 0);
                foreach (var item in person_SettingEntities)
                {
                    //每日签到任务
                    if (item.s_type == 1)
                    {
                        //获取今日是否签到
                        dm_intergraldetailEntity dm_IntergraldetailEntity = dm_IntergralDetailService.GetLastSignData(user_id);
                        item.finishcount = dm_IntergraldetailEntity.createtime.ToString("yyyy-MM-dd") == DateTime.Now.ToString("yyyy-MM-dd") ? 1 : 0;
                    }
                    else if (item.s_type == 2)
                    {//邀请粉丝任务
                        item.finishcount = dm_UserRelationService.GetMyChildCount(user_id);
                    }
                    else if (item.s_type == 4)
                    {//购物任务
                        item.finishcount = dm_OrderService.GetMyOrderCount(user_id);
                    }
                }
                return person_SettingEntities;
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

        #region 领取任务

        #endregion

        #region 获取升级合伙人任务

        #endregion

        #region 申请成为合伙人

        #endregion

    }
}
