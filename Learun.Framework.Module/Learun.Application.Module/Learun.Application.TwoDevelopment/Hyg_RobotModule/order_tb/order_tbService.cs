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
    /// 日 期：2019-11-22 21:57
    /// 描 述：淘宝订单列表
    /// </summary>
    public class order_tbService : RepositoryFactory
    {
        #region 构造函数和属性

        private string fieldSql;
        public order_tbService()
        {
            fieldSql=@"
                t.tb_paid_time,
                t.tk_paid_time,
                t.pay_price,
                t.pub_share_fee,
                t.trade_id,
                t.tk_order_role,
                t.tk_earning_time,
                t.adzone_id,
                t.pub_share_rate,
                t.refund_tag,
                t.subsidy_rate,
                t.tk_total_rate,
                t.item_category_name,
                t.seller_nick,
                t.pub_id,
                t.alimama_rate,
                t.subsidy_type,
                t.item_img,
                t.pub_share_pre_fee,
                t.alipay_total_price,
                t.item_title,
                t.site_name,
                t.item_num,
                t.subsidy_fee,
                t.alimama_share_fee,
                t.trade_parent_id,
                t.order_type,
                t.tk_create_time,
                t.flow_source,
                t.terminal_type,
                t.click_time,
                t.tk_status,
                t.item_price,
                t.item_id,
                t.adzone_name,
                t.total_commission_rate,
                t.item_link,
                t.site_id,
                t.seller_shop_title,
                t.income_rate,
                t.total_commission_fee,
                t.tk_commission_pre_fee_for_media_platform,
                t.tk_commission_fee_for_media_platform,
                t.tk_commission_rate_for_media_platform,
                t.special_id,
                t.relation_id,
                t.tk_deposit_time,
                t.tb_deposit_time,
                t.deposit_price,
                t.app_key
            ";
        }
        #endregion

        #region 获取数据

        /// <summary>
        /// 获取列表数据
        /// <summary>
        /// <returns></returns>
        public IEnumerable<order_tbEntity> GetList( string queryJson )
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
                strSql.Append(" FROM order_tb t ");
                return this.BaseRepository("robot_DB").FindList<order_tbEntity>(strSql.ToString());
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
        public IEnumerable<order_tbEntity> GetPageList(Pagination pagination, string queryJson)
        {
            try
            {
                var strSql = new StringBuilder();
                strSql.Append("SELECT ");
                strSql.Append(fieldSql);
                strSql.Append(" FROM order_tb t ");
                return this.BaseRepository("robot_DB").FindList<order_tbEntity>(strSql.ToString(), pagination);
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
        public order_tbEntity GetEntity(string keyValue)
        {
            try
            {
                return this.BaseRepository("robot_DB").FindEntity<order_tbEntity>(keyValue);
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
        /// <param name="keyValue">订单编号</param>
        /// <summary>
        /// <returns></returns>
        public void DeleteEntity(string keyValue)
        {
            try
            {
                this.BaseRepository("robot_DB").Delete<order_tbEntity>(t=>t.trade_id== keyValue);
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
        public void SaveEntity(string keyValue, order_tbEntity entity)
        {
            try
            {
                if (!string.IsNullOrEmpty(keyValue))
                {
                    //entity.Modify(keyValue);
                    this.BaseRepository("robot_DB").Update(entity);
                }
                else
                {
                    //entity.Create();
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
