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
    /// 日 期：2021-01-11 11:36
    /// 描 述：明细说明
    /// </summary>
    public class dm_basesetting_tipService : RepositoryFactory
    {
        #region 构造函数和属性

        private string fieldSql;
        public dm_basesetting_tipService()
        {
            fieldSql = @"
                t.id,
                t.appid,
                t.task_do_tip,
                t.task_one_tip,
                t.task_two_tip,
                t.task_parners_one_tip,
                t.task_parners_two_tip,
                t.shop_pay_tip,
                t.shop_one_tip,
                t.shop_two_tip,
                t.shop_parners_one_tip,
                t.shop_parners_two_tip,
                t.opengent_one_tip,
                t.opengent_two_tip,
                t.opengent_three_tip,
                t.opengent_parners_one_tip,
                t.opengent_parners_two_tip,
                t.upgradegent_one_tip,
                t.upgradegent_two_tip,
                t.upgradegent_three_tip,
                t.upgradegent_parners_one_tip,
                t.upgradegent_parners_two_tip,
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
        public IEnumerable<dm_basesetting_tipEntity> GetList(string queryJson)
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
                strSql.Append(" FROM dm_basesetting_tip t ");
                return this.BaseRepository("dm_data").FindList<dm_basesetting_tipEntity>(strSql.ToString());
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
        public IEnumerable<dm_basesetting_tipEntity> GetPageList(Pagination pagination, string queryJson)
        {
            try
            {
                var strSql = new StringBuilder();
                strSql.Append("SELECT ");
                strSql.Append(fieldSql);
                strSql.Append(" FROM dm_basesetting_tip t ");
                return this.BaseRepository("dm_data").FindList<dm_basesetting_tipEntity>(strSql.ToString(), pagination);
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
        public dm_basesetting_tipEntity GetEntity(int keyValue)
        {
            try
            {
                return this.BaseRepository("dm_data").FindEntity<dm_basesetting_tipEntity>(keyValue);
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
                this.BaseRepository("dm_data").Delete<dm_basesetting_tipEntity>(t => t.id == keyValue);
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
        public void SaveEntity(int keyValue, dm_basesetting_tipEntity entity)
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

        #region 保存站长明细信息
        public void SaveEntityByAppID(string appid, dm_basesetting_tipEntity entity)
        {
            try
            {
                dm_basesetting_tipEntity dm_Basesetting_TipEntity = this.BaseRepository("dm_data").FindEntity<dm_basesetting_tipEntity>(t => t.appid == appid);
                if (dm_Basesetting_TipEntity.IsEmpty())
                {
                    entity.Create();
                    entity.appid = appid;
                    this.BaseRepository("dm_data").Insert(entity);
                }
                else
                {
                    entity.Modify(dm_Basesetting_TipEntity.id);
                    this.BaseRepository("dm_data").Update(entity);
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

        #region
        public dm_basesetting_tipEntity GetEntityByAppID(string appid)
        {
            try
            {
                dm_basesetting_tipEntity dm_Basesetting_TipEntity = this.BaseRepository("dm_data").FindEntity<dm_basesetting_tipEntity>(t => t.appid == appid);

                return dm_Basesetting_TipEntity;
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
