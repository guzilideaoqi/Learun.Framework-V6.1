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
        private DM_Task_Person_RecordService dm_Task_Person_RecordService = new DM_Task_Person_RecordService();
        private DM_UserService dm_UserService = new DM_UserService();
        private DM_APP_Partners_RecordService dm_APP_Partners_RecordService = new DM_APP_Partners_RecordService();

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
                t.rewardcount,
                t.finishcount,
                t.finishstatus,
                t.typeimage,
                t.btntext
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
        public dm_task_person_settingEntity GetEntity(int? keyValue)
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

                #region 清除缓存
                UserInfo userInfo = LoginUserInfo.Get();
                string cacheKey = "PersonSetting" + userInfo.companyId;

                redisCache.Remove(cacheKey, 7);
                #endregion
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
                UserInfo userInfo = LoginUserInfo.Get();

                if (keyValue > 0)
                {
                    entity.Modify(keyValue);
                    this.BaseRepository("dm_data").Update(entity);
                }
                else
                {
                    entity.appid = userInfo.companyId;
                    entity.Create();
                    this.BaseRepository("dm_data").Insert(entity);
                }

                #region 清除缓存
                string cacheKey = "PersonSetting" + userInfo.companyId;

                redisCache.Remove(cacheKey, 7);
                #endregion
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
                        if (dm_IntergraldetailEntity.IsEmpty())
                        {
                            item.finishcount = 0;
                            item.finishstatus = 0;
                            item.btntext = "签到";
                        }
                        else {
                            item.finishcount = dm_IntergraldetailEntity.createtime.ToString("yyyy-MM-dd") == DateTime.Now.ToString("yyyy-MM-dd") ? 1 : 0;
                            item.finishstatus = item.finishcount == 1 ? 2 : 0;
                            item.btntext = item.finishstatus == 0 ? "签到" : "已完成";
                        }
                    }
                    else if (item.s_type == 2)
                    {//邀请粉丝任务
                        dm_userEntity dm_UserEntity = dm_UserService.GetEntityByCache(user_id);
                        item.finishcount = (int)dm_UserEntity.mychildcount;
                        item.finishstatus = GetFinishStatus(item.finishcount, item.needcount, user_id, item.id);
                        item.remark += string.Format("({0}/{1})", item.finishcount, item.needcount);

                        item.btntext = getBtnText(item.finishstatus, item.s_type);
                    }
                    else if (item.s_type == 4)
                    {//购物任务
                        dm_user_relationEntity dm_User_RelationEntity = dm_UserRelationService.GetEntityByUserID(user_id);
                        item.finishcount = dm_User_RelationEntity.ordercount;
                        item.finishstatus = GetFinishStatus(item.finishcount, item.needcount, user_id, item.id);

                        item.remark += string.Format("({0}/{1})", item.finishcount, item.needcount);

                        item.btntext = getBtnText(item.finishstatus, item.s_type);
                    }
                    else if (item.s_type == 6)
                    {
                        item.finishstatus = GetFinishStatus(item.finishcount, item.needcount, user_id, item.id);
                        item.finishcount = 0;//浏览任务此字段无意义

                        item.btntext = item.finishstatus == 2 ? "已完成" : "去浏览";
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

        int GetFinishStatus(int currentCount, int? needCount, int user_id, int? task_id)
        {
            if (currentCount < needCount)
                return 0;
            else
            {
                dm_task_person_recordEntity dm_Task_Person_RecordEntity = dm_Task_Person_RecordService.GetMyPersonRecord(user_id, task_id);
                if (!dm_Task_Person_RecordEntity.IsEmpty())
                {
                    return dm_Task_Person_RecordEntity.status == 1 ? 2 : 1;
                }
                else
                {
                    return 1;
                }
            }
        }

        string getBtnText(int finishstatus, int? s_type)
        {
            string btnText = "未完成";
            switch (finishstatus)
            {
                case 0:
                    btnText = s_type == 2 ? "去邀请" : "未完成";
                    break;
                case 1:
                    btnText = "领取";
                    break;
                case 2:
                    btnText = "已完成";
                    break;
            }
            return btnText;
        }
        #endregion

        #region 领取任务
        public void ReceiveAwards(int user_id, int? task_id)
        {
            IRepository db = null;
            try
            {
                #region 获取任务记录
                dm_task_person_settingEntity dm_Task_Person_SettingEntity = GetEntity(task_id);
                #endregion
                if (dm_Task_Person_SettingEntity.s_type == 1)
                {
                    dm_UserService.SignIn(user_id);
                }
                else
                {
                    dm_userEntity dm_UserEntity = dm_UserService.GetEntityByCache(user_id);

                    if (dm_Task_Person_SettingEntity.s_type == 4)
                    {
                        dm_user_relationEntity dm_User_RelationEntity = dm_UserRelationService.GetEntityByUserID(user_id);
                        if (dm_User_RelationEntity.ordercount < dm_Task_Person_SettingEntity.needcount)
                            throw new Exception("请完成购物任务后再来领取!");
                    }
                    else if (dm_Task_Person_SettingEntity.s_type == 2) {
                        if (dm_UserEntity.mychildcount < dm_Task_Person_SettingEntity.needcount)
                            throw new Exception("请完成邀请任务后再来领取!");
                    }

                    dm_task_person_recordEntity dm_Task_Person_RecordEntity = dm_Task_Person_RecordService.GetMyPersonRecord(user_id, task_id);
                    if (dm_Task_Person_RecordEntity.IsEmpty())
                    {
                        #region 增加任务领取记录
                        dm_Task_Person_RecordEntity = new dm_task_person_recordEntity();
                        dm_Task_Person_RecordEntity.user_id = user_id;
                        dm_Task_Person_RecordEntity.task_id = task_id;
                        dm_Task_Person_RecordEntity.createtime = DateTime.Now;
                        dm_Task_Person_RecordEntity.status = 1;
                        #endregion


                        db = BaseRepository("dm_data").BeginTrans();
                        //积分任务
                        if (dm_Task_Person_SettingEntity.rewardtype == 0)
                        {
                            #region 增加积分变更明细
                            int stepvalue = int.Parse(dm_Task_Person_SettingEntity.rewardcount.ToString());//积分无小数
                            dm_intergraldetailEntity dm_IntergraldetailEntity = new dm_intergraldetailEntity();
                            dm_IntergraldetailEntity.type = 4;
                            dm_IntergraldetailEntity.profitLoss = 1;
                            dm_IntergraldetailEntity.user_id = user_id;
                            dm_IntergraldetailEntity.createtime = DateTime.Now;
                            dm_IntergraldetailEntity.currentvalue = dm_UserEntity.integral + stepvalue;
                            dm_IntergraldetailEntity.stepvalue = stepvalue;
                            dm_IntergraldetailEntity.title = dm_Task_Person_SettingEntity.title;
                            dm_IntergraldetailEntity.remark = dm_Task_Person_SettingEntity.remark;
                            dm_IntergraldetailEntity.Create();

                            dm_UserEntity.integral = dm_IntergraldetailEntity.currentvalue;
                            #endregion
                            db.Insert(dm_UserEntity);
                        }
                        else
                        { //余额任务
                            #region 增加余额变更明细
                            dm_accountdetailEntity dm_AccountdetailEntity = new dm_accountdetailEntity();
                            dm_AccountdetailEntity.user_id = user_id;
                            dm_AccountdetailEntity.type = 10;
                            dm_AccountdetailEntity.profitLoss = CommonHelper.GetProfitLoss(10);
                            dm_AccountdetailEntity.createtime = DateTime.Now;
                            dm_AccountdetailEntity.currentvalue = dm_UserEntity.accountprice + dm_Task_Person_SettingEntity.rewardcount;
                            dm_AccountdetailEntity.stepvalue = dm_Task_Person_SettingEntity.rewardcount;
                            dm_AccountdetailEntity.title = dm_Task_Person_SettingEntity.title;
                            dm_AccountdetailEntity.remark = dm_Task_Person_SettingEntity.remark;
                            dm_AccountdetailEntity.Create();
                            #endregion

                            dm_UserEntity.accountprice = dm_AccountdetailEntity.currentvalue;
                            db.Insert(dm_AccountdetailEntity);
                        }

                        dm_UserEntity.Modify(user_id);
                        db.Update(dm_UserEntity);
                        db.Insert(dm_Task_Person_RecordEntity);

                        db.Commit();
                    }
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

        #region 获取升级合伙人任务
        /// <summary>
        /// 获取合伙人任务
        /// </summary>
        /// <param name="user_id"></param>
        /// <param name="appid"></param>
        /// <returns></returns>
        public IEnumerable<dm_task_person_settingEntity> GetPartnersProcess(int user_id, string appid)
        {
            try
            {
                IEnumerable<dm_task_person_settingEntity> person_SettingEntities = GetListByCache(appid, 1);

                foreach (var item in person_SettingEntities)
                {
                    if (item.s_type == 3)
                    {//邀请粉丝任务
                        item.finishcount = dm_UserRelationService.GetMyChildCount(user_id);
                        item.remark += string.Format("({0}/{1})", item.finishcount, item.needcount);
                    }
                    else if (item.s_type == 5)
                    {//购物任务
                        item.finishcount = dm_OrderService.GetMyOrderCount(user_id);
                        item.remark += string.Format("({0}/{1})", item.finishcount, item.needcount);
                    }
                    item.finishstatus = item.finishcount >= item.needcount ? 1 : 0;
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

        #region 申请成为合伙人
        /// <summary>
        /// 申请成为合伙人
        /// </summary>
        /// <param name="user_id"></param>
        public void ApplyPartners(int user_id, string appid)
        {
            IEnumerable<dm_task_person_settingEntity> person_SettingEntities = GetListByCache(appid, 1);

            foreach (var item in person_SettingEntities)
            {
                if (item.s_type == 3)
                {//邀请粉丝任务
                    item.finishcount = dm_UserRelationService.GetMyChildCount(user_id);
                    if (item.finishcount < item.needcount)
                        throw new Exception("当前条件不满足!");
                }
                else if (item.s_type == 5)
                {//购物任务
                    item.finishcount = dm_OrderService.GetMyOrderCount(user_id);
                    if (item.finishcount < item.needcount)
                        throw new Exception("当前条件不满足!");
                }
            }

            dm_apply_partners_recordEntity dm_Apply_Partners_RecordEntity = dm_APP_Partners_RecordService.GetEntityByUserID(user_id);
            if (dm_Apply_Partners_RecordEntity.IsEmpty())
            {
                dm_Apply_Partners_RecordEntity = new dm_apply_partners_recordEntity();
                dm_Apply_Partners_RecordEntity.user_id = user_id;
                dm_Apply_Partners_RecordEntity.appid = appid;
                dm_Apply_Partners_RecordEntity.createtime = DateTime.Now;

                dm_APP_Partners_RecordService.SaveEntity(0, dm_Apply_Partners_RecordEntity);
            }
            else
            {
                throw new Exception("当前申请记录正在审核中,请勿重复提交!");
            }
        }
        #endregion
    }
}
