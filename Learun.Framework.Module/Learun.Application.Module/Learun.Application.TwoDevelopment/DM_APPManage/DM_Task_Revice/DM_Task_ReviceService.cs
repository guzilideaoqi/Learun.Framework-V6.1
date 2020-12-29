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
    /// 日 期：2020-04-16 16:03
    /// 描 述：任务接受记录
    /// </summary>
    public class DM_Task_ReviceService : RepositoryFactory
    {
        private ICache redisCache = CacheFactory.CaChe();

        private DM_TaskService dm_TaskService = new DM_TaskService();
        private DM_UserService dM_UserService = new DM_UserService();
        private DM_UserRelationService dM_UserRelationService = new DM_UserRelationService();
        private DM_BaseSettingService dM_BaseSettingService = new DM_BaseSettingService();

        #region 构造函数和属性

        private string fieldSql;
        public DM_Task_ReviceService()
        {
            fieldSql = @"
                t.id,
                t.task_id,
                t.user_id,
                t.revice_time,
                t.status,
                t.check_time,
                t.submit_time,
                t.submit_data,
                t.createtime
            ";
        }
        #endregion

        #region 获取数据

        /// <summary>
        /// 获取列表数据
        /// <summary>
        /// <returns></returns>
        public IEnumerable<dm_task_reviceEntity> GetList(string queryJson)
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
                strSql.Append(" FROM dm_task_revice t ");
                return this.BaseRepository("dm_data").FindList<dm_task_reviceEntity>(strSql.ToString());
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
        public IEnumerable<dm_task_reviceEntity> GetPageList(Pagination pagination, string queryJson)
        {
            try
            {
                var strSql = new StringBuilder();
                strSql.Append("SELECT ");
                strSql.Append(fieldSql);
                strSql.Append(" FROM dm_task_revice t ");
                return this.BaseRepository("dm_data").FindList<dm_task_reviceEntity>(strSql.ToString(), pagination);
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
        /// 获取任务接受详情
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
                strSql.Append("select r.*,U.nickname,u.realname,u.phone,u.headpic from dm_task_revice r LEFT JOIN dm_user u on r.user_id=u.id where 1=1");
                if (!queryParam["User_ID"].IsEmpty())
                {
                    strSql.Append(" and u.id=" + queryParam["User_ID"].ToString());
                }

                if (!queryParam["Task_ID"].IsEmpty())
                {
                    strSql.Append(" and r.task_id=" + queryParam["Task_ID"].ToString());
                }

                if (!queryParam["Status"].IsEmpty())
                {
                    strSql.Append(" and r.status=" + queryParam["Status"].ToString());
                }

                return BaseRepository("dm_data").FindTable(strSql.ToString(), pagination);
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
        public dm_task_reviceEntity GetEntity(int? keyValue)
        {
            try
            {
                return this.BaseRepository("dm_data").FindEntity<dm_task_reviceEntity>(keyValue);
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
        /// 获取用户接受记录
        /// </summary>
        /// <param name="user_id"></param>
        /// <param name="task_id"></param>
        /// <returns></returns>
        public dm_task_reviceEntity GetReviceEntity(int? user_id, int? task_id, int revice_id = 0)
        {
            try
            {
                if (user_id <= 0)
                    return null;
                if (revice_id > 0)
                    return GetEntity(revice_id);
                else
                    return this.BaseRepository("dm_data").FindEntity<dm_task_reviceEntity>(t => t.user_id == user_id && t.task_id == task_id && t.status != 4);
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
        /// 获取我的接受任务
        /// </summary>
        /// <param name="user_id"></param>
        /// <returns></returns>
        public DataTable GetMyReviceTask(int user_id, int TaskStatus, Pagination pagination)
        {
            try
            {
                StringBuilder stringBuilder = new StringBuilder();
                stringBuilder.Append("select t.*,r.status ReviceStatus,r.id Revice_ID,r.revice_time from dm_task_revice r left join dm_task t on r.task_id=t.id where r.user_id=" + user_id);

                if (TaskStatus != -1)
                    stringBuilder.Append(" and r.status=" + TaskStatus);

                return this.BaseRepository("dm_data").FindTable(stringBuilder.ToString(), pagination);
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
                this.BaseRepository("dm_data").Delete<dm_task_reviceEntity>(t => t.id == keyValue);
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
        public void SaveEntity(int keyValue, dm_task_reviceEntity entity)
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

        #region 获取任务接受列表
        public IEnumerable<dm_task_reviceEntity> GetReviceListByTaskID(int task_id)
        {
            try
            {
                string cacheKey = "ReviceTaskList" + task_id;
                IEnumerable<dm_task_reviceEntity> dm_Task_ReviceEntities = redisCache.Read<IEnumerable<dm_task_reviceEntity>>(cacheKey, 7);
                if (dm_Task_ReviceEntities == null)
                {
                    dm_Task_ReviceEntities = this.BaseRepository("dm_data").FindList<dm_task_reviceEntity>(t => t.task_id == task_id);
                    if (dm_Task_ReviceEntities.Count() > 0)
                        redisCache.Write<IEnumerable<dm_task_reviceEntity>>(cacheKey, dm_Task_ReviceEntities, DateTime.Now.AddMinutes(5), 7);
                }
                return dm_Task_ReviceEntities;
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

        #region 接受任务
        public dm_task_reviceEntity ReviceTask(dm_task_reviceEntity dm_Task_ReviceEntity, string appid)
        {
            IRepository db = null;
            try
            {
                dm_basesettingEntity dm_BasesettingEntity = dM_BaseSettingService.GetEntityByCache(appid);
                if (dm_BasesettingEntity.IsEmpty())
                    throw new Exception("配置信息获取错误!");

                dm_taskEntity dm_TaskEntity = dm_TaskService.GetEntity(dm_Task_ReviceEntity.task_id);
                if (dm_TaskEntity.IsEmpty())
                    throw new Exception("任务id错误!");
                if (dm_TaskEntity.task_status == -2)
                    throw new Exception("任务正在审核中，暂时无法接受!");
                if (dm_TaskEntity.task_status == 1)
                    throw new Exception("您来晚了一步,当前任务已抢光!");
                if (dm_TaskEntity.task_status == 2)
                    throw new Exception("当前任务已取消,您可接受其他任务!");
                if (dm_TaskEntity.task_status == 3)
                    throw new Exception("当前任务已下架,您可接受其他任务!");
                /*一个任务可重复接受  2020-07-27*/
                /*dm_task_reviceEntity reviceEntity = GetReviceEntity(dm_Task_ReviceEntity.user_id, dm_Task_ReviceEntity.task_id);
                if (!reviceEntity.IsEmpty())
                    throw new Exception("请勿重复接受该任务!");*/

                dm_userEntity dm_UserEntity = dM_UserService.GetEntity(dm_Task_ReviceEntity.user_id);
                if (dm_UserEntity.userlevel != 1 && dm_UserEntity.userlevel != 2)
                    throw new Exception("当前等级无法接受任务,请升级后重试!");

                //判断当前未在审核状态的任务数量
                int noFinishCount = BaseRepository("dm_data").IQueryable<dm_task_reviceEntity>(t => (t.status == 1 || t.status == 2) && t.user_id == dm_Task_ReviceEntity.user_id).Count();
                if (noFinishCount >= dm_BasesettingEntity.revicetaskcountlimit)
                    throw new Exception("同时接单任务数已达到上限,请先完成其他任务后再来接单!");


                dm_Task_ReviceEntity.revice_time = DateTime.Now;
                dm_Task_ReviceEntity.status = 1;
                dm_Task_ReviceEntity.Create();

                dm_TaskEntity.revicecount += 1;
                dm_TaskEntity.Modify(dm_TaskEntity.id);

                db = BaseRepository("dm_data").BeginTrans();
                db.Update(dm_TaskEntity);
                db.Insert(dm_Task_ReviceEntity);
                db.Commit();

                return dm_Task_ReviceEntity;
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

        #region 提交资料
        public dm_task_reviceEntity SubmitMeans(dm_task_reviceEntity dm_Task_ReviceEntity)
        {
            try
            {
                dm_task_reviceEntity dm_Task_ReviceEntity_New = GetEntity(dm_Task_ReviceEntity.id);
                if (dm_Task_ReviceEntity_New.status == 2)
                    throw new Exception("资料已提交,请勿重复提交,耐心等待审核!");
                if (dm_Task_ReviceEntity_New.status == 4)
                    throw new Exception("您已经取消该任务,无需再提交资料!");
                dm_taskEntity dm_TaskEntity = dm_TaskService.GetEntity(dm_Task_ReviceEntity_New.task_id);
                if (dm_TaskEntity.task_status == 2)
                    throw new Exception("该任务已取消,资料提交失败!");
                dm_Task_ReviceEntity_New.status = 2;
                dm_Task_ReviceEntity_New.submit_time = DateTime.Now;
                dm_Task_ReviceEntity.Modify(dm_Task_ReviceEntity_New.id);
                dm_Task_ReviceEntity_New.submit_data = dm_Task_ReviceEntity.submit_data;
                BaseRepository("dm_data").Update(dm_Task_ReviceEntity_New);

                return dm_Task_ReviceEntity_New;
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

        #region 取消任务
        public dm_task_reviceEntity CancelByRevicePerson(int revice_id, int IsPubCancel = 0)
        {
            IRepository db = null;
            try
            {
                /* 取消任务
                 * 1、更改接受任务表的任务状态
                 * 2、修改任务主表的接单人数
                 * 3、向任务发布人增加取消任务通知
                 */
                dm_task_reviceEntity dm_Task_ReviceEntity = GetEntity(revice_id);
                #region 更改接受任务表状态
                if (dm_Task_ReviceEntity.IsEmpty())
                    throw new Exception("接受任务记录异常!");
                if (dm_Task_ReviceEntity.status == 2)
                    throw new Exception("任务正在审核中，取消任务失败!");
                if (dm_Task_ReviceEntity.status == 3)
                    throw new Exception("任务已完成，取消任务失败!");
                if (dm_Task_ReviceEntity.status == 4)
                    throw new Exception("任务已取消，请勿重复提交!");

                dm_taskEntity dm_TaskEntity = dm_TaskService.GetEntity(dm_Task_ReviceEntity.task_id);


                if (IsPubCancel == 1)
                {
                    DateTime diffTime = ((DateTime)dm_Task_ReviceEntity.revice_time).AddHours(dm_TaskEntity.task_time_limit);
                    if (DateTime.Now < diffTime)
                        throw new Exception("该用户提交资料未超过设定时间,无法取消任务,您可在" + diffTime.ToString("yyyy-MM-dd HH:mm:ss") + "重试操作!");
                }


                dm_Task_ReviceEntity.status = 4;
                dm_Task_ReviceEntity.canceltime = DateTime.Now;
                dm_Task_ReviceEntity.Modify(dm_Task_ReviceEntity.id);
                #endregion

                #region 修改任务主表数据
                dm_TaskEntity.revicecount -= 1;
                dm_TaskEntity.Modify(dm_TaskEntity.id);
                #endregion

                #region 增加消息记录
                dm_messagerecordEntity dm_MessagerecordEntity = new dm_messagerecordEntity
                {
                    isread = 0,
                    user_id = dm_TaskEntity.user_id,
                    messagetitle = "接单人取消",
                    messagecontent = "您发布的任务编号:" + dm_TaskEntity.task_no + "接单人已取消,请及时查看!",
                    messagetype = 2,
                    createtime = DateTime.Now
                };
                #endregion

                db = BaseRepository("dm_data").BeginTrans();
                db.Update(dm_Task_ReviceEntity);
                db.Update(dm_TaskEntity);
                db.Insert(dm_MessagerecordEntity);

                db.Commit();

                return dm_Task_ReviceEntity;
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

        #region 审核任务
        private List<dm_accountdetailEntity> dm_AccountdetailEntities = new List<dm_accountdetailEntity>();
        private List<dm_userEntity> calculateComissionEntities = new List<dm_userEntity>();
        public dm_task_reviceEntity AuditTask(int revice_id)
        {
            IRepository db = null;
            try
            {
                /*
                 * 任务审核(任务审核成功)
                 * 1、更改任务接受表的状态
                 * 2、更改任务主表的完成数量
                 * 3、向任务接受人发送消息记录
                 * 4、执行返佣
                 */

                decimal do_task_commission = 0, one_agent_commission = 0, two_agent_commission = 0, one_partners_commission = 0, two_partners_commission = 0;

                dm_userEntity currentUser = null, one_User = null, two_User = null, one_partners = null, two_partners = null;
                string currentNickName = "";

                dm_task_reviceEntity dm_Task_ReviceEntity = GetEntity(revice_id);
                if (dm_Task_ReviceEntity.status == 4)
                    throw new Exception("该任务已取消,审核失败!");
                if (dm_Task_ReviceEntity.status == 1)
                    throw new Exception("该任务未提交资料,审核失败!");
                if (dm_Task_ReviceEntity.status == 3)
                    throw new Exception("该任务已审核通过,请勿重复审核!");
                dm_Task_ReviceEntity.status = 3;
                dm_Task_ReviceEntity.check_time = DateTime.Now;

                #region 修改任务主表数据
                dm_taskEntity dm_TaskEntity = dm_TaskService.GetEntity(dm_Task_ReviceEntity.task_id);
                if (dm_TaskEntity.task_status == 2)
                    throw new Exception("该任务已取消,审核失败!");
                dm_TaskEntity.finishcount += 1;
                if (dm_TaskEntity.finishcount >= dm_TaskEntity.needcount)
                    dm_TaskEntity.task_status = 1;
                dm_TaskEntity.Modify(dm_TaskEntity.id);
                #endregion

                #region 任务审核消息
                dm_messagerecordEntity dm_MessagerecordEntity = new dm_messagerecordEntity
                {
                    isread = 0,
                    user_id = dm_Task_ReviceEntity.user_id,
                    messagetitle = "任务审核通过",
                    messagecontent = "您接受的任务编号:" + dm_TaskEntity.task_no + "已审核通过,请及时查看!",
                    messagetype = 3,
                    createtime = DateTime.Now
                };
                #endregion

                #region 执行返佣
                /*做任务人根据等级返佣金*/
                /*上级从服务费中计算佣金*/
                //获取上下级关系
                IEnumerable<dm_user_relationEntity> userRelationList = dM_UserRelationService.GetParentRelation(dm_Task_ReviceEntity.user_id);

                ///获取上下级关系的用户信息
                IEnumerable<dm_userEntity> userList = dM_UserService.GetParentUser(dm_Task_ReviceEntity.user_id);

                #region 做任务人返佣
                currentUser = userList.Where(t => t.id == dm_Task_ReviceEntity.user_id).FirstOrDefault();
                currentNickName = currentUser.nickname;//记录下接单人的昵称  防止丢失
                if (currentUser.IsEmpty())
                    throw new Exception("用户信息异常!");
                if (currentUser.userlevel == 0)
                    throw new Exception("用户等级异常,返佣失败!");
                else if (currentUser.userlevel == 1)
                    do_task_commission = dm_TaskEntity.juniorcommission;
                else if (currentUser.userlevel == 2)
                    do_task_commission = dm_TaskEntity.seniorcommission;
                else
                    throw new Exception("用户无等级,返佣失败!");
                if (do_task_commission > 0)
                {
                    dm_Task_ReviceEntity.comissionamount = do_task_commission;
                    currentUser = CalculateComission(currentUser.id, do_task_commission, currentUser.accountprice);
                    dm_AccountdetailEntities.Add(GeneralAccountDetail(currentUser.id, 14, "做任务返佣", "您接受的任务编号" + dm_TaskEntity.task_no + "已返佣到账,请及时查收!", do_task_commission, currentUser.accountprice));
                }
                #endregion
                dm_Task_ReviceEntity.Modify(dm_Task_ReviceEntity.id);//由于需要修改任务所得佣金  故修改放到此处

                dm_basesettingEntity dm_BasesettingEntity = dM_BaseSettingService.GetEntityByCache(dm_TaskEntity.appid);

                #region 更改一级账户余额及明细
                dm_user_relationEntity dm_User_RelationEntity_one = userRelationList.Where(t => t.user_id == dm_Task_ReviceEntity.user_id).FirstOrDefault();
                one_User = userList.Where(t => t.id == dm_User_RelationEntity_one.parent_id).FirstOrDefault();
                if (!one_User.IsEmpty())
                {
                    if (one_User.userlevel > 0)
                    {//做任务人为代理身份才会返佣
                        one_agent_commission = ConvertComission(dm_BasesettingEntity.task_one * dm_TaskEntity.servicefee);
                        if (one_agent_commission > 0)
                        {
                            one_User = CalculateComission(one_User.id, one_agent_commission, one_User.accountprice);
                            dm_AccountdetailEntities.Add(GeneralAccountDetail(one_User.id, 15, "下级做任务", "您的下级《" + currentNickName + "》任务已完成,奖励已发放到您的账户,继续努力哟!", one_agent_commission, one_User.accountprice));
                        }
                    }


                    #region 更改二级账户余额及明细
                    dm_user_relationEntity dm_User_RelationEntity_two = userRelationList.Where(t => t.user_id == one_User.id).FirstOrDefault();
                    two_User = userList.Where(t => t.id == dm_User_RelationEntity_two.parent_id).FirstOrDefault();
                    if (!two_User.IsEmpty())
                    {
                        if (two_User.userlevel > 0)
                        {
                            two_agent_commission = ConvertComission(dm_BasesettingEntity.task_two * dm_TaskEntity.servicefee);
                            if (two_agent_commission > 0)
                            {
                                two_User = CalculateComission(two_User.id, two_agent_commission, two_User.accountprice);
                                dm_AccountdetailEntities.Add(GeneralAccountDetail(two_User.id, 16, "下下级做任务", "您的二级《" + currentNickName + "》任务已完成,奖励已发放到您的账户,继续努力哟!", two_agent_commission, two_User.accountprice));
                            }
                        }
                    }
                    #endregion
                }
                #endregion

                #region 获取当前用户所属合伙人(一级合伙人)
                one_partners = dM_UserService.GetUserByPartnersID(dm_User_RelationEntity_one.partners_id);
                if (!one_partners.IsEmpty())
                {
                    one_partners_commission = ConvertComission(dm_BasesettingEntity.task_one_partners * dm_TaskEntity.servicefee);
                    if (one_partners_commission > 0)
                    {
                        one_partners = CalculateComission(one_partners.id, one_partners_commission, one_partners.accountprice);
                        dm_AccountdetailEntities.Add(GeneralAccountDetail(one_partners.id, 17, "团队成员做任务", "您的团队成员《" + currentNickName + "》任务已完成,奖励已发放到您的账户,继续努力哟!", one_partners_commission, one_partners.accountprice));
                    }

                    #region 二级合伙人
                    dm_user_relationEntity dm_User_RelationEntity_one_partners = dM_UserRelationService.GetEntityByUserID(one_partners.id);
                    two_partners = dM_UserService.GetEntityByCache(dm_User_RelationEntity_one_partners.parent_id);
                    //two_partners = dM_UserService.GetUserByPartnersID(dm_User_RelationEntity_one_partners.partners_id);
                    if (!two_partners.IsEmpty())
                    {
                        if (two_partners.partnersstatus == 1)
                        {//二级用户为合伙人时才进行返利
                            two_partners_commission = ConvertComission(dm_BasesettingEntity.task_two_partners * dm_TaskEntity.servicefee);
                            if (two_partners_commission > 0)
                            {
                                two_partners = CalculateComission(two_partners.id, two_partners_commission, two_partners.accountprice);
                                dm_AccountdetailEntities.Add(GeneralAccountDetail(two_partners.id, 18, "下级团队成员做任务", "您的下级团队成员《" + currentNickName + "》任务已完成,奖励已发放到您的账户,继续努力哟!", two_partners_commission, two_partners.accountprice));
                            }
                        }
                    }
                    #endregion
                }
                #endregion
                #endregion

                if (calculateComissionEntities.Count > 0)
                {
                    //必须加上这个变量,用于清除当前返利账户的余额
                    foreach (var item in calculateComissionEntities)
                    {
                        item.Modify(item.id);
                    }
                    db = BaseRepository("dm_data").BeginTrans();
                    db.Update(dm_Task_ReviceEntity);//更改接单表信息
                    db.Update(dm_TaskEntity);//更改任务主表信息
                    db.Insert(dm_MessagerecordEntity);//增加任务审核消息(发给接受任务的人)
                    db.Insert(dm_MessagerecordEntity);//增加消息记录
                    db.Insert(dm_AccountdetailEntities);//增加账户余额明细
                    db.Update(calculateComissionEntities);//批量修改用户信息
                    db.Commit();
                }

                return dm_Task_ReviceEntity;
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

        private decimal ConvertComission(decimal? comissionamount)
        {
            return Math.Round(Convert.ToDecimal(comissionamount / 100m), 2);
        }

        /// <summary>
        /// 生成账户明细
        /// </summary>
        /// <param name="user_id"></param>
        /// <param name="type">账户余额变更来源 类型 1订单佣金  2一级粉丝订单  3二级粉丝订单  4团队订单  5下级团队订单  6下级开通代理  7下下级开通代理 8团队成员  9下级团队成员 10进度任务增加 11申请提现 12发布任务 13取消发布任务 14自己做任务  15下级做任务  16下下级做任务 17团队成员做任务  18下级团队成员做任务</param>
        /// <param name="title"></param>
        /// <param name="remark"></param>
        /// <param name="billdetailCommission"></param>
        /// <param name="currentaccountprice"></param>
        /// <returns></returns>
        private dm_accountdetailEntity GeneralAccountDetail(int? user_id, int type, string title, string remark, decimal billdetailCommission, decimal? currentaccountprice)
        {
            return new dm_accountdetailEntity
            {
                createtime = DateTime.Now,
                remark = remark,
                stepvalue = billdetailCommission,
                currentvalue = currentaccountprice,
                title = title,
                type = type,
                user_id = user_id,
                profitLoss=CommonHelper.GetProfitLoss(type)
            };
        }

        private dm_userEntity CalculateComission(int? user_id, decimal? commission, decimal? currentaccount)
        {
            dm_userEntity calculateComissionEntity = calculateComissionEntities.Where((dm_userEntity t) => t.id == user_id).FirstOrDefault();
            if (calculateComissionEntity.IsEmpty())
            {
                calculateComissionEntity = new dm_userEntity
                {
                    id = user_id,
                    accountprice = currentaccount + commission
                };
                calculateComissionEntities.Add(calculateComissionEntity);
            }
            else
            {
                dm_userEntity dm_userEntity = calculateComissionEntity;
                dm_userEntity.accountprice += commission;
            }
            return calculateComissionEntity;
        }
        #endregion
    }
}
