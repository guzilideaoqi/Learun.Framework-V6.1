using Dapper;
using Learun.DataBase.Repository;
using Learun.Util;
using System;
using System.Collections.Generic;
using System.Data;
using System.Text;
using System.Linq;
using Learun.Cache.Base;
using Learun.Cache.Factory;

namespace Learun.Application.TwoDevelopment.DM_APPManage
{
    /// <summary>
    /// 版 本 Learun-ADMS V6.1.6.0 力软敏捷开发框架
    /// Copyright (c) 2013-2017 上海力软信息技术有限公司
    /// 创 建：超级管理员
    /// 日 期：2020-04-16 16:01
    /// 描 述：任务中心
    /// </summary>
    public class DM_TaskService : RepositoryFactory
    {
        private DM_BaseSettingService dm_BaseSettingService = new DM_BaseSettingService();
        private DM_UserService dm_UserService = new DM_UserService();
        private DM_UserRelationService dm_UserRelationService = new DM_UserRelationService();

        #region 构造函数和属性

        private string fieldSql;
        public DM_TaskService()
        {
            fieldSql = @"
                t.id,
                t.task_no,
                t.task_title,
                t.task_type,
                t.task_status,
                t.task_description,
                t.task_operate,
                t.plaform,
                t.sort,
                t.createtime,
                t.totalcommission,
                t.servicefee,
                t.juniorcommission,
                t.seniorcommission,
                t.singlecommission,
                t.needcount,
                t.finishcount,
                t.user_id,
                t.appid
            ";
        }
        #endregion

        #region 获取数据
        /// <summary>
        /// 获取列表数据
        /// <summary>
        /// <returns></returns>
        public IEnumerable<dm_taskEntity> GetList(string queryJson)
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
                strSql.Append(" FROM dm_task t ");
                if (!queryParam["appid"].IsEmpty())
                {
                    strSql.Append(" where t.appid='" + queryParam["appid"].ToString() + "'");
                }
                return this.BaseRepository("dm_data").FindList<dm_taskEntity>(strSql.ToString());
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
        public IEnumerable<dm_taskEntity> GetPageList(Pagination pagination, string queryJson)
        {
            try
            {
                var queryParam = queryJson.ToJObject();

                var strSql = new StringBuilder();
                strSql.Append("SELECT ");
                strSql.Append(fieldSql);
                strSql.Append(" FROM dm_task t ");
                if (!queryParam["appid"].IsEmpty())
                {
                    strSql.Append(" where t.appid='" + queryParam["appid"].ToString() + "'");
                }
                if (!queryParam["user_id"].IsEmpty())
                {
                    strSql.Append(" where t.user_id='" + queryParam["user_id"].ToString() + "'");
                }
                return this.BaseRepository("dm_data").FindList<dm_taskEntity>(strSql.ToString(), pagination);
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
        public dm_taskEntity GetEntity(int? keyValue)
        {
            try
            {
                return this.BaseRepository("dm_data").FindEntity<dm_taskEntity>(keyValue);
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
                this.BaseRepository("dm_data").Delete<dm_taskEntity>(t => t.id == keyValue);
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
        public void SaveEntity(int keyValue, dm_taskEntity entity)
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

        #region 发布任务
        /// <summary>
        /// web端发布任务不用扣除余额，直接发就好
        /// </summary>
        /// <param name="entity"></param>
        public void ReleaseTaskByWeb(dm_taskEntity entity)
        {
            try
            {
                if (entity.singlecommission <= 0)
                    throw new Exception("任务佣金不能小于0!");
                if (entity.needcount <= 0)
                    throw new Exception("任务参与数不能小于0!");
                UserInfo userInfo = LoginUserInfo.Get();

                entity.totalcommission = entity.singlecommission * entity.needcount;//需要从用户账户扣除的金额
                dm_basesettingEntity dm_BaseSettingEntity = dm_BaseSettingService.GetEntityByCache(userInfo.companyId);
                entity.seniorcommission = Math.Round((entity.singlecommission * dm_BaseSettingEntity.task_do_senior) / 100, 2);//高级代理佣金
                entity.juniorcommission = Math.Round((entity.singlecommission * dm_BaseSettingEntity.task_do_junior) / 100, 2);//初级代理佣金
                entity.servicefee = Math.Round((entity.singlecommission * dm_BaseSettingEntity.task_servicefee) / 100, 2);//任务服务费(奖励在服务费中分发)
                entity.task_no = DateTime.Now.ToString("yyyyMMddHHmmssfff") + entity.user_id.ToString().PadLeft(6, '0');
                entity.sort = GetSort(null, entity.totalcommission);
                entity.appid = userInfo.companyId;
                entity.Create();
                entity.plaform = 0;

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
        /// <summary>
        /// 发布任务
        /// </summary>
        /// <param name="entity"></param>
        public void ReleaseTask(dm_taskEntity entity)
        {
            IRepository db = null;
            try
            {
                if (entity.singlecommission <= 0)
                    throw new Exception("任务佣金不能小于0!");
                if (entity.needcount <= 0)
                    throw new Exception("任务参与数不能小于0!");
                dm_userEntity dm_UserEntity = dm_UserService.GetEntity(entity.user_id);
                entity.totalcommission = entity.singlecommission * entity.needcount;//需要从用户账户扣除的金额
                if (dm_UserEntity.accountprice < entity.totalcommission)
                    throw new Exception("账户余额不足，请充值后重新发布!");

                dm_user_relationEntity dm_User_RelationEntity = dm_UserRelationService.GetEntityByUserID(entity.user_id);
                if (dm_User_RelationEntity.IsEmpty())
                    throw new Exception("用户数据异常!");

                dm_basesettingEntity dm_BaseSettingEntity = dm_BaseSettingService.GetEntityByCache(entity.appid);
                entity.seniorcommission = Math.Round((entity.singlecommission * dm_BaseSettingEntity.task_do_senior) / 100, 2);//高级代理佣金
                entity.juniorcommission = Math.Round((entity.singlecommission * dm_BaseSettingEntity.task_do_junior) / 100, 2);//初级代理佣金
                entity.servicefee = Math.Round((entity.singlecommission * dm_BaseSettingEntity.task_servicefee) / 100, 2);//任务服务费(奖励在服务费中分发)
                entity.task_no = DateTime.Now.ToString("yyyyMMddHHmmssfff") + entity.user_id.ToString().PadLeft(6, '0');
                entity.sort = GetSort(dm_User_RelationEntity, entity.totalcommission);

                entity.Create();
                dm_UserEntity.accountprice -= entity.totalcommission;
                dm_UserEntity.Modify(dm_UserEntity.id);
                dm_User_RelationEntity.taskcount += 1;
                dm_User_RelationEntity.Modify(dm_User_RelationEntity.id);


                dm_accountdetailEntity dm_AccountdetailEntity = new dm_accountdetailEntity
                {
                    createtime = DateTime.Now,
                    remark = "发布任务扣除" + entity.task_no,
                    stepvalue = entity.totalcommission,
                    currentvalue = dm_UserEntity.accountprice,
                    title = "发布任务",
                    type = 12,
                    user_id = entity.user_id
                };

                db = BaseRepository("dm_data").BeginTrans();
                db.Insert(entity);
                db.Update(dm_UserEntity);
                db.Insert(dm_AccountdetailEntity);
                db.Update(dm_User_RelationEntity);
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

        public decimal GetSort(dm_user_relationEntity dm_User_RelationEntity, decimal totalCommission)
        {
            int days = Time.DiffDays(DateTime.Parse("2020-01-01 00:00:00"), DateTime.Now);

            if (dm_User_RelationEntity == null)
                return Math.Round(days * 0.4M + totalCommission * 0.4M, 2);
            else
                return Math.Round(days * 0.4M + totalCommission * 0.4M + dm_User_RelationEntity.taskcount * 0.2M - dm_User_RelationEntity.taskreportcount, 2);
        }
        #endregion

        #region 取消任务
        public void CancelByReleasePerson(int task_id)
        {
            IRepository db = null;
            try
            {
                DM_Task_ReviceService dm_Task_ReviceService = new DM_Task_ReviceService();

                IEnumerable<dm_task_reviceEntity> dm_Task_ReviceEntities = dm_Task_ReviceService.GetReviceListByTaskID(task_id);
                if (dm_Task_ReviceEntities.Where(t => t.status == 2).Count() > 0)
                    throw new Exception("当前存在未审核的资料,任务无法取消,请审核之后重试!");
                dm_taskEntity dm_TaskEntity = GetEntity(task_id);
                if (dm_TaskEntity.task_status == 2)
                    throw new Exception("该任务已取消!");
                if (dm_TaskEntity.task_status == 1 || dm_TaskEntity.finishcount == dm_TaskEntity.needcount)
                    throw new Exception("该任务已完成!");
                if (dm_TaskEntity.finishcount > dm_TaskEntity.needcount)
                    throw new Exception("错误码:1001--任务存在异常记录,请联系管理员!");

                //取消任务
                dm_TaskEntity.task_status = 2;
                dm_userEntity dm_UserEntity = dm_UserService.GetEntity(dm_TaskEntity.user_id);

                #region 存在两种退款  1、全部退  2部分退
                decimal caclePrice = 0;
                if (dm_TaskEntity.finishcount == 0)
                {//全部退
                    caclePrice = dm_TaskEntity.totalcommission;
                }
                else
                {//部分退
                    caclePrice = Math.Round((dm_TaskEntity.needcount - dm_TaskEntity.finishcount) * dm_TaskEntity.singlecommission, 2);
                }
                dm_UserEntity.accountprice += caclePrice;
                #endregion

                dm_accountdetailEntity dm_AccountdetailEntity = new dm_accountdetailEntity
                {
                    createtime = DateTime.Now,
                    remark = "取消发布任务" + dm_TaskEntity.task_no,
                    stepvalue = caclePrice,
                    currentvalue = dm_UserEntity.accountprice,
                    title = "取消发布任务",
                    type = 13,
                    user_id = dm_TaskEntity.user_id
                };

                db = BaseRepository("dm_data").BeginTrans();
                dm_UserEntity.Modify(dm_UserEntity.id);
                db.Update(dm_UserEntity);
                dm_TaskEntity.Modify(dm_TaskEntity.id);
                db.Update(dm_TaskEntity);
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
    }
}
