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
    /// 日 期：2020-04-14 17:35
    /// 描 述：提现申请记录
    /// </summary>
    public class DM_Apply_CashRecordService : RepositoryFactory
    {
        private DM_UserIBLL dm_UserIBLL = new DM_UserBLL();
        #region 构造函数和属性

        private string fieldSql;
        public DM_Apply_CashRecordService()
        {
            fieldSql = @"
                t.id,
                t.user_id,
                t.price,
                t.status,
                t.paytype,
                t.remark,
                t.createtime,
                t.failreason,
                t.currentprice
            ";
        }
        #endregion

        #region 获取数据

        /// <summary>
        /// 获取列表数据
        /// <summary>
        /// <returns></returns>
        public IEnumerable<dm_apply_cashrecordEntity> GetList(string queryJson)
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
                strSql.Append(" FROM dm_apply_cashrecord t ");
                return this.BaseRepository("dm_data").FindList<dm_apply_cashrecordEntity>(strSql.ToString());
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
        public IEnumerable<dm_apply_cashrecordEntity> GetPageList(Pagination pagination, string queryJson)
        {
            try
            {
                var strSql = new StringBuilder();
                strSql.Append("SELECT ");
                strSql.Append(fieldSql);
                strSql.Append(" FROM dm_apply_cashrecord t ");
                return this.BaseRepository("dm_data").FindList<dm_apply_cashrecordEntity>(strSql.ToString(), pagination);
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
        /// 获取提现记录
        /// </summary>
        /// <param name="pagination"></param>
        /// <param name="queryJson"></param>
        /// <returns></returns>
        public DataTable GetPageListByDataTable(Pagination pagination, string queryJson)
        {
            try
            {
                var queryParam = queryJson.ToJObject();
                var strSql = new StringBuilder();
                strSql.Append("select c.*,u.nickname,u.realname,u.phone,u.zfb from dm_apply_cashrecord c left join dm_user u on c.user_id=u.id where 1=1");
                if (!queryParam["txt_user_id"].IsEmpty())
                {
                    strSql.Append(" and u.id='" + queryParam["txt_user_id"].ToString() + "'");
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
        public dm_apply_cashrecordEntity GetEntity(int keyValue)
        {
            try
            {
                return this.BaseRepository("dm_data").FindEntity<dm_apply_cashrecordEntity>(keyValue);
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
                this.BaseRepository("dm_data").Delete<dm_apply_cashrecordEntity>(t => t.id == keyValue);
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
        public void SaveEntity(int keyValue, dm_apply_cashrecordEntity entity)
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


        #region 申请提现
        public void ApplyAccountCash(int user_id, decimal price, string remark)
        {
            IRepository db = null;
            try
            {
                dm_userEntity dm_UserEntity = dm_UserIBLL.GetEntity(user_id);//不从缓存取，此处需要验证账户余额
                if (dm_UserEntity.IsEmpty())
                    throw new Exception("用户信息异常!");
                if (dm_UserEntity.isreal == 0)
                    throw new Exception("您的账号未实名，禁止提现!");
                if (dm_UserEntity.zfb.IsEmpty())
                    throw new Exception("支付宝账号未绑定,禁止提现!");
                if (price > dm_UserEntity.accountprice)
                {
                    throw new Exception("账户余额不足!");
                }
                else
                {
                    dm_UserEntity.accountprice -= price;
                    dm_UserEntity.Modify(user_id);

                    #region 增加提现记录
                    dm_apply_cashrecordEntity dm_Apply_CashrecordEntity = new dm_apply_cashrecordEntity();
                    dm_Apply_CashrecordEntity.user_id = user_id;
                    dm_Apply_CashrecordEntity.createtime = DateTime.Now;
                    dm_Apply_CashrecordEntity.paytype = 0;
                    dm_Apply_CashrecordEntity.price = price;
                    dm_Apply_CashrecordEntity.currentprice = dm_UserEntity.accountprice;
                    dm_Apply_CashrecordEntity.remark = remark;
                    dm_Apply_CashrecordEntity.status = 0;
                    #endregion

                    db = BaseRepository("dm_data").BeginTrans();
                    db.Update(dm_UserEntity);
                    db.Insert(dm_Apply_CashrecordEntity);

                    db.Commit();
                }
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

        #region 审核提现记录
        /// <summary>
        /// 审核提现记录
        /// </summary>
        /// <param name="id">提现记录id</param>
        public void CheckApplyCashRecord(int id, int paytype)
        {
            IRepository db = null;
            try
            {
                dm_apply_cashrecordEntity dm_Apply_CashrecordEntity = GetEntity(id);
                if (dm_Apply_CashrecordEntity.IsEmpty())
                    throw new Exception("提现记录不存在!");

                dm_Apply_CashrecordEntity.paytype = paytype;
                dm_Apply_CashrecordEntity.status = 1;
                dm_Apply_CashrecordEntity.checktime = DateTime.Now;

                dm_accountdetailEntity dm_AccountdetailEntity = new dm_accountdetailEntity();
                dm_AccountdetailEntity.currentvalue = dm_Apply_CashrecordEntity.currentprice;
                dm_AccountdetailEntity.stepvalue = dm_Apply_CashrecordEntity.price;
                dm_AccountdetailEntity.type = 11;
                dm_AccountdetailEntity.title = "提现";
                dm_AccountdetailEntity.remark = "账户余额提现";
                dm_AccountdetailEntity.user_id = dm_Apply_CashrecordEntity.user_id;
                dm_AccountdetailEntity.createtime = DateTime.Now;

                db = BaseRepository("dm_data").BeginTrans();
                db.Update(dm_Apply_CashrecordEntity);
                db.Insert(dm_AccountdetailEntity);
                db.Commit();
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

        #region 我的提现列表
        public IEnumerable<dm_apply_cashrecordEntity> GetMyCashRecord(int user_id, Pagination pagination)
        {
            try
            {
                var strSql = new StringBuilder();
                strSql.Append("SELECT ");
                strSql.Append(fieldSql);
                strSql.Append(" FROM dm_apply_cashrecord t ");

                if (user_id > 0)
                {
                    strSql.Append(" where t.user_id=");
                    strSql.Append(user_id.ToString());
                }

                return this.BaseRepository("dm_data").FindList<dm_apply_cashrecordEntity>(strSql.ToString(), pagination);
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
