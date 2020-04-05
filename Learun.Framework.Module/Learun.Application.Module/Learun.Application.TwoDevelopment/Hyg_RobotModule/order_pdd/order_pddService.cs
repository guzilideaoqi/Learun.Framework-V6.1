using Dapper;
using Learun.DataBase.Repository;
using Learun.Util;
using System;
using System.Collections.Generic;
using System.Data;
using System.Text;

namespace Learun.Application.TwoDevelopment.Hyg_RobotModule
{
    /// <summary>
    /// 版 本 Learun-ADMS V6.1.6.0 力软敏捷开发框架
    /// Copyright (c) 2013-2017 上海力软信息技术有限公司
    /// 创 建：超级管理员
    /// 日 期：2019-11-22 22:10
    /// 描 述：拼多多订单列表
    /// </summary>
    public class order_pddService : RepositoryFactory
    {
        #region 构造函数和属性

        private string fieldSql;
        public order_pddService()
        {
            fieldSql=@"
                t.order_sn,
                t.goods_id,
                t.goods_name,
                t.goods_thumbnail_url,
                t.goods_quantity,
                t.goods_price,
                t.order_amount,
                t.promotion_rate,
                t.promotion_amount,
                t.batch_no,
                t.order_status,
                t.order_status_desc,
                t.order_create_time,
                t.order_pay_time,
                t.order_group_success_time,
                t.order_receive_time,
                t.order_verify_time,
                t.order_settle_time,
                t.order_modify_at,
                t.type,
                t.group_id,
                t.auth_duo_id,
                t.zs_duo_id,
                t.custom_parameters,
                t.cps_sign,
                t.url_last_generate_time,
                t.point_time,
                t.return_status,
                t.p_id,
                t.cpa_new
            ";
        }
        #endregion

        #region 获取数据

        /// <summary>
        /// 获取列表数据
        /// <summary>
        /// <returns></returns>
        public IEnumerable<order_pddEntity> GetList( string queryJson )
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
                strSql.Append(" FROM order_pdd t ");
                return this.BaseRepository("robot_DB").FindList<order_pddEntity>(strSql.ToString());
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
        public IEnumerable<order_pddEntity> GetPageList(Pagination pagination, string queryJson)
        {
            try
            {
                var strSql = new StringBuilder();
                strSql.Append("SELECT ");
                strSql.Append(fieldSql);
                strSql.Append(" FROM order_pdd t ");
                return this.BaseRepository("robot_DB").FindList<order_pddEntity>(strSql.ToString(), pagination);
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
        public order_pddEntity GetEntity(string keyValue)
        {
            try
            {
                return this.BaseRepository("robot_DB").FindEntity<order_pddEntity>(keyValue);
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
                this.BaseRepository("robot_DB").Delete<order_pddEntity>(t=>t.order_sn == keyValue);
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
        public void SaveEntity(string keyValue, order_pddEntity entity)
        {
            try
            {
                if (!string.IsNullOrEmpty(keyValue))
                {
                    entity.Modify(keyValue);
                    this.BaseRepository("robot_DB").Update(entity);
                }
                else
                {
                    entity.Create();
                    this.BaseRepository("robot_DB").Insert(entity);
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
